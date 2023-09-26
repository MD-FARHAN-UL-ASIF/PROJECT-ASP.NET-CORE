using HealthRX.EF;
using HealthRX.Models;
using HealthRX.Models.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace HealthRX.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext db;
        private readonly ILogger<ProductController> logger;
        private readonly IWebHostEnvironment env;

        public ProductController(DataContext data, ILogger<ProductController> logger, IWebHostEnvironment env)
        {
            db = data;
            this.logger = logger;
            this.env = env;
        }
        [HttpGet]
        [Route("product/create")]
        public IActionResult Create()
        {
            var catagory = db.Catagories.ToList();

            return View(catagory);
        }
        [HttpPost]
        [Route("product/create")]
        public IActionResult Create(ProductDTO dto)
        {
            var catagory = db.Catagories.ToList();

            string fileName = UploadedFile(dto);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryID = dto.CategoryID,
                Price = dto.Price,
                Manufacturer = dto.Manufacturer,
                ExpiryDate = dto.ExpiryDate,
                PictureLink = fileName
            };
            db.Products.Add(product);
            db.SaveChanges();

            return View(catagory);
        }
        [HttpGet]
        [Route("product/list")]
        public IActionResult List()
        {
            var categories = db.Catagories.ToList();
            var products = db.Products.ToList();

            // Create a list to store ProductCategoryDTO objects
            var productCategoryList = new List<ProductCategoryDTO>();

            foreach (var product in products)
            {
                var category = categories.FirstOrDefault(c => c.Id == product.CategoryID);

                var productCategory = new ProductCategoryDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryID = product.CategoryID,
                    Price = product.Price,
                    Manufacturer = product.Manufacturer,
                    ExpiryDate = product.ExpiryDate,
                    PictureLink = product.PictureLink,
                    CatId = category?.Id ?? 0, 
                    CatName = category?.Name ?? "Uncategorized" 
                };

                productCategoryList.Add(productCategory);
            }

            return View(productCategoryList);

        }

        [HttpGet]
        [Route("product/details/{id}")]
        public IActionResult Details(int id)
        {
            var product = db.Products.Find(id);

            var categories = db.Catagories.ToList();

            var categoryDTOs = new List<CatagoryDTO>();

            foreach (var category in categories)
            {
                var catDTO = new CatagoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                };

                categoryDTOs.Add(catDTO);
            }


            var item = new ProductCategoryDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryID = product.CategoryID,
                Price = product.Price,
                Manufacturer = product.Manufacturer,
                ExpiryDate = product.ExpiryDate,
                PictureLink = product.PictureLink,
                CatagoryDTOs = categoryDTOs
            };

            return View(item);
        }

        [HttpPost]
        [Route("product/update")]

        public IActionResult Update(ProductDTO product)
        {
            var exObj = db.Products.Find(product.Id);
            var pic = UploadedFile(product);

            exObj.Name = product.Name;
            exObj.Description = product.Description;
            exObj.CategoryID = product.CategoryID;
            exObj.Price = product.Price;
            exObj.Manufacturer = product.Manufacturer;
            exObj.ExpiryDate = product.ExpiryDate;
            exObj.PictureLink = pic;

            db.Products.Update(exObj);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpPost]
        [Route("product/delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var item = db.Products.Find(id);

            if(item != null)
            {
                db.Products.Remove(item); 
                db.SaveChanges(); 
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        [Route("product/search")]
        public IActionResult Serarch(string Request)
        {
            var item = db.Products.Where(x => x.Name.Contains(Request)).FirstOrDefault();
            
            return View(item);

        }

        [HttpGet]
        [Route("product/productdetails/{id}")]
        public IActionResult ProductDetails(int id)
        {
            var item = db.Products.Find(id);
            return View(item);
        }

        private string UploadedFile(ProductDTO model)
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
