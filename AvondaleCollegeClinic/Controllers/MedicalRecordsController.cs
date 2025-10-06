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

    public class MedicalRecordsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;
        private readonly UserManager<AvondaleCollegeClinicUser> _userManager;

        public MedicalRecordsController(AvondaleCollegeClinicContext context, UserManager<AvondaleCollegeClinicUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MedicalRecords
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            // Who is logged in?
            var me = await _userManager.GetUserAsync(User);
            if (me == null) return Challenge();

            var userId = me.Id;
            var uname = me.UserName ?? string.Empty;
            var email = me.Email ?? string.Empty;

            // Resolve domain IDs
            var studentId = await _context.Students
                .Where(s => s.IdentityUserId == userId || s.StudentID == uname || s.Email == email)
                .Select(s => s.StudentID)
                .FirstOrDefaultAsync();

            var caregiverId = await _context.Caregivers
                .Where(c => c.IdentityUserId == userId || c.CaregiverID == uname || c.Email == email)
                .Select(c => c.CaregiverID)
                .FirstOrDefaultAsync();

            // Base query
            var query = _context.MedicalRecords
                .Include(m => m.Student)
                .Include(m => m.Doctor)
                .AsQueryable();

            // Ownership filter
            if (User.IsInRole("Student") && !string.IsNullOrEmpty(studentId))
            {
                query = query.Where(m => m.StudentID == studentId);
            }
            else if (User.IsInRole("Caregiver") && !string.IsNullOrEmpty(caregiverId))
            {
                query = query.Where(m => m.Student.CaregiverID == caregiverId);
            }
            else if (User.IsInRole("Doctor"))
            {
                // leave unfiltered, or add: query = query.Where(m => m.DoctorID == doctorId);
            }
            else if (User.IsInRole("Admin"))
            {
                // see all
            }
            else
            {
                return Forbid();
            }

            // Search
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = $"%{searchString.Trim()}%";
                query = query.Where(m =>
                    EF.Functions.Like(m.Student.FirstName, term) ||
                    EF.Functions.Like(m.Student.LastName, term) ||
                    EF.Functions.Like(m.Doctor.FirstName, term) ||
                    EF.Functions.Like(m.Doctor.LastName, term));
            }

            // Sort
            query = sortOrder switch
            {
                "date_desc" => query.OrderByDescending(m => m.Date),
                _ => query.OrderBy(m => m.Date)
            };

            // Paging
            const int pageSize = 10;
            int page = pageNumber ?? 1;

            var total = await query.CountAsync();
            var rows = await query.AsNoTracking()
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return View(new PaginatedList<MedicalRecord>(rows, total, page, pageSize));
        }



        [Authorize(Roles = "Admin,Doctor")]
        // GET: MedicalRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        [Authorize(Roles = "Admin,Doctor")]
        // GET: MedicalRecords/Create
        public IActionResult Create()
        {
            ViewBag.StudentID = new SelectList(_context.Students.Select(s => new {
                s.StudentID,
                FullName = s.FirstName + " " + s.LastName
            }), "StudentID", "FullName");

            ViewBag.DoctorID = new SelectList(_context.Doctors.Select(d => new {
                d.DoctorID,
                FullName = d.FirstName + " " + d.LastName
            }), "DoctorID", "FullName");
            return View();
        }
        [Authorize(Roles = "Admin,Doctor")]
        // POST: MedicalRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicalRecordID,StudentID,DoctorID,Notes,Date")] MedicalRecord medicalRecord)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(medicalRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", medicalRecord.DoctorID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", medicalRecord.StudentID);
            return View(medicalRecord);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // GET: MedicalRecords/Edit/5
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
        [Authorize(Roles = "Admin,Doctor")]
        // POST: MedicalRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicalRecordID,StudentID,DoctorID,Notes,Date")] MedicalRecord medicalRecord)
        {
            if (id != medicalRecord.MedicalRecordID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalRecordExists(medicalRecord.MedicalRecordID))
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
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", medicalRecord.DoctorID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", medicalRecord.StudentID);
            return View(medicalRecord);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // GET: MedicalRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        [Authorize(Roles = "Admin,Doctor")]
        // POST: MedicalRecords/Delete/5
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

        private bool MedicalRecordExists(int id)
        {
            return _context.MedicalRecords.Any(e => e.MedicalRecordID == id);
        }
    }
}
