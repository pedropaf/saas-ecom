using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SaasEcom.Data.Models;
using SaasEcom.Data;
using Microsoft.AspNet.Identity.Owin;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    public class SubscriptionPlansController : Controller
    {
        // GET: /Billing/SubscriptionPlans/
        public async Task<ActionResult> Index()
        {
            return View(await Request.GetOwinContext().Get<ApplicationDbContext>().SubscriptionPlans.ToListAsync());
        }

        // GET: /Billing/SubscriptionPlans/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionPlan subscriptionplan = await Request.GetOwinContext().Get<ApplicationDbContext>().SubscriptionPlans.FindAsync(id);
            if (subscriptionplan == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionplan);
        }

        // GET: /Billing/SubscriptionPlans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Billing/SubscriptionPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,FriendlyId,Name,Price,Interval,TrialPeriodInDays,StatementDescription")] SubscriptionPlan subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                var db = Request.GetOwinContext().Get<ApplicationDbContext>();
                db.SubscriptionPlans.Add(subscriptionplan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(subscriptionplan);
        }

        // GET: /Billing/SubscriptionPlans/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionPlan subscriptionplan = await Request.GetOwinContext().Get<ApplicationDbContext>().SubscriptionPlans.FindAsync(id);
            if (subscriptionplan == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionplan);
        }

        // POST: /Billing/SubscriptionPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,FriendlyId,Name,Price,Interval,TrialPeriodInDays,StatementDescription")] SubscriptionPlan subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                var db = Request.GetOwinContext().Get<ApplicationDbContext>();
                db.Entry(subscriptionplan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subscriptionplan);
        }

        // GET: /Billing/SubscriptionPlans/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionPlan subscriptionplan = await Request.GetOwinContext().Get<ApplicationDbContext>().SubscriptionPlans.FindAsync(id);
            if (subscriptionplan == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionplan);
        }

        // POST: /Billing/SubscriptionPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var db = Request.GetOwinContext().Get<ApplicationDbContext>();
            SubscriptionPlan subscriptionplan = await db.SubscriptionPlans.FindAsync(id);
            db.SubscriptionPlans.Remove(subscriptionplan);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Request.GetOwinContext().Get<ApplicationDbContext>().Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
