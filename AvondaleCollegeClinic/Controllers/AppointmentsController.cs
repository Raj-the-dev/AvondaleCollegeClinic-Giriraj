using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Helpers;
using AvondaleCollegeClinic.Models;

namespace AvondaleCollegeClinic.Controllers
{
    [Authorize] // you must be signed in for all actions here
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

        // =========================
        // Helpers to find "who am I"
        // =========================

        // If the current user is a Student, return their StudentID; otherwise null
        private async Task<string?> GetCurrentStudentIdAsync()
        {
            var me = await _users.GetUserAsync(User);
            if (me == null) return null;
            var uid = me.Id;
            var uname = me.UserName ?? "";
            var email = me.Email ?? "";

            return await _context.Students
                .Where(s => s.IdentityUserId == uid || s.StudentID == uname || s.Email == email)
                .Select(s => s.StudentID)
                .FirstOrDefaultAsync();
        }

        // If the current user is a Caregiver, return their CaregiverID; otherwise null
        private async Task<string?> GetCurrentCaregiverIdAsync()
        {
            var me = await _users.GetUserAsync(User);
            if (me == null) return null;
            var uid = me.Id;
            var uname = me.UserName ?? "";
            var email = me.Email ?? "";

            return await _context.Caregivers
                .Where(c => c.IdentityUserId == uid || c.CaregiverID == uname || c.Email == email)
                .Select(c => c.CaregiverID)
                .FirstOrDefaultAsync();
        }

        // Build the Student dropdown based on role:
        // - Student sees ONLY themselves (and we lock the dropdown)
        // - Caregiver sees ONLY their students
        // - Admin/Doctor/Teacher see ALL students
        // Build the Student dropdown with role-based restriction and "lock" flag
        private async Task PopulateStudentSelectAsync(string? selectedStudentId = null)
        {
            // Default: everyone can see all students
            var studentsQuery = _context.Students
                .Select(s => new { s.StudentID, FullName = s.FirstName + " " + s.LastName })
                .OrderBy(s => s.FullName);

            bool lockStudent = false;

            // If the signed-in user is a Student, restrict to themselves and lock the dropdown
            if (User.IsInRole("Student"))
            {
                var me = await _users.GetUserAsync(User);
                if (me != null)
                {
                    var myStudentId = await _context.Students
                        .Where(s => s.IdentityUserId == me.Id || s.Email == me.Email || s.StudentID == me.UserName)
                        .Select(s => s.StudentID)
                        .FirstOrDefaultAsync();

                    if (!string.IsNullOrWhiteSpace(myStudentId))
                    {
                        selectedStudentId ??= myStudentId;
                        lockStudent = true;

                        // Only include *me* in the list
                        studentsQuery = _context.Students
                            .Where(s => s.StudentID == myStudentId)
                            .Select(s => new { s.StudentID, FullName = s.FirstName + " " + s.LastName })
                            .OrderBy(s => s.FullName);
                    }
                }
            }

            var students = await studentsQuery.ToListAsync();
            // If nothing selected yet, pick the first row (if any)
            selectedStudentId ??= students.FirstOrDefault()?.StudentID;

            ViewBag.StudentID = new SelectList(students, "StudentID", "FullName", selectedStudentId);
            ViewBag.LockStudent = lockStudent; // view uses this to render a disabled select + hidden input
        }

        // Build the Doctor dropdown and remember the *first* doctor's id for initial slot load
        private async Task PopulateDoctorSelectAsync(string? selectedDoctorId = null)
        {
            var doctors = await _context.Doctors
                .Select(d => new { d.DoctorID, FullName = d.FirstName + " " + d.LastName })
                .OrderBy(d => d.FullName)
                .ToListAsync();

            // If nothing selected yet, choose the first doctor (if any)
            selectedDoctorId ??= doctors.FirstOrDefault()?.DoctorID;

            ViewBag.DoctorID = new SelectList(doctors, "DoctorID", "FullName", selectedDoctorId);

            // This is what your Create() uses to preload slots on first render
            ViewBag.FirstDoctorId = selectedDoctorId;
        }

        // =========================
        // Slot generator
        // =========================
        // Returns a list of SelectListItem: Value = ISO 8601 string (round-trip), Text = "h:mm tt"
        // If editingAppointmentId is supplied, that appointment’s existing time will not be treated as "booked".
        private async Task<List<SelectListItem>> BuildSlotsAsync(string doctorId, DateTime date, int? editingAppointmentId = null)
        {
            var items = new List<SelectListItem>();
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorID == doctorId);
            if (doctor == null) return items;

