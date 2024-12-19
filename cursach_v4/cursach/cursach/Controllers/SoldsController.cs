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
        private readonly CursachItemsContext _context;

        public SoldsController(CursachItemsContext context)
        {
            _context = context;
        }

        // GET: Solds
        public async Task<IActionResult> Index()
        {
            var cursachItemsContext = _context.Solds.Include(s => s.Client);
            return View(await cursachItemsContext.ToListAsync());
        }

        // GET: SelectName
        public IActionResult SelectName()
        {
            return View();
        }

        // GET: Solds/Details/5
        [HttpGet("Solds/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
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

        // POST: Solds/Details?
        [HttpGet("Solds/Details")]
        public async Task<IActionResult> Details(DateOnly saleDate)
        {
            if (saleDate == DateOnly.MinValue)
            {
                return NotFound("itemName is required.");
            }

            var sold = await _context.Solds
                .Include(r => r.Client)
                .FirstOrDefaultAsync(m => m.SaleDate == saleDate);

            if (sold == null)
            {
                return NotFound($"No item found with Item Name: {sold}");
            }

            return View(sold);
        }

        // GET: Solds/Create
        public IActionResult Create()
        {
            ViewData["Client"] = new SelectList(_context.Clients, "ClientId", "LastName");
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
            ViewData["Client"] = new SelectList(_context.Clients, "ClientId", "LastName", sold.ClientId);
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
