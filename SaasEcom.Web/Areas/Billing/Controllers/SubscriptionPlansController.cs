using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SaasEcom.Core.DataServices;
using SaasEcom.Core.DataServices.Storage;
using SaasEcom.Core.Infrastructure.Facades;
using SaasEcom.Core.Infrastructure.Helpers;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe;
using SaasEcom.Core.Models;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;
using SaasEcom.Web.Data;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "subscription-plans")]
    public class SubscriptionPlansController : Controller
    {
        private AccountDataService<SaasEcomDbContext<SaasEcomUser>, SaasEcomUser> _accountDataService;
        private AccountDataService<SaasEcomDbContext<SaasEcomUser>, SaasEcomUser> AccountDataService
        {
            get
            {
                return _accountDataService ??
                  (_accountDataService = new AccountDataService<SaasEcomDbContext<SaasEcomUser>, SaasEcomUser>(Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        private SubscriptionPlansFacade _subscriptionPlansFacade;
        private SubscriptionPlansFacade SubscriptionPlansFacade
        {
            get
            {
                return _subscriptionPlansFacade ??
                  (_subscriptionPlansFacade = new SubscriptionPlansFacade(
                      new SubscriptionPlanDataService(Request.GetOwinContext().Get<ApplicationDbContext>()),
                      new SubscriptionPlanProvider(AccountDataService.GetStripeSecretKey())));
            }
        }

        public async Task<ActionResult> Index()
        {
            return View(await SubscriptionPlansFacade.GetAllAsync());
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
                await SubscriptionPlansFacade.AddAsync(subscriptionplan);
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

            SubscriptionPlan subscriptionplan = await SubscriptionPlansFacade.FindAsync(id.Value);
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
                await SubscriptionPlansFacade.UpdateAsync(subscriptionplan);
                TempData.Add("flash", new FlashSuccessViewModel("The subscription plan has been updated successfully."));
                return RedirectToAction("Index");
            }
            return View(subscriptionplan);
        }

        [HttpGet, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            switch (await SubscriptionPlansFacade.DeleteAsync(id))
            {
                case 0:
                    TempData.Add("flash", new FlashSuccessViewModel("The subscription plan has been deleted successfully."));
                    break;
                case 1:
                    TempData.Add("flash", new FlashSuccessViewModel("The subscription plan has been disabled successfully."));
                    break;
                default:
                    TempData.Add("flash", new FlashDangerViewModel("There was a problem deleting the subscription plan. Please try again."));
                    break;
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
