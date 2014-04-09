using System;
using System.Collections.Generic;
using System.Linq;
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

        public void SubscribeUser(ApplicationUser user, string planId)
        {
            var plan = _dbContext.SubscriptionPlans.First(x => x.FriendlyId == planId);

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
            _dbContext.SaveChanges();
        }

        public List<Subscription> UserSubscriptions(string name)
        {
            return _dbContext.Subscriptions.Where(s => s.User.UserName == name).Select(s => s).ToList();
        }
    }
}
