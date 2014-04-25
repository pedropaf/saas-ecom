using Mvc.Mailer;

namespace SaasEcom.Web.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage Welcome()
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "Welcome";
				x.ViewName = "Welcome";
				x.To.Add("some-email@example.com");
			});
		}
 
		public virtual MvcMailMessage PasswordReset()
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "PasswordReset";
				x.ViewName = "PasswordReset";
				x.To.Add("some-email@example.com");
			});
		}
 
		public virtual MvcMailMessage Invoice()
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "Invoice";
				x.ViewName = "Invoice";
				x.To.Add("some-email@example.com");
			});
		}
 	}
}