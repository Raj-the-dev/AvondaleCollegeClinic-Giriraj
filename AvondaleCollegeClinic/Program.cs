using AvondaleCollegeClinic.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AvondaleCollegeClinicContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AvondaleCollegeClinicContextConnection")
        ?? throw new InvalidOperationException("Connection string not found.")));

// --- Identity setup (with roles + password rules) ---
builder.Services.AddIdentity<AvondaleCollegeClinicUser, IdentityRole>(options =>
{
    // Password rules (not too strict, not too weak)
    options.Password.RequiredLength = 8;     // min 8 characters
    options.Password.RequireDigit = true;    // must have at least one number
    options.Password.RequireUppercase = true;// must have at least one capital letter
    options.Password.RequireLowercase = true;// must have at least one lowercase letter
    options.Password.RequireNonAlphanumeric = false; // special characters not required
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.AllowedForNewUsers = true;

    // Usernames / emails
    options.User.RequireUniqueEmail = true; // students/teachers/doctors use emails
})
.AddEntityFrameworkStores<AvondaleCollegeClinicContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();


builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Identity/Account/Login";        // redirect here if not logged in
    opt.AccessDeniedPath = "/Identity/Account/AccessDenied"; // redirect if not authorized
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
    options.AddPolicy("StudentOnly", p => p.RequireRole("Student"));
    options.AddPolicy("TeacherOnly", p => p.RequireRole("Teacher"));
    options.AddPolicy("DoctorOnly", p => p.RequireRole("Doctor"));
    options.AddPolicy("CaregiverOnly", p => p.RequireRole("Caregiver"));
});


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<AvondaleCollegeClinicUser>>();

    string[] roles = { "Admin", "Student", "Teacher", "Doctor", "Caregiver" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed default admin
    var adminEmail = "admin@avondaleclinic.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new AvondaleCollegeClinicUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Admin",
            MustSetPassword = false
        };

        var create = await userManager.CreateAsync(adminUser, "Admin#2025!");
        if (create.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
