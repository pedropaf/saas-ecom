using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaasEcom.Data.DataServices.Interfaces;

namespace SaasEcom.Data.DataServices.Stripe
{
    public class StripeCardService : ICardService
    {
        public Task<IList<Models.CreditCard>> GetAllAsync(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<Models.CreditCard> FindAsync(string customerId, int? cardId)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Models.CreditCard creditcard)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string customerId, Models.CreditCard creditcard)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string customerId, int cardId)
        {
            throw new NotImplementedException();
        }
    }
}
