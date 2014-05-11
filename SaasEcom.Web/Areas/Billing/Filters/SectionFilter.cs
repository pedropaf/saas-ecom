using System.Web.Mvc;

namespace SaasEcom.Web.Areas.Billing.Filters
{
    public class SectionFilter : ActionFilterAttribute
    {
        public string Section { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.Section = Section;

            base.OnActionExecuted(filterContext);
        }
    }
}