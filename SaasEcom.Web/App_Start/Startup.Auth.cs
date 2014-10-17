using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SaasEcom.Web.Data;
using SaasEcom.Web.Identity;

namespace SaasEcom.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseGoogleAuthentication("873693220515-v6rf15tmc9rvmm0o1klhiklscmvuna0f.apps.googleusercontent.com", "XBhtXX_AE2YZqK_gx8EuQAoS");
                //clientId: "204631176008-o8mocg7j5vpc6g9p1c48k87rlgspifl6.apps.googleusercontent.com",
                //clientSecret: "lH7P7x3Ew53-B8VzlIc4EMem");

            // Register these two callback methods to create one instance of each per Request
            app.CreatePerOwinContext<ApplicationDbContext>(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
        }
    }
}
