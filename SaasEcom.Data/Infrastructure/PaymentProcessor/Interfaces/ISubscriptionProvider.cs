using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Interfaces
{
    public interface ISubscriptionProvider
    {
        void SubscribeUser(ApplicationUser user, string planId, int trialInDays = 0);
        Task<List<Subscription>> UserSubscriptionsAsync(string userId);
        void EndSubscription(string userStripeId, string subStripeId, bool cancelAtPeriodEnd = false);
    }
}
