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
    /// <summary>
    /// Subscriptions Facade to manage the subscription for your application's users.
    /// </summary>
    public class SubscriptionsFacade
    {
        private readonly ISubscriptionDataService _subscriptionDataService;
        private readonly ISubscriptionProvider _subscriptionProvider;
        private readonly ICardProvider _cardProvider;
        private readonly ICustomerProvider _customerProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionsFacade"/> class.
        /// </summary>
        /// <param name="data">The subscription data service.</param>
        /// <param name="subscriptionProvider">The subscription provider.</param>
        /// <param name="cardProvider">The card provider.</param>
        /// <param name="customerProvider">The customer provider.</param>
        public SubscriptionsFacade(ISubscriptionDataService data, ISubscriptionProvider subscriptionProvider, ICardProvider cardProvider, ICustomerProvider customerProvider)
        {
            _subscriptionDataService = data;
            _subscriptionProvider = subscriptionProvider;
            _cardProvider = cardProvider;
            _customerProvider = customerProvider;
        }

        /// <summary>
        /// Creates a new user in Stripe and database.
        /// </summary>
        /// <param name="user">Application User</param>
        /// <param name="planId">Plan Id to subscribe the user to</param>
        /// <returns>Task</returns>
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
        
        private string GetStripeSubscriptionId(StripeCustomer stripeUser)
        {
            return stripeUser.StripeSubscriptionList.TotalCount > 0 ? stripeUser.StripeSubscriptionList.StripeSubscriptions.First().Id : null;
        }

        /// <summary>
        /// Subscribe an existing user to a plan.
        /// </summary>
        /// <param name="user">Application User</param>
        /// <param name="planId">Stripe plan Id</param>
        /// <param name="creditCard">Credit card to pay this subscription.</param>
        /// <param name="trialInDays"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Cancel subscription from Stripe
        /// </summary>
        /// <param name="subscriptionId">Stripe subscription Id</param>
        /// <param name="user">Application user</param>
        /// <param name="cancelAtPeriodEnd">Cancel immediately or when the paid period ends (default immediately)</param>
        /// <returns></returns>
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

        /// <summary>
        /// Change Subscription Plan (Upgrade / Downgrade)
        /// </summary>
        /// <param name="userId">Application User Id</param>
        /// <param name="stripeUserId">Stripe User Id</param>
        /// <param name="newPlanId">New Subscription Plan Id</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the default payment credit card for a user.
        /// </summary>
        /// <param name="userId">Application User Id</param>
        /// <returns>Credit Card or Null</returns>
        public async Task<CreditCard> DefaultCreditCard(string userId)
        {
            return (await _cardProvider.GetAllAsync(userId)).FirstOrDefault();
        }

        /// <summary>
        /// Get a list of active subscriptions for the User
        /// </summary>
        /// <param name="userId">Application User Id</param>
        /// <returns>List of Active Subscriptions</returns>
        public async Task<List<Subscription>> UserActiveSubscriptionsAsync(string userId)
        {
            return await _subscriptionDataService.UserActiveSubscriptionsAsync(userId);
        }
    }
}
