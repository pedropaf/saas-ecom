using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.DataServices.Interfaces;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Data.Models;
using Stripe;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe
{
    public class CardProvider : ICardProvider
    {
        // Data dependencies
        private readonly ICardDataService _cardDataService;

        // Stripe dependencies
        private readonly StripeCardService _cardService;

        public CardProvider(string apiKey, ICardDataService cardDataService)
        {
            this._cardDataService = cardDataService;
            this._cardService = new StripeCardService(apiKey);
        }

        public async Task<IList<CreditCard>> GetAllAsync(string customerId)
        {
            return await _cardDataService.GetAllAsync(customerId);
        }

        public async Task<CreditCard> FindAsync(string customerId, int? cardId)
        {
            return await _cardDataService.FindAsync(customerId, cardId);
        }

        public async Task AddAsync(ApplicationUser user, CreditCard card)
        {
            // Save to Stripe
            var stripeCustomerId = user.StripeCustomerId;
            AddCardToStripe(card, stripeCustomerId);

            // Save to storage
            card.ApplicationUserId = user.Id;
            await _cardDataService.AddAsync(card);
        }

        public async Task UpdateAsync(ApplicationUser user, CreditCard creditcard)
        {
            // Remove current card from stripe
            var currentCard = await _cardDataService.FindAsync(user.Id, creditcard.Id, true);
            var stripeCustomerId = user.StripeCustomerId;
            _cardService.Delete(stripeCustomerId, currentCard.StripeId);

            this.AddCardToStripe(creditcard, stripeCustomerId);

            // Update card in the DB
            creditcard.ApplicationUserId = user.Id;
            await _cardDataService.UpdateAsync(user.Id, creditcard);
        }
        
        public async Task<bool> CardBelongToUser(int cardId, string userId)
        {
            return await this._cardDataService.AnyAsync(cardId, userId);
        }

        public Task DeleteAsync(string customerId, int cardId)
        {
            throw new NotImplementedException();
        }
        
        private StripeCard AddCardToStripe(CreditCard card, string stripeCustomerId)
        {
            var options = new StripeCardCreateOptions
            {
                CardAddressCity = card.AddressCity,
                CardAddressCountry = card.AddressCountry,
                CardAddressLine1 = card.AddressLine1,
                CardAddressLine2 = card.AddressLine2,
                CardAddressState = card.AddressState,
                CardAddressZip = card.AddressZip,
                CardCvc = card.Cvc,
                CardExpirationMonth = card.ExpirationMonth,
                CardExpirationYear = card.ExpirationYear,
                CardName = card.Name,
                TokenId = card.StripeToken
            };

            return _cardService.Create(stripeCustomerId, options);
        }
    }
}
