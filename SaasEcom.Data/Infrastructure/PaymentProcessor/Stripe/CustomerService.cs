using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.Models;
using Stripe;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe
{
    public class CustomerService
    {
        private readonly StripeCustomerService _customerService;

        public CustomerService(string apiKey)
        {
            _customerService = new StripeCustomerService(apiKey);
        }

        public async Task<StripeCustomer> CreateCustomerAsync(ApplicationUser user, string planId = null)
        {
            var customer = new StripeCustomerCreateOptions
            {
                AccountBalance = 0,
                // Card
                Email = user.Email,
            };

            if (!string.IsNullOrEmpty(planId))
            {
                customer.PlanId = planId;
            }

            return await Task.Run(() => _customerService.Create(customer));
        }

        public StripeCustomer UpdateCustomer(ApplicationUser user, CreditCard card)
        {
            var customer = new StripeCustomerUpdateOptions
            {
                Email = user.Email,

                // Card Details
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
                CardNumber = card.CardNumber
            };

            return _customerService.Update(user.StripeCustomerId, customer);
        }

        public StripeCustomer GetCustomer(string customerId)
        {
            return _customerService.Get(customerId);
        }

        public void DeleteCustomer(string customerId)
        {
            _customerService.Delete(customerId);
        }

        public IEnumerable<StripeCustomer> GetAllCustomers(int limit = 100)
        {
            return _customerService.List(new StripeCustomerListOptions() { Limit = limit });
        }
    }
}
