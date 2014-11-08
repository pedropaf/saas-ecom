using SaasEcom.Core.DataServices;
using SaasEcom.Core.Models;

namespace SaasEcom.Web.Data
{
    public class ApplicationDbContext : SaasEcomDbContext<SaasEcomUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public ApplicationDbContext(string connectionString) : base(connectionString) { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}