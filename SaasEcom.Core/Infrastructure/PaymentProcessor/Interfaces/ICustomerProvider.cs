using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    public interface ICustomerProvider
    {
        Task<object> CreateCustomerAsync(SaasEcomUser user, string planId = null);
        object UpdateCustomer(SaasEcomUser user, CreditCard card);
    }
}
