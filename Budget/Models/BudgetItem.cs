using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace Budgeter.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Frequency { get; set; }
        public int CategoryId { get; set; }
        public int BudgetId { get; set; }
        public float Amount { get; set; }
        public virtual Budgets Budget { get; set; }
        public virtual Category Category { get; set; }
    }
}