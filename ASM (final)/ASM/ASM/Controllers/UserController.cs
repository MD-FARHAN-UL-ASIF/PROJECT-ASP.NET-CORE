using ASM.EF;
using ASM.Models;
using ASM.Models.DTOs;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ASM.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext db;
        private readonly ILogger<UserController> _logger;
        public UserController(DataContext data, ILogger<UserController> logger)
        {
            db = data;
            _logger = logger;
        }

        [HttpGet]
        [Route("patient/register")]
        public IActionResult Register()
        {
            var locations = db.Locations.ToList();
            return View(locations);
        }
        [HttpPost]
        [Route("patient/register")]
        public IActionResult Register(PatientDTO dTO)
        {
            var patient = new Patient()
            {
                Name = dTO.Name,
                Description = dTO.Description,
                PhoneNumber = dTO.PhoneNumber,
                DOB = dTO.DOB,
                Email = dTO.Email,
                UserName = dTO.UserName,
                Password = dTO.Password,
                Location = dTO.Location,
                Gender = dTO.Gender,
            };
            db.Patients.Add(patient);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginDTO dTO)
        {
            var patient = db.Patients.FirstOrDefault(p => p.UserName == dTO.UserName && p.Password == dTO.Password);
            var doctor = db.Doctors.FirstOrDefault(d => d.UserName == dTO.UserName && d.Password == dTO.Password);
            var admin = db.Admins.FirstOrDefault(a => a.UserName == dTO.UserName && a.Password == dTO.Password);

            if (patient != null)
            {
                HttpContext.Session.SetString("email", patient.Email);
                HttpContext.Session.SetString("username", patient.UserName);
                return RedirectToAction("Patient", "Dashboard");
            }
            else if (doctor != null)
            {
                HttpContext.Session.SetString("email", doctor.Email);
                HttpContext.Session.SetString("username", doctor.UserName);
                return RedirectToAction("Doctor", "Dashboard");
            }
            else if (admin != null)
            {
                HttpContext.Session.SetString("email", admin.Email);
                HttpContext.Session.SetString("username", admin.UserName);
                return RedirectToAction("Admin", "Dashboard");
            }
            else
            {
                var msg = "Invalid credentials!";
                return View(msg);
            }
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
        [HttpGet]
        [Route("search/doctor")]
        public IActionResult Search()
        {
            var category = db.Categories.ToList();
            return View(category);
        }
        [HttpPost]
        [Route("search/doctor")]
        public IActionResult Search(SearchDoctorDTO dTO)
        {
            var email = HttpContext.Session.GetString("email");
            var patient = db.Patients.Where(x => x.Email == email).FirstOrDefault();
            var location = Convert.ToInt32(patient.Location);

            var doctor = db.Doctors
                .Include(d => d.Category)
                .Include(d => d.Hospital)
                .Where(x => x.LoacationId == location && x.CategoryId == dTO.CategoryId)
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

            TempData["doc"] = JsonConvert.SerializeObject(doctor);

            return RedirectToAction("SearchResult");
        }
        [HttpGet]
        [Route("search/doctor/result")]
        public IActionResult SearchResult()
        {
            var doctorJson = TempData["doc"] as string;
            var doctor = JsonConvert.DeserializeObject<List<DoctorDTO>>(doctorJson);

            return View(doctor);
        }
    }
}
