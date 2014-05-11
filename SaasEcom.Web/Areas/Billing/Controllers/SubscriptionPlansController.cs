using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SaasEcom.Data.Models;
using SaasEcom.Data;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Web.Areas.Billing.Filters;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "subscription-plans")]
    public class SubscriptionPlansController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View(await Request.GetOwinContext().Get<ApplicationDbContext>().SubscriptionPlans.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: /Billing/SubscriptionPlans/Create
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

        // POST: /Billing/SubscriptionPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // TODO: Validate if there are users subscribed to this plan.

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
