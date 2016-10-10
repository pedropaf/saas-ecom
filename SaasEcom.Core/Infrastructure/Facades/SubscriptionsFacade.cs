using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.DataServices.Storage;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Core.Models;
using Stripe;

namespace SaasEcom.Core.Infrastructure.Facades
{
    /// <summary>
    /// Subscriptions Facade to manage the subscription for your application's users.
    /// </summary>
    /// <remarks>
    /// 	<para>This is one of the main classes that you will instantiate from your application to interact with SaasEcom.Core. This class is using internally the data
    /// services to store all the billing related data in the database, as well as the Payment Provider to inegrate all the billing data with Stripe and keep it in
    /// sync.</para>
    /// </remarks>
    public class SubscriptionsFacade
    {
        private readonly ISubscriptionDataService _subscriptionDataService;
        private readonly ISubscriptionPlanDataService _subscriptionPlanDataService;
        private readonly ISubscriptionProvider _subscriptionProvider;
        private readonly ICustomerProvider _customerProvider;
        private readonly IChargeProvider _chargeProvider;
        private readonly ICardProvider _cardProvider;
        private readonly ICardDataService _cardDataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionsFacade" /> class.
        /// </summary>
        /// <param name="data">The subscription data service.</param>
        /// <param name="subscriptionProvider">The subscription provider.</param>
        /// <param name="cardProvider">The card provider.</param>
        /// <param name="cardDataService">The card data service.</param>
        /// <param name="customerProvider">The customer provider.</param>
        /// <param name="subscriptionPlanDataService">The subscription plan data service.</param>
        /// <param name="chargeProvider">The charge provider.</param>
        public SubscriptionsFacade(
            ISubscriptionDataService data,
            ISubscriptionProvider subscriptionProvider,
            ICardProvider cardProvider,
            ICardDataService cardDataService,
            ICustomerProvider customerProvider,
            ISubscriptionPlanDataService subscriptionPlanDataService,
            IChargeProvider chargeProvider)
        {
            _subscriptionDataService = data;
            _subscriptionProvider = subscriptionProvider;
            _cardProvider = cardProvider;
            _customerProvider = customerProvider;
            _subscriptionPlanDataService = subscriptionPlanDataService;
            _chargeProvider = chargeProvider;
            _cardDataService = cardDataService;
        }

        /// <summary>
        /// Subscribes the user to a Stripe plan. If the user doesn't exist in Stripe, is created
        /// </summary>
        /// <param name="user">Application User</param>
        /// <param name="planId">Plan Id to subscribe the user to</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <param name="creditCard">The credit card.</param>
        /// <returns>
        /// Subscription
        /// </returns>
        public async Task<Subscription> SubscribeUserAsync
            (SaasEcomUser user, string planId, decimal taxPercent = 0, CreditCard creditCard = null)
        {
            Subscription subscription;
            
            // If the user isn't created in Stripe 
            if (string.IsNullOrEmpty(user.StripeCustomerId))
            {
                // Save the subscription in the DB
                subscription = await _subscriptionDataService.SubscribeUserAsync(user, planId, trialPeriodInDays: null, taxPercent: taxPercent);

                // Create a new customer in Stripe and subscribe him to the plan
                var cardToken = creditCard == null ? null : creditCard.StripeToken;
                var stripeUser = (StripeCustomer) await _customerProvider.CreateCustomerAsync(user, planId, null, cardToken);
                user.StripeCustomerId = stripeUser.Id; // Add stripe user Id to the user

                // Save Stripe Subscription Id in the DB
                subscription.StripeId = GetStripeSubscriptionIdForNewCustomer(stripeUser);
                await _subscriptionDataService.UpdateSubscriptionAsync(subscription);
            }
            else // Create new subscription in Stripe and DB
            {
                subscription = await this.SubscribeUserAsync(user, planId, creditCard, 0, taxPercent: taxPercent);
            }

            // Update tax percent on stripe
            if (taxPercent > 0)
            {
                await this.UpdateSubscriptionTax(user, subscription.StripeId, taxPercent);
            }

            return subscription;
        }

        /// <summary>
        /// Subscribe an existing user to a plan.
        /// </summary>
        /// <param name="user">Application User</param>
        /// <param name="planId">Stripe plan Id</param>
        /// <param name="creditCard">Credit card to pay this subscription.</param>
        /// <param name="trialInDays">The trial in days.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns></returns>
        private async Task<Subscription> SubscribeUserAsync(SaasEcomUser user, string planId, CreditCard creditCard, int trialInDays = 0, decimal taxPercent = 0)
        {
            // Save payment details
            if (creditCard != null)
            {
                if (creditCard.Id == 0)
                {
                    await _cardProvider.AddAsync(user, creditCard);
                }
                else
                {
                    await _cardProvider.UpdateAsync(user, creditCard);
                }
            }

            // Save subscription details
            var subscriptionId = _subscriptionProvider.SubscribeUser
                (user, planId, trialInDays: trialInDays, taxPercent: taxPercent); // Stripe
            var subscription = await this._subscriptionDataService.SubscribeUserAsync(user, planId, trialInDays, taxPercent, subscriptionId); // DB

            return subscription;
        }


