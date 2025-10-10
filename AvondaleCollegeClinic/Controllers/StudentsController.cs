using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;
using Microsoft.Data.SqlClient;
using AvondaleCollegeClinic.Helpers;
using NuGet.DependencyResolver;
using Microsoft.AspNetCore.Authorization;

namespace AvondaleCollegeClinic.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public StudentsController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        // GET: Students

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var students = from s in _context.Students
                               .Include(s => s.Caregivers)                     
                               .Include(s => s.Homeroom).ThenInclude(h => h.Teacher)
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                           || s.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.DOB);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.DOB);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<Student>.CreateAsync(
                students.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Admin,Teacher")]
        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Caregivers)                                    
                .Include(s => s.Homeroom).ThenInclude(h => h.Teacher)
                .FirstOrDefaultAsync(m => m.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create()
        {
            var student = new Student
            {
                StudentID = GenerateStudentID(),
                DOB = DateTime.Today
            };

            var caregivers = await _context.Caregivers
                .Select(c => new { c.CaregiverID, FullName = c.FirstName + " " + c.LastName })
                .OrderBy(c => c.FullName)
                .ToListAsync();

            ViewData["CaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName");
            ViewData["ExtraCaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName"); // optional

            var homerooms = await _context.Homerooms
                .Include(h => h.Teacher)
                .Select(h => new
                {
                    h.HomeroomID,
                    DisplayName = h.Teacher.FirstName + " " + h.Teacher.LastName + " - " + h.Teacher.TeacherCode
                })
                .OrderBy(h => h.DisplayName)
                .ToListAsync();
            ViewData["HomeroomID"] = new SelectList(homerooms, "HomeroomID", "DisplayName");

            return View(student);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create(
            [Bind("StudentID,FirstName,LastName,DOB,Email,HomeroomID,ImageFile,ImagePath")] Student student,
            string caregiverId,              // required
            string? extraCaregiverId)        // optional
        {
            // simple validation: main caregiver required
            if (string.IsNullOrWhiteSpace(caregiverId))
                ModelState.AddModelError("CaregiverID", "Please select a caregiver.");

            // (keep any uniqueness checks you already had)
            if (await _context.Students.AnyAsync(s => s.Email == student.Email))
                ModelState.AddModelError("Email", "This email is already in use by another student.");

            if (ModelState.IsValid)
            {
                // repopulate dropdowns
                var caregivers = await _context.Caregivers
                    .Select(c => new { c.CaregiverID, FullName = c.FirstName + " " + c.LastName })
                    .OrderBy(c => c.FullName).ToListAsync();
                ViewData["CaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName", caregiverId);
                ViewData["ExtraCaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName", extraCaregiverId);

                var homerooms = await _context.Homerooms.Include(h => h.Teacher)
                    .Select(h => new { h.HomeroomID, DisplayName = h.Teacher.FirstName + " " + h.Teacher.LastName + " - " + h.Teacher.TeacherCode })
                    .OrderBy(h => h.DisplayName).ToListAsync();
                ViewData["HomeroomID"] = new SelectList(homerooms, "HomeroomID", "DisplayName", student.HomeroomID);
                return View(student);
            }

            // image upload (unchanged)
            if (student.ImageFile != null && student.ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/students");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + "_" + Path.GetFileName(student.ImageFile.FileName);
                using var fs = new FileStream(Path.Combine(uploads, fileName), FileMode.Create);
                await student.ImageFile.CopyToAsync(fs);
                student.ImagePath = "/images/students/" + fileName;
            }

            // attach up to 2 caregivers
            student.Caregivers = new List<Caregiver>();
            var main = await _context.Caregivers.FindAsync(caregiverId);
            if (main != null) student.Caregivers.Add(main);

            if (!string.IsNullOrWhiteSpace(extraCaregiverId) && extraCaregiverId != caregiverId)
            {
                var extra = await _context.Caregivers.FindAsync(extraCaregiverId);
                if (extra != null) student.Caregivers.Add(extra);
            }

            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var student = await _context.Students
                .Include(s => s.Caregivers)
                .FirstOrDefaultAsync(s => s.StudentID == id);
            if (student == null) return NotFound();

            // preselect up to two caregivers
            var primaryId = student.Caregivers.Select(c => c.CaregiverID).FirstOrDefault();
            var extraId = student.Caregivers.Select(c => c.CaregiverID).Skip(1).FirstOrDefault();

            var caregivers = await _context.Caregivers
                .Select(c => new { c.CaregiverID, FullName = c.FirstName + " " + c.LastName })
                .OrderBy(c => c.FullName).ToListAsync();
            ViewData["CaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName", primaryId);
            ViewData["ExtraCaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName", extraId);

            var homerooms = await _context.Homerooms.Include(h => h.Teacher)
                .Select(h => new { h.HomeroomID, DisplayName = h.Teacher.FirstName + " " + h.Teacher.LastName + " - " + h.Teacher.TeacherCode })
                .OrderBy(h => h.DisplayName).ToListAsync();
            ViewData["HomeroomID"] = new SelectList(homerooms, "HomeroomID", "DisplayName", student.HomeroomID);

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(
            string id,
            [Bind("StudentID,FirstName,LastName,DOB,Email,HomeroomID,ImageFile,ImagePath")] Student form,
            string caregiverId,          // required
            string? extraCaregiverId)    // optional
        {
            if (id != form.StudentID) return NotFound();

            var student = await _context.Students
                .Include(s => s.Caregivers)
                .FirstOrDefaultAsync(s => s.StudentID == id);
            if (student == null) return NotFound();

            if (string.IsNullOrWhiteSpace(caregiverId))
                ModelState.AddModelError("CaregiverID", "Please select a caregiver.");

            if (ModelState.IsValid)
            {
                var caregivers = await _context.Caregivers
                    .Select(c => new { c.CaregiverID, FullName = c.FirstName + " " + c.LastName })
                    .OrderBy(c => c.FullName).ToListAsync();
                ViewData["CaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName", caregiverId);
                ViewData["ExtraCaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName", extraCaregiverId);

                var homerooms = await _context.Homerooms.Include(h => h.Teacher)
                    .Select(h => new { h.HomeroomID, DisplayName = h.Teacher.FirstName + " " + h.Teacher.LastName + " - " + h.Teacher.TeacherCode })
                    .OrderBy(h => h.DisplayName).ToListAsync();
                ViewData["HomeroomID"] = new SelectList(homerooms, "HomeroomID", "DisplayName", form.HomeroomID);

                return View(form);
            }

            // scalar updates
            student.FirstName = form.FirstName;
            student.LastName = form.LastName;
            student.DOB = form.DOB;
            student.Email = form.Email;
            student.HomeroomID = form.HomeroomID;

            // image upload (unchanged)
            if (form.ImageFile != null && form.ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/students");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + "_" + Path.GetFileName(form.ImageFile.FileName);
                using var fs = new FileStream(Path.Combine(uploads, fileName), FileMode.Create);
                await form.ImageFile.CopyToAsync(fs);
                student.ImagePath = "/images/students/" + fileName;
            }

            // replace caregivers with up to two chosen
            student.Caregivers.Clear();

            var main = await _context.Caregivers.FindAsync(caregiverId);
            if (main != null) student.Caregivers.Add(main);

            if (!string.IsNullOrWhiteSpace(extraCaregiverId) && extraCaregiverId != caregiverId)
            {
                var extra = await _context.Caregivers.FindAsync(extraCaregiverId);
                if (extra != null) student.Caregivers.Add(extra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        [Authorize(Roles = "Admin,Teacher")]
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Caregivers)                                  
                .Include(s => s.Homeroom)
                .FirstOrDefaultAsync(m => m.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }

        private string GenerateStudentID()
        {
            // Start the ID with "ac" + current year's last two digits (e.g., "ac25" for 2025)
            string yearPrefix = "ac" + DateTime.Now.ToString("yy");

            // Find the most recent student ID that starts with that prefix
            var latestId = _context.Students
                .Where(s => s.StudentID.StartsWith(yearPrefix))
                .OrderByDescending(s => s.StudentID)
                .Select(s => s.StudentID)
                .FirstOrDefault();

            // Figure out what the next number should be
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(latestId) && latestId.Length == 8)
            {
                int.TryParse(latestId.Substring(4), out nextNumber);
                nextNumber++; // Increment for the new ID
            }

            // Return something like "ac250001", "ac250002", etc.
            return $"{yearPrefix}{nextNumber:D4}";
        }

    }
}
