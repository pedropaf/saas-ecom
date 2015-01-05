using System.Linq;
using System.Web;

namespace SaasEcom.Core.Infrastructure.Helpers
{
    /// <summary>
    /// Helper class to retrieve user's IP and Location
    /// </summary>
    public static class GeoLocation
    {
        /// <summary>
        /// Gets the user ip.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string GetUserIP(HttpRequestBase request)
        {
            var ip = (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null
                      && request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                     ? request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                     : request.ServerVariables["REMOTE_ADDR"];
            if (ip.Contains(","))
            {
                ip = ip.Split(',').First().Trim();
            }

            return ip;
        }

        // TODO: Add Maxmind GeoIP Service
    }
}
