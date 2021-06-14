using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetClassLib;

namespace BudgetWebApp.Controllers
{
    public class LoginController : Controller
    {
        BudgetDatabaseContext db = new BudgetDatabaseContext();

        public LoginController(BudgetDatabaseContext context) 
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Login(string username, string password)
        {
            Users user = await db.Users.Where(u => u.Username.Equals(username) && u.Password.Equals(Utils.hashPassword(password))).FirstOrDefaultAsync();
            if (user != null)
            {
                HttpContext.Session.SetString("LoggedInUser", user.Username);
                TempData["UsernameAsTempData"] = user.Username;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Incorect Details";
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string Username, string Password)
        {
            Users newUser = new Users();
            newUser.Username = Username;
            newUser.Password = Utils.hashPassword(Password);

            if (ModelState.IsValid)
            {
                db.Users.Add(newUser);
                await db.SaveChangesAsync();
                ViewBag.Success = "User Successfully added !";
                ModelState.Clear();// This will clear whatever form items have been populated
                return View();
            } // Here I'm returning the model as there's an error and the user needs to see what has been entered. 
            return View(newUser);
        }
    }
}
