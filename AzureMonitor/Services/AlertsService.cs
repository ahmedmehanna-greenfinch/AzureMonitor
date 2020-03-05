using System.Net.Http;
using System.Threading.Tasks;
using AzureMonitor.ConfigModels;
using Microsoft.Identity.Client;
using AzureMonitor.Helper;
using AzureMonitor.Responses;
using Newtonsoft.Json;

namespace AzureMonitor.Services
{
    public static class AlertsService
    {
        public static async Task<AlertsResponse> GetAlerts(IConfidentialClientApplication app, AppConfig config)
        {
            var result = await AuthenticationService.AcquireTokenForClient(app, config.AlertsConfig.Scopes);

            if (result != null)
            {
                var httpClient = new HttpClient();
                var apiCaller = new ProtectedApiCallHelper(httpClient);
                var url = config.AlertsConfig.GetFullUrl(config.AzureConfig.SubscriptionId);
                var jsonResponse = await apiCaller.CallWebApiAndProcessResultASync(url, result.AccessToken);

                if (jsonResponse != null)
                {
                    var alerts = JsonConvert.DeserializeObject<AlertsResponse>(jsonResponse);

                    return alerts;
                }
            }

            return null;
        }
    }
}
