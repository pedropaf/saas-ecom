using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Core.Models;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    public interface ISubscriptionProvider
    {
        void SubscribeUser(SaasEcomUser user, string planId, int trialInDays = 0);
        Task<List<Subscription>> UserSubscriptionsAsync(string userId);
        void EndSubscription(string userStripeId, string subStripeId, bool cancelAtPeriodEnd = false);
    }
}
