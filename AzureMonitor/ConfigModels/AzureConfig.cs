using System.Globalization;

namespace AzureMonitor.ConfigModels
{
    public class AzureConfig
    {
        public string Instance { get; set; } = "https://login.windows.net/{0}";

        public string Tenant { get; set; }

        public string ClientId { get; set; }

        public string SubscriptionId { get; set; }

        public string Authority => string.Format(CultureInfo.InvariantCulture, Instance, Tenant);

        public string ClientSecret { get; set; }

        public string CertificateName { get; set; }
    }
}
