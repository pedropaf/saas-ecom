using System.ComponentModel.DataAnnotations;

namespace SaasEcom.Core.Models
{
    public class SubscriptionPlanProperty
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        [Required]
        public virtual SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
