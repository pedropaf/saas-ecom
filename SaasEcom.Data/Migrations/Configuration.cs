namespace SaasEcom.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
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
        }
    }
}
