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
using AvondaleCollegeClinic.Areas.Identity.Data;
using System.Globalization;

namespace AvondaleCollegeClinic.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;
        private readonly UserManager<AvondaleCollegeClinicUser> _users;   

        public AppointmentsController(
            AvondaleCollegeClinicContext context,
            UserManager<AvondaleCollegeClinicUser> users)              
        {
            _context = context;
            _users = users;                                               
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";

            if (searchString != null) pageNumber = 1;
            else searchString = currentFilter;
            ViewData["CurrentFilter"] = searchString;

            // Who is signed in?
            var me = await _users.GetUserAsync(User);
            if (me == null) return Challenge();

            var uid = me.Id;
            var uname = me.UserName ?? "";
            var email = me.Email ?? "";

            // Resolve domain IDs for the current user
            var studentId = await _context.Students
                .Where(s => s.IdentityUserId == uid || s.StudentID == uname || s.Email == email)
                .Select(s => s.StudentID)
                .FirstOrDefaultAsync();

            var caregiverId = await _context.Caregivers
                .Where(c => c.IdentityUserId == uid || c.CaregiverID == uname || c.Email == email)
                .Select(c => c.CaregiverID)
                .FirstOrDefaultAsync();

            // Base query
            var query = _context.Appointments
                .Include(a => a.Student)
                .Include(a => a.Doctor)
                .AsQueryable();

            // Ownership filter
            if (User.IsInRole("Student") && !string.IsNullOrEmpty(studentId))
            {
                query = query.Where(a => a.StudentID == studentId);
            }
            else if (User.IsInRole("Caregiver") && !string.IsNullOrEmpty(caregiverId))
            {
                query = query.Where(a => a.Student.CaregiverID == caregiverId);
            }
            else if (User.IsInRole("Admin") || User.IsInRole("Doctor") || User.IsInRole("Teacher"))
            {
                // see all (leave unfiltered)
            }
            else
            {
                return Forbid();
            }

            // Search (name/status/reason)
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = $"%{searchString.Trim()}%";
                query = query.Where(a =>
                    EF.Functions.Like(a.Student.FirstName, term) ||
                    EF.Functions.Like(a.Student.LastName, term) ||
                    EF.Functions.Like(a.Doctor.FirstName, term) ||
                    EF.Functions.Like(a.Doctor.LastName, term) ||
                    EF.Functions.Like(a.Reason, term) ||
                    EF.Functions.Like(a.Status.ToString(), term));
            }

            // Sort
            query = sortOrder switch
            {
                "date_desc" => query.OrderByDescending(a => a.AppointmentDateTime),
                "Status" => query.OrderBy(a => a.Status),
                "status_desc" => query.OrderByDescending(a => a.Status),
                _ => query.OrderBy(a => a.AppointmentDateTime)
            };

            // Paging (in DB)
            const int pageSize = 5;
            int page = pageNumber ?? 1;

            var total = await query.CountAsync();
            var rows = await query.AsNoTracking()
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return View(new PaginatedList<Appointment>(rows, total, page, pageSize));
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
