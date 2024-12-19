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
    public class AvailablesController : Controller
    {
        private readonly CursachItemsContext _context;

        public AvailablesController(CursachItemsContext context)
        {
            _context = context;
        }

        // GET: Availables
        public async Task<IActionResult> Index()
        {
            return View(await _context.Availables.ToListAsync());
        }

        // GET: Availables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var available = await _context.Availables
                .FirstOrDefaultAsync(m => m.AvailableId == id);
            if (available == null)
            {
                return NotFound();
            }

            return View(available);
        }

        // GET: Availables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Availables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AvailableId,DateListed")] Available available)
        {
            if (ModelState.IsValid)
            {
                _context.Add(available);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(available);
        }

        // GET: Availables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var available = await _context.Availables.FindAsync(id);
            if (available == null)
            {
                return NotFound();
            }
            return View(available);
        }

        // POST: Availables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AvailableId,DateListed")] Available available)
        {
            if (id != available.AvailableId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(available);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvailableExists(available.AvailableId))
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
            return View(available);
        }

        // GET: Availables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var available = await _context.Availables
                .FirstOrDefaultAsync(m => m.AvailableId == id);
            if (available == null)
            {
                return NotFound();
            }

            return View(available);
        }

        // POST: Availables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var available = await _context.Availables.FindAsync(id);
            if (available != null)
            {
                _context.Availables.Remove(available);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvailableExists(int id)
        {
            return _context.Availables.Any(e => e.AvailableId == id);
        }
    }
}
