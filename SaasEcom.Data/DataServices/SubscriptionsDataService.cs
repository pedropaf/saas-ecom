using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.DataServices
{
    public class SubscriptionsDataService
    {
        private readonly ApplicationDbContext _dbContext;

        public SubscriptionsDataService(ApplicationDbContext context)
        {
            this._dbContext = context;
        }

        public async Task<int> SubscribeUserAsync(ApplicationUser user, string planId)
        {
            var plan = await _dbContext.SubscriptionPlans.FirstAsync(x => x.FriendlyId == planId);

            var s = new Subscription
            {
                Start = DateTime.UtcNow,
                End = null,
                TrialEnd = DateTime.UtcNow.AddDays(30),
                TrialStart = DateTime.UtcNow,
                User = user,
                SubscriptionPlan = plan
            };

            _dbContext.Subscriptions.Add(s);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Subscription>> UserSubscriptionsAsync(string userId)
        {
            return await _dbContext.Subscriptions.Where(s => s.User.Id == userId).Select(s => s).ToListAsync();
        }

        public async Task EndSubscriptionAsync(int subscriptionId)
        {
            var dbSub = await _dbContext.Subscriptions.FindAsync(subscriptionId);
            dbSub.End = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        public bool SubscriptionBelongsToUser(string userId, int subscriptionId)
        {
            var user = _dbContext.Users.Find(userId);
            return user.Subscriptions.Any(s => s.Id == subscriptionId);
        }
    }
}
