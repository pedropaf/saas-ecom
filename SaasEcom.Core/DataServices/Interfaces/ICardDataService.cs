using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Interfaces
{
    /// <summary>
    /// Interface for CRUD related to credit cards in the database.
    /// </summary>
    public interface ICardDataService
    {
        /// <summary>
        /// 	<para>Gets all credit cards for an user from the database.</para>
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IList<CreditCard>> GetAllAsync(string userId);

        /// <summary>Finds the credit card asynchronously from the database given the user identifier and credit card identifier.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cardId">The card identifier.</param>
        /// <param name="noTracking">if set to <c>true</c> [no tracking].</param>
        /// <returns></returns>
        Task<CreditCard> FindAsync(string userId, int? cardId, bool noTracking = false);

        /// <summary>
        /// 	<para>Adds the credit card asynchronously to the database.</para>
        /// </summary>
        /// <param name="creditcard">The creditcard.</param>
        /// <returns></returns>
        Task AddAsync(CreditCard creditcard);

        /// <summary>
        /// Adds or update default card asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="creditCard">The credit card.</param>
        /// <returns></returns>
        Task AddOrUpdateDefaultCardAsync(string userId, CreditCard creditCard);

        /// <summary>Updates the credit card for a user in the database.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="creditCard">The credit card.</param>
        /// <returns></returns>
        Task UpdateAsync(string userId, CreditCard creditCard);

        /// <summary>Deletes the credit card from the database.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cardId">The card identifier.</param>
        /// <returns></returns>
        Task DeleteAsync(string userId, int cardId);

        /// <summary>Checks if there is any card existing in the DB given the card identifier and user identifier.</summary>
        /// <param name="cardId">The card identifier.</param>
        /// <param name="userId">The customer identifier.</param>
        /// <returns>bool</returns>
        Task<bool> AnyAsync(int? cardId, string userId);
    }
}
