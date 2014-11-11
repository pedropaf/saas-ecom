using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SaasEcom.Core.Models;
using Stripe;
using $rootnamespace$.Controllers;

namespace $rootnamespace$.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
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