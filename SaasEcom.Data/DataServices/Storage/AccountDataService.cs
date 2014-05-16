using System;
using System.Data.Entity;
using System.Threading.Tasks;
using SaasEcom.Data.DataServices.Interfaces;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.DataServices.Storage
{
    public class AccountDataService : IAccountDataService
    {
        private ApplicationDbContext DbContext { get; set; }

        public AccountDataService(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task<ApplicationUser> GetUserAsync(string userId)
        {
            return await DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<StripeAccount> GetStripeAccountAsync(string userId)
        {
            return await DbContext.StripeAccounts.FirstOrDefaultAsync(
                stripeAccount => stripeAccount.ApplicationUser.Id == userId);
        }

        public async Task AddStripeAccountAsync(StripeAccount stripeAccount)
        {
            stripeAccount.StripeTestPublicApiKey = stripeAccount.StripeTestPublicApiKey.Trim();
            stripeAccount.StripeTestSecretApiKey = stripeAccount.StripeTestSecretApiKey.Trim();
            stripeAccount.StripeLivePublicApiKey = stripeAccount.StripeLivePublicApiKey.Trim();
            stripeAccount.StripeLiveSecretApiKey = stripeAccount.StripeLiveSecretApiKey.Trim();

            DbContext.StripeAccounts.Add(stripeAccount);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateStripeAccountAsync(StripeAccount stripeAccount)
        {
            StripeAccount sa = await DbContext.StripeAccounts
                .FirstOrDefaultAsync(m => m.Id == stripeAccount.Id && m.ApplicationUser.Id == stripeAccount.ApplicationUser.Id);
            
            if (sa == null)
                throw new ArgumentException("Stripe Account");

            sa.StripeTestPublicApiKey = stripeAccount.StripeTestPublicApiKey.Trim();
            sa.StripeTestSecretApiKey = stripeAccount.StripeTestSecretApiKey.Trim();
            sa.StripeLivePublicApiKey = stripeAccount.StripeLivePublicApiKey.Trim();
            sa.StripeLiveSecretApiKey = stripeAccount.StripeLiveSecretApiKey.Trim();

            await DbContext.SaveChangesAsync();
        }
    }
}
