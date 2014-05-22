using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaasEcom.Data.Infrastructure.Helpers;

namespace SaasEcom.Data.Models
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }

        [Display(Name = "Plan Identifier")]
        [Required(ErrorMessage = "Please set a plan identifier.")]
        public string FriendlyId { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        [Range(0.0, 10000)]
        public double Price { get; set; }
        
        public string Currency { get; set; }

        [NotMapped]
        public Currency CurrencyDetails {
            get { return CurrencyHelper.GetCurrencyInfo(Currency); } 
        }

        [Required]
        public SubscriptionInterval Interval { get; set; }

        [Display(Name = "Trial period in days")]
        [Range(0, 365)]
        public int TrialPeriodInDays { get; set; }

        public bool Disabled { get; set; }

        public virtual StripeAccount StripeAccount { get; set; }

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

