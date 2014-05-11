using Stripe;

namespace SaasEcom.Data.Infrastructure.Analytics.Stripe
{
    public class StripeEventLogger : IEventLogger
    {
        private readonly StripeIndexer _indexer;

        public StripeEventLogger()
        {
            this._indexer = new StripeIndexer();
        }

        public void LogEvent<T>(T logEvent)
        {
            if (typeof (T) != typeof (StripeEvent))
            {
                return;
            }

            var stripeEvent = logEvent as StripeEvent;

            // All the event types explained here: https://stripe.com/docs/api#event_types
            switch (stripeEvent.Type)
            {
                case "account.updated": //Occurs whenever an account status or property has changed.
                    StripeAccount stripeAccount = Mapper<StripeAccount>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(stripeAccount);
                    break;
                
                case "account.application.deauthorized": // Occurs whenever a user deauthorizes an application. Sent to the related application only.
                    // TODO                    
                    break;
                
                case "application_fee.created": // Occurs whenever an application fee is created on a charge.
                case "application_fee.refunded": // Occurs whenever an application fee is refunded, whether from refunding a charge or from refunding the application fee directly, including partial refunds.
                    StripeApplicationFee applicationFee =
                        Mapper<StripeApplicationFee>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(applicationFee);
                    break;
                
                case "balance.available": // Occurs whenever your Stripe balance has been updated (e.g. when a charge collected is available to be paid out). By default, Stripe will automatically transfer any funds in your balance to your bank account on a daily basis.
                    StripeBalance balance = Mapper<StripeBalance>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(balance);
                    break;
    
                case "charge.succeeded": // Occurs whenever a new charge is created and is successful.
                case "charge.failed": // Occurs whenever a failed charge attempt occurs.
                case "charge.refunded": // Occurs whenever a charge is refunded, including partial refunds.
                case "charge.captured": // Occurs whenever a previously uncaptured charge is captured.
                case "charge.updated": // Occurs whenever a charge description or metadata is updated.
                    StripeCharge charge = Mapper<StripeCharge>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(charge);
                    break;
                
                case "charge.dispute.created": // Occurs whenever a customer disputes a charge with their bank (chargeback).
                case "charge.dispute.updated": // Occurs when the dispute is updated (usually with evidence).
                case "charge.dispute.closed": // Occurs when the dispute is resolved and the dispute status changes to won or lost.
                    StripeDispute dispute = Mapper<StripeDispute>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(dispute);
                    break;
                
                case "customer.created": // Occurs whenever a new customer is created.
                case "customer.updated": // Occurs whenever any property of a customer changes.
                case "customer.deleted": // Occurs whenever a customer is deleted.
                    StripeCustomer customer = Mapper<StripeCustomer>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(customer);
                    break;
                
                case "customer.card.created": // Occurs whenever a new card is created for the customer.
                case "customer.card.updated": // Occurs whenever a card's details are changed.
                case "customer.card.deleted": // Occurs whenever a card is removed from a customer.
                    StripeCard card = Mapper<StripeCard>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(card);
                    break;
                
                case "customer.subscription.created": // Occurs whenever a customer with no subscription is signed up for a plan.
                case "customer.subscription.updated": // Occurs whenever a subscription changes. Examples would include switching from one plan to another, or switching status from trial to active.
                case "customer.subscription.deleted": // Occurs whenever a customer ends their subscription.
                case "customer.subscription.trial_will_end": // Occurs three days before the trial period of a subscription is scheduled to end.
                    StripeSubscription subscription =
                        Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(subscription);
                    break;
                
                case "customer.discount.created": // Occurs whenever a coupon is attached to a customer.
                case "customer.discount.updated": // Occurs whenever a customer is switched from one coupon to another.
                case "customer.discount.deleted":
                    StripeDiscount discount = Mapper<StripeDiscount>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(discount);
                    break;
                
                case "invoice.created": // Occurs whenever a new invoice is created. If you are using webhooks, Stripe will wait one hour after they have all succeeded to attempt to pay the invoice; the only exception here is on the first invoice, which gets created and paid immediately when you subscribe a customer to a plan. If your webhooks do not all respond successfully, Stripe will continue retrying the webhooks every hour and will not attempt to pay the invoice. After 3 days, Stripe will attempt to pay the invoice regardless of whether or not your webhooks have succeeded. See how to respond to a webhook.
                case "invoice.updated": // Occurs whenever an invoice changes (for example, the amount could change).
                case "invoice.payment_succeeded": // Occurs whenever an invoice attempts to be paid, and the payment succeeds.
                case "invoice.payment_failed": // Occurs whenever an invoice attempts to be paid, and the payment fails. This can occur either due to a declined payment, or because the customer has no active card. A particular case of note is that if a customer with no active card reaches the end of its free trial, an invoice.payment_failed notification will occur.
                    StripeInvoice invoice = Mapper<StripeInvoice>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(invoice);
                    break;
                
                case "invoiceitem.created": // Occurs whenever an invoice item is created.
                case "invoiceitem.updated": // Occurs whenever an invoice item is updated.
                case "invoiceitem.deleted": // Occurs whenever an invoice item is deleted.
                    StripeInvoiceItem invoiceItem = Mapper<StripeInvoiceItem>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(invoiceItem);
                    break;
                
                case "plan.created": // Occurs whenever a plan is created.
                case "plan.updated": // Occurs whenever a plan is updated.
                case "plan.deleted": // Occurs whenever a plan is deleted.
                    StripePlan plan = Mapper<StripePlan>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(plan);
                    break;
                
                case "coupon.created": // Occurs whenever a coupon is created.
                case "coupon.deleted": // Occurs whenever a coupon is deleted.
                    StripeCoupon coupon = Mapper<StripeCoupon>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(coupon);
                    break;
                
                case "transfer.created": // Occurs whenever a new transfer is created.
                case "transfer.updated": // Occurs whenever the description or metadata of a transfer is updated.
                case "transfer.paid": // Occurs whenever a sent transfer is expected to be available in the destination bank account. If the transfer failed, a transfer.failed webhook will additionally be sent at a later time.
                case "transfer.failed": // Occurs whenever Stripe attempts to send a transfer and that transfer fails.
                    StripeTransfer transfer = Mapper<StripeTransfer>.MapFromJson(stripeEvent.Data.Object.ToString());
                    _indexer.IndexEvent(transfer);
                    break;
            }
        }
    }
}
