using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Helpers;
using AvondaleCollegeClinic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AvondaleCollegeClinic.Controllers
{
    public class HomeroomsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public HomeroomsController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }


        // Helpers
        // Build the Teacher dropdown list once in one place.
        // We show "FirstName LastName" but keep the real value as TeacherID.
        private SelectList BuildTeacherSelectList(string? selected = null)
        {
            var teachers = _context.Teachers
                .Select(t => new
                {
                    t.TeacherID,
                    FullName = t.FirstName + " " + t.LastName
                })
                .OrderBy(t => t.FullName)
                .ToList();

            return new SelectList(teachers, "TeacherID", "FullName", selected);
        }

        // Make a new HomeroomID based on the current year and a running number.
        // Example: hr250001 for the first homeroom in 2025.
        // lastIdForYear is the latest id we already have for this year.
        private static string GenerateHomeroomId(string? lastIdForYear)
        {
            string yy = DateTime.Now.Year.ToString().Substring(2); // "25" for 2025
            int next = 1;

            if (!string.IsNullOrEmpty(lastIdForYear))
            {
                // lastId looks like "hr250012"
                // "hr" = 0..1, "yy" = 2..3, number starts at 4
                int.TryParse(lastIdForYear.Substring(4), out next);
                next++; // move to next number
            }

            return $"hr{yy}{next:D4}";
        }


        // Index

        [Authorize(Roles = "Admin,Teacher,Doctor,Caregiver,Student")]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["BlockSortParm"] = string.IsNullOrEmpty(sortOrder) ? "block_desc" : "";
            ViewData["TeacherSortParm"] = sortOrder == "Teacher" ? "teacher_desc" : "Teacher";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var query = _context.Homerooms
                .Include(h => h.Teacher)
                .AsNoTracking()
                .AsQueryable();

            // Filter
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var term = searchString.Trim().ToLower();
                query = query.Where(h =>
                    h.Teacher.FirstName.ToLower().Contains(term) ||
                    h.Teacher.LastName.ToLower().Contains(term) ||
                    h.Block.ToString().ToLower().Contains(term));
            }

            // Sort
            query = sortOrder switch
            {
                "block_desc" => query.OrderByDescending(h => h.Block),
                "Teacher" => query.OrderBy(h => h.Teacher.LastName).ThenBy(h => h.Teacher.FirstName),
                "teacher_desc" => query.OrderByDescending(h => h.Teacher.LastName).ThenByDescending(h => h.Teacher.FirstName),
                _ => query.OrderBy(h => h.Block)
            };

            // Paging
            const int pageSize = 5;
            return View(await PaginatedList<Homeroom>.CreateAsync(query, pageNumber ?? 1, pageSize));
        }


        // Details

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var homeroom = await _context.Homerooms
                .Include(h => h.Teacher)
                .FirstOrDefaultAsync(m => m.HomeroomID == id);

            if (homeroom == null) return NotFound();

            return View(homeroom);
        }


        // Create (GET)

        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Create()
        {
            ViewData["TeacherID"] = BuildTeacherSelectList();
            return View();
        }


        // Create (POST)

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([Bind("YearLevel,TeacherID,Block,ClassNumber")] Homeroom homeroom)
        {
            // Unique: one homeroom per teacher
            bool teacherHasHomeroom = await _context.Homerooms.AnyAsync(h => h.TeacherID == homeroom.TeacherID);
            if (teacherHasHomeroom)
                ModelState.AddModelError("TeacherID", "This teacher already has a homeroom.");

            if (ModelState.IsValid)
            {
                // Generate new ID for the current year
                string yy = DateTime.Now.Year.ToString().Substring(2);
                var lastId = await _context.Homerooms
                    .Where(h => h.HomeroomID.StartsWith($"hr{yy}"))
                    .OrderByDescending(h => h.HomeroomID)
                    .Select(h => h.HomeroomID)
                    .FirstOrDefaultAsync();

                homeroom.HomeroomID = GenerateHomeroomId(lastId);

                _context.Homerooms.Add(homeroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Re-render with errors
            ViewData["TeacherID"] = BuildTeacherSelectList(homeroom.TeacherID);
            return View(homeroom);
        }


        // Edit (GET)

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var homeroom = await _context.Homerooms.FindAsync(id);
            if (homeroom == null) return NotFound();

            ViewData["TeacherID"] = BuildTeacherSelectList(homeroom.TeacherID);
            return View(homeroom);
        }


        // Edit (POST)

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(string id, [Bind("HomeroomID,YearLevel,TeacherID,Block,ClassNumber")] Homeroom homeroom)
        {
            if (id != homeroom.HomeroomID) return NotFound();

            // Unique: one homeroom per teacher (exclude this row)
            bool teacherHasAnother =
                await _context.Homerooms.AnyAsync(h => h.TeacherID == homeroom.TeacherID && h.HomeroomID != homeroom.HomeroomID);

            if (teacherHasAnother)
                ModelState.AddModelError("TeacherID", "This teacher already has a homeroom.");

            if (ModelState.IsValid)
            {
                // Straightforward update
                _context.Entry(homeroom).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Re-render with errors
            ViewData["TeacherID"] = BuildTeacherSelectList(homeroom.TeacherID);
            return View(homeroom);
        }


        // Delete (GET/POST)

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var homeroom = await _context.Homerooms
                .Include(h => h.Teacher)
                .FirstOrDefaultAsync(m => m.HomeroomID == id);

            if (homeroom == null) return NotFound();

            return View(homeroom);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var homeroom = await _context.Homerooms.FindAsync(id);
            if (homeroom != null)
            {
                _context.Homerooms.Remove(homeroom);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HomeroomExists(string id) =>
            _context.Homerooms.Any(e => e.HomeroomID == id);
    }
}