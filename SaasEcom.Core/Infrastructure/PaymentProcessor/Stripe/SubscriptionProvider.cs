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

        public void SubscribeUser(ApplicationUser user, string planId, int trialInDays = 0)
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
            throw new NotImplementedException();
        }

        public void EndSubscription(string userStripeId, string subStripeId, bool cancelAtPeriodEnd = false)
        {
            this._subscriptionService.Cancel(userStripeId, subStripeId, cancelAtPeriodEnd);
        }

        public StripeSubscription UpdateSubscriptionAsync(string customerId, string subStripeId, CreditCard creditCard, string planId)
        {
            var myUpdatedSubscription = new StripeSubscriptionUpdateOptions
            {
                CardNumber = creditCard.CardNumber,
                CardExpirationYear = creditCard.ExpirationYear,
                CardExpirationMonth = creditCard.ExpirationMonth,
                CardAddressCountry = creditCard.AddressCountry,
                CardAddressLine1 = creditCard.AddressLine1,
                CardAddressLine2 = creditCard.AddressLine2,
                CardAddressState = creditCard.AddressState,
                CardAddressZip = creditCard.AddressZip,
                CardName = creditCard.Name,
                CardCvc = creditCard.Cvc,
                CardAddressCity = creditCard.AddressCity,
                PlanId = planId
            };

            return _subscriptionService.Update(customerId, subStripeId, myUpdatedSubscription);
        }
    }
}
