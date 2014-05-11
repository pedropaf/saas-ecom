using System.Web.Mvc;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "dashboard")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new BaseViewModel());
        }
	}
}
