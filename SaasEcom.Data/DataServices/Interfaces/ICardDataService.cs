using SaasEcom.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaasEcom.Data.DataServices.Interfaces
{
    public interface ICardDataService
    {
        Task<IList<CreditCard>> GetAllAsync(string customerId);
        Task<CreditCard> FindAsync(string customerId, int? cardId);
        Task AddAsync(CreditCard creditcard);
        Task UpdateAsync(string customerId, CreditCard creditcard);
        Task DeleteAsync(string customerId, int cardId);
    }
}
