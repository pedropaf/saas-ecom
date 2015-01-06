using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Core.Models;
using Stripe;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe
{
    /// <summary>
    /// Implementation for subscription management with Stripe
    /// </summary>
    public class SubscriptionProvider : ISubscriptionProvider
    {
        private readonly StripeSubscriptionService _subscriptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionProvider"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public SubscriptionProvider(string apiKey)
        {
            this._subscriptionService = new StripeSubscriptionService(apiKey);
        }

        /// <summary>
        /// Subscribes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <param name="trialInDays">The trial in days.</param>
        public void SubscribeUser(SaasEcomUser user, string planId, int trialInDays = 0)
        {
            this._subscriptionService.Create(user.StripeCustomerId, planId,
                new StripeSubscriptionUpdateOptions
                {
                    PlanId = planId,
                    TrialEnd = DateTime.UtcNow.AddDays(trialInDays)
                });
        }

        /// <summary>
        /// Gets the User's subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<List<Subscription>> UserSubscriptionsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ends the subscription.
        /// </summary>
        /// <param name="userStripeId">The user stripe identifier.</param>
        /// <param name="subStripeId">The sub stripe identifier.</param>
        /// <param name="cancelAtPeriodEnd">if set to <c>true</c> [cancel at period end].</param>
        public void EndSubscription(string userStripeId, string subStripeId, bool cancelAtPeriodEnd = false)
        {
            this._subscriptionService.Cancel(userStripeId, subStripeId, cancelAtPeriodEnd);
        }

        /// <summary>
        /// Updates the subscription. (Change subscription plan)
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="subStripeId">The sub stripe identifier.</param>
        /// <param name="newPlanId">The new plan identifier.</param>
        /// <returns></returns>
        public bool UpdateSubscription(string customerId, string subStripeId, string newPlanId)
        {
            var result = true;
            try
            {
                var currentSubscription = this._subscriptionService.Get(customerId, subStripeId);

                var myUpdatedSubscription = new StripeSubscriptionUpdateOptions
                {
                    PlanId = newPlanId,
                    TrialEnd = currentSubscription.TrialEnd // Keep the same trial window as initially created.
                };

                _subscriptionService.Update(customerId, subStripeId, myUpdatedSubscription);
            }
            catch (Exception ex)
            {
                // TODO: Log
                result = false;
            }

            return result;
        }
    }
}
