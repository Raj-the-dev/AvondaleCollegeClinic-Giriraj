using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;
using AvondaleCollegeClinic.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AvondaleCollegeClinic.Controllers
{

    public class CaregiversController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public CaregiversController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,Teacher,Caregivers")]
        // GET: Caregivers
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            var caregivers = await _context.Caregivers.AsNoTracking().ToListAsync();

            // Search / Filter
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                caregivers = caregivers.Where(c =>
                    c.FirstName.ToLower().Contains(searchString) ||
                    c.LastName.ToLower().Contains(searchString) ||
                    c.Email.ToLower().Contains(searchString) ||
                    c.Relationship.ToString().ToLower().Contains(searchString)
                ).ToList();
            }

            // Sort
            switch (sortOrder)
            {
                case "name_desc":
                    caregivers = caregivers.OrderByDescending(c => c.LastName).ToList();
                    break;
                case "Date":
                    caregivers = caregivers.OrderBy(c => c.DOB).ToList();
                    break;
                case "date_desc":
                    caregivers = caregivers.OrderByDescending(c => c.DOB).ToList();
                    break;
                default:
                    caregivers = caregivers.OrderBy(c => c.LastName).ToList();
                    break;
            }

            // Pagination
            int pageSize = 6;
            int page = pageNumber ?? 1;
            var totalCount = caregivers.Count;
            var pagedCaregivers = caregivers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var paginatedList = new PaginatedList<Caregiver>(pagedCaregivers, totalCount, page, pageSize);
            return View(paginatedList);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Caregivers/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caregiver = await _context.Caregivers
                .FirstOrDefaultAsync(m => m.CaregiverID == id);
            if (caregiver == null)
            {
                return NotFound();
            }

            return View(caregiver);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Caregivers/Create
        public IActionResult Create()
        {
            var caregiver = new Caregiver
            {
                CaregiverID = GenerateCaregiverID(), // auto-generate the Caregiver ID
                DOB = DateTime.Now                   // pre-fill DOB with today (user can change)
            };

            return View(caregiver);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // POST: Caregivers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaregiverID,FirstName,LastName,DOB,Email,Phone,Relationship,ImageFile,ImagePath")] Caregiver caregiver)
        {
            if (!ModelState.IsValid)
            {
                // Check email uniqueness
                if (await _context.Caregivers.AnyAsync(c => c.Email == caregiver.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use by another caregiver.");
                    return View(caregiver);
                }

                // Check full name uniqueness
                if (await _context.Caregivers.AnyAsync(c => c.FirstName == caregiver.FirstName && c.LastName == caregiver.LastName))
                {
                    ModelState.AddModelError("", "A caregiver with the same name already exists.");
                    return View(caregiver);
                }

                // Handle image upload
                if (caregiver.ImageFile != null && caregiver.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/caregivers");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(caregiver.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                        await caregiver.ImageFile.CopyToAsync(fs);

                    caregiver.ImagePath = "/images/caregivers/" + uniqueFileName;
                }

                _context.Add(caregiver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(caregiver);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Caregivers/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caregiver = await _context.Caregivers.FindAsync(id);
            if (caregiver == null)
            {
                return NotFound();
            }
            return View(caregiver);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // POST: Caregivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CaregiverID,FirstName,LastName,DOB,Email,Phone,Relationship,ImageFile")] Caregiver form)
        {
            if (id != form.CaregiverID) return NotFound();

            if (!ModelState.IsValid)
            {
                var caregiver = await _context.Caregivers.FirstOrDefaultAsync(c => c.CaregiverID == id);
                if (caregiver == null) return NotFound();

                caregiver.FirstName = form.FirstName;
                caregiver.LastName = form.LastName;
                caregiver.DOB = form.DOB;
                caregiver.Email = form.Email;
                caregiver.Phone = form.Phone;
                caregiver.Relationship = form.Relationship;

                // Check email
                if (await _context.Caregivers.AnyAsync(c => c.Email == caregiver.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use by another caregiver.");
                    return View(caregiver);
                }

                // Check full name
                if (await _context.Caregivers.AnyAsync(c => c.FirstName == caregiver.FirstName && c.LastName == caregiver.LastName))
                {
                    ModelState.AddModelError("", "A caregiver with the same name already exists.");
                    return View(caregiver);
                }

                if (form.ImageFile != null && form.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/caregivers");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(form.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                        await form.ImageFile.CopyToAsync(fs);

                    caregiver.ImagePath = "/images/caregivers/" + uniqueFileName;
                    // (optional) delete old file here
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(form);
        }
        [Authorize(Roles = "Admin,Teacher")]
        //GET: Caregivers/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caregiver = await _context.Caregivers
                .FirstOrDefaultAsync(m => m.CaregiverID == id);
            if (caregiver == null)
            {
                return NotFound();
            }

            return View(caregiver);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // POST: Caregivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {
            var caregiver = await _context.Caregivers.FindAsync(id);
            if (caregiver != null)
            {
                _context.Caregivers.Remove(caregiver);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaregiverExists(string id)
        {
            return _context.Caregivers.Any(e => e.CaregiverID == id);
        }
        private string GenerateCaregiverID()
        {
            // Start the ID with "acc" + current year's last two digits (e.g., "acc25" for 2025)
            string yearPrefix = "acc" + DateTime.Now.ToString("yy");

            // Find the most recent caregiver ID that starts with that prefix
            var latestId = _context.Caregivers
                .Where(c => c.CaregiverID.StartsWith(yearPrefix))
                .OrderByDescending(c => c.CaregiverID)
                .Select(c => c.CaregiverID)
                .FirstOrDefault();

            // Figure out what the next number should be
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(latestId) && latestId.Length == 9) // "acc25" + 4 digits = 9 chars
            {
                int.TryParse(latestId.Substring(5), out nextNumber);
                nextNumber++; // Increment for the new ID
            }

            // Return something like "acc250001", "acc250002", etc.
            return $"{yearPrefix}{nextNumber:D4}";
        }
    }
}
