using SaasEcom.Data.Models;

namespace SaasEcom.Data.Infrastructure.PaymentProcessor.Interfaces
{
    public interface ISubscriptionPlanProvider
    {
        object Add(SubscriptionPlan plan);
        object Update(SubscriptionPlan plan);
        void Delete(string planId);
    }
}
