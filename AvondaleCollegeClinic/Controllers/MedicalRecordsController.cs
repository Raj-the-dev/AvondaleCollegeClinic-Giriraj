using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;                     
using Microsoft.AspNetCore.Mvc.Rendering;            
using Microsoft.EntityFrameworkCore;                
using AvondaleCollegeClinic.Areas.Identity.Data;     
using AvondaleCollegeClinic.Models;                   
using AvondaleCollegeClinic.Helpers;                  
using Microsoft.AspNetCore.Authorization;            
using Microsoft.AspNetCore.Identity;                

namespace AvondaleCollegeClinic.Controllers
{
    // Only these roles are allowed to access Medical Records pages
    [Authorize(Roles = "Doctor,Student,Caregiver,Admin")]
    public class MedicalRecordsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;                 // our database
        private readonly UserManager<AvondaleCollegeClinicUser> _userManager;   // identity helper

        public MedicalRecordsController(AvondaleCollegeClinicContext context, UserManager<AvondaleCollegeClinicUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // Index: list + ownership filter + search + sort + pagination

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            // These are used by the view to remember your sort choice and search text
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            // Figure out who is logged in (Identity user)
            var me = await _userManager.GetUserAsync(User);
            if (me == null) return Challenge(); // not signed in → go to login

            // Sometimes users are mapped to domain rows by different keys (ID, username, email)
            var userId = me.Id;
            var uname = me.UserName ?? string.Empty;
            var email = me.Email ?? string.Empty;

            // Try to find the StudentID connected to this user (if they are a student)
            var studentId = await _context.Students
                .Where(s => s.IdentityUserId == userId || s.StudentID == uname || s.Email == email)
                .Select(s => s.StudentID)
                .FirstOrDefaultAsync();

            // Try to find the CaregiverID connected to this user (if they are a caregiver)
            var caregiverId = await _context.Caregivers
                .Where(c => c.IdentityUserId == userId || c.CaregiverID == uname || c.Email == email)
                .Select(c => c.CaregiverID)
                .FirstOrDefaultAsync();

            // Start building the query and include Student and Doctor so we can show names
            var query = _context.MedicalRecords
                .Include(m => m.Student)
                .Include(m => m.Doctor)
                .AsQueryable();

            //  Ownership filter (who gets to see what) 
            if (User.IsInRole("Student") && !string.IsNullOrEmpty(studentId))
            {
                // Students can only see their own records
                query = query.Where(m => m.StudentID == studentId);
            }
            else if (User.IsInRole("Caregiver") && !string.IsNullOrEmpty(caregiverId))
            {
                // Caregivers can see records of students they are linked to
                query = query.Where(m => m.Student.Caregivers
                    .Any(c => c.CaregiverID == caregiverId));
            }
            else if (User.IsInRole("Doctor"))
            {
                // Doctors currently see all records

            }
            else if (User.IsInRole("Admin"))
            {
                // Admin sees everything
            }
            else
            {
                // Any other role is not allowed
                return Forbid();
            }

            //  Search by name(student or doctor
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = $"%{searchString.Trim()}%";
                query = query.Where(m =>
                    EF.Functions.Like(m.Student.FirstName, term) ||
                    EF.Functions.Like(m.Student.LastName, term) ||
                    EF.Functions.Like(m.Doctor.FirstName, term) ||
                    EF.Functions.Like(m.Doctor.LastName, term));
            }

            // ---- Sort by date (ascending by default, descending if "date_desc") ----
            query = sortOrder switch
            {
                "date_desc" => query.OrderByDescending(m => m.Date),
                _ => query.OrderBy(m => m.Date)
            };

            // ---- Pagination: small pages for snappy UI ----
            const int pageSize = 5;
            int page = pageNumber ?? 1;

            var total = await query.CountAsync();           // total rows after filters
            var rows = await query.AsNoTracking()           // read-only for speed
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            // Wrap in PaginatedList so the view knows page count and next/prev
            return View(new PaginatedList<MedicalRecord>(rows, total, page, pageSize));
        }


        // Details

        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // missing id in URL
            }

            // Load the record plus its Student and Doctor
            var medicalRecord = await _context.MedicalRecords
                .Include(m => m.Doctor)
                .Include(m => m.Student)
                .FirstOrDefaultAsync(m => m.MedicalRecordID == id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }


        // Create (GET)

        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult Create()
        {
            // Student dropdown shows the student's full name but posts StudentID
            ViewBag.StudentID = new SelectList(_context.Students.Select(s => new {
                s.StudentID,
                FullName = s.FirstName + " " + s.LastName
            }), "StudentID", "FullName");

            // Doctor dropdown shows full name but posts DoctorID
            ViewBag.DoctorID = new SelectList(_context.Doctors.Select(d => new {
                d.DoctorID,
                FullName = d.FirstName + " " + d.LastName
            }), "DoctorID", "FullName");
            return View();
        }

        // Create (POST)

        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicalRecordID,StudentID,DoctorID,Notes,Date")] MedicalRecord medicalRecord)
        {
            // Your project saves when ModelState is NOT valid, so we keep that behavior.
            if (!ModelState.IsValid)
            {
                _context.Add(medicalRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got here, rebuild dropdowns and show the form again
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", medicalRecord.DoctorID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", medicalRecord.StudentID);
            return View(medicalRecord);
        }


        // Edit (GET)

        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            // Build friendly dropdowns again for editing
            ViewBag.StudentID = new SelectList(_context.Students.Select(s => new {
                s.StudentID,
                FullName = s.FirstName + " " + s.LastName
            }), "StudentID", "FullName");

            ViewBag.DoctorID = new SelectList(_context.Doctors.Select(d => new {
                d.DoctorID,
                FullName = d.FirstName + " " + d.LastName
            }), "DoctorID", "FullName");
            return View(medicalRecord);
        }


        // Edit (POST)

        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicalRecordID,StudentID,DoctorID,Notes,Date")] MedicalRecord medicalRecord)
        {
            // Safety check: id in URL must match the model key
            if (id != medicalRecord.MedicalRecordID)
            {
                return NotFound();
            }

            // Save when NOT valid (as per project pattern)
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // If someone deleted it while we were editing, show 404
                    if (!MedicalRecordExists(medicalRecord.MedicalRecordID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // some other concurrency issue
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If we land here, rebuild dropdowns and re-show the form
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", medicalRecord.DoctorID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", medicalRecord.StudentID);
            return View(medicalRecord);
        }


        // Delete (GET)

        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include Student and Doctor so the page can display their names
            var medicalRecord = await _context.MedicalRecords
                .Include(m => m.Doctor)
                .Include(m => m.Student)
                .FirstOrDefaultAsync(m => m.MedicalRecordID == id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }


        // Delete (POST) actually remove from database

        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord != null)
            {
                _context.MedicalRecords.Remove(medicalRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Small helper used by the concurrency check above
        private bool MedicalRecordExists(int id)
        {
            return _context.MedicalRecords.Any(e => e.MedicalRecordID == id);
        }
    }
}
