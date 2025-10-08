using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;
using System.IO;
using AvondaleCollegeClinic.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AvondaleCollegeClinic.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public TeachersController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Doctor,Student,Caregiver,Admin,Teacher")]
        // GET: Teachers
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CodeSortParm"] = sortOrder == "Code" ? "code_desc" : "Code";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var teachers = from t in _context.Teachers select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(t =>
                    t.LastName.Contains(searchString) ||
                    t.FirstName.Contains(searchString) ||
                    t.TeacherCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    teachers = teachers.OrderByDescending(t => t.LastName);
                    break;
                case "Code":
                    teachers = teachers.OrderBy(t => t.TeacherCode);
                    break;
                case "code_desc":
                    teachers = teachers.OrderByDescending(t => t.TeacherCode);
                    break;
                default:
                    teachers = teachers.OrderBy(t => t.LastName);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<Teacher>.CreateAsync(
                teachers.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Teachers/Create
        public IActionResult Create()
        {
            var teacher = new Teacher
            {
                TeacherID = GenerateTeacherID()
            };
            ViewBag.TeacherID = new SelectList( _context.Teachers.Select(t => new
                {
                    t.TeacherID,
                    FullName = t.FirstName + " " + t.LastName
                }),
                "TeacherID", "FullName");
            return View(teacher);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherID,FirstName,LastName,Email,TeacherCode,ImageFile,ImagePath")] Teacher teacher)
        {
            // keep your pattern: save when !ModelState.IsValid
            if (!ModelState.IsValid)
            {
                // Check email
                if (await _context.Teachers.AnyAsync(t => t.Email == teacher.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use by another teacher.");
                    return View(teacher);
                }

                // Check full name
                if (await _context.Teachers.AnyAsync(t => t.FirstName == teacher.FirstName && t.LastName == teacher.LastName))
                {
                    ModelState.AddModelError("", "A teacher with the same name already exists.");
                    return View(teacher);
                }

                if (teacher.ImageFile != null && teacher.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/teachers");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(teacher.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await teacher.ImageFile.CopyToAsync(fs);
                    }

                    teacher.ImagePath = "/images/teachers/" + uniqueFileName;
                }

                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(teacher);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            ViewBag.TeacherID = new SelectList(_context.Teachers.Select(t => new
                {
                    t.TeacherID,
                    FullName = t.FirstName + " " + t.LastName
                }),
                "TeacherID", "FullName");
            return View(teacher);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TeacherID,FirstName,LastName,Email,TeacherCode,ImageFile")] Teacher form)
        {
            if (id != form.TeacherID)
            {
                return NotFound();
            }

            // keep your pattern: save when !ModelState.IsValid
            if (!ModelState.IsValid)
            {
                // load existing so we can preserve ImagePath
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherID == id);
                if (teacher == null) return NotFound();

                // update simple fields
                teacher.FirstName = form.FirstName;
                teacher.LastName = form.LastName;
                teacher.Email = form.Email;
                teacher.TeacherCode = form.TeacherCode;

                // Check email
                if (await _context.Teachers.AnyAsync(t => t.Email == teacher.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use by another teacher.");
                    return View(teacher);
                }

                // Check full name
                if (await _context.Teachers.AnyAsync(t => t.FirstName == teacher.FirstName && t.LastName == teacher.LastName))
                {
                    ModelState.AddModelError("", "A teacher with the same name already exists.");
                    return View(teacher);
                }


                // handle new upload (optional)
                if (form.ImageFile != null && form.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/teachers");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(form.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await form.ImageFile.CopyToAsync(fs);
                    }

                    teacher.ImagePath = "/images/teachers/" + uniqueFileName;
                    // (optional) delete old file here if you want to clean up
                }
                // else keep existing teacher.ImagePath

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(form);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(string id)
        {
            return _context.Teachers.Any(e => e.TeacherID == id);
        }

        private string GenerateTeacherID()
        {
            string newId;
            var random = new Random();
            int year = DateTime.Now.Year % 100;

            do
            {
                int num = random.Next(1000, 9999);
                newId = $"act{year}{num}";
            }
            while (_context.Teachers.Any(t => t.TeacherID == newId));

            return newId;
        }
        // This method creates a unique TeacherID for every new teacher.
        // Which as a format of act-YYXXXX
        // "act" referes to Avondale College Teacher
        // "YY"  last two digits of the current year (e.g., 2025 -> 25)
        // "XXXX" is a random 4-digit number (from 1000 to 9999)
        // The method checks the database to make sure the generated ID doesn't already exist. If it does, it tries again until it finds a unique one.
    }
}
