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
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeposit { get; set; }
        public int? HouseholdId { get; set; }
        public Household Household { get; set; }
    }
}