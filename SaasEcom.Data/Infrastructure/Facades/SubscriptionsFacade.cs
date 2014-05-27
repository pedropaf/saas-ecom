using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Data.DataServices.Interfaces;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.Infrastructure.Facades
{
    public class SubscriptionsFacade
    {
        private readonly ISubscriptionDataService _subscriptionDataService;
        private readonly ISubscriptionProvider _subscriptionProvider;
        private readonly ICardProvider _cardProvider;

        public SubscriptionsFacade(ISubscriptionDataService data, ISubscriptionProvider subscriptionProvider, ICardProvider cardProvider)
        {
            _subscriptionDataService = data;
            _subscriptionProvider = subscriptionProvider;
            _cardProvider = cardProvider;
        }

        public async Task SubscribeUserAsync(ApplicationUser user, string planId, CreditCard creditCard, int trialInDays = 0)
        {
            // Save subscription details
            _subscriptionProvider.SubscribeUser(user, planId, trialInDays);
            await this._subscriptionDataService.SubscribeUserAsync(user, planId, trialInDays);

            // Save payment details
            if (creditCard.Id == 0)
            {
                await _cardProvider.AddAsync(user, creditCard);
            }
            else
            {
                await _cardProvider.UpdateAsync(user, creditCard);
            }
        }

        public async Task<bool> EndSubscriptionAsync(int subscriptionId, ApplicationUser user, bool cancelAtPeriodEnd = false)
        {
            bool res = true;
            try
            {
                var subscription = await _subscriptionDataService.UserActiveSubscriptionAsync(user.Id);
                if (subscription != null && subscription.Id == subscriptionId)
                {
                    await _subscriptionDataService.EndSubscriptionAsync(subscriptionId);
                    _subscriptionProvider.EndSubscription(user.StripeCustomerId, subscription.StripeId, cancelAtPeriodEnd);
                }
            }
            catch (Exception)
            {
                // TODO: Log
                res = false;
            }

            return res;
        }

        public async Task<CreditCard> DefaultCreditCard(string userId)
        {
            return (await _cardProvider.GetAllAsync(userId)).FirstOrDefault() ?? new CreditCard();
        }

        public async Task<List<Subscription>> UserActiveSubscriptionsAsync(string userId)
        {
            return await _subscriptionDataService.UserActiveSubscriptionsAsync(userId);
        }
    }
}
