using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe
{
    public class CardService
    {
        private readonly StripePaymentProcessorProvider _stripeService;

        public CardService(string apiKey)
        {
            this._stripeService = new StripePaymentProcessorProvider(apiKey);
        }

        public Task<IList<Models.CreditCard>> GetAllAsync(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<Models.CreditCard> FindAsync(string customerId, int? cardId)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(ApplicationUser user, CreditCard creditcard)
        {
            var stripeCustomerId = user.StripeCustomerId;
            this._stripeService.AddCard(stripeCustomerId, creditcard);
            
            return null;
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
