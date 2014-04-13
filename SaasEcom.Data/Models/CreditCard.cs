using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaasEcom.Data.Models
{
    public class CreditCard
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
        public string Cvc { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        
        [NotMapped]
        public string CardNumber { get; set; }

        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
