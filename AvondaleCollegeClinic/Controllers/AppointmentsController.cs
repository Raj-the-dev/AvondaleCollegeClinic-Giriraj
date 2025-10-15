using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;               
using Microsoft.AspNetCore.Identity;                 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;                
using Microsoft.EntityFrameworkCore;                     // EF Core query extensions
using AvondaleCollegeClinic.Areas.Identity.Data;        // Identity user type
using AvondaleCollegeClinic.Helpers;                    // PaginatedList<T>
using AvondaleCollegeClinic.Models;                  

namespace AvondaleCollegeClinic.Controllers
{
    [Authorize] // you must be signed in for all actions here
    public class AppointmentsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;         // EF Core DbContext
        private readonly UserManager<AvondaleCollegeClinicUser> _users;  // Identity user service

        public AppointmentsController(
            AvondaleCollegeClinicContext context,
            UserManager<AvondaleCollegeClinicUser> users)
        {
            _context = context;
            _users = users;
        }

        // If the current user is a Student, return their StudentID; otherwise null.
        // We match in three ways to be flexible:
        // 1) FK link via IdentityUserId
        // 2) Username equals StudentID (from first-time setup)
        // 3) Email match
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

        // If the current user is a Caregiver, return their CaregiverID; otherwise null.
        // Same three matching strategies as above.
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

        // Build the Student dropdown with role-based filtering.
        // - Student role: only their own record and lock the dropdown in the view
        // - Caregiver role: only students they are linked to (see note: this version filters only for Student role)
        // - Others (Admin/Doctor/Teacher): all students
        // Also sets ViewBag.LockStudent so the view can disable the select and add a hidden field.
        private async Task PopulateStudentSelectAsync(string? selectedStudentId = null)
        {
            // Default query: all students, as "StudentID + FullName", sorted by name
            var studentsQuery = _context.Students
                .Select(s => new { s.StudentID, FullName = s.FirstName + " " + s.LastName })
                .OrderBy(s => s.FullName);

            bool lockStudent = false;

            // If the signed-in user is a Student, restrict to just themselves and lock the dropdown
            if (User.IsInRole("Student"))
            {
                var me = await _users.GetUserAsync(User);
                if (me != null)
                {
                    // We try to find the one Student row that matches this user
                    var myStudentId = await _context.Students
                        .Where(s => s.IdentityUserId == me.Id || s.Email == me.Email || s.StudentID == me.UserName)
                        .Select(s => s.StudentID)
                        .FirstOrDefaultAsync();

                    if (!string.IsNullOrWhiteSpace(myStudentId))
                    {
                        selectedStudentId ??= myStudentId;
                        lockStudent = true;

                        // Narrow list down to just "me"
                        studentsQuery = _context.Students
                            .Where(s => s.StudentID == myStudentId)
                            .Select(s => new { s.StudentID, FullName = s.FirstName + " " + s.LastName })
                            .OrderBy(s => s.FullName);
                    }
                }
            }

            // Execute the query and load the items
            var students = await studentsQuery.ToListAsync();

            // If nothing is selected yet and we have rows, pick the first as default
            selectedStudentId ??= students.FirstOrDefault()?.StudentID;

            // Pass to the view as a SelectList and a boolean flag for locking
            ViewBag.StudentID = new SelectList(students, "StudentID", "FullName", selectedStudentId);
            ViewBag.LockStudent = lockStudent; // view uses this to render a disabled select + hidden input
        }

        // Build the Doctor dropdown and store the first doctor's ID for initial slot loading in Create()
        private async Task PopulateDoctorSelectAsync(string? selectedDoctorId = null)
        {
            var doctors = await _context.Doctors
                .Select(d => new { d.DoctorID, FullName = d.FirstName + " " + d.LastName })
                .OrderBy(d => d.FullName)
                .ToListAsync();

            // Pick the first doctor if none selected yet (helps with initial slot preload)
            selectedDoctorId ??= doctors.FirstOrDefault()?.DoctorID;

            ViewBag.DoctorID = new SelectList(doctors, "DoctorID", "FullName", selectedDoctorId);
            ViewBag.FirstDoctorId = selectedDoctorId; // used by Create() to preload slots
        }


