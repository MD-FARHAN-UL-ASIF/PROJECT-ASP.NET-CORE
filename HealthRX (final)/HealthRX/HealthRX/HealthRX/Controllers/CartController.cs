using HealthRX.EF;
using HealthRX.Models;
using HealthRX.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace HealthRX.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext db;
        private readonly ILogger<CartController> logger;
        private readonly IWebHostEnvironment env;

        public CartController(DataContext data, ILogger<CartController> logger, IWebHostEnvironment env)
        {
            db = data;
            this.logger = logger;
            this.env = env;
        }

        [Route("cart/add/{id}")]
        public IActionResult AddToCart(int id)
        {
            if (HttpContext.Session.GetString("Cart") == null)
            {
                var product = db.Products.Where(x => x.Id == id).FirstOrDefault();

                List<Cart> cart = new List<Cart>
                {
                    new Cart
                    {
                        Product = product,
                        Quantity = 1
                    }
                };

                var serializedCart = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString("Cart", serializedCart);
            }
            else
            {
                var product = db.Products.Where(x => x.Id == id).FirstOrDefault();

                string cartJson = HttpContext.Session.GetString("Cart");
                List<Cart> cart = JsonConvert.DeserializeObject<List<Cart>>(cartJson);
                int index = IsInCart(id);

                if(index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart = new List<Cart>
                    {
                        new Cart
                        {
                            Product = product,
                            Quantity = 1
                        }
                    };
                }

                var serializedCart = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString("Cart", serializedCart);
            }
            return RedirectToAction("GetCart");
        }

        [Route("cart/remove/{id}")]
        public IActionResult RemoveCart(int id)
        {
            string cartJson = HttpContext.Session.GetString("Cart");
            List<Cart> cart = JsonConvert.DeserializeObject<List<Cart>>(cartJson);

            int index = IsInCart(id);
            cart.RemoveAt(index);
            var serializedCart = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", serializedCart);
            return RedirectToAction("GetCart");
        }

        [HttpGet]
        [Route("cart/getcart")]
        public IActionResult GetCart()
        {
            string cartJson = HttpContext.Session.GetString("Cart");
            List<Cart> cart = JsonConvert.DeserializeObject<List<Cart>>(cartJson);

            return View(cart);
        }

        public int IsInCart(int id)
        {
            string cartJson = HttpContext.Session.GetString("Cart");

            if (!string.IsNullOrEmpty(cartJson))
            {
                List<Cart> cart = JsonConvert.DeserializeObject<List<Cart>>(cartJson);

                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].Product.Id == id)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        [HttpGet]
        [Route("cart/checkout")]
        public IActionResult Checkout() 
        {
            var email = HttpContext.Session.GetString("email");

            var customer = db.Users.Where(x => x.Email == email).FirstOrDefault();

            var orders = db.Orders.Where(x => x.CustomerId == customer.Id).ToList();

            return View(orders);
        }

        [HttpPost]
        [Route("cart/checkout")]
        public IActionResult Checkout(string Address)
        {
            string cartJson = HttpContext.Session.GetString("Cart");
            List<Cart> cart = JsonConvert.DeserializeObject<List<Cart>>(cartJson);
            
            var email = HttpContext.Session.GetString("email");
            
            var customer = db.Users.Where(x => x.Email == email).FirstOrDefault();
            
            // Calculate the total cost
            decimal totalCost = 0;

            foreach (var cartItem in cart)
            {
                totalCost += (cartItem.Product.Price * cartItem.Quantity);
            }

            // Create an order
            var order = new Order
            {
                Address = Address,
                CustomerId = customer.Id, // Replace with the actual customer ID
                OrderDate = DateTime.Now,
                TotalCost = totalCost,
                OrderItems = new List<OrderItem>()
            };

            // Add order items
            foreach (var cartItem in cart)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.Product.Id,
                    Quantity = cartItem.Quantity,
                    ItemCost = cartItem.Product.Price * cartItem.Quantity
                };

                order.OrderItems.Add(orderItem);
            }

            // Save the order to the database (assuming you have a DbContext)
            db.Orders.Add(order);
            db.SaveChanges();


            TempData["Order"] = JsonConvert.SerializeObject(order);


            // Clear the cart
            HttpContext.Session.Remove("Cart");

            // Redirect to a confirmation page or wherever needed
            return RedirectToAction("Pay");
        }


        [HttpGet]
        [Route("pay")]
        public IActionResult Pay()
        {
            return View();
        }

        [HttpPost]
        [Route("pay")]
        public IActionResult Pay(Payment payment)
        {
            var email = HttpContext.Session.GetString("email");

            var customer = db.Users.Where(x => x.Email == email).FirstOrDefault();

            var order = db.Orders.Where(x => x.OrderDate.Date == DateTime.Now.Date && x.CustomerId == customer.Id).FirstOrDefault();

            var pay = new Payment
            {
                ProductId = order.Id,
                CustomerId = customer.Id,
                TrxId = payment.TrxId
            };

            db.Payments.Add(pay);
            db.SaveChanges();

            return RedirectToAction("Thanks");
        }

        [HttpGet]
        [Route("thanks")]
        public IActionResult Thanks()
        {
            return View();
        }

    }
}
