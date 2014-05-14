using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.DataServices.Interfaces
{
    public interface IAccountDataService
    {
        Task<ApplicationUser> GetUser(string userId);

        Task<StripeAccount> GetStripeAccount(string userId);
    }
}
