using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SaasEcom.Core.Models;
using SaasEcom.Web.Areas.Billing.ViewModels;
using Stripe;

namespace SaasEcom.Web
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<SaasEcomUser, CustomerViewModel>()
                .AfterMap((user, viewModel) =>
                {
                    var creditCard = user.CreditCards.FirstOrDefault();
                    if (creditCard != null)
                    {
                        viewModel.Name = creditCard.Name;
                        viewModel.Address = string.Format("{0} {1}", creditCard.AddressLine1, creditCard.AddressLine2);
                        viewModel.City = creditCard.AddressCity;
                    }

                    viewModel.SubscriptionPlan = user.Subscriptions.FirstOrDefault() == null
                        ? "No subscription"
                        : user.Subscriptions.First().SubscriptionPlan.Name;
                    viewModel.SubscriptionPlanPrice = user.Subscriptions.FirstOrDefault() == null
                        ? "--"
                        : user.Subscriptions.First().SubscriptionPlan.Price.ToString();
                    viewModel.SubscriptionPlanCurrency = user.Subscriptions.FirstOrDefault() == null
                        ? ""
                        : user.Subscriptions.First().SubscriptionPlan.CurrencyDetails.CurrencySymbol;

                    var sum = user.Invoices.Sum(i => i.Total);
                    viewModel.TotalRevenue = sum != null ? sum.Value / 100 : 0;
                });

            Mapper.CreateMap<Invoice, InvoiceViewModel>()
                .AfterMap((invoice, model) =>
                {
                    model.CurrencySymbol = invoice.CurrencyDetails.CurrencySymbol;
                });
            Mapper.CreateMap<Invoice.LineItem, InvoiceViewModel.LineItem>();
            Mapper.CreateMap<Invoice.Period, InvoiceViewModel.Period>();
            Mapper.CreateMap<Invoice.Plan, InvoiceViewModel.Plan>();

            // Stripe invoice to model
            Mapper.CreateMap<StripeInvoice, Invoice>()
                .ForMember(invoice => invoice.Id, opt => opt.Ignore())
                .AfterMap((stripeInvoice, invoice) =>
                {
                    invoice.StripeId = stripeInvoice.Id;
                    invoice.StripeCustomerId = stripeInvoice.CustomerId;
                    invoice.LineItems = Mapper.Map<List<StripeInvoiceItem>, List<Invoice.LineItem>>(stripeInvoice.StripeInvoiceLines.StripeInvoiceItems);
                });
            Mapper.CreateMap<StripeInvoiceItem, Invoice.LineItem>()
                .ForMember(item => item.Id, opt => opt.Ignore())
                .AfterMap((sLine, line) => line.StripeLineItemId = sLine.Id);
            Mapper.CreateMap<StripePeriod, Invoice.Period>();
            Mapper.CreateMap<StripePlan, Invoice.Plan>()
                .AfterMap((sPlan, plan) =>
                {
                    plan.AmountInCents = sPlan.Amount;
                    plan.StripePlanId = sPlan.Id;
                });
        }
    }
}
