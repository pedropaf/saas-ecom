using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    /// <summary>
    /// Interface for CRUD related to customers with Stripe
    /// </summary>
    public interface ICustomerProvider
    {
        /// <summary>
        /// Creates the customer asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <returns></returns>
        Task<object> CreateCustomerAsync(SaasEcomUser user, string planId = null);

        /// <summary>
        /// Updates the customer.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        object UpdateCustomer(SaasEcomUser user, CreditCard card);

        /// <summary>
        /// Deletes the customer.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        object DeleteCustomer(SaasEcomUser user);
    }
}
