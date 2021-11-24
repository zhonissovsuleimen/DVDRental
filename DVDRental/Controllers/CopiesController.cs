using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DVDRental.Data;
using DVDRental.Models;

namespace DVDRental.Controllers
{
    public class CopiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CopiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Copies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Copy.ToListAsync());
        }

        // GET: Copies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var copy = await _context.Copy
                .FirstOrDefaultAsync(m => m.id == id);
            if (copy == null)
            {
                return NotFound();
            }

            return View(copy);
        }

        // GET: Copies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Copies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,available,movieId")] Copy copy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(copy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(copy);
        }

        // GET: Copies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var copy = await _context.Copy.FindAsync(id);
            if (copy == null)
            {
                return NotFound();
            }
            return View(copy);
        }

        // POST: Copies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,available,movieId")] Copy copy)
        {
            if (id != copy.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(copy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CopyExists(copy.id))
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
            return View(copy);
        }

        // GET: Copies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var copy = await _context.Copy
                .FirstOrDefaultAsync(m => m.id == id);
            if (copy == null)
            {
                return NotFound();
            }

            return View(copy);
        }

        // POST: Copies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var copy = await _context.Copy.FindAsync(id);
            _context.Copy.Remove(copy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CopyExists(int id)
        {
            return _context.Copy.Any(e => e.id == id);
        }
    }
}
