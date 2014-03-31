using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SaasEcom.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SaasEcom.Data.ApplicationDbContext context)
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
            
            if (userManager.Users.FirstOrDefault(u => u.UserName == "admin") == null)
            {
                var user = new ApplicationUser { UserName = "admin" };
                userManager.Create(user, "password");
                userManager.AddToRole(user.Id, "admin");
            }
        }
    }
}
