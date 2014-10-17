using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    public interface IInvoiceProvider
    {
        Task<List<Models.Invoice>> UserInvoicesAsync(string id);
        Task<Models.Invoice> UserInvoiceAsync(string userId, int invoiceId);
        Task<int> CreateOrUpdateAsync(Models.Invoice invoice);
    }
}
