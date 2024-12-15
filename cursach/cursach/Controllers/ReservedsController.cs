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
    public class ReservedsController : Controller
    {
        private readonly CursachContext _context;

        public ReservedsController(CursachContext context)
        {
            _context = context;
        }

        // GET: Reserveds
        public async Task<IActionResult> Index()
        {
            var cursachContext = _context.Reserveds.Include(r => r.Client);
            return View(await cursachContext.ToListAsync());
        }

        // GET: Items/Details?itemName
        public async Task<IActionResult> Details(string reservedDate)
        {
            if (string.IsNullOrWhiteSpace(reservedDate))
            {
                return NotFound("ReservedDate is required.");
            }

            DateOnly parsedDate;
            try
            {
                // Преобразуем строку в DateOnly
                parsedDate = DateOnly.Parse(reservedDate);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid date format. Please use YYYY-MM-DD.");
            }

            var reserved = await _context.Reserveds
                .Include(r => r.Client)
                .FirstOrDefaultAsync(r => r.ReservedDate == parsedDate);

            if (reserved == null)
            {
                return NotFound($"No reserved item found with date: {reservedDate}");
            }

            return View(reserved);
        }


        public IActionResult SelectName()
        {
            return View();
        }

        // GET: Reserveds/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var reserved = await _context.Reserveds
        //        .Include(r => r.Client)
        //        .FirstOrDefaultAsync(m => m.ReservedId == id);
        //    if (reserved == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(reserved);
        //}

        // GET: Reserveds/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId");
            return View();
        }

        // POST: Reserveds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservedId,ClientId,ReservedDate,ExpirationDate,InterestRate")] Reserved reserved)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserved);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", reserved.ClientId);
            return View(reserved);
        }

        // GET: Reserveds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserved = await _context.Reserveds.FindAsync(id);
            if (reserved == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", reserved.ClientId);
            return View(reserved);
        }

        // POST: Reserveds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservedId,ClientId,ReservedDate,ExpirationDate,InterestRate")] Reserved reserved)
        {
            if (id != reserved.ReservedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservedExists(reserved.ReservedId))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "ClientId", reserved.ClientId);
            return View(reserved);
        }

        // GET: Reserveds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserved = await _context.Reserveds
                .Include(r => r.Client)
                .FirstOrDefaultAsync(m => m.ReservedId == id);
            if (reserved == null)
            {
                return NotFound();
            }

            return View(reserved);
        }

        // POST: Reserveds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserved = await _context.Reserveds.FindAsync(id);
            if (reserved != null)
            {
                _context.Reserveds.Remove(reserved);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservedExists(int id)
        {
            return _context.Reserveds.Any(e => e.ReservedId == id);
        }
    }
}
