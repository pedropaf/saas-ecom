using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data;
using SaasEcom.Data.DataServices.Storage;
using SaasEcom.Data.Infrastructure.Facades;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "dashboard")]
    public class DashboardController : Controller
    {
        private AccountDataService _accountDataService;
        private AccountDataService AccountDataService
        {
            get { return _accountDataService ??
                    (_accountDataService = new AccountDataService(Request.GetOwinContext().Get<ApplicationDbContext>())); }
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

        public async Task<ViewResult> Index()
        {
            var model = new DashboardViewModel
            {
                IsStripeSetup = AccountDataService.GetStripeAccount() != null,
                SubscriptionPlans = await SubscriptionPlansFacade.GetAllAsync()
            };

            return View(model);
        }
	}
}
