using System.ComponentModel.DataAnnotations;

namespace SaasEcom.Core.Models
{
    /// <summary>
    /// Subscription plan property. Eg: 3 Gb Max storage, 1000 projects, etc.
    /// </summary>
    public class SubscriptionPlanProperty
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets the subscription plan that the property belongs to.
        /// </summary>
        /// <value>
        /// The subscription plan.
        /// </value>
        [Required]
        public virtual SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
