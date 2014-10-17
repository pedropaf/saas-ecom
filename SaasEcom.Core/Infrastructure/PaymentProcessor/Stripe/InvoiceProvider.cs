using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe
{
    public class InvoiceProvider : IInvoiceProvider
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
