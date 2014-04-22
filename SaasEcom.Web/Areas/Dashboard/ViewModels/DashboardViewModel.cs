using System.Collections.Generic;
using SaasEcom.Data.Models;

namespace SaasEcom.Web.Areas.Dashboard.ViewModels
{
    public class DashboardViewModel
    {
        public List<SubscriptionViewModel> Subscriptions { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}