using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace SaasEcom.Data.PaymentProcessor.Stripe
{
    // TODO: Make methods Async
    // TODO: Abstract Interface and make more generic
    public class StripePaymentProcessorProvider
    {
        private readonly string _apiKey;

        private StripeCustomerService _customerService;
        private StripeCustomerService CustomerService()
        {
            return _customerService ?? (_customerService = new StripeCustomerService(_apiKey));
        }

        private StripePlanService _planService;
        private StripePlanService PlanService()
        {
            return _planService ?? (_planService = new StripePlanService(_apiKey));
        }

        private StripeInvoiceService _invoiceService;
        private StripeInvoiceService InvoiceService()
        {
            return _invoiceService ?? (_invoiceService = new StripeInvoiceService(_apiKey));
        }

        public StripePaymentProcessorProvider(string apiKey)
        {
            _apiKey = apiKey;
        }

        #region Subscription Plans

        public object CreateSubscriptionPlan()
        {
            throw new NotImplementedException();
        }

        public object UpdateSubscriptionPlan()
        {
            throw new NotImplementedException();
        }

        public object GetSubscriptionPlan()
        {
            throw new NotImplementedException();
        }

        public object GetAllPlans()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Customers

        public object CreateCustomer()
        {
            throw new NotImplementedException();
        }

        public object UpdateCustomer()
        {
            throw new NotImplementedException();
        }

        public object GetCustomer()
        {
            throw new NotImplementedException();
        }

        public object DeleteCustomer()
        {
            throw new NotImplementedException();
        }

        public object GetAllCustomers()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Subscriptions

        public object UpdateCustomerSubscription()
        {
            throw new NotImplementedException();
        }

        public object CancelCustomerSubscription()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Charges

        #endregion

        #region Cards

        #endregion

        #region Tokens

        #endregion

        #region Invoices

        #endregion

        #region Account

        #endregion
    }
}
