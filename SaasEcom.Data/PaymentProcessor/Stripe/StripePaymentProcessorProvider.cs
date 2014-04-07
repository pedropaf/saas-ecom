using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using SaasEcom.Data.Models;
using Stripe;

namespace SaasEcom.Data.PaymentProcessor.Stripe
{
    // TODO: Make methods Async
    // TODO: Abstract Interface and make more generic
    public class StripePaymentProcessorProvider
    {
        private readonly string _apiKey;

        private StripeCustomerService _customerService;
        private StripeCustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new StripeCustomerService(_apiKey)); }
        }

        private StripePlanService _planService;
        private StripePlanService PlanService
        {
            get { return _planService ?? (_planService = new StripePlanService(_apiKey)); }
        }

        private StripeInvoiceService _invoiceService;
        private StripeInvoiceService InvoiceService
        {
            get { return _invoiceService ?? (_invoiceService = new StripeInvoiceService(_apiKey)); }
        }

        public StripePaymentProcessorProvider(string apiKey)
        {
            _apiKey = apiKey;
        }

        #region Subscription Plans

        public StripePlan CreateSubscriptionPlan(SubscriptionPlan plan)
        {
            var result = PlanService.Create(new StripePlanCreateOptions
            {
                Id = plan.FriendlyId,
                Name = plan.Name,
                AmountInCents = (int) Math.Round(plan.Price * 100),
                Currency = "GBP",
                Interval = GetInterval(plan.Interval),
                TrialPeriodDays = plan.TrialPeriodInDays,
                IntervalCount = 1, // The number of intervals (specified in the interval property) between each subscription billing. For example, interval=month and interval_count=3 bills every 3 months.
            });

            return result;
        }
        
        public StripePlan UpdateSubscriptionPlan(SubscriptionPlan plan)
        {
            var res = PlanService.Update(plan.FriendlyId, new StripePlanUpdateOptions
            {
                Name = plan.Name
            });

            return res;
        }

        public StripePlan GetSubscriptionPlan(string planId)
        {
            try
            {
                return PlanService.Get(planId);
            }
            catch (StripeException ex)
            {
                return null;
            }
        }

        public IEnumerable<StripePlan> GetAllPlans(int count = 100, int offset = 0)
        {
            return PlanService.List(count, offset);
        }

        public void DeleteSubscriptionPlan(string planId)
        {
            PlanService.Delete(planId);
        }

        #endregion

        #region Customers

        public StripeCustomer CreateCustomer(ApplicationUser user, string planId = null)
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

            return CustomerService.Create(customer);
        }

        public StripeCustomer UpdateCustomer(ApplicationUser user, CreditCard card)
        {
            var customer = new StripeCustomerUpdateOptions
            {
                Email = user.Email,
                
                // Card Details
                CardAddressCity = card.CardAddressCity,
                CardAddressCountry = card.CardAddressCountry,
                CardAddressLine1 = card.CardAddressLine1,
                CardAddressLine2 = card.CardAddressLine2,
                CardAddressState = card.CardAddressState,
                CardAddressZip = card.CardAddressZip,
                CardCvc = card.CardCvc,
                CardExpirationMonth = card.CardExpirationMonth,
                CardExpirationYear = card.CardExpirationYear,
                CardName = card.CardName,
                CardNumber = card.CardNumber
            };

            return CustomerService.Update(user.StripeCustomerId, customer);
        }

        public StripeCustomer GetCustomer(string customerId)
        {
            return CustomerService.Get(customerId);
        }

        public void DeleteCustomer(string customerId)
        {
            CustomerService.Delete(customerId);
        }

        public IEnumerable<StripeCustomer> GetAllCustomers(int count = 100, int offset = 0)
        {
            return CustomerService.List(count, offset);
        }

        #endregion

        #region Subscriptions

        public StripeSubscription UpdateCustomerSubscription(string customerId, CreditCard creditCard, string planId)
        {
            var myUpdatedSubscription = new StripeCustomerUpdateSubscriptionOptions
            {
                CardNumber = creditCard.CardNumber,
                CardExpirationYear = creditCard.CardExpirationYear,
                CardExpirationMonth = creditCard.CardExpirationMonth,
                CardAddressCountry = creditCard.CardAddressCountry,
                CardAddressLine1 = creditCard.CardAddressLine1,
                CardAddressLine2 = creditCard.CardAddressLine2,
                CardAddressState = creditCard.CardAddressState,
                CardAddressZip = creditCard.CardAddressZip,
                CardName = creditCard.CardName,
                CardCvc = creditCard.CardCvc,
                PlanId = planId
            };

            return CustomerService.UpdateSubscription(customerId, myUpdatedSubscription);
        }

        // TODO: This cancels all subscriptions for the customer. Add support for individual subscriptions
        public StripeSubscription CancelCustomerSubscription(string customerId, bool cancelAtPeriodEnd = false)
        {
            return CustomerService.CancelSubscription(customerId, cancelAtPeriodEnd);
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

        #region Helpers
        
        private static string GetInterval(SubscriptionPlan.SubscriptionInterval interval)
        {
            string result = null;

            switch (interval)
            {
                case (SubscriptionPlan.SubscriptionInterval.Monthly):
                    result = "month";
                    break;
                case (SubscriptionPlan.SubscriptionInterval.Yearly):
                    result = "year";
                    break;
                case (SubscriptionPlan.SubscriptionInterval.Weekly):
                    result = "week";
                    break;
                case (SubscriptionPlan.SubscriptionInterval.EveryThreeMonths):
                    result = "3-month";
                    break;
                case (SubscriptionPlan.SubscriptionInterval.EverySixMonths):
                    result = "6-month";
                    break;
            }

            return result;
        }

        #endregion
    }
}
