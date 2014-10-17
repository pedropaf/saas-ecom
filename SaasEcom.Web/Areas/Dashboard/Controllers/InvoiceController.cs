using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Core;
using SaasEcom.Core.DataServices.Storage;
using SaasEcom.Web.Data;

namespace SaasEcom.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private InvoiceDataService _invoiceDataService;
        private InvoiceDataService InvoiceDataService
        {
            get { return _invoiceDataService ??
                    (_invoiceDataService = new InvoiceDataService(Request.GetOwinContext().Get<ApplicationDbContext>()));}
        }

        public async Task<ViewResult> Detail(int id)
        {
            var invoice = await InvoiceDataService.UserInvoiceAsync(User.Identity.GetUserId(), id);
            return View(invoice);
        }
	}
}