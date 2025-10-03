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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace AvondaleCollegeClinic.Controllers
{
    [Authorize(Roles = "Doctor,Student,Caregiver,Admin")]
    public class PrescriptionsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;
        private readonly UserManager<AvondaleCollegeClinicUser> _userManager;

        public PrescriptionsController(AvondaleCollegeClinicContext context, UserManager<AvondaleCollegeClinicUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Prescriptions
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["PrescriptionSortParm"] = String.IsNullOrEmpty(sortOrder) ? "presc_desc" : "";
            ViewData["DosageSortParm"] = sortOrder == "Dosage" ? "dosage_desc" : "Dosage";
            ViewData["CurrentFilter"] = searchString;

            // Base query: include related entities
            var query = _context.Prescriptions
                .Include(p => p.Diagnosis)
                    .ThenInclude(d => d.Appointment)
                        .ThenInclude(a => a.Student)
                .AsQueryable();

            // Optional search (DB-level LIKE for speed)
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = $"%{searchString.Trim()}%";
                query = query.Where(p =>
                    EF.Functions.Like(p.Medication, term) ||
                    EF.Functions.Like(p.Dosage, term) ||
                    EF.Functions.Like(p.Diagnosis.Appointment.Student.FirstName, term) ||
                    EF.Functions.Like(p.Diagnosis.Appointment.Student.LastName, term)
                );
            }

            var prescriptions = await query.AsNoTracking().ToListAsync();

            // Client-side filtering (extra layer, not strictly necessary if DB filtering works)
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                prescriptions = prescriptions.Where(p =>
                    p.Medication.ToLower().Contains(searchString) ||
                    p.Dosage.ToLower().Contains(searchString) ||
                    p.Diagnosis.Appointment.Student.FirstName.ToLower().Contains(searchString) ||
                    p.Diagnosis.Appointment.Student.LastName.ToLower().Contains(searchString)
                ).ToList();
            }

            // Sorting
            switch (sortOrder)
            {
                case "presc_desc":
                    prescriptions = prescriptions.OrderByDescending(p => p.Medication).ToList();
                    break;
                case "Dosage":
                    prescriptions = prescriptions.OrderBy(p => p.Dosage).ToList();
                    break;
                case "dosage_desc":
                    prescriptions = prescriptions.OrderByDescending(p => p.Dosage).ToList();
                    break;
                default:
                    prescriptions = prescriptions.OrderBy(p => p.Medication).ToList();
                    break;
            }

            // Pagination
            int pageSize = 10;
            int page = pageNumber ?? 1;
            return View(new PaginatedList<Prescription>(
                prescriptions.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                prescriptions.Count, page, pageSize
            ));
        }


        [Authorize(Roles = "Admin,Doctor")]
        // GET: Prescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions
                .Include(p => p.Diagnosis)
                .FirstOrDefaultAsync(m => m.PrescriptionID == id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // GET: Prescriptions/Create
        public IActionResult Create()
        {
            ViewBag.DiagnosisID = new SelectList(_context.Diagnoses
                .Include(d => d.Appointment)
                    .ThenInclude(a => a.Student)
                .Include(d => d.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Select(d => new
                {
                    d.DiagnosisID,
                    Display = d.Appointment.Student.FirstName + " " + d.Appointment.Student.LastName +
                              " by " + d.Appointment.Doctor.FirstName + " " + d.Appointment.Doctor.LastName +
                              " - " + d.Appointment.AppointmentDateTime.ToString("dd MMM yyyy")
                }),
                "DiagnosisID", "Display");

            return View();
        }

        [Authorize(Roles = "Admin,Doctor")]
        // POST: Prescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrescriptionID,DiagnosisID,Medication,Dosage,StartDate,EndDate")] Prescription prescription)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(prescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiagnosisID"] = new SelectList(_context.Diagnoses, "DiagnosisID", "DiagnosisID", prescription.DiagnosisID);
            return View(prescription);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // GET: Prescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }
            ViewBag.DiagnosisID = new SelectList(_context.Diagnoses
                .Include(d => d.Appointment)
                    .ThenInclude(a => a.Student)
                .Include(d => d.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Select(d => new
                {
                    d.DiagnosisID,
                    Display = d.Appointment.Student.FirstName + " " + d.Appointment.Student.LastName +
                              " by " + d.Appointment.Doctor.FirstName + " " + d.Appointment.Doctor.LastName +
                              " - " + d.Appointment.AppointmentDateTime.ToString("dd MMM yyyy")
                }),
                "DiagnosisID", "Display", prescription.DiagnosisID);

            return View(prescription);
            return View(prescription);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // POST: Prescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrescriptionID,DiagnosisID,Medication,Dosage,StartDate,EndDate")] Prescription prescription)
        {
            if (id != prescription.PrescriptionID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrescriptionExists(prescription.PrescriptionID))
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
            ViewData["DiagnosisID"] = new SelectList(_context.Diagnoses, "DiagnosisID", "DiagnosisID", prescription.DiagnosisID);
            return View(prescription);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // GET: Prescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescription = await _context.Prescriptions
                .Include(p => p.Diagnosis)
                .FirstOrDefaultAsync(m => m.PrescriptionID == id);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription != null)
            {
                _context.Prescriptions.Remove(prescription);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionExists(int id)
        {
            return _context.Prescriptions.Any(e => e.PrescriptionID == id);
        }

    }
}
