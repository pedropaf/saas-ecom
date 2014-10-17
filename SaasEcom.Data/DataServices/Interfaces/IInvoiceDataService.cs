using SaasEcom.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaasEcom.Data.DataServices.Interfaces
{
    public interface IInvoiceDataService
    {
        Task<List<Invoice>> UserInvoicesAsync(string id);
        Task<Invoice> UserInvoiceAsync(string userId, int invoiceId);
        Task<int> CreateOrUpdateAsync(Invoice invoice);
        Task<List<Invoice>> GetInvoicesAsync();
    }
}
