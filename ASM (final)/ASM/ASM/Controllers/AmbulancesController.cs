using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.EF;
using ASM.Models;
using Microsoft.CodeAnalysis;

namespace ASM.Controllers
{
    public class AmbulancesController : Controller
    {
        private readonly DataContext _context;

        public AmbulancesController(DataContext context)
        {
            _context = context;
        }

        // GET: Ambulances
        [HttpGet]
        [Route("ambulance/index")]
        public async Task<IActionResult> Index()
        {
              return _context.Ambulances != null ? 
                          View(await _context.Ambulances.ToListAsync()) :
                          Problem("Entity set 'DataContext.Ambulances'  is null.");
        }

        // GET: Ambulances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ambulances == null)
            {
                return NotFound();
            }

            var ambulance = await _context.Ambulances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ambulance == null)
            {
                return NotFound();
            }

            return View(ambulance);
        }

        // GET: Ambulances/Create
        public IActionResult Create()
        {
            var locations = _context.Locations.ToList();
            ViewBag.LocationList = new SelectList(locations, "Id", "Name");
            return View();
        }

        // POST: Ambulances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LocationId,Rate,LicenceNo")] Ambulance ambulance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ambulance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ambulance);
        }

        // GET: Ambulances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ambulances == null)
            {
                return NotFound();
            }

            var ambulance = await _context.Ambulances.FindAsync(id);
            if (ambulance == null)
            {
                return NotFound();
            }
            return View(ambulance);
        }

        // POST: Ambulances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LocationId,Rate,LicenceNo")] Ambulance ambulance)
        {
            if (id != ambulance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ambulance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmbulanceExists(ambulance.Id))
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
            return View(ambulance);
        }

        // GET: Ambulances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ambulances == null)
            {
                return NotFound();
            }

            var ambulance = await _context.Ambulances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ambulance == null)
            {
                return NotFound();
            }

            return View(ambulance);
        }

        // POST: Ambulances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ambulances == null)
            {
                return Problem("Entity set 'DataContext.Ambulances'  is null.");
            }
            var ambulance = await _context.Ambulances.FindAsync(id);
            if (ambulance != null)
            {
                _context.Ambulances.Remove(ambulance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("ambulance/nearby")]
        public IActionResult Search()
        {
            var email = HttpContext.Session.GetString("email");
            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();

            var loc = Convert.ToInt32(patient.Location);

            var ambulance = _context.Ambulances.Where(x => x.LocationId == loc).ToList();

            return View(ambulance);
        }

        [Route("ambulance/book/{id}")]
        public IActionResult Book(int id)
        {
            var email = HttpContext.Session.GetString("email");
            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();

            var book = new Book
            {
                AmbulanceId = id,
                PatientId = patient.Id
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("Ambulance");
        }

        [HttpGet]
        [Route("ambulance/my")]
        public IActionResult Ambulance()
        {
            var email = HttpContext.Session.GetString("email");
            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();
            var booked = _context.Books.Where(x => x.PatientId == patient.Id).ToList();
            foreach(var item in booked)
            {
                var ambulance = _context.Ambulances.Where(x => x.Id == item.AmbulanceId).ToList();
                return View(ambulance);
            }
            return View();
        }

        private bool AmbulanceExists(int id)
        {
          return (_context.Ambulances?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
