using System.Configuration;
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
    // TODO: Integrate with Stripe
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


        private object StripeService
        {
            get { return _stripeService ?? 
                        new StripePaymentProcessorProvider(ConfigurationManager.AppSettings["stripe_key"]);}
        }

        // GET: /Dashboard/Card/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CreditCard creditcard = await CardDataService.FindAsync(User.Identity.GetUserId(), id);
            if (creditcard == null)
            {
                return HttpNotFound();
            }
            return View(creditcard);
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
                creditcard.ApplicationUserId = User.Identity.GetUserId();

                await CardDataService.AddAsync(creditcard);

                TempData.Add("flash", new FlashSuccessViewModel("Your credit card has been saved successfully."));

                return RedirectToAction("Index", "Manage");
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
            if (creditcard == null)
            {
                return HttpNotFound();
            }
            return View(creditcard);
        }

        // POST: /Dashboard/Card/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreditCard creditcard)
        {
            if (ModelState.IsValid)
            {
                await CardDataService.UpdateAsync(User.Identity.GetUserId(), creditcard);
                return RedirectToAction("Index");
            }
            return View(creditcard);
        }

        // GET: /Dashboard/Card/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCard creditcard = await CardDataService.FindAsync(User.Identity.GetUserId(), id);
            if (creditcard == null)
            {
                return HttpNotFound();
            }
            return View(creditcard);
        }

        // POST: /Dashboard/Card/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToRouteResult> DeleteConfirmed(int id)
        {
            await CardDataService.DeleteAsync(User.Identity.GetUserId(), id);
            return RedirectToAction("Index");
        }
    }
}
