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
        // Identity services to create users and sign them in
        private readonly UserManager<AvondaleCollegeClinicUser> _users;
        private readonly SignInManager<AvondaleCollegeClinicUser> _signIn;

        // App database so we can look up Student/Teacher/Doctor/Caregiver by ID
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

        // This holds the form fields. Razor binds posted values into it.
        [BindProperty] public InputModel Input { get; set; } = new();

        public class InputModel
        {
            // The school-issued ID. We accept it for any role.
            [Required, Display(Name = "Your Student/Teacher/Doctor/Caregiver ID")]
            public string Identifier { get; set; } = string.Empty;

            // Password rules. At least 8 characters.
            [Required, DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 8,
                ErrorMessage = "Password must be at least 8 chars with upper, lower and a number.")]
            public string Password { get; set; } = string.Empty;

            // Must match Password. Razor will show a message if it does not.
            [DataType(DataType.Password), Compare(nameof(Password))]
            public string ConfirmPassword { get; set; } = string.Empty;

            // Simple keyword we store with the user for extra checks later.
            [Required, MaxLength(40)]
            [Display(Name = "City you were born in (keyword)")]
            public string CityOfBirth { get; set; } = string.Empty;
        }

        // GET just shows the page. No work needed.
        public void OnGet() { }

        // POST handles form submit. We create the Identity user here.
        public async Task<IActionResult> OnPostAsync()
        {
            // If data annotations failed, redisplay with errors.
            if (!ModelState.IsValid) return Page();

            // 1) Figure out which role this ID belongs to (Student/Teacher/Doctor/Caregiver).
            var role = await DetermineRoleAsync(Input.Identifier);
            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "We can’t find that ID in the system.");
                return Page();
            }

            // 2) Pull profile info from the domain table to prefill the Identity user.
            // We do not show these on the form. It keeps the flow simple and consistent.
            (string first, string last, string email, string avatar) = await GetProfileInfo(Input.Identifier);

            // Stop if the email is already tied to an Identity account.
            // This avoids duplicate registrations for the same person.
            var existingUser = await _users.FindByEmailAsync(email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "This ID/email is already registered. Use normal Login.");
                return Page();
            }

            // 3) Create the Identity user with what we know.
            var user = new AvondaleCollegeClinicUser
            {
                UserName = Input.Identifier,     // sign in with the school ID
                Email = email,
                FirstName = first,
                LastName = last,
                CityOfBirth = Input.CityOfBirth, // store the keyword
                EmailConfirmed = true,           // we trust mail from school records
                AvatarPath = avatar              // copy their profile photo path if present
            };

            // Create the user with a hashed password. Identity enforces password rules.
            var result = await _users.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                // Show all Identity errors back to the form.
                foreach (var e in result.Errors) ModelState.AddModelError(string.Empty, e.Description);
                return Page();
            }

            // Add the correct role so authorization works across the app.
            await _users.AddToRoleAsync(user, role);

            // Sign them in so they land in the app right away.
            await _signIn.SignInAsync(user, isPersistent: false);

            // Go to the home page (change if you prefer a different landing page).
            return LocalRedirect("~/");
        }

        // Looks up name, email, and avatar path from the matching domain table.
        private async Task<(string first, string last, string email, string avatar)> GetProfileInfo(string id)
        {
            // Try Student by StudentID
            var s = await _db.Students.FirstOrDefaultAsync(x => x.StudentID == id);
            if (s != null) return (s.FirstName, s.LastName, s.Email, s.ImagePath ?? "");

            // Try Teacher by TeacherID
            var t = await _db.Teachers.FirstOrDefaultAsync(x => x.TeacherID == id);
            if (t != null) return (t.FirstName, t.LastName, t.Email, t.ImagePath ?? "");

            // Try Doctor by DoctorID
            var d = await _db.Doctors.FirstOrDefaultAsync(x => x.DoctorID == id);
            if (d != null) return (d.FirstName, d.LastName, d.Email, d.ImagePath ?? "");

            // Try Caregiver by CaregiverID
            // Note: CaregiverID is a string in your model. ToString() is harmless but not needed.
            var c = await _db.Caregivers.FirstOrDefaultAsync(x => x.CaregiverID.ToString() == id);
            if (c != null) return (c.FirstName, c.LastName, c.Email, c.ImagePath ?? "");

            // If no match, return blanks so caller can handle it.
            return ("", "", "", "");
        }

        // Checks which table contains the ID and returns the role name.
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
