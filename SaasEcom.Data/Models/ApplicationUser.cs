using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SaasEcom.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string StripeCustomerId { get; set; }

        public virtual IList<Subscription> Subscriptions { get; set; }
    }
}
