using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Interfaces
{
    public interface IAccountDataService
    {
        Task<ApplicationUser> GetUserAsync(string userId);
        StripeAccount GetStripeAccount();
        Task AddOrUpdateStripeAccountAsync(StripeAccount stripeAccount);
        Task<List<ApplicationUser>> GetCustomersAsync();
        string GetStripeSecretKey();
        string GetStripePublicKey();
    }
}
