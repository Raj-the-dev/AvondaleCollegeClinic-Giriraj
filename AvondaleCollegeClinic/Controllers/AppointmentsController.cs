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
    public class AppointmentsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public AppointmentsController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
            ViewData["CurrentFilter"] = searchString;

            var appointments = await _context.Appointments
                .Include(a => a.Student)
                .Include(a => a.Doctor)
                .AsNoTracking()
                .ToListAsync();

            // 🔍 Filter
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                appointments = appointments.Where(a =>
                    a.Student.FirstName.ToLower().Contains(searchString) ||
                    a.Student.LastName.ToLower().Contains(searchString) ||
                    a.Doctor.FirstName.ToLower().Contains(searchString) ||
                    a.Doctor.LastName.ToLower().Contains(searchString) ||
                    a.Reason.ToLower().Contains(searchString) ||
                    a.Status.ToString().ToLower().Contains(searchString)
                ).ToList();
            }

            // ↕ Sort
            switch (sortOrder)
            {
                case "date_desc":
                    appointments = appointments.OrderByDescending(a => a.AppointmentDateTime).ToList();
                    break;
                case "Status":
                    appointments = appointments.OrderBy(a => a.Status).ToList();
                    break;
                case "status_desc":
                    appointments = appointments.OrderByDescending(a => a.Status).ToList();
                    break;
                default:
                    appointments = appointments.OrderBy(a => a.AppointmentDateTime).ToList();
                    break;
            }

            // 📄 Pagination
            int pageSize = 10;
            int page = pageNumber ?? 1;
            var totalCount = appointments.Count;
            var pagedAppointments = appointments.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var paginatedList = new PaginatedList<Appointment>(pagedAppointments, totalCount, page, pageSize);
            return View(paginatedList);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            var appointment = new Appointment();
            ViewBag.StudentID = new SelectList(_context.Students.Select(s => new {
                s.StudentID,
                FullName = s.FirstName + " " + s.LastName
            }), "StudentID", "FullName");

            ViewBag.DoctorID = new SelectList(_context.Doctors.Select(d => new {
                d.DoctorID,
                FullName = d.FirstName + " " + d.LastName
            }), "DoctorID", "FullName");
            return View(appointment);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentID,StudentID,DoctorID,AppointmentDateTime,Status,Reason")] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", appointment.DoctorID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", appointment.StudentID);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewBag.StudentID = new SelectList(_context.Students.Select(s => new {
                s.StudentID,
                FullName = s.FirstName + " " + s.LastName
            }), "StudentID", "FullName");

            ViewBag.DoctorID = new SelectList(_context.Doctors.Select(d => new {
                d.DoctorID,
                FullName = d.FirstName + " " + d.LastName
            }), "DoctorID", "FullName");
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentID,StudentID,DoctorID,AppointmentDateTime,Status,Reason")] Appointment appointment)
        {
            if (id != appointment.AppointmentID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentID))
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
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", appointment.DoctorID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentID", "StudentID", appointment.StudentID);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }
    }
}
