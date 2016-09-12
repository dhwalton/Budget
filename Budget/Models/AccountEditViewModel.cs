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
    public class AccountEditViewModel
    {
        public Account Account { get; set; }
        public ICollection<Transaction> ActiveTransactions { get; set; }
        public ICollection<Transaction> InactiveTransactions { get; set; }
    }
}