using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Core;
using SaasEcom.Core.DataServices.Storage;
using SaasEcom.Core.Infrastructure.Facades;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe;
using SaasEcom.Core.Models;
using SaasEcom.Web.Areas.Dashboard.ViewModels;
using SaasEcom.Web.Data;

namespace SaasEcom.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    public class HomeController : Controller
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

        private SubscriptionsFacade _subscriptionsFacade;
        private SubscriptionsFacade SubscriptionsFacade
        {
            get
            {
                return _subscriptionsFacade ?? (_subscriptionsFacade = new SubscriptionsFacade(
                    new SubscriptionDataService<ApplicationDbContext, SaasEcomUser>(HttpContext.GetOwinContext().Get<ApplicationDbContext>()),
                    new SubscriptionProvider(AccountDataService.GetStripeSecretKey()),
                    new CardProvider(AccountDataService.GetStripeSecretKey(),
                        new CardDataService<ApplicationDbContext, SaasEcomUser>(Request.GetOwinContext().Get<ApplicationDbContext>())),
                    new CustomerProvider("apiKey")));
            }
        }

        public async Task<ViewResult> Index()
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            var invService = new InvoiceDataService(context);

            var defaultCard = await SubscriptionsFacade.DefaultCreditCard(User.Identity.GetUserId());

            var viewModel = new DashboardViewModel
            {
                Invoices = await invService.UserInvoicesAsync(User.Identity.GetUserId()),
                Subscriptions = (await SubscriptionsFacade.UserActiveSubscriptionsAsync(User.Identity.GetUserId())).Select(
                    s => new SubscriptionViewModel
                    {
                        Subscription = s,
                        CreditCard = defaultCard
                    }).ToList()
            };

            return View(viewModel);
        }
	}
}
