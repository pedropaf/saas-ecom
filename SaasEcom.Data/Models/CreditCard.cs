using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaasEcom.Data.Models
{
    public sealed class CreditCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Last4 { get; set; }
        public string Type { get; set; }
        public string Fingerprint { get; set; }
        
        [DisplayName("City")]
        public string AddressCity { get; set; }
        [DisplayName("Country")]
        public string AddressCountry { get; set; }
        [DisplayName("Address")]
        public string AddressLine1 { get; set; }
        [DisplayName("Address (line 2)")]
        public string AddressLine2 { get; set; }
        [DisplayName("State")]
        public string AddressState { get; set; }
        [DisplayName("Post code")]
        public string AddressZip { get; set; }
        
        [Required]
        [MaxLength(4)]
        public string Cvc { get; set; }

        [Required]
        [Range(1, 12)]
        [DisplayName("Exp. Month")]
        public string ExpirationMonth { get; set; }

        [Required]
        [Range(2014, 2030)]
        [DisplayName("Exp. Year")]
        public string ExpirationYear { get; set; }

        [Required]
        [MaxLength(20)]
        [NotMapped]
        [DisplayName("Card Number")]
        public string CardNumber { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
