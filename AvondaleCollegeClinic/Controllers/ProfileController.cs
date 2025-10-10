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
    [Authorize] // must be signed in for any action below
    public class ProfileController : Controller
    {
        private readonly AvondaleCollegeClinicContext _db;
        private readonly UserManager<AvondaleCollegeClinicUser> _users;

        public ProfileController(AvondaleCollegeClinicContext db, UserManager<AvondaleCollegeClinicUser> users)
        {
            _db = db;
            _users = users;
        }

        // STUDENT → "My Student Record"
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Student()
        {
            var u = await _users.GetUserAsync(User);

            var model = await _db.Students
                .Include(s => s.Homeroom).ThenInclude(h => h.Teacher)
                .Include(s => s.Caregivers)           
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.IdentityUserId == u.Id || s.Email == u.Email);

            if (model == null) return NotFound();
            return View("~/Views/ProfileView/StudentProfile.cshtml", model);
        }

        // STUDENT → "My Caregiver"
        // CAREGIVER → "My Caregiver Record"
        [Authorize(Roles = "Student,Caregiver")]
        public async Task<IActionResult> Caregiver()
        {
            var u = await _users.GetUserAsync(User);

            Caregiver? model;

            if (await _users.IsInRoleAsync(u, "Student"))
            {
                var student = await _db.Students
                    .Include(s => s.Caregivers)        // ⬅️ CHANGED
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.IdentityUserId == u.Id || s.Email == u.Email);
                if (student == null) return NotFound();

                // Show a list of their caregivers
                var caregivers = student.Caregivers?.ToList() ?? new List<Caregiver>();
                return View("~/Views/ProfileView/CaregiverList.cshtml", caregivers);
            }
            else
            {
                model = await _db.Caregivers
                    .Include(c => c.Students)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.IdentityUserId == u.Id || c.Email == u.Email);
            }

            if (model == null) return NotFound();
            return View("~/Views/ProfileView/CaregiverProfile.cshtml", model);
        }

        // TEACHER → "My Teacher Record"
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Teacher()
        {
            var u = await _users.GetUserAsync(User);

            var model = await _db.Teachers
                .Include(t => t.Homeroom)    // <- single nav (1:1)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.IdentityUserId == u.Id || t.Email == u.Email);

            if (model == null) return NotFound();
            return View("~/Views/ProfileView/TeacherProfile.cshtml", model);
        }

        // DOCTOR → "My Doctor Record"
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Doctor()
        {
            var u = await _users.GetUserAsync(User);

            var model = await _db.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdentityUserId == u.Id || d.Email == u.Email);

            if (model == null) return NotFound();
            return View("~/Views/ProfileView/DoctorProfile.cshtml", model);
        }

    }
}
