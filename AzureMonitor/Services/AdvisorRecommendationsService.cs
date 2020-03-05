using System.Net.Http;
using System.Threading.Tasks;
using AzureMonitor.ConfigModels;
using Microsoft.Identity.Client;
using AzureMonitor.Helper;
using AzureMonitor.Responses;
using Newtonsoft.Json;

namespace AzureMonitor.Services
{
    public static class AdvisorRecommendationsService
    {

        public static async Task<RecommendationsResponse> GetAdvisorRecommendations(IConfidentialClientApplication app, AppConfig config)
        {

            var result = await AuthenticationService.AcquireTokenForClient(app, config.RecommendationsConfig.Scopes);

            if (result != null)
            {
                var httpClient = new HttpClient();
                var apiCaller = new ProtectedApiCallHelper(httpClient);
                var url = config.RecommendationsConfig.GetFullUrl(config.AzureConfig.SubscriptionId);
                var jsonResponse = await apiCaller.CallWebApiAndProcessResultASync(url, result.AccessToken);

                if (jsonResponse != null)
                {
                    var recommendations = JsonConvert.DeserializeObject<RecommendationsResponse>(jsonResponse);

                    return recommendations;
                }
            }

            return null;
        }
    }
}
