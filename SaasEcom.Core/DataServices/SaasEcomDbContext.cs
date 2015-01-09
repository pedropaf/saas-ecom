using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices
{
    /// <summary>
    /// Implementation of the Base database context.
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    public class SaasEcomDbContext<TUser> : 
        IdentityDbContext<TUser>, IDbContext<TUser> where TUser : IdentityUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaasEcomDbContext{TUser}"/> class.
        /// </summary>
        public SaasEcomDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaasEcomDbContext{TUser}"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SaasEcomDbContext(string connectionString) : base(connectionString, throwIfV1Schema: false)
        {
        }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        IDbSet<TUser> IDbContext<TUser>.Users
        {
            get { return base.Users; }
            set { base.Users = value; }
        }

        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>
        /// <value>
        /// The subscriptions.
        /// </value>
        public DbSet<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Gets or sets the subscription plans.
        /// </summary>
        /// <value>
        /// The subscription plans.
        /// </value>
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

        /// <summary>
        /// Gets or sets the subscription plan properties.
        /// </summary>
        /// <value>
        /// The subscription plan properties.
        /// </value>
        public DbSet<SubscriptionPlanProperty> SubscriptionPlanProperties { get; set; }

        /// <summary>
        /// Gets or sets the invoices.
        /// </summary>
        /// <value>
        /// The invoices.
        /// </value>
        public DbSet<Invoice> Invoices { get; set; }

        /// <summary>
        /// Gets or sets the credit cards.
        /// </summary>
        /// <value>
        /// The credit cards.
        /// </value>
        public DbSet<CreditCard> CreditCards { get; set; }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns></returns>
        public new Task SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        /// <summary>
        /// Entries the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public new DbEntityEntry<T> Entry<T>(T entity) where T : class
        {
            return base.Entry(entity);
        }
    }
}
