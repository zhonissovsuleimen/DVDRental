using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DVDRental.Data;
using DVDRental.Models;
using Microsoft.AspNetCore.Identity;

namespace DVDRental.Controllers
{
    public class RentalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index()
        {
            ViewData["users"] = _context.Users.ToList();
            ViewData["movies"] = _context.Movies.ToList();
            ViewData["copies"] = _context.Copies.ToList();
            return View(await _context.Rentals.ToListAsync());
        }

        // GET: History
        public async Task<IActionResult> History()
        {
            ViewData["movies"] = _context.Movies.ToList();
            ViewData["copies"] = _context.Copies.ToList();
            string userId = _context.Users.Where(u => u.UserName == User.Identity.Name).First().Id;
            var rentals = await _context.Rentals.Where(r => r.userId == userId).ToListAsync();
            return View(rentals);
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .FirstOrDefaultAsync(m => m.id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["users"] = _context.Users.ToList();
            var copies = _context.Copies.Where(c => c.available).ToList();
            copies.Sort((a, b) => a.id - b.id);
            ViewData["copies"] = copies;
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int copyId, string userId,[Bind("id")] Rental rental)
        {
            rental.copyId = copyId;
            rental.userId = userId;
            rental.rentDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(rental);
                _context.Copies.Find(copyId).available = false;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["copies"] = _context.Copies.ToList();
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,userId,copyId,rentDate,returnDate")] Rental rental)
        {
            if (id != rental.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(rental.returnDate != null) 
                    {
                        _context.Copies.Find(rental.copyId).available = true;
                    }
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.id))
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
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .FirstOrDefaultAsync(m => m.id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.id == id);
        }
    }
}
