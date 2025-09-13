using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace AvondaleCollegeClinic.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public IActionResult OnGet() => RedirectToPage("./Login");
        public IActionResult OnPost() => RedirectToPage("./Login");
    }
}
