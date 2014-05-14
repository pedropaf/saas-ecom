using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "settings")]
    public class SettingsController : Controller
    {
        public ActionResult Index()
        {
            var model = new SettingsViewModel();

            // TODO: Get Stripe Account
            var userId = User.Identity.GetUserId();


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStripeAccount()
        {
            throw new NotImplementedException();
        }
	}
}