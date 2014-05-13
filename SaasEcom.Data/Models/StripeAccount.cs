namespace SaasEcom.Data.Models
{
    public class StripeAccount
    {
        public int Id { get; set; }

        public bool LiveMode { get; set; }

        public string StripeLivePublicApiKey { get; set; }
        public string StripeLiveSecretApiKey { get; set; }

        public string StripeTestPublicApiKey { get; set; }
        public string StripeTestSecretApiKey { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
