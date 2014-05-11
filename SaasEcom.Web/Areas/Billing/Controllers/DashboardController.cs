using System.Web.Mvc;
using SaasEcom.Web.Areas.Billing.Filters;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "dashboard")]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
