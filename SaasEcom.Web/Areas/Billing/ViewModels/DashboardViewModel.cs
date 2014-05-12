using System.Collections.Generic;
using SaasEcom.Data.Models;

namespace SaasEcom.Web.Areas.Billing.ViewModels
{
    public class DashboardViewModel
    {
        public List<SubscriptionPlan> SubscriptionPlans { get; set; }
    }
}