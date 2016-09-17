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
    public class Household
    {
        public Household()
        {
            Accounts = new HashSet<Account>();
            Budgets = new HashSet<Budgets>();
            Users = new HashSet<ApplicationUser>();
            Invites = new HashSet<Invite>();
            Categories = new HashSet<Category>();
        }

        public float MonthlyExpenses(DateTimeOffset monthDate)
        {
            return Accounts.Where(a => a.Active).Sum(a => a.MonthlyExpenses(monthDate));
        }

        public float MonthlyIncome(DateTimeOffset monthDate)
        {
            return Accounts.Where(a => a.Active).Sum(a => a.MonthlyIncome(monthDate));
        }

        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Owner")]
        public string OwnerId { get; set; }
        public bool Active { get; set; }
        public bool Demo { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<Account> Accounts {get; set; }
        public virtual ICollection<Budgets> Budgets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}