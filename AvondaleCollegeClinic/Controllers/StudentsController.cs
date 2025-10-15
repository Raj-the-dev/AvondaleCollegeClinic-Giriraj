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
        private readonly AvondaleCollegeClinicContext _context; // our database link for app data

        public StudentsController(AvondaleCollegeClinicContext context)
        {
            _context = context; // keep the context for later use
        }

        // GET: Students
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder; // remember the chosen sort in the view
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : ""; // toggle name sort
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date"; // toggle date sort

            if (searchString != null)
            {
                pageNumber = 1; // reset to first page when search changes
            }
            else
            {
                searchString = currentFilter; // keep old search when paging
            }

            ViewData["CurrentFilter"] = searchString; // keep search text in the box

            var students = from s in _context.Students
                               .Include(s => s.Caregivers)                     // also load caregiver links
                               .Include(s => s.Homeroom).ThenInclude(h => h.Teacher) // also load homeroom and teacher
                           select s; // start the query

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                           || s.FirstName.Contains(searchString)); // filter by first or last name
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName); // sort by last name descending
                    break;
                case "Date":
                    students = students.OrderBy(s => s.DOB); // sort by birth date ascending
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.DOB); // sort by birth date descending
                    break;
                default:
                    students = students.OrderBy(s => s.LastName); // default sort by last name ascending
                    break;
            }

            int pageSize = 6; // show six rows per page
            return View(await PaginatedList<Student>.CreateAsync(
                students.AsNoTracking(), pageNumber ?? 1, pageSize)); // run the query with paging and no tracking for speed
        }

        [Authorize(Roles = "Admin,Teacher")] // only admin and teacher can view details
        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound(); // id missing
            }

            var student = await _context.Students
                .Include(s => s.Caregivers)                                    // include caregivers so we can show them
                .Include(s => s.Homeroom).ThenInclude(h => h.Teacher)          // include homeroom and teacher
                .FirstOrDefaultAsync(m => m.StudentID == id);                  // find the row by id

            if (student == null)
            {
                return NotFound(); // no match found
            }

            return View(student); // show the details page
        }

        [Authorize(Roles = "Admin,Teacher")] // only admin and teacher can create
        public async Task<IActionResult> Create()
        {
            var student = new Student
            {
                StudentID = GenerateStudentID(), // make a new student id
                DOB = DateTime.Today             // default date of birth to today
            };

            var caregivers = await _context.Caregivers
                .Select(c => new { c.CaregiverID, FullName = c.FirstName + " " + c.LastName })
                .OrderBy(c => c.FullName)
                .ToListAsync(); // list for caregiver dropdowns

            ViewData["CaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName"); // main caregiver select
            ViewData["ExtraCaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName"); // optional extra caregiver

            var homerooms = await _context.Homerooms
                .Include(h => h.Teacher)
                .Select(h => new
                {
                    h.HomeroomID,
                    DisplayName = h.Teacher.FirstName + " " + h.Teacher.LastName + " - " + h.Teacher.TeacherCode
                })
                .OrderBy(h => h.DisplayName)
                .ToListAsync(); // list for homeroom dropdown
            ViewData["HomeroomID"] = new SelectList(homerooms, "HomeroomID", "DisplayName"); // set homeroom select

            return View(student); // open the form with defaults
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")] // only admin and teacher can submit create
        public async Task<IActionResult> Create(
            [Bind("StudentID,FirstName,LastName,DOB,Email,HomeroomID,ImageFile,ImagePath")] Student student,
            string caregiverId,              // main caregiver id from the form
            string? extraCaregiverId)        // extra caregiver id from the form
        {
            // main caregiver must be chosen
            if (string.IsNullOrWhiteSpace(caregiverId))
                ModelState.AddModelError("CaregiverID", "Please select a caregiver.");

            // email must be unique for students
            if (await _context.Students.AnyAsync(s => s.Email == student.Email))
                ModelState.AddModelError("Email", "This email is already in use by another student.");

            if (ModelState.IsValid)
            {
                // something is invalid
                // rebuild dropdowns and show the form again
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

            // save image if one was chosen
            if (student.ImageFile != null && student.ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/students"); // folder path
                Directory.CreateDirectory(uploads); // ensure folder exists
                var fileName = Guid.NewGuid() + "_" + Path.GetFileName(student.ImageFile.FileName); // unique file name
                using var fs = new FileStream(Path.Combine(uploads, fileName), FileMode.Create); // open target file
                await student.ImageFile.CopyToAsync(fs); // copy upload
                student.ImagePath = "/images/students/" + fileName; // set web path for the image
            }

            // link up to two caregivers
            student.Caregivers = new List<Caregiver>(); // start the list
            var main = await _context.Caregivers.FindAsync(caregiverId); // load main caregiver
            if (main != null) student.Caregivers.Add(main); // add main caregiver

            if (!string.IsNullOrWhiteSpace(extraCaregiverId) && extraCaregiverId != caregiverId)
            {
                var extra = await _context.Caregivers.FindAsync(extraCaregiverId); // load extra caregiver
                if (extra != null) student.Caregivers.Add(extra); // add extra caregiver
            }

            _context.Add(student); // add the student row
            await _context.SaveChangesAsync(); // write to the database
            return RedirectToAction(nameof(Index)); // go back to the list
        }

        [Authorize(Roles = "Admin,Teacher")] // only admin and teacher can edit
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound(); // no id given

            var student = await _context.Students
                .Include(s => s.Caregivers) // need current caregiver links
                .FirstOrDefaultAsync(s => s.StudentID == id); // get row
            if (student == null) return NotFound(); // not found

            // pick the first caregiver and the second caregiver if present
            var primaryId = student.Caregivers.Select(c => c.CaregiverID).FirstOrDefault(); // first id
            var extraId = student.Caregivers.Select(c => c.CaregiverID).Skip(1).FirstOrDefault(); // second id

            var caregivers = await _context.Caregivers
                .Select(c => new { c.CaregiverID, FullName = c.FirstName + " " + c.LastName })
                .OrderBy(c => c.FullName).ToListAsync(); // dropdown data
            ViewData["CaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName", primaryId); // preselect main
            ViewData["ExtraCaregiverID"] = new SelectList(caregivers, "CaregiverID", "FullName", extraId); // preselect extra

            var homerooms = await _context.Homerooms.Include(h => h.Teacher)
                .Select(h => new { h.HomeroomID, DisplayName = h.Teacher.FirstName + " " + h.Teacher.LastName + " - " + h.Teacher.TeacherCode })
                .OrderBy(h => h.DisplayName).ToListAsync(); // homeroom list
            ViewData["HomeroomID"] = new SelectList(homerooms, "HomeroomID", "DisplayName", student.HomeroomID); // preselect homeroom

            return View(student); // show the edit form
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")] // only admin and teacher can submit edit
        public async Task<IActionResult> Edit(
            string id,
            [Bind("StudentID,FirstName,LastName,DOB,Email,HomeroomID,ImageFile,ImagePath")] Student form,
            string caregiverId,          // main caregiver from form
            string? extraCaregiverId)    // extra caregiver from form
        {
            if (id != form.StudentID) return NotFound(); // guard against wrong id

            var student = await _context.Students
                .Include(s => s.Caregivers) // need current links to update them
                .FirstOrDefaultAsync(s => s.StudentID == id);
            if (student == null) return NotFound(); // not found

            if (string.IsNullOrWhiteSpace(caregiverId))
                ModelState.AddModelError("CaregiverID", "Please select a caregiver."); // main caregiver required

            if (ModelState.IsValid)
            {
                // something invalid
                // rebuild dropdowns and return the form
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

            // copy simple fields from the form
            student.FirstName = form.FirstName;
            student.LastName = form.LastName;
            student.DOB = form.DOB;
            student.Email = form.Email;
            student.HomeroomID = form.HomeroomID;

            // save a new image if present
            if (form.ImageFile != null && form.ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/students"); // folder path
                Directory.CreateDirectory(uploads); // ensure folder exists
                var fileName = Guid.NewGuid() + "_" + Path.GetFileName(form.ImageFile.FileName); // unique file name
                using var fs = new FileStream(Path.Combine(uploads, fileName), FileMode.Create); // open target file
                await form.ImageFile.CopyToAsync(fs); // copy upload
                student.ImagePath = "/images/students/" + fileName; // set web path
            }

            // replace caregiver links
            student.Caregivers.Clear(); // clear old links

            var main = await _context.Caregivers.FindAsync(caregiverId); // load main caregiver
            if (main != null) student.Caregivers.Add(main); // link main caregiver

            if (!string.IsNullOrWhiteSpace(extraCaregiverId) && extraCaregiverId != caregiverId)
            {
                var extra = await _context.Caregivers.FindAsync(extraCaregiverId); // load extra caregiver
                if (extra != null) student.Caregivers.Add(extra); // link extra caregiver
            }

            await _context.SaveChangesAsync(); // write changes
            return RedirectToAction(nameof(Index)); // back to list
        }

        [Authorize(Roles = "Admin,Teacher")] // only admin and teacher can delete
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound(); // no id
            }

            var student = await _context.Students
                .Include(s => s.Caregivers)                                  // include caregivers for display
                .Include(s => s.Homeroom)                                    // include homeroom for display
                .FirstOrDefaultAsync(m => m.StudentID == id);                // load the row

            if (student == null)
            {
                return NotFound(); // not found
            }

            return View(student); // show confirm page
        }

        [Authorize(Roles = "Admin,Teacher")] // only admin and teacher can perform delete
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Students.FindAsync(id); // find the row
            if (student != null)
            {
                _context.Students.Remove(student); // mark for delete
                await _context.SaveChangesAsync(); // write to database
            }

            return RedirectToAction(nameof(Index)); // back to list
        }

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.StudentID == id); // helper to check if a row exists
        }

        private string GenerateStudentID()
        {
            // Start the ID with ac and the last two digits of the year
            string yearPrefix = "ac" + DateTime.Now.ToString("yy"); // example ac25

            // Find the latest id that uses this year prefix
            var latestId = _context.Students
                .Where(s => s.StudentID.StartsWith(yearPrefix))
                .OrderByDescending(s => s.StudentID)
                .Select(s => s.StudentID)
                .FirstOrDefault(); // newest id for this year

            // Work out the next number
            int nextNumber = 1; // default is one
            if (!string.IsNullOrEmpty(latestId) && latestId.Length == 8)
            {
                int.TryParse(latestId.Substring(4), out nextNumber); // read the number part
                nextNumber++; // go to the next number
            }

            // Return an id like ac250001
            return $"{yearPrefix}{nextNumber:D4}"; // year plus four digits
        }

    }
}
