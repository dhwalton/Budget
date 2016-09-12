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
    public class BudgetViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public BudgetViewModel(int Id)
        {
            Budget = db.Budgets.Find(Id);
            IncomeBudgetItems = Budget.BudgetItems.Where(i => i.Amount > 0).ToList();
            ExpenseBudgetItems = Budget.BudgetItems.Where(i => i.Amount <= 0).ToList();
            IncomeCategories = db.Categories.Where(c => c.IsDeposit).ToList();
            ExpenseCategories = db.Categories.Where(c => c.IsDeposit == false).ToList();
        }
        public Budgets Budget { get; set; }
        public ICollection<Category> IncomeCategories { get; set; }
        public ICollection<Category> ExpenseCategories { get; set; }
        public ICollection<BudgetItem> IncomeBudgetItems { get; set; }
        public ICollection<BudgetItem> ExpenseBudgetItems { get; set; }
    }
}