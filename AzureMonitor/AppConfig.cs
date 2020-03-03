using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AzureMonitor
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

    public class AlertsConfig
    {
        public string Endpoint { get; set; }

        // With client credentials flows the scopes is ALWAYS of the shape "resource/.default", as the 
        // application permissions need to be set statically (in the portal or by PowerShell), and then granted by
        // a tenant administrator. The Graph endpoint may have to be changed for national cloud scenarios, refer to
        // https://docs.microsoft.com/graph/deployments#microsoft-graph-and-graph-explorer-service-root-endpoints
        public string[] Scopes { get; set; }

        public string ApiVersion { get; set; }
        public string TimeRange { get; set; }
        public int PageCount { get; set; }

        public string GetFullUrl(string subscriptionId)
        {
            var url = string.Empty;
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(Endpoint))
            {
                var queryString = string.Empty;

                if (!string.IsNullOrEmpty(ApiVersion))
                {
                    parameters.Add("api-version", this.ApiVersion);
                }

                if (!string.IsNullOrEmpty(TimeRange))
                {
                    parameters.Add("timeRange", this.TimeRange);
                }

                if (PageCount > 0)
                {
                    parameters.Add("pageCount", this.PageCount);
                }

                if (parameters.Count > 0)
                {
                    queryString = $"?{string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";
                }
                
                url = $"{string.Format(this.Endpoint, subscriptionId)}{queryString}";
            }

            return url;
        }
    }

    public class RecommendationsConfig
    {
        public string Endpoint { get; set; }
        public string[] Scopes { get; set; }
        public string ApiVersion { get; set; }

        public string GetFullUrl(string subscriptionId)
        {
            var url = string.Empty;
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(Endpoint))
            {
                var queryString = string.Empty;

                if (!string.IsNullOrEmpty(ApiVersion))
                {
                    parameters.Add("api-version", this.ApiVersion);
                }

                if (parameters.Count > 0)
                {
                    queryString = $"?{string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";
                }

                url = $"{string.Format(this.Endpoint, subscriptionId)}{queryString}";
            }

            return url;
        }
    }
}
