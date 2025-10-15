using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AvondaleCollegeClinic.Controllers
{
    [Authorize] // you must be signed in for every action in this controller
    public class ProfileController : Controller
    {
        private readonly AvondaleCollegeClinicContext _db; // database connection for our app data
        private readonly UserManager<AvondaleCollegeClinicUser> _users; // helper to read the signed in identity user

        public ProfileController(AvondaleCollegeClinicContext db, UserManager<AvondaleCollegeClinicUser> users)
        {
            _db = db; // keep the db so we can use it later
            _users = users; // keep the user manager so we can look up the current user
        }

        // STUDENT → "My Student Record"
        [Authorize(Roles = "Student")] // only a student can view this page
        public async Task<IActionResult> Student()
        {
            var u = await _users.GetUserAsync(User); // get the identity user from the cookie

            var model = await _db.Students
                .Include(s => s.Homeroom).ThenInclude(h => h.Teacher) // also load the homeroom and the teacher
                .Include(s => s.Caregivers)                           // also load linked caregivers
                .AsNoTracking()                                       // read only query for speed
                .FirstOrDefaultAsync(s => s.IdentityUserId == u.Id || s.Email == u.Email); // match the student row for this user

            if (model == null) return NotFound(); // if not found show 404
            return View("~/Views/ProfileView/StudentProfile.cshtml", model); // send the model to the student profile view
        }

        // STUDENT → "My Caregiver"
        // CAREGIVER → "My Caregiver Record"
        [Authorize(Roles = "Student,Caregiver")] // both roles can use this action
        public async Task<IActionResult> Caregiver()
        {
            var u = await _users.GetUserAsync(User); // current identity user

            Caregiver? model; // will hold a single caregiver when the current user is a caregiver

            if (await _users.IsInRoleAsync(u, "Student"))
            {
                var student = await _db.Students
                    .Include(s => s.Caregivers)        // load the list of caregivers for this student
                    .AsNoTracking()                    // read only query
                    .FirstOrDefaultAsync(s => s.IdentityUserId == u.Id || s.Email == u.Email); // find the student for this user
                if (student == null) return NotFound(); // student not found

                // Show a list of their caregivers
                var caregivers = student.Caregivers?.ToList() ?? new List<Caregiver>(); // make a simple list for the view
                return View("~/Views/ProfileView/CaregiverList.cshtml", caregivers); // render the caregiver list view
            }
            else
            {
                model = await _db.Caregivers
                    .Include(c => c.Students)          // a caregiver can have many students
                    .AsNoTracking()                    // read only query
                    .FirstOrDefaultAsync(c => c.IdentityUserId == u.Id || c.Email == u.Email); // find the caregiver for this user
            }

            if (model == null) return NotFound(); // caregiver not found
            return View("~/Views/ProfileView/CaregiverProfile.cshtml", model); // render the caregiver profile page
        }

        // TEACHER → "My Teacher Record"
        [Authorize(Roles = "Teacher")] // only teachers can open this
        public async Task<IActionResult> Teacher()
        {
            var u = await _users.GetUserAsync(User); // current identity user

            var model = await _db.Teachers
                .Include(t => t.Homeroom)    // load the single homeroom for this teacher
                .AsNoTracking()              // read only query
                .FirstOrDefaultAsync(t => t.IdentityUserId == u.Id || t.Email == u.Email); // find the teacher row

            if (model == null) return NotFound(); // teacher not found
            return View("~/Views/ProfileView/TeacherProfile.cshtml", model); // show the teacher profile page
        }

        // DOCTOR → "My Doctor Record"
        [Authorize(Roles = "Doctor")] // only doctors can open this
        public async Task<IActionResult> Doctor()
        {
            var u = await _users.GetUserAsync(User); // current identity user

            var model = await _db.Doctors
                .AsNoTracking()                       // read only query
                .FirstOrDefaultAsync(d => d.IdentityUserId == u.Id || d.Email == u.Email); // find the doctor row

            if (model == null) return NotFound(); // doctor not found
            return View("~/Views/ProfileView/DoctorProfile.cshtml", model); // show the doctor profile page
        }

    }
}
