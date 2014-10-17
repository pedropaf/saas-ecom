using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using SaasEcom.Core;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.DataServices.Storage;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;
using SaasEcom.Web.Data;
using SaasEcom.Web.Helpers;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "invoices")]
    public class InvoicesController : Controller
    {
        private IInvoiceDataService _invoiceDataService;
        private IInvoiceDataService InvoiceDataService
        {
            get { return _invoiceDataService ??
                    (_invoiceDataService = new InvoiceDataService(Request.GetOwinContext().Get<ApplicationDbContext>())); }
        }

        public async Task<ViewResult> Index()
        {
            var model = new InvoicesViewModel
            {
                Invoices = await InvoiceDataService.GetInvoicesAsync()
            };

            return View(model);
        }

        public async Task<JsonNetResult> GetInvoices()
        {
            var invoices = await InvoiceDataService.GetInvoicesAsync();

            return new JsonNetResult { Data = Mapper.Map<List<InvoiceViewModel>>(invoices), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}