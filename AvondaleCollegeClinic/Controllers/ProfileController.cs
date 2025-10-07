using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;

namespace AvondaleCollegeClinic.Controllers
{
    [Authorize] // all users must be logged in
    public class ProfileController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;
        private readonly UserManager<AvondaleCollegeClinicUser> _userManager;

        public ProfileController(AvondaleCollegeClinicContext context, UserManager<AvondaleCollegeClinicUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Student
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Student()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var student = await _context.Students
                .Include(s => s.Homeroom).ThenInclude(h => h.Teacher)
                .Include(s => s.Caregiver)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.IdentityUserId == user.Id || s.Email == user.Email);

            if (student == null)
                return NotFound("Student record not found.");

            return View("~/Views/ProfileView/StudentProfile.cshtml", student);
        }

        // Caregiver
        [Authorize(Roles = "Caregiver")]
        public async Task<IActionResult> Caregiver()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var caregiver = await _context.Caregivers
                .Include(c => c.Students)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdentityUserId == user.Id || c.Email == user.Email);

            if (caregiver == null)
                return NotFound("Caregiver record not found.");

            return View("~/Views/ProfileView/CaregiverProfile.cshtml", caregiver);
        }

        // Teacher
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Teacher()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var teacher = await _context.Teachers
                .Include(t => t.Homerooms)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.IdentityUserId == user.Id || t.Email == user.Email);

            if (teacher == null)
                return NotFound("Teacher record not found.");

            return View("~/Views/ProfileView/TeacherProfile.cshtml", teacher);
        }

        // Doctor
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Doctor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var doctor = await _context.Doctors
                .Include(d => d.Availabilities)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdentityUserId == user.Id || d.Email == user.Email);

            if (doctor == null)
                return NotFound("Doctor record not found.");

            return View("~/Views/ProfileView/DoctorProfile.cshtml", doctor);
        }
    }
}
