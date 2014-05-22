using SaasEcom.Data.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SaasEcom.Data.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string StripeId { get; set; }

        [MaxLength(50)]
        [Index]
        public string StripeCustomerId { get; set; }

        public virtual ApplicationUser Customer { get; set; }

        public virtual StripeAccount StripeAccount { get; set; }

        public DateTime? Date { get; set; }
        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }
        
        public virtual ICollection<LineItem> LineItems { get; set; }
        public int? Subtotal { get; set; }
        public int? Total { get; set; }
        public bool? Attempted { get; set; }
        public bool? Closed { get; set; }
        [Index]
        public bool? Paid { get; set; }
        public int? AttemptCount { get; set; }
        public int? AmountDue { get; set; }
        public int? StartingBalance { get; set; }
        public int? EndingBalance { get; set; }
        public DateTime? NextPaymentAttempt { get; set; }
        public int? Charge { get; set; }
        public int? Discount { get; set; }
        public int? ApplicationFee { get; set; }
        public string Currency { get; set; }
        
        [NotMapped]
        public Currency CurrencyDetails
        {
            get { return CurrencyHelper.GetCurrencyInfo(Currency); }
        }

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

        public class LineItem
        {
            public int Id { get; set; }
            public string StripeLineItemId { get; set; }
            public string Type { get; set; }
            public int? Amount { get; set; }
            public string Currency { get; set; }
            public bool Proration { get; set; }
            public Period Period { get; set; }
            public int? Quantity { get; set; }
            public Plan Plan { get; set; }
        }

        public class Period
        {
            public DateTime? Start { get; set; }
            public DateTime? End { get; set; }
        }

        public class Plan
        {
            public string StripePlanId { get; set; }
            public string Interval { get; set; }
            public string Name { get; set; }
            public DateTime? Created { get; set; }
            public int? AmountInCents { get; set; }
            public string Currency { get; set; }
            public int IntervalCount { get; set; }
            public int? TrialPeriodDays { get; set; }
            public string StatementDescription { get; set; }
        }
    }
}