        /// <summary>
        /// Updates the subscription tax.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="subscriptionId">The subscription stripe identifier.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns>boolean</returns>
        public async Task<bool> UpdateSubscriptionTax(SaasEcomUser user, string subscriptionId, decimal taxPercent)
        {
            // DB
            await _subscriptionDataService.UpdateSubscriptionTax(subscriptionId, taxPercent);

            // Stripe
            return _subscriptionProvider.UpdateSubscriptionTax(user.StripeCustomerId, subscriptionId, taxPercent);
        }

        /// <summary>
        /// Cancel subscription from Stripe
        /// </summary>
        /// <param name="subscriptionId">Stripe subscription Id</param>
        /// <param name="user">Application user</param>
        /// <param name="cancelAtPeriodEnd">Cancel immediately or when the paid period ends (default immediately)</param>
        /// <param name="reasonToCancel">The reason to cancel.</param>
        /// <returns>The Date when the subscription ends (it can be future if cancelAtPeriodEnd is true)</returns>
        public async Task<DateTime?> EndSubscriptionAsync(int subscriptionId, 
            SaasEcomUser user, bool cancelAtPeriodEnd = false, string reasonToCancel = null)
        {
            DateTime? subscriptionEnd = null;
            try
            {
                var subscription = await _subscriptionDataService.UserActiveSubscriptionAsync(user.Id);
                if (subscription != null && subscription.Id == subscriptionId)
                {
                    subscriptionEnd = _subscriptionProvider.EndSubscription(user.StripeCustomerId, subscription.StripeId, cancelAtPeriodEnd);

                    await _subscriptionDataService.EndSubscriptionAsync(subscriptionId, subscriptionEnd.Value, reasonToCancel);
                }
            }
            catch (Exception)
            {
                // TODO: Log
                subscriptionEnd = null;
            }

            return subscriptionEnd;
        }

        // TODO: Maybe remove this method?
        /// <summary>
        /// Change Subscription Plan (Upgrade / Downgrade) (When the user can have only one active subscription)
        /// </summary>
        /// <param name="userId">Application User Id</param>
        /// <param name="stripeUserId">Stripe User Id</param>
        /// <param name="newPlanId">New Subscription Plan Id</param>
        /// <param name="proRate">if set to <c>true</c> [pro rate].</param>
        /// <returns></returns>
        public async Task<bool> UpdateSubscriptionAsync(string userId, string stripeUserId, string newPlanId, bool proRate = true)
        {
            var activeSubscription = await _subscriptionDataService.UserActiveSubscriptionAsync(userId);

            if (activeSubscription != null &&
                (activeSubscription.SubscriptionPlan.Id != newPlanId || activeSubscription.End != null)) // Check end date in case that we are re-activating
            {
                // Update Stripe
                if (_subscriptionProvider.UpdateSubscription(stripeUserId, activeSubscription.StripeId, newPlanId, proRate))
                {
                    // Update DB
                    activeSubscription.SubscriptionPlanId = newPlanId;
                    activeSubscription.End = null; // In case that we are reactivating
                    await _subscriptionDataService.UpdateSubscriptionAsync(activeSubscription);
                    return true;
                }
            }

            return false;
        }

