using Postal;
using SaasEcom.Data.Models;

namespace SaasEcom.Web.ViewModels
{
    public abstract class BaseEmail : Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
    }

    public class ResetPasswordEmail : BaseEmail
    {
        public string ResetPasswordLink { get; set; }
    }

    public class WelcomeEmail : BaseEmail
    {
    }

    public class InvoiceEmail : BaseEmail
    {
        public Invoice Invoice { get; set; }
    }
}