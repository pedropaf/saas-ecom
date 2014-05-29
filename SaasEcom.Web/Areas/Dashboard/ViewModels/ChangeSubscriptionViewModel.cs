using System.Collections.Generic;
using SaasEcom.Data.Models;

namespace SaasEcom.Web.Areas.Dashboard.ViewModels
{
    public class ChangeSubscriptionViewModel
    {
        public List<SubscriptionPlan> SubscriptionPlans { get; set; }

        public string CurrentSubscription { get; set; }
        public string NewPlan { get; set; }
    }
}