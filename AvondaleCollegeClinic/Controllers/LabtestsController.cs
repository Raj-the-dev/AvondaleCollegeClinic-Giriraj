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
    [Authorize(Roles = "Doctor,Student,Caregiver,Admin")]
    public class LabtestsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;
        private readonly UserManager<AvondaleCollegeClinicUser> _userManager;

        public LabtestsController(AvondaleCollegeClinicContext context, UserManager<AvondaleCollegeClinicUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Labtests
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TestSortParm"] = String.IsNullOrEmpty(sortOrder) ? "test_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            // Base query: include related entities
            var query = _context.LabTests
                .Include(l => l.MedicalRecord)
                    .ThenInclude(m => m.Student)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = $"%{searchString.Trim()}%";
                query = query.Where(l =>
                    EF.Functions.Like(l.TestType, term) ||
                    EF.Functions.Like(l.MedicalRecord.Student.FirstName, term) ||
                    EF.Functions.Like(l.MedicalRecord.Student.LastName, term)
                );
            }

            // Sort (DB-level)
            query = sortOrder switch
            {
                "test_desc" => query.OrderByDescending(l => l.TestType),
                _ => query.OrderBy(l => l.TestType)
            };

            var labtests = await query.AsNoTracking().ToListAsync();

            // Client-side filtering (extra layer)
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                labtests = labtests.Where(l =>
                    l.TestType.ToLower().Contains(searchString)
                ).ToList();
            }

            // Sort again client-side (if needed)
            switch (sortOrder)
            {
                case "test_desc":
                    labtests = labtests.OrderByDescending(l => l.TestType).ToList();
                    break;
                default:
                    labtests = labtests.OrderBy(l => l.TestType).ToList();
                    break;
            }

            // Pagination
            int pageSize = 10;
            int page = pageNumber ?? 1;
            return View(new PaginatedList<Labtest>(
                labtests.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                labtests.Count, page, pageSize
            ));
        }


        [Authorize(Roles = "Admin,Doctor")]
        // GET: Labtests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        // GET: Labtests/Create
        public IActionResult Create()
        {
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
        [Authorize(Roles = "Admin,Doctor")]
        // POST: Labtests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabtestID,RecordID,TestType,PDFFile,PDFPath,ResultDate")] Labtest labtest)
        {
            // KEEPING YOUR PATTERN: save when !ModelState.IsValid
            if (!ModelState.IsValid)
            {
                // --- PDF SAVE (Create) ---
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

                    // store relative path to serve later
                    labtest.File = "/files/labtests/" + uniqueFileName;
                }
                // --- end PDF SAVE (Create) ---

                _context.Add(labtest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RecordID"] = new SelectList(_context.MedicalRecords, "MedicalRecordID", "MedicalRecordID", labtest.RecordID);
            return View(labtest);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // GET: Labtests/Edit/5
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
            ViewData["RecordID"] = new SelectList(_context.MedicalRecords, "MedicalRecordID", "MedicalRecordID", labtest.RecordID);
            return View(labtest);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // POST: Labtests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LabtestID,RecordID,TestType,File,PDFFile,ResultDate")] Labtest labtest)
        {
            if (id != labtest.LabtestID)
            {
                return NotFound();
            }

            // KEEPING YOUR PATTERN: save when !ModelState.IsValid
            if (!ModelState.IsValid)
            {
                // Load existing to preserve current file path if no new file is uploaded
                var existing = await _context.LabTests.AsNoTracking()
                    .FirstOrDefaultAsync(l => l.LabtestID == id);
                if (existing == null)
                {
                    return NotFound();
                }

                // --- PDF SAVE (Edit) ---
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

                    // Assuming "File" is your path column on Labtest for Edit
                    labtest.File = "/files/labtests/" + uniqueFileName;

                    // (optional) clean up the old file:
                    // if (!string.IsNullOrEmpty(existing.File)) { ... delete existing.File ... }
                }
                else
                {
                    // No new file selected—keep the old path
                    labtest.File = existing.File;
                }
                // --- end PDF SAVE (Edit) ---

                try
                {
                    _context.Update(labtest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabtestExists(labtest.LabtestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["RecordID"] = new SelectList(_context.MedicalRecords, "MedicalRecordID", "MedicalRecordID", labtest.RecordID);
            return View(labtest);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // GET: Labtests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        // POST: Labtests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var labtest = await _context.LabTests.FindAsync(id);
            if (labtest != null)
            {
                _context.LabTests.Remove(labtest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabtestExists(int id)
        {
            return _context.LabTests.Any(e => e.LabtestID == id);
        }

    }
}
