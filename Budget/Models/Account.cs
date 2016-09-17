using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgeter.Models
{
    public class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }

        public float BalanceAmt()
        {
            return Transactions
                    .Where(t => t.Active)
                    .Where(t => t.Void == false)
                    .Sum(t => t.Amount);
        }

        public float MonthlyExpenses(DateTimeOffset monthDate)
        {
            return Transactions
                    .Where(t => t.Active)
                    .Where(t => t.Void == false)
                    .Where(t => t.Date.Month == monthDate.Month)
                    .Where(t => t.Date.Year == monthDate.Year)
                    .Where(t => t.Amount < 0)
                    .Sum(t => t.Amount);
        }

        public float MonthlyIncome(DateTimeOffset monthDate)
        {
            return Transactions
                    .Where(t => t.Active)
                    .Where(t => t.Void == false)
                    .Where(t => t.Date.Month == monthDate.Month)
                    .Where(t => t.Date.Year == monthDate.Year)
                    .Where(t => t.Amount > 0)
                    .Sum(t => t.Amount);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int HouseholdId { get; set; }
        public string OwnerId { get; set; }
        public float Balance { get; set; }
        public bool Active { get; set; }
        public float ReconciledBalance { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual Household Household { get; set; }
    }
}