using Microsoft.AspNetCore.Mvc;

namespace ASM.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
