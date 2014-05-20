using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data;
using SaasEcom.Data.DataServices.Storage;
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
            get
            {
                return _accountDataService ??
                    (_accountDataService = new AccountDataService(Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        private SubscriptionPlanDataService _subscriptionPlanDataService;
        private SubscriptionPlanDataService SubscriptionPlanDataService
        {
            get
            {
                return _subscriptionPlanDataService ??
                    (_subscriptionPlanDataService = new SubscriptionPlanDataService(Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        public async Task<ViewResult> Index()
        {
            var model = new DashboardViewModel
            {
                IsStripeSetup = AccountDataService.GetStripeAccount() != null,
                SubscriptionPlans = await SubscriptionPlanDataService.GetAllAsync()
            };

            return View(model);
        }
	}
}
