using System.Collections.Generic;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    /// <summary>
    /// Interface for CRUD related to subscription plans with Stripe
    /// </summary>
    public interface ISubscriptionPlanProvider
    {
        /// <summary>
        /// Adds the specified plan.
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <returns></returns>
        object Add(SubscriptionPlan plan);

        /// <summary>
        /// Updates the specified plan.
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <returns></returns>
        object Update(SubscriptionPlan plan);

        /// <summary>
        /// Deletes the specified plan identifier.
        /// </summary>
        /// <param name="planId">The plan identifier.</param>
        void Delete(string planId);

        /// <summary>
        /// Finds the subscription plan asynchronous.
        /// </summary>
        /// <param name="planId">The plan identifier.</param>
        /// <returns></returns>
        SubscriptionPlan FindAsync(string planId);

        /// <summary>
        /// Gets all subscription plans asynchronous.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        IEnumerable<SubscriptionPlan> GetAllAsync(object options);
    }
}
