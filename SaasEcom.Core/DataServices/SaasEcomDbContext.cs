using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices
{
    public class SaasEcomDbContext : Microsoft.AspNet.Identity.EntityFramework.IdentityDbContext<ApplicationUser>, IDbContext
    {
        public SaasEcomDbContext()
            : base()
        {
        }

        public SaasEcomDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<StripeAccount> StripeAccounts { get; set; }
        public new Task SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public new DbEntityEntry<T> Entry<T>(T entity) where T : class
        {
            return base.Entry(entity);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(u => u.StripeAccount)
                .WithRequired(sa => sa.ApplicationUser);
        }
    }
}
