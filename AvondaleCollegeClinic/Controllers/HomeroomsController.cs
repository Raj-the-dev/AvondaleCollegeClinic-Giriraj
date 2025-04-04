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
    public class HomeroomsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public HomeroomsController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        // GET: Homerooms
        public async Task<IActionResult> Index()
        {
            var avondaleCollegeClinicContext = _context.Homerooms.Include(h => h.Teacher);
            return View(await avondaleCollegeClinicContext.ToListAsync());
        }

        // GET: Homerooms/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeroom = await _context.Homerooms
                .Include(h => h.Teacher)
                .FirstOrDefaultAsync(m => m.HomeroomID == id);
            if (homeroom == null)
            {
                return NotFound();
            }

            return View(homeroom);
        }

        // GET: Homerooms/Create
        public IActionResult Create()
        {
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "TeacherID");
            return View();
        }

        // POST: Homerooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeroomID,YearLevel,TeacherID,Class")] Homeroom homeroom)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(homeroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "TeacherID", homeroom.TeacherID);
            return View(homeroom);
        }

        // GET: Homerooms/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeroom = await _context.Homerooms.FindAsync(id);
            if (homeroom == null)
            {
                return NotFound();
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "TeacherID", homeroom.TeacherID);
            return View(homeroom);
        }

        // POST: Homerooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("HomeroomID,YearLevel,TeacherID,Class")] Homeroom homeroom)
        {
            if (id != homeroom.HomeroomID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(homeroom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeroomExists(homeroom.HomeroomID))
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
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "TeacherID", homeroom.TeacherID);
            return View(homeroom);
        }

        // GET: Homerooms/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeroom = await _context.Homerooms
                .Include(h => h.Teacher)
                .FirstOrDefaultAsync(m => m.HomeroomID == id);
            if (homeroom == null)
            {
                return NotFound();
            }

            return View(homeroom);
        }

        // POST: Homerooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var homeroom = await _context.Homerooms.FindAsync(id);
            if (homeroom != null)
            {
                _context.Homerooms.Remove(homeroom);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeroomExists(string id)
        {
            return _context.Homerooms.Any(e => e.HomeroomID == id);
        }
    }
}
