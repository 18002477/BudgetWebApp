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
    public class BudgetItemsController : Controller
    {
        private readonly BudgetDatabaseContext _context;

        public BudgetItemsController(BudgetDatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                string username = HttpContext.Session.GetString("LoggedInUser");
                return View(_context.BudgetItems.Where(b => b.Username.Equals(username)).ToList());
            }
            else
            {
                TempData["LoginFirst"] = "You need to login first";
                return RedirectToAction("Login", "Login");
            }
        }

        // GET: BudgetItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.HomeLoan = HomeLoanCal();
            ViewBag.Balance = AvailableMoney();
            ViewBag.CarInstallment = CarInstallmentCal();
            ViewBag.Expenses = ExpensesCal();
            ViewBag.LoanApproval = loanApproval();
            if (id == null)
            {
                return NotFound();
            }

            var budgetItems = await _context.BudgetItems
                .Include(b => b.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.BudgetId == id);
            if (budgetItems == null)
            {
                return NotFound();
            }

            return View(budgetItems);
        }

        // GET: BudgetItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BudgetItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int BudgetId, decimal MonthlyIncome, decimal TaxDeducted, decimal Groceries, decimal WaterAndLight, decimal TravelCosts, decimal Cellphone, decimal OtherExpense, decimal Rent, decimal PropyPrice, decimal PropDeposit, int PropInterestRate, int PropRepayMonths, string CarModel, decimal CarPrice, decimal CarDeposit, int CarInterstRate, decimal CarInsurancePrem, string Username)
        {
            string username = HttpContext.Session.GetString("LoggedInUser");
            BudgetItems budget = new BudgetItems()
            {
                BudgetId = BudgetId,
                MonthlyIncome = MonthlyIncome,
                TaxDeducted = TaxDeducted,
                Groceries = Groceries,
                WaterAndLight = WaterAndLight,
                TravelCosts = TravelCosts,
                Cellphone = Cellphone,
                OtherExpense = OtherExpense,
                Rent = Rent,
                PropyPrice = PropyPrice,
                PropDeposit = PropDeposit,
                PropInterestRate = PropInterestRate,
                PropRepayMonths = PropRepayMonths,
                CarModel = CarModel,
                CarPrice = CarPrice,
                CarDeposit = CarDeposit,
                CarInterstRate = CarInterstRate,
                CarInsurancePrem = CarInsurancePrem,
                Username = username
            };
            _context.BudgetItems.Add(budget);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: BudgetItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetItems = await _context.BudgetItems.FindAsync(id);
            if (budgetItems == null)
            {
                return NotFound();
            }
            return View(budgetItems);
        }

        // POST: BudgetItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int BudgetId, decimal MonthlyIncome, decimal TaxDeducted, decimal Groceries, decimal WaterAndLight, decimal TravelCosts, decimal Cellphone, decimal OtherExpense, decimal Rent, decimal PropyPrice, decimal PropDeposit, int PropInterestRate, int PropRepayMonths, string CarModel, decimal CarPrice, decimal CarDeposit, int CarInterstRate, decimal CarInsurancePrem, string Username)
        {
            string username = HttpContext.Session.GetString("LoggedInUser");
            BudgetItems budget = new BudgetItems()
            {
                BudgetId = BudgetId,
                MonthlyIncome = MonthlyIncome,
                TaxDeducted = TaxDeducted,
                Groceries = Groceries,
                WaterAndLight = WaterAndLight,
                TravelCosts = TravelCosts,
                Cellphone = Cellphone,
                OtherExpense = OtherExpense,
                Rent = Rent,
                PropyPrice = PropyPrice,
                PropDeposit = PropDeposit,
                PropInterestRate = PropInterestRate,
                PropRepayMonths = PropRepayMonths,
                CarModel = CarModel,
                CarPrice = CarPrice,
                CarDeposit = CarDeposit,
                CarInterstRate = CarInterstRate,
                CarInsurancePrem = CarInsurancePrem,
                Username = username
            };
            if (BudgetId != budget.BudgetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetItemsExists(budget.BudgetId))
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
            return View(budget);
        }


        // GET: BudgetItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetItems = await _context.BudgetItems
                .Include(b => b.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.BudgetId == id);
            if (budgetItems == null)
            {
                return NotFound();
            }

            return View(budgetItems);
        }

        // POST: BudgetItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budgetItems = await _context.BudgetItems.FindAsync(id);
            _context.BudgetItems.Remove(budgetItems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Method used to retrieve the homeloan calculation
        public decimal HomeLoanCal()
        {
            decimal loan = 0;
            string username = HttpContext.Session.GetString("LoggedInUser");
            BudgetItems budget = _context.BudgetItems.Where(b => b.Username == username).FirstOrDefault();
            loan = Expenses.HomeLoan((decimal)budget.PropyPrice, (decimal)budget.PropDeposit, (int)budget.PropInterestRate, (int)budget.PropRepayMonths);
            return loan;
        }
        // Method used to retrieve the CarInstallment calculation
        public decimal CarInstallmentCal()
        {
            decimal installment = 0;
            string username = HttpContext.Session.GetString("LoggedInUser");
            BudgetItems budget = _context.BudgetItems.Where(b => b.Username == username).FirstOrDefault();
            installment = Expenses.CarInstallment(budget.CarModel, (decimal)budget.CarPrice, (decimal)budget.CarDeposit, (int)budget.CarInterstRate, (decimal)budget.CarInsurancePrem);
            return installment;
        }
        // Method used to retrieve the expense calculation
        public decimal ExpensesCal()
        {
            string username = HttpContext.Session.GetString("LoggedInUser");
            BudgetItems budget = _context.BudgetItems.Where(b => b.Username == username).FirstOrDefault();
            decimal expenditure = 0;
            expenditure = Expenses.Expense((decimal)budget.TaxDeducted, (decimal)budget.Groceries, (decimal)budget.WaterAndLight, (decimal)budget.TravelCosts, (decimal)budget.Cellphone, (decimal)budget.OtherExpense);
            return expenditure;
        }
        // Method used to calculate the avalable balance
        public decimal AvailableMoney()
        {
            string username = HttpContext.Session.GetString("LoggedInUser");
            BudgetItems budget = _context.BudgetItems.Where(b => b.Username == username).FirstOrDefault();
            decimal balance = 0;
            balance = budget.MonthlyIncome - (ExpensesCal() - HomeLoanCal() - CarInstallmentCal());
            return Math.Round(balance, 2);
        }
        // Method used to calculate a third of the user's monthly income
        public decimal loanApproval()
        {
            decimal alert = 0;
            int third = 1 / 3;
            string username = HttpContext.Session.GetString("LoggedInUser");
            BudgetItems budget = _context.BudgetItems.Where(b => b.Username == username).FirstOrDefault();
            alert = budget.MonthlyIncome * third;
            return alert;
        }

        private bool BudgetItemsExists(int id)
        {
            return _context.BudgetItems.Any(e => e.BudgetId == id);
        }
    }
}
