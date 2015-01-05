using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Core.Models;
using Stripe;

namespace SaasEcom.Core.Infrastructure.Facades
{
    public class SubscriptionsFacade
    {
        private readonly ISubscriptionDataService _subscriptionDataService;
        private readonly ISubscriptionProvider _subscriptionProvider;
        private readonly ICardProvider _cardProvider;
        private readonly ICustomerProvider _customerProvider;

        public SubscriptionsFacade(ISubscriptionDataService data, ISubscriptionProvider subscriptionProvider, ICardProvider cardProvider, ICustomerProvider customerProvider)
        {
            _subscriptionDataService = data;
            _subscriptionProvider = subscriptionProvider;
            _cardProvider = cardProvider;
            _customerProvider = customerProvider;
        }

        public async Task SubscribeNewUserAsync(SaasEcomUser user, string planId)
        {
            // Subscribe the user to the plan
            var subscription = await _subscriptionDataService.SubscribeUserAsync(user, planId);

            // Create a new customer in Stripe and subscribe him to the plan
            var stripeUser = (StripeCustomer)await _customerProvider.CreateCustomerAsync(user, planId);

            // Add subscription Id to the user
            user.StripeCustomerId = stripeUser.Id;

            subscription.StripeId = GetStripeSubscriptionId(stripeUser);
            await _subscriptionDataService.UpdateSubscriptionAsync(subscription);
        }
        
        private string GetStripeSubscriptionId(Stripe.StripeCustomer stripeUser)
        {
            return stripeUser.StripeSubscriptionList.TotalCount > 0 ? stripeUser.StripeSubscriptionList.StripeSubscriptions.First().Id : null;
        }

        public async Task SubscribeUserAsync(SaasEcomUser user, string planId, CreditCard creditCard, int trialInDays = 0)
        {
            // Save subscription details
            _subscriptionProvider.SubscribeUser(user, planId, trialInDays); // Stripe
            await this._subscriptionDataService.SubscribeUserAsync(user, planId, trialInDays); // DB

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

        public async Task<bool> EndSubscriptionAsync(int subscriptionId, SaasEcomUser user, bool cancelAtPeriodEnd = false)
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

        public async Task<bool> UpdateSubscriptionAsync(string userId, string stripeUserId, string newPlanId)
        {
            var activeSubscription = await _subscriptionDataService.UserActiveSubscriptionAsync(userId);

            if (activeSubscription != null && activeSubscription.SubscriptionPlan.Id != newPlanId)
            {
                // Update Stripe
                if (_subscriptionProvider.UpdateSubscription(stripeUserId, activeSubscription.StripeId, newPlanId))
                {
                    // Update DB
                    activeSubscription.SubscriptionPlan = null;
                    activeSubscription.SubscriptionPlanId = newPlanId;
                    await _subscriptionDataService.UpdateSubscriptionAsync(activeSubscription);
                    return true;
                }
            }

            return false;
        }

        public async Task<CreditCard> DefaultCreditCard(string userId)
        {
            return (await _cardProvider.GetAllAsync(userId)).FirstOrDefault();
        }

        public async Task<List<Subscription>> UserActiveSubscriptionsAsync(string userId)
        {
            return await _subscriptionDataService.UserActiveSubscriptionsAsync(userId);
        }
    }
}
