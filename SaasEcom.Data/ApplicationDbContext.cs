using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SaasEcom.Data.Models;

namespace SaasEcom.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<StripeAccount> StripeAccounts { get; set; }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(u => u.StripeAccount)
                .WithRequired(sa => sa.ApplicationUser);
        }
    }
}
