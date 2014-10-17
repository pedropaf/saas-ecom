using System.Collections.Generic;
using SaasEcom.Core.Models;

namespace SaasEcom.Web.Areas.Dashboard.ViewModels
{
    public class DashboardViewModel
    {
        public List<SubscriptionViewModel> Subscriptions { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}