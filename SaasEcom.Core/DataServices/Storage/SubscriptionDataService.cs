using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Core.DataServices.Interfaces;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.DataServices.Storage
{
    /// <summary>
    /// Implementation for CRUD related to subscriptions in the database.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    public class SubscriptionDataService<TContext, TUser> : ISubscriptionDataService
        where TContext : IDbContext<TUser>
        where TUser : class
    {
        private readonly TContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionDataService{TContext, TUser}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SubscriptionDataService(TContext context)
        {
            this._dbContext = context;
        }

        /// <summary>
        /// Subscribes the user asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <param name="trialPeriodInDays">The trial period in days.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns>
        /// The subscription
        /// </returns>
        public async Task<Subscription> SubscribeUserAsync(SaasEcomUser user, string planId, int? trialPeriodInDays = null, decimal taxPercent = 0)
        {
            var plan = await _dbContext.SubscriptionPlans.FirstOrDefaultAsync(x => x.Id == planId);

            if (plan == null)
            {
                throw new ArgumentException(string.Format("There's no plan with Id: {0}", planId));
            }

            var s = new Subscription
            {
                Start = DateTime.UtcNow,
                End = null,
                TrialEnd = DateTime.UtcNow.AddDays(trialPeriodInDays ?? plan.TrialPeriodInDays),
                TrialStart = DateTime.UtcNow,
                UserId = user.Id,
                SubscriptionPlan = plan,
                Status = trialPeriodInDays == null ? "active" : "trialing",
                TaxPercent = taxPercent
            };

            _dbContext.Subscriptions.Add(s);
            await _dbContext.SaveChangesAsync();

            return s;
        }

        /// <summary>
        /// Get the User's active subscription asynchronous. Only the first (valid if your customers can have only 1 subscription at a time).
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// The subscription
        /// </returns>
        public async Task<Subscription> UserActiveSubscriptionAsync(string userId)
        {
            return (await UserActiveSubscriptionsAsync(userId)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the User's subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<List<Subscription>> UserSubscriptionsAsync(string userId)
        {
            return await _dbContext.Subscriptions.Where(s => s.User.Id == userId).Select(s => s).ToListAsync();
        }

        /// <summary>
        /// Get the User's active subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// The list of Subscriptions
        /// </returns>
        public async Task<List<Subscription>> UserActiveSubscriptionsAsync(string userId)
        {
            return await _dbContext.Subscriptions
                .Where(s => s.User.Id == userId && s.Status != "canceled" && s.Status != "unpaid")
                .Where(s => s.End == null || s.End > DateTime.UtcNow)
                .Include(s => s.SubscriptionPlan.Properties)
                .Select(s => s).ToListAsync();
        }

        /// <summary>
        /// Ends the subscription asynchronous.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="subscriptionEnDateTime">The subscription en date time.</param>
        /// <param name="reasonToCancel">The reason to cancel.</param>
        /// <returns></returns>
        public async Task EndSubscriptionAsync(int subscriptionId, DateTime subscriptionEnDateTime, string reasonToCancel)
        {
            var dbSub = await _dbContext.Subscriptions.FindAsync(subscriptionId);
            dbSub.End = subscriptionEnDateTime;
            dbSub.ReasonToCancel = reasonToCancel;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the subscription asynchronous.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <returns></returns>
        public async Task UpdateSubscriptionAsync(Subscription subscription)
        {
            _dbContext.Entry(subscription).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Updates the subscription tax.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task UpdateSubscriptionTax(string subscriptionId, decimal taxPercent)
        {
            var subscription = await _dbContext.Subscriptions.Where(s => s.StripeId == subscriptionId).FirstOrDefaultAsync();
            subscription.TaxPercent = taxPercent;
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Deletes the subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task DeleteSubscriptionsAsync(string userId)
        {
            foreach (var subscription in _dbContext.Subscriptions.Where(s => s.UserId == userId).Select(s =>s))
            {
                _dbContext.Subscriptions.Remove(subscription);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
