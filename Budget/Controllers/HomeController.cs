using Budgeter.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Budgeter.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            // see if user is logged in
            if (User.Identity.IsAuthenticated)
            { 
                // find the user
                var user = db.Users.Find(User.Identity.GetUserId());
            
                // see if the user is part of a household
                if (user.Household != null)
                {
                    // redirect to that household
                    return RedirectToAction("Edit", "Households", new { id = user.HouseholdId });
                }
            }
            // otherwise, go to the landing page
            return View();
        }

        [Authorize]
        public ActionResult _HouseholdMenuItem()
        {
            var userId = User.Identity.GetUserId();

            // only return households to which this user belongs
            var model = db.Households.Where(h => h.Users.Where(u => u.Id == userId).Any());

            return PartialView(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}