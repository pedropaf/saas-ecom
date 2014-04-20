using System;
using System.Configuration;
using System.Diagnostics;
using SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe;

namespace SaasEcom.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<SaasEcom.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // Setup roles for Identity Provider
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("admin"))
            {
                roleManager.Create(new IdentityRole {Name = "admin"});
            }
            if (!roleManager.RoleExists("subscriber"))
            {
                roleManager.Create(new IdentityRole { Name = "subscriber" });
            }

            // Setup users for Identity Provider
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            if (userManager.Users.FirstOrDefault(u => u.UserName == "admin@admin.com") == null)
            {
                var user = new ApplicationUser { UserName = "admin@admin.com" };
                userManager.Create(user, "password");
                userManager.AddToRole(user.Id, "admin");
            }

            // Create Subscriptions Plans
            var starterPlan = new SubscriptionPlan
            {
                FriendlyId = "Starter",
                Interval = SubscriptionPlan.SubscriptionInterval.Monthly,
                Name = "Starter",
                Price = 14.99,
                TrialPeriodInDays = 30,
                StatementDescription = "SAAS billing starter"
            };
            var premiumPlan = new SubscriptionPlan
            {
                FriendlyId = "Premium",
                Interval = SubscriptionPlan.SubscriptionInterval.Monthly,
                Name = "Premium",
                Price = 29.99,
                TrialPeriodInDays = 30,
                StatementDescription = "SAAS billing premium"
            };
            var ultimatePlan = new SubscriptionPlan
            {
                FriendlyId = "Ultimate",
                Interval = SubscriptionPlan.SubscriptionInterval.Monthly,
                Name = "Ultimate",
                Price = 74.99,
                TrialPeriodInDays = 30,
                StatementDescription = "SAAS billing ultimate"
            };

            context.SubscriptionPlans.AddOrUpdate(p => p.FriendlyId, starterPlan, premiumPlan, ultimatePlan);
            context.SaveChanges();

            // Create plans in Stripe
            try
            {
                var stripeService = new StripePaymentProcessorProvider(ConfigurationManager.AppSettings.Get("stripe_secret_key"));

                var plan = stripeService.GetSubscriptionPlan(starterPlan.FriendlyId);
                if (plan == null)
                {
                    stripeService.CreateSubscriptionPlan(starterPlan);
                }

                plan = stripeService.GetSubscriptionPlan(premiumPlan.FriendlyId);
                if (plan == null)
                {
                    stripeService.CreateSubscriptionPlan(premiumPlan);
                }

                plan = stripeService.GetSubscriptionPlan(ultimatePlan.FriendlyId);
                if (plan == null)
                {
                    stripeService.CreateSubscriptionPlan(ultimatePlan);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Add the stripe-key in Web.config.");
            }
        }
    }
}
