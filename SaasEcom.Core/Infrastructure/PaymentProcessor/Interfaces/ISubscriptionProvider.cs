using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    /// <summary>
    /// Interface for subscription management with Stripe
    /// </summary>
    public interface ISubscriptionProvider
    {
        /// <summary>
        /// Subscribes the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <param name="trialInDays">The trial in days.</param>
        /// <param name="taxPercent">The tax percent.</param>
        void SubscribeUser(SaasEcomUser user, string planId, int trialInDays = 0, decimal taxPercent = 0);

        /// <summary>
        /// Gets the User's subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<List<Subscription>> UserSubscriptionsAsync(string userId);

        /// <summary>
        /// Ends the subscription.
        /// </summary>
        /// <param name="userStripeId">The user stripe identifier.</param>
        /// <param name="subStripeId">The sub stripe identifier.</param>
        /// <param name="cancelAtPeriodEnd">if set to <c>true</c> [cancel at period end].</param>
        void EndSubscription(string userStripeId, string subStripeId, bool cancelAtPeriodEnd = false);

        /// <summary>
        /// Updates the subscription. (Change subscription plan)
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="subStripeId">The sub stripe identifier.</param>
        /// <param name="newPlanId">The new plan identifier.</param>
        /// <returns></returns>
        bool UpdateSubscription(string customerId, string subStripeId, string newPlanId);

        /// <summary>
        /// Updates the subscription tax.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="subStripeId">The sub stripe identifier.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns></returns>
        bool UpdateSubscriptionTax(string customerId, string subStripeId, decimal taxPercent = 0);
    }
}
