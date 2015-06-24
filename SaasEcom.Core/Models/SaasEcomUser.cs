using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SaasEcom.Core.Models
{
    /// <summary>
    /// Saas Ecom User, used as base class for your Application User
    /// </summary>
    public abstract class SaasEcomUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the registration date for the user.
        /// </summary>
        /// <value>
        /// The registration date.
        /// </value>
        public virtual DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the last login time.
        /// </summary>
        /// <value>
        /// The last login time.
        /// </value>
        public virtual DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// Gets or sets the stripe customer identifier.
        /// </summary>
        /// <value>
        /// The stripe customer identifier.
        /// </value>
        public string StripeCustomerId { get; set; }

        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>
        /// <value>
        /// The subscriptions.
        /// </value>
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Gets or sets the invoices.
        /// </summary>
        /// <value>
        /// The invoices.
        /// </value>
        public virtual IList<Invoice> Invoices { get; set; }

        /// <summary>
        /// Gets or sets the credit cards.
        /// </summary>
        /// <value>
        /// The credit cards.
        /// </value>
        public virtual IList<CreditCard> CreditCards { get; set; } // The actual credit card number is not stored! 

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>
        /// The ip address.
        /// </value>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the ip address country.
        /// </summary>
        /// <value>
        /// The ip address country.
        /// </value>
        public string IPAddressCountry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SaasEcomUser"/> is delinquent. Whether or not the latest charge for the customer’s latest invoice has failed
        /// </summary>
        /// <value>
        ///   <c>true</c> if delinquent; otherwise, <c>false</c>.
        /// </value>
        public bool Delinquent { get; set; }

        /// <summary>
        /// Gets or sets the lifetime value for the customer (total spent in the app)
        /// </summary>
        /// <value>
        /// The lifetime value.
        /// </value>
        public decimal LifetimeValue { get; set; }

        /// <summary>
        /// Generates the user identity asynchronous.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SaasEcomUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        /// <summary>
        /// Determines whether [has user details].
        /// </summary>
        /// <returns></returns>
        public bool HasUserDetails()
        {
            if (string.IsNullOrEmpty(FirstName) ||
                string.IsNullOrEmpty(LastName))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether [has payment details].
        /// </summary>
        /// <returns></returns>
        public bool HasPaymentDetails()
        {
            return CreditCards != null && CreditCards.Any();
        }
    }
}
