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
    public class StudentsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public StudentsController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var avondaleCollegeClinicContext = _context.Students.Include(s => s.Caregiver).Include(s => s.Homeroom);
            return View(await avondaleCollegeClinicContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["CaregiverID"] = new SelectList(_context.Caregivers, "CaregiverID", "CaregiverID");
            ViewData["HomeroomID"] = new SelectList(_context.Homerooms, "HomeroomID", "HomeroomID");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,FirstName,LastName,Photo,DOB,Email,HomeroomID,CaregiverID")] Student student)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaregiverID"] = new SelectList(_context.Caregivers, "CaregiverID", "CaregiverID", student.CaregiverID);
            ViewData["HomeroomID"] = new SelectList(_context.Homerooms, "HomeroomID", "HomeroomID", student.HomeroomID);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["CaregiverID"] = new SelectList(_context.Caregivers, "CaregiverID", "CaregiverID", student.CaregiverID);
            ViewData["HomeroomID"] = new SelectList(_context.Homerooms, "HomeroomID", "HomeroomID", student.HomeroomID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StudentID,FirstName,LastName,Photo,DOB,Email,HomeroomID,CaregiverID")] Student student)
        {
            if (id != student.StudentID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentID))
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
            ViewData["CaregiverID"] = new SelectList(_context.Caregivers, "CaregiverID", "CaregiverID", student.CaregiverID);
            ViewData["HomeroomID"] = new SelectList(_context.Homerooms, "HomeroomID", "HomeroomID", student.HomeroomID);
            return View(student);
        }

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

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }
    }
}
