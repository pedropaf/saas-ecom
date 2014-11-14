﻿using System;
using System.Collections.Generic;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Core.Models;
using Stripe;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe
{
    public class SubscriptionPlanProvider : ISubscriptionPlanProvider
    {
        private readonly string _apiKey;

        private StripePlanService _planService;

        public SubscriptionPlanProvider(string apiKey)
        {
            _apiKey = apiKey;
        }

        private StripePlanService PlanService
        {
            get { return _planService ?? (_planService = new StripePlanService(_apiKey)); }
        }

        public IEnumerable<StripePlan> GetAllAsync(StripeListOptions options)
        {
            return PlanService.List(options);
        }

        public StripePlan FindAsync(string planId)
        {
            try
            {
                return PlanService.Get(planId);
            }
            catch (StripeException ex)
            {
                return null;
            }
        }

        public object Add(SubscriptionPlan plan)
        {
            var result = PlanService.Create(new StripePlanCreateOptions
            {
                Id = plan.FriendlyId,
                Name = plan.Name,
                Amount = (int)Math.Round(plan.Price * 100),
                Currency = plan.Currency,
                Interval = GetInterval(plan.Interval),
                TrialPeriodDays = plan.TrialPeriodInDays,
                IntervalCount = 1, // The number of intervals (specified in the interval property) between each subscription billing. For example, interval=month and interval_count=3 bills every 3 months.
            });

            return result;
        }

        public object Update(SubscriptionPlan plan)
        {
            var res = PlanService.Update(plan.FriendlyId, new StripePlanUpdateOptions
            {
                Name = plan.Name
            });

            return res;
        }

        public void Delete(string planId)
        {
            PlanService.Delete(planId);
        }

        public StripePlan GetSubscriptionPlan(string planId)
        {
            try
            {
                return PlanService.Get(planId);
            }
            catch (StripeException ex)
            {
                return null;
            }
        }

        public IEnumerable<StripePlan> GetAllPlans(StripeListOptions options)
        {
            return PlanService.List(options);
        }

        private static string GetInterval(SubscriptionPlan.SubscriptionInterval interval)
        {
            string result = null;

            switch (interval)
            {
                case (SubscriptionPlan.SubscriptionInterval.Monthly):
                    result = "month";
                    break;
                case (SubscriptionPlan.SubscriptionInterval.Yearly):
                    result = "year";
                    break;
                case (SubscriptionPlan.SubscriptionInterval.Weekly):
                    result = "week";
                    break;
                case (SubscriptionPlan.SubscriptionInterval.EveryThreeMonths):
                    result = "3-month";
                    break;
                case (SubscriptionPlan.SubscriptionInterval.EverySixMonths):
                    result = "6-month";
                    break;
            }

            return result;
        }
    }
}