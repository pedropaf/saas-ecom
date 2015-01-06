using System;
using System.Collections.Generic;
using System.Linq;
using SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces;
using SaasEcom.Core.Models;
using Stripe;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Stripe
{
    /// <summary>
    /// 
    /// </summary>
    public class SubscriptionPlanProvider : ISubscriptionPlanProvider
    {
        private readonly string _apiKey;

        private StripePlanService _planService;
        private StripePlanService PlanService
        {
            get { return _planService ?? (_planService = new StripePlanService(_apiKey)); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionPlanProvider"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public SubscriptionPlanProvider(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// Adds the specified plan.
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <returns></returns>
        public object Add(SubscriptionPlan plan)
        {
            var result = PlanService.Create(new StripePlanCreateOptions
            {
                Id = plan.Id,
                Name = plan.Name,
                Amount = (int)Math.Round(plan.Price * 100),
                Currency = plan.Currency,
                Interval = GetInterval(plan.Interval),
                TrialPeriodDays = plan.TrialPeriodInDays,
                IntervalCount = 1, // The number of intervals (specified in the interval property) between each subscription billing. For example, interval=month and interval_count=3 bills every 3 months.
            });

            return result;
        }

        /// <summary>
        /// Updates the specified plan.
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <returns></returns>
        public object Update(SubscriptionPlan plan)
        {
            var res = PlanService.Update(plan.Id, new StripePlanUpdateOptions
            {
                Name = plan.Name
            });

            return res;
        }

        /// <summary>
        /// Deletes the specified plan identifier.
        /// </summary>
        /// <param name="planId">The plan identifier.</param>
        public void Delete(string planId)
        {
            PlanService.Delete(planId);
        }

        /// <summary>
        /// Finds the subscription plan by Id asynchronous.
        /// </summary>
        /// <param name="planId">The plan identifier.</param>
        /// <returns>Stripe plan</returns>
        public SubscriptionPlan FindAsync(string planId)
        {
            try
            {
                var stripePlan = PlanService.Get(planId);

                return SubscriptionPlanMapper(stripePlan);
            }
            catch (StripeException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all subscription plans asynchronous.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public IEnumerable<SubscriptionPlan> GetAllAsync(object options)
        {
            var result = PlanService.List((StripeListOptions)options);

            return result.Select(SubscriptionPlanMapper);
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

        private static SubscriptionPlan.SubscriptionInterval GetInterval(string interval)
        {
            switch (interval)
            {
                case ("month"):
                    return SubscriptionPlan.SubscriptionInterval.Monthly;
                case ("year"):
                    return SubscriptionPlan.SubscriptionInterval.Yearly;
                case ("week"):
                    return SubscriptionPlan.SubscriptionInterval.Weekly;
                case ("3-month"):
                    return SubscriptionPlan.SubscriptionInterval.EveryThreeMonths;
                case ("6-month"):
                    return SubscriptionPlan.SubscriptionInterval.EverySixMonths;
            }

            return 0;
        }

        private static SubscriptionPlan SubscriptionPlanMapper(StripePlan stripePlan)
        {
            return new SubscriptionPlan
            {
                Id = stripePlan.Id,
                Name = stripePlan.Name,
                Currency = stripePlan.Currency,
                Interval = GetInterval(stripePlan.Interval),
                Price = stripePlan.Amount,
                TrialPeriodInDays = stripePlan.TrialPeriodDays ?? 0,

            };
        }
    }
}
