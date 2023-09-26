using HealthRX.EF;
using HealthRX.Models;
using HealthRX.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace HealthRX.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly DataContext db;
        public UserController(ILogger<UserController> logger, DataContext data) 
        {
            _logger = logger;
            db = data;
        }

        [HttpGet]
        [Route("user/register")]
        public IActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        [Route("user/register")]
        public IActionResult Register(UserDTO userDTO)
        {
            var user = new User()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                UserName = userDTO.UserName,
                Status = "Active",
                Role = "Customer"
            };
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Route("user/login")]
        public IActionResult Login()
        {
            return View(); 
        }
        [HttpPost]
        [Route("user/login")]
        public IActionResult Login(UserDTO userDTO)
        {
            var user = db.Users
                .Where(x => x.UserName == userDTO.UserName && x.Password == userDTO.Password)
                .FirstOrDefault();
            if(user == null)
            {
                var msg = "Invalid credentials";
                ViewBag.Msg = msg;
                return View();
            }
            else
            {
                if(user.Role == "Admin")
                {
                    HttpContext.Session.SetString("email", user.Email);
                    HttpContext.Session.SetString("username", user.UserName);
                    HttpContext.Session.SetString("role", user.Role);
                    return RedirectToAction("Admin", "Dashboard");
                }
                else
                {
                    HttpContext.Session.SetString("email", user.Email);
                    HttpContext.Session.SetString("username", user.UserName);
                    HttpContext.Session.SetString("role", user.Role);
                    return RedirectToAction("Customer", "Dashboard");
                }
            }
            
        }
        [HttpGet]
        [Route("user/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
        [HttpGet]
        [Route("user/profile")]
        public IActionResult Profile()
        {
            var email = HttpContext.Session.GetString("email");
            var user = db.Users.Where(x =>  x.Email == email).FirstOrDefault();
            if(user != null)
            {
                return View(user);
            }
            return View();
        }

        [HttpPost]
        [Route("user/profile")]
        public IActionResult Update(UserDTO dto)
        {
            var exUser = db.Users.Where(x => x.Email == dto.Email).FirstOrDefault();

            if(exUser != null)
            {
                exUser.Name = dto.Name;
                exUser.Email = dto.Email;
                exUser.UserName = dto.UserName;

                db.Users.Update(exUser);
                db.SaveChanges();
                return RedirectToAction("Profile");
            }

            return RedirectToAction("Profile", new { msg = "Unable to update!"});
        }

        [HttpGet]
        [Route("user/changepassword")]
        public IActionResult ChangePassword()
        {
            var email = HttpContext.Session.GetString("email");

            var user = db.Users.Where(x => x.Email == email).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [Route("user/changepassword")]
        public IActionResult ChangePassword(UserDTO dto)
        {
            var email = HttpContext.Session.GetString("email");
            var user = db.Users.Where(x => x.Email == email).FirstOrDefault();

            if(user != null)
            {
                user.Password = dto.Password;
                db.Users.Update(user);
                db.SaveChanges();
                return RedirectToAction("Admin", "Dashboard"); 
            }
            return View(new { msg = "Unable to Change Password!"});
        }

        [HttpGet]
        [Route("user/customer/profile")]
        public IActionResult CustomerProfile()
        {
            var email = HttpContext.Session.GetString("email");
            var user = db.Users.Where(x => x.Email == email).FirstOrDefault();
            if (user != null)
            {
                return View(user);
            }
            return View();
        }
        [HttpPost]
        [Route("user/customer/profile")]
        public IActionResult CustomerUpdate(UserDTO dto)
        {
            var exUser = db.Users.Where(x => x.Email == dto.Email).FirstOrDefault();

            if (exUser != null)
            {
                exUser.Name = dto.Name;
                exUser.Email = dto.Email;
                exUser.UserName = dto.UserName;

                db.Users.Update(exUser);
                db.SaveChanges();
                return RedirectToAction("CustomerProfile");
            }

            return RedirectToAction("CustomerProfile", new { msg = "Unable to update!" });
        }

        [HttpGet]
        [Route("user/customer/changepassword")]
        public IActionResult CustomerChangePassword()
        {
            var email = HttpContext.Session.GetString("email");

            var user = db.Users.Where(x => x.Email == email).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [Route("user/customer/changepassword")]
        public IActionResult CustomerChangePassword(UserDTO dto)
        {
            var email = HttpContext.Session.GetString("email");
            var user = db.Users.Where(x => x.Email == email).FirstOrDefault();

            if (user != null)
            {
                user.Password = dto.Password;
                db.Users.Update(user);
                db.SaveChanges();
                return RedirectToAction("Customer", "Dashboard");
            }
            return View(new { msg = "Unable to Change Password!" });
        }

    }
}
