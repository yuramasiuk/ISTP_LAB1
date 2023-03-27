using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostServiceWebApplication.Models;

namespace PostServiceWebApplication.Controllers
{
    public class ParcelHistoriesController : Controller
    {
        private readonly PostServiceContext _context;

        public ParcelHistoriesController(PostServiceContext context)
        {
            _context = context;
        }

        // GET: ParcelHistories
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Clients");
            }
            ViewBag.ParcelId = id;
            var historyOfParcel = _context.ParcelHistories.Where(p => p.ParcelId == id).Include(p => p.Location).Include(p => p.Parcel);
            return View(await historyOfParcel.ToListAsync());
        }

        // GET: ParcelHistories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.ParcelHistories == null)
            {
                return NotFound();
            }

            var parcelHistory = await _context.ParcelHistories
                .Include(p => p.Location)
                .Include(p => p.Parcel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcelHistory == null)
            {
                return NotFound();
            }

            return View(parcelHistory);
        }

        // GET: ParcelHistories/Create
        public IActionResult Create(int id)
        {
            ViewBag.ParcelId = id;
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Address");
            return View();
        }

        // POST: ParcelHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ParcelId, [Bind("LocationId,ArrivalDate")] ParcelHistory parcelHistory)
        {
            parcelHistory.ParcelId = ParcelId;
            if (ModelState.IsValid)
            {
                _context.Add(parcelHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", parcelHistory.LocationId);
            ViewData["ParcelId"] = new SelectList(_context.Parcels, "Id", "Id", parcelHistory.ParcelId);
            return View(parcelHistory);
        }

        // GET: ParcelHistories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.ParcelHistories == null)
            {
                return NotFound();
            }

            var parcelHistory = await _context.ParcelHistories.FindAsync(id);
            if (parcelHistory == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", parcelHistory.LocationId);
            ViewData["ParcelId"] = new SelectList(_context.Parcels, "Id", "Id", parcelHistory.ParcelId);
            return View(parcelHistory);
        }

        // POST: ParcelHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,LocationId,ParcelId,ArrivalDate")] ParcelHistory parcelHistory)
        {
            if (id != parcelHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parcelHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParcelHistoryExists(parcelHistory.Id))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Id", parcelHistory.LocationId);
            ViewData["ParcelId"] = new SelectList(_context.Parcels, "Id", "Id", parcelHistory.ParcelId);
            return View(parcelHistory);
        }

        // GET: ParcelHistories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.ParcelHistories == null)
            {
                return NotFound();
            }

            var parcelHistory = await _context.ParcelHistories
                .Include(p => p.Location)
                .Include(p => p.Parcel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcelHistory == null)
            {
                return NotFound();
            }

            return View(parcelHistory);
        }

        // POST: ParcelHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.ParcelHistories == null)
            {
                return Problem("Entity set 'PostServiceContext.ParcelHistories'  is null.");
            }
            var parcelHistory = await _context.ParcelHistories.FindAsync(id);
            if (parcelHistory != null)
            {
                _context.ParcelHistories.Remove(parcelHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParcelHistoryExists(long id)
        {
          return (_context.ParcelHistories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
