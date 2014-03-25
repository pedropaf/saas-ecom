using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StripeSaas.Startup))]
namespace StripeSaas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
