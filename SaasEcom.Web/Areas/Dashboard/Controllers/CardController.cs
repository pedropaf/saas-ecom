using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data;
using SaasEcom.Data.DataServices.Storage;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Interfaces;
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

        private ICardProvider _cardService;
        private ICardProvider CardService
        {
            get { return _cardService ?? (_cardService = new CardProvider(AccountDataService.GetStripeSecretKey(), 
                new CardDataService(HttpContext.GetOwinContext().Get<ApplicationDbContext>()))); }
        }


        // ACTIONS
        
        public ActionResult Create()
        {
            ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();

            var model = new EditCreditCardViewModel
            {
                CreditCard = new CreditCard()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditCreditCardViewModel model)
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

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new EditCreditCardViewModel
            {
                CreditCard = await CardService.FindAsync(User.Identity.GetUserId(), id)
            };

            // If the card doesn't exist or doesn't belong the logged in user
            if (model.CreditCard == null || model.CreditCard.ApplicationUserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            model.CreditCard.ClearCreditCardDetails();

            ViewBag.PublishableKey = AccountDataService.GetStripePublicKey();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditCreditCardViewModel model)
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
    }
}
