using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.Infrastructure.Facades
{
    /// <summary>
    /// Subscription Plans Facade to manage the subscription plans for your application.
    /// </summary>
    public class SubscriptionPlansFacade
    {
        private readonly ISubscriptionPlanDataService _subscriptionDataService;
        private readonly ISubscriptionPlanProvider _subscriptionPlanProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionPlansFacade"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="planProvider">The plan provider.</param>
        public SubscriptionPlansFacade(ISubscriptionPlanDataService data, ISubscriptionPlanProvider planProvider)
        {
            _subscriptionDataService = data;
            _subscriptionPlanProvider = planProvider;
        }

        /// <summary>
        /// Gets all subscription plans asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<SubscriptionPlan>> GetAllAsync()
        {
            return await _subscriptionDataService.GetAllAsync();
        }

        /// <summary>
        /// Adds the subscription plan asynchronous.
        /// </summary>
        /// <param name="subscriptionPlan">The subscription plan.</param>
        /// <returns></returns>
        public async Task AddAsync(SubscriptionPlan subscriptionPlan)
        {
            await _subscriptionDataService.AddAsync(subscriptionPlan);
            _subscriptionPlanProvider.Add(subscriptionPlan);
        }

        /// <summary>
        /// Updates the subscription plan asynchronous.
        /// </summary>
        /// <param name="subscriptionPlan">The subscription plan.</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(SubscriptionPlan subscriptionPlan)
        {
            int res = await _subscriptionDataService.UpdateAsync(subscriptionPlan);
            _subscriptionPlanProvider.Update(subscriptionPlan);

            return res;
        }

        /// <summary>
        /// Deletes the subscription plan asynchronous.
        /// </summary>
        /// <param name="subscriptionPlanId">The subscription plan identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">subscriptionPlanId</exception>
        public async Task<int> DeleteAsync(string subscriptionPlanId)
        {
            int result = -1;

            var plan = await _subscriptionDataService.FindAsync(subscriptionPlanId);

            if (plan == null)
            {
                throw new ArgumentException("subscriptionPlanId");    
            }

            if (!plan.Disabled)
            {
                var countUsersInPlan = await _subscriptionDataService.CountUsersAsync(plan.Id);

                // If plan has users only disable
                if (countUsersInPlan > 0)
                {
                    await _subscriptionDataService.DisableAsync(subscriptionPlanId);
                    result = 1;
                }
                else
                {
                    await _subscriptionDataService.DeleteAsync(subscriptionPlanId);
                    result = 0;
                }

                // Delete from Stripe
                _subscriptionPlanProvider.Delete(plan.Id);
            }

            return result;
        }

        /// <summary>
        /// Finds the subscription plan asynchronous.
        /// </summary>
        /// <param name="subscriptionPlanId">The subscription plan identifier.</param>
        /// <returns>The Subscription Plan</returns>
        public async Task<SubscriptionPlan> FindAsync(string subscriptionPlanId)
        {
            return await _subscriptionDataService.FindAsync(subscriptionPlanId);
        }
    }
}
