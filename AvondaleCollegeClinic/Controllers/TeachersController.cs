using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;

namespace AvondaleCollegeClinic.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public TeachersController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teachers.ToListAsync());
        }

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

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherID,FirstName,LastName,Email,TeacherCode")] Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

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

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TeacherID,FirstName,LastName,Email,TeacherCode")] Teacher teacher)
        {
            if (id != teacher.TeacherID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherID))
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
            return View(teacher);
        }

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
                newId = $"act-{year}{num}";
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
