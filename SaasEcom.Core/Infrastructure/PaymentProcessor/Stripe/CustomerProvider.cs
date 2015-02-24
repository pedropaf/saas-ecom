using System;
using System.Threading.Tasks;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Core.Models;
using Stripe;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe
{
    /// <summary>
    /// Interface for CRUD related to customers with Stripe
    /// </summary>
    public class CustomerProvider : ICustomerProvider
    {
        // Stripe Dependencies
        private readonly StripeCustomerService _customerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerProvider"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public CustomerProvider(string apiKey)
        {
            _customerService = new StripeCustomerService(apiKey);
        }

        /// <summary>
        /// Creates the customer asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <param name="trialEnd">The trial end.</param>
        /// <param name="cardToken">The card token.</param>
        /// <returns></returns>
        public async Task<object> CreateCustomerAsync(SaasEcomUser user, string planId = null, DateTime? trialEnd = null, string cardToken = null)
        {
            var customer = new StripeCustomerCreateOptions
            {
                AccountBalance = 0,
                Email = user.Email
            };

            if (!string.IsNullOrEmpty(cardToken))
            {
                customer.Card = new StripeCreditCardOptions
                {
                    TokenId = cardToken
                };
            }

            if (!string.IsNullOrEmpty(planId))
            {
                customer.PlanId = planId;
                customer.TrialEnd = trialEnd;
            }

            var stripeUser = await Task.Run(() => _customerService.Create(customer));
            return stripeUser;
        }

        /// <summary>
        /// Updates the customer.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public object UpdateCustomer(SaasEcomUser user, CreditCard card)
        {
            var customer = new StripeCustomerUpdateOptions
            {
                Email = user.Email,

                // Card Details
                Card =  new StripeCreditCardOptions
                {
                    TokenId = card.StripeToken
                }
            };

            return _customerService.Update(user.StripeCustomerId, customer);
        }

        /// <summary>
        /// Deletes the customer.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DeleteCustomer(SaasEcomUser user)
        {
            _customerService.Delete(user.StripeCustomerId);
        }
    }
}