        // TODO: Remove UserId (not used)
        /// <summary>
        /// Updates the subscription asynchronous, if the new plan is more expensive the customer is charged immediately
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="stripeUserId">The stripe user identifier.</param>
        /// <param name="stripeSubscriptionId">The current subscription stripe identifier.</param>
        /// <param name="newPlanId">The new plan identifier.</param>
        /// <param name="proRate">if set to <c>true</c> [pro rate].</param>
        /// <returns></returns>
        public async Task<bool> UpdateSubscriptionAsync(string userId, string stripeUserId, string stripeSubscriptionId, string newPlanId, bool proRate = true)
        {
            var subscription = _subscriptionDataService.FindById(stripeSubscriptionId);

            if (subscription != null &&
                (subscription.SubscriptionPlan.Id != newPlanId || subscription.End != null)) // Check end date in case that we are re-activating
            {
                bool changingPlan = subscription.SubscriptionPlan.Id != newPlanId;

                var currentPlan = await _subscriptionPlanDataService.FindAsync(subscription.SubscriptionPlanId);
                var newPlan = await _subscriptionPlanDataService.FindAsync(newPlanId);

                // Do Stripe charge if the new plan is more expensive
                if (changingPlan && currentPlan.Price < newPlan.Price)
                {
                    var upgradeCharge = await CalculateProRata(newPlanId) - await CalculateProRata(subscription.SubscriptionPlanId);

                    var upgradeChargeWithTax = upgradeCharge*(1 + subscription.TaxPercent/100);

                    string error;
                    _chargeProvider.CreateCharge((int)upgradeChargeWithTax, await GetPlanCurrency(newPlanId), "Fluxifi Upgrade", stripeUserId, out error);

                    if (!string.IsNullOrEmpty(error))
                    {
                        return false;
                    }
                }
                
                // Update Stripe
                if (_subscriptionProvider.UpdateSubscription(stripeUserId, subscription.StripeId, newPlanId, proRate))
                {
                    // Update DB
                    subscription.SubscriptionPlanId = newPlanId;
                    subscription.End = null; // In case that we are reactivating
                    await _subscriptionDataService.UpdateSubscriptionAsync(subscription);
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

        // TODO: Pass the subscription Id
        /// <summary>This method returns the number of days of trial left for a given user. It will return 0 if there aren't any days left or no active subscriptions for the user.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception caption="" cref="System.NotImplementedException"></exception>
        public async Task<int> DaysTrialLeftAsync(string userId)
        {
            var currentSubscription = (await this.UserActiveSubscriptionsAsync(userId)).FirstOrDefault();

            if (currentSubscription == null)
            {
                return 0;
            }
            else if (currentSubscription.IsTrialing())
            {
                var currentDate = DateTime.UtcNow;
                TimeSpan? timeSpan = currentSubscription.TrialEnd - currentDate;

                return timeSpan.Value.Hours > 12 ? timeSpan.Value.Days + 1 : timeSpan.Value.Days;
            }

            return 0;
        }

        /// <summary>
        /// Subscribes the user, with a billing cycle that goes from the 1st of the month asynchronous.
        /// Creates the user in Stripe if doesn't exist already.
        /// Saves de Subscription data in the database if the subscription suceeds.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <param name="card">The card.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns></returns>
        public async Task SubscribeUserNaturalMonthAsync(SaasEcomUser user, string planId, CreditCard card, decimal taxPercent = 0)
        {
            if (string.IsNullOrEmpty(user.StripeCustomerId))
            {
                // Create a new customer in Stripe and save card
                var stripeUser = (StripeCustomer)await _customerProvider.CreateCustomerAsync(user, cardToken: card.StripeToken);
                user.StripeCustomerId = stripeUser.Id;
                card.SaasEcomUserId = user.Id;
                await _cardDataService.AddAsync(card);
            }
            else if (card != null && !string.IsNullOrEmpty(card.StripeToken))
            {
                // Update the default card for the user
                var customer = (StripeCustomer)_customerProvider.UpdateCustomer(user, card);
                card.SaasEcomUserId = user.Id;
                card.StripeId = customer.DefaultSourceId;
                await _cardDataService.AddOrUpdateDefaultCardAsync(user.Id, card);
            }

            var stripeSubscription = (StripeSubscription)_subscriptionProvider.SubscribeUserNaturalMonth(user, planId, GetStartNextMonth(), taxPercent);
            await _subscriptionDataService.SubscribeUserAsync(user, planId, (int?)null, taxPercent, stripeSubscription.Id);
        }

        /// <summary>
        /// Deletes the subscriptions.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task DeleteSubscriptions(string userId)
        {
            await this._subscriptionDataService.DeleteSubscriptionsAsync(userId);
        }

        #region Helpers
        private async Task<string> GetPlanCurrency(string planId)
        {
            var plan = await _subscriptionPlanDataService.FindAsync(planId);

            return plan.Currency;
        }

        private async Task<int> CalculateProRata(string planId)
        {
            var plan = await _subscriptionPlanDataService.FindAsync(planId);

            var now = DateTime.UtcNow;
            var beginningMonth = new DateTime(now.Year, now.Month, 1);
            var endMonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59);

            var totalHoursMonth = (endMonth - beginningMonth).TotalHours;
            var hoursRemaining = (endMonth - now).TotalHours;

            var amountInCurrency = plan.Price * hoursRemaining / totalHoursMonth;

            switch (plan.Currency.ToLower())
            {
                case ("usd"):
                case ("gbp"):
                case ("eur"):
                    return (int)Math.Ceiling(amountInCurrency * 100);
                default:
                    return (int)Math.Ceiling(amountInCurrency);
            }
        }

        private DateTime? GetStartNextMonth()
        {
            var now = DateTime.UtcNow;
            var year = now.Month == 12 ? now.Year + 1 : now.Year;
            var month = now.Month == 12 ? 1 : now.Month + 1;

            return new DateTime(year, month, 1);
        }

        private string GetStripeSubscriptionIdForNewCustomer(StripeCustomer stripeUser)
        {
            return stripeUser.StripeSubscriptionList.TotalCount > 0 ? 
                stripeUser.StripeSubscriptionList.Data.First().Id : null;
        }
        #endregion
    }
}
