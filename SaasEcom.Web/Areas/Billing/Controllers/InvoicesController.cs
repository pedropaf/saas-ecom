using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SaasEcom.Data;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data.DataServices.Interfaces;
using SaasEcom.Data.DataServices.Storage;
using SaasEcom.Web.Areas.Billing.Filters;
using SaasEcom.Web.Areas.Billing.ViewModels;

namespace SaasEcom.Web.Areas.Billing.Controllers
{
    [Authorize(Roles = "admin")]
    [SectionFilter(Section = "invoices")]
    public class InvoicesController : Controller
    {
        private IInvoiceService _invoiceDataService;
        private IInvoiceService InvoiceDataService
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
	}
}