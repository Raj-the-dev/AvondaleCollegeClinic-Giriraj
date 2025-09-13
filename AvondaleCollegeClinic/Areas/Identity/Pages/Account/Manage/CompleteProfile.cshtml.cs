using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AvondaleCollegeClinic.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class CompleteProfileModel : PageModel
    {
        private readonly UserManager<AvondaleCollegeClinicUser> _userManager;
        private readonly AvondaleCollegeClinicContext _db;

        public CompleteProfileModel(UserManager<AvondaleCollegeClinicUser> userManager, AvondaleCollegeClinicContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public bool NeedsPassword { get; set; }
        public string? ModelImagePath { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required, MaxLength(50)] public string FirstName { get; set; } = "";
            [Required, MaxLength(50)] public string LastName { get; set; } = "";
            [MaxLength(50)] public string? CityOfBirth { get; set; }

            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6)]
            public string? NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Compare(nameof(NewPassword))]
            public string? ConfirmPassword { get; set; }

            public bool UseModelImage { get; set; } = true;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            NeedsPassword = !await _userManager.HasPasswordAsync(user);

            // Pre-fill fields
            Input.FirstName = user.FirstName ?? "";
            Input.LastName = user.LastName ?? "";
            Input.CityOfBirth = user.CityOfBirth;

            // Expose model photo if available
            ModelImagePath = await GetLinkedModelImage(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            NeedsPassword = !await _userManager.HasPasswordAsync(user);
            ModelImagePath = await GetLinkedModelImage(user);

            if (!ModelState.IsValid) return Page();

            // Update profile info
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.CityOfBirth = Input.CityOfBirth;

            if (NeedsPassword)
            {
                if (string.IsNullOrWhiteSpace(Input.NewPassword))
                {
                    ModelState.AddModelError(string.Empty, "Password is required.");
                    return Page();
                }

                var addPass = await _userManager.AddPasswordAsync(user, Input.NewPassword!);
                if (!addPass.Succeeded)
                {
                    foreach (var e in addPass.Errors) ModelState.AddModelError(string.Empty, e.Description);
                    return Page();
                }

                user.MustSetPassword = false;
            }

            if (Input.UseModelImage && !string.IsNullOrEmpty(ModelImagePath))
            {
                user.AvatarPath = ModelImagePath;
            }

            await _userManager.UpdateAsync(user);
            return RedirectToPage("/Index"); // Manage home or wherever you like
        }

        private async Task<string?> GetLinkedModelImage(AvondaleCollegeClinicUser user)
        {
            switch (user.UserKind)
            {
                case UserKind.Student:
                    var s = await _db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.IdentityUserId == user.Id);
                    return s?.ImagePath;
                case UserKind.Teacher:
                    var t = await _db.Teachers.AsNoTracking().FirstOrDefaultAsync(x => x.IdentityUserId == user.Id);
                    return t?.ImagePath;
                case UserKind.Doctor:
                    var d = await _db.Doctors.AsNoTracking().FirstOrDefaultAsync(x => x.IdentityUserId == user.Id);
                    return d?.ImagePath;
                case UserKind.Caregiver:
                    var c = await _db.Caregivers.AsNoTracking().FirstOrDefaultAsync(x => x.IdentityUserId == user.Id);
                    return c?.ImagePath;
                default:
                    return user.AvatarPath;
            }
        }
    }
}
