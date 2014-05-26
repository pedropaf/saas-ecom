using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.DataServices.Interfaces;
using SaasEcom.Data.Models;
using Stripe;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe
{
    public class SubscriptionService
    {
        private readonly StripeCardService _cardService;
        private readonly StripeCustomerService _customerService;

        private readonly ICardService _cardDataService;
        private readonly ISubscriptionService _subscriptionDataService;

        public SubscriptionService(string apiKey, ICardService cardDataService, ISubscriptionService subscriptionService)
        {
            this._cardService = new StripeCardService(apiKey);
            this._customerService = new StripeCustomerService(apiKey);
            
            this._cardDataService = cardDataService;
            this._subscriptionDataService = subscriptionService;
        }

        public async Task SubscribeUserAsync(ApplicationUser user, string planId, int trialInDays = 0)
        {
            // Subscribe to stripe
            this._customerService.UpdateSubscription(user.StripeCustomerId,
                new StripeCustomerUpdateSubscriptionOptions
                {
                    PlanId = planId,
                    TrialEnd = DateTime.UtcNow.AddDays(trialInDays)
                });

            // Subscribe to DB
            await this._subscriptionDataService.SubscribeUserAsync(user, planId, trialInDays);
        }

        public Task<List<Subscription>> UserSubscriptionsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EndSubscriptionAsync(int subscriptionId, ApplicationUser user, bool cancelAtPeriodEnd = false)
        {
            bool res = true;
            try
            {
                var subscription = await _subscriptionDataService.UserActiveSubscriptionAsync(user.Id);
                if (subscription != null && subscription.Id == subscriptionId)
                {
                    // Cancel in DB
                    await _subscriptionDataService.EndSubscriptionAsync(subscriptionId);

                    // Cancel subscription in stripe
                    this._customerService.CancelSubscription(user.StripeCustomerId, /*subscription.StripeId,*/ cancelAtPeriodEnd);
                }
            }
            catch (Exception)
            {
                // TODO: Log
                res = false;
            }

            return res;
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
