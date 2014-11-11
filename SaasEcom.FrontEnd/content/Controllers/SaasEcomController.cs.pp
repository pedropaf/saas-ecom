using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
    public class SaasEcomController : Controller
    {
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

        private AccountDataService<ApplicationDbContext, ApplicationUser> _accountDataService;
        private AccountDataService<ApplicationDbContext, ApplicationUser> AccountDataService
        {
            get
            {
                return _accountDataService ??
                  (_accountDataService = new AccountDataService<ApplicationDbContext, ApplicationUser>
                      (Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        private ICardProvider _cardService;
        private ICardProvider CardService
        {
            get
            {
                return _cardService ?? (_cardService = new CardProvider(AccountDataService.GetStripeSecretKey(),
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
                // SubscriptionsFacade.UpdateSubscriptionAsync(User.Identity.GetUserId(), model.CurrentSubscription, model.NewPlan);

                TempData.Add("flash", new FlashSuccessViewModel("Your plan has been updated."));
            }
            else
            {
                TempData.Add("flash", new FlashSuccessViewModel("Sorry, there was an error updating your plan, try again or contact support."));
            }

            return RedirectToAction("Index", "Home");
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


        public ActionResult AddCreditCard()
        {
            ViewBag.PublishableKey = ConfigurationManager.AppSettings["StripeApiPublishableKey"];

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
                var user = await AccountDataService.GetUserAsync(User.Identity.GetUserId());
                await CardService.AddAsync(user, model.CreditCard);

                TempData.Add("flash", new FlashSuccessViewModel("Your credit card has been saved successfully."));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();

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
            ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeCreditCard(CreditCardViewModel model)
        {
            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid && await CardService.CardBelongToUser(model.CreditCard.Id, userId))
            {
                var user = await AccountDataService.GetUserAsync(userId);
                await CardService.UpdateAsync(user, model.CreditCard);
                TempData.Add("flash", new FlashSuccessViewModel("Your credit card has been updated successfully."));
                return RedirectToAction("Index", "Home");
            }
            ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();

            return View(model);
        }

        public async Task<ViewResult> Invoice(int id)
        {
            var invoice = await InvoiceDataService.UserInvoiceAsync(User.Identity.GetUserId(), id);
            return View(invoice);
        }
    }

    #region ViewModels

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