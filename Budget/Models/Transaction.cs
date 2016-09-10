using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace Budgeter.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public bool Reconciled { get; set; }
        public float ReconciledAmount { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yy, h:mm tt}")]
        public DateTimeOffset Date { get; set; }

        public bool Void { get; set; }

        public float Amount { get; set; }
        public bool Active { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Account Account { get; set; }
        public virtual Category Category { get; set; }
    }
}