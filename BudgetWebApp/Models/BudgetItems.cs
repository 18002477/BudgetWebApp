using System;
using System.Collections.Generic;

namespace BudgetWebApp.Models
{
    public partial class BudgetItems
    {
        public int BudgetId { get; set; }
        public decimal MonthlyIncome { get; set; }
        public decimal TaxDeducted { get; set; }
        public decimal Groceries { get; set; }
        public decimal WaterAndLight { get; set; }
        public decimal TravelCosts { get; set; }
        public decimal Cellphone { get; set; }
        public decimal OtherExpense { get; set; }
        public decimal? Rent { get; set; }
        public decimal? PropyPrice { get; set; }
        public decimal? PropDeposit { get; set; }
        public int? PropInterestRate { get; set; }
        public int? PropRepayMonths { get; set; }
        public string CarModel { get; set; }
        public decimal? CarPrice { get; set; }
        public decimal? CarDeposit { get; set; }
        public int? CarInterstRate { get; set; }
        public decimal? CarInsurancePrem { get; set; }
        public string Username { get; set; }

        public virtual Users UsernameNavigation { get; set; }
    }
}
