namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    /// <summary>
    /// Interface for CRUD related to charges with Stripe
    /// </summary>
    public interface IChargeProvider
    {
        /// <summary>
        /// Creates the charge.
        /// </summary>
        /// <param name="amount">The amount: A positive integer in the smallest currency unit (e.g 100 cents to charge $1.00, or 1 to charge ¥1, a 0-decimal currency) representing how much to charge the card. The minimum amount is $0.50 (or equivalent in charge currency).</param>
        /// <param name="currency">The currency: Three-letter ISO currency code representing the currency in which the charge was made.</param>
        /// <param name="description">The description: An arbitrary string which you can attach to a charge object. It is displayed when in the web interface alongside the charge. Note that if you use Stripe to send automatic email receipts to your customers, your receipt emails will include the description of the charge(s) that they are describing.</param>
        /// <param name="customerId">The customer identifier: The ID of an existing customer that will be charged in this request.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        bool CreateCharge(int amount, string currency, string description, string customerId, out string error);

        // void GetCharge();
        // void UpdateCharge();
        // void ListCharges();
    }
}
