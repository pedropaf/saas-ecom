using System.Collections.Generic;
using SaasEcom.Data.Models;

namespace SaasEcom.Web.Areas.Billing.ViewModels
{
    public class DashboardViewModel
    {
        public bool IsStripeSetup { get; set; }

        public List<SubscriptionPlan> SubscriptionPlans { get; set; }
    }
}