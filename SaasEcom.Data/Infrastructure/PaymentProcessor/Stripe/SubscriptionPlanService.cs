using System;
using System.Collections.Generic;
using SaasEcom.Data.Models;
using Stripe;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Stripe
{
    public class SubscriptionPlanService
    {
        private readonly string _apiKey;

        private StripePlanService _planService;

        public SubscriptionPlanService(string apiKey)
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

        public StripePlan Add(Models.SubscriptionPlan plan)
        {
            var result = PlanService.Create(new StripePlanCreateOptions
            {
                Id = plan.FriendlyId,
                Name = plan.Name,
                Amount = (int)Math.Round(plan.Price * 100),
                Currency = "GBP",
                Interval = GetInterval(plan.Interval),
                TrialPeriodDays = plan.TrialPeriodInDays,
                IntervalCount = 1, // The number of intervals (specified in the interval property) between each subscription billing. For example, interval=month and interval_count=3 bills every 3 months.
            });

            return result;
        }

        public StripePlan Update(SubscriptionPlan plan)
        {
            var res = PlanService.Update(plan.FriendlyId, new StripePlanUpdateOptions
            {
                Name = plan.Name
            });

            return res;
        }

        public void DeleteAsync(string planId)
        {
            PlanService.Delete(planId);
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
