using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Data.Models;
using Stripe;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe
{
    public class SubscriptionProvider : ISubscriptionProvider
    {
        private readonly StripeCustomerService _customerService;

        public SubscriptionProvider(string apiKey)
        {
            this._customerService = new StripeCustomerService(apiKey);
        }

        public void SubscribeUser(ApplicationUser user, string planId, int trialInDays = 0)
        {
            this._customerService.UpdateSubscription(user.StripeCustomerId,
                new StripeCustomerUpdateSubscriptionOptions
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
            this._customerService.CancelSubscription(userStripeId, /*subStripeId,*/ cancelAtPeriodEnd);
        }

        public StripeSubscription UpdateSubscriptionAsync(string customerId, CreditCard creditCard, string planId)
        {
            var myUpdatedSubscription = new StripeCustomerUpdateSubscriptionOptions
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

            return _customerService.UpdateSubscription(customerId, myUpdatedSubscription);
        }
    }
}
