using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaasEcom.Data.Models;

namespace SaasEcom.Web.Mappers
{
    public static class InvoiceMappers
    {
        public static Invoice MapToInvoice(dynamic stripeInvoice)
        {
            return new Invoice
            {
                StripeId = stripeInvoice.Id,
                StripeCustomerId = stripeInvoice.CustomerId,
                TotalAmount = stripeInvoice.Total,
                Date = stripeInvoice.Date,
                
                // InvoiceConcept = Build,
                
                // TODO: Add the rest

            };
        }
    }
}
