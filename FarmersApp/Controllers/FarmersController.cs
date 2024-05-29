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
    public class FarmersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarmersController(ApplicationDbContext context)
        {
            _context = context;
        }
     
        // GET: Farmers
        public async Task<IActionResult> Index(int id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Employee")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (User.Identity != null)
            {
                if (User.IsInRole("Employee"))
                {


                    ViewData["EmployeeID"] = await _context.Employees.Where(x => x.Email == User.Identity.Name).Select(x => x.EmployeeId).FirstOrDefaultAsync();
                }
                else
                {
                    ViewData["FarmerID"] = await _context.Farmers.Where(x => x.Email == User.Identity.Name).Select(x => x.FarmerId).FirstOrDefaultAsync();

                }
            }
            var applicationDbContext = _context.Farmers.Include(f => f.Employee).Where(e => e.EmployeeId == id); // Retrieve the list of farmers for a specific employee
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Farmers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Employee")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (id == null || _context.Farmers == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .Include(f => f.Employee)
                .FirstOrDefaultAsync(m => m.FarmerId == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // GET: Farmers/Create
        public IActionResult Create()
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Employee")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            return View();
        }

        // POST: Farmers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Surname,Email,EmployeeId")] Farmer farmer)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Employee")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (ModelState.IsValid)
            {
                farmer.EmployeeId = await _context.Employees.Where(x => x.Email == User.Identity.Name).Select(x => x.EmployeeId).FirstOrDefaultAsync();
                _context.Add(farmer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),new {id = 1});
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", farmer.EmployeeId);
            return View(farmer);
        }

        // GET: Farmers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Employee")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (id == null || _context.Farmers == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", farmer.EmployeeId);
            return View(farmer);
        }

        // POST: Farmers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FarmerId,Name,Surname,Email,EmployeeId")] Farmer farmer)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Employee")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (id != farmer.FarmerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farmer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmerExists(farmer.FarmerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var EmployeeID = await _context.Employees.Where(x => x.Email == User.Identity.Name).Select(x => x.EmployeeId).FirstOrDefaultAsync();

                return RedirectToAction(nameof(Index), new { id = EmployeeID });
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", farmer.EmployeeId);
            return View(farmer);
        }

        // GET: Farmers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Employee")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (id == null || _context.Farmers == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .Include(f => f.Employee)
                .FirstOrDefaultAsync(m => m.EmployeeId == 1);
            if (farmer == null)
            {
                return NotFound();
            }
           
            return View(farmer);
        }

        // POST: Farmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated || (!User.IsInRole("Employee")))
            {

                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            }
            if (_context.Farmers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Farmers'  is null.");
            }
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
            }
            
            await _context.SaveChangesAsync();
            var EmployeeID = await _context.Employees.Where(x => x.Email == User.Identity.Name).Select(x => x.EmployeeId).FirstOrDefaultAsync();

            return RedirectToAction(nameof(Index), new { id = EmployeeID });
        }

        private bool FarmerExists(int id)
        {
          return (_context.Farmers?.Any(e => e.FarmerId == id)).GetValueOrDefault();
        }
    }
}
