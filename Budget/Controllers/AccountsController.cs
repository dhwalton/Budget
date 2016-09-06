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

namespace Budget.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.Household).Include(a => a.Owner);
            return View(accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create(int? householdId)
        {
            // redirect to household index if the id = null
            if (householdId == null)
            {
                return RedirectToAction("Index", "Households");
            }

            var user = db.Users.Find(User.Identity.GetUserId());
            var household = db.Households.Find(householdId);

            // redirect to household index if the user isn't in the household
            if (!household.Users.Contains(user))
            {
                return RedirectToAction("Index", "Households");
            }


            //ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            //ViewBag.HouseholdId = householdId;
            ViewBag.Household = household;
            ViewBag.OwnerId = new SelectList(household.Users, "Id", "DisplayName");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,HouseholdId,OwnerId")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.Balance = 0;
                account.ReconciledBalance = 0;
                account.Active = true;
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Edit","Households",new { id = account.HouseholdId } );
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", account.OwnerId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();

            // redirect to details if the user does not own the account or account's household
            if (userId != account.OwnerId && userId != account.Household.OwnerId)
            {
                return RedirectToAction("Details", new { id = id });
            }



            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", account.OwnerId);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,HouseholdId,OwnerId,Balance,Active,ReconciledBalance")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit","Households", new { id = account.HouseholdId });
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", account.OwnerId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
