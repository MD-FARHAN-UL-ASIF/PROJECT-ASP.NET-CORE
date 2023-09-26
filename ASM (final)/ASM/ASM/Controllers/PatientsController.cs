using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.EF;
using ASM.Models;
using ASM.Models.DTOs;

namespace ASM.Controllers
{
    public class PatientsController : Controller
    {
        private readonly DataContext _context;

        public PatientsController(DataContext context)
        {
            _context = context;
        }

        // GET: Patients
        [HttpGet]
        [Route("patient/index")]
        public async Task<IActionResult> Index()
        {
              return _context.Patients != null ? 
                          View(await _context.Patients.ToListAsync()) :
                          Problem("Entity set 'DataContext.Patients'  is null.");
        }

        // GET: Patients/Details/5
        [HttpGet]
        [Route("patient/details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        [HttpGet]
        [Route("patient/create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("patient/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,PhoneNumber,DOB,Email,UserName,Password,Gender,Location")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        [HttpGet]
        [Route("patient/edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("patient/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,PhoneNumber,DOB,Email,UserName,Password,Gender,Location")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        [HttpGet]
        [Route("patient/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("patient/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Patients == null)
            {
                return Problem("Entity set 'DataContext.Patients'  is null.");
            }
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("patient/changepassword")]
        public IActionResult ChangePassword()
        {
            var email = HttpContext.Session.GetString("email");

            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();

            return View(patient);
        }

        [HttpPost]
        [Route("patient/changepassword")]
        public IActionResult ChangePassword(PatientDTO dTO)
        {
            var email = HttpContext.Session.GetString("email");

            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();

            patient.Password = dTO.Password;

            _context.Patients.Update(patient);
            _context.SaveChanges();

            return RedirectToAction("Patient", "Dashboard");
        }

        [HttpGet]
        [Route("patient/updateprofile")]
        public IActionResult UpdateProfile()
        {
            var email = HttpContext.Session.GetString("email");

            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();

            return View(patient);
        }

        [HttpPost]
        [Route("patient/updateprofile")]
        public IActionResult UpdateProfile(PatientDTO dTO)
        {
            var email = HttpContext.Session.GetString("email");

            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();

            patient.Name =dTO.Name;
            patient.UserName =dTO.UserName;
            patient.PhoneNumber =dTO.PhoneNumber;
            patient.Location =dTO.Location;
            patient.Email = dTO.Email;
            patient.Description = dTO.Description;

            _context.Patients.Update(patient);
            _context.SaveChanges();

            return RedirectToAction("Patient", "Dashboard");
        }

        private bool PatientExists(int id)
        {
          return (_context.Patients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
