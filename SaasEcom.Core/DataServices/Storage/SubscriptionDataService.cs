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
        /// <returns>
        /// The subscription
        /// </returns>
        public async Task<Subscription> SubscribeUserAsync(SaasEcomUser user, string planId, int? trialPeriodInDays = null)
        {
            var plan = await _dbContext.SubscriptionPlans.FirstAsync(x => x.Id == planId);

            var s = new Subscription
            {
                Start = DateTime.UtcNow,
                End = null,
                TrialEnd = DateTime.UtcNow.AddDays(trialPeriodInDays ?? plan.TrialPeriodInDays),
                TrialStart = DateTime.UtcNow,
                UserId = user.Id,
                SubscriptionPlan = plan,
                Status = trialPeriodInDays == null ? "active" : "trialing"
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
            return await
                _dbContext.Subscriptions
                    .Include(s => s.SubscriptionPlan)
                    .Where(s => s.User.Id == userId && s.End == null)
                    .FirstOrDefaultAsync();
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
                .Select(s => s).ToListAsync();
        }

        /// <summary>
        /// Ends the subscription asynchronous.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <returns></returns>
        public async Task EndSubscriptionAsync(int subscriptionId)
        {
            var dbSub = await _dbContext.Subscriptions.FindAsync(subscriptionId);
            dbSub.End = DateTime.UtcNow;
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
    }
}
