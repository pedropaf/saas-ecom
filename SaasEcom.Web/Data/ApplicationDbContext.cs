using SaasEcom.Core.DataServices;

namespace SaasEcom.Web.Data
{
    public class ApplicationDbContext : SaasEcomDbContext
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