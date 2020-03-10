using System.IO;
using AzureMonitor.ConfigModels;
using AzureMonitor.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AzureMonitor.Services
{
    public class AppConfigService: IAppConfigService
    {
        public AppConfig Config { get;}

        public AppConfigService()
        {
            Config = ReadFromJsonFile("appsettings.json");
        }

        private AppConfig ReadFromJsonFile(string path)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path);

            var configuration = builder.Build();
            return configuration.Get<AppConfig>();
        }
    }
}
