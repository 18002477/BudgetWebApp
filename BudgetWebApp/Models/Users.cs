using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetWebApp.Models
{
    public partial class Users
    {
        public Users()
        {
            BudgetItems = new HashSet<BudgetItems>();
            Savings = new HashSet<Savings>();
        }

        [Required]
        [StringLength(1000, MinimumLength = 6, ErrorMessage = " The Username must be at least have 6 characters")]
        public string Username { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = " The Password must be at least have 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual ICollection<BudgetItems> BudgetItems { get; set; }
        public virtual ICollection<Savings> Savings { get; set; }
    }
}
