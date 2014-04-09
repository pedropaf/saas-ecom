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

        [DisplayFormat(DataFormatString = "{MMMM 0:dd, yyyy}")]
        public DateTime? Start { get; set; }

        [DisplayFormat(DataFormatString = "{MMMM 0:dd, yyyy}")]
        public DateTime? End { get; set; }

        [DisplayFormat(DataFormatString = "{MMMM 0:dd, yyyy}")]
        public DateTime? TrialStart { get; set; }

        [DisplayFormat(DataFormatString = "{MMMM 0:dd, yyyy}")]
        public DateTime? TrialEnd { get; set; }

        public virtual SubscriptionPlan SubscriptionPlan { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
