using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Storage
{
    public class AccountDataService : IAccountDataService
    {
        private IDbContext DbContext { get; set; }

        public AccountDataService(IDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task<ApplicationUser> GetUserAsync(string userId)
        {
            return await DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public StripeAccount GetStripeAccount()
        {
            // The system is designed to have only one account at the moment
            return DbContext.StripeAccounts.FirstOrDefault();
        }

        public async Task AddOrUpdateStripeAccountAsync(StripeAccount stripeAccount)
        {
            StripeAccount sa = await DbContext.Users.Include(u => u.StripeAccount)
                .Where(u => u.Id == stripeAccount.ApplicationUser.Id).Select(u => u.StripeAccount).FirstOrDefaultAsync();

            if (sa == null)
            {
                var user = await DbContext.Users.FirstAsync(u => u.Id == stripeAccount.ApplicationUser.Id);
                user.StripeAccount = stripeAccount;
            }
            else
            {
                sa.LiveMode = stripeAccount.LiveMode;
                sa.StripeLivePublicApiKey = stripeAccount.StripeLivePublicApiKey;
                sa.StripeLiveSecretApiKey = stripeAccount.StripeLiveSecretApiKey;
                sa.StripeTestPublicApiKey = stripeAccount.StripeTestPublicApiKey;
                sa.StripeTestSecretApiKey = stripeAccount.StripeTestSecretApiKey;
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<List<ApplicationUser>> GetCustomersAsync()
        {
            var customers = await DbContext.Users
                .Include(u => u.Roles).Include(u => u.Invoices)
                .Where(u => u.StripeCustomerId != null) // Not admin
                .Select(u => u).ToListAsync();

            return customers;
        }

        public string GetStripeSecretKey()
        {
            var account = this.GetStripeAccount();
            return account.LiveMode ? account.StripeLiveSecretApiKey : account.StripeTestSecretApiKey;
        }

        public string GetStripePublicKey()
        {
            var account = this.GetStripeAccount();
            return account.LiveMode ? account.StripeLivePublicApiKey : account.StripeTestPublicApiKey;
        }
    }
}
