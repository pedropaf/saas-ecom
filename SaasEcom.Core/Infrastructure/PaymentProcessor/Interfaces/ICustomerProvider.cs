using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    public interface ICustomerProvider
    {
        Task<object> CreateCustomerAsync(ApplicationUser user, string planId = null);
        object UpdateCustomer(ApplicationUser user, CreditCard card);
    }
}
