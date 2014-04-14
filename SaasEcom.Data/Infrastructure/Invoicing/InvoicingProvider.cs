using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.Infrastructure.Invoicing
{
    public class InvoicingProvider
    {
        public List<Invoice> IssueNewInvoices()
        {
            throw new NotImplementedException();
        }

        public List<Invoice> GetUnpaidInvoices()
        {
            throw new NotImplementedException();
        }
    }

    public static class InvoicingHelper
    {
        public static async Task NotifyCustomersByEmailAsync(this List<Invoice> invoices)
        {
            await Task.Delay(1000);
        }

        public static async Task RememberCustomersByEmailAsync(this List<Invoice> invoices)
        {
            await Task.Delay(1000);
        }
    }
}
