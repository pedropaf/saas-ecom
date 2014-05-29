using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using SaasEcom.Data;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data.DataServices.Storage;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;
using SaasEcom.Web.Helpers;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "customers")]
    public class CustomersController : Controller
    {
        private AccountDataService _accountDataService;
        private AccountDataService AccountDataService
        {
            get { return _accountDataService ??
                    (_accountDataService = new AccountDataService(Request.GetOwinContext().Get<ApplicationDbContext>())); }
        }

        public async Task<ViewResult> Index()
        {
            ViewBag.AnyCustomers = (await AccountDataService.GetCustomersAsync()).Any();
            
            return View();
        }

        public async Task<JsonNetResult> GetCustomers()
        {
            var customers = await AccountDataService.GetCustomersAsync();

            return new JsonNetResult { Data = Mapper.Map<List<CustomerViewModel>>(customers), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}