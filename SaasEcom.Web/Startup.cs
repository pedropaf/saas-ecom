using Microsoft.Owin;
using Owin;
using SaasEcom.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace SaasEcom.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
