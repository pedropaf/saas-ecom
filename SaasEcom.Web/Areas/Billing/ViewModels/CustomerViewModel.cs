using System.Collections.Generic;
using SaasEcom.Data.Models;

namespace SaasEcom.Web.Areas.Billing.ViewModels
{
    public sealed class CustomerViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string StripeCustomerId { get; set; }

        public string SubscriptionPlan { get; set; }
        public string SubscriptionPlanPrice { get; set; }
        public string SubscriptionPlanCurrency { get; set; }

        public IList<Invoice> Invoices { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}