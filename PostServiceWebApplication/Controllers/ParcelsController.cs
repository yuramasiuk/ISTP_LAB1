using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostServiceWebApplication.Models;

namespace PostServiceWebApplication.Controllers
{
    public class ParcelsController : Controller
    {
        private readonly PostServiceContext _context;

        public ParcelsController(PostServiceContext context)
        {
            _context = context;
        }

        // GET: Parcels
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null || name == null)
            {
                return RedirectToAction("Index", "Clients");
            }
            ViewBag.ClientId = id;
            ViewBag.ClientName = name;
            var parcelsByClient = _context.Parcels
                .Where(p => p.ClientFromId == id)
                .Where(p => p.IsDeleted == false)
                .Include(p => p.ClientFrom)
                .Include(p => p.ClientTo)
                .Include(p => p.Status)
                .Include(p => p.Type);
            return View(await parcelsByClient.ToListAsync());
        }

        // GET: Parcels/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Parcels == null)
            {
                return NotFound();
            }

            var parcel = await _context.Parcels
                .Include(p => p.ClientFrom)
                .Include(p => p.ClientTo)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcel == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "ParcelHistories", new { id = parcel.Id});
        }

        // GET: Parcels/Create
        public IActionResult Create(int id)
        {
            ViewBag.ClientId = id;
            ViewBag.ClientName = _context.Clients.Where(a => a.Id == id).FirstOrDefault()!.Name;
            ViewData["ClientToId"] = new SelectList(_context.Clients.Where(c => c.Id != id), "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            ViewData["TypeId"] = new SelectList(_context.ParcelTypes, "Id", "Id");
            return View();
        }

        // POST: Parcels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ClientFromId, [Bind("ClientToId,TypeId,StatusId,IsFragile")] Parcel parcel)
        {
            parcel.ClientFromId = ClientFromId;
            if (ModelState.IsValid && parcel.ClientFromId != parcel.ClientToId)
            {
                _context.Add(parcel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Parcels", new { id = ClientFromId, name = _context.Clients.Where(a => a.Id == ClientFromId).FirstOrDefault()!.Name });
            }
            ViewBag.ClientId = parcel.ClientFromId;
            ViewBag.ClientName = _context.Clients.Where(a => a.Id == parcel.ClientFromId).FirstOrDefault()!.Name;
            ViewData["ClientToId"] = new SelectList(_context.Clients, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            ViewData["TypeId"] = new SelectList(_context.ParcelTypes, "Id", "Id");
            return View(parcel);
        }

        // GET: Parcels/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Parcels == null)
            {
                return NotFound();
            }

            var parcel = await _context.Parcels.FindAsync(id);
            if (parcel == null)
            {
                return NotFound();
            }
            ViewData["ClientFromId"] = new SelectList(_context.Clients, "Id", "Id", parcel.ClientFromId);
            ViewData["ClientToId"] = new SelectList(_context.Clients, "Id", "Id", parcel.ClientToId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", parcel.StatusId);
            ViewData["TypeId"] = new SelectList(_context.ParcelTypes, "Id", "Id", parcel.TypeId);
            return View(parcel);
        }

        // POST: Parcels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ClientFromId,ClientToId,TypeId,StatusId,IsFragile")] Parcel parcel)
        {
            if (id != parcel.Id)
            {
                return NotFound();
            }

            parcel.IsDeleted = false;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parcel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParcelExists(parcel.Id))
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
            ViewData["ClientFromId"] = new SelectList(_context.Clients, "Id", "Id", parcel.ClientFromId);
            ViewData["ClientToId"] = new SelectList(_context.Clients, "Id", "Id", parcel.ClientToId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", parcel.StatusId);
            ViewData["TypeId"] = new SelectList(_context.ParcelTypes, "Id", "Id", parcel.TypeId);
            return View(parcel);
        }

        // GET: Parcels/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Parcels == null)
            {
                return NotFound();
            }

            var parcel = await _context.Parcels
                .Include(p => p.ClientFrom)
                .Include(p => p.ClientTo)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcel == null)
            {
                return NotFound();
            }

            return View(parcel);
        }

        // POST: Parcels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Parcels == null)
            {
                return Problem("Entity set 'PostServiceContext.Parcels'  is null.");
            }
            var parcel = await _context.Parcels.FindAsync(id);
            if (parcel != null)
            {
                parcel.IsDeleted = true;
                _context.Update(parcel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParcelExists(long id)
        {
          return (_context.Parcels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
