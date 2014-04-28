using SaasEcom.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaasEcom.Data.DataServices.Interfaces
{
    public interface IInvoiceDataService
    {
        Task<List<Invoice>> UserInvoicesAsync(string id);
        Task<Invoice> UserInvoiceAsync(string userId, int invoiceId);
        Task<int> CreateOrUpdateAsync(Invoice invoice);
    }
}
