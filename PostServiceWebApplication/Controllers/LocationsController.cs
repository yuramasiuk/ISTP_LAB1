using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostServiceWebApplication.Models;

namespace PostServiceWebApplication.Controllers
{
    public class LocationsController : Controller
    {
        private readonly PostServiceContext _context;

        public LocationsController(PostServiceContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null || name == null)
            {
                return RedirectToAction("Index", "Clients");
            }
            ViewBag.TypeId = id;
            ViewBag.TypeName = name;
            var locationsByType = _context.Locations.Where(l => l.TypeId == id).Include(l => l.Type);
            return View(await locationsByType.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        public IActionResult Create(int id)
        {
            ViewBag.TypeId = id;
            ViewBag.TypeName = _context.LocationTypes.Where(l => l.Id == id).FirstOrDefault()!.Name;
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int TypeId, [Bind("Address")] Location location)
        {
            location.TypeId = TypeId;
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Locations", new { id = location.TypeId, name = _context.LocationTypes.Where(l => l.Id == location.TypeId).FirstOrDefault()!.Name });
            }
            ViewBag.TypeId = location.TypeId;
            ViewBag.TypeName = _context.LocationTypes.Where(l => l.Id == location.TypeId).FirstOrDefault()!.Name;
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.LocationTypes, "Id", "Id", location.TypeId);
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Address,TypeId")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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
            ViewData["TypeId"] = new SelectList(_context.LocationTypes, "Id", "Id", location.TypeId);
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Locations == null)
            {
                return Problem("Entity set 'PostServiceContext.Locations'  is null.");
            }
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(long id)
        {
          return (_context.Locations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
