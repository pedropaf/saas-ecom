using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SaasEcom.Data;
using SaasEcom.Data.DataServices;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe;
using SaasEcom.Data.Models;
using SaasEcom.Web.Areas.Dashboard.ViewModels;

namespace SaasEcom.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private StripePaymentProcessorProvider _stripeService;
        private StripePaymentProcessorProvider StripeService
        {
            get
            {
                return _stripeService ??
                      (_stripeService = new StripePaymentProcessorProvider(ConfigurationManager.AppSettings["stripe_secret_key"]));
            }
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }

                TempData.Add("flash", new FlashSuccessViewModel("Your password has been changed successfully."));

                return RedirectToAction("Index", "Home");
            }
            AddErrors(result);
            return View(model);
        }

        public async Task<ActionResult> CancelSubscription(int id)
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var subscriptionsService = new SubscriptionDataService(db);

            if (subscriptionsService.SubscriptionBelongsToUser(User.Identity.GetUserId(), id))
            {
                await subscriptionsService.EndSubscriptionAsync(id);

                var user = db.Users.Find(User.Identity.GetUserId());
                this.StripeService.CancelCustomerSubscription(user.StripeCustomerId); // TODO: Cancel individual subscription
            }

            TempData.Add("flash", new FlashSuccessViewModel("Your subscription has been cancelled."));

            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion
	}
}