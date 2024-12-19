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
        private readonly CursachItemsContext _context;

        public ItemsController(CursachItemsContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var cursachItemsContext = _context.Items
                .Include(i => i.ItemType)
                .Include(i => i.Available)
                .Include(i => i.Reserved)
                .Include(i => i.Sold);
            return View(await cursachItemsContext.ToListAsync());
        }

        // GET: SelectName
        public IActionResult SelectName()
        {
            return View();
        }

        // GET: Items/Details/5
        [HttpGet("Items/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Details?itemName
        [HttpGet("Items/Details")]
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


        // GET: Items/Create
        public IActionResult Create()
        {

            ViewData["ItemType"] = new SelectList(_context.ItemTypes, "ItemTypeId", "NameType");

            ViewData["Available"] = new SelectList(_context.Availables, "AvailableId", "DateListed");
            ViewData["Reserved"] = new SelectList(_context.Reserveds, "ReservedId", "ReservedDate");
            ViewData["Sold"] = new SelectList(_context.Solds, "SoldId", "SaleDate");
            ViewData["StatusOptions"] = new SelectList(new List<string> { "Available", "Reserved", "Sold" });

            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemName,ItemTypeId,Price,Status,AvailableId,ReservedId,SoldId")] Item item)
        {

            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ItemType"] = new SelectList(_context.ItemTypes, "ItemTypeId", "NameType", item.ItemTypeId);

            ViewData["Available"] = new SelectList(_context.Availables, "AvailableId", "DateListed", item.AvailableId);
            ViewData["Reserved"] = new SelectList(_context.Reserveds, "ReservedId", "ReservedDate", item.ReservedId);
            ViewData["Sold"] = new SelectList(_context.Solds, "SoldId", "SaleDate", item.SoldId);

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
            ViewData["ItemType"] = new SelectList(_context.ItemTypes, "ItemTypeId", "NameType", item.ItemTypeId);
            ViewData["Available"] = new SelectList(_context.Availables, "AvailableId", "DateListed", item.AvailableId);
            ViewData["Reserved"] = new SelectList(_context.Reserveds, "ReservedId", "ReservedDate", item.ReservedId);
            ViewData["SoldId"] = new SelectList(_context.Solds, "SoldId", "SaleDate", item.SoldId);
            ViewData["StatusOptions"] = new SelectList(new List<string> { "Available", "Reserved", "Sold" });
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemName,ItemTypeId,Price,Status,AvailableId,ReservedId,SoldId")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (item.Status == "Available")
                {
                    item.ReservedId = null;
                    item.SoldId = null;
                }
                else if(item.Status == "Reserved")
                {
                    item.AvailableId = null;
                    item.SoldId = null;
                }
                else if (item.Status == "Sold")
                {
                    item.AvailableId = null;
                    item.ReservedId = null;
                }

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
            ViewData["ItemType"] = new SelectList(_context.ItemTypes, "ItemTypeId", "NameType", item.ItemTypeId);
            ViewData["Available"] = new SelectList(_context.Availables, "AvailableId", "DateListed", item.AvailableId);
            ViewData["Reserved"] = new SelectList(_context.Reserveds, "ReservedId", "ReservedDate", item.ReservedId);
            ViewData["Sold"] = new SelectList(_context.Solds, "SoldId", "SaleDate", item.SoldId);
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
