using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaasEcom.Data.DataServices.Interfaces;

namespace SaasEcom.Data.DataServices.Stripe
{
    public class StripeInvoiceService : IInvoiceService
    {
        public Task<List<Models.Invoice>> UserInvoicesAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Invoice> UserInvoiceAsync(string userId, int invoiceId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateOrUpdateAsync(Models.Invoice invoice)
        {
            throw new NotImplementedException();
        }
    }
}
