using System.Threading.Tasks;
using SaasEcom.Data.Infrastructure.Invoicing;

namespace SaasEcom.Tasks
{
    public class InvoicesTask
    {
        public void Run()
        {
            var service = new InvoicingProvider();
        
            // Tasks
            var tIssueInvoices = service.IssueNewInvoices().NotifyCustomersByEmailAsync();
            var tChaseUnpaidInvoices = service.GetUnpaidInvoices().RememberCustomersByEmailAsync();

            Task.WaitAll(tIssueInvoices, tChaseUnpaidInvoices);
        }
    }
}
