using System.Collections.Generic;

namespace Budgeter.Models
{
    public class HouseholdEditViewModel
    {
        public Household Household { get; set; }
        public ICollection<Account> ActiveAccounts {get;set;}
        public ICollection<Account> InactiveAccounts { get; set; }
        public ICollection<Budgets> ActiveBudgets { get; set; }
    }
}