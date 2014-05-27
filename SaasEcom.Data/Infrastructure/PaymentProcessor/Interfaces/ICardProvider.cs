using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Interfaces
{
    public interface ICardProvider
    {
        Task AddAsync(ApplicationUser user, CreditCard card);
        Task UpdateAsync(ApplicationUser user, CreditCard creditcard);
        Task DeleteAsync(string customerId, int cardId);
        Task<IList<CreditCard>> GetAllAsync(string customerId);

        Task<CreditCard> FindAsync(string userId, int? cardId);
        Task<bool> CardBelongToUser(int cardId, string userId);
    }
}
