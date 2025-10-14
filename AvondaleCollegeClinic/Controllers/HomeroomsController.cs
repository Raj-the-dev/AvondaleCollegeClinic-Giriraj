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

    public class HomeroomsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public HomeroomsController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,Teacher,Doctor,Caregiver,Student")]
        // GET: Homerooms
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["BlockSortParm"] = String.IsNullOrEmpty(sortOrder) ? "block_desc" : "";
            ViewData["TeacherSortParm"] = sortOrder == "Teacher" ? "teacher_desc" : "Teacher";
            ViewData["CurrentFilter"] = searchString;

            var homerooms = await _context.Homerooms
                .Include(h => h.Teacher)
                .AsNoTracking()
                .ToListAsync();

            // 🔍 Filter
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                homerooms = homerooms.Where(h =>
                    h.Teacher.FirstName.ToLower().Contains(searchString) ||
                    h.Teacher.LastName.ToLower().Contains(searchString) ||
                    h.Block.ToString().ToLower().Contains(searchString)
                ).ToList();
            }

            // ↕ Sort
            switch (sortOrder)
            {
                case "block_desc":
                    homerooms = homerooms.OrderByDescending(h => h.Block).ToList();
                    break;
                case "Teacher":
                    homerooms = homerooms.OrderBy(h => h.Teacher.LastName).ToList();
                    break;
                case "teacher_desc":
                    homerooms = homerooms.OrderByDescending(h => h.Teacher.LastName).ToList();
                    break;
                default:
                    homerooms = homerooms.OrderBy(h => h.Block).ToList();
                    break;
            }

            // 📄 Pagination
            int pageSize = 5;
            int page = pageNumber ?? 1;
            var totalCount = homerooms.Count;
            var pagedHomerooms = homerooms.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var paginatedList = new PaginatedList<Homeroom>(pagedHomerooms, totalCount, page, pageSize);
            return View(paginatedList);
        }
        [Authorize(Roles = "Admin,Teacher")]

        // GET: Homerooms/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeroom = await _context.Homerooms
                .Include(h => h.Teacher)
                .FirstOrDefaultAsync(m => m.HomeroomID == id);
            if (homeroom == null)
            {
                return NotFound();
            }

            return View(homeroom);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Homerooms/Create
        public IActionResult Create()
        {
            ViewData["TeacherID"] = new SelectList(
                _context.Teachers.Select(t => new
                {
                    t.TeacherID,
                    FullName = t.FirstName + " " + t.LastName
                }),
                "TeacherID",
                "FullName"
            );

            return View();
        }


        // POST: Homerooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create([Bind("YearLevel,TeacherID,Block,ClassNumber")] Homeroom homeroom)
        {
            // Simple pre-check against the unique TeacherID constraint
            if (await _context.Homerooms.AnyAsync(h => h.TeacherID == homeroom.TeacherID))
                ModelState.AddModelError("TeacherID", "This teacher already has a homeroom.");

            // (optional) also prevent duplicate class in same year/block combo
            // if (await _context.Homerooms.AnyAsync(h =>
            //     h.YearLevel == homeroom.YearLevel && h.Block == homeroom.Block && h.ClassNumber == homeroom.ClassNumber))
            //     ModelState.AddModelError("ClassNumber", "That class already exists for this year/block.");

            // Generate new id only when we’re going to save
            if (!ModelState.IsValid)
            {
                try
                {
                    string year = DateTime.Now.Year.ToString().Substring(2);
                    var lastId = await _context.Homerooms
                        .Where(h => h.HomeroomID.StartsWith($"hr{year}"))
                        .OrderByDescending(h => h.HomeroomID)
                        .Select(h => h.HomeroomID)
                        .FirstOrDefaultAsync();

                    int nextNumber = 1;
                    if (!string.IsNullOrEmpty(lastId) && int.TryParse(lastId.Substring(4), out var n)) nextNumber = n + 1;

                    homeroom.HomeroomID = $"hr{year}{nextNumber:D4}";

                    _context.Add(homeroom);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    // If DB unique index is hit (race condition), show same friendly error
                    ModelState.AddModelError("TeacherID", "This teacher already has a homeroom.");
                }
            }

            // Rebuild dropdown and return view with errors
            ViewData["TeacherID"] = new SelectList(
                _context.Teachers.Select(t => new { t.TeacherID, FullName = t.FirstName + " " + t.LastName }),
                "TeacherID", "FullName", homeroom.TeacherID);

            return View(homeroom);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Homerooms/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeroom = await _context.Homerooms.FindAsync(id);
            if (homeroom == null)
            {
                return NotFound();
            }
            ViewData["TeacherID"] = new SelectList(
                    _context.Teachers.Select(t => new
                    {
                        t.TeacherID,
                        FullName = t.FirstName + " " + t.LastName
                    }),
                "TeacherID",
                "FullName",
                homeroom.TeacherID
            );
            return View(homeroom);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // POST: Homerooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(string id, [Bind("HomeroomID,YearLevel,TeacherID,Block,ClassNumber")] Homeroom homeroom)
        {
            if (id != homeroom.HomeroomID) return NotFound();

            // Pre-check: same TeacherID on a different row?
            bool duplicateTeacher = await _context.Homerooms
                .AnyAsync(h => h.TeacherID == homeroom.TeacherID && h.HomeroomID != homeroom.HomeroomID);
            if (duplicateTeacher)
                ModelState.AddModelError("TeacherID", "This teacher already has a homeroom.");

            // (optional) prevent duplicate class tuple on other rows
            // bool duplicateClass = await _context.Homerooms.AnyAsync(h =>
            //     h.HomeroomID != homeroom.HomeroomID &&
            //     h.YearLevel == homeroom.YearLevel && h.Block == homeroom.Block && h.ClassNumber == homeroom.ClassNumber);
            // if (duplicateClass)
            //     ModelState.AddModelError("ClassNumber", "That class already exists for this year/block.");

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(homeroom);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("TeacherID", "This teacher already has a homeroom.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Homerooms.Any(e => e.HomeroomID == homeroom.HomeroomID))
                        return NotFound();
                    throw;
                }
            }

            // Rebuild dropdown and return view with errors
            ViewData["TeacherID"] = new SelectList(
                _context.Teachers.Select(t => new { t.TeacherID, FullName = t.FirstName + " " + t.LastName }),
                "TeacherID", "FullName", homeroom.TeacherID);

            return View(homeroom);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Homerooms/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeroom = await _context.Homerooms
                .Include(h => h.Teacher)
                .FirstOrDefaultAsync(m => m.HomeroomID == id);
            if (homeroom == null)
            {
                return NotFound();
            }

            return View(homeroom);
        }
        [Authorize(Roles = "Admin,Teacher")]
        // POST: Homerooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var homeroom = await _context.Homerooms.FindAsync(id);
            if (homeroom != null)
            {
                _context.Homerooms.Remove(homeroom);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeroomExists(string id)
        {
            return _context.Homerooms.Any(e => e.HomeroomID == id);
        }
    }
}
