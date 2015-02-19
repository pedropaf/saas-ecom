using System.Collections.Generic;
using System.Linq;
using SaasEcom.Core.Models;
using Stripe;

namespace SaasEcom.Core.Infrastructure
{
    /// <summary>
    /// Mapper for Stripe classes to SaasEcom classes
    /// </summary>
    public class Mappers
    {
        #region Stripe to SaasEcom Mapper
        public static Invoice Map(StripeInvoice stripeInvoice)
        {
            var invoice = new Invoice
            {
                AmountDue = stripeInvoice.AmountDue,
                ApplicationFee = stripeInvoice.ApplicationFee,
                AttemptCount = stripeInvoice.AttemptCount,
                Attempted = stripeInvoice.Attempted,
                Closed = stripeInvoice.Closed,
                Currency = stripeInvoice.Currency,
                Date = stripeInvoice.Date,
                Description = stripeInvoice.Description,
                // Discount = Map(stripeInvoice.StripeDiscount),
                EndingBalance = stripeInvoice.EndingBalance,
                Forgiven = stripeInvoice.Forgiven,
                NextPaymentAttempt = stripeInvoice.NextPaymentAttempt,
                Paid = stripeInvoice.Paid,
                PeriodStart = stripeInvoice.PeriodStart,
                PeriodEnd = stripeInvoice.PeriodEnd,
                ReceiptNumber = stripeInvoice.ReceiptNumber,
                StartingBalance = stripeInvoice.StartingBalance,
                StripeCustomerId = stripeInvoice.CustomerId,
                StatementDescriptor = stripeInvoice.StatementDescriptor,
                Tax = stripeInvoice.Tax,
                TaxPercent = stripeInvoice.TaxPercent,
                StripeId = stripeInvoice.Id,
                Subtotal = stripeInvoice.Subtotal,
                Total = stripeInvoice.Total,
                LineItems = Map(stripeInvoice.StripeInvoiceLineItems.Data)
            };

            return invoice;
        }

        private static ICollection<Invoice.LineItem> Map(IEnumerable<StripeInvoiceLineItem> list)
        {
            if (list == null)
                return null;

            return list.Select(i => new Invoice.LineItem
            {
                Amount = i.Amount,
                Currency = i.Currency,
                Period = Map(i.StripePeriod),
                Plan = Map(i.Plan),
                Proration = i.Proration,
                Quantity = i.Quantity,
                StripeLineItemId = i.Id,
                Type = i.Type
            }).ToList();
        }

        private static Invoice.Plan Map(StripePlan stripePlan)
        {
            return new Invoice.Plan
            {
                AmountInCents = stripePlan.Amount,
                Created = stripePlan.Created,
                Currency = stripePlan.Currency,
                StatementDescriptor = stripePlan.StatementDescriptor,
                Interval = stripePlan.Interval,
                IntervalCount = stripePlan.IntervalCount,
                Name = stripePlan.Name,
                StripePlanId = stripePlan.Id,
                TrialPeriodDays = stripePlan.TrialPeriodDays
            };
        }

        private static Invoice.Period Map(StripePeriod stripePeriod)
        {
            if (stripePeriod == null)
                return null;

            return new Invoice.Period
            {
                Start = stripePeriod.Start,
                End = stripePeriod.End
            };
        }
        #endregion
    }
}
