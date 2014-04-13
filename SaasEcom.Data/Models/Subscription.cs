using System;

namespace SaasEcom.Data.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime? TrialStart { get; set; }
        public DateTime? TrialEnd { get; set; }

        public int SubscriptionPlanId { get; set; }
        public virtual SubscriptionPlan SubscriptionPlan { get; set; }

        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string Status()
        {
            return IsTrialing() ? 
                string.Format("Trial until {0}", ((DateTime)TrialEnd).ToShortDateString()) :
                string.Format("Next invoice {0}", DateTime.UtcNow.ToShortDateString());
        }

        public bool IsTrialing()
        {
            return TrialStart != null && TrialEnd != null && TrialEnd > DateTime.UtcNow;
        }
    }
}
