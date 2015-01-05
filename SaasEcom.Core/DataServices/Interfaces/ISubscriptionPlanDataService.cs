using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Interfaces
{
    public interface ISubscriptionPlanDataService
    {
        Task<List<SubscriptionPlan>> GetAllAsync();
        Task<SubscriptionPlan> FindAsync(string planId);
        Task AddAsync(SubscriptionPlan plan);
        Task<int> UpdateAsync(SubscriptionPlan plan);
        Task<int> DeleteAsync(string planId);
        Task<int> DisableAsync(string planId);
        Task<int> CountUsersAsync(string planId);
    }
}
