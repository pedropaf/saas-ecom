using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SaasEcom.Data;
using SaasEcom.Data.DataServices.Storage;
using SaasEcom.Data.Infrastructure.Facades;
using SaasEcom.Data.Infrastructure.Identity;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe;
using SaasEcom.Data.Models;
using SaasEcom.Web.Areas.Dashboard.ViewModels;

namespace SaasEcom.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        private AccountDataService _accountDataService;
        private AccountDataService AccountDataService
        {
            get { return _accountDataService ??
                    (_accountDataService = new AccountDataService(Request.GetOwinContext().Get<ApplicationDbContext>())); }
        }

        private SubscriptionsFacade _subscriptionsFacade;
        private SubscriptionsFacade SubscriptionsFacade
        {
            get
            {
                return _subscriptionsFacade ?? (_subscriptionsFacade = new SubscriptionsFacade(
                    new SubscriptionDataService(HttpContext.GetOwinContext().Get<ApplicationDbContext>()), 
                    new SubscriptionProvider(AccountDataService.GetStripeSecretKey()),
                    new CardProvider(AccountDataService.GetStripeSecretKey(), new CardDataService(Request.GetOwinContext().Get<ApplicationDbContext>()))));
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

        public ActionResult ChangePassword()
        {
            return View();
        }

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
            if (await SubscriptionsFacade.EndSubscriptionAsync(id, await AccountDataService.GetUserAsync(User.Identity.GetUserId())))
            {
                TempData.Add("flash", new FlashSuccessViewModel("Your subscription has been cancelled."));
            }
            else
            {
                TempData.Add("flash", new FlashDangerViewModel("Sorry, there was a problem cancelling your subscription."));
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Subscribe(string plan)
        {
            var model = new SubscribeViewModel
            {
                PlanFriendlyId = plan,
                CreditCard = await SubscriptionsFacade.DefaultCreditCard(User.Identity.GetUserId()) ?? new CreditCard()
            };
            model.CreditCard.ClearCreditCardDetails();
            ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Subscribe(SubscribeViewModel details)
        {
            if (ModelState.IsValid)
            {
                var user = await AccountDataService.GetUserAsync(User.Identity.GetUserId());
                await this.SubscriptionsFacade.SubscribeUserAsync(user, details.PlanFriendlyId, details.CreditCard);
                TempData.Add("flash", new FlashSuccessViewModel("Thanks for signing up again."));
            }
            else
            {
                TempData.Add("flash", new FlashDangerViewModel("Sorry, there was a problem creating your subscription. Please try again."));
                ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();
                return View(details);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ViewResult> ChangeSubscription()
        {
            var currentSubscription = (await SubscriptionsFacade.UserActiveSubscriptionsAsync(User.Identity.GetUserId())).FirstOrDefault();

            var model = new ChangeSubscriptionViewModel
            {
                SubscriptionPlans = await SubscriptionPlansFacade.GetAllAsync(),
                CurrentSubscription = currentSubscription != null ? currentSubscription.SubscriptionPlan.FriendlyId : string.Empty
            };
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeSubscription(ChangeSubscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Update subscription!!

                TempData.Add("flash", new FlashSuccessViewModel("Your plan has been updated."));
            }
            else
            {
                TempData.Add("flash", new FlashSuccessViewModel("Sorry, there was an error updating your plan, try again or contact support."));
            }

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