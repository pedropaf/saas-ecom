using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaasEcom.Core.Models
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

        public int UserId { get; set; }
        public virtual SaasEcomUser User { get; set; }

        [Index]
        [MaxLength(50)]
        public string StripeId { get; set; }

        public string Status()
        {
            string result = null;

            if (IsTrialing())
            {
                result = string.Format("Trial until {0}", TrialEnd.Value.ToShortDateString());

            }
            else if (IsTerminated())
            {
                result = string.Format("Terminated since {0}", End.Value.ToShortDateString());
            }
            else
            {
                result = "Subscription Active";
            }
            return result;
        }

        private bool IsTerminated()
        {
            return this.End != null && this.End < DateTime.UtcNow;
        }

        public bool IsTrialing()
        {
            return TrialStart != null && TrialEnd != null && TrialEnd > DateTime.UtcNow;
        }
    }
}
