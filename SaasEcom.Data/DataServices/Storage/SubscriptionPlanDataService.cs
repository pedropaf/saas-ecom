using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using SaasEcom.Data.DataServices.Interfaces;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.DataServices.Storage
{
    public class SubscriptionPlanDataService : ISubscriptionPlanService
    {
        private readonly ApplicationDbContext _dbContext;

        public SubscriptionPlanDataService(ApplicationDbContext context)
        {
            this._dbContext = context;
        }

        public Task<List<SubscriptionPlan>> GetAllAsync()
        {
            return _dbContext.SubscriptionPlans.ToListAsync();
        }

        public Task<SubscriptionPlan> FindAsync(int planId)
        {
            return _dbContext.SubscriptionPlans.FirstOrDefaultAsync(x => x.Id == planId);
        }

        public Task AddAsync(SubscriptionPlan plan)
        {
            _dbContext.SubscriptionPlans.Add(plan);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(SubscriptionPlan plan)
        {
            // By definition only the plan name can be updated
            var dbPlan = await FindAsync(plan.Id);
            dbPlan.Name = plan.Name;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int planId)
        {
            var dbPlan = await FindAsync(planId);
            _dbContext.SubscriptionPlans.Remove(dbPlan);
        }

        public async Task<int> DisableAsync(int id)
        {
            var dbPlan = await FindAsync(id);
            dbPlan.Disabled = true;
            return await _dbContext.SaveChangesAsync();
        }
    }
}
