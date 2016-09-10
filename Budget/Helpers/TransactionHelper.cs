using Budgeter.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text;

public static class TransactionHelper
{
    private static ApplicationDbContext dbContext = new ApplicationDbContext();

    public static bool CategoryIsDeposit(int categoryId)
    {
        return dbContext.Categories.Find(categoryId).IsDeposit;
    }

}
