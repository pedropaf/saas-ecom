using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Interfaces
{
    /// <summary>
    /// Interface for CRUD related to subscriptions in the database.
    /// </summary>
    public interface ISubscriptionDataService
    {
        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="stripeSubscriptionId">The stripe subscription identifier.</param>
        /// <returns></returns>
        Subscription FindById(string stripeSubscriptionId);

        /// <summary>
        /// Subscribes the user asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <param name="trialPeriodInDays">The trial period in days.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <param name="stripeId">The stripe identifier.</param>
        /// <returns>
        /// The subscription
        /// </returns>
        Task<Subscription> SubscribeUserAsync(SaasEcomUser user, string planId, int? trialPeriodInDays = null,
            decimal taxPercent = 0, string stripeId = null);

        /// <summary>
        /// Subscribes the user asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <param name="trialPeriodEnds">The trial period ends.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <param name="stripeId">The stripe identifier.</param>
        /// <returns>
        /// The subscription
        /// </returns>
        Task<Subscription> SubscribeUserAsync(SaasEcomUser user, string planId, DateTime? trialPeriodEnds = null,
            decimal taxPercent = 0, string stripeId = null);

        /// <summary>
        /// Gets the User's subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<List<Subscription>> UserSubscriptionsAsync(string userId);

        /// <summary>
        /// Get the User's active subscription asynchronous. Only the first (valid if your customers can have only 1 subscription at a time).
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The subscription</returns>
        Task<Subscription> UserActiveSubscriptionAsync(string userId);

        /// <summary>
        /// Get the User's active subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The list of Subscriptions</returns>
        Task<List<Subscription>> UserActiveSubscriptionsAsync(string userId);

        /// <summary>
        /// Ends the subscription asynchronous.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="subscriptionEnDateTime">The subscription en date time.</param>
        /// <param name="reasonToCancel">The reason to cancel.</param>
        /// <returns></returns>
        Task EndSubscriptionAsync(int subscriptionId, DateTime subscriptionEnDateTime, string reasonToCancel);

        /// <summary>
        /// Updates the subscription asynchronous.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <returns></returns>
        Task UpdateSubscriptionAsync(Subscription subscription);

        /// <summary>
        /// Updates the subscription tax.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns></returns>
        Task UpdateSubscriptionTax(string subscriptionId, decimal taxPercent);

        /// <summary>
        /// Deletes the subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task DeleteSubscriptionsAsync(string userId);
    }
}