            // Simple "is this a working day?" switch on the doctor’s weekly flags
            bool Works(DayOfWeek dow) => dow switch
            {
                DayOfWeek.Monday => doctor.WorksMon,
                DayOfWeek.Tuesday => doctor.WorksTue,
                DayOfWeek.Wednesday => doctor.WorksWed,
                DayOfWeek.Thursday => doctor.WorksThu,
                DayOfWeek.Friday => doctor.WorksFri,
                DayOfWeek.Saturday => doctor.WorksSat,
                DayOfWeek.Sunday => doctor.WorksSun,
                _ => false
            };

            if (!Works(date.DayOfWeek)) return items; // not a working day → no slots

            // Make times for start/end on that date
            var start = date.Date.Add(doctor.DailyStartTime);
            var end = date.Date.Add(doctor.DailyEndTime);
            var step = TimeSpan.FromMinutes(doctor.SlotMinutes <= 0 ? 30 : doctor.SlotMinutes);

            // Find already-booked times for that doctor on that date
            var booked = await _context.Appointments
                .Where(a => a.DoctorID == doctorId && a.AppointmentDateTime.Date == date.Date)
                .Where(a => editingAppointmentId == null || a.AppointmentID != editingAppointmentId.Value)
                .Select(a => a.AppointmentDateTime)
                .ToListAsync();
            var bookedSet = booked.ToHashSet();

