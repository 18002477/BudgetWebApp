using System;
using System.Collections.Generic;

namespace BudgetWebApp.Models
{
    public partial class Savings
    {
        public int SavingId { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime Date { get; set; }
        public int NoOfYears { get; set; }
        public string Reason { get; set; }
        public string Username { get; set; }

        public virtual Users UsernameNavigation { get; set; }
    }
}
