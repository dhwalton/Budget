using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace Budgeter.Models
{
    public class InviteViewModel
    {
        public int Id { get; set; }  // do I need this to insert a record?
        public int HouseholdId { get; set; }
        public string EmailAddress { get; set; }
    }
}