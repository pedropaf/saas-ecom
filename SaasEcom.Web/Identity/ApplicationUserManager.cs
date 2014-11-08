using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SaasEcom.Core.Infrastructure;
using SaasEcom.Core.Models;
using SaasEcom.Web.Data;

namespace SaasEcom.Web.Identity
{
    public class ApplicationUserManager : UserManager<SaasEcomUser>
    {
        public ApplicationUserManager(IUserStore<SaasEcomUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<SaasEcomUser>(context.Get<ApplicationDbContext>()))
            {
                PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = true,
                    RequireLowercase = false,
                    RequireUppercase = false,
                },
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5),
                MaxFailedAccessAttemptsBeforeLockout = 15,
                EmailService = new EmailIdentityService()
            };

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<SaasEcomUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }
}
