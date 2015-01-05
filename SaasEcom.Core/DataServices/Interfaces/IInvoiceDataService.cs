using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Interfaces
{
    /// <summary>
    /// Interface for CRUD related to invoices in the database.
    /// </summary>
    public interface IInvoiceDataService
    {
        /// <summary>
        /// Gets the User's invoices asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>List of invoices.</returns>
        Task<List<Invoice>> UserInvoicesAsync(string userId);

        /// <summary>
        /// Gets the invoice given a users identifier and the invoice identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>The invoice</returns>
        Task<Invoice> UserInvoiceAsync(string userId, int invoiceId);

        /// <summary>
        /// Creates the or update asynchronous.
        /// </summary>
        /// <param name="invoice">The invoice.</param>
        /// <returns>int</returns>
        Task<int> CreateOrUpdateAsync(Invoice invoice);

        /// <summary>
        /// Gets all the invoices asynchronous.
        /// </summary>
        /// <returns>List of invoices.</returns>
        Task<List<Invoice>> GetInvoicesAsync();
    }
}
