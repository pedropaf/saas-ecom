using System.Collections.Generic;
using System.Linq;
using SaasEcom.Data.Models;
using Stripe;

namespace SaasEcom.Web.Mappers
{
    // TODO: Move to automapper
    public static class InvoiceMappers
    {
        public static Invoice MapToInvoice(StripeInvoice stripeInvoice)
        {
            return new Invoice
            {
                StripeId = stripeInvoice.Id,
                StripeCustomerId = stripeInvoice.CustomerId,
                Subtotal = stripeInvoice.Subtotal,
                Total = stripeInvoice.Total,
                Date = stripeInvoice.Date,
                PeriodStart = stripeInvoice.PeriodStart,
                PeriodEnd = stripeInvoice.PeriodEnd,
                StartingBalance = stripeInvoice.StartingBalance,
                EndingBalance = stripeInvoice.EndingBalance,
                Currency = stripeInvoice.Currency,
                LineItems = MapLineItems(stripeInvoice.StripeInvoiceLines),
                AmountDue = stripeInvoice.AmountDue,
                AttemptCount = stripeInvoice.AttemptCount,
                Attempted = stripeInvoice.Attempted,
                Closed = stripeInvoice.Closed,
                Paid = stripeInvoice.Paid,
                NextPaymentAttempt = stripeInvoice.NextPaymentAttempt
                // Discount
            };
        }

        private static ICollection<Invoice.LineItem> MapLineItems(StripeInvoiceLines items)
        {
            return items.StripeInvoiceItems.Select(item => new Invoice.LineItem
            {
                Amount = item.Amount,
                Period = new Invoice.Period
                {
                    Start = item.Period.Start,
                    End = item.Period.End
                },
                Plan = new Invoice.Plan
                {
                    AmountInCents = item.Plan.Amount,
                    Currency = item.Plan.Currency,
                    Interval = item.Plan.Interval,
                    IntervalCount = item.Plan.IntervalCount,
                    Name = item.Plan.Name,
                    StripePlanId = item.Plan.Id,
                    TrialPeriodDays = item.Plan.TrialPeriodDays,
                    Created = null
                },
                Currency = item.Currency,
                Proration = item.Proration,
                Quantity = item.Quantity,
                StripeLineItemId = item.Id
            }).ToList();
        }
    }
}
