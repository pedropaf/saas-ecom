using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.DataServices
{
    public class InvoicesDataServices
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoicesDataServices(ApplicationDbContext context)
        {
            this._dbContext = context;
        }

        public Task<List<Invoice>> UserInvoicesAsync(string name)
        {
            return _dbContext.Invoices.Where(i => i.Customer.UserName == name).Select(s => s).ToListAsync();
        }

        public async Task<int> CreateAsync(Invoice invoice)
        {
            // Set user Id
            var user = await _dbContext.Users.Where(u => u.StripeCustomerId == invoice.StripeCustomerId).FirstOrDefaultAsync();

            if (user == null)
            {
                // TODO: Log error
                return -1;
            }

            invoice.Customer = user;
            _dbContext.Invoices.Add(invoice);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
