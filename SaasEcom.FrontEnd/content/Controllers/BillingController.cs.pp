using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Core.DataServices.Storage;
using SaasEcom.Core.Infrastructure.Facades;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe;
using SaasEcom.Core.Models;
using $rootnamespace$.Models;
using $rootnamespace$.Views.SaasEcom.ViewModels;

namespace $rootnamespace$.Controllers
{
    [Authorize]
    public class BillingController : Controller
    {
	    private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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

        private SubscriptionsFacade _subscriptionsFacade;
        private SubscriptionsFacade SubscriptionsFacade
        {
            get
            {
                return _subscriptionsFacade ?? (_subscriptionsFacade = new SubscriptionsFacade(
                    new SubscriptionDataService<ApplicationDbContext, ApplicationUser>
                        (HttpContext.GetOwinContext().Get<ApplicationDbContext>()),
                    new SubscriptionProvider(ConfigurationManager.AppSettings["StripeApiSecretKey"]),
                    new CardProvider(ConfigurationManager.AppSettings["StripeApiSecretKey"],
                        new CardDataService<ApplicationDbContext, ApplicationUser>(Request.GetOwinContext().Get<ApplicationDbContext>())),
                    new CustomerProvider(ConfigurationManager.AppSettings["StripeApiSecretKey"])));
            }
        }

