using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;

namespace AvondaleCollegeClinic.Controllers
{
    public class LabtestsController : Controller
    {
        private readonly AvondaleCollegeClinicContext _context;

        public LabtestsController(AvondaleCollegeClinicContext context)
        {
            _context = context;
        }

        // GET: Labtests
        public async Task<IActionResult> Index()
        {
            var avondaleCollegeClinicContext = _context.LabTests.Include(l => l.MedicalRecord);
            return View(await avondaleCollegeClinicContext.ToListAsync());
        }

        // GET: Labtests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labtest = await _context.LabTests
                .Include(l => l.MedicalRecord)
                .FirstOrDefaultAsync(m => m.LabtestID == id);
            if (labtest == null)
            {
                return NotFound();
            }

            return View(labtest);
        }

        // GET: Labtests/Create
        public IActionResult Create()
        {
            ViewBag.RecordID = new SelectList(_context.MedicalRecords
                .Include(m => m.Student)
                .Select(m => new
                {
                    m.MedicalRecordID,
                    FullName = m.Student.FirstName + " " + m.Student.LastName
                }),
                "MedicalRecordID", "FullName");

            return View();
        }

        // POST: Labtests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabtestID,RecordID,TestType,File,ProtectedPDF,ResultDate")] Labtest labtest)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(labtest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecordID"] = new SelectList(_context.MedicalRecords, "MedicalRecordID", "MedicalRecordID", labtest.RecordID);
            return View(labtest);
        }

        // GET: Labtests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labtest = await _context.LabTests.FindAsync(id);
            if (labtest == null)
            {
                return NotFound();
            }
            ViewData["RecordID"] = new SelectList(_context.MedicalRecords, "MedicalRecordID", "MedicalRecordID", labtest.RecordID);
            return View(labtest);
        }

        // POST: Labtests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LabtestID,RecordID,TestType,File,ProtectedPDF,ResultDate")] Labtest labtest)
        {
            if (id != labtest.LabtestID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(labtest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabtestExists(labtest.LabtestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecordID"] = new SelectList(_context.MedicalRecords, "MedicalRecordID", "MedicalRecordID", labtest.RecordID);
            return View(labtest);
        }

        // GET: Labtests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labtest = await _context.LabTests
                .Include(l => l.MedicalRecord)
                .FirstOrDefaultAsync(m => m.LabtestID == id);
            if (labtest == null)
            {
                return NotFound();
            }

            return View(labtest);
        }

        // POST: Labtests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var labtest = await _context.LabTests.FindAsync(id);
            if (labtest != null)
            {
                _context.LabTests.Remove(labtest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabtestExists(int id)
        {
            return _context.LabTests.Any(e => e.LabtestID == id);
        }
    }
}
