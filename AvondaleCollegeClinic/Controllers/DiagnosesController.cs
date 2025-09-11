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

namespace AvondaleCollegeClinic.Controllers
{
    public class DiagnosesController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public DiagnosesController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        // GET: Diagnoses
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            var diagnoses = await _context.Diagnoses
                .Include(d => d.Appointment)
                    .ThenInclude(a => a.Student)
                .Include(d => d.Appointment.Doctor)
                .AsNoTracking()
                .ToListAsync();

            // 🔍 Filter
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                diagnoses = diagnoses.Where(d =>
                    d.Description.ToLower().Contains(searchString) ||
                    d.Appointment.Student.FirstName.ToLower().Contains(searchString) ||
                    d.Appointment.Student.LastName.ToLower().Contains(searchString) ||
                    d.Appointment.Doctor.FirstName.ToLower().Contains(searchString) ||
                    d.Appointment.Doctor.LastName.ToLower().Contains(searchString)
                ).ToList();
            }

            // ↕ Sort
            switch (sortOrder)
            {
                case "date_desc":
                    diagnoses = diagnoses.OrderByDescending(d => d.DateDiagnosed).ToList();
                    break;
                default:
                    diagnoses = diagnoses.OrderBy(d => d.DateDiagnosed).ToList();
                    break;
            }

            // 📄 Pagination
            int pageSize = 10;
            int page = pageNumber ?? 1;
            var totalCount = diagnoses.Count;
            var pagedDiagnoses = diagnoses.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var paginatedList = new PaginatedList<Diagnosis>(pagedDiagnoses, totalCount, page, pageSize);
            return View(paginatedList);
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Appointment)
                .FirstOrDefaultAsync(m => m.DiagnosisID == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // GET: Diagnoses/Create
        public IActionResult Create()
        {
            ViewBag.AppointmentID = new SelectList(_context.Appointments
                .Include(a => a.Student)
                .Include(a => a.Doctor)
                .Select(a => new
                {
                    a.AppointmentID,
                    Display = a.Student.FirstName + " " + a.Student.LastName +
                              " by " + a.Doctor.FirstName + " " + a.Doctor.LastName +
                              " - " + a.AppointmentDateTime.ToString("dd MMM yyyy")
                }),
                "AppointmentID", "Display");

            return View();
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiagnosisID,AppointmentID,Description,DateDiagnosed")] Diagnosis diagnosis)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(diagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentID", "AppointmentID", diagnosis.AppointmentID);
            return View(diagnosis);
        }

        // GET: Diagnoses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis == null)
            {
                return NotFound();
            }
            ViewBag.AppointmentID = new SelectList(_context.Appointments
                    .Include(a => a.Student)
                    .Include(a => a.Doctor)
                    .Select(a => new
                    {
                        a.AppointmentID,
                        Display = a.Student.FirstName + " " + a.Student.LastName +
                                  " by " + a.Doctor.FirstName + " " + a.Doctor.LastName +
                                  " - " + a.AppointmentDateTime.ToString("dd MMM yyyy")
                    }),
                    "AppointmentID", "Display", diagnosis.AppointmentID);
            return View(diagnosis);
        }

        // POST: Diagnoses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiagnosisID,AppointmentID,Description,DateDiagnosed")] Diagnosis diagnosis)
        {
            if (id != diagnosis.DiagnosisID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisExists(diagnosis.DiagnosisID))
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
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "AppointmentID", "AppointmentID", diagnosis.AppointmentID);
            return View(diagnosis);
        }

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Appointment)
                .FirstOrDefaultAsync(m => m.DiagnosisID == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis != null)
            {
                _context.Diagnoses.Remove(diagnosis);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosisExists(int id)
        {
            return _context.Diagnoses.Any(e => e.DiagnosisID == id);
        }
    }
}
