using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FarmersApp.Models;

namespace FarmersApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
         public async Task<IActionResult> FarmerProducts(int FarmerId, int CategoryId, DateTime startDate, DateTime endDate)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Employee")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "Name");
            var applicationDbContext = _context.Products.Include(p => p.Category).Include(p => p.FarmerNavigation).Where(e => e.FarmerNavigation.FarmerId == FarmerId && e.CategoryId == CategoryId && (e.ProductionDate >= startDate && e.ProductionDate <= endDate ));
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Products
        public async Task<IActionResult> Index(int id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Farmer")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
           
            var applicationDbContext = _context.Products.Include(p => p.Category).Include(p => p.FarmerNavigation).Where(e => e.FarmerNavigation.FarmerId == id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Farmer")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.FarmerNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Farmer")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ProductionDate,CategoryId")] Product product)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Farmer")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (ModelState.IsValid)
            {
                var FarmerID = await _context.Farmers.Where(x => x.Email == User.Identity.Name).Select(x => x.FarmerId).FirstOrDefaultAsync();

                product.Farmer = FarmerID;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = FarmerID });
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
           
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Farmer")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,ProductionDate,CategoryId,Farmer")] Product product)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Farmer")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var FarmerID = await _context.Farmers.Where(x => x.Email == User.Identity.Name).Select(x => x.FarmerId).FirstOrDefaultAsync();

                return RedirectToAction(nameof(Index),new {id = FarmerID});
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["Farmer"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId", product.Farmer);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Farmer")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.FarmerNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Farmer")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            var FarmerID = await _context.Farmers.Where(x => x.Email == User.Identity.Name).Select(x => x.FarmerId).FirstOrDefaultAsync();

         
            return RedirectToAction(nameof(Index), new { id = FarmerID });
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
