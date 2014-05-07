using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaasEcom.Data;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data.Infrastructure.Identity;
using SaasEcom.Web.Areas.Billing.ViewModels;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    public class CustomersController : Controller
    {
        // GET: /Billing/Customers/
        public ActionResult Index()
        {
            var db = Request.GetOwinContext().Get<ApplicationDbContext>();

            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var users = db.Users.ToList(); // TODO: Filter by subscriber only

            var model = new CustomersViewModel
            {
                Customers = users
            };

            return View(model);
        }
	}
}