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
        // Identity services:
        // _signIn handles sign-in cookies and lockout logic
        // _users lets us find users by email or username
        private readonly SignInManager<AvondaleCollegeClinicUser> _signIn;
        private readonly UserManager<AvondaleCollegeClinicUser> _users;

        public LoginModel(SignInManager<AvondaleCollegeClinicUser> signIn, UserManager<AvondaleCollegeClinicUser> users)
        {
            _signIn = signIn;
            _users = users;
        }

        // The form fields are bound into this object on POST
        [BindProperty] public InputModel Input { get; set; } = new();

        // Where to send the user after a successful login
        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            // Users can type either their email or their ID here
            [Required]
            [Display(Name = "Email or ID")]
            public string Identifier { get; set; } = string.Empty;

            // Plain password input; Identity will check hashing and lockout rules
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        // GET: pre-store the target url or default to site root
        public void OnGet(string? returnUrl = null) => ReturnUrl = returnUrl ?? Url.Content("~/");

        // POST: try to sign the user in
        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            // If no return url was provided, go home after login
            returnUrl ??= Url.Content("~/");

            // If form validation failed, show errors and stop
            if (!ModelState.IsValid) return Page();

            // Try to find the user:
            // first by email, then by username (we store the school ID as username for first-time setup)
            AvondaleCollegeClinicUser? user =
                await _users.FindByEmailAsync(Input.Identifier) ??
                await _users.FindByNameAsync(Input.Identifier);

            // If no matching user, give a friendly hint about first-time setup
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "User not found. If this is your first time, use First-time setup.");
                return Page();
            }

            // Block accounts that do not have a password yet
            // This forces them through the first-time setup flow to create one
            if (!await _users.HasPasswordAsync(user))
            {
                ModelState.AddModelError(string.Empty, "You must set your password first. Use First-time setup.");
                return Page();
            }

            // Ask Identity to check the password and sign in
            // isPersistent:false = session cookie only
            // lockoutOnFailure:true = too many wrong attempts will lock the account
            var res = await _signIn.PasswordSignInAsync(user, Input.Password, isPersistent: false, lockoutOnFailure: true);

            // Success: send them to the original destination (or home)
            if (res.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            // If they are locked out, show a specific message
            if (res.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account locked. Try again later.");
                return Page();
            }

            // Otherwise a generic invalid login error
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
