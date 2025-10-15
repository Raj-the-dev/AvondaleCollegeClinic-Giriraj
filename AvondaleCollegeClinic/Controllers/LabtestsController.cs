using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;                       // MVC base types like Controller and IActionResult
using Microsoft.AspNetCore.Mvc.Rendering;             // For SelectList dropdowns
using Microsoft.EntityFrameworkCore;                  // EF Core features (Include, ThenInclude, async, etc.)
using AvondaleCollegeClinic.Areas.Identity.Data;      // Identity user type
using AvondaleCollegeClinic.Models;                   // Labtest and related models
using AvondaleCollegeClinic.Helpers;                  // PaginatedList<T> helper
using Microsoft.AspNetCore.Authorization;             // [Authorize] attributes
using Microsoft.AspNetCore.Identity;                  // UserManager to get current user

namespace AvondaleCollegeClinic.Controllers
{
    // Only these roles can use this controller
    [Authorize(Roles = "Doctor,Student,Caregiver,Admin")]
    public class LabtestsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;                        // our database context
        private readonly UserManager<AvondaleCollegeClinicUser> _userManager;          // helps find the signed-in user

        public LabtestsController(AvondaleCollegeClinicContext context, UserManager<AvondaleCollegeClinicUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // Index = list + ownership + search + sort + pagination

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            // Keep UI state for headers and search box
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TestSortParm"] = string.IsNullOrEmpty(sortOrder) ? "test_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            // Who is signed in right now
            var me = await _userManager.GetUserAsync(User);
            if (me == null) return Challenge(); // not logged in → send to login

            // We can match a domain row by IdentityUserId OR by UserName (ID used at first-time) OR Email
            var userId = me.Id;
            var uname = me.UserName ?? string.Empty;
            var email = me.Email ?? string.Empty;

            // Try to get StudentID linked to this Identity user
            var studentId = await _context.Students
                .Where(s => s.IdentityUserId == userId || s.StudentID == uname || s.Email == email)
                .Select(s => s.StudentID)
                .FirstOrDefaultAsync();

            // Try to get CaregiverID linked to this Identity user
            var caregiverId = await _context.Caregivers
                .Where(c => c.IdentityUserId == userId || c.CaregiverID == uname || c.Email == email)
                .Select(c => c.CaregiverID)
                .FirstOrDefaultAsync();

            // Build base query and include who ordered it and for which student
            // We go through MedicalRecord to reach Student and Doctor
            var query = _context.LabTests
                .Include(l => l.MedicalRecord)
                    .ThenInclude(m => m.Student)
                .Include(l => l.MedicalRecord)
                    .ThenInclude(m => m.Doctor)
                .AsQueryable();

            // ---- Ownership filter (who can see which rows) ----
            if (User.IsInRole("Student") && !string.IsNullOrEmpty(studentId))
            {
                // Students see their own lab tests only
                query = query.Where(l => l.MedicalRecord.StudentID == studentId);
            }
            else if (User.IsInRole("Caregiver") && !string.IsNullOrEmpty(caregiverId))
            {
                // Caregivers see lab tests for the students they are linked to
                query = query.Where(l => l.MedicalRecord.Student.Caregivers
                    .Any(c => c.CaregiverID == caregiverId));
            }
            // Doctor and Admin currently see all. You could add a doctor filter if needed.

            // ---- Search box ----
            // We allow searching by test type, student name, doctor name
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = $"%{searchString.Trim()}%";
                query = query.Where(l =>
                    EF.Functions.Like(l.TestType, term) ||
                    EF.Functions.Like(l.MedicalRecord.Student.FirstName, term) ||
                    EF.Functions.Like(l.MedicalRecord.Student.LastName, term) ||
                    EF.Functions.Like(l.MedicalRecord.Doctor.FirstName, term) ||
                    EF.Functions.Like(l.MedicalRecord.Doctor.LastName, term));
            }

            // ---- Sort by test type asc/desc ----
            query = sortOrder switch
            {
                "test_desc" => query.OrderByDescending(l => l.TestType),
                _ => query.OrderBy(l => l.TestType)
            };

