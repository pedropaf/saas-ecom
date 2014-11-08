using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    public interface ICardProvider
    {
        Task AddAsync(SaasEcomUser user, CreditCard card);
        Task UpdateAsync(SaasEcomUser user, CreditCard creditcard);
        Task DeleteAsync(string customerId, int cardId);
        Task<IList<CreditCard>> GetAllAsync(string customerId);
        Task<CreditCard> FindAsync(string userId, int? cardId);
        Task<bool> CardBelongToUser(int cardId, string userId);
    }
}
