using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SaasEcom.Data;
using Microsoft.AspNet.Identity.Owin;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    public class CustomersController : Controller
    {
        // GET: /Billing/Customers/
        public ActionResult Index()
        {
            var db = Request.GetOwinContext().Get<ApplicationDbContext>();




            return View();
        }
	}
}