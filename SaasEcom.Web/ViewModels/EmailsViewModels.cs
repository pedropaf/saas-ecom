using Postal;

namespace SaasEcom.Web.ViewModels
{
    public class ResetPasswordEmail : Email
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }

        public string ResetPasswordLink { get; set; }
    }
}