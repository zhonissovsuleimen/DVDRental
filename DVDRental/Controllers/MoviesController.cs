using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DVDRental.Data;
using DVDRental.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace DVDRental.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string sortOrder, string sortItem)
        {
            ViewData["copies"] = _context.Copies.Where(c => c.available).ToList();
            IEnumerable<Movie> movies = await _context.Movies.ToListAsync();
            ViewBag.NextSortOrder = sortOrder == null || sortOrder == "descending" ? "ascending" : "descending";
            ViewBag.TitleSortingSymbol = sortItem == "title" ? ViewBag.NextSortOrder == "ascending" ? "▾" : "▴" : "";
            ViewBag.YearSortingSymbol = sortItem == "year" ? ViewBag.NextSortOrder == "ascending" ? "▾" : "▴" : "";
            ViewBag.PriceSortingSymbol = sortItem == "price" ? ViewBag.NextSortOrder == "ascending" ? "▾" : "▴" : "";
            ViewBag.SortItem = sortItem;
            switch (sortOrder)
            {
                case "descending":
                    switch (sortItem)
                    {
                        case "title":
                            movies = movies.OrderByDescending(p => p.title);
                            break;
                        case "year":
                            movies = movies.OrderByDescending(p => p.year);
                            break;
                        case "price":
                            movies = movies.OrderByDescending(p => p.price);
                            break;
                        default: break;
                    }
                    break;
                case "ascending":
                    switch (sortItem)
                    {
                        case "title":
                            movies = movies.OrderBy(p => p.title);
                            break;
                        case "year":
                            movies = movies.OrderBy(p => p.year);
                            break;
                        case "price":
                            movies = movies.OrderBy(p => p.price);
                            break;
                        default: break;
                    }
                    break;
                default:
                    break;
            }
            return View(movies);
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int tmdbid, double price, [Bind("id, price")] Movie movie)
        {
            movie = new Movie(tmdbid, price);
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("price")] Movie movie)
        {
            if (id != movie.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.id == id);
        }
    }
}
