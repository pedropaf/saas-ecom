using System.Web.Mvc;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        // GET: /Billing/Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}
