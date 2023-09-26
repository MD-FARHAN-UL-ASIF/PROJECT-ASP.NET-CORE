using ASM.EF;
using ASM.Models;
using ASM.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ASM.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly DataContext _context;

        public AppointmentsController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("appointment/index")]
        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("email");
            var doctor = _context.Doctors.Where(x => x.Email == email).FirstOrDefault();
            var appointments = _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Hospital)
                .ThenInclude(h => h.Location)
                .Where(x => x.DoctorId == doctor.Id)
                .Select(a => new AppointmentDTO
                {
                    Date = a.Date,
                    LocationName = a.Doctor.Hospital.Location.Name,
                    HospitalName = a.Doctor.Hospital.Name,
                    CategoryName = a.Doctor.Category.Name,
                    DoctorName = a.Doctor.Name,
                    Note = a.Note
                })
                .ToList();

            return View(appointments);
        }

        [HttpGet]
        [Route("patient/appointment")]
        public IActionResult PatientAppointment()
        {
            var email = HttpContext.Session.GetString("email");
            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();
            var appointments = _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Hospital)
                .ThenInclude(h => h.Location)
                .Where(x => x.PatientId == patient.Id)
                .Select(a => new AppointmentDTO
                {
                    Id = a.Id,
                    Date = a.Date,
                    LocationName = a.Doctor.Hospital.Location.Name,
                    HospitalName = a.Doctor.Hospital.Name,
                    CategoryName = a.Doctor.Category.Name,
                    DoctorName = a.Doctor.Name,
                    Note = a.Note
                })
                .ToList();

            return View(appointments);
        }

        [HttpGet]
        [Route("pay/{id}")]
        public IActionResult Pay(int id)
        {
            var doctor = _context.Doctors.Where(x => x.Id == id).FirstOrDefault();
            var email = HttpContext.Session.GetString("email");
            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();
            TempData["doctor"] = JsonConvert.SerializeObject(doctor); 
            TempData["patient"] = JsonConvert.SerializeObject(patient);
            return View();
        }


        [HttpPost]
        [Route("pay/{id}")]
        public IActionResult Pay(int id, Payment payment)
        {
            var doctorJson = TempData["doctor"] as string;
            var patientJson = TempData["patient"] as string;
            var doctor = JsonConvert.DeserializeObject<Doctor>(doctorJson);
            var patient = JsonConvert.DeserializeObject<Patient>(patientJson);

            var pay = new Payment
            {
                TrxId = payment.TrxId,
                PatientId = patient.Id,
                DoctorId = doctor.Id
            };

            _context.Payments.Add(pay);
            _context.SaveChanges();
            
            return RedirectToAction("Thankyou");

        }
        [HttpGet]
        public IActionResult Thankyou()
        {
            return View();
        }

        [HttpGet]
        [Route("appointment/create")]
        public IActionResult Create()
        {
            var email = HttpContext.Session.GetString("email");
            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();


            // Create an instance of the Appointment model
            var appointment = new Appointment
            {
                // Assign patient-related properties as needed
                PatientId = patient.Id,
                // Other properties...
            };

            // Populate location and category dropdowns
            ViewBag.Locations = GetLocations();
            ViewBag.Catagories = GetCatagories();

            return View(appointment);
        }

        [HttpPost]
        [Route("appointment/create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Appointment Appointment)
        {
            _context.Add(Appointment);
            _context.SaveChanges();
            return RedirectToAction("Patient", "Dashboard");
        }

        [Route("appintment/book/{id}")]
        public IActionResult Book(int id) 
        {
            var doctor = _context.Doctors.Where(x => x.Id == id).FirstOrDefault();
            var email = HttpContext.Session.GetString("email");
            var patient = _context.Patients.Where(x => x.Email == email).FirstOrDefault();
            var appintment = new Appointment
            {
                Date = DateTime.Now.Date.AddDays(1),
                LocationId = doctor.LoacationId,
                HospitalId = doctor.HospitalId,
                CatagoryId = doctor.CategoryId,
                DoctorId = doctor.Id,
                Note = "i am sick",
                PatientId = patient.Id
            };

            _context.Appointments.Add(appintment);
            _context.SaveChanges();

            return RedirectToAction("PatientAppointment");
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

        [HttpGet]
        public JsonResult GetHospitalsByLocation(int locationId)
        {
            List<SelectListItem> hospitals = _context.Hospitals
                .Where(c => c.LocationId == locationId)
                .OrderBy(n => n.Name)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
                }).ToList();

            return Json(hospitals);
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
                Text = "----Select Category----"
            };

            lstCatagories.Insert(0, defItem);

            return lstCatagories;
        }

        [HttpGet]
        public JsonResult GetDoctorsByCategoryAndHospital(int categoryId, int hospitalId)
        {
            List<SelectListItem> doctors = _context.Doctors
                .Where(d => d.CategoryId == categoryId && d.HospitalId == hospitalId)
                .OrderBy(d => d.Name)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();

            return Json(doctors);
        }



        [HttpGet]
        [Route("appointment/details/{id}")]
        public IActionResult Details(int id)
        {
            var appointment = _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Hospital)
                .ThenInclude(h => h.Location)
                .FirstOrDefault(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            var appointmentDTO = new AppointmentDTO
            {
                Id = appointment.Id,
                Date = appointment.Date,
                LocationName = appointment.Doctor.Hospital.Location.Name,
                HospitalName = appointment.Doctor.Hospital.Name,
                CategoryName = appointment.Doctor.Category.Name,
                DoctorName = appointment.Doctor.Name,
                Note = appointment.Note
            };

            return View(appointmentDTO);
        }



        [HttpGet]
        [Route("appointment/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var appointment = _context.Appointments.Find(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [Route("appointment/delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var appointment = _context.Appointments.Find(id);

            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(a => a.Id == id);
        }
    }
}
