using HealthRX.EF;
using HealthRX.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthRX.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly DataContext db;
        public DashboardController(ILogger<DashboardController> logger, DataContext data)
        {
            _logger = logger;
            db = data;
        }

        [HttpGet]
        [Route("dashboard/admin")]
        public IActionResult Admin()
        {
            var prescriptions = db.Prescritions.ToList();
            return View(prescriptions);
        }

        [HttpGet]
        [Route("dashboard/customer")]
        public IActionResult Customer()
        {
            var products = db.Products.ToList();

            var SearchItemId = TempData["SearchItemId"] as int?; // Use int? for nullable int
            var SearchItemName = TempData["SearchItemName"] as string;
            var SearchItemDescription = TempData["SearchItemDescription"] as string;
            var SearchItemCategoryID = TempData["SearchItemCategoryID"] as int?; // Use int? for nullable int
            var SearchItemPriceStr = TempData["SearchItemPrice"] as string; // Store as string
            var SearchItemManufacturer = TempData["SearchItemManufacturer"] as string;
            var SearchItemExpiryDate = TempData["SearchItemExpiryDate"] as DateTime?; // Use DateTime? for nullable DateTime
            var SearchItemExpiryPictureLink = TempData["SearchItemExpiryPictureLink"] as string;
            var searchMessage = TempData["SearchMessage"] as string;

            // Your search criteria and TempData retrieval code here...

            decimal? SearchItemPrice = null; // Initialize as null

            if (!string.IsNullOrEmpty(SearchItemPriceStr) && decimal.TryParse(SearchItemPriceStr, out var parsedPrice))
            {
                SearchItemPrice = parsedPrice;
            }

            var searchItem = new Product
            {
                Id = SearchItemId ?? 0, // Provide a default value if it's null
                Name = SearchItemName,
                Description = SearchItemDescription,
                CategoryID = SearchItemCategoryID ?? 0, // Provide a default value if it's null
                Price = SearchItemPrice ?? 0, // Provide a default value if it's null
                Manufacturer = SearchItemManufacturer,
                ExpiryDate = SearchItemExpiryDate ?? DateTime.MinValue, // Provide a default value if it's null
                PictureLink = SearchItemExpiryPictureLink
            };

            // Initialize ViewBag.Item with an empty Product object if no matching product is found
            ViewBag.Item = searchItem;

            return View(products);
        }
    }
}
