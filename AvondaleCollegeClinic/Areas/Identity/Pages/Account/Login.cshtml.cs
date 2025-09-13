using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AvondaleCollegeClinic.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AvondaleCollegeClinicUser> _signInManager;
        private readonly UserManager<AvondaleCollegeClinicUser> _userManager;
        private readonly AvondaleCollegeClinicContext _db;

        public LoginModel(
            SignInManager<AvondaleCollegeClinicUser> signInManager,
            UserManager<AvondaleCollegeClinicUser> userManager,
            AvondaleCollegeClinicContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
        }

        [BindProperty] public InputModel Input { get; set; } = new();
        [BindProperty] public bool AdminMode { get; set; }
        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            // Regular user mode (S/T/D/C)
            [Display(Name = "ID or Email")]
            public string? Identifier { get; set; } // StudentID/TeacherID/DoctorID/CaregiverID or email

            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [EmailAddress, Display(Name = "Email (first-time claim)")]
            public string? EmailForFirstTime { get; set; }


            // Admin mode
            [EmailAddress]
            public string? AdminEmail { get; set; }
        }

        public void OnGet(string? returnUrl = null, int? admin = null)
        {
            AdminMode = admin == 1;
            ViewData["AdminMode"] = AdminMode;
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["AdminMode"] = AdminMode;

            if (AdminMode)
            {
                if (string.IsNullOrWhiteSpace(Input.AdminEmail) || string.IsNullOrWhiteSpace(Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Admin email and password are required.");
                    return Page();
                }

                var adminUser = await _userManager.FindByEmailAsync(Input.AdminEmail);
                if (adminUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid admin login.");
                    return Page();
                }

                var res = await _signInManager.PasswordSignInAsync(adminUser, Input.Password!, false, lockoutOnFailure: true);
                if (res.Succeeded) return LocalRedirect(returnUrl);

                ModelState.AddModelError(string.Empty, "Invalid admin login.");
                return Page();
            }

            // ----- USER MODE (S/T/D/C) -----

            if (string.IsNullOrWhiteSpace(Input.Identifier))
            {
                ModelState.AddModelError(string.Empty, "Please enter your ID or Email.");
                return Page();
            }

            var identifier = Input.Identifier.Trim();

            // If a password is provided, try normal password login:
            if (!string.IsNullOrEmpty(Input.Password))
            {
                // Accept either email or ID
                AvondaleCollegeClinicUser? user = null;

                if (identifier.Contains("@"))
                {
                    user = await _userManager.FindByEmailAsync(identifier);
                }
                else
                {
                    // resolve ID -> user
                    user = await ResolveUserByDomainId(identifier);
                }

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(user, Input.Password!, false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    // If they still must set password (edge-case), push them to CompleteProfile
                    if (user.MustSetPassword) return RedirectToPage("/Account/Manage/CompleteProfile");
                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            // No password given => first-time claim flow
            // Find their model by ID or email and create/link an Identity user if needed.
            var (foundType, foundObj) = await FindDomainByIdentifierOrEmail(identifier, Input.EmailForFirstTime);

            if (foundType == UserKind.Admin || foundObj == null)
            {
                ModelState.AddModelError(string.Empty, "Could not find a matching Student/Teacher/Doctor/Caregiver by that ID or Email.");
                return Page();
            }

            // Load existing or create a new Identity user and link it.
            AvondaleCollegeClinicUser? existingUser = await ResolveUserFromDomain(foundType, foundObj);
            if (existingUser == null)
            {
                // create new identity user (no password yet)
                var (first, last, email, avatar) = ExtractProfile(foundType, foundObj);
                var emailToUse = Input.EmailForFirstTime ?? email ?? $"{identifier}@noemail.local";
                var user = new AvondaleCollegeClinicUser
                {
                    UserName = emailToUse,
                    Email = emailToUse,
                    EmailConfirmed = true,
                    FirstName = first,
                    LastName = last,
                    AvatarPath = avatar,
                    UserKind = foundType,
                    MustSetPassword = true
                };

                var createRes = await _userManager.CreateAsync(user);
                if (!createRes.Succeeded)
                {
                    foreach (var e in createRes.Errors) ModelState.AddModelError(string.Empty, e.Description);
                    return Page();
                }

                // Link to the domain row
                await LinkUserToDomain(user, foundType, foundObj);
                existingUser = user;
            }
            else
            {
                // Ensure flag so we force password set
                if (!await _userManager.HasPasswordAsync(existingUser))
                {
                    existingUser.MustSetPassword = true;
                    await _userManager.UpdateAsync(existingUser);
                }
            }

            // Sign in and force them to CompleteProfile
            await _signInManager.SignInAsync(existingUser, isPersistent: false);
            return RedirectToPage("/Account/Manage/CompleteProfile");
        }

        // ---------- helpers ----------
        private async Task<AvondaleCollegeClinicUser?> ResolveUserByDomainId(string id)
        {
            // Try each domain model by ID
            var s = await _db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.StudentID == id);
            if (s?.AvondaleCollegeClinicUserAccount != null) return await _userManager.FindByIdAsync(s.IdentityUserId);

            var t = await _db.Teachers.AsNoTracking().FirstOrDefaultAsync(x => x.TeacherID == id);
            if (t?.AvondaleCollegeClinicUserAccount != null) return await _userManager.FindByIdAsync(t.IdentityUserId);

            var d = await _db.Doctors.AsNoTracking().FirstOrDefaultAsync(x => x.DoctorID == id);
            if (d?.AvondaleCollegeClinicUserAccount != null) return await _userManager.FindByIdAsync(d.IdentityUserId);

            if (int.TryParse(id, out var cgId))
            {
                var c = await _db.Caregivers.AsNoTracking().FirstOrDefaultAsync(x => x.CaregiverID == cgId);
                if (c?.AvondaleCollegeClinicUserAccount != null) return await _userManager.FindByIdAsync(c.IdentityUserId);
            }
            return null;
        }

        private async Task<(UserKind kind, object? model)> FindDomainByIdentifierOrEmail(string identifier, string? email)
        {
            // Try ID first
            var s = await _db.Students.FirstOrDefaultAsync(x => x.StudentID == identifier);
            if (s != null) return (UserKind.Student, s);

            var t = await _db.Teachers.FirstOrDefaultAsync(x => x.TeacherID == identifier);
            if (t != null) return (UserKind.Teacher, t);

            var d = await _db.Doctors.FirstOrDefaultAsync(x => x.DoctorID == identifier);
            if (d != null) return (UserKind.Doctor, d);

            if (int.TryParse(identifier, out var cgId))
            {
                var c = await _db.Caregivers.FirstOrDefaultAsync(x => x.CaregiverID == cgId);
                if (c != null) return (UserKind.Caregiver, c);
            }

            // Try email if provided
            if (!string.IsNullOrWhiteSpace(email))
            {
                email = email.Trim();
                var se = await _db.Students.FirstOrDefaultAsync(x => x.Email == email);
                if (se != null) return (UserKind.Student, se);

                var te = await _db.Teachers.FirstOrDefaultAsync(x => x.Email == email);
                if (te != null) return (UserKind.Teacher, te);

                var de = await _db.Doctors.FirstOrDefaultAsync(x => x.Email == email);
                if (de != null) return (UserKind.Doctor, de);

                var ce = await _db.Caregivers.FirstOrDefaultAsync(x => x.Email == email);
                if (ce != null) return (UserKind.Caregiver, ce);
            }

            return (UserKind.Admin, null);
        }

        private static (string? first, string? last, string? email, string? avatar) ExtractProfile(UserKind kind, object model)
        {
            switch (kind)
            {
                case UserKind.Student:
                    {
                        var s = (Student)model;
                        return (s.FirstName, s.LastName, s.Email, s.ImagePath);
                    }
                case UserKind.Teacher:
                    {
                        var t = (Teacher)model;
                        return (t.FirstName, t.LastName, t.Email, t.ImagePath);
                    }
                case UserKind.Doctor:
                    {
                        var d = (Doctor)model;
                        return (d.FirstName, d.LastName, d.Email, d.ImagePath);
                    }
                case UserKind.Caregiver:
                    {
                        var c = (Caregiver)model;
                        return (c.FirstName, c.LastName, c.Email, c.ImagePath);
                    }
                default:
                    return (null, null, null, null);
            }
        }

        private async Task LinkUserToDomain(AvondaleCollegeClinicUser user, UserKind kind, object model)
        {
            switch (kind)
            {
                case UserKind.Student:
                    var s = (Student)model;
                    s.IdentityUserId = user.Id;
                    _db.Students.Update(s);
                    break;
                case UserKind.Teacher:
                    var t = (Teacher)model;
                    t.IdentityUserId = user.Id;
                    _db.Teachers.Update(t);
                    break;
                case UserKind.Doctor:
                    var d = (Doctor)model;
                    d.IdentityUserId = user.Id;
                    _db.Doctors.Update(d);
                    break;
                case UserKind.Caregiver:
                    var c = (Caregiver)model;
                    c.IdentityUserId = user.Id;
                    _db.Caregivers.Update(c);
                    break;
            }
            await _db.SaveChangesAsync();
        }

        private async Task<AvondaleCollegeClinicUser?> ResolveUserFromDomain(UserKind kind, object model)
        {
            string? userId = kind switch
            {
                UserKind.Student => ((Student)model).IdentityUserId,
                UserKind.Teacher => ((Teacher)model).IdentityUserId,
                UserKind.Doctor => ((Doctor)model).IdentityUserId,
                UserKind.Caregiver => ((Caregiver)model).IdentityUserId,
                _ => null
            };
            return string.IsNullOrEmpty(userId) ? null : await _userManager.FindByIdAsync(userId);
        }
    }
}
