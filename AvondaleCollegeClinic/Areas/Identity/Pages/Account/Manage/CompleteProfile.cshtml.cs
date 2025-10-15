using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace AvondaleCollegeClinic.Areas.Identity.Pages.Account.Manage
{
    // Only signed-in users can open this page
    [Authorize]
    public class CompleteProfileModel : PageModel
    {
        // Identity service to read and update the current user
        private readonly UserManager<AvondaleCollegeClinicUser> _userManager;

        // App DbContext to look up linked domain models (Student, Teacher, etc.)
        private readonly AvondaleCollegeClinicContext _db;

        // Constructor. ASP.NET Core gives us these services
        public CompleteProfileModel(UserManager<AvondaleCollegeClinicUser> userManager, AvondaleCollegeClinicContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        // If the user does not have a password yet, we will show password fields
        public bool NeedsPassword { get; set; }

        // If the linked model (Student/Teacher/etc.) has an image, we expose its path
        public string? ModelImagePath { get; set; }

        // This object receives form inputs from the page
        [BindProperty]
        public InputModel Input { get; set; } = new();

        // Form fields with simple validation rules
        public class InputModel
        {
            [Required, MaxLength(50)] public string FirstName { get; set; } = "";
            [Required, MaxLength(50)] public string LastName { get; set; } = "";
            [MaxLength(50)] public string? CityOfBirth { get; set; }

            // New password fields. Optional unless the account has no password yet
            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6)]
            public string? NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Compare(nameof(NewPassword))]
            public string? ConfirmPassword { get; set; }

            // If true, copy the profile photo from the linked model onto the user account
            public bool UseModelImage { get; set; } = true;
        }

        // GET: load current values and show the form
        public async Task<IActionResult> OnGetAsync()
        {
            // Find the signed-in user from the Identity system
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Decide if we must ask for a password on this screen
            NeedsPassword = !await _userManager.HasPasswordAsync(user);

            // Pre-fill text boxes with current values
            Input.FirstName = user.FirstName ?? "";
            Input.LastName = user.LastName ?? "";
            Input.CityOfBirth = user.CityOfBirth;

            // Try to fetch an existing image path from the linked domain model
            ModelImagePath = await GetLinkedModelImage(user);

            // Render the page
            return Page();
        }

        // POST: save changes after the form is submitted
        public async Task<IActionResult> OnPostAsync()
        {
            // Get the current user again (never trust client posted IDs)
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Recompute these in case the page was open for a while
            NeedsPassword = !await _userManager.HasPasswordAsync(user);
            ModelImagePath = await GetLinkedModelImage(user);

            // If form validation failed, show errors and keep user input
            if (!ModelState.IsValid) return Page();

            // Copy safe editable fields from Input into the user record
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.CityOfBirth = Input.CityOfBirth;

            // If this account has no password yet, we require one now
            if (NeedsPassword)
            {
                // Block empty password and show a friendly error
                if (string.IsNullOrWhiteSpace(Input.NewPassword))
                {
                    ModelState.AddModelError(string.Empty, "Password is required.");
                    return Page();
                }

                // Let Identity add a hashed password using its built-in rules
                var addPass = await _userManager.AddPasswordAsync(user, Input.NewPassword!);
                if (!addPass.Succeeded)
                {
                    // Show each Identity error message back to the user
                    foreach (var e in addPass.Errors) ModelState.AddModelError(string.Empty, e.Description);
                    return Page();
                }

                // We are done forcing a password set on first login
                user.MustSetPassword = false;
            }

            // If user asked to use the model image and we found one, copy path onto the user
            if (Input.UseModelImage && !string.IsNullOrEmpty(ModelImagePath))
            {
                user.AvatarPath = ModelImagePath;
            }

            // Save all changes to the user
            await _userManager.UpdateAsync(user);

            // Go back to the Manage index page
            return RedirectToPage("/Index");
        }

        // Helper. Look up the linked domain entity by Identity user id and return its stored image path
        private async Task<string?> GetLinkedModelImage(AvondaleCollegeClinicUser user)
        {
            switch (user.UserKind)
            {
                case UserKind.Student:
                    // AsNoTracking for read-only speed. Match on the FK IdentityUserId
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
                    // If we do not have a linked model type, fall back to the user’s own avatar
                    return user.AvatarPath;
            }
        }
    }
}
