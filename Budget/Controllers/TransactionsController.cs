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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transaction.Include(t => t.Account).Include(t => t.Category).Include(t => t.User);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transaction.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        //public ActionResult Create()
        //{
        //    ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name");
        //    ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
        //    return View();
        //}

       [HttpGet]
       public ActionResult ToggleTransaction(int id)
        {
            var transaction = db.Transaction.Find(id);
            var userId = User.Identity.GetUserId();
            // user must be the owner of account or owner of household
            if (userId == transaction.Account.OwnerId || userId == transaction.Account.Household.OwnerId)
            {
                // toggle active
                transaction.Active = !transaction.Active;
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Edit","Accounts", new { id = transaction.AccountId });
        }

        [HttpGet]
        public ActionResult VoidTransaction(int id)
        {
            var transaction = db.Transaction.Find(id);
            transaction.Void = !transaction.Void;
            db.Entry(transaction).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "Accounts", new { id = transaction.AccountId });
        }

        // GET: Transactions/Create
        public ActionResult Create(int accountId, bool isDeposit)
        {
            ViewBag.AccountId = accountId;

            // only show deposit or withdrawal categories
            if (isDeposit)
            {
                ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.IsDeposit), "Id", "Name");
            }
            else
            {
                ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.IsDeposit == false), "Id", "Name");
            }
            
            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }


        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,TypeId,CategoryId,UserId,Reconciled,ReconciledAmount,Description,Date,Amount,Active")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                // always start with a positive amount
                transaction.Amount = Math.Abs(transaction.Amount);

                // check to see if the transaction isn't a deposit
                if (!TransactionHelper.CategoryIsDeposit(transaction.CategoryId))
                {
                    // if not, make the amount negative
                    transaction.Amount *= -1;
                }

                // initialize some of the fields
                transaction.UserId = User.Identity.GetUserId();
                transaction.TypeId = 0;
                transaction.Active = true;
                transaction.Reconciled = false;
                transaction.ReconciledAmount = 0;

                db.Transaction.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Edit","Accounts",new { id = transaction.AccountId });
            }

            //ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", transaction.UserId);
            //return View(transaction);

            return RedirectToAction("Edit", "Accounts", new { id = transaction.AccountId });
        }


        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transaction.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", transaction.UserId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,TypeId,CategoryId,UserId,Reconciled,ReconciledAmount,Description,Date,Amount,Active")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit","Accounts", new { id = transaction.AccountId });
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", transaction.UserId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transaction.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transaction.Find(id);
            db.Transaction.Remove(transaction);
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
