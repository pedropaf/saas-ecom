using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data;
using SaasEcom.Data.DataServices.Storage;
using SaasEcom.Data.Models;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "settings")]
    public class SettingsController : Controller
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

        public async Task<ViewResult> Index()
        {
            var sa = await AccountDataService.GetStripeAccountAsync(User.Identity.GetUserId());
            var model = new SettingsViewModel
            {
                StripeAccount = sa ?? new StripeAccount()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> EditStripeAccount(SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.StripeAccount.ApplicationUser = await AccountDataService.GetUserAsync(User.Identity.GetUserId());

                if (model.StripeAccount.Id == 0)
                {
                    await AccountDataService.AddStripeAccountAsync(model.StripeAccount);
                }
                else
                {
                    await AccountDataService.UpdateStripeAccountAsync(model.StripeAccount);
                }
            }

            // TODO: Add flash

            return View("Index", model);
        }
	}
}