using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthRX.EF;
using HealthRX.Models;
using HealthRX.Models.DTOs;
using Newtonsoft.Json;
using System.Numerics;

namespace HealthRX.Controllers
{
    public class CatagoriesController : Controller
    {
        private readonly DataContext _context;

        public CatagoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: Catagories
        [HttpGet]
        [Route("category/index")]
        public async Task<IActionResult> Index()
        {
              return _context.Catagories != null ? 
                          View(await _context.Catagories.ToListAsync()) :
                          Problem("Entity set 'DataContext.Catagories'  is null.");
        }

        // GET: Catagories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Catagories == null)
            {
                return NotFound();
            }

            var catagory = await _context.Catagories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catagory == null)
            {
                return NotFound();
            }

            return View(catagory);
        }

        // GET: Catagories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catagories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Catagory catagory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catagory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(catagory);
        }

        // GET: Catagories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Catagories == null)
            {
                return NotFound();
            }

            var catagory = await _context.Catagories.FindAsync(id);
            if (catagory == null)
            {
                return NotFound();
            }
            return View(catagory);
        }

        // POST: Catagories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Catagory catagory)
        {
            if (id != catagory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catagory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatagoryExists(catagory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(catagory);
        }

        // GET: Catagories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Catagories == null)
            {
                return NotFound();
            }

            var catagory = await _context.Catagories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catagory == null)
            {
                return NotFound();
            }

            return View(catagory);
        }

        // POST: Catagories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Catagories == null)
            {
                return Problem("Entity set 'DataContext.Catagories'  is null.");
            }
            var catagory = await _context.Catagories.FindAsync(id);
            if (catagory != null)
            {
                _context.Catagories.Remove(catagory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("category/search")]
        public IActionResult Search()
        {
            var categories = _context.Catagories.ToList();
            return View(categories);
        }

        [HttpPost]
        [Route("category/search")]
        public IActionResult Search(CatagoryDTO dTO)
        {
            var item = _context.Products.Where(x => x.CategoryID == dTO.Id).ToList();
            TempData["items"] = JsonConvert.SerializeObject(item);
            return RedirectToAction("Products");
        }

        [HttpGet]
        [Route("category/products")]
        public IActionResult Products()
        {
            var jsonProducts = TempData["items"] as string;
            var products = JsonConvert.DeserializeObject<List<Product>>(jsonProducts);
            return View(products);
        }

        private bool CatagoryExists(int id)
        {
          return (_context.Catagories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
