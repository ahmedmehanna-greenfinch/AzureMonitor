using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.IO;

namespace AzureMonitor.ConfigModels
{
    public class AppConfig
    {
        public string Instance { get; set; } = "https://login.windows.net/{0}";

        public string Tenant { get; set; }

        public string ClientId { get; set; }

        public string SubscriptionId { get; set; }

        public string Authority => string.Format(CultureInfo.InvariantCulture, Instance, Tenant);

        public AlertsConfig AlertsConfig { get; set; }

        public RecommendationsConfig RecommendationsConfig { get; set; }

        public string ClientSecret { get; set; }

        public string CertificateName { get; set; }

        public static AppConfig ReadFromJsonFile(string path)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path);

            var configuration = builder.Build();
            return configuration.Get<AppConfig>();
        }
    }
}
