using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Storage
{
    /// <summary>
    /// Implementation for CRUD related to credit cards in the database.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    public class CardDataService<TContext, TUser> : ICardDataService
        where TContext : IDbContext<TUser>
        where TUser : SaasEcomUser
    {
        private readonly TContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardDataService{TContext, TUser}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CardDataService(TContext context)
        {
            this._dbContext = context;
        }

        /// <summary>
        /// Gets all credit cards for an user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Customer Id: {0} doesn't exist.</exception>
        public async Task<IList<CreditCard>> GetAllAsync(string userId)
        {
            var user = await this._dbContext.Users.Include(u => u.CreditCards)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentException("Customer Id: {0} doesn't exist.", userId);
            }

            return user.CreditCards;
        }

        /// <summary>
        /// Finds the credit card.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cardId">The card identifier.</param>
        /// <param name="noTracking">if set to <c>true</c> [no tracking].</param>
        /// <returns></returns>
        public async Task<CreditCard> FindAsync(string userId, int? cardId, bool noTracking = false)
        {
            if (noTracking)
            {
                return await this._dbContext.CreditCards.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.SaasEcomUserId == userId && c.Id == cardId);
            }

            return await this._dbContext.CreditCards
                .FirstOrDefaultAsync(c => c.SaasEcomUserId == userId && c.Id == cardId);
        }

        /// <summary>
        /// Checks if there is any card existing in the DB.
        /// </summary>
        /// <param name="cardId">The card identifier.</param>
        /// <param name="userId">The customer identifier.</param>
        /// <returns>
        /// bool
        /// </returns>
        public async Task<bool> AnyAsync(int? cardId, string userId)
        {
            return await this._dbContext.CreditCards
                .AnyAsync(c => c.SaasEcomUserId == userId && c.Id == cardId);
        }

        /// <summary>
        /// Adds the credit card.
        /// </summary>
        /// <param name="creditCard">The credit card.</param>
        /// <returns></returns>
        public async Task AddAsync(CreditCard creditCard)
        {
            _dbContext.CreditCards.Add(creditCard);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddOrUpdateDefaultCardAsync(string userId, CreditCard creditCard)
        {
            var card = this._dbContext.CreditCards.FirstOrDefault(c => c.SaasEcomUserId == userId);

            if (card != null)
            {
                _dbContext.CreditCards.Remove(card);
                await _dbContext.SaveChangesAsync();
            }

            await this.AddAsync(creditCard);
        }

        /// <summary>
        /// Updates the credit card.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="creditCard">The credit card.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">cardId</exception>
        public async Task UpdateAsync(string userId, CreditCard creditCard)
        {
            if (!this._dbContext.CreditCards.Any(c => c.SaasEcomUserId == userId && c.Id == creditCard.Id))
            {
                throw new ArgumentException("cardId");
            }

            _dbContext.Entry(creditCard).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the credit card.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cardId">The card identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">cardId</exception>
        public async Task DeleteAsync(string userId, int cardId)
        {
            CreditCard creditcard = await this._dbContext.CreditCards
                .FirstOrDefaultAsync(c => c.SaasEcomUserId == userId && c.Id == cardId);

            if (creditcard == null)
            {
                throw new ArgumentException("cardId");
            }

            _dbContext.CreditCards.Remove(creditcard);
            await _dbContext.SaveChangesAsync();
        }
    }
}
