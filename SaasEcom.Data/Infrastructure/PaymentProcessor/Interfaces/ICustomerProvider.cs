using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Interfaces
{
    public interface ICustomerProvider
    {
        Task<object> CreateCustomerAsync(ApplicationUser user, string planId = null);
        object UpdateCustomer(ApplicationUser user, CreditCard card);
    }
}
