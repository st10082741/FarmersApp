using FarmersApp.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmersApp
{
    public class AppFilter:IActionFilter
    {
        private readonly ApplicationDbContext _context;

        public AppFilter(ApplicationDbContext context)
        {
            _context = context;
        }
        public  void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            if (controller != null)
            {
                var userName = context.HttpContext.User.Identity.Name;
                if (userName != null)
                {
                    if (context.HttpContext.User.IsInRole("Employee"))
                    {


                        controller.ViewData["EmployeeID"] = _context.Employees.Where(x => x.Email == context.HttpContext.User.Identity.Name).Select(x => x.EmployeeId).FirstOrDefault();
                    }
                    else
                    {
                        controller.ViewData["FarmerID"] =  _context.Farmers.Where(x => x.Email == context.HttpContext.User.Identity.Name).Select(x => x.FarmerId).FirstOrDefault();

                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after execution
        }

    }
}

