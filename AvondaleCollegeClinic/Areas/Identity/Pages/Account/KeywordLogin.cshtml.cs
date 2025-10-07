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

        [BindProperty] public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required, Display(Name = "Email or ID")]
            public string Identifier { get; set; } = string.Empty;

            [Required, MaxLength(40), Display(Name = "City you were born in")]
            public string Keyword { get; set; } = string.Empty;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _users.FindByEmailAsync(Input.Identifier) ??
                       await _users.FindByNameAsync(Input.Identifier);

            if (user is null || string.IsNullOrWhiteSpace(user.CityOfBirth))
            {
                ModelState.AddModelError(string.Empty, "We can’t verify your keyword.");
                return Page();
            }

            var a = (user.CityOfBirth ?? string.Empty).Trim().ToLowerInvariant();
            var b = Input.Keyword.Trim().ToLowerInvariant();

            if (a != b)
            {
                ModelState.AddModelError(string.Empty, "Keyword doesn’t match.");
                return Page();
            }

            // sign in and force them to set/change password immediately
            user.MustSetPassword = true;
            await _users.UpdateAsync(user);

            await _signIn.SignInAsync(user, isPersistent: false);

            // If they don't have a password, send to SetPassword page; otherwise to ChangePassword
            if (!await _users.HasPasswordAsync(user))
                return Redirect("~/Identity/Account/Manage/SetPassword");

            return Redirect("~/Identity/Account/Manage/SetNewPassword");
        }
    }
}
