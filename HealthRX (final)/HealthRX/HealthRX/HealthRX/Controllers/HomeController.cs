using HealthRX.EF;
using HealthRX.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HealthRX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext db;

        public HomeController(ILogger<HomeController> logger, DataContext data)
        {
            _logger = logger;
            db = data;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}