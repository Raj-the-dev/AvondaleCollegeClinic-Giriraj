using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;                       // MVC base types
using Microsoft.AspNetCore.Mvc.Rendering;             // SelectList helpers
using Microsoft.EntityFrameworkCore;                  // EF Core queries
using AvondaleCollegeClinic.Areas.Identity.Data;      // Identity types (not used directly here)
using AvondaleCollegeClinic.Models;                   // Doctor model
using System.IO;                                      // File IO for image uploads
using AvondaleCollegeClinic.Helpers;                  // PaginatedList<T>
using Microsoft.AspNetCore.Authorization;             // [Authorize]
using Microsoft.AspNetCore.Identity;                  // Identity (not used here but ok)

namespace AvondaleCollegeClinic.Controllers
{
    [Authorize(Roles = "Doctor,Student,Caregiver,Admin,Teachers")]
    public class DoctorsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context; // our database

        public DoctorsController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Doctor,Student,Caregiver,Admin,Teachers")]
        // GET: Doctors
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            // Remember current sort mode and search term for the UI
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SpecSortParm"] = sortOrder == "Spec" ? "spec_desc" : "Spec";
            ViewData["CurrentFilter"] = searchString;

            // Pull all doctors (no tracking = faster reads when not editing)
            var doctors = await _context.Doctors.AsNoTracking().ToListAsync();

            // -------- Filter (search box) --------
            // User can type a name or a specialization
            if (!string.IsNullOrEmpty(searchString))
            {
                string search = searchString.ToLower();
                doctors = doctors.Where(d =>
                    d.FirstName.ToLower().Contains(search) ||
                    d.LastName.ToLower().Contains(search) ||
                    d.Specialization.ToString().ToLower().Contains(search)
                ).ToList();
            }

            // -------- Sort --------
            // Choose sort order based on the query string
            switch (sortOrder)
            {
                case "name_desc":
                    doctors = doctors.OrderByDescending(d => d.LastName).ToList();
                    break;
                case "Spec":
                    doctors = doctors.OrderBy(d => d.Specialization).ToList();
                    break;
                case "spec_desc":
                    doctors = doctors.OrderByDescending(d => d.Specialization).ToList();
                    break;
                default:
                    doctors = doctors.OrderBy(d => d.LastName).ToList();
                    break;
            }

            // -------- Pagination --------
            // Show a small page of results at a time
            int pageSize = 6;
            int page = pageNumber ?? 1;
            var totalDoctors = doctors.Count;
            var pagedDoctors = doctors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Wrap in our helper so the view can render next/prev buttons
            var paginatedList = new PaginatedList<Doctor>(pagedDoctors, totalDoctors, page, pageSize);
            return View(paginatedList);
        }

        [Authorize(Roles = "Admin,Doctor")]
        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            // Guard for missing id
            if (id == null) return NotFound();

            // Find the doctor row by key
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        [Authorize(Roles = "Admin,Doctor")]
        // GET: Doctors/Create
        public IActionResult Create()
        {
            // Pre-fill a new random DoctorID so the form shows it
            var doctor = new Doctor
            {
                DoctorID = GenerateDoctorID()
            };
            return View(doctor);
        }

        [Authorize(Roles = "Admin,Doctor")]
        // POST: Doctors/Create
        // The [Bind(...)] protects from overposting by listing allowed properties.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorID,FirstName,LastName,Specialization,Email,Phone,ImageFile,ImagePath")] Doctor doctor)
        {
            // NOTE: This project uses an inverted pattern:
            // it proceeds when ModelState is NOT valid. We keep your behavior and add comments only.
            if (!ModelState.IsValid)
            {
                // 1) Unique email check
                if (await _context.Doctors.AnyAsync(d => d.Email == doctor.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use by another doctor.");
                    return View(doctor);
                }

                // 2) Unique full name check
                if (await _context.Doctors.AnyAsync(d => d.FirstName == doctor.FirstName && d.LastName == doctor.LastName))
                {
                    ModelState.AddModelError("", "A doctor with the same name already exists.");
                    return View(doctor);
                }

                // 3) Optional image upload to /wwwroot/images/doctors
                if (doctor.ImageFile != null && doctor.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/doctors");
                    Directory.CreateDirectory(uploadsFolder); // ensure folder exists

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(doctor.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await doctor.ImageFile.CopyToAsync(fileStream);
                    }

                    // Store the web path, not the disk path
                    doctor.ImagePath = "/images/doctors/" + uniqueFileName;
                }

                // 4) Save the new row
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got here, just return the form as-is
            return View(doctor);
        }

        [Authorize(Roles = "Admin,Doctor")]
        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Edit(string id, [Bind("DoctorID,FirstName,LastName,Specialization,Email,Phone,ImageFile")] Doctor form)
        {
            // Safety: the URL id must match the model id
            if (id != form.DoctorID) return NotFound();

            // Same inverted pattern here: do updates when ModelState is NOT valid
            if (!ModelState.IsValid)
            {
                // Load the tracked entity to update
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorID == id);
                if (doctor == null) return NotFound();

                // Copy simple fields
                doctor.FirstName = form.FirstName;
                doctor.LastName = form.LastName;
                doctor.Specialization = form.Specialization;
                doctor.Email = form.Email;


                // Unique email check excluding this record
                bool emailInUse = await _context.Doctors
                    .AnyAsync(d => d.Email == doctor.Email && d.DoctorID != doctor.DoctorID);
                if (emailInUse)
                {
                    ModelState.AddModelError("Email", "This email is already in use by another doctor.");
                    return View(doctor);
                }

                // Unique name check excluding this record
                bool sameNameExists = await _context.Doctors
                    .AnyAsync(d => d.FirstName == doctor.FirstName
                                && d.LastName == doctor.LastName
                                && d.DoctorID != doctor.DoctorID);
                if (sameNameExists)
                {
                    ModelState.AddModelError("", "A doctor with the same name already exists.");
                    return View(doctor);
                }

                // Optional new image upload
                if (form.ImageFile != null && form.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/doctors");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(form.ImageFile.FileName);

                    using var fs = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create);
                    await form.ImageFile.CopyToAsync(fs);

                    doctor.ImagePath = "/images/doctors/" + uniqueFileName;
                }

                // Save updates
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we reach here, show the form again with submitted values
            return View(form);
        }

        [Authorize(Roles = "Admin,Doctor")]
        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        [Authorize(Roles = "Admin,Doctor")]
        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Find and remove the record
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Quick existence check often used in scaffolds
        private bool DoctorExists(string id)
        {
            return _context.Doctors.Any(e => e.DoctorID == id);
        }

        // DoctorID generator
        // Pattern: "acd" + YY + 4 random digits, unique in the database
        private string GenerateDoctorID()
        {
            string newId;
            var random = new Random();
            int year = DateTime.Now.Year % 100; // last two digits

            do
            {
                int num = random.Next(1000, 9999); // random 4-digit number
                newId = $"acd{year}{num}";
            }
            while (_context.Doctors.Any(d => d.DoctorID == newId)); // ensure uniqueness

            return newId;
        }

        // Notes about the ID format (from your comment):
        // acd-YYXXXX where:
        //  - "acd" means Avondale College Doctors
        //  - "YY" is the last two digits of the current year
        //  - "XXXX" is a random 4-digit number between 1000 and 9999
        // The code checks the database in a loop until it finds an unused value.
    }
}
