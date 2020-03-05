using Microsoft.Extensions.Configuration;
using System.IO;

namespace AzureMonitor.ConfigModels
{
    public class AppConfig
    {
        public AzureConfig AzureConfig { get; set; }

        public AlertsConfig AlertsConfig { get; set; }

        public RecommendationsConfig RecommendationsConfig { get; set; }

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
