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

namespace AvondaleCollegeClinic.Controllers
{
    public class CaregiversController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public CaregiversController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

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
            int pageSize = 10;
            int page = pageNumber ?? 1;
            var totalCount = caregivers.Count;
            var pagedCaregivers = caregivers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var paginatedList = new PaginatedList<Caregiver>(pagedCaregivers, totalCount, page, pageSize);
            return View(paginatedList);
        }

        // GET: Caregivers/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Caregivers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Caregivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaregiverID,FirstName,LastName,DOB,Email,Phone,Relationship,ImageFile,ImagePath")] Caregiver caregiver)
        {
            if (!ModelState.IsValid)
            {
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

        // GET: Caregivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Caregivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaregiverID,FirstName,LastName,DOB,Email,Phone,Relationship,ImageFile")] Caregiver form)
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

        // GET: Caregivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Caregivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caregiver = await _context.Caregivers.FindAsync(id);
            if (caregiver != null)
            {
                _context.Caregivers.Remove(caregiver);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaregiverExists(int id)
        {
            return _context.Caregivers.Any(e => e.CaregiverID == id);
        }
    }
}