            // ---- Pagination (small pages for faster UI) ----
            const int pageSize = 5;
            int page = pageNumber ?? 1;
            var total = await query.CountAsync();
            var rows = await query.AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Wrap in PaginatedList so the view can show next/prev buttons
            return View(new PaginatedList<Labtest>(rows, total, page, pageSize));
        }


        // Details

        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // bad URL
            }

            // Load the lab test and its medical record so we can show context
            var labtest = await _context.LabTests
                .Include(l => l.MedicalRecord)
                .FirstOrDefaultAsync(m => m.LabtestID == id);
            if (labtest == null)
            {
                return NotFound();
            }

            return View(labtest);
        }


        // Create (GET)

        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult Create()
        {
            // Build a dropdown showing "Student Full Name" for each MedicalRecord
            ViewBag.RecordID = new SelectList(_context.MedicalRecords
                .Include(m => m.Student)
                .Select(m => new
                {
                    m.MedicalRecordID,
                    FullName = m.Student.FirstName + " " + m.Student.LastName
                }),
                "MedicalRecordID", "FullName");

            return View();
        }


        // Create (POST)  — file upload + save

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Create([Bind("LabtestID,RecordID,TestType,PDFFile,PDFPath,ResultDate")] Labtest labtest)
        {
            if (!ModelState.IsValid)
            {
                // --- Save uploaded PDF (Create) ---
                if (labtest.PDFFile != null && labtest.PDFFile.Length > 0)
                {
                    // Files go to wwwroot/files/labtests
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files/labtests");
                    Directory.CreateDirectory(uploadsFolder); // ensure folder exists

                    // Make a unique file name so we never overwrite
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(labtest.PDFFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Copy the uploaded file stream to disk
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await labtest.PDFFile.CopyToAsync(fileStream);
                    }

                    // Store the WEB path in the database so the app can link to it
                    labtest.File = "/files/labtests/" + uniqueFileName;
                }
                // --- end PDF SAVE (Create) ---

                // Save row to database
                _context.Add(labtest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we reach here, rebuild dropdown and show form again
            ViewData["RecordID"] = new SelectList(_context.MedicalRecords, "MedicalRecordID", "MedicalRecordID", labtest.RecordID);
            return View(labtest);
        }


        // Edit (GET)

        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labtest = await _context.LabTests.FindAsync(id);
            if (labtest == null)
            {
                return NotFound();
            }
            // Simple dropdown with IDs (enough for internal edit)
            ViewData["RecordID"] = new SelectList(_context.MedicalRecords, "MedicalRecordID", "MedicalRecordID", labtest.RecordID);
            return View(labtest);
        }


        // Edit (POST) — replace or keep existing PDF

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Edit(int id, [Bind("LabtestID,RecordID,TestType,File,PDFFile,ResultDate")] Labtest labtest)
        {
            if (id != labtest.LabtestID)
            {
                return NotFound(); // safety check
            }

            if (!ModelState.IsValid)
            {
                // Load existing row so we can keep its current file path when no new file is uploaded
                var existing = await _context.LabTests.AsNoTracking()
                    .FirstOrDefaultAsync(l => l.LabtestID == id);
                if (existing == null)
                {
                    return NotFound();
                }

                //  Save uploaded PDF (Edit)-
                if (labtest.PDFFile != null && labtest.PDFFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files/labtests");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(labtest.PDFFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await labtest.PDFFile.CopyToAsync(fileStream);
                    }

                    // Update DB path to point to the new file
                    labtest.File = "/files/labtests/" + uniqueFileName;

                }
                else
                {
                    // No new file chosen → keep the previous path
                    labtest.File = existing.File;
                }
                // --- end PDF SAVE (Edit) ---

                try
                {
                    // Update and save
                    _context.Update(labtest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // If the row disappeared while editing, show 404
                    if (!LabtestExists(labtest.LabtestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        // Otherwise bubble up the error
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If validation said "valid" under your pattern, rebuild dropdown and show form again
            ViewData["RecordID"] = new SelectList(_context.MedicalRecords, "MedicalRecordID", "MedicalRecordID", labtest.RecordID);
            return View(labtest);
        }


        // Delete (GET + POST)

        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load the row and show a confirmation page
            var labtest = await _context.LabTests
                .Include(l => l.MedicalRecord)
                .FirstOrDefaultAsync(m => m.LabtestID == id);
            if (labtest == null)
            {
                return NotFound();
            }

            return View(labtest);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Remove the row and save
            var labtest = await _context.LabTests.FindAsync(id);
            if (labtest != null)
            {
                _context.LabTests.Remove(labtest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Small helper used by the concurrency check
        private bool LabtestExists(int id)
        {
            return _context.LabTests.Any(e => e.LabtestID == id);
        }
    }
}
