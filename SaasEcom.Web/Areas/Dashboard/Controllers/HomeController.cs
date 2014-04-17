using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Index()
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            var subService = new SubscriptionsDataService(context);
            var invService = new InvoicesDataServices(context);

            var viewModel = new DashboardViewModel
            {
                Subscriptions = subService.UserSubscriptions(User.Identity.Name),
                Invoices = invService.UserInvoices(User.Identity.Name)
            };

            return View(viewModel);
        }
	}
}