using System.Collections.Generic;
using System.Linq;

namespace AzureMonitor.ConfigModels
{
    public class AlertsConfig
    {
        public string Endpoint { get; set; }

        // With client credentials flows the scopes is ALWAYS of the shape "resource/.default", as the 
        // application permissions need to be set statically (in the portal or by PowerShell), and then granted by
        // a tenant administrator. The Graph endpoint may have to be changed for national cloud scenarios, refer to
        // https://docs.microsoft.com/graph/deployments#microsoft-graph-and-graph-explorer-service-root-endpoints
        public string[] Scopes { get; set; }

        public AlertsFilters AlertsFilters { get; set; }

        public string GetFullUrl(string subscriptionId)
        {
            var url = string.Empty;
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(Endpoint))
            {
                var queryString = string.Empty;

                if (!string.IsNullOrEmpty(AlertsFilters.ApiVersion))
                {
                    parameters.Add("api-version", AlertsFilters.ApiVersion);
                }

                if (!string.IsNullOrEmpty(AlertsFilters.TimeRange))
                {
                    parameters.Add("timeRange", AlertsFilters.TimeRange);
                }
                else if (string.IsNullOrEmpty(AlertsFilters.TimeRange) && AlertsFilters.CustomTimeRange.HasValue)
                {
                    parameters.Add("customTimeRange", AlertsFilters.CustomTimeRange.GetParameterValue);
                }

                if (AlertsFilters.PageCount > 0)
                {
                    parameters.Add("pageCount", AlertsFilters.PageCount);
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

    public class AlertsFilters
    {
        public string ApiVersion { get; set; }
        public string TimeRange { get; set; }
        public int PageCount { get; set; }
        public CustomTimeRange CustomTimeRange { get; set; }
    }

    public class CustomTimeRange
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public bool HasValue => !string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime);
        public string GetParameterValue => $"{StartTime}/{EndTime}";
    }
}
