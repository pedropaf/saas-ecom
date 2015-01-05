using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices
{
    /// <summary>
    /// Interface for Database context.
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    public interface IDbContext<TUser> where TUser : class
    {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        IDbSet<TUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>
        /// <value>
        /// The subscriptions.
        /// </value>
        DbSet<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Gets or sets the subscription plans.
        /// </summary>
        /// <value>
        /// The subscription plans.
        /// </value>
        DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

        /// <summary>
        /// Gets or sets the invoices.
        /// </summary>
        /// <value>
        /// The invoices.
        /// </value>
        DbSet<Invoice> Invoices { get; set; }

        /// <summary>
        /// Gets or sets the credit cards.
        /// </summary>
        /// <value>
        /// The credit cards.
        /// </value>
        DbSet<CreditCard> CreditCards { get; set; }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Entries the specified entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
