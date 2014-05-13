using SaasEcom.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaasEcom.Data.DataServices.Interfaces
{
    public interface ISubscriptionService
    {
        Task<int> SubscribeUserAsync(ApplicationUser user, string planId);
        Task<List<Subscription>> UserSubscriptionsAsync(string userId);
        Task EndSubscriptionAsync(int subscriptionId);
        bool SubscriptionBelongsToUser(string userId, int subscriptionId);
    }
}
