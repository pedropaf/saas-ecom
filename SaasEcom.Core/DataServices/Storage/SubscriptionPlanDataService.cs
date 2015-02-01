using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Storage
{
    /// <summary>
    /// Implementation for CRUD related to subscription plans in the database.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    public class SubscriptionPlanDataService<TContext, TUser> : ISubscriptionPlanDataService
        where TContext : IDbContext<TUser>
        where TUser : class
    {
        private readonly TContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionPlanDataService{TContext, TUser}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SubscriptionPlanDataService(TContext context)
        {
            this._dbContext = context;
        }

        /// <summary>
        /// Gets all subscription plans asynchronous.
        /// </summary>
        /// <returns>
        /// List of Subscription Plans
        /// </returns>
        public Task<List<SubscriptionPlan>> GetAllAsync()
        {
            return _dbContext.SubscriptionPlans.Include(sp => sp.Properties).ToListAsync();
        }


        /// <summary>
        /// Finds the subscription plan asynchronously.
        /// </summary>
        /// <param name="planId">The plan identifier.</param>
        /// <returns>SubscriptionPlan</returns>
        public Task<SubscriptionPlan> FindAsync(string planId)
        {
            return _dbContext.SubscriptionPlans.Include(sp => sp.Properties).FirstOrDefaultAsync(x => x.Id == planId);
        }

        /// <summary>
        /// Adds the subscription plan asynchronously.
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <returns></returns>
        public Task AddAsync(SubscriptionPlan plan)
        {
            _dbContext.SubscriptionPlans.Add(plan);
            return _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the subscription plan asynchronously.
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(SubscriptionPlan plan)
        {
            // By definition only the plan name can be updated
            var dbPlan = await FindAsync(plan.Id);
            dbPlan.Name = plan.Name;
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the subscription plan asynchronously.
        /// </summary>
        /// <param name="planId">The plan identifier.</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string planId)
        {
            var dbPlan = await FindAsync(planId);
            _dbContext.SubscriptionPlans.Remove(dbPlan);
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Disables the subscription plan asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<int> DisableAsync(string id)
        {
            var dbPlan = await FindAsync(id);
            dbPlan.Disabled = true;
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Counts the users subscribed to a plan.
        /// </summary>
        /// <param name="planId">The plan identifier.</param>
        /// <returns></returns>
        public async Task<int> CountUsersAsync(string planId)
        {
            var count = await _dbContext.Subscriptions
                .Where(s => s.End == null || s.End > DateTime.UtcNow)
                .CountAsync(s => s.SubscriptionPlanId == planId);
            return count;
        }
    }
}
