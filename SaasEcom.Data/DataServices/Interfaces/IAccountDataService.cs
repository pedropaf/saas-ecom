using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.DataServices.Interfaces
{
    public interface IAccountDataService
    {
        Task<ApplicationUser> GetUserAsync(string userId);

        Task<StripeAccount> GetStripeAccountAsync(string userId);
    }
}
