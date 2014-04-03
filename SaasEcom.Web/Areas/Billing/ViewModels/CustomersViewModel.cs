using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaasEcom.Web.Areas.Billing.ViewModels
{
    public class CustomersViewModel
    {
        public List<Data.Models.ApplicationUser> Customers { get; set; }
    }
}