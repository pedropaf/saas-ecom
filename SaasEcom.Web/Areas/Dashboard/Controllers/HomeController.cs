using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data;
using SaasEcom.Data.DataServices;
using SaasEcom.Web.Areas.Dashboard.ViewModels;

namespace SaasEcom.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public async Task<ViewResult> Index()
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            var subService = new SubscriptionsDataService(context);
            var invService = new InvoicesDataServices(context);
            var cardService = new CardDataService(context);
            var defaultCard = (await cardService.GetAllAsync(User.Identity.GetUserId())).FirstOrDefault();

            var viewModel = new DashboardViewModel
            {
                Invoices = await invService.UserInvoicesAsync(User.Identity.Name),
                Subscriptions = (await subService.UserSubscriptionsAsync(User.Identity.Name)).Select(
                    s => new SubscriptionViewModel
                    {
                        Subscription = s,
                        CreditCard = defaultCard
                    }).ToList()
            };

            return View(viewModel);
        }

        [ChildActionOnly]
        public PartialViewResult AvailablePlans()
        {
            return PartialView("_AvailablePlans");
        }
	}
}