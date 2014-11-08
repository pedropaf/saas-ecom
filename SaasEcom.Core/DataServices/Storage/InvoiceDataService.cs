using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Storage
{
    public class InvoiceDataService : IInvoiceDataService
    {
        private readonly IDbContext<SaasEcomUser> _dbContext;

        public InvoiceDataService(IDbContext<SaasEcomUser> context)
        {
            this._dbContext = context;
        }

        public async Task<List<Invoice>> UserInvoicesAsync(string id)
        {
            return await _dbContext.Invoices.Where(i => i.Customer.Id == id).Select(s => s).ToListAsync();
        }

        public async Task<Invoice> UserInvoiceAsync(string userId, int invoiceId)
        {
            return await _dbContext.Invoices.Where(i => i.Customer.Id == userId && i.Id == invoiceId).Select(s => s).FirstOrDefaultAsync();
        }

        public async Task<int> CreateOrUpdateAsync(Invoice invoice)
        {
            var res = -1;

            var dbInvoice = _dbContext.Invoices.Find(invoice.Id);

            if (dbInvoice == null)
            {
                // Set user Id
                var user = await _dbContext.Users.Where(u => u.StripeCustomerId == invoice.StripeCustomerId).FirstOrDefaultAsync();

                if (user != null)
                {
                    invoice.Customer = user;
                    _dbContext.Invoices.Add(invoice);
                    res = await _dbContext.SaveChangesAsync();
                }
            }
            else
            {
                dbInvoice.Paid = invoice.Paid;
                dbInvoice.Attempted = invoice.Attempted;
                dbInvoice.AttemptCount = invoice.AttemptCount;
                dbInvoice.NextPaymentAttempt = invoice.NextPaymentAttempt;
                dbInvoice.Closed = invoice.Closed;
                res = await _dbContext.SaveChangesAsync();
            }

            return res;
        }

        public async Task<List<Invoice>> GetInvoicesAsync()
        {
            var invoices = await _dbContext.Invoices.Include(i => i.Customer).Select(i => i).ToListAsync();

            return invoices;
        }
    }
}
