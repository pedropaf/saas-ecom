using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data;
using SaasEcom.Data.DataServices;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe;
using SaasEcom.Data.Models;
using SaasEcom.Web.Areas.Dashboard.ViewModels;

namespace SaasEcom.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        public CardController()
        {
        }

        public CardController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationDbContext DbContext
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
        }

        private CardDataService _cardService;
        private CardDataService CardDataService
        {
            get { return _cardService ?? new CardDataService(DbContext); }
        }

        private StripePaymentProcessorProvider _stripeService;
        private StripePaymentProcessorProvider StripeService
        {
            get { return _stripeService ?? 
                        new StripePaymentProcessorProvider(ConfigurationManager.AppSettings["stripe_secret_key"]);}
        }

        // GET: /Dashboard/Card/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Dashboard/Card/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreditCard creditcard)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                creditcard.ApplicationUserId = userId;

                // Add card to Stripe
                var stripeCustomerId = DbContext.Users.First(u => u.Id == userId).StripeCustomerId;
                this.StripeService.AddCard(stripeCustomerId, creditcard);
                
                // Add card to DB
                await CardDataService.AddAsync(creditcard);

                TempData.Add("flash", new FlashSuccessViewModel("Your credit card has been saved successfully."));

                return RedirectToAction("Index", "Home");
            }

            return View(creditcard);
        }

        // GET: /Dashboard/Card/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCard creditcard = await CardDataService.FindAsync(User.Identity.GetUserId(), id);

            // If the card doesn't exist or doesn't belong the logged in user
            if (creditcard == null || creditcard.ApplicationUserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            creditcard.ExpirationMonth = null;
            creditcard.ExpirationYear = null;
            creditcard.Last4 = null;
            creditcard.Fingerprint = null;
            creditcard.StripeId = null;
            creditcard.StripeToken = null;
            creditcard.Cvc = null;
            creditcard.Type = null;

            return View(creditcard);
        }

        // POST: /Dashboard/Card/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreditCard creditcard)
        {
            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid && await CardBelongToUser(creditcard.Id, userId))
            {
                // Remove current card from stripe
                var currentCard = await CardDataService.FindAsync(userId, creditcard.Id);
                var stripeCustomerId = DbContext.Users.First(u => u.Id == userId).StripeCustomerId;
                StripeService.DeleteCard(stripeCustomerId, currentCard.StripeId);

                // Add card to Stripe
                StripeService.AddCard(stripeCustomerId, creditcard);

                // Update card in the DB
                creditcard.ApplicationUserId = userId;
                await CardDataService.UpdateAsync(User.Identity.GetUserId(), creditcard);

                TempData.Add("flash", new FlashSuccessViewModel("Your credit card has been updated successfully."));

                return RedirectToAction("Index", "Home");
            }

            return View(creditcard);
        }

        private async Task<bool> CardBelongToUser(int cardId, string userId)
        {
            return await this.CardDataService.AnyAsync(cardId, userId);
        }
    }
}
