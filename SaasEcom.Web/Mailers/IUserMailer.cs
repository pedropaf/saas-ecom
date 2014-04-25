using Mvc.Mailer;

namespace SaasEcom.Web.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome();
			MvcMailMessage PasswordReset();
			MvcMailMessage Invoice();
	}
}