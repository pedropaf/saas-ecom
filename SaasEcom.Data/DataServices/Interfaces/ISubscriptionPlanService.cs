using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.DataServices.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlan>> GetAllAsync();
        Task<SubscriptionPlan> FindAsync(int planId);
        Task AddAsync(SubscriptionPlan plan);
        Task<int> UpdateAsync(SubscriptionPlan plan);
        Task DeleteAsync(int planId);
    }
}
