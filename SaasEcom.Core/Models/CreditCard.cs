using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaasEcom.Core.Models
{
    /// <summary>
    /// Credit Card
    /// </summary>
    public class CreditCard
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the stripe identifier.
        /// </summary>
        /// <value>
        /// The stripe identifier.
        /// </value>
        public string StripeId { get; set; }

        /// <summary>
        /// Gets or sets the stripe token. Represents a credit card stored in Stripe.
        /// </summary>
        /// <value>
        /// The stripe token.
        /// </value>
        public string StripeToken { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the last 4 digits of the credit card.
        /// </summary>
        /// <value>
        /// The last4.
        /// </value>
        public string Last4 { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Uniquely identifies this particular card number. You can use this attribute to check whether two customers who’ve signed up with you are using the same card number, for example.
        /// </summary>
        /// <value>
        /// The fingerprint.
        /// </value>
        public string Fingerprint { get; set; }

        /// <summary>
        /// Gets or sets the address city.
        /// </summary>
        /// <value>
        /// The address city.
        /// </value>
        [Required]
        [DisplayName("City")]
        public string AddressCity { get; set; }

        /// <summary>
        /// Gets or sets the address country.
        /// </summary>
        /// <value>
        /// The address country.
        /// </value>
        [Required]
        [DisplayName("Country")]
        public string AddressCountry { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        [Required]
        [DisplayName("Address")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        [DisplayName("Address")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the state of the address.
        /// </summary>
        /// <value>
        /// The state of the address.
        /// </value>
        [DisplayName("State")]
        public string AddressState { get; set; }

        /// <summary>
        /// Gets or sets the address zip.
        /// </summary>
        /// <value>
        /// The address zip.
        /// </value>
        [Required]
        [DisplayName("Post code")]
        public string AddressZip { get; set; }

        /// <summary>
        /// Gets or sets the card number. (Not stored in the Database)
        /// </summary>
        /// <value>
        /// The card number.
        /// </value>
        [MaxLength(16)]
        [NotMapped]
        [DisplayName("Card Number")]
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the CVC.
        /// </summary>
        /// <value>
        /// The CVC.
        /// </value>
        [Required(ErrorMessage = "Required")]
        [MaxLength(4, ErrorMessage = "Too long")]
        [DisplayName("CVC")]
        public string Cvc { get; set; }

        /// <summary>
        /// Gets or sets the expiration month.
        /// </summary>
        /// <value>
        /// The expiration month.
        /// </value>
        [Required(ErrorMessage = "Required")]
        [Range(1, 12, ErrorMessage = "Invalid")]
        public string ExpirationMonth { get; set; }

        /// <summary>
        /// Gets or sets the expiration year.
        /// </summary>
        /// <value>
        /// The expiration year.
        /// </value>
        [Required(ErrorMessage = "Required")]
        [Range(2015, 2030, ErrorMessage = "Invalid")]
        public string ExpirationYear { get; set; }

        /// <summary>
        /// Gets or sets the Two-letter ISO code representing the country of the card.  (This is returned by Stripe)
        /// </summary>
        /// <value>
        /// The card country.
        /// </value>
        public string CardCountry { get; set; }

        /// <summary>
        /// Gets or sets the saas ecom user identifier.
        /// </summary>
        /// <value>
        /// The saas ecom user identifier.
        /// </value>
        public string SaasEcomUserId { get; set; }

        /// <summary>
        /// Clears the credit card details.
        /// </summary>
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
