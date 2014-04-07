using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaasEcom.Web.Areas.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Dashboard/Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}