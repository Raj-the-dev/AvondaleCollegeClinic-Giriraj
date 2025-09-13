using AvondaleCollegeClinic.Areas.Identity.Data;   // added so we can use our custom IdentityUser class
using Microsoft.AspNetCore.Identity;                // added so we can work with roles and identity
using Microsoft.EntityFrameworkCore;
using AvondaleCollegeClinic.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AvondaleCollegeClinicContextConnection") ?? throw new InvalidOperationException("Connection string 'AvondaleCollegeClinicContextConnection' not found.");

builder.Services.AddDbContext<AvondaleCollegeClinicContext>(options => options.UseSqlServer(connectionString));
builder.Services
    // changed to use our custom user class instead of the default IdentityUser
    .AddDefaultIdentity<AvondaleCollegeClinicUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; // added so users don’t need to confirm email to login first time
        options.Password.RequiredLength = 6;            // added to make password rules easier
        options.Password.RequireNonAlphanumeric = false; // added so we don’t need special characters
        options.Password.RequireUppercase = false;       // added so uppercase letters are not required
        options.Password.RequireDigit = false;           // added so numbers are not required
    })
    .AddRoles<IdentityRole>()                           // added so we can use roles (like Admin, Doctor, etc.)
    .AddEntityFrameworkStores<AvondaleCollegeClinicContext>(); // added so identity knows to store users/roles in our DB

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
