﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using aspNetAuth.Models;
using System.Text;
using aspNetAuth.Services;
using Microsoft.EntityFrameworkCore;

namespace aspNetAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user) {
            if (ModelState.IsValid) {
                // Manual Auth is custom class to hold hash methods
                user.Password = ManualAuth.Sha256(user.Password);
                // Add user and save changes to database.
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Welcome));
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user) {

            if (ModelState.IsValid) {
                // attempt to get a user with the matching username from DB.
                User GetUser = await _context.Users.SingleOrDefaultAsync(u => u.UserName == user.UserName);
                // if no match on username skip password check.
                if (GetUser != null) {
                    // compare hashed passwords.
                    if (ManualAuth.Sha256Check(user.Password, GetUser.Password)) {
                        // if password match is true return treats.
                        return View("Treats");
                    }
                }
            }
            return View("LoginFail");
        }

        public IActionResult Welcome() {
            return View();
        }

        public ViewResult Login() {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
