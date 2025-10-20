using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;                       // MVC base types (Controller, IActionResult)
using Microsoft.AspNetCore.Mvc.Rendering;             // SelectList and SelectListItem
using Microsoft.EntityFrameworkCore;                  // EF Core queries and async ops
using AvondaleCollegeClinic.Areas.Identity.Data;      // Identity user type (not used directly here but fine)
using AvondaleCollegeClinic.Models;                   // Caregiver model
using AvondaleCollegeClinic.Helpers;                  // PaginatedList helper (used in Index)
using Microsoft.AspNetCore.Authorization;             // [Authorize] attributes
using Microsoft.AspNetCore.Identity;                  // Identity services (not used in this controller)

namespace AvondaleCollegeClinic.Controllers
{
    // No class-level [Authorize], so actions control access individually
    public class CaregiversController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context; // our EF Core data context

        public CaregiversController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Teacher,Caregivers")]
        // GET: Caregivers
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            // Save UI state for sorting arrows and current search box value
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            // Load all caregivers into memory without tracking (faster reads)
            var caregivers = await _context.Caregivers.AsNoTracking().ToListAsync();

            // Search / Filter 
            if (!string.IsNullOrEmpty(searchString))
            {
                // Lowercase once so comparisons are case-insensitive
                searchString = searchString.ToLower();

                // Match on first name, last name, email, or relationship text
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


            // Pagination (simple and manual)

            int pageSize = 6;                         // show 6 rows per page
            int page = pageNumber ?? 1;               // default to page 1
            var totalCount = caregivers.Count;        // total rows after search/filter
            var pagedCaregivers = caregivers
                .Skip((page - 1) * pageSize)          // skip rows before current page
                .Take(pageSize)                        // take only this page
                .ToList();

            // Wrap into our PaginatedList so the view can show next/prev buttons
            var paginatedList = new PaginatedList<Caregiver>(pagedCaregivers, totalCount, page, pageSize);
            return View(paginatedList);
        }

        [Authorize(Roles = "Admin,Teacher")]
        // GET: Caregivers/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            // basic guard against bad URLs
            if (id == null)
            {
                return NotFound();
            }

