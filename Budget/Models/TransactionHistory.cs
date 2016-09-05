using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace Budgeter.Models
{
    public class TransactionHistory
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string UserId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTimeOffset ChangedDate { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}