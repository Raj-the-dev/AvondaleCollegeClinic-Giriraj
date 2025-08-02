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
    public class DoctorsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public DoctorsController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(string searchString)
        {
            var doctors = from s in _context.Doctors
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                doctors = doctors.Where(s => s.LastName.Contains(searchString)
                                           || s.FirstName.Contains(searchString));
            }

            return View(await doctors.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            var doctor = new Doctor
            {
                DoctorID = GenerateDoctorID()
            };
            return View(doctor);
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorID,FirstName,LastName,Photo,Specialization,Email,Phone")] Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DoctorID,FirstName,LastName,Photo,Specialization,Email,Phone")] Doctor doctor)
        {
            if (id != doctor.DoctorID) return NotFound();

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.DoctorID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(string id)
        {
            return _context.Doctors.Any(e => e.DoctorID == id);
        }

        private string GenerateDoctorID()
        {
            string newId;
            var random = new Random();
            int year = DateTime.Now.Year % 100;

            do
            {
                int num = random.Next(1000, 9999);
                newId = $"acd-{year}{num}";
            }
            while (_context.Doctors.Any(d => d.DoctorID == newId));

            return newId;
        }
        // This method creates a unique DoctorID for every new teacher.
        // Which as a format of acd-YYXXXX
        // "act" referes to Avondale College Doctors
        // "YY"  last two digits of the current year (e.g., 2025 -> 25)
        // "XXXX" is a random 4-digit number (from 1000 to 9999)
        // The method checks the database to make sure the generated ID doesn't already exist. If it does, it tries again until it finds a unique one.
    }
}
