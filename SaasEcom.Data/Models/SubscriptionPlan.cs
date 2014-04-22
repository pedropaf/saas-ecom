using System.ComponentModel.DataAnnotations;

namespace SaasEcom.Data.Models
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }
        public string FriendlyId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public SubscriptionInterval Interval { get; set; }
        public int TrialPeriodInDays { get; set; }
        public string StatementDescription { get; set; }

        public enum SubscriptionInterval
        {
            Monthly,
            Yearly,
            Weekly,

            [Display(Name="Every 6 months")]
            EverySixMonths,

            [Display(Name="Every 3 months")]
            EveryThreeMonths
        }
    }
}

