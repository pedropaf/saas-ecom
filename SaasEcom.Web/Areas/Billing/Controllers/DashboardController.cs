using System.Web.Mvc;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "dashboard")]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            var model = new DashboardViewModel();

            return View(model);
        }
	}
}
