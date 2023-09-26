using Microsoft.AspNetCore.Mvc;

namespace ASM.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        [Route("dashboard/admin")]
        public IActionResult Admin()
        {
            return View();
        }
        [HttpGet]
        [Route("dashboard/doctor")]
        public IActionResult Doctor()
        {
            return View();
        }

        [HttpGet]
        [Route("dashboard/patient")]
        public IActionResult Patient()
        {
            return View();
        }
    }
}
