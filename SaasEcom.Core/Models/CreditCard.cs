using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaasEcom.Core.Models
{
    public class CreditCard
    {
        public int Id { get; set; }

        public string StripeId { get; set; }
        public string StripeToken { get; set; }

        [Required]
        public string Name { get; set; }

        public string Last4 { get; set; }
        public string Type { get; set; }
        public string Fingerprint { get; set; }

        [Required]
        [DisplayName("City")]
        public string AddressCity { get; set; }

        [Required]
        [DisplayName("Country")]
        public string AddressCountry { get; set; }

        [Required]
        [DisplayName("Address")]
        public string AddressLine1 { get; set; }
        
        [DisplayName("Address")]
        public string AddressLine2 { get; set; }
        
        [DisplayName("State")]
        public string AddressState { get; set; }

        [Required]
        [DisplayName("Post code")]
        public string AddressZip { get; set; }

        [Required]
        [MaxLength(16)]
        [NotMapped]
        [DisplayName("Card Number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(4, ErrorMessage = "Too long")]
        [DisplayName("CVC")]
        public string Cvc { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, 12, ErrorMessage = "Invalid")]
        public string ExpirationMonth { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(2014, 2030, ErrorMessage = "Invalid")]
        public string ExpirationYear { get; set; }

        public string SaasEcomUserId { get; set; }

        public void ClearCreditCardDetails()
        {
            this.ExpirationMonth = null;
            this.ExpirationYear = null;
            this.Last4 = null;
            this.Fingerprint = null;
            this.StripeId = null;
            this.StripeToken = null;
            this.Cvc = null;
            this.Type = null;
        }
    }
}
