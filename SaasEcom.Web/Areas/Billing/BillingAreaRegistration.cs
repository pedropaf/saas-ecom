using System.Web.Mvc;

namespace SaasEcom.Web.Areas.Billing
{
    public class BillingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Billing";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Billing_default",
                "Billing/{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                new[] { "SaasEcom.Web.Areas.Billing.Controllers" }
            );
        }
    }
}