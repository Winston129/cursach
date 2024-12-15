using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cursach.Data;
using cursach.Models;

namespace cursach.Controllers
{
    public class SoldsController : Controller
    {
        private readonly CursachContext _context;

        public SoldsController(CursachContext context)
        {
            _context = context;
        }

        // GET: Solds
        public async Task<IActionResult> Index()
        {
            var cursachContext = _context.Solds.Include(s => s.Client);
            return View(await cursachContext.ToListAsync());
        }

        // GET: Items/Details?itemName
        public async Task<IActionResult> Details(string soldDate)
        {
            if (string.IsNullOrWhiteSpace(soldDate))
            {
                return NotFound("SoldDate is required.");
            }

            DateOnly parsedDate;
            try
            {
                // Преобразуем строку в DateOnly
                parsedDate = DateOnly.Parse(soldDate);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid date format. Please use YYYY-MM-DD.");
            }

            var sold = await _context.Solds
                .Include(r => r.Client)
                .FirstOrDefaultAsync(r => r.SaleDate == parsedDate);

            if (sold == null)
            {
                return NotFound($"No sold item found with date: {soldDate}");
            }

            return View(sold);
        }


        public IActionResult SelectName()
        {
            return View();
        }

        // GET: Solds/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var sold = await _context.Solds
        //        .Include(s => s.Client)
        //        .FirstOrDefaultAsync(m => m.SoldId == id);
        //    if (sold == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(sold);
        //}

        // GET: Solds/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId");
            return View();
        }

        // POST: Solds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SoldId,ClientId,SaleDate")] Sold sold)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sold);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", sold.ClientId);
            return View(sold);
        }

        // GET: Solds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sold = await _context.Solds.FindAsync(id);
            if (sold == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", sold.ClientId);
            return View(sold);
        }

        // POST: Solds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SoldId,ClientId,SaleDate")] Sold sold)
        {
            if (id != sold.SoldId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sold);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoldExists(sold.SoldId))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", sold.ClientId);
            return View(sold);
        }

        // GET: Solds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sold = await _context.Solds
                .Include(s => s.Client)
                .FirstOrDefaultAsync(m => m.SoldId == id);
            if (sold == null)
            {
                return NotFound();
            }

            return View(sold);
        }

        // POST: Solds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sold = await _context.Solds.FindAsync(id);
            if (sold != null)
            {
                _context.Solds.Remove(sold);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoldExists(int id)
        {
            return _context.Solds.Any(e => e.SoldId == id);
        }
    }
}
