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
        public string AddressCity { get; set; }
        public string AddressCountry { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressState { get; set; }
        public string AddressZip { get; set; }
        
        [MaxLength(4)]
        public string Cvc { get; set; }
        
        [Range(1, 12)]
        public string ExpirationMonth { get; set; }

        [Range(2014, 2030)]
        public string ExpirationYear { get; set; }
        
        [MaxLength(20)]
        [NotMapped]
        public string CardNumber { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
