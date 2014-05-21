using System.Linq;
using AutoMapper;
using SaasEcom.Data.Models;
using SaasEcom.Web.Areas.Billing.ViewModels;

namespace SaasEcom.Web
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<ApplicationUser, CustomerViewModel>()
                .AfterMap((user, viewModel) =>
                {
                    viewModel.SubscriptionPlan = user.Subscriptions.FirstOrDefault() == null
                        ? "No subscription"
                        : user.Subscriptions.First().SubscriptionPlan.Name;
                    viewModel.SubscriptionPlanPrice = user.Subscriptions.FirstOrDefault() == null
                        ? "--"
                        : user.Subscriptions.First().SubscriptionPlan.Price.ToString();
                    viewModel.SubscriptionPlanCurrency = user.Subscriptions.FirstOrDefault() == null
                        ? ""
                        : user.Subscriptions.First().SubscriptionPlan.CurrencySymbol;
                });
        }
    }
}
