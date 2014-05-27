using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.DataServices.Interfaces
{
    public interface ISubscriptionPlanDataService
    {
        Task<List<SubscriptionPlan>> GetAllAsync();
        Task<SubscriptionPlan> FindAsync(int planId);
        Task AddAsync(SubscriptionPlan plan);
        Task<int> UpdateAsync(SubscriptionPlan plan);
        Task<int> DeleteAsync(int planId);

        Task<int> CountUsersAsync(int planId);
    }
}
