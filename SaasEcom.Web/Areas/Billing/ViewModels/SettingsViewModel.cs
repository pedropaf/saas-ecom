using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaasEcom.Data.Models;

namespace SaasEcom.Web.Areas.Billing.ViewModels
{
    public class SettingsViewModel
    {
        public StripeAccount StripeAccount { get; set; }
    }
}