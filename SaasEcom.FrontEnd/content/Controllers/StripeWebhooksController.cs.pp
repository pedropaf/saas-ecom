using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using SaasEcom.Core.DataServices.Storage;
using SaasEcom.Core.Models;
using Stripe;
using $rootnamespace$.Models;

namespace $rootnamespace$.Controllers
{
    public class StripeWebhooksController : Controller
    {
        private InvoiceDataService<ApplicationDbContext, ApplicationUser> _invoiceDataService;
        private InvoiceDataService<ApplicationDbContext, ApplicationUser> InvoiceDataService
        {
            get
            {
                return _invoiceDataService ??
                     (_invoiceDataService = new InvoiceDataService<ApplicationDbContext, ApplicationUser>(Request.GetOwinContext().Get<ApplicationDbContext>()));
            }
        }

        // GET: /StripeWebhooks/
        public async Task<HttpStatusCodeResult> Index()
        {
            var json = new StreamReader(Request.InputStream).ReadToEnd();

            var stripeEvent = StripeEventUtility.ParseEvent(json);

            #region All Event types
            // All the event types explained here: https://stripe.com/docs/api#event_types
            switch (stripeEvent.Type)
            {
                case "account.updated": //Occurs whenever an account status or property has changed.
                    break;
                case "account.application.deauthorized": // Occurs whenever a user deauthorizes an application. Sent to the related application only.
                    break;
                case "application_fee.created": // Occurs whenever an application fee is created on a charge.
                    break;
                case "application_fee.refunded": // Occurs whenever an application fee is refunded, whether from refunding a charge or from refunding the application fee directly, including partial refunds.
                    break;
                case "balance.available": // Occurs whenever your Stripe balance has been updated (e.g. when a charge collected is available to be paid out). By default, Stripe will automatically transfer any funds in your balance to your bank account on a daily basis.
                    break;
                case "charge.succeeded": // Occurs whenever a new charge is created and is successful.
                    break;
                case "charge.failed": // Occurs whenever a failed charge attempt occurs.
                    break;
                case "charge.refunded": // Occurs whenever a charge is refunded, including partial refunds.
                    var stripeCharge = Stripe.Mapper<StripeCharge>.MapFromJson(stripeEvent.Data.Object.ToString());
                    break;
                case "charge.captured": // Occurs whenever a previously uncaptured charge is captured.
                    break;
                case "charge.updated": // Occurs whenever a charge description or metadata is updated.
                    break;
                case "charge.dispute.created": // Occurs whenever a customer disputes a charge with their bank (chargeback).
                    break;
                case "charge.dispute.updated": // Occurs when the dispute is updated (usually with evidence).
                    break;
                case "charge.dispute.closed": // Occurs when the dispute is resolved and the dispute status changes to won or lost.
                    break;
                case "customer.created": // Occurs whenever a new customer is created.
                    break;
                case "customer.updated": // Occurs whenever any property of a customer changes.
                    break;
                case "customer.deleted": // Occurs whenever a customer is deleted.
                    break;
                case "customer.card.created": // Occurs whenever a new card is created for the customer.
                    break;
                case "customer.card.updated": // Occurs whenever a card's details are changed.
                    break;
                case "customer.card.deleted": // Occurs whenever a card is removed from a customer.
                    break;
                case "customer.subscription.created": // Occurs whenever a customer with no subscription is signed up for a plan.
                    break;
                case "customer.subscription.updated": // Occurs whenever a subscription changes. Examples would include switching from one plan to another, or switching status from trial to active.
                    break;
                case "customer.subscription.deleted": // Occurs whenever a customer ends their subscription.
                    break;
                case "customer.subscription.trial_will_end": // Occurs three days before the trial period of a subscription is scheduled to end.
                    // TODO: If the user hasn't added credit card details -> Send email reminder.
                    break;
                case "customer.discount.created": // Occurs whenever a coupon is attached to a customer.
                    break;
                case "customer.discount.updated": // Occurs whenever a customer is switched from one coupon to another.
                    break;
                case "customer.discount.deleted":
                    break;
                case "invoice.created": // Occurs whenever a new invoice is created. If you are using webhooks, Stripe will wait one hour after they have all succeeded to attempt to pay the invoice; the only exception here is on the first invoice, which gets created and paid immediately when you subscribe a customer to a plan. If your webhooks do not all respond successfully, Stripe will continue retrying the webhooks every hour and will not attempt to pay the invoice. After 3 days, Stripe will attempt to pay the invoice regardless of whether or not your webhooks have succeeded. See how to respond to a webhook.
                case "invoice.payment_succeeded": // Occurs whenever an invoice attempts to be paid, and the payment succeeds.
                case "invoice.payment_failed": // Occurs whenever an invoice attempts to be paid, and the payment fails. This can occur either due to a declined payment, or because the customer has no active card. A particular case of note is that if a customer with no active card reaches the end of its free trial, an invoice.payment_failed notification will occur.
                    var stripeInvoice = Stripe.Mapper<StripeInvoice>.MapFromJson(stripeEvent.Data.Object.ToString());
                    var invoice = Mapper.Map<Invoice>(stripeInvoice);
                    await InvoiceDataService.CreateOrUpdateAsync(invoice);
                    // TODO: Send invoice by email
                    break;
                case "invoice.updated": // Occurs whenever an invoice changes (for example, the amount could change).
                    // TODO: Update invoice
                    break;
                case "invoiceitem.created": // Occurs whenever an invoice item is created.
                    break;
                case "invoiceitem.updated": // Occurs whenever an invoice item is updated.
                    break;
                case "invoiceitem.deleted": // Occurs whenever an invoice item is deleted.
                    break;
                case "plan.created": // Occurs whenever a plan is created.
                    break;
                case "plan.updated": // Occurs whenever a plan is updated.
                    break;
                case "plan.deleted": // Occurs whenever a plan is deleted.
                    break;
                case "coupon.created": // Occurs whenever a coupon is created.
                    break;
                case "coupon.deleted": // Occurs whenever a coupon is deleted.
                    break;
                case "transfer.created": // Occurs whenever a new transfer is created.
                    break;
                case "transfer.updated": // Occurs whenever the description or metadata of a transfer is updated.
                    break;
                case "transfer.paid": // Occurs whenever a sent transfer is expected to be available in the destination bank account. If the transfer failed, a transfer.failed webhook will additionally be sent at a later time.
                    break;
                case "transfer.failed": // Occurs whenever Stripe attempts to send a transfer and that transfer fails.
                    break;
            }
            #endregion

            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }
    }
}
