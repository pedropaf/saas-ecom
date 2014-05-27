using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.DataServices.Interfaces;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.Infrastructure.Facades
{
    public class SubscriptionPlansFacade
    {
        private readonly ISubscriptionPlanDataService _subscriptionDataService;
        private readonly ISubscriptionPlanProvider _subscriptionPlanProvider;

        public SubscriptionPlansFacade(ISubscriptionPlanDataService data, ISubscriptionPlanProvider planProvider)
        {
            _subscriptionDataService = data;
            _subscriptionPlanProvider = planProvider;
        }

        public async Task<List<SubscriptionPlan>> GetAllAsync()
        {
            return await _subscriptionDataService.GetAllAsync();
        }

        public async Task AddAsync(SubscriptionPlan plan)
        {
            await _subscriptionDataService.AddAsync(plan);
            _subscriptionPlanProvider.Add(plan);
        }

        public async Task<int> UpdateAsync(SubscriptionPlan plan)
        {
            int res = await _subscriptionDataService.UpdateAsync(plan);
            _subscriptionPlanProvider.Update(plan);

            return res;
        }

        public async Task<int> DeleteAsync(int planId)
        {
            int result = -1;

            var plan = await _subscriptionDataService.FindAsync(planId);

            if (plan == null)
            {
                throw new ArgumentException("planId");    
            }

            if (!plan.Disabled)
            {
                var countUsersInPlan = await _subscriptionDataService.CountUsersAsync(plan.Id);

                // If plan has users only disable
                if (countUsersInPlan > 0)
                {
                    await _subscriptionDataService.DisableAsync(planId);
                    result = 1;
                }
                else
                {
                    await _subscriptionDataService.DeleteAsync(planId);
                    result = 0;
                }

                // Delete from Stripe
                _subscriptionPlanProvider.Delete(plan.FriendlyId);
            }

            return result;
        }

        public async Task<SubscriptionPlan> FindAsync(int planId)
        {
            return await _subscriptionDataService.FindAsync(planId);
        }
    }
}
