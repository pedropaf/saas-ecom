using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SaasEcom.Data.DataServices.Storage;
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
        private SubscriptionPlanDataService _subscriptionPlanDataService;
        private SubscriptionPlanDataService SubscriptionPlanDataService
        {
            get
            {
                return _subscriptionPlanDataService ??
                    (_subscriptionPlanDataService = new SubscriptionPlanDataService(Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        public async Task<ActionResult> Index()
        {
            return View(await SubscriptionPlanDataService.GetAllAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,FriendlyId,Name,Price,Interval,TrialPeriodInDays,StatementDescription")] SubscriptionPlan subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                await SubscriptionPlanDataService.AddAsync(subscriptionplan);
                
                // TODO: Add flash message

                return RedirectToAction("Index");
            }

            return View(subscriptionplan);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubscriptionPlan subscriptionplan = await SubscriptionPlanDataService.FindAsync(id.Value);
            if (subscriptionplan == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionplan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Name")] SubscriptionPlan subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                await SubscriptionPlanDataService.UpdateAsync(subscriptionplan); 

                // TODO: Add flash message

                return RedirectToAction("Index");
            }
            return View(subscriptionplan);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // TODO: Validate if there are users subscribed to this plan.
            // If plan has users only disable
            if (true)
            {
                await SubscriptionPlanDataService.DisableAsync(id);
            }
            else
            {
                await SubscriptionPlanDataService.DeleteAsync(id);    
            }
            
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
