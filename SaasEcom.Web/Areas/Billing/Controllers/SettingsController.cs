using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Core.DataServices;
using SaasEcom.Core.DataServices.Storage;
using SaasEcom.Core.Models;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;
using SaasEcom.Web.Data;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "settings")]
    public class SettingsController : Controller
    {
        private AccountDataService<SaasEcomDbContext<SaasEcomUser>, SaasEcomUser>  _accountDataService;
        private AccountDataService<SaasEcomDbContext<SaasEcomUser>, SaasEcomUser> AccountDataService
        {
            get { return _accountDataService ??
                    (_accountDataService = new AccountDataService<SaasEcomDbContext<SaasEcomUser>, SaasEcomUser>(Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        public ViewResult Index()
        {
            var sa = AccountDataService.GetStripeAccount();
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
            ModelState.Remove("StripeAccount.LiveMode"); // boolean field makes validation fail.
            if (ModelState.IsValid)
            {
                model.StripeAccount.SaasEcomUser = await AccountDataService.GetUserAsync(User.Identity.GetUserId());

                var action = model.StripeAccount.Id == 0 ? "saved" : "updated";
                await AccountDataService.AddOrUpdateStripeAccountAsync(model.StripeAccount);

                TempData.Add("flash", new FlashSuccessViewModel("Your stripe details have been " + action + " successfully."));
            }
            else
            {
                TempData.Add("flash", new FlashDangerViewModel("There was an error saving your stripe details"));
            }

            return View("Index", model);
        }
	}
}