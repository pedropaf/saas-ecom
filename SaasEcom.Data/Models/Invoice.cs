using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaasEcom.Data.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string StripeId { get; set; }
        public string StripeCustomerId { get; set; }

        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser Customer { get; set; }

        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        public string BillingName { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }

        public string InvoiceConcept { get; set; }

        public decimal TotalAmount { get; set; }

        public Status InvoiceStatus { get; set; }

        public enum Status
        {
            Pending, Paid, Failed
        }

        // Add line items?
    }
}
