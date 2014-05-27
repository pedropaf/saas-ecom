using System.Collections.Generic;
using System.Threading.Tasks;
using SaasEcom.Data.Models;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Interfaces
{
    public interface ISubscriptionProvider
    {
        Task SubscribeUserAsync(ApplicationUser user, string planId, int trialInDays = 0);
        Task<List<Subscription>> UserSubscriptionsAsync(string userId);
        Task<bool> EndSubscriptionAsync(int subscriptionId, ApplicationUser user, bool cancelAtPeriodEnd = false);
    }
}
