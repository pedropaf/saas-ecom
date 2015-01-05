using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaasEcom.Core.Infrastructure.Helpers;

namespace SaasEcom.Core.Models
{
    /// <summary>
    /// Subscription Plan
    /// </summary>
    public class SubscriptionPlan
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Display(Name = "Plan Identifier")]
        [Required(ErrorMessage = "Please set a plan identifier.")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [Required]
        [Range(0.0, 1000000)]
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets the currency details.
        /// </summary>
        /// <value>
        /// The currency details.
        /// </value>
        [NotMapped]
        public Currency CurrencyDetails {
            get { return CurrencyHelper.GetCurrencyInfo(Currency); } 
        }

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        [Required]
        public SubscriptionInterval Interval { get; set; }

        /// <summary>
        /// Gets or sets the trial period in days.
        /// </summary>
        /// <value>
        /// The trial period in days.
        /// </value>
        [Display(Name = "Trial period in days")]
        [Range(0, 365)]
        public int TrialPeriodInDays { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SubscriptionPlan"/> is disabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disabled; otherwise, <c>false</c>.
        /// </value>
        public bool Disabled { get; set; }

        /// <summary>
        /// Subscription Interval
        /// </summary>
        public enum SubscriptionInterval
        {
            /// <summary>
            /// Monthly
            /// </summary>
            Monthly,

            /// <summary>
            /// Yearly
            /// </summary>
            Yearly,

            /// <summary>
            /// Weekly
            /// </summary>
            Weekly,

            /// <summary>
            /// Every 6 months
            /// </summary>
            [Display(Name="Every 6 months")]
            EverySixMonths,

            /// <summary>
            /// Every 3 months
            /// </summary>
            [Display(Name="Every 3 months")]
            EveryThreeMonths
        }
    }
}

