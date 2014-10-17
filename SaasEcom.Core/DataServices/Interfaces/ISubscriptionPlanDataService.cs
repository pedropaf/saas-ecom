using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Interfaces
{
    public interface ISubscriptionPlanDataService
    {
        Task<List<SubscriptionPlan>> GetAllAsync();
        Task<SubscriptionPlan> FindAsync(int planId);
        Task AddAsync(SubscriptionPlan plan);
        Task<int> UpdateAsync(SubscriptionPlan plan);
        Task<int> DeleteAsync(int planId);
        Task<int> DisableAsync(int planId);
        Task<int> CountUsersAsync(int planId);
    }
}
