using System;
using Nest;
using Stripe;

namespace SaasEcom.Data.Infrastructure.Analytics.Stripe
{
    public class StripeIndexer
    {
        private ElasticClient _elasticsearchClient;

        public StripeIndexer()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node, defaultIndex: "my-application");
            _elasticsearchClient = new ElasticClient(settings);    
        }
        
        internal void IndexEvent(StripeCharge charge)
        {
            throw new NotImplementedException();
        }
    }
}
