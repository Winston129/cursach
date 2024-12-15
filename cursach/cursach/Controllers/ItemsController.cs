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
    public class ItemsController : Controller
    {
        private readonly CursachContext _context;

        public ItemsController(CursachContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var cursachContext = _context.Items.Include(i => i.Available).Include(i => i.ItemType).Include(i => i.Reserved).Include(i => i.Sold);
            return View(await cursachContext.ToListAsync());
        }

        // GET: Items/Details?itemName
        public async Task<IActionResult> Details(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                return NotFound("itemName is required.");
            }

            var item = await _context.Items
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(i => i.ItemName == itemName);

            if (item == null)
            {
                return NotFound($"No item found with Item Name: {itemName}");
            }

            return View(item);
        }

        public IActionResult SelectName()
        {
            return View();
        }

        // GET: Items/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var item = await _context.Items
        //        .Include(i => i.Available)
        //        .Include(i => i.ItemType)
        //        .Include(i => i.Reserved)
        //        .Include(i => i.Sold)
        //        .FirstOrDefaultAsync(m => m.ItemId == id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(item);
        //}

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["AvailableId"] = new SelectList(_context.Availables, "AvailableId", "AvailableId");
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "NameType");
            ViewData["ReservedId"] = new SelectList(_context.Reserveds, "ReservedId", "ReservedId");
            ViewData["SoldId"] = new SelectList(_context.Solds, "SoldId", "SoldId");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemName,ItemTypeId,Price,AvailableId,ReservedId,SoldId,Status")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AvailableId"] = new SelectList(_context.Availables, "AvailableId", "AvailableId", item.AvailableId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", item.ItemTypeId);
            ViewData["ReservedId"] = new SelectList(_context.Reserveds, "ReservedId", "ReservedId", item.ReservedId);
            ViewData["SoldId"] = new SelectList(_context.Solds, "SoldId", "SoldId", item.SoldId);

            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["AvailableId"] = new SelectList(_context.Availables, "AvailableId", "AvailableId", item.AvailableId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", item.ItemTypeId);
            ViewData["ReservedId"] = new SelectList(_context.Reserveds, "ReservedId", "ReservedId", item.ReservedId);
            ViewData["SoldId"] = new SelectList(_context.Solds, "SoldId", "SoldId", item.SoldId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemName,ItemTypeId,Price,AvailableId,ReservedId,SoldId,Status")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
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
            ViewData["AvailableId"] = new SelectList(_context.Availables, "AvailableId", "AvailableId", item.AvailableId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", item.ItemTypeId);
            ViewData["ReservedId"] = new SelectList(_context.Reserveds, "ReservedId", "ReservedId", item.ReservedId);
            ViewData["SoldId"] = new SelectList(_context.Solds, "SoldId", "SoldId", item.SoldId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Available)
                .Include(i => i.ItemType)
                .Include(i => i.Reserved)
                .Include(i => i.Sold)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}
