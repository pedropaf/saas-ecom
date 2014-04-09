using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaasEcom.Data.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime? TrialStart { get; set; }
        public DateTime? TrialEnd { get; set; }

        public virtual SubscriptionPlan SubscriptionPlan { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
