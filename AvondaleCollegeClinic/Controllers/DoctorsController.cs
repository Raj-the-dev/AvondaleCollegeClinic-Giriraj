using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;
using System.IO;
using AvondaleCollegeClinic.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AvondaleCollegeClinic.Controllers
{

    public class DoctorsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public DoctorsController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Doctor,Student,Caregiver,Admin")]
        // GET: Doctors
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SpecSortParm"] = sortOrder == "Spec" ? "spec_desc" : "Spec";
            ViewData["CurrentFilter"] = searchString;

            var doctors = await _context.Doctors.AsNoTracking().ToListAsync();

            // Filter (searchString applies to FirstName, LastName, Specialization enum)
            if (!string.IsNullOrEmpty(searchString))
            {
                string search = searchString.ToLower();
                doctors = doctors.Where(d =>
                    d.FirstName.ToLower().Contains(search) ||
                    d.LastName.ToLower().Contains(search) ||
                    d.Specialization.ToString().ToLower().Contains(search)
                ).ToList();
            }

            // Sort
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

            // Pagination
            int pageSize = 10;
            int page = pageNumber ?? 1;
            var totalDoctors = doctors.Count;
            var pagedDoctors = doctors.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var paginatedList = new PaginatedList<Doctor>(pagedDoctors, totalDoctors, page, pageSize);
            return View(paginatedList);
        }


        [Authorize(Roles = "Admin,Doctor")]
        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // GET: Doctors/Create
        public IActionResult Create()
        {
            var doctor = new Doctor
            {
                DoctorID = GenerateDoctorID()
            };
            return View(doctor);
        }
        [Authorize(Roles = "Admin,Doctor")]
        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorID,FirstName,LastName,Specialization,Email,Phone,ImageFile,ImagePath")] Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                // Check email
                if (await _context.Doctors.AnyAsync(d => d.Email == doctor.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use by another doctor.");
                    return View(doctor);
                }

                // Check full name
                if (await _context.Doctors.AnyAsync(d => d.FirstName == doctor.FirstName && d.LastName == doctor.LastName))
                {
                    ModelState.AddModelError("", "A doctor with the same name already exists.");
                    return View(doctor);
                }
                if (doctor.ImageFile != null && doctor.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/doctors");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(doctor.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await doctor.ImageFile.CopyToAsync(fileStream);
                    }

                    doctor.ImagePath = "/images/doctors/" + uniqueFileName;
                }

                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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
        [Authorize(Roles = "Admin,Doctor")]
        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DoctorID,FirstName,LastName,Specialization,Email,Phone,ImageFile")] Doctor form)
        {
            if (id != form.DoctorID) return NotFound();

            if (!ModelState.IsValid)
            {
                // Load existing row so we don't lose current ImagePath
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorID == id);
                if (doctor == null) return NotFound();

                // Update simple fields
                doctor.FirstName = form.FirstName;
                doctor.LastName = form.LastName;    
                doctor.Specialization = form.Specialization;
                doctor.Email = form.Email;
                doctor.Phone = form.Phone;
                // Check email
                if (await _context.Doctors.AnyAsync(d => d.Email == doctor.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use by another doctor.");
                    return View(doctor);
                }

                // Check full name
                if (await _context.Doctors.AnyAsync(d => d.FirstName == doctor.FirstName && d.LastName == doctor.LastName))
                {
                    ModelState.AddModelError("", "A doctor with the same name already exists.");
                    return View(doctor);
                }

                // Handle new image (optional)
                if (form.ImageFile != null && form.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/doctors");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(form.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await form.ImageFile.CopyToAsync(fileStream);
                    }

                    doctor.ImagePath = "/images/doctors/" + uniqueFileName;
                    
                }
               

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(string id)
        {
            return _context.Doctors.Any(e => e.DoctorID == id);
        }

        private string GenerateDoctorID()
        {
            string newId;
            var random = new Random();
            int year = DateTime.Now.Year % 100;

            do
            {
                int num = random.Next(1000, 9999);
                newId = $"acd{year}{num}";
            }
            while (_context.Doctors.Any(d => d.DoctorID == newId));

            return newId;
        }
        // This method creates a unique DoctorID for every new teacher.
        // Which as a format of acd-YYXXXX
        // "act" referes to Avondale College Doctors
        // "YY"  last two digits of the current year (e.g., 2025 -> 25)
        // "XXXX" is a random 4-digit number (from 1000 to 9999)
        // The method checks the database to make sure the generated ID doesn't already exist. If it does, it tries again until it finds a unique one.
    }
}
