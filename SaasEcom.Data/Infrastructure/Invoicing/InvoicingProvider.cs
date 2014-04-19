using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.Infrastructure.Invoicing
{
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
