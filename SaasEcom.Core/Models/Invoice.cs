using SaasEcom.Core.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SaasEcom.Core.Models
{
    /// <summary>
    /// Invoice
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the stripe identifier.
        /// </summary>
        /// <value>
        /// The stripe identifier.
        /// </value>
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string StripeId { get; set; }

        /// <summary>
        /// Gets or sets the stripe customer identifier.
        /// </summary>
        /// <value>
        /// The stripe customer identifier.
        /// </value>
        [MaxLength(50)]
        [Index]
        public string StripeCustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        public virtual SaasEcomUser Customer { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime? Date { get; set; }
        
        /// <summary>
        /// Gets or sets the period start.
        /// </summary>
        /// <value>
        /// The period start.
        /// </value>
        public DateTime? PeriodStart { get; set; }
        
        /// <summary>
        /// Gets or sets the period end.
        /// </summary>
        /// <value>
        /// The period end.
        /// </value>
        public DateTime? PeriodEnd { get; set; }

        /// <summary>
        /// Gets or sets the line items.
        /// </summary>
        /// <value>
        /// The line items.
        /// </value>
        public virtual ICollection<LineItem> LineItems { get; set; }

        /// <summary>
        /// Gets or sets the subtotal.
        /// </summary>
        /// <value>
        /// The subtotal.
        /// </value>
        public int? Subtotal { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public int? Total { get; set; }

        /// <summary>
        /// Gets or sets the attempted.
        /// </summary>
        /// <value>
        /// The attempted.
        /// </value>
        public bool? Attempted { get; set; }

        /// <summary>
        /// Gets or sets the closed.
        /// </summary>
        /// <value>
        /// The closed.
        /// </value>
        public bool? Closed { get; set; }

        /// <summary>
        /// Gets or sets the paid.
        /// </summary>
        /// <value>
        /// The paid.
        /// </value>
        [Index]
        public bool? Paid { get; set; }

        /// <summary>
        /// Gets or sets the attempt count.
        /// </summary>
        /// <value>
        /// The attempt count.
        /// </value>
        public int? AttemptCount { get; set; }

        /// <summary>
        /// Gets or sets the amount due.
        /// </summary>
        /// <value>
        /// The amount due.
        /// </value>
        public int? AmountDue { get; set; }

        /// <summary>
        /// Gets or sets the starting balance.
        /// </summary>
        /// <value>
        /// The starting balance.
        /// </value>
        public int? StartingBalance { get; set; }

        /// <summary>
        /// Gets or sets the ending balance.
        /// </summary>
        /// <value>
        /// The ending balance.
        /// </value>
        public int? EndingBalance { get; set; }

        /// <summary>
        /// Gets or sets the next payment attempt.
        /// </summary>
        /// <value>
        /// The next payment attempt.
        /// </value>
        public DateTime? NextPaymentAttempt { get; set; }

        /// <summary>
        /// Gets or sets the charge.
        /// </summary>
        /// <value>
        /// The charge.
        /// </value>
        public int? Charge { get; set; }

        /// <summary>
        /// Gets or sets the discount.
        /// </summary>
        /// <value>
        /// The discount.
        /// </value>
        public int? Discount { get; set; }

        /// <summary>
        /// Gets or sets the application fee.
        /// </summary>
        /// <value>
        /// The application fee.
        /// </value>
        public int? ApplicationFee { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; set; }

        public virtual BillingAddress BillingAddress { get; set; }

        /// <summary>
        /// Gets the currency details.
        /// </summary>
        /// <value>
        /// The currency details.
        /// </value>
        [NotMapped]
        public Currency CurrencyDetails
        {
            get { return CurrencyHelper.GetCurrencyInfo(Currency); }
        }

        /// <summary>
        /// Gets the invoice period.
        /// </summary>
        /// <value>
        /// The invoice period.
        /// </value>
        [NotMapped]
        public string InvoicePeriod
        {
            get
            {
                var start = this.PeriodStart;
                var end = this.PeriodEnd;

                if (LineItems != null && LineItems.Any())
                {
                    var p = LineItems.First();
                    start = p.Period.Start;
                    end = p.Period.End;
                }

                return string.Format("{0} - {1}", start.Value.ToString("d MMM yyyy"), end.Value.ToString("d MMM yyyy"));
            }
        }

        /// <summary>
        /// Invoice Line Item
        /// </summary>
        public class LineItem
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the stripe line item identifier.
            /// </summary>
            /// <value>
            /// The stripe line item identifier.
            /// </value>
            public string StripeLineItemId { get; set; }

            /// <summary>
            /// Gets or sets the type.
            /// </summary>
            /// <value>
            /// The type.
            /// </value>
            public string Type { get; set; }

            /// <summary>
            /// Gets or sets the amount.
            /// </summary>
            /// <value>
            /// The amount.
            /// </value>
            public int? Amount { get; set; }

            /// <summary>
            /// Gets or sets the currency.
            /// </summary>
            /// <value>
            /// The currency.
            /// </value>
            public string Currency { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="LineItem"/> is proration.
            /// </summary>
            /// <value>
            ///   <c>true</c> if proration; otherwise, <c>false</c>.
            /// </value>
            public bool Proration { get; set; }

            /// <summary>
            /// Gets or sets the period.
            /// </summary>
            /// <value>
            /// The period.
            /// </value>
            public Period Period { get; set; }

            /// <summary>
            /// Gets or sets the quantity.
            /// </summary>
            /// <value>
            /// The quantity.
            /// </value>
            public int? Quantity { get; set; }

            /// <summary>
            /// Gets or sets the plan.
            /// </summary>
            /// <value>
            /// The plan.
            /// </value>
            public Plan Plan { get; set; }
        }

        /// <summary>
        /// Invoice Period
        /// </summary>
        public class Period
        {
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
        }

        /// <summary>
        /// Invoice Plan
        /// </summary>
        public class Plan
        {
            /// <summary>
            /// Gets or sets the stripe plan identifier.
            /// </summary>
            /// <value>
            /// The stripe plan identifier.
            /// </value>
            public string StripePlanId { get; set; }

            /// <summary>
            /// Gets or sets the interval.
            /// </summary>
            /// <value>
            /// The interval.
            /// </value>
            public string Interval { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the created.
            /// </summary>
            /// <value>
            /// The created.
            /// </value>
            public DateTime? Created { get; set; }

            /// <summary>
            /// Gets or sets the amount in cents.
            /// </summary>
            /// <value>
            /// The amount in cents.
            /// </value>
            public int? AmountInCents { get; set; }

            /// <summary>
            /// Gets or sets the currency.
            /// </summary>
            /// <value>
            /// The currency.
            /// </value>
            public string Currency { get; set; }

            /// <summary>
            /// Gets or sets the interval count.
            /// </summary>
            /// <value>
            /// The interval count.
            /// </value>
            public int IntervalCount { get; set; }

            /// <summary>
            /// Gets or sets the trial period days.
            /// </summary>
            /// <value>
            /// The trial period days.
            /// </value>
            public int? TrialPeriodDays { get; set; }

            /// <summary>
            /// Gets or sets the statement description.
            /// </summary>
            /// <value>
            /// The statement description.
            /// </value>
            public string StatementDescription { get; set; }
        }
    }
}
