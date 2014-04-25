using System.Web.Mvc;

namespace SaasEcom.Web.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(Duration = 604800)]
        public ActionResult Index()
        {
            return View();
        }
    }
}