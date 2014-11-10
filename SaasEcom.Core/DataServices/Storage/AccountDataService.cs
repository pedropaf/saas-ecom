using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Storage
{
    public class AccountDataService<TContext, TUser> : IAccountDataService<TUser>
        where TContext : IDbContext<TUser>
        where TUser : SaasEcomUser
    {
        private TContext DbContext { get; set; }

        public AccountDataService(TContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task<TUser> GetUserAsync(string userId)
        {
            return await DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<TUser>> GetCustomersAsync()
        {
            var customers = await DbContext.Users
                .Include(u => u.Roles).Include(u => u.Invoices)
                .Where(u => u.StripeCustomerId != null) // Not admin
                .Select(u => u).ToListAsync();

            return customers;
        }

        public string GetStripeSecretKey()
        {
            return ConfigurationManager.AppSettings["StripeApiSecretKey"];
        }

        public string GetStripePublicKey()
        {
            return ConfigurationManager.AppSettings["StripeApiPublishableKey"];
        }
    }
}
