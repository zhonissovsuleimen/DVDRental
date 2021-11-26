using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DVDRental.Data;
using DVDRental.Models;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            ViewData["movies"] = _context.Movies.ToList();
            List<Copy> copies = await _context.Copies.ToListAsync();
            copies.Sort((a, b) => a.id - b.id);
            return View(copies);
        }

        // GET: Copies/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var copy = await _context.Copies
                .FirstOrDefaultAsync(m => m.id == id);
            if (copy == null)
            {
                return NotFound();
            }
            ViewData["movies"] = _context.Movies.ToList();
            return View(copy);
        }

        // GET: Copies/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["movies"] = _context.Movies.ToList();
            return View();
        }

        // POST: Copies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var copy = await _context.Copies.FindAsync(id);
            if (copy == null)
            {
                return NotFound();
            }
            ViewData["movies"] = _context.Movies.ToList();
            return View(copy);
        }

        // POST: Copies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var copy = await _context.Copies
                .FirstOrDefaultAsync(m => m.id == id);
            if (copy == null)
            {
                return NotFound();
            }
            ViewData["movies"] = _context.Movies.ToList();
            return View(copy);
        }

        // POST: Copies/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var copy = await _context.Copies.FindAsync(id);
            _context.Copies.Remove(copy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CopyExists(int id)
        {
            return _context.Copies.Any(e => e.id == id);
        }
    }
}
