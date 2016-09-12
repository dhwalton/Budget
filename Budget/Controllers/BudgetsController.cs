using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Budgeter.Models;

namespace Budget.Controllers
{
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        public ActionResult Index()
        {
            var budgets = db.Budgets.Include(b => b.Household);
            return View(budgets.ToList());
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budgets budgets = db.Budgets.Find(id);
            if (budgets == null)
            {
                return HttpNotFound();
            }
            return View(budgets);
        }

        // GET: Budgets/Create
        public ActionResult Create(int householdId)
        {
            ViewBag.HouseholdId = householdId;
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Active,HouseholdId,Name")] Budgets budgets)
        {
            if (ModelState.IsValid)
            {
                budgets.Active = true;
                db.Budgets.Add(budgets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budgets.HouseholdId);
            return View(budgets);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetViewModel model = new BudgetViewModel(id ?? 1);
            if (model == null)
            {
                return HttpNotFound();
            }
           
            return View(model);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Active,HouseholdId,Name")] Budgets budgets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = budgets.Id });
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budgets.HouseholdId);
            return View(budgets);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budgets budgets = db.Budgets.Find(id);
            if (budgets == null)
            {
                return HttpNotFound();
            }
            return View(budgets);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budgets budgets = db.Budgets.Find(id);
            db.Budgets.Remove(budgets);
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
