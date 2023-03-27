using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostServiceWebApplication.Models;

namespace PostServiceWebApplication.Controllers
{
    public class ParcelTypesController : Controller
    {
        private readonly PostServiceContext _context;

        public ParcelTypesController(PostServiceContext context)
        {
            _context = context;
        }

        // GET: ParcelTypes
        public async Task<IActionResult> Index()
        {
              return _context.ParcelTypes != null ? 
                          View(await _context.ParcelTypes.ToListAsync()) :
                          Problem("Entity set 'PostServiceContext.ParcelTypes'  is null.");
        }

        // GET: ParcelTypes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.ParcelTypes == null)
            {
                return NotFound();
            }

            var parcelType = await _context.ParcelTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcelType == null)
            {
                return NotFound();
            }

            return View(parcelType);
        }

        // GET: ParcelTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParcelTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Length,Width,Height,ShipmentCost")] ParcelType parcelType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parcelType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parcelType);
        }

        // GET: ParcelTypes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.ParcelTypes == null)
            {
                return NotFound();
            }

            var parcelType = await _context.ParcelTypes.FindAsync(id);
            if (parcelType == null)
            {
                return NotFound();
            }
            return View(parcelType);
        }

        // POST: ParcelTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Length,Width,Height,ShipmentCost")] ParcelType parcelType)
        {
            if (id != parcelType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parcelType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParcelTypeExists(parcelType.Id))
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
            return View(parcelType);
        }

        // GET: ParcelTypes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.ParcelTypes == null)
            {
                return NotFound();
            }

            var parcelType = await _context.ParcelTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcelType == null)
            {
                return NotFound();
            }

            return View(parcelType);
        }

        // POST: ParcelTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.ParcelTypes == null)
            {
                return Problem("Entity set 'PostServiceContext.ParcelTypes'  is null.");
            }
            var parcelType = await _context.ParcelTypes.FindAsync(id);
            if (parcelType != null)
            {
                _context.ParcelTypes.Remove(parcelType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParcelTypeExists(long id)
        {
          return (_context.ParcelTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
