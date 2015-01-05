using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Core.Models;
using Stripe;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe
{
    public class SubscriptionProvider : ISubscriptionProvider
    {
        private readonly StripeSubscriptionService _subscriptionService;

        public SubscriptionProvider(string apiKey)
        {
            this._subscriptionService = new StripeSubscriptionService(apiKey);
        }

        public void SubscribeUser(SaasEcomUser user, string planId, int trialInDays = 0)
        {
            this._subscriptionService.Create(user.StripeCustomerId, planId,
                new StripeSubscriptionUpdateOptions
                {
                    PlanId = planId,
                    TrialEnd = DateTime.UtcNow.AddDays(trialInDays)
                });
        }

        public Task<List<Subscription>> UserSubscriptionsAsync(string userId)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        public void EndSubscription(string userStripeId, string subStripeId, bool cancelAtPeriodEnd = false)
        {
            this._subscriptionService.Cancel(userStripeId, subStripeId, cancelAtPeriodEnd);
        }

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
