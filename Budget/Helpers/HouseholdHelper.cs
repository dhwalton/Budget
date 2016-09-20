using Budgeter.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text;

public class HouseholdHelper
{
    private ApplicationDbContext db = new ApplicationDbContext();

    public void AddUserToHousehold(string userId, int householdId)
    {
        var user = db.Users.Find(userId);
        var household = db.Households.Find(householdId);
        household.Users.Add(user);
        db.SaveChanges();
    }

    public void RemoveUserFromHousehold(string userId, int householdId)
    {
        var user = db.Users.Find(userId);
        var household = db.Households.Find(householdId);
        household.Users.Remove(user);
        db.SaveChanges();
    }

    public List<Invite> UserInvites(string userId)
    {
        var user = db.Users.Find(userId);
        return db.Invites
                .Where(i => i.Email == user.Email)
                .Where(i => i.Accepted == false)
                .ToList();
    }

    // check for an invitation id for a user & household
    public int GetInviteId(string userId, int householdId)
    {
        var user = db.Users.Find(userId);

        // look for this user's invitation to the household
        var invite = db.Invites
                    .Where(i => i.Email == user.Email)
                    .Where(i => i.HouseholdId == householdId)
                    .Where(i => i.Accepted == false).FirstOrDefault();

        if (invite == null)
        {
            // invitation was not found
            return -1;
        }

        // invitation was found
        return invite.Id;
    }

    // does the userId match the household ownerId?
    public bool UserOwnsHousehold(string userId, int householdId)
    {
        if (userId == db.Households.Find(householdId).OwnerId) return true;
        return false;
    }

    public bool DeactivateHousehold(int householdId)
    {
        var household = db.Households.Find(householdId);
        if (household != null)
        {
            // deactivate household
            household.Active = false;

            // save to DB
            db.SaveChanges();
            
            // household was successfully deactivated
            return true;
        }

        // household wasn't found
        return false;
    }

    // remove an invitation to a household, if one exists
    public bool RemoveInvitation(int inviteId)
    {
        var invite = db.Invites.Find(inviteId);
        if (invite != null)
        {
            // remove the invitation
            db.Invites.Remove(invite);
            // save changes to database
            db.SaveChanges();
            // invitation was found and removed
            return true;
        }
        // invitation was not found
        return false;
    }


    // add some default categories to a household
    public ICollection<Category> DefaultCategories()
    {
        HashSet<Category> Categories = new HashSet<Category>();
        Categories.Add(new Category()
        {
            Name = "Misc Deposit",
            IsDeposit = true,
            Default = true
        });
        Categories.Add(new Category()
        {
            Name = "Paycheck",
            IsDeposit = true,
            Default = false
        });
        Categories.Add(new Category()
        {
            Name = "Gift",
            IsDeposit = true,
            Default = false
        });
        Categories.Add(new Category()
        {
            Name = "Misc Withdrawal",
            IsDeposit = false,
            Default = true
        });
        Categories.Add(new Category()
        {
            Name = "Groceries",
            IsDeposit = false,
            Default = false
        });
        Categories.Add(new Category()
        {
            Name = "Entertainment",
            IsDeposit = false,
            Default = false
        });
        Categories.Add(new Category()
        {
            Name = "Rent",
            IsDeposit = false,
            Default = false
        });
        Categories.Add(new Category()
        {
            Name = "Mortgage",
            IsDeposit = false,
            Default = false
        });
        Categories.Add(new Category()
        {
            Name = "Utilities",
            IsDeposit = false,
            Default = false
        });
        Categories.Add(new Category()
        {
            Name = "Savings",
            IsDeposit = false,
            Default = false
        });
        

        return Categories;
    }

}
