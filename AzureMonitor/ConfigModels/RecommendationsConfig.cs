using System.Collections.Generic;
using System.Linq;

namespace AzureMonitor.ConfigModels
{
    public class RecommendationsConfig
    {
        public string Endpoint { get; set; }
        public string[] Scopes { get; set; }

        public RecommendationsFilters RecommendationsFilters { get; set; }

        public string GetFullUrl(string subscriptionId)
        {
            var url = string.Empty;
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(Endpoint))
            {
                var queryString = string.Empty;

                if (!string.IsNullOrEmpty(RecommendationsFilters.ApiVersion))
                {
                    parameters.Add("api-version", RecommendationsFilters.ApiVersion);
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

    public class RecommendationsFilters
    {
        public string ApiVersion { get; set; }
    }

}
