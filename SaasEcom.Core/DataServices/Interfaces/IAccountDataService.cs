using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Interfaces
{
    public interface IAccountDataService<TUser> where TUser : SaasEcomUser
    {
        Task<TUser> GetUserAsync(string userId);
        StripeAccount GetStripeAccount();
        Task AddOrUpdateStripeAccountAsync(StripeAccount stripeAccount);
        Task<List<TUser>> GetCustomersAsync();
        string GetStripeSecretKey();
        string GetStripePublicKey();
    }
}
