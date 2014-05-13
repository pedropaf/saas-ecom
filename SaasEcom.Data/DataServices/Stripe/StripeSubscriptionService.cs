using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaasEcom.Data.DataServices.Interfaces;

namespace SaasEcom.Data.DataServices.Stripe
{
    public class StripeSubscriptionService : ISubscriptionService
    {
        public Task<int> SubscribeUserAsync(Models.ApplicationUser user, string planId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Models.Subscription>> UserSubscriptionsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task EndSubscriptionAsync(int subscriptionId)
        {
            throw new NotImplementedException();
        }

        public bool SubscriptionBelongsToUser(string userId, int subscriptionId)
        {
            throw new NotImplementedException();
        }
    }
}
