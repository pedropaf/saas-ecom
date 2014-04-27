using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Data;
using SaasEcom.Data.DataServices;

namespace SaasEcom.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private InvoicesDataServices _invoicesDataServices;
        private InvoicesDataServices InvoicesDataService
        {
            get
            {
                return _invoicesDataServices ?? 
                    (_invoicesDataServices = new InvoicesDataServices(Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        // GET: /Dashboard/Invoice/Detail?id=1
        public async Task<ViewResult> Detail(int id)
        {
            var invoice = await InvoicesDataService.UserInvoiceAsync(User.Identity.GetUserId(), id);
            return View(invoice);
        }
	}
}