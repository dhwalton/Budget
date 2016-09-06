using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Budgeter.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Configuration;

namespace Budget.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdHelper householdHelper = new HouseholdHelper();

        // GET: Households
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            // only return households to which this user belongs
            var households = db.Households.Include(h => h.Owner).Where(h => h.Users.Where(u => u.Id == userId).Any());

            // instantiate the view model
            var model = new HouseholdListViewModel
            {
                Households = households,
                Invites = householdHelper.UserInvites(userId)
            };

            return View(model);
        }

        public ActionResult RemoveUser(string userEmail, int householdId)
        {
            // find the user from the email address given
            var user = db.Users.Where(u => u.Email == userEmail).FirstOrDefault();
            var thisUserId = User.Identity.GetUserId();

            // find the household from the Id given
            var household = db.Households.Find(householdId);

            // make sure this user has permission to remove
            if ((thisUserId == household.OwnerId || user.Id == thisUserId) && user != null)
            {
                // an owner trying to remove themselves can only do it if there are no other users present
                if (!(thisUserId == household.OwnerId && household.Users.Count() > 1))
                {
                    // deactivate the household if the owner is removing himself
                    if (thisUserId == household.OwnerId)
                    {
                        householdHelper.DeactivateHousehold(householdId);
                    }
                }
                else
                {
                    // remove the user from the household
                    householdHelper.RemoveUserFromHousehold(user.Id, householdId);
                }
            }
            return RedirectToAction("Edit", new { id = householdId });
        }

        public ActionResult DeclineInvitation(int householdId)
        {
            // get current user id
            var userId = User.Identity.GetUserId();

            // get the correct invitation id
            var inviteId = householdHelper.GetInviteId(userId, householdId);

            if (inviteId > 0)
            {
                // remove the invitation
                householdHelper.RemoveInvitation(inviteId);
            }
            return RedirectToAction("Index");
        }

        public ActionResult AcceptInvitation(int householdId)
        {
            // get current user id
            var userId = User.Identity.GetUserId();

            // get the correct invitation id
            var inviteId = householdHelper.GetInviteId(userId, householdId);

            // make sure the invitation exists
            if (inviteId > 0)
            {
                // remove the invitation
                householdHelper.RemoveInvitation(inviteId);

                // add the user to the household
                householdHelper.AddUserToHousehold(userId, householdId);
            }
                   
            return RedirectToAction("Index");
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            
            // ensure that this user is a member of the household
            var user = db.Users.Find(User.Identity.GetUserId());
            if (!household.Users.Contains(user))
            {
                // kick them out (maybe redirect to an error view here?)
                return RedirectToAction("Index");
            }

            if (household == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentUserId = User.Identity.GetUserId();

            return View(household);
        }

        public ActionResult Invite()
        {
            return View();
        }

        /*
         * Return Values:
         *  1: Success
         *  0: Failure - User does not manage the household
         * -1: Failure - Model State is invalid
         * -2: Failure - Invited user is already in the household
         * -3: Failure - Invited user is already invited
         */
        public int InviteUser([Bind(Include = "Id, HouseholdId, EmailAddress")] InviteViewModel model)
        {
            // validate the model
            if (ModelState.IsValid)
            {
                // instantiate the household
                var household = db.Households.Find(model.HouseholdId);

                // get the current user's id
                var userId = User.Identity.GetUserId();
                
                // check to see if this is the household owner
                if (household.OwnerId == userId)
                {
                    // see if the invited user has already been invited
                    var invitations = db.Invites.Where(i => i.Email == model.EmailAddress)
                        .Where(i => i.HouseholdId == model.HouseholdId)
                        .Where(i => i.Accepted == false);

                    if (invitations.Count() > 0)
                    {
                        // user is already invited to this household
                        return -3;
                    }

                    // see if the user already exists in the system
                    var invitedUser = db.Users.FirstOrDefault(u => u.Email == model.EmailAddress);
                    if (invitedUser != null)
                    {
                        // see if the user is already in the household
                        if (household.Users.Contains(invitedUser))
                        {
                            // user already is in the household (failure)
                            return -2;
                        }
                    }

                    // use sendgrid to send an email
                    var user = db.Users.Find(userId);
                    var message = new SendGrid.SendGridMessage()
                    {
                        From = new MailAddress(user.Email, user.FirstName + " " + user.LastName),
                        Subject = "Household Budgeter - Invitation to join '" + household.Name + "'",
                        Text = user.FirstName + " " + user.LastName + " has invited you to join the household '" + household.Name + "'"
                               + "on Budgeter! Click the link below to register or login!\n\n"
                               + "http://dhwalton-budget.azurewebsites.net"

                    };

                    message.AddTo(model.EmailAddress);
                    var key = ConfigurationManager.AppSettings["SendGridAPIKey"];
                    var transportWeb = new SendGrid.Web(key);
                    transportWeb.DeliverAsync(message);

                    // create a new invite record
                    var invite = new Invite()
                    {
                        Accepted = false,
                        Email = model.EmailAddress,
                        HouseholdId = model.HouseholdId
                    };

                    // add the record to the invite table
                    db.Invites.Add(invite);
                    db.SaveChanges();

                    // success
                    return 1;
                
                }
                else
                {
                    // permissions failure
                    return 0;
                }
            }

            // model state failure
            return -1;
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,OwnerId,Active,Demo")] Household household)
        {
            if (ModelState.IsValid)
            {
                household.OwnerId = User.Identity.GetUserId();
                var user = db.Users.Find(household.OwnerId);
                household.Users.Add(user);
                db.Households.Add(household);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", household.OwnerId);
            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            var userId = User.Identity.GetUserId();
            
            // ensure that the user is the owner of the household
            if (userId != household.OwnerId)
            {
                // if the user isn't the owner, see if they are in the household
                var user = db.Users.Find(userId);
                if (household.Users.Contains(user))
                {
                    // if so, redirect to Details
                    return RedirectToAction("Details", new { id = id });
                }
                // otherwise, redirect to the index
                return RedirectToAction("Index");
            }

            if (household == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrentUserId = User.Identity.GetUserId();
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "DisplayName", household.OwnerId);
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,OwnerId,Active,Demo")] Household household)
        {
            // ensure that the user is the owner of the household
            if (User.Identity.GetUserId() != household.OwnerId)
            {
                // kick them out (maybe redirect to an error view here?)
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", household.OwnerId);
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
