using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Storage
{
    public class CardDataService : ICardDataService
    {
        private readonly IDbContext _dbContext;

        public CardDataService(IDbContext context)
        {
            this._dbContext = context;
        }

        public async Task<IList<CreditCard>> GetAllAsync(string customerId)
        {
            var user = await this._dbContext.Users.Include(u => u.CreditCards)
                .FirstOrDefaultAsync(u => u.Id == customerId);

            if (user == null)
            {
                throw new ArgumentException("Customer Id: {0} doesn't exist.", customerId);
            }

            return user.CreditCards;
        }

        public async Task<CreditCard> FindAsync(string customerId, int? cardId, bool noTracking = false)
        {
            if (noTracking)
            {
                return await this._dbContext.CreditCards.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.ApplicationUserId == customerId && c.Id == cardId);
            }

            return await this._dbContext.CreditCards
                .FirstOrDefaultAsync(c => c.ApplicationUserId == customerId && c.Id == cardId);
        }

        public async Task<bool> AnyAsync(int? cardId, string customerId)
        {
            return await this._dbContext.CreditCards
                .AnyAsync(c => c.ApplicationUserId == customerId && c.Id == cardId);
        }

        public async Task AddAsync(CreditCard creditcard)
        {
            _dbContext.CreditCards.Add(creditcard);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(string customerId, CreditCard creditcard)
        {
            if (!this._dbContext.CreditCards.Any(c => c.ApplicationUserId == customerId && c.Id == creditcard.Id))
            {
                throw new ArgumentException("cardId");
            }

            _dbContext.Entry(creditcard).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string customerId, int cardId)
        {
            CreditCard creditcard = await this._dbContext.CreditCards
                .FirstOrDefaultAsync(c => c.ApplicationUserId == customerId && c.Id == cardId);

            if (creditcard == null)
            {
                throw new ArgumentException("cardId");
            }

            _dbContext.CreditCards.Remove(creditcard);
            await _dbContext.SaveChangesAsync();
        }
    }
}