        // Slot generator Build available time slots for a doctor on a given date.Returns list items where:
        // - Value is an ISO 8601 "round-trip" string (ToString("O")) for precise server parsing later
        // - Text is a friendly "h:mm tt" time like "9:00 AM"
        // If editingAppointmentId is supplied, we do NOT treat that appointment's current time as "booked"
        // so the user can keep their existing time when editing.
        private async Task<List<SelectListItem>> BuildSlotsAsync(string doctorId, DateTime date, int? editingAppointmentId = null)
        {
            var items = new List<SelectListItem>();

            // Load the doctor so we can read working days, hours, and slot size
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorID == doctorId);
            if (doctor == null) return items;

            // Helper function that checks the doctor's weekly availability flags for a given day-of-week
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

            // If the doctor does not work that day, there are no slots
            if (!Works(date.DayOfWeek)) return items;

            // Build concrete start and end DateTimes using this specific date + doctor's daily window
            var start = date.Date.Add(doctor.DailyStartTime);
            var end = date.Date.Add(doctor.DailyEndTime);

            // Pick the slot step in minutes. Fall back to 30 if misconfigured (0 or negative).
            var step = TimeSpan.FromMinutes(doctor.SlotMinutes <= 0 ? 30 : doctor.SlotMinutes);

            // Query all booked appointments for this doctor on that date.
            // If we are editing, exclude the current appointment so we don't block its own time.
            var booked = await _context.Appointments
                .Where(a => a.DoctorID == doctorId && a.AppointmentDateTime.Date == date.Date)
                .Where(a => editingAppointmentId == null || a.AppointmentID != editingAppointmentId.Value)
                .Select(a => a.AppointmentDateTime)
                .ToListAsync();

            // Use a HashSet for O(1) lookups during the loop below
            var bookedSet = booked.ToHashSet();

