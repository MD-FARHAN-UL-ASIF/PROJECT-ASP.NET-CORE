using ASM.Models.DTOs;
using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.EF;

namespace ASM.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly DataContext _context;

        public DoctorsController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("doctor/index")]
        public IActionResult Index()
        {
            var doctors = _context.Doctors
                .Include(d => d.Category)
                .Include(d => d.Hospital)

                .Select(d => new DoctorDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Designation = d.Designation,
                    Email = d.Email,
                    CatagoryName = d.Category.Name,
                    HospitalName = d.Hospital.Name,
                    LocationName = d.Hospital.Location.Name
                })
                .ToList();

            return View(doctors);
        }


        [HttpGet]
        [Route("doctor/create")]
        public IActionResult Create()
        {
            Doctor Doctor = new Doctor();
            ViewBag.Catagories = GetCatagories();
            ViewBag.Locations = GetLocations();
            return View(Doctor);
        }

        [HttpPost]
        [Route("doctor/create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Doctor Doctor)
        {
            _context.Doctors.Add(Doctor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("doctor/details/{id}")]
        public IActionResult Details(int Id)
        {
            Doctor Doctor = _context.Doctors
                .Where(c => c.Id == Id).FirstOrDefault();
            ViewBag.Catagories = GetCatagories();
            ViewBag.Locations = GetLocations();
            return View(Doctor);
        }

        [HttpGet]
        [Route("doctor/edit/{id}")]
        public IActionResult Edit(int Id)
        {
            Doctor Doctor = _context.Doctors
                .Where(c => c.Id == Id).FirstOrDefault();

            ViewBag.Catagories = GetCatagories();
            ViewBag.Locations = GetLocations();

            return View(Doctor);
        }

        [HttpPost]
        [Route("doctor/edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Doctor Doctor)
        {
            _context.Attach(Doctor);
            _context.Entry(Doctor).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("doctor/delete/{id}")]
        public IActionResult Delete(int Id)
        {
            Doctor Doctor = _context.Doctors
                .Where(c => c.Id == Id).FirstOrDefault();

            return View(Doctor);
        }

        [HttpPost]
        [Route("doctor/delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Doctor Doctor)
        {
            _context.Attach(Doctor);
            _context.Entry(Doctor).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("doctor/list")]
        public IActionResult DoctorsList()
        {
            var doctors = _context.Doctors
                .Include(d => d.Category)
                .Include(d => d.Hospital)

                .Select(d => new DoctorDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Designation = d.Designation,
                    Email = d.Email,
                    CatagoryName = d.Category.Name,
                    HospitalName = d.Hospital.Name,
                    LocationName = d.Hospital.Location.Name
                })
                .ToList();

            return View(doctors);
        }

        [HttpGet]
        [Route("doctor/changepassword")]
        public IActionResult ChangePassword()
        {
            var email = HttpContext.Session.GetString("email");

            var doctor = _context.Doctors.Where(x => x.Email == email).FirstOrDefault();

            return View(doctor);
        }

        [HttpPost]
        [Route("doctor/changepassword")]
        public IActionResult ChangePassword(DoctorDTO dTO)
        {
            var email = HttpContext.Session.GetString("email");

            var doctor = _context.Doctors.Where(x => x.Email == email).FirstOrDefault();

            doctor.Password = dTO.Password;

            _context.Doctors.Update(doctor);
            _context.SaveChanges();

            return RedirectToAction("Doctor", "Dashboard");
        }

        [HttpGet]
        [Route("doctor/updateprofile")]
        public IActionResult UpdateProfile()
        {
            var email = HttpContext.Session.GetString("email");

            var doctor = _context.Doctors.Where(x => x.Email == email).FirstOrDefault();

            var loc = _context.Locations.ToList();
            var hos = _context.Hospitals.ToList();

            var item = new DocLocHos
            {
                Doctor = doctor,
                Locationz = loc,
                Hospitalz = hos
            };

            return View(item);
        }

        [HttpPost]
        [Route("doctor/updateprofile")]
        public IActionResult UpdateProfile(Doctor dTO)
        {
            var email = HttpContext.Session.GetString("email");

            var doctor = _context.Doctors.Where(x => x.Email == email).FirstOrDefault();

            doctor.Name = dTO.Name;
            doctor.UserName = dTO.UserName;
            doctor.Email = dTO.Email;
            doctor.HospitalId = dTO.HospitalId;
            doctor.LoacationId = dTO.LoacationId;

            _context.Doctors.Update(doctor);
            _context.SaveChanges();

            return RedirectToAction("Doctor", "Dashboard");
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
                Text = "----Select Locations----"
            };

            lstLocations.Insert(0, defItem);

            return lstLocations;
        }
        private List<SelectListItem> GetCatagories()
        {
            var lstCatagories = new List<SelectListItem>();

            List<Category> Catagories = _context.Categories.ToList();

            lstCatagories = Catagories.Select(ct => new SelectListItem()
            {
                Value = ct.Id.ToString(),
                Text = ct.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Catagory----"
            };

            lstCatagories.Insert(0, defItem);

            return lstCatagories;
        }
        [HttpGet]
        public JsonResult GetHospitalsByLocation(int locationId)
        {

            List<SelectListItem> hospitals = _context.Hospitals
              .Where(c => c.LocationId == locationId)
              .OrderBy(n => n.Name)
              .Select(n =>
              new SelectListItem
              {
                  Value = n.Id.ToString(),
                  Text = n.Name
              }).ToList();

            return Json(hospitals);

        }
    }
}
