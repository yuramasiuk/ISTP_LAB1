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
    public class LocationTypesController : Controller
    {
        private readonly PostServiceContext _context;

        public LocationTypesController(PostServiceContext context)
        {
            _context = context;
        }

        // GET: LocationTypes
        public async Task<IActionResult> Index()
        {
              return _context.LocationTypes != null ? 
                          View(await _context.LocationTypes.ToListAsync()) :
                          Problem("Entity set 'PostServiceContext.LocationTypes'  is null.");
        }

        // GET: LocationTypes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.LocationTypes == null)
            {
                return NotFound();
            }

            var locationType = await _context.LocationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationType == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Locations", new { id = locationType.Id, name = locationType.Name });
        }

        // GET: LocationTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OpenTime,CloseTime,Capacity")] LocationType locationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(locationType);
        }

        // GET: LocationTypes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.LocationTypes == null)
            {
                return NotFound();
            }

            var locationType = await _context.LocationTypes.FindAsync(id);
            if (locationType == null)
            {
                return NotFound();
            }
            return View(locationType);
        }

        // POST: LocationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,OpenTime,CloseTime,Capacity")] LocationType locationType)
        {
            if (id != locationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationTypeExists(locationType.Id))
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
            return View(locationType);
        }

        // GET: LocationTypes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.LocationTypes == null)
            {
                return NotFound();
            }

            var locationType = await _context.LocationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationType == null)
            {
                return NotFound();
            }

            return View(locationType);
        }

        // POST: LocationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.LocationTypes == null)
            {
                return Problem("Entity set 'PostServiceContext.LocationTypes'  is null.");
            }
            var locationType = await _context.LocationTypes.FindAsync(id);
            if (locationType != null)
            {
                _context.LocationTypes.Remove(locationType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationTypeExists(long id)
        {
          return (_context.LocationTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
