using AvondaleCollegeClinic.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Areas.Identity.Pages.Account.Manage
{
    public class SetNewPasswordModel : PageModel
    {
        private readonly UserManager<AvondaleCollegeClinicUser> _users;
        private readonly SignInManager<AvondaleCollegeClinicUser> _signIn;
        private readonly ILogger<SetNewPasswordModel> _log;

        public SetNewPasswordModel(
            UserManager<AvondaleCollegeClinicUser> users,
            SignInManager<AvondaleCollegeClinicUser> signIn,
            ILogger<SetNewPasswordModel> log)
        {
            _users = users;
            _signIn = signIn;
            _log = log;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            [StringLength(100, MinimumLength = 8,
                ErrorMessage = "Password must be at least {2} characters.")]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();
            return Page(); // uses your site layout; no old-password anywhere
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();

            // We intentionally DO NOT ask for the old password.
            var token = await _users.GeneratePasswordResetTokenAsync(user);
            var result = await _users.ResetPasswordAsync(user, token, Input.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return Page();
            }

            // Clear “must change” flag if present
            if (user.MustSetPassword)
            {
                user.MustSetPassword = false;
                await _users.UpdateAsync(user);
            }

            // Refresh the auth cookie so no more redirects happen
            await _signIn.SignInAsync(user, isPersistent: false);
            _log.LogInformation("User set a new password without asking for the old password.");

            TempData["StatusMessage"] = "Your password has been updated.";
            return LocalRedirect("~/");
        }

    }
}
