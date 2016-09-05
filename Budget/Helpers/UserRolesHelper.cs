﻿using Budgeter.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text;

public class UserRolesHelper
{
    private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
    private ApplicationDbContext db = new ApplicationDbContext();


    //public void EmailUser(string userId, TicketNotifications n)
    //{
    //    var user = db.Users.Find(userId); 
    //    if (user == null)
    //    {
    //        // Don't reveal that the user does not exist or is not confirmed
    //        return;
    //    }

    //    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
    //    // Send an email with this link
    //    //string code = await manager.GeneratePasswordResetTokenAsync(user.Id);
    //    manager.SendEmail(user.Id, "New Activity on Ticket '" + n.Ticket.Title + "'", 
    //            "Ticket '" + n.Ticket.Title + "' has new activity: " + n.Message + "<p><a href='https://dhwalton-bugtracker.azurewebsites.net'>Click Here to Login.</a>");
    //    return;
    //}

    public string UserRolesString(string userId)
    {
        var sb = new StringBuilder();
        var roles = ListUserRoles(userId);
        foreach (var role in roles)
        {
            sb.Append(role);
            sb.Append(",");
        }
        return sb.ToString();
    }

    public bool IsUserInRole(string userId, string roleName)
    {
        return manager.IsInRole(userId, roleName);
    }

    public IList<IdentityRole> ListAllRoles()
    {
        return db.Roles.ToList();
    }


    public ICollection<string> ListUserRoles(string userId)
    {
        return manager.GetRoles(userId);
    }

    public bool AddUserToRole(string userId, string roleName)
    {
        var result = manager.AddToRole(userId, roleName);
        return result.Succeeded;
    }

    public bool AddUserToRoles(string userId, string[] roleNames)
    {
        var result = manager.AddToRoles(userId, roleNames);
        return result.Succeeded;
    }

    public bool RemoveUserFromRole(string userId, string roleName)
    {
        var result = manager.RemoveFromRole(userId, roleName);
        return result.Succeeded;
    }

    public ICollection<ApplicationUser> UsersInRole(string roleName)
    {
        var resultList = new List<ApplicationUser>();
        var List = manager.Users.ToList();
        foreach (var user in List)
        {
            if (IsUserInRole(user.Id, roleName))
                resultList.Add(user);
        }
        //var roleId = db.Roles.FirstOrDefault(r => r.Name == roleName).Id;
        //resultList = manager.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId)).ToList();
        return resultList;
    }

    public ICollection<ApplicationUser> UsersNotInRole(string roleName)
    {
        var resultList = new List<ApplicationUser>();
        var List = manager.Users.ToList();
        foreach (var user in List)
        {
            if (!IsUserInRole(user.Id, roleName))
                resultList.Add(user);
        }
        //var roleId = db.Roles.FirstOrDefault(r => r.Name == roleName).Id;
        //resultList = manager.Users.Where(u => u.Roles.Any(r => r.RoleId != roleId)).ToList();
        return resultList;
    }

   

        // Some methods to help with getting/setting user fields
        public string GetUserFirstName(string userId)
        {
            return manager.FindById(userId).FirstName;
        }

        public string GetUserLastName(string userId)
        {
            return manager.FindById(userId).LastName;
        }

        public string GetUserDisplayName(string userId)
        {
            return manager.FindById(userId).DisplayName;   
        }

        public IdentityResult SetUserFirstName(string userId, string newFirstName)
        {
            var user = manager.FindById(userId);
            user.FirstName = newFirstName;
            return manager.Update(user);

           // db.Entry(user).State = EntityState.Modified;
           // return db.SaveChanges();
        }

        public IdentityResult SetUserLastName(string userId, string newLastName)
        {
            var user = manager.FindById(userId);
            user.LastName = newLastName;
            //db.Entry(user).State = EntityState.Modified;
            return manager.Update(user);
        }

        public IdentityResult SetUserDisplayName(string userId, string newDisplayName)
        {
            var user = manager.FindById(userId);
            user.DisplayName = newDisplayName;
           // db.Entry(user).State = EntityState.Modified;
            //return db.SaveChanges();
            return manager.Update(user);
        }

        public IList<ApplicationUser> AllUsers()
        {
            return manager.Users.ToList();
        }

        public ApplicationUser GetUserById(string Id)
        {
            return manager.FindById(Id);
        }

        public ApplicationUser GetUserByName(string Name)
        {
            return manager.FindByName(Name);
        }

    public bool isDemoUser(string userId)
    {
        if (manager.IsInRole(userId,"Demo Admin") ||
            manager.IsInRole(userId,"Demo Project Manager") || 
            manager.IsInRole(userId,"Demo Developer") ||
            manager.IsInRole(userId,"Demo Submitter"))
        {
            return true;
        }
        return false;
    }

}
