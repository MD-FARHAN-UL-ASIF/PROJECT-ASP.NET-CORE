using HealthRX.EF;
using HealthRX.Models;
using HealthRX.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HealthRX.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly DataContext db;
        private readonly IWebHostEnvironment env;
        public PrescriptionController(DataContext data, IWebHostEnvironment env) 
        {
            db = data;
            this.env = env;
        }

        [HttpGet]
        [Route("prescription/medicine")]
        public IActionResult GetMedicine()
        {
            return View();
        }

        [HttpPost]
        [Route("prescription/medicine")]
        public IActionResult GetMedicine(PrescriptionDTO dto)
        {
            var email = HttpContext.Session.GetString("email");
            var user = db.Users.Where(x => x.Email == email).FirstOrDefault();
            var filename = UploadedFile(dto);
            var prescription = new Prescrition
            {
                PatientId = user.Id,
                PictureLink = filename,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,

            };
            
            db.Prescritions.Add(prescription);
            db.SaveChanges();

            return View();
        }

        [HttpGet]
        [Route("prescription/list")]
        public IActionResult List()
        {
            var prescriptions = db.Prescritions.ToList();
            return View(prescriptions);
        }

        private string UploadedFile(PrescriptionDTO model)
        {
            string uniqueFileName = null;

            if (model.PictureLink != null)
            {
                string uploadsFolder = Path.Combine(env.WebRootPath, "img/med");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PictureLink.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.PictureLink.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
