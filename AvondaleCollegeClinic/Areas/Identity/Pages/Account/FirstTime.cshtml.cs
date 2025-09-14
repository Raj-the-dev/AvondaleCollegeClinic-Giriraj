using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models; // to check IDs exist
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AvondaleCollegeClinic.Areas.Identity.Pages.Account
{
    public class FirstTimeModel : PageModel
    {
        private readonly UserManager<AvondaleCollegeClinicUser> _users;
        private readonly SignInManager<AvondaleCollegeClinicUser> _signIn;
        private readonly AvondaleCollegeClinicContext _db;

        public FirstTimeModel(
            UserManager<AvondaleCollegeClinicUser> users,
            SignInManager<AvondaleCollegeClinicUser> signIn,
            AvondaleCollegeClinicContext db)
        {
            _users = users;
            _signIn = signIn;
            _db = db;
        }

        [BindProperty] public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required, Display(Name = "Your Student/Teacher/Doctor/Caregiver ID")]
            public string Identifier { get; set; } = string.Empty;

            [Required, DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 8,
                ErrorMessage = "Password must be at least 8 chars with upper, lower and a number.")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password), Compare(nameof(Password))]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required, MaxLength(40)]
            [Display(Name = "City you were born in (keyword)")]
            public string CityOfBirth { get; set; } = string.Empty;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Step 1: Find domain user by ID
            var role = await DetermineRoleAsync(Input.Identifier);
            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "We can’t find that ID in the system.");
                return Page();
            }

            // Step 2: Pull name/email from DB (not shown on form, just used internally)
            (string first, string last, string email, string avatar) = await GetProfileInfo(Input.Identifier);

            // If email is already taken by another IdentityUser
            var existingUser = await _users.FindByEmailAsync(email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "This ID/email is already registered. Use normal Login.");
                return Page();
            }

            // Step 3: Create Identity user
            var user = new AvondaleCollegeClinicUser
            {
                UserName = Input.Identifier, // login by ID
                Email = email,
                FirstName = first,
                LastName = last,
                CityOfBirth = Input.CityOfBirth,
                EmailConfirmed = true,
                AvatarPath = avatar
            };

            var result = await _users.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError(string.Empty, e.Description);
                return Page();
            }

            await _users.AddToRoleAsync(user, role);
            await _signIn.SignInAsync(user, isPersistent: false);

            return LocalRedirect("~/");
        }

        private async Task<(string first, string last, string email, string avatar)> GetProfileInfo(string id)
        {
            var s = await _db.Students.FirstOrDefaultAsync(x => x.StudentID == id);
            if (s != null) return (s.FirstName, s.LastName, s.Email, s.ImagePath ?? "");

            var t = await _db.Teachers.FirstOrDefaultAsync(x => x.TeacherID == id);
            if (t != null) return (t.FirstName, t.LastName, t.Email, t.ImagePath ?? "");

            var d = await _db.Doctors.FirstOrDefaultAsync(x => x.DoctorID == id);
            if (d != null) return (d.FirstName, d.LastName, d.Email, d.ImagePath ?? "");

            var c = await _db.Caregivers.FirstOrDefaultAsync(x => x.CaregiverID.ToString() == id);
            if (c != null) return (c.FirstName, c.LastName, c.Email, c.ImagePath ?? "");

            return ("", "", "", "");
        }


        private async Task<string?> DetermineRoleAsync(string id)
        {
            id = id.Trim();

            if (await _db.Students.AsNoTracking().AnyAsync(s => s.StudentID == id)) return "Student";
            if (await _db.Teachers.AsNoTracking().AnyAsync(t => t.TeacherID == id)) return "Teacher";
            if (await _db.Doctors.AsNoTracking().AnyAsync(d => d.DoctorID == id)) return "Doctor";
            if (await _db.Caregivers.AsNoTracking().AnyAsync(c => c.CaregiverID == id)) return "Caregiver";

            return null;
        }
    }
}