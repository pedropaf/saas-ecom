using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Stripe;

namespace SaasEcom.Web.Controllers
{
    public class StripeWebhooksController : Controller
    {
        // GET: /StripeWebhooks/
        public ActionResult Index()
        {
            var json = new StreamReader(Request.InputStream).ReadToEnd();

            var stripeEvent = StripeEventUtility.ParseEvent(json);

            // take a look at all the types here: https://stripe.com/docs/api#event_types
            switch (stripeEvent.Type)
            {
                case "charge.succeeded": // Occurs whenever a new charge is created and is successful.
                    // TODO
                    break;

                case "charge.failed": // Occurs whenever a failed charge attempt occurs.
                    // TODO
                    break;

                case "charge.refunded": // Occurs whenever a charge is refunded, including partial refunds.
                    var stripeCharge = Stripe.Mapper<StripeCharge>.MapFromJson(stripeEvent.Data.Object.ToString());
                    // TODO
                    break;

                case "charge.captured": // Occurs whenever a previously uncaptured charge is captured.
                    // TODO
                    break;

                case "customer.subscription.trial_will_end": // Occurs three days before the trial period of a subscription is scheduled to end.
                    // TODO
                    break;

                case "invoice.created": // Occurs whenever a new invoice is created. If you are using webhooks, Stripe will wait one hour after they have all succeeded to attempt to pay the invoice; the only exception here is on the first invoice, which gets created and paid immediately when you subscribe a customer to a plan. If your webhooks do not all respond successfully, Stripe will continue retrying the webhooks every hour and will not attempt to pay the invoice. After 3 days, Stripe will attempt to pay the invoice regardless of whether or not your webhooks have succeeded. See how to respond to a webhook.
                    // TODO
                    break;

                case "invoice.payment_succeeded": // Occurs whenever an invoice attempts to be paid, and the payment succeeds.
                    // TODO
                    break;

                case "invoice.payment_failed": // Occurs whenever an invoice attempts to be paid, and the payment fails. This can occur either due to a declined payment, or because the customer has no active card. A particular case of note is that if a customer with no active card reaches the end of its free trial, an invoice.payment_failed notification will occur.
                    // TODO
                    break;
            }

            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }
	}
}