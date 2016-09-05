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
    public class Invite
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int HouseholdId { get; set; }
        public bool Accepted { get; set; }
        public virtual Household Household { get; set; }
    }
}