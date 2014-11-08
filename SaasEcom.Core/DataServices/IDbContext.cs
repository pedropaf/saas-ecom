using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices
{
    public interface IDbContext<TUser> where TUser : class
    {
        IDbSet<TUser> Users { get; set; }
        DbSet<Subscription> Subscriptions { get; set; }
        DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        DbSet<Invoice> Invoices { get; set; }
        DbSet<CreditCard> CreditCards { get; set; }
        DbSet<StripeAccount> StripeAccounts { get; set; }

        Task<int> SaveChangesAsync();
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
