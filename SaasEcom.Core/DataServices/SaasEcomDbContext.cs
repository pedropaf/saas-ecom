using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices
{
    public class SaasEcomDbContext<TUser> : 
        IdentityDbContext<TUser>, IDbContext<TUser> where TUser : IdentityUser
    {
        public SaasEcomDbContext()
        {
        }

        public SaasEcomDbContext(string connectionString) : base(connectionString, throwIfV1Schema: false)
        {
        }

        IDbSet<TUser> IDbContext<TUser>.Users
        {
            get { return base.Users; }
            set { base.Users = value; }
        }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public new Task SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public new DbEntityEntry<T> Entry<T>(T entity) where T : class
        {
            return base.Entry(entity);
        }
    }
}
