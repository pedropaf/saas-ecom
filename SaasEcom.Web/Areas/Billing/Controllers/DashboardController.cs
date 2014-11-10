using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Core.DataServices;
using SaasEcom.Core.DataServices.Storage;
using SaasEcom.Core.Infrastructure.Facades;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe;
using SaasEcom.Core.Models;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;
using SaasEcom.Web.Data;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "dashboard")]
    public class DashboardController : Controller
    {
        private AccountDataService<SaasEcomDbContext<SaasEcomUser>, SaasEcomUser> _accountDataService;
        private AccountDataService<SaasEcomDbContext<SaasEcomUser>, SaasEcomUser> AccountDataService
        {
            get { return _accountDataService ??
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
