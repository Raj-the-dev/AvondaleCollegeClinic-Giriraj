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
                           .Include(s => s.Caregiver)
                           .Include(s => s.Homeroom)
                               .ThenInclude(h => h.Teacher)
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
                .Include(s => s.Caregiver)
                .Include(s => s.Homeroom).ThenInclude(h => h.Teacher)
                .FirstOrDefaultAsync(m => m.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Students/Create
        public IActionResult Create()
        {
            var student = new Student
            {
                StudentID = GenerateStudentID(),
                DOB = DateTime.Now
            };

            ViewData["CaregiverID"] = new SelectList(
                _context.Caregivers.Select(c => new
                {
                    c.CaregiverID,
                    FullName = c.FirstName + " " + c.LastName
                }).ToList(),
                "CaregiverID",
                "FullName"
            );

            ViewData["HomeroomID"] = new SelectList(
                _context.Homerooms.Include(h => h.Teacher).Select(h => new
                {
                    h.HomeroomID,
                    DisplayName = h.Teacher.FirstName + " " + h.Teacher.LastName + " - " + h.Teacher.TeacherCode
                }).ToList(),
                "HomeroomID",
                "DisplayName"
            );

            return View(student);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,FirstName,LastName,Photo,DOB,Email,HomeroomID,CaregiverID,ImageFile,ImagePath")] Student student)
        {
            if (!ModelState.IsValid)
            {
                string uniqueFileName = null;

                // Check email
                if (await _context.Students.AnyAsync(s => s.Email == student.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use by another student.");
                    return View(student);
                }

                // Check full name
                if (await _context.Students.AnyAsync(s => s.FirstName == student.FirstName && s.LastName == student.LastName))
                {
                    ModelState.AddModelError("", "A student with the same name already exists.");
                    return View(student);
                }

                if (student.ImageFile != null)
                {
                    // Ensure directory exists
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/students");
                    Directory.CreateDirectory(uploadsFolder);

                    // Generate unique filename
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(student.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await student.ImageFile.CopyToAsync(fileStream);
                    }

                    // Store relative path
                    student.ImagePath = "/images/students/" + uniqueFileName;
                }

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if validation fails
            ViewData["CaregiverID"] = new SelectList(
                _context.Caregivers.Select(c => new
                {
                    c.CaregiverID,
                    FullName = c.FirstName + " " + c.LastName
                }).ToList(),
                "CaregiverID",
                "FullName",
                student.CaregiverID
            );

            ViewData["HomeroomID"] = new SelectList(
                _context.Homerooms.Include(h => h.Teacher).Select(h => new
                {
                    h.HomeroomID,
                    DisplayName = h.Teacher.FirstName + " " + h.Teacher.LastName + " - " + h.Teacher.TeacherCode
                }).ToList(),
                "HomeroomID",
                "DisplayName",
                student.HomeroomID
            );
            return View(student);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            ViewData["CaregiverID"] = new SelectList(
                _context.Caregivers.Select(c => new
                {
                    c.CaregiverID,
                    FullName = c.FirstName + " " + c.LastName
                }).ToList(),
                "CaregiverID",
                "FullName",
                student.CaregiverID
            );

            ViewData["HomeroomID"] = new SelectList(
                _context.Homerooms.Include(h => h.Teacher).Select(h => new
                {
                    h.HomeroomID,
                    DisplayName = h.Teacher.FirstName + " " + h.Teacher.LastName + " - " + h.Teacher.TeacherCode
                }).ToList(),
                "HomeroomID",
                "DisplayName",
                student.HomeroomID
            );

            return View(student);
        }


        [Authorize(Roles = "Admin,Teacher")]
        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,[Bind("StudentID,FirstName,LastName,Photo,DOB,Email,HomeroomID,CaregiverID,ImageFile")]Student form)
        {
            if (id != form.StudentID)
                return NotFound();

            // Save only when valid
            if (!ModelState.IsValid)
            {
                // Load the existing student so we can preserve the current ImagePath
                var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
                if (student == null) return NotFound();

                // Update simple fields
                student.FirstName = form.FirstName;
                student.LastName = form.LastName;
                student.DOB = form.DOB;
                student.Email = form.Email;
                student.HomeroomID = form.HomeroomID;
                student.CaregiverID = form.CaregiverID;


                // If a new image was uploaded, save it and update ImagePath
                if (form.ImageFile != null && form.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/students");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(form.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await form.ImageFile.CopyToAsync(fileStream);
                    }

                    student.ImagePath = "/images/students/" + uniqueFileName;
                     
                }
    

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

         
            ViewData["CaregiverID"] = new SelectList(
                _context.Caregivers.Select(c => new { c.CaregiverID, FullName = c.FirstName + " " + c.LastName }).ToList(),
                "CaregiverID", "FullName", form.CaregiverID);

            ViewData["HomeroomID"] = new SelectList(
                _context.Homerooms.Include(h => h.Teacher).Select(h => new
                {
                    h.HomeroomID,
                    DisplayName = h.Teacher.FirstName + " " + h.Teacher.LastName + " - " + h.Teacher.TeacherCode
                }).ToList(),
                "HomeroomID", "DisplayName", form.HomeroomID);

            return View(form);
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
                .Include(s => s.Caregiver)
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
