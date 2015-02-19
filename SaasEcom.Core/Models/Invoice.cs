using SaasEcom.Core.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SaasEcom.Core.Models
{
    /// <summary>
    /// Invoices are statements of what a customer owes for a particular billing period, including subscriptions, invoice items, and any automatic proration adjustments if necessary.
    /// Once an invoice is created, payment is automatically attempted. Note that the payment, while automatic, does not happen exactly at the time of invoice creation. If you have configured webhooks, the invoice will wait until one hour after the last webhook is successfully sent (or the last webhook times out after failing).
    /// Any customer credit on the account is applied before determining how much is due for that invoice (the amount that will be actually charged). If the amount due for the invoice is less than 50 cents (the minimum for a charge), we add the amount to the customer's running account balance to be added to the next invoice. If this amount is negative, it will act as a credit to offset the next invoice. Note that the customer account balance does not include unpaid invoices; it only includes balances that need to be taken into account when calculating the amount due for the next invoice.
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
        /// Total of all subscriptions, invoice items, and prorations on the invoice before any discount is applied
        /// </summary>
        /// <value>
        /// The subtotal.
        /// </value>
        public int? Subtotal { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// Total after discount
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public int? Total { get; set; }

        /// <summary>
        /// Gets or sets the attempted.
        /// Whether or not an attempt has been made to pay the invoice. An invoice is not attempted until 1 hour after the invoice.created webhook, for example, so you might not want to display that invoice as unpaid to your users.
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
        /// Whether or not payment was successfully collected for this invoice. An invoice can be paid (most commonly) with a charge or with credit from the customer’s account balance.
        /// </summary>
        /// <value>
        /// The paid.
        /// </value>
        [Index]
        public bool? Paid { get; set; }

        /// <summary>
        /// Gets or sets the attempt count.
        /// Number of payment attempts made for this invoice, from the perspective of the payment retry schedule. Any payment attempt counts as the first attempt, and subsequently only automatic retries increment the attempt count. In other words, manual payment attempts after the first attempt do not affect the retry schedule.
        /// </summary>
        /// <value>
        /// The attempt count.
        /// </value>
        public int? AttemptCount { get; set; }

        /// <summary>
        /// Gets or sets the amount due.
        /// Final amount due at this time for this invoice. If the invoice’s total is smaller than the minimum charge amount, for example, or if there is account credit that can be applied to the invoice, the amount_due may be 0. If there is a positive starting_balance for the invoice (the customer owes money), the amount_due will also take that into account. The charge that gets generated for the invoice will be for the amount specified in amount_due.
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
        /// The time at which payment will next be attempted.
        /// </summary>
        /// <value>
        /// The next payment attempt.
        /// </value>
        public DateTime? NextPaymentAttempt { get; set; }

        /// <summary>
        /// Gets or sets the application fee.
        /// The fee in cents that will be applied to the invoice and transferred to the application owner’s Stripe account when the invoice is paid.
        /// </summary>
        /// <value>
        /// The application fee.
        /// </value>
        public int? ApplicationFee { get; set; }

        /// <summary>
        /// Gets or sets the tax.
        /// The amount of tax included in the total, calculated from tax_percent and the subtotal. If no tax_percent is defined, this value will be null.
        /// </summary>
        /// <value>
        /// The tax.
        /// </value>
        public int? Tax { get; set; }

        /// <summary>
        /// This percentage of the subtotal has been added to the total amount of the invoice, including invoice line items and discounts. This field is inherited from the subscription’s tax_percent field, but can be changed before the invoice is paid. This field defaults to null.
        /// </summary>
        /// <value>
        /// The tax percent.
        /// </value>
        public decimal? TaxPercent { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the billing address.
        /// </summary>
        /// <value>
        /// The billing address.
        /// </value>
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the statement description.
        /// </summary>
        /// <value>
        /// The statement description.
        /// </value>
        public string StatementDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        public string ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Plan"/> is forgiven.
        /// Whether or not the invoice has been forgiven. Forgiving an invoice instructs us to update the subscription status as if the invoice were succcessfully paid. Once an invoice has been forgiven, it cannot be unforgiven or reopened.
        /// </summary>
        /// <value>
        ///   <c>true</c> if forgiven; otherwise, <c>false</c>.
        /// </value>
        public bool Forgiven { get; set; }

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
            /// Whether or not the invoice item was created automatically as a proration adjustment when the customer switched plans
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
            /// If the invoice item is a proration, the quantity of the subscription that the proration was computed for.
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
            /// One of day, week, month or year. The frequency with which a subscription should be billed.
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
            /// The amount in cents to be charged on the interval specified
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
            /// The number of intervals (specified in the interval property) between each subscription billing. For example, interval=month and interval_count=3 bills every 3 months.
            /// </summary>
            /// <value>
            /// The interval count.
            /// </value>
            public int IntervalCount { get; set; }

            /// <summary>
            /// Gets or sets the trial period days.
            /// Number of trial period days granted when subscribing a customer to this plan. Null if the plan has no trial period.
            /// </summary>
            /// <value>
            /// The trial period days.
            /// </value>
            public int? TrialPeriodDays { get; set; }

            /// <summary>
            /// Gets or sets the statement descriptor.
            /// Extra information about a charge for the customer’s credit card statement.
            /// </summary>
            /// <value>
            /// The statement descriptor.
            /// </value>
            public string StatementDescriptor { get; set; }
        }
    }
}
