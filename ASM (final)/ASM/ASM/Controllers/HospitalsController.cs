using ASM.Models.DTOs;
using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using ASM.EF;

namespace ASM.Controllers
{
    public class HospitalsController : Controller
    {
        private readonly DataContext _context;

        public HospitalsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("hospital/index")]
        public IActionResult Index()
        {
            var hospitals = _context.Hospitals
                .Include(h => h.Location)
                .Select(h => new HospitalDTO
                {
                    Id = h.Id,
                    Code = h.Code,
                    Name = h.Name,
                    HospitalLocation = h.Location != null ? h.Location.Name : string.Empty
                })
                .ToList();

            return View(hospitals);
        }

        [HttpGet]
        [Route("hospital/create")]
        public IActionResult Create()
        {
            //Hospital Hospital = new Hospital();
            ViewBag.Locations = GetLocations();
            //return View(Hospital);
            return View();
        }


        [HttpPost]
        [Route("hospital/create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hospital Hospital)
        {
            _context.Hospitals.Add(Hospital);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        [Route("hospital/details/{id}")]
        public IActionResult Details(int Id)
        {
            Hospital Hospital = _context.Hospitals
              .Where(c => c.Id == Id).FirstOrDefault();
            ViewBag.Locations = GetLocations();
            return View(Hospital);
        }

        [HttpGet]
        [Route("hospital/edit/{id}")]
        public IActionResult Edit(int Id)
        {
            Hospital Hospital = _context.Hospitals
              .Where(c => c.Id == Id).FirstOrDefault();

            ViewBag.Locations = GetLocations();

            return View(Hospital);
        }

        [HttpPost]
        [Route("hospital/edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Hospital Hospital)
        {
            _context.Attach(Hospital);
            _context.Entry(Hospital).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Route("hospital/delete/{id}")]
        public IActionResult Delete(int Id)
        {
            Hospital Hospital = _context.Hospitals
              .Where(c => c.Id == Id).FirstOrDefault();

            return View(Hospital);
        }

        [HttpPost]
        [Route("hospital/delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Hospital Hospital)
        {
            _context.Attach(Hospital);
            _context.Entry(Hospital).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        private List<SelectListItem> GetLocations()
        {
            var lstLocations = new List<SelectListItem>();

            List<Location> Locations = _context.Locations.ToList();

            lstLocations = Locations.Select(ct => new SelectListItem()
            {
                Value = ct.Id.ToString(),
                Text = ct.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Location----"
            };

            lstLocations.Insert(0, defItem);

            return lstLocations;
        }
    }
}
