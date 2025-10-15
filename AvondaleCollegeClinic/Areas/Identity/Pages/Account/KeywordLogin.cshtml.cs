using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AvondaleCollegeClinic.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvondaleCollegeClinic.Areas.Identity.Pages.Account
{
    public class KeywordLoginModel : PageModel
    {
        private readonly UserManager<AvondaleCollegeClinicUser> _users;
        private readonly SignInManager<AvondaleCollegeClinicUser> _signIn;

        public KeywordLoginModel(UserManager<AvondaleCollegeClinicUser> users, SignInManager<AvondaleCollegeClinicUser> signIn)
        {
            _users = users;
            _signIn = signIn;
        }

        // Holds the form inputs from the page
        [BindProperty] public InputModel Input { get; set; } = new();

        public class InputModel
        {
            // User can type email or ID here
            [Required, Display(Name = "Email or ID")]
            public string Identifier { get; set; } = string.Empty;

            // Secret keyword they set earlier (their birth city)
            [Required, MaxLength(40), Display(Name = "City you were born in")]
            public string Keyword { get; set; } = string.Empty;
        }

        // GET just shows the form
        public void OnGet() { }

        // POST handles the keyword login attempt
        public async Task<IActionResult> OnPostAsync()
        {
            // If required fields are missing, show the page with errors
            if (!ModelState.IsValid) return Page();

            // Try find by email first; if not found, try by username (we use ID as username elsewhere)
            var user = await _users.FindByEmailAsync(Input.Identifier) ??
                       await _users.FindByNameAsync(Input.Identifier);

            // If no user, or they never set a keyword, we cannot verify
            if (user is null || string.IsNullOrWhiteSpace(user.CityOfBirth))
            {
                ModelState.AddModelError(string.Empty, "We can’t verify your keyword.");
                return Page();
            }

            // Compare stored keyword and input in a safe way:
            // trim spaces and compare in lower case so "Auckland" equals "auckland"
            var a = (user.CityOfBirth ?? string.Empty).Trim().ToLowerInvariant();
            var b = Input.Keyword.Trim().ToLowerInvariant();

            if (a != b)
            {
                // Wrong keyword -> do not reveal which part failed
                ModelState.AddModelError(string.Empty, "Keyword doesn’t match.");
                return Page();
            }

            // Keyword is correct:
            // mark the account so the user must set or change password now
            user.MustSetPassword = true;
            await _users.UpdateAsync(user);

            // Sign them in with a normal non-persistent cookie
            await _signIn.SignInAsync(user, isPersistent: false);

            // If they do not have a password yet, send them to SetPassword
            if (!await _users.HasPasswordAsync(user))
                return Redirect("~/Identity/Account/Manage/SetPassword");

            // If they already have a password, force them to set a new one
            return Redirect("~/Identity/Account/Manage/SetNewPassword");
        }
    }
}