            // Walk through the day in fixed steps and add any time that is not already booked
            for (var t = start; t < end; t = t.Add(step))
            {
                if (!bookedSet.Contains(t))
                {
                    items.Add(new SelectListItem
                    {
                        Value = t.ToString("O"),     // exact round-trip value for form POST
                        Text = t.ToString("h:mm tt") // nice display text
                    });
                }
            }
            return items;
        }


        // Index (list with search + sort + pagination)

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            // Track current sort mode for the view
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";

            // Reset to page 1 if the user typed a new search term
            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            // Base query includes Student and Doctor for display
            var appointments = _context.Appointments
                .Include(a => a.Student)
                .Include(a => a.Doctor)
                .AsQueryable();

            // Search
            // Try to interpret the search string as an AppointmentStatus
            AppointmentStatus? statusFilter = null;
            if (!string.IsNullOrWhiteSpace(searchString) &&
                Enum.TryParse<AppointmentStatus>(searchString.Trim(), true, out var st))
                statusFilter = st;

            // Text search over student name, doctor name, and reason
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = searchString.Trim();
                appointments = appointments.Where(a =>
                    a.Student.FirstName.Contains(term) ||
                    a.Student.LastName.Contains(term) ||
                    a.Doctor.FirstName.Contains(term) ||
                    a.Doctor.LastName.Contains(term) ||
                    a.Reason.Contains(term));
            }

            // Apply status filter if present
            if (statusFilter.HasValue)
                appointments = appointments.Where(a => a.Status == statusFilter.Value);

            // Sort
            switch (sortOrder)
            {
                case "date_desc":
                    appointments = appointments.OrderByDescending(a => a.AppointmentDateTime);
                    break;
                case "Status":
                    appointments = appointments.OrderBy(a => a.Status);
                    break;
                case "status_desc":
                    appointments = appointments.OrderByDescending(a => a.Status);
                    break;
                default:
                    appointments = appointments.OrderBy(a => a.AppointmentDateTime);
                    break;
            }

            // Pagination: fixed 5 rows per page using helper PaginatedList<T>
            int pageSize = 5;
            return View(await PaginatedList<Appointment>.CreateAsync(
                appointments.AsNoTracking(), pageNumber ?? 1, pageSize));
        }



        // Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Load a single appointment with related Student and Doctor for display fields
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // Create (GET)
        [Authorize]
        public async Task<IActionResult> Create()
        {
            // Build dropdowns
            await PopulateStudentSelectAsync();
            await PopulateDoctorSelectAsync();

            // Build date options for the next 30 week days only 
            var dateOptions = Next30Weekdays();
            ViewBag.DateOptions = new SelectList(dateOptions, "Value", "Text");

            // Preload slots for the first doctor and first allowed date
            var firstAllowed = dateOptions.FirstOrDefault()?.Value;
            List<SelectListItem> slotItems = new();
            var defaultDoctorId = (string?)ViewBag.FirstDoctorId;
            if (!string.IsNullOrEmpty(defaultDoctorId) && firstAllowed != null)
            {
                var d = DateTime.Parse(firstAllowed).Date;
                slotItems = await BuildSlotsAsync(defaultDoctorId, d);
            }
            ViewBag.Slots = new SelectList(slotItems, "Value", "Text");

            // Default status = Confirmed for convenience
            return View(new Appointment { Status = AppointmentStatus.Confirmed });
        }

        // AJAX: Get slots for doctor + date
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSlots(string doctorId, string date)
        {
            // Basic input check
            if (string.IsNullOrWhiteSpace(doctorId) || string.IsNullOrWhiteSpace(date))
                return Json(Array.Empty<object>());

            // Parse date safely. If fail, return empty list
            if (!DateTime.TryParse(date, out var day))
                return Json(Array.Empty<object>());

            // Build and return a simple JSON array { value, text } for the client
            var items = await BuildSlotsAsync(doctorId, day.Date);
            var payload = items.Select(i => new { value = i.Value, text = i.Text }).ToList();
            return Json(payload);
        }

        // Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken] // prevents CSRF attacks on form submission
        public async Task<IActionResult> Create(
            [Bind("StudentID,DoctorID,Reason,Status")] Appointment model, // Status is posted too
            string selectedSlot,                                         // ISO string from the slot dropdown
            string selectedDate)                                         // yyyy-MM-dd from date input (for reload)
        {
            // If the user is a Student, enforce their own StudentID and ignore any posted value
            if (User.IsInRole("Student"))
            {
                var mySid = await GetCurrentStudentIdAsync();
                if (!string.IsNullOrWhiteSpace(mySid))
                    model.StudentID = mySid;          // ignore any posted StudentID
            }

            // Only Admin / Doctor / Teacher may decide status. Others force Confirmed.
            bool canEditStatus = User.IsInRole("Admin") || User.IsInRole("Doctor") || User.IsInRole("Teacher");
            if (!canEditStatus)
            {
                model.Status = AppointmentStatus.Confirmed; // lock status for others (incl. Caregivers)
            }

            // 1) Basic required checks for dropdowns and slot
            if (string.IsNullOrWhiteSpace(model.DoctorID))
                ModelState.AddModelError("DoctorID", "Please select a doctor.");

            if (string.IsNullOrWhiteSpace(model.StudentID))
                ModelState.AddModelError("StudentID", "Please select a student.");

            if (string.IsNullOrWhiteSpace(selectedSlot))
                ModelState.AddModelError("AppointmentDateTime", "Please pick a time slot.");

            // Parse the slot string into a DateTime (round-trip format "O")
            DateTime slotTime = default;
            if (!string.IsNullOrWhiteSpace(selectedSlot))
            {
                if (!DateTime.TryParse(selectedSlot, null, System.Globalization.DateTimeStyles.RoundtripKind, out slotTime))
                    ModelState.AddModelError("AppointmentDateTime", "Invalid time slot format.");
            }

            // 2) Availability check and save on success.
            // Note: this project pattern saves when ModelState is NOT valid after we re-check availability.
            if (!ModelState.IsValid)
            {
                // Build list of valid ISO strings for the target day (prevents race conditions)
                var available = await BuildSlotsAsync(model.DoctorID, slotTime.Date);
                var validIsoValues = available.Select(i => i.Value).ToHashSet();

                if (!validIsoValues.Contains(selectedSlot))
                {
                    // Slot got taken between page load and submit
                    ModelState.AddModelError("AppointmentDateTime", "That slot is no longer available. Please pick another.");
                }
                else
                {
                    // Slot still valid -> save and redirect
                    model.AppointmentDateTime = slotTime;
                    _context.Appointments.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // 3) If there were errors, rebuild dropdowns and slots so the form can re-render with context
            await PopulateStudentSelectAsync(model.StudentID);
            await PopulateDoctorSelectAsync(model.DoctorID);

            // Keep the user’s chosen date in the date picker
            var date = string.IsNullOrWhiteSpace(selectedDate) ? DateTime.Today : DateTime.Parse(selectedDate).Date;
            ViewBag.SelectedDate = date.ToString("yyyy-MM-dd");

            // Rebuild the slots for the selected doctor/date so the user can try again
            var slotItems = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(model.DoctorID))
                slotItems = await BuildSlotsAsync(model.DoctorID, date);
            ViewBag.Slots = new SelectList(slotItems, "Value", "Text", selectedSlot);

            return View(model);
        }


        // Edit (GET)
        [Authorize(Roles = "Admin,Doctor,Teacher,Student,Caregiver")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Load the appointment with related names for display
            var appt = await _context.Appointments
                .Include(a => a.Student)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentID == id);

            if (appt == null) return NotFound();

            // Student dropdown preselected to current
            await PopulateStudentSelectAsync(appt.StudentID);

            // Doctor dropdown preselected to current
            ViewBag.DoctorID = new SelectList(
                await _context.Doctors
                    .Select(d => new { d.DoctorID, FullName = d.FirstName + " " + d.LastName })
                    .OrderBy(d => d.FullName)
                    .ToListAsync(),
                "DoctorID", "FullName", appt.DoctorID);

            // Choose the currently booked date
            var chosenDate = appt.AppointmentDateTime.Date;

            // Provide date options as weekdays including the chosen date
            var dateOptions = Next30WeekdaysIncluding(chosenDate);
            ViewBag.DateOptions = new SelectList(dateOptions, "Value", "Text", chosenDate.ToString("yyyy-MM-dd"));

            // Build available slots for that doctor and date.
            // We pass the appointment ID so the current time is not considered "booked".
            var slots = await BuildSlotsAsync(appt.DoctorID, chosenDate, appt.AppointmentID);

            // Ensure current time appears in the list even if fully booked otherwise
            var currentIso = appt.AppointmentDateTime.ToString("O");
            if (!slots.Any(s => s.Value == currentIso))
            {
                slots.Add(new SelectListItem
                {
                    Value = currentIso,
                    Text = appt.AppointmentDateTime.ToString("h:mm tt")
                });
            }

            // Sort by time text for a friendly order and preselect current slot
            ViewBag.Slots = new SelectList(slots.OrderBy(s => s.Text), "Value", "Text", currentIso);
            return View(appt);
        }


        // Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Doctor,Teacher,Student,Caregiver")]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("AppointmentID,StudentID,DoctorID,Status,Reason")] Appointment form,
            string? selectedDate,   // yyyy-MM-dd
            string? selectedSlot)   // ISO 8601 ("O")
        {
            if (id != form.AppointmentID) return NotFound();

            // Load the tracked entity to update
            var appt = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentID == id);
            if (appt == null) return NotFound();

            // If Student role, force the StudentID to their own ID
            if (User.IsInRole("Student"))
            {
                var mySid = await GetCurrentStudentIdAsync();
                if (!string.IsNullOrWhiteSpace(mySid))
                    form.StudentID = mySid;
            }

            // Always allow these to change
            appt.StudentID = form.StudentID;
            appt.DoctorID = form.DoctorID;
            appt.Reason = form.Reason;

            // Only Admin/Doctor/Teacher can change Status. Others force Confirmed.
            bool canEditStatus = User.IsInRole("Admin") || User.IsInRole("Doctor") || User.IsInRole("Teacher");
            appt.Status = canEditStatus ? form.Status : AppointmentStatus.Confirmed;

            // Parse inputs safely Start with existing time so we always have a value
            DateTime newDateTime = appt.AppointmentDateTime;
            bool slotParsed = false;

            // Parse ISO slot into DateTime with round-trip settings
            if (!string.IsNullOrWhiteSpace(selectedSlot) &&
                DateTime.TryParse(selectedSlot, null,
                    System.Globalization.DateTimeStyles.RoundtripKind, out var parsedSlotDt))
            {
                newDateTime = parsedSlotDt;
                slotParsed = true;
            }
            else
            {
                ModelState.AddModelError("AppointmentDateTime", "Please choose a valid time slot.");
            }

            // Default chosenDate to the parsed slot date; override if a valid yyyy-MM-dd was posted
            DateTime chosenDate = newDateTime.Date;
            if (!string.IsNullOrWhiteSpace(selectedDate) &&
                DateTime.TryParseExact(selectedDate, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var parsedDate))
            {
                chosenDate = parsedDate.Date;
            }

            //  Availability check and save (same inverted pattern: save when !ModelState.IsValid) 
            if (!ModelState.IsValid && slotParsed)
            {
                var available = await BuildSlotsAsync(appt.DoctorID, chosenDate, appt.AppointmentID);
                var availableIso = available.Select(s => s.Value).ToHashSet();
                var newIso = newDateTime.ToString("O");

                if (!availableIso.Contains(newIso))
                {
                    // Someone else booked it in the meantime
                    ModelState.AddModelError("AppointmentDateTime", "This time is no longer available. Please pick another slot.");
                }
                else
                {
                    // Still free -> save and go back to list
                    appt.AppointmentDateTime = newDateTime;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            //  Rebuild dropdowns & lists when there are validation errors

            // Student dropdown
            await PopulateStudentSelectAsync(appt.StudentID);

            // Doctor dropdown
            ViewBag.DoctorID = new SelectList(
                await _context.Doctors
                    .Select(d => new { d.DoctorID, FullName = d.FirstName + " " + d.LastName })
                    .OrderBy(d => d.FullName)
                    .ToListAsync(),
                "DoctorID", "FullName", appt.DoctorID);

            // Date options include the chosen date
            var dateOptions = Next30WeekdaysIncluding(chosenDate);
            ViewBag.DateOptions = new SelectList(dateOptions, "Value", "Text", chosenDate.ToString("yyyy-MM-dd"));

            // Build slots again and ensure the current selection stays visible
            var slotsAgain = await BuildSlotsAsync(appt.DoctorID, chosenDate, appt.AppointmentID);
            var keepIso = slotParsed ? newDateTime.ToString("O") : appt.AppointmentDateTime.ToString("O");
            if (!slotsAgain.Any(s => s.Value == keepIso))
                slotsAgain.Add(new SelectListItem { Value = keepIso, Text = DateTime.Parse(keepIso).ToString("h:mm tt") });

            ViewBag.Slots = new SelectList(
                slotsAgain.OrderBy(s => DateTime.Parse(s.Value)), "Value", "Text", keepIso);

            return View(appt);
        }


        // Delete (GET)

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

        // Actually remove the appointment after the user confirms
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null) _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Build a list of weekday dates from today to 30 days ahead.
        // Each item has Value = yyyy-MM-dd and Text = "Mon, 01 Jan 2025".
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

        // Build a weekday list starting from a specific date (inclusive) up to 30 days from today.
        // Useful for Edit where we want to include the already chosen date even if it is not today.
        private static List<SelectListItem> Next30WeekdaysIncluding(DateTime startInclusive)
        {
            var items = new List<SelectListItem>();
            var end = DateTime.Today.AddDays(30);
            for (var d = startInclusive.Date; d <= end; d = d.AddDays(1))
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
