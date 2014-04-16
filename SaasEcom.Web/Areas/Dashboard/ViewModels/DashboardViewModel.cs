using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaasEcom.Data.Models;

namespace SaasEcom.Web.Areas.Dashboard.ViewModels
{
    public class DashboardViewModel
    {
        public List<Subscription> Subscriptions { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}