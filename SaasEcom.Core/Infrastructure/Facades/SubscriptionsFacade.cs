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
        /// Creates a new user in Stripe and database.
        /// </summary>
        /// <param name="user">Application User</param>
        /// <param name="planId">Plan Id to subscribe the user to</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task SubscribeNewUserAsync(SaasEcomUser user, string planId, decimal taxPercent = 0)
        {
            // Subscribe the user to the plan
            var subscription = await _subscriptionDataService.SubscribeUserAsync(user, planId, trialPeriodInDays: null, taxPercent: taxPercent);

            // Create a new customer in Stripe and subscribe him to the plan
            var stripeUser = (StripeCustomer)await _customerProvider.CreateCustomerAsync(user, planId);

            // Add subscription Id to the user
            user.StripeCustomerId = stripeUser.Id;

            // Save subscription Id
            subscription.StripeId = GetStripeSubscriptionId(stripeUser);
            await _subscriptionDataService.UpdateSubscriptionAsync(subscription);
        
            // Update tax percent on stripe
            if (taxPercent > 0)
            {
                await this.UpdateSubscriptionTax(user, subscription.StripeId, taxPercent);
            }
        }

        /// <summary>
        /// Subscribes the user to stripe asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task SubscribeUserToStripeAsync(SaasEcomUser user)
        {
            var res = await _subscriptionDataService.UserSubscriptionsAsync(user.Id);

            if (res != null)
            {
                var subscription = res.First();

                // Create a new customer in Stripe and subscribe him to the plan
                var stripeUser = (StripeCustomer)await _customerProvider.CreateCustomerAsync(
                    user, subscription.SubscriptionPlanId, subscription.TrialEnd);

                // Add subscription Id to the user
                user.StripeCustomerId = stripeUser.Id;

                // Save subscription Id
                subscription.StripeId = GetStripeSubscriptionId(stripeUser);
                await _subscriptionDataService.UpdateSubscriptionAsync(subscription);

                // Update tax percent on stripe
                if (subscription.TaxPercent > 0)
                {
                    await this.UpdateSubscriptionTax(user, subscription.StripeId, subscription.TaxPercent);
                }
            }
        }

        /// <summary>
        /// Subscribes the user natural month asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <param name="card">The card.</param>
        /// <param name="description">The description.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns></returns>
        public async Task<string> SubscribeUserNaturalMonthAsync(SaasEcomUser user, 
            string planId, CreditCard card, string description, decimal taxPercent = 0)
        {
            var trialEnds = GetStartNextMonth();

            if (string.IsNullOrEmpty(user.StripeCustomerId))
            {
                // Create a new customer in Stripe and save card
                var stripeUser = (StripeCustomer) await _customerProvider.CreateCustomerAsync
                    (user, null, trialEnds , card.StripeToken);
                
                // Add subscription Id to the user
                user.StripeCustomerId = stripeUser.Id;

                await _cardDataService.AddAsync(card);
            }
            else if (!string.IsNullOrEmpty(card.StripeToken))
            {
                // Update the default card for the user
                _customerProvider.UpdateCustomer(user, card);
                card.SaasEcomUserId = user.Id;
                await _cardDataService.AddOrUpdateDefaultCardAsync(user.Id, card);
            }

            // Create pro-rata charge for the remaining of the month
            var amountInCents = await CalculateProRata(planId);

            var planCurrency = await GetPlanCurrency(planId);

            // If Charge succeeds -> Create Subscription
            string error;
            if (this._chargeProvider.CreateCharge(amountInCents, planCurrency, description, user.StripeCustomerId, out error))
            {
                // Create Subscription
                var subscriptionId = _subscriptionProvider.SubscribeUser(user, planId, trialEnds, taxPercent);
                var subscription = await _subscriptionDataService.SubscribeUserAsync
                    (user, planId, trialEnds, taxPercent, subscriptionId);

                // Update tax percent on stripe
                if (subscription.TaxPercent > 0)
                {
                    await this.UpdateSubscriptionTax(user, subscription.StripeId, subscription.TaxPercent);
                }
            }
            else
            {
                return error;
            }

            return null;
        }

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
            var endMonth = new DateTime (now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59);

            var totalHoursMonth = (endMonth - beginningMonth).TotalHours;
            var hoursRemaining = (endMonth - now).TotalHours;

            var amountInCurrency = plan.Price*hoursRemaining/totalHoursMonth;

            switch (plan.Currency.ToLower())
            {
                case("usd"):
                case("gbp"):
                case("eur"):
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
        /// <param name="trialInDays">The trial in days.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns></returns>
        public async Task SubscribeUserAsync(SaasEcomUser user, string planId, CreditCard creditCard, int trialInDays = 0, decimal taxPercent = 0)
        {
            // Save subscription details
            var subscriptionId = _subscriptionProvider.SubscribeUser
                (user, planId, trialInDays: trialInDays, taxPercent: taxPercent); // Stripe
            await this._subscriptionDataService.SubscribeUserAsync(user, planId, trialInDays, taxPercent, subscriptionId); // DB

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
        /// <param name="reasonToCancel">The reason to cancel.</param>
        /// <returns>The Date when the subscription ends (it can be future if cancelAtPeriodEnd is true)</returns>
        public async Task<DateTime?> EndSubscriptionAsync(int subscriptionId, SaasEcomUser user, bool cancelAtPeriodEnd = false, string reasonToCancel = null)
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

            if (activeSubscription != null && 
                (activeSubscription.SubscriptionPlan.Id != newPlanId || activeSubscription.End != null)) // Check end date in case that we are re-activating
            {
                // Update Stripe
                if (_subscriptionProvider.UpdateSubscription(stripeUserId, activeSubscription.StripeId, newPlanId))
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
        /// Deletes the subscriptions.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task DeleteSubscriptions(string userId)
        {
            await this._subscriptionDataService.DeleteSubscriptionsAsync(userId);
        }
    }
}
