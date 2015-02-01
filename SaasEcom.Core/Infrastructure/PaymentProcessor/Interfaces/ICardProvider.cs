using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    /// <summary>
    /// Interface for CRUD related to credit cards with Stripe
    /// </summary>
    public interface ICardProvider
    {
        /// <summary>
        /// Adds the credit card asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        Task AddAsync(SaasEcomUser user, CreditCard card);

        /// <summary>
        /// Updates the credit card asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="creditcard">The creditcard.</param>
        /// <returns></returns>
        Task UpdateAsync(SaasEcomUser user, CreditCard creditcard);

        /// <summary>
        /// Deletes the credit card asynchronous.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="custStripeId">The customer stripe identifier.</param>
        /// <param name="cardId">The card identifier.</param>
        /// <returns></returns>
        Task DeleteAsync(string customerId, string custStripeId, int cardId);

        /// <summary>
        /// Gets all the credit cards asynchronous.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>The list of credit cards</returns>
        Task<IList<CreditCard>> GetAllAsync(string customerId);

        /// <summary>
        /// Finds the credit card asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cardId">The card identifier.</param>
        /// <returns>The credit card</returns>
        Task<CreditCard> FindAsync(string userId, int? cardId);

        /// <summary>
        /// Check if the Card belong to user.
        /// </summary>
        /// <param name="cardId">The card identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true or false</returns>
        Task<bool> CardBelongToUser(int cardId, string userId);
    }
}