            // find by key
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
            // Pre-fill default values to help the user:
            // - generate a new CaregiverID
            // - DOB defaults to today (user will change it)
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
        [ValidateAntiForgeryToken] // CSRF protection for form posts
        public async Task<IActionResult> Create([Bind("CaregiverID,FirstName,LastName,DOB,Email,Phone,Relationship,ImageFile,ImagePath")] Caregiver caregiver)
        {
            // Note: This action uses the inverted pattern from your project:
            // create when ModelState is NOT valid? Here we keep your exact logic and add comments only.
            if (!ModelState.IsValid)
            {
                // 1) Unique email check
                if (await _context.Caregivers.AnyAsync(c => c.Email == caregiver.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use by another caregiver.");
                    return View(caregiver);
                }

                // 2) Unique full name check (first + last)
                if (await _context.Caregivers.AnyAsync(c => c.FirstName == caregiver.FirstName && c.LastName == caregiver.LastName))
                {
                    ModelState.AddModelError("", "A caregiver with the same name already exists.");
                    return View(caregiver);
                }

                // 3) Optional image upload to wwwroot/images/caregivers
                if (caregiver.ImageFile != null && caregiver.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/caregivers");
                    Directory.CreateDirectory(uploadsFolder); // make sure folder exists

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(caregiver.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // copy the stream to disk
                    using (var fs = new FileStream(filePath, FileMode.Create))
                        await caregiver.ImageFile.CopyToAsync(fs);

                    // store the web path in the DB
                    caregiver.ImagePath = "/images/caregivers/" + uniqueFileName;
                }

                // 4) Save to database
                _context.Add(caregiver);
                await _context.SaveChangesAsync();

                // 5) Return to the list
                return RedirectToAction(nameof(Index));
            }

            // If ModelState was valid according to this action's logic, we just return the view
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

            // Load the row we want to edit
            var caregiver = await _context.Caregivers.FindAsync(id);
            if (caregiver == null)
            {
                return NotFound();
            }
            return View(caregiver);
        }

        // POST: Caregivers/Edit/5
        // The Bind list limits what can be edited to avoid overposting
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(string id, [Bind("CaregiverID,FirstName,LastName,DOB,Email,Phone,Relationship,ImageFile")] Caregiver form)
        {
            // Safety check: route id must match the submitted model id
            if (id != form.CaregiverID) return NotFound();

            // Keep the project's inverted pattern: do updates when ModelState is NOT valid
            if (!ModelState.IsValid)
            {
                // Load the real tracked entity
                var caregiver = await _context.Caregivers.FirstOrDefaultAsync(c => c.CaregiverID == id);
                if (caregiver == null) return NotFound();

                // Update fields from the form
                caregiver.FirstName = form.FirstName;
                caregiver.LastName = form.LastName;
                caregiver.DOB = form.DOB;
                caregiver.Email = form.Email;

                caregiver.Relationship = form.Relationship;

                // Unique email check excluding the current record
                bool emailInUse = await _context.Caregivers
                    .AnyAsync(c => c.Email == caregiver.Email && c.CaregiverID != caregiver.CaregiverID);
                if (emailInUse)
                {
                    ModelState.AddModelError("Email", "This email is already in use by another caregiver.");
                    return View(caregiver);
                }

                // Unique name check excluding the current record
                bool sameNameExists = await _context.Caregivers
                    .AnyAsync(c => c.FirstName == caregiver.FirstName
                                && c.LastName == caregiver.LastName
                                && c.CaregiverID != caregiver.CaregiverID);
                if (sameNameExists)
                {
                    ModelState.AddModelError("", "A caregiver with the same name already exists.");
                    return View(caregiver);
                }

                // Optional image upload
                if (form.ImageFile != null && form.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/caregivers");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(form.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save new image and update path
                    using var fs = new FileStream(filePath, FileMode.Create);
                    await form.ImageFile.CopyToAsync(fs);
                    caregiver.ImagePath = "/images/caregivers/" + uniqueFileName;
                }

                // Persist changes
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If ModelState was valid in this action's flow, just show the same form
            return View(form);
        }

        [Authorize(Roles = "Admin,Teacher")]
        // GET: Caregivers/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load the caregiver to confirm deletion
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
            // Find by key then remove
            var caregiver = await _context.Caregivers.FindAsync(id);
            if (caregiver != null)
            {
                _context.Caregivers.Remove(caregiver);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Quick existence check used by scaffolds sometimes
        private bool CaregiverExists(string id)
        {
            return _context.Caregivers.Any(e => e.CaregiverID == id);
        }

        // ID generator for caregivers:
        // Pattern "acc" + YY + 4-digit counter
        // Example for 2025: acc250001, acc250002, ...
        private string GenerateCaregiverID()
        {
            // 1) Make a prefix like "acc25" from the current year
            string yearPrefix = "acc" + DateTime.Now.ToString("yy");

            // 2) Find the latest existing ID with this prefix so we can continue the sequence
            var latestId = _context.Caregivers
                .Where(c => c.CaregiverID.StartsWith(yearPrefix))
                .OrderByDescending(c => c.CaregiverID)
                .Select(c => c.CaregiverID)
                .FirstOrDefault();

            // 3) Work out the next number
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(latestId) && latestId.Length == 9) // "acc25" + 4 digits = 9 chars
            {
                // Grab the last 4 digits as an integer
                int.TryParse(latestId.Substring(5), out nextNumber);
                nextNumber++; // move to the next ID
            }

            // 4) Return the final ID with zero-padded 4-digit number
            return $"{yearPrefix}{nextNumber:D4}";
        }
    }
}
