using SaasEcom.Data.Models;

namespace SaasEcom.Web.Areas.Dashboard.ViewModels
{
    public class SubscribeViewModel
    {
        public string PlanFriendlyId { get; set; }

        public CreditCard CreditCard { get; set; }
    }
}