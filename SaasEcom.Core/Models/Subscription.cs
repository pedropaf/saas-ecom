using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaasEcom.Core.Models
{
    /// <summary>
    /// Subscription
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public DateTime? Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public DateTime? End { get; set; }

        /// <summary>
        /// Gets or sets the trial start.
        /// </summary>
        /// <value>
        /// The trial start.
        /// </value>
        public DateTime? TrialStart { get; set; }

        /// <summary>
        /// Gets or sets the trial end.
        /// </summary>
        /// <value>
        /// The trial end.
        /// </value>
        public DateTime? TrialEnd { get; set; }

        /// <summary>
        /// Gets or sets the subscription plan identifier.
        /// </summary>
        /// <value>
        /// The subscription plan identifier.
        /// </value>
        public string SubscriptionPlanId { get; set; }

        /// <summary>
        /// Gets or sets the subscription plan.
        /// </summary>
        /// <value>
        /// The subscription plan.
        /// </value>
        public virtual SubscriptionPlan SubscriptionPlan { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        [ForeignKey("UserId")]
        public virtual SaasEcomUser User { get; set; }

        /// <summary>
        /// Gets or sets the stripe identifier.
        /// </summary>
        /// <value>
        /// The stripe identifier.
        /// </value>
        [Index]
        [MaxLength(50)]
        public string StripeId { get; set; }

        /// <summary>
        /// Subscription status.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Determines whether this instance is trialing.
        /// </summary>
        /// <returns></returns>
        public bool IsTrialing()
        {
            return TrialStart != null && TrialEnd != null && TrialEnd > DateTime.UtcNow;
        }
        private bool IsTerminated()
        {
            return this.End != null && this.End < DateTime.UtcNow;
        }
    }
}
