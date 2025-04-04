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
    public class DoctorAvailabilitiesController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public DoctorAvailabilitiesController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        // GET: DoctorAvailabilities
        public async Task<IActionResult> Index()
        {
            var avondaleCollegeClinicContext = _context.DoctorAvailabilities.Include(d => d.Doctor);
            return View(await avondaleCollegeClinicContext.ToListAsync());
        }

        // GET: DoctorAvailabilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorAvailability = await _context.DoctorAvailabilities
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.DoctorAvailabilityID == id);
            if (doctorAvailability == null)
            {
                return NotFound();
            }

            return View(doctorAvailability);
        }

        // GET: DoctorAvailabilities/Create
        public IActionResult Create()
        {
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID");
            return View();
        }

        // POST: DoctorAvailabilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorAvailabilityID,DoctorID,AvailableDate,StartTime,EndTime")] DoctorAvailability doctorAvailability)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(doctorAvailability);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", doctorAvailability.DoctorID);
            return View(doctorAvailability);
        }

        // GET: DoctorAvailabilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorAvailability = await _context.DoctorAvailabilities.FindAsync(id);
            if (doctorAvailability == null)
            {
                return NotFound();
            }
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", doctorAvailability.DoctorID);
            return View(doctorAvailability);
        }

        // POST: DoctorAvailabilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorAvailabilityID,DoctorID,AvailableDate,StartTime,EndTime")] DoctorAvailability doctorAvailability)
        {
            if (id != doctorAvailability.DoctorAvailabilityID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorAvailability);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorAvailabilityExists(doctorAvailability.DoctorAvailabilityID))
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
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", doctorAvailability.DoctorID);
            return View(doctorAvailability);
        }

        // GET: DoctorAvailabilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorAvailability = await _context.DoctorAvailabilities
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.DoctorAvailabilityID == id);
            if (doctorAvailability == null)
            {
                return NotFound();
            }

            return View(doctorAvailability);
        }

        // POST: DoctorAvailabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctorAvailability = await _context.DoctorAvailabilities.FindAsync(id);
            if (doctorAvailability != null)
            {
                _context.DoctorAvailabilities.Remove(doctorAvailability);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorAvailabilityExists(int id)
        {
            return _context.DoctorAvailabilities.Any(e => e.DoctorAvailabilityID == id);
        }
    }
}