            // Build available slot list
            for (var t = start; t < end; t = t.Add(step))
            {
                if (!bookedSet.Contains(t))
                {
                    items.Add(new SelectListItem
                    {
                        Value = t.ToString("O"),     // ISO 8601 round-trip
                        Text = t.ToString("h:mm tt")
                    });
                }
            }
            return items;
        }

        // =========================
        // List / Index
        // =========================
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";

            if (searchString != null) pageNumber = 1;
            else searchString = currentFilter;
            ViewData["CurrentFilter"] = searchString;

            // Base query with joins so names show in the table
            var query = _context.Appointments
                .Include(a => a.Student)
                .Include(a => a.Doctor)
                .AsQueryable();

            // Ownership filter by role
            if (User.IsInRole("Student"))
            {
                var sid = await GetCurrentStudentIdAsync();
                query = query.Where(a => a.StudentID == sid);
            }
            else if (User.IsInRole("Caregiver"))
            {
                var cid = await GetCurrentCaregiverIdAsync();
                query = query.Where(a => a.Student.Caregivers.Any(c => c.CaregiverID == cid));
            }
            else if (User.IsInRole("Admin") || User.IsInRole("Doctor") || User.IsInRole("Teacher"))
            {
                // see all
            }
            else
            {
                return Forbid();
            }

            // Search by names, reason, or status
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

            // Paging
            const int pageSize = 5;
            int page = pageNumber ?? 1;

            var total = await query.CountAsync();
            var rows = await query.AsNoTracking()
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return View(new PaginatedList<Appointment>(rows, total, page, pageSize));
        }

        // =========================
        // Details
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // =========================
        // Create (GET)
        // =========================
        [Authorize]
        public async Task<IActionResult> Create()
        {
            await PopulateStudentSelectAsync();
            await PopulateDoctorSelectAsync();

            var dateOptions = Next30Weekdays();
            ViewBag.DateOptions = new SelectList(dateOptions, "Value", "Text");

            // Preload slots for the first allowed date (optional but nice)
            var firstAllowed = dateOptions.FirstOrDefault()?.Value;
            List<SelectListItem> slotItems = new();
            var defaultDoctorId = (string?)ViewBag.FirstDoctorId;
            if (!string.IsNullOrEmpty(defaultDoctorId) && firstAllowed != null)
            {
                var d = DateTime.Parse(firstAllowed).Date;
                slotItems = await BuildSlotsAsync(defaultDoctorId, d);
            }
            ViewBag.Slots = new SelectList(slotItems, "Value", "Text");

            return View(new Appointment { Status = AppointmentStatus.Pending });
        }

        // =========================
        // AJAX: Get slots for doctor + date
        // =========================
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSlots(string doctorId, string date)
        {
            if (string.IsNullOrWhiteSpace(doctorId) || string.IsNullOrWhiteSpace(date))
                return Json(Array.Empty<object>());

            if (!DateTime.TryParse(date, out var day))
                return Json(Array.Empty<object>());

            var items = await BuildSlotsAsync(doctorId, day.Date);
            var payload = items.Select(i => new { value = i.Value, text = i.Text }).ToList();
            return Json(payload);
        }

        // =========================
        // Create (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("StudentID,DoctorID,Reason,Status")] Appointment model, // Status is posted too
            string selectedSlot,                                         // ISO string from the slot dropdown
            string selectedDate)                                         // yyyy-MM-dd from date input (for reload)
        {
            // 1) Basic required checks for dropdowns and slot
            if (string.IsNullOrWhiteSpace(model.DoctorID))
                ModelState.AddModelError("DoctorID", "Please select a doctor.");

            if (string.IsNullOrWhiteSpace(model.StudentID))
                ModelState.AddModelError("StudentID", "Please select a student.");

            if (string.IsNullOrWhiteSpace(selectedSlot))
                ModelState.AddModelError("AppointmentDateTime", "Please pick a time slot.");

            // Parse the slot string into a DateTime (it was emitted with "O" format)
            DateTime slotTime = default;
            if (!string.IsNullOrWhiteSpace(selectedSlot))
            {
                if (!DateTime.TryParse(selectedSlot, null, System.Globalization.DateTimeStyles.RoundtripKind, out slotTime))
                    ModelState.AddModelError("AppointmentDateTime", "Invalid time slot format.");
            }

            // 2) If basic validation passed, verify the slot is still available
            if (!ModelState.IsValid)
            {
                var available = await BuildSlotsAsync(model.DoctorID, slotTime.Date);
                var validIsoValues = available.Select(i => i.Value).ToHashSet();

                if (!validIsoValues.Contains(selectedSlot))
                {
                    // Someone else booked it just now, or it’s not a working day/time.
                    ModelState.AddModelError("AppointmentDateTime", "That slot is no longer available. Please pick another.");
                }
                else
                {
                    // We’re good → save
                    model.AppointmentDateTime = slotTime;

                    // If you want newly created appointments to always start as Confirmed, do this:
                    // model.Status = AppointmentStatus.Confirmed;

                    _context.Appointments.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // 3) If we reach here, there were errors → rebuild dropdowns & slot list so the form can re-render

            await PopulateStudentSelectAsync(model.StudentID);
            await PopulateDoctorSelectAsync(model.DoctorID);

            var date = string.IsNullOrWhiteSpace(selectedDate) ? DateTime.Today : DateTime.Parse(selectedDate).Date;
            ViewBag.SelectedDate = date.ToString("yyyy-MM-dd");

            var slotItems = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(model.DoctorID))
                slotItems = await BuildSlotsAsync(model.DoctorID, date);
            ViewBag.Slots = new SelectList(slotItems, "Value", "Text", selectedSlot);

            return View(model);
        }

        // =========================
        // Edit (GET)
        // =========================
        [Authorize(Roles = "Admin,Doctor,Teacher,Student,Caregiver")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Load the row including navs so we can show names in the view
            var appt = await _context.Appointments
                .Include(a => a.Student)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentID == id);

            if (appt == null) return NotFound();

            // Build student dropdown (preselect current)
            ViewBag.StudentID = new SelectList(
                await _context.Students
                    .Select(s => new { s.StudentID, FullName = s.FirstName + " " + s.LastName })
                    .OrderBy(s => s.FullName)
                    .ToListAsync(),
                "StudentID", "FullName", appt.StudentID);

            // Build doctor dropdown (preselect current)
            ViewBag.DoctorID = new SelectList(
                await _context.Doctors
                    .Select(d => new { d.DoctorID, FullName = d.FirstName + " " + d.LastName })
                    .OrderBy(d => d.FullName)
                    .ToListAsync(),
                "DoctorID", "FullName", appt.DoctorID);

            // Pre-select the current date in yyyy-MM-dd so the <input type="date"> shows it
            var date = appt.AppointmentDateTime.Date;
            ViewBag.SelectedDate = date.ToString("yyyy-MM-dd");

            // Build available slots for that doctor & date, but allow the current slot even if "booked"
            var slots = await BuildSlotsAsync(appt.DoctorID, date, appt.AppointmentID);

            // Ensure the current selected time is present
            var currentIso = appt.AppointmentDateTime.ToString("O");
            if (!slots.Any(s => s.Value == currentIso))
            {
                slots.Add(new SelectListItem
                {
                    Value = currentIso,
                    Text = appt.AppointmentDateTime.ToString("h:mm tt")
                });
            }

            ViewBag.Slots = new SelectList(slots.OrderBy(s => s.Text), "Value", "Text", currentIso);
            return View(appt);
        }

        // =========================
        // Edit (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Doctor,Teacher,Student,Caregiver")]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("AppointmentID,StudentID,DoctorID,Status,Reason")] Appointment form,
            string? selectedDate,   // from <input type="date">, format: yyyy-MM-dd
            string? selectedSlot)   // from slot dropdown, format: ISO 8601 ("O")
        {
            if (id != form.AppointmentID) return NotFound();

            // Load the row we’re updating
            var appt = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentID == id);
            if (appt == null) return NotFound();

            // Update basic fields first (date/time comes after we validate slot)
            appt.StudentID = form.StudentID;
            appt.DoctorID = form.DoctorID;
            appt.Status = form.Status;
            appt.Reason = form.Reason;

            // ---- Parse date exactly as yyyy-MM-dd (what the HTML date input posts) ----
            // If user didn’t change date, use current appointment date to rebuild the form if needed
            DateTime date = appt.AppointmentDateTime.Date;
            if (!string.IsNullOrWhiteSpace(selectedDate))
            {
                if (!DateTime.TryParseExact(
                        selectedDate,
                        "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out date))
                {
                    ModelState.AddModelError("AppointmentDateTime", "Invalid date format.");
                }
            }

            // ---- Parse slot: it is emitted/stored with "O" (ISO 8601) ----
            DateTime newDateTime = appt.AppointmentDateTime;
            if (string.IsNullOrWhiteSpace(selectedSlot) ||
                !DateTime.TryParse(
                    selectedSlot,
                    null,
                    System.Globalization.DateTimeStyles.RoundtripKind,
                    out newDateTime))
            {
                ModelState.AddModelError("AppointmentDateTime", "Please choose a valid time slot.");
            }

            // If everything is valid so far, confirm the slot is still available for that doctor/date
            if (!ModelState.IsValid)
            {
                var available = await BuildSlotsAsync(appt.DoctorID, date, appt.AppointmentID);
                var availableIso = available.Select(s => s.Value).ToHashSet();
                var newIso = newDateTime.ToString("O");

                if (!availableIso.Contains(newIso))
                {
                    ModelState.AddModelError("AppointmentDateTime", "This time is no longer available. Please pick another slot.");
                }
                else
                {
                    // All good → save and exit
                    appt.AppointmentDateTime = newDateTime;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // If we’re here, there were validation errors → rebuild dropdowns & slots and redisplay the form
            ViewBag.StudentID = new SelectList(
                await _context.Students
                    .Select(s => new { s.StudentID, FullName = s.FirstName + " " + s.LastName })
                    .OrderBy(s => s.FullName)
                    .ToListAsync(),
                "StudentID", "FullName", appt.StudentID);

            ViewBag.DoctorID = new SelectList(
                await _context.Doctors
                    .Select(d => new { d.DoctorID, FullName = d.FirstName + " " + d.LastName })
                    .OrderBy(d => d.FullName)
                    .ToListAsync(),
                "DoctorID", "FullName", appt.DoctorID);

            ViewBag.SelectedDate = date.ToString("yyyy-MM-dd");

            var slotsAgain = await BuildSlotsAsync(appt.DoctorID, date, appt.AppointmentID);
            var currIso = appt.AppointmentDateTime.ToString("O");
            if (!slotsAgain.Any(s => s.Value == currIso))
            {
                slotsAgain.Add(new SelectListItem { Value = currIso, Text = appt.AppointmentDateTime.ToString("h:mm tt") });
            }
            ViewBag.Slots = new SelectList(slotsAgain.OrderBy(s => s.Text), "Value", "Text", selectedSlot ?? currIso);

            return View(appt);
        }




        // =========================
        // Delete
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null) _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private static List<SelectListItem> Next30Weekdays()
        {
            var items = new List<SelectListItem>();
            var today = DateTime.Today;
            var end = today.AddDays(30);
            for (var d = today; d <= end; d = d.AddDays(1))
            {
                if (d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
                items.Add(new SelectListItem
                {
                    Value = d.ToString("yyyy-MM-dd"),
                    Text = d.ToString("ddd, dd MMM yyyy")
                });
            }
            return items;
        }



    }
}
