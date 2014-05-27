using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SaasEcom.Data.DataServices.Storage;
using SaasEcom.Data.Infrastructure.Helpers;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe;
using SaasEcom.Data.Models;
using SaasEcom.Data;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    // TODO: Refactor class to use provider
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "subscription-plans")]
    public class SubscriptionPlansController : Controller
    {
        private AccountDataService _accountDataService;
        private AccountDataService AccountDataService
        {
            get
            {
                return _accountDataService ??
                  (_accountDataService = new AccountDataService(Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        // DB
        private SubscriptionPlanDataService _subscriptionPlanDataService;
        private SubscriptionPlanDataService SubscriptionPlanDataService
        {
            get
            {
                return _subscriptionPlanDataService ??
                    (_subscriptionPlanDataService = new SubscriptionPlanDataService(Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        // Stripe
        private SubscriptionPlanProvider _subscriptionPlanProvider;
        private SubscriptionPlanProvider StripePlanProvider
        {
            get
            {
                return _subscriptionPlanProvider ??
                    (new SubscriptionPlanProvider(AccountDataService.GetStripeSecretKey()));
            }
        }

        public async Task<ActionResult> Index()
        {
            return View(await SubscriptionPlanDataService.GetAllAsync());
        }

        public ActionResult Create()
        {
            ViewBag.Currencies = CurrenciesSelect(RegionInfo.CurrentRegion.ISOCurrencySymbol);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,FriendlyId,Name,Price,Interval,Currency,TrialPeriodInDays")] SubscriptionPlan subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                // DB
                await SubscriptionPlanDataService.AddAsync(subscriptionplan);

                // Stripe
                StripePlanProvider.Add(subscriptionplan);

                TempData.Add("flash", new FlashSuccessViewModel("The subscription plan has been created successfully."));

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
        public async Task<ActionResult> Edit([Bind(Include = "Id,FriendlyId,Name,Price,Interval,Currency,TrialPeriodInDays")] SubscriptionPlan subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                // DB
                await SubscriptionPlanDataService.UpdateAsync(subscriptionplan);

                // Stripe
                StripePlanProvider.Update(subscriptionplan);

                TempData.Add("flash", new FlashSuccessViewModel("The subscription plan has been updated successfully."));

                return RedirectToAction("Index");
            }
            return View(subscriptionplan);
        }

        [HttpGet, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var plan = await SubscriptionPlanDataService.FindAsync(id);

            if (!plan.Disabled)
            {
                var countUsersInPlan = await SubscriptionPlanDataService.CountUsersAsync(plan.Id);

                // If plan has users only disable
                if (countUsersInPlan > 0)
                {
                    // DB
                    await SubscriptionPlanDataService.DisableAsync(id);
                    TempData.Add("flash", new FlashSuccessViewModel("The subscription plan has been disabled successfully."));
                }
                else
                {
                    await SubscriptionPlanDataService.DeleteAsync(id);
                    TempData.Add("flash", new FlashSuccessViewModel("The subscription plan has been deleted successfully."));
                }

                // Delete from Stripe
                StripePlanProvider.Delete(plan.FriendlyId);
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

        private static IEnumerable<SelectListItem> CurrenciesSelect(string selected)
        {
            return CurrencyHelper.Currencies.Select(c => new SelectListItem
            {
                Selected = c.Value == selected,
                Text = string.Format("{0} - {1}", c.Value, c.Key),
                Value = c.Value
            });
        }
    }
}
