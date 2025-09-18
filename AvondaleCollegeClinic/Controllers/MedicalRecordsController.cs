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
    [Authorize(Roles = "Admin, Student, Caregiver, Doctor")]

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
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            // 1) Who is logged in?
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var userId = user.Id;
            var uname = user.UserName ?? "";
            var email = user.Email ?? "";

            // 2) Resolve domain IDs
            var studentId = await _context.Students
                .Where(s => s.IdentityUserId == userId || s.StudentID == uname || s.Email == email)
                .Select(s => s.StudentID)
                .FirstOrDefaultAsync();

            var caregiverId = await _context.Caregivers
                .Where(c => c.IdentityUserId == userId || c.CaregiverID == uname || c.Email == email)
                .Select(c => c.CaregiverID)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(studentId) && string.IsNullOrEmpty(caregiverId))
                return Forbid();

            // 3) Query
            var query = _context.MedicalRecords
                .Include(m => m.Student)
                .Include(m => m.Doctor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(studentId))
                query = query.Where(m => m.StudentID == studentId);
            else if (!string.IsNullOrEmpty(caregiverId))
                query = query.Where(m => m.Student.CaregiverID == caregiverId);

            // 4) Search
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = $"%{searchString.Trim()}%";
                query = query.Where(m =>
                    EF.Functions.Like(m.Student.FirstName, term) ||
                    EF.Functions.Like(m.Student.LastName, term) ||
                    EF.Functions.Like(m.Doctor.FirstName, term) ||
                    EF.Functions.Like(m.Doctor.LastName, term)
                );
            }

            // 5) Sort
            query = sortOrder switch
            {
                "date_desc" => query.OrderByDescending(m => m.Date),
                _ => query.OrderBy(m => m.Date)
            };

            var records = await query.AsNoTracking().ToListAsync();


            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                records = records.Where(m =>
                    m.Student.FirstName.ToLower().Contains(searchString) ||
                    m.Student.LastName.ToLower().Contains(searchString) ||
                    m.Doctor.FirstName.ToLower().Contains(searchString) ||
                    m.Doctor.LastName.ToLower().Contains(searchString)
                ).ToList();
            }

            switch (sortOrder)
            {
                case "date_desc":
                    records = records.OrderByDescending(m => m.Date).ToList();
                    break;
                default:
                    records = records.OrderBy(m => m.Date).ToList();
                    break;
            }

            int pageSize = 10;
            int page = pageNumber ?? 1;
            return View(new PaginatedList<MedicalRecord>(
                records.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                records.Count, page, pageSize
            ));
        }


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
