using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SaasEcom.Core.Infrastructure.Helpers;

namespace SaasEcom.Core.Models
{
    /// <summary>
    /// Subscription Plan
    /// </summary>
    public class SubscriptionPlan
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionPlan"/> class.
        /// </summary>
        public SubscriptionPlan()
        {
            this.Properties = new List<SubscriptionPlanProperty>();
        }

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
        /// Collection of properties related to this plan (Maximum users, storage, etc)
        /// </summary>
        public virtual ICollection<SubscriptionPlanProperty> Properties { get; set; }

        /// <summary>
        /// Gets or sets the reason to cancel.
        /// </summary>
        /// <value>
        /// The reason to cancel.
        /// </value>
        public string ReasonToCancel { get; set; }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetProperty(string key)
        {
            return this.Properties.Where(i => key != null && i.Key == key).Select(i => i.Value).FirstOrDefault();
        }

        /// <summary>
        /// Gets the property int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Property for key:  + key + does not exist.</exception>
        public int GetPropertyInt(string key)
        {
            var property = this.Properties.Where(i => key != null && i.Key == key).Select(i => i.Value).FirstOrDefault();

            if (property != null)
            {
                return int.Parse(property);
            }
             
            throw new Exception("Property for key: " + key + "does not exist.");
        }

        /// <summary>
        /// Gets the property long.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Property for key:  + key + does not exist.</exception>
        public long GetPropertyLong(string key)
        {
            var property = this.Properties.Where(i => key != null && i.Key == key).Select(i => i.Value).FirstOrDefault();

            if (property != null)
            {
                return long.Parse(property);
            }

            throw new Exception("Property for key: " + key + "does not exist.");
        }

        /// <summary>
        /// Subscription Interval
        /// </summary>
        public enum SubscriptionInterval
        {
            /// <summary>
            /// Monthly
            /// </summary>
            [Display(ResourceType = typeof (Resources.SaasEcom), Name = "SubscriptionInterval_Monthly_Monthly")]
            Monthly = 1,

            /// <summary>
            /// Yearly
            /// </summary>
            [Display(ResourceType = typeof (Resources.SaasEcom), Name = "SubscriptionInterval_Yearly_Yearly")]
            Yearly = 2,

            /// <summary>
            /// Weekly
            /// </summary>
            [Display(ResourceType = typeof (Resources.SaasEcom), Name = "SubscriptionInterval_Weekly_Weekly")]
            Weekly = 3,

            /// <summary>
            /// Every 6 months
            /// </summary>
            [Display(ResourceType = typeof (Resources.SaasEcom), Name = "SubscriptionInterval_EverySixMonths_Every_6_months")]
            EverySixMonths = 4,

            /// <summary>
            /// Every 3 months
            /// </summary>
            [Display(ResourceType = typeof (Resources.SaasEcom), Name = "SubscriptionInterval_EveryThreeMonths_Every_3_months")]
            EveryThreeMonths = 5
        }
    }
}