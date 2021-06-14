using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetWebApp.Models;
using Microsoft.AspNetCore.Http;
using BudgetClassLib;

namespace BudgetWebApp.Controllers
{
    public class SavingsController : Controller
    {
        private readonly BudgetDatabaseContext _context;

        public SavingsController(BudgetDatabaseContext context)
        {
            _context = context;
        }

        // GET: Savings
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                string username = HttpContext.Session.GetString("LoggedInUser");
                return View(await _context.Savings.Where(s => s.Username.Equals(username)).ToListAsync());
            }
            else
            {
                TempData["LoginFirst"] = "You need to login first";
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: Savings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.MonthlySavings = monthlySavings();
            ViewBag.Interest = interest();
            ViewBag.SavingsValue = savingsValue();
            if (id == null)
            {
                return NotFound();
            }

            var savings = await _context.Savings
                .Include(s => s.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.SavingId == id);
            if (savings == null)
            {
                return NotFound();
            }

            return View(savings);
        }

        // GET: Savings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Savings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int SavingId, decimal TargetAmount, DateTime Date, int NoOfYears, string Reason, string Username)
        {
            string username = HttpContext.Session.GetString("LoggedInUser");
            Savings savings = new Savings()
            {
                SavingId = SavingId,
                TargetAmount = TargetAmount,
                Date = Date,
                NoOfYears = NoOfYears,
                Reason = Reason,
                Username = username
            };
            _context.Savings.Add(savings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Savings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savings = await _context.Savings.FindAsync(id);
            if (savings == null)
            {
                return NotFound();
            }
            return View(savings);
        }

        // POST: Savings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int SavingId, decimal TargetAmount, DateTime Date, int NoOfYears, string Reason, string Username)
        {
            string username = HttpContext.Session.GetString("LoggedInUser");
            Savings savings = new Savings()
            {
                SavingId = SavingId,
                TargetAmount = TargetAmount,
                Date = Date,
                NoOfYears = NoOfYears,
                Reason = Reason,
                Username = username
            };
            if (SavingId != savings.SavingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(savings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SavingsExists(savings.SavingId))
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
            return View(savings);
        }

        // GET: Savings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var savings = await _context.Savings
                .Include(s => s.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.SavingId == id);
            if (savings == null)
            {
                return NotFound();
            }

            return View(savings);
        }

        // POST: Savings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var savings = await _context.Savings.FindAsync(id);
            _context.Savings.Remove(savings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Method used to retrieve the user's monthly savings 
        public decimal monthlySavings()
        {
            decimal save = 0;
            string username = HttpContext.Session.GetString("LoggedInUser");
            Savings savings = _context.Savings.Where(b => b.Username == username).FirstOrDefault();
            save = UserSavings.MonthlySavings((decimal)savings.TargetAmount, (int)savings.NoOfYears);
            return save;
        }
        // Mehod used to retrive the user's future value interest
        public double interest()
        {
            double futureValue = 0;
            string username = HttpContext.Session.GetString("LoggedInUser");
            Savings savings = _context.Savings.Where(b => b.Username == username).FirstOrDefault();
            futureValue = UserSavings.AmountEarned((decimal)savings.TargetAmount, (int)savings.NoOfYears);
            return futureValue;
        }
        // Method used to calculated the total value of the investment
        public decimal savingsValue()
        {
            decimal total = 0;
            string username = HttpContext.Session.GetString("LoggedInUser");
            Savings savings = _context.Savings.Where(b => b.Username == username).FirstOrDefault();
            total = savings.TargetAmount + Convert.ToDecimal(interest());
            return Math.Round(total);
        }

        private bool SavingsExists(int id)
        {
            return _context.Savings.Any(e => e.SavingId == id);
        }
    }
}
