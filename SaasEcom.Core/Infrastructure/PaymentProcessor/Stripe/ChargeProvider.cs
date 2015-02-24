using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;
using Stripe;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe
{
    /// <summary>
    /// Implementation for CRUD related to charges with Stripe
    /// </summary>
    public class ChargeProvider : IChargeProvider
    {
        // Stripe Dependencies
        private readonly StripeChargeService _chargeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChargeProvider"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public ChargeProvider(string apiKey)
        {
            _chargeService = new StripeChargeService(apiKey);
        }

        /// <summary>
        /// Creates the charge.
        /// </summary>
        /// <param name="amount">The amount: A positive integer in the smallest currency unit (e.g 100 cents to charge $1.00, or 1 to charge ¥1, a 0-decimal currency) representing how much to charge the card. The minimum amount is $0.50 (or equivalent in charge currency).</param>
        /// <param name="currency">The currency: Three-letter ISO currency code representing the currency in which the charge was made.</param>
        /// <param name="description">The description: An arbitrary string which you can attach to a charge object. It is displayed when in the web interface alongside the charge. Note that if you use Stripe to send automatic email receipts to your customers, your receipt emails will include the description of the charge(s) that they are describing.</param>
        /// <param name="customerId">The customer identifier: The ID of an existing customer that will be charged in this request.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool CreateCharge(int amount, string currency, string description, string customerId, out string error)
        {
            var options = new StripeChargeCreateOptions
            {
                Amount = amount,
                Currency = currency,
                Description = description
            };

            if (!string.IsNullOrEmpty(customerId))
            {
                options.CustomerId = customerId;
            }

            var result = _chargeService.Create(options);

            if (result.Captured != null && result.Captured.Value)
            {
                error = null;
                return true;
            }
            else
            {
                error = result.FailureMessage;
                return false;
            }
        }
    }
}
