using System;
using Nest;

namespace SaasEcom.Data.Infrastructure.Analytics.Stripe
{
    // UI Idea! Filter account / Filter everything by subscription plan!

    public class StripeSearcher
    {
        private ElasticClient _elasticsearchClient;
       
        public StripeSearcher(string accountId)
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            _elasticsearchClient = new ElasticClient(settings);
            AccountId = accountId;
        }

        public string AccountId { get; set; }

        public object GetCustomers()
        {
            throw new NotImplementedException();
        }

        public object GetCustomers(string planId)
        {
            throw new NotImplementedException();
        }

        public object GetActivePlans()
        {
            throw new NotImplementedException();
        }

        public object GetMaintenanceModePlans()
        {
            throw new NotImplementedException();
        }

        public object GetAllPlans()
        {
            throw new NotImplementedException();
        }

        public object GetRevenue()
        {
            // Calculate revenue based on invoices (paid)
            throw new NotImplementedException();
        }

        public object GetRevenue(string planId)
        {
            throw new NotImplementedException();
        }

        // Get Plan Upgrades / Downgrades
    }
}
