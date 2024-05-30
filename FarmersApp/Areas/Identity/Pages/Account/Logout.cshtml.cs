// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using FarmersApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FarmersApp.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly ApplicationDbContext _context;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, ApplicationDbContext context)
        {
            _context = context;
          
            _signInManager = signInManager;
            _logger = logger;
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity != null)
            {
                if (User.IsInRole("Employee"))
                {


                    ViewData["EmployeeID"] = _context.Employees.Where(x => x.Email == User.Identity.Name).Select(x => x.EmployeeId).FirstOrDefault();
                }
                else
                {
                    ViewData["FarmerID"] = _context.Farmers.Where(x => x.Email == User.Identity.Name).Select(x => x.FarmerId).FirstOrDefault();

                }
            }
        }
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
