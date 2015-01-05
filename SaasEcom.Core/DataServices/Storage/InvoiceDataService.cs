using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Storage
{
    /// <summary>
    /// Implementation for CRUD related to invoices in the database.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    public class InvoiceDataService<TContext, TUser> : IInvoiceDataService
        where TContext : IDbContext<TUser>
        where TUser : SaasEcomUser
    {
        private readonly TContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceDataService{TContext, TUser}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public InvoiceDataService(TContext context)
        {
            this._dbContext = context;
        }

        /// <summary>
        /// Returns all the invoice given a user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>List of invoices</returns>
        public async Task<List<Invoice>> UserInvoicesAsync(string userId)
        {
            return await _dbContext.Invoices.Where(i => i.Customer.Id == userId).Select(s => s).ToListAsync();
        }

        /// <summary>
        /// Gets the invoice given a users identifier and the invoice identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>
        /// The invoice
        /// </returns>
        public async Task<Invoice> UserInvoiceAsync(string userId, int invoiceId)
        {
            return await _dbContext.Invoices.Where(i => i.Customer.Id == userId && i.Id == invoiceId).Select(s => s).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates the or update asynchronous.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        /// <returns>
        /// int
        /// </returns>
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

        /// <summary>
        /// Gets all the invoices asynchronous.
        /// </summary>
        /// <returns>
        /// List of invoices.
        /// </returns>
        public async Task<List<Invoice>> GetInvoicesAsync()
        {
            var invoices = await _dbContext.Invoices.Include(i => i.Customer).Select(i => i).ToListAsync();

            return invoices;
        }
    }
}
