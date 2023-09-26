using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.EF;
using ASM.Models;

namespace ASM.Controllers
{
    public class LocationsController : Controller
    {
        private readonly DataContext _context;

        public LocationsController(DataContext context)
        {
            _context = context;
        }

        // GET: Locations
        [HttpGet]
        [Route("location/index")]
        public async Task<IActionResult> Index()
        {
              return _context.Locations != null ? 
                          View(await _context.Locations.ToListAsync()) :
                          Problem("Entity set 'DataContext.Locations'  is null.");
        }

        // GET: Locations/Details/5
        [HttpGet]
        [Route("location/details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        [HttpGet]
        [Route("location/create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("location/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Code")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Edit/5
        [HttpGet]
        [Route("location/edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
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
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("location/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code")] Location location)
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
            return View(location);
        }

        // GET: Locations/Delete/5
        [HttpGet]
        [Route("location/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("location/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Locations == null)
            {
                return Problem("Entity set 'DataContext.Locations'  is null.");
            }
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
          return (_context.Locations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