        private InvoiceDataService<ApplicationDbContext, ApplicationUser> _invoiceDataService;
        private InvoiceDataService<ApplicationDbContext, ApplicationUser> InvoiceDataService
        {
            get
            {
                return _invoiceDataService ??
                       (_invoiceDataService =
                           new InvoiceDataService<ApplicationDbContext, ApplicationUser>(
                               Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        private ICardProvider _cardService;
        private ICardProvider CardService
        {
            get
            {
                return _cardService ?? (_cardService = new CardProvider(ConfigurationManager.AppSettings["StripeApiSecretKey"],
                    new CardDataService<ApplicationDbContext, ApplicationUser>(HttpContext.GetOwinContext().Get<ApplicationDbContext>())));
            }
        }

        private SubscriptionPlansFacade _subscriptionPlansFacade;
        private SubscriptionPlansFacade SubscriptionPlansFacade
        {
            get
            {
                return _subscriptionPlansFacade ?? (_subscriptionPlansFacade = new SubscriptionPlansFacade(
                    new SubscriptionPlanDataService<ApplicationDbContext, ApplicationUser>(HttpContext.GetOwinContext().Get<ApplicationDbContext>()),
                    new SubscriptionPlanProvider(ConfigurationManager.AppSettings["StripeApiSecretKey"])));
            }
        }

        public async Task<ViewResult> Index()
        {
            ViewBag.Subscriptions = await SubscriptionsFacade.UserActiveSubscriptionsAsync(User.Identity.GetUserId());
            ViewBag.PaymentDetails = await SubscriptionsFacade.DefaultCreditCard(User.Identity.GetUserId());
            ViewBag.Invoices = await InvoiceDataService.UserInvoicesAsync(User.Identity.GetUserId());

            return View();
        }

        public async Task<ViewResult> ChangeSubscription()
        {
            var currentSubscription = (await SubscriptionsFacade.UserActiveSubscriptionsAsync(User.Identity.GetUserId())).FirstOrDefault();

            var model = new ChangeSubscriptionViewModel
            {
                SubscriptionPlans = await SubscriptionPlansFacade.GetAllAsync(),
                CurrentSubscription = currentSubscription != null ? currentSubscription.SubscriptionPlan.Id : string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeSubscription(ChangeSubscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SubscriptionsFacade.UpdateSubscriptionAsync(user.Id, user.StripeCustomerId, model.NewPlan);

                // TempData.Add("flash", new FlashSuccessViewModel("Your subscription plan has been updated."));
            }
            else
            {
                // TempData.Add("flash", new FlashSuccessViewModel("Sorry, there was an error updating your plan, try again or contact support."));
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CancelSubscription(int id)
        {
			return View(new CancelSubscriptionViewModel { Id = id });
        }

		[HttpPost]
		public async Task<ActionResult> CancelSubscription(CancelSubscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentSubscription = (await SubscriptionsFacade.UserActiveSubscriptionsAsync(User.Identity.GetUserId())).FirstOrDefault();
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                DateTime? endDate; // Because we are passing CancelAtTheEndOfPeriod to EndSubscription, we get the date when the subscription will be cancelled
                if (currentSubscription != null &&
                    (endDate = await SubscriptionsFacade.EndSubscriptionAsync(currentSubscription.Id, user, true, model.Reason)) != null)
                {
                    // TempData.Add("flash", new FlashSuccessViewModel("Your subscription has been cancelled."));
                }
                else
                {
                    // TempData.Add("flash", new FlashDangerViewModel("Sorry, there was a problem cancelling your subscription."));
                }

                return RedirectToAction("Index", "Billing");
            }

			return View(model);
        }

		public async Task<ActionResult> ReActivateSubscription()
		{
            var currentSubscription = (await SubscriptionsFacade.UserActiveSubscriptionsAsync(User.Identity.GetUserId())).FirstOrDefault();
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (currentSubscription != null &&
                await SubscriptionsFacade.UpdateSubscriptionAsync(user.Id, user.StripeCustomerId, currentSubscription.SubscriptionPlanId))
            {
                // TempData.Add("flash", new FlashSuccessViewModel("Your subscription plan has been re-activated."));
            }
            else
            {
                // TempData.Add("flash", new FlashDangerViewModel("Ooops! There was a problem re-activating your subscription. Please, try again."));
            }

            return RedirectToAction("Index");
		} 

        public ActionResult AddCreditCard()
        {
            return View(new CreditCardViewModel
            {
                CreditCard = new CreditCard()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCreditCard(CreditCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await CardService.AddAsync(user, model.CreditCard);

                // TempData.Add("flash", new FlashSuccessViewModel("Your credit card has been saved successfully."));

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<ActionResult> ChangeCreditCard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new CreditCardViewModel
            {
                CreditCard = await CardService.FindAsync(User.Identity.GetUserId(), id)
            };

            // If the card doesn't exist or doesn't belong the logged in user
            if (model.CreditCard == null || model.CreditCard.SaasEcomUserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            model.CreditCard.ClearCreditCardDetails();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeCreditCard(CreditCardViewModel model)
        {
            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid && await CardService.CardBelongToUser(model.CreditCard.Id, userId))
            {
                var user = await UserManager.FindByIdAsync(userId);
                await CardService.UpdateAsync(user, model.CreditCard);
                
                // TempData.Add("flash", new FlashSuccessViewModel("Your credit card has been updated successfully."));
                
                return RedirectToAction("Index");
            }

            return View(model);
        }

		public async Task<ViewResult> BillingAddress()
        {
			// TODO: Get Billing address from your model
			var model = new BillingAddress();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BillingAddress(BillingAddress model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Call your service to save the billing address


                // TempData.Add("flash", new FlashSuccessViewModel("Your billing address has been saved."));

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<ViewResult> Invoice(int id)
        {
            var invoice = await InvoiceDataService.UserInvoiceAsync(User.Identity.GetUserId(), id);
            return View(invoice);
        }

		public async Task<ActionResult> DeleteAccount()
        {
            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            
            // Delete User
            await _userManager.DeleteAsync(user);
            
            // TODO: Delete user data

            SignInManager.AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }

    #region ViewModels

	public class CancelSubscriptionViewModel
    {
		public int Id { get; set; }

        [Required]
        [Display(Name = "Reason why you want to cancel")]
        public string Reason { get; set; }
    }

    public class CreditCardViewModel
    {
        public CreditCard CreditCard { get; set; }
    }

    public class ChangeSubscriptionViewModel
    {
        public List<SubscriptionPlan> SubscriptionPlans { get; set; }
        public string CurrentSubscription { get; set; }
        public string NewPlan { get; set; }
    }

    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string StripeId { get; set; }
        public string StripeCustomerId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }

        public int? Subtotal { get; set; }
        public int? Total { get; set; }
        public bool? Attempted { get; set; }
        public bool? Closed { get; set; }
        public bool? Paid { get; set; }
        public int? AttemptCount { get; set; }
        public int? AmountDue { get; set; }
        public int? StartingBalance { get; set; }
        public int? EndingBalance { get; set; }
        public DateTime? NextPaymentAttempt { get; set; }
        public int? Charge { get; set; }
        public int? Discount { get; set; }
        public int? ApplicationFee { get; set; }
        public string CurrencySymbol { get; set; }
        public string InvoicePeriod { get; set; }

        public ICollection<LineItem> LineItems { get; set; }

        public class LineItem
        {
            public int Id { get; set; }
            public string StripeLineItemId { get; set; }
            public string Type { get; set; }
            public int? Amount { get; set; }
            public string Currency { get; set; }
            public bool Proration { get; set; }
            public Period Period { get; set; }
            public int? Quantity { get; set; }
            public Plan Plan { get; set; }
        }

        public class Period
        {
            public DateTime? Start { get; set; }
            public DateTime? End { get; set; }
        }

        public class Plan
        {
            public string StripePlanId { get; set; }
            public string Interval { get; set; }
            public string Name { get; set; }
            public DateTime? Created { get; set; }
            public int? AmountInCents { get; set; }
            public string Currency { get; set; }
            public int IntervalCount { get; set; }
            public int? TrialPeriodDays { get; set; }
            public string StatementDescription { get; set; }
        }
    }

    #endregion
}