using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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

            var viewModel = new DashboardViewModel
            {
                Subscriptions = await subService.UserSubscriptionsAsync(User.Identity.Name),
                Invoices = await invService.UserInvoicesAsync(User.Identity.Name)
            };

            return View(viewModel);
        }
	}
}