using SaasEcom.Core.Models;

namespace SaasEcom.Core.Infrastructure.PaymentProcessor.Interfaces
{
    public interface ISubscriptionPlanProvider
    {
        object Add(SubscriptionPlan plan);
        object Update(SubscriptionPlan plan);
        void Delete(string planId);
    }
}
