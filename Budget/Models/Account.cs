using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Budgeter.Models
{
    public class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
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