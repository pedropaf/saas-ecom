using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data;
using SaasEcom.Data.DataServices.Storage;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe;
using SaasEcom.Data.Models;
using SaasEcom.Web.Areas.Dashboard.ViewModels;

namespace SaasEcom.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private AccountDataService _accountDataService;
        private AccountDataService AccountDataService
        {
            get { return _accountDataService ??
                    (_accountDataService = new AccountDataService(Request.GetOwinContext().Get<ApplicationDbContext>())); }
        }

        private CardService _cardService;
        private CardService CardService
        {
            get { return _cardService ?? (_cardService = new CardService(AccountDataService.GetStripeSecretKey(), 
                new CardDataService(HttpContext.GetOwinContext().Get<ApplicationDbContext>()))); }
        }


        // ACTIONS
        
        public ActionResult Create()
        {
            ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();

            return View(new CreditCard());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreditCard creditcard)
        {
            if (ModelState.IsValid)
            {
                var user = await AccountDataService.GetUserAsync(User.Identity.GetUserId());
                await CardService.AddAsync(user, creditcard);

                TempData.Add("flash", new FlashSuccessViewModel("Your credit card has been saved successfully."));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();

            return View(creditcard);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CreditCard creditcard = await CardService.FindAsync(User.Identity.GetUserId(), id);

            // If the card doesn't exist or doesn't belong the logged in user
            if (creditcard == null || creditcard.ApplicationUserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            creditcard.ClearCreditCardDetails();

            ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();

            return View(creditcard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreditCard creditcard)
        {
            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid && await CardService.CardBelongToUser(creditcard.Id, userId))
            {
                var user = await AccountDataService.GetUserAsync(userId);
                await CardService.UpdateAsync(user, creditcard);

                TempData.Add("flash", new FlashSuccessViewModel("Your credit card has been updated successfully."));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();

            return View(creditcard);
        }
    }
}
