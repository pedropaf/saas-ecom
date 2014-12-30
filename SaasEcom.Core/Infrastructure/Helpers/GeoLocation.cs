using System.Linq;
using System.Web;

namespace SaasEcom.Core.Infrastructure.Helpers
{
    public static class GeoLocation
    {
        public static string GetUserIP(HttpContextBase context)
        {
            var ip = (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null
                      && context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                     ? context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                     : context.Request.ServerVariables["REMOTE_ADDR"];
            if (ip.Contains(","))
            {
                ip = ip.Split(',').First().Trim();
            }

            return ip;
        }

        // TODO: Add Maxmind GeoIP Service
    }
}
