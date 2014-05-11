using System;
using Nest;
using Stripe;

namespace SaasEcom.Data.Infrastructure.Analytics.Stripe
{
    public class StripeIndexer
    {
        private readonly ElasticClient _elasticsearchClient;

        public StripeIndexer()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            _elasticsearchClient = new ElasticClient(settings);    
        }
        
        internal void IndexEvent(StripeCharge charge)
        {
            _elasticsearchClient.Index(charge, i => i.Index("stripe-charges"));
        }

        internal void IndexEvent(StripeDispute dispute)
        {
            _elasticsearchClient.Index(dispute, i => i.Index("stripe-disputes"));
        }

        internal void IndexEvent(StripeAccount stripeAccount)
        {
            _elasticsearchClient.Index(stripeAccount, i => i.Index("stripe-accounts"));
        }

        internal void IndexEvent(StripeApplicationFee applicationFee)
        {
            _elasticsearchClient.Index(applicationFee, i => i.Index("stripe-application-fees"));
        }

        internal void IndexEvent(StripeBalance balance)
        {
            _elasticsearchClient.Index(balance, i => i.Index("stripe-balances"));
        }

        internal void IndexEvent(StripeCustomer customer)
        {
            _elasticsearchClient.Index(customer, i => i.Index("stripe-customers"));
        }

        internal void IndexEvent(StripeCard card)
        {
            _elasticsearchClient.Index(card, i => i.Index("stripe-cards"));
        }

        internal void IndexEvent(StripeSubscription subscription)
        {
            _elasticsearchClient.Index(subscription, i => i.Index("stripe-subscriptions"));
        }

        internal void IndexEvent(StripeDiscount discount)
        {
            _elasticsearchClient.Index(discount, i => i.Index("stripe-discounts"));
        }

        internal void IndexEvent(StripeInvoice invoice)
        {
            _elasticsearchClient.Index(invoice, i => i.Index("stripe-invoices"));
        }

        internal void IndexEvent(StripeInvoiceItem invoiceItem)
        {
            _elasticsearchClient.Index(invoiceItem, i => i.Index("stripe-invoice-items"));
        }

        internal void IndexEvent(StripePlan plan)
        {
            _elasticsearchClient.Index(plan, i => i.Index("stripe-plans"));
        }

        internal void IndexEvent(StripeCoupon coupon)
        {
            _elasticsearchClient.Index(coupon, i => i.Index("stripe-coupons"));
        }

        internal void IndexEvent(StripeTransfer transfer)
        {
            _elasticsearchClient.Index(transfer, i => i.Index("stripe-transfers"));
        }
    }
}
