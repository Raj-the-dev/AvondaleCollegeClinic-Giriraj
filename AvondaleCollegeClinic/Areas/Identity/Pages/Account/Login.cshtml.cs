using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AvondaleCollegeClinic.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvondaleCollegeClinic.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AvondaleCollegeClinicUser> _signIn;
        private readonly UserManager<AvondaleCollegeClinicUser> _users;

        public LoginModel(SignInManager<AvondaleCollegeClinicUser> signIn, UserManager<AvondaleCollegeClinicUser> users)
        {
            _signIn = signIn;
            _users = users;
        }

        [BindProperty] public InputModel Input { get; set; } = new();
        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Email or ID")]
            public string Identifier { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public void OnGet(string? returnUrl = null) => ReturnUrl = returnUrl ?? Url.Content("~/");

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid) return Page();

            // find user by email first, then by username (where we store IDs for first-time setup)
            AvondaleCollegeClinicUser? user =
                await _users.FindByEmailAsync(Input.Identifier) ??
                await _users.FindByNameAsync(Input.Identifier);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "User not found. If this is your first time, use First-time setup.");
                return Page();
            }

            // DO NOT allow password-less sign in here. If user never set a password, block and send to setup.
            if (!await _users.HasPasswordAsync(user))
            {
                ModelState.AddModelError(string.Empty, "You must set your password first. Use First-time setup.");
                return Page();
            }

            var res = await _signIn.PasswordSignInAsync(user, Input.Password, isPersistent: false, lockoutOnFailure: true);
            if (res.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            if (res.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account locked. Try again later.");
                return Page();
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
