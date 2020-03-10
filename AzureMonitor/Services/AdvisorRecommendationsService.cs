using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using AzureMonitor.Helper;
using AzureMonitor.Interfaces;
using AzureMonitor.Responses;
using Newtonsoft.Json;

namespace AzureMonitor.Services
{
    public class AdvisorRecommendationsService: IAdvisorRecommendationsService
    {
        private readonly IAppConfigService _appConfigService;
        private readonly IConfidentialClientApplication _app;

        public AdvisorRecommendationsService(IAuthenticationService authenticationService, IAppConfigService appConfigService)
        {
            _appConfigService = appConfigService;
            _app = authenticationService.GetAzureConfidentialClientApplication();
        }

        public async Task<RecommendationsResponse> GetAllRecommendations()
        {
            var result = await AuthenticationService.AcquireAzureTokenForClient(_app, _appConfigService.Config.RecommendationsConfig.Scopes);

            if (result != null)
            {
                var httpClient = new HttpClient();
                var apiCaller = new ProtectedApiCallHelper(httpClient);
                var url = _appConfigService.Config.RecommendationsConfig.GetFullUrl(_appConfigService.Config.AzureConfig.SubscriptionId);
                var jsonResponse = await apiCaller.CallWebApiAndProcessResultASync(url, result.AccessToken);

                if (jsonResponse != null)
                {
                    var recommendations = JsonConvert.DeserializeObject<RecommendationsResponse>(jsonResponse);

                    return recommendations;
                }
            }

            return null;
        }

        public async Task<List<Recommendations>> GetSecurityRecommendations()
        {
            var recommendations = await GetAllRecommendations();

            var securityRecommendations = recommendations.Recommendations
                .Where(r => r.Properties.Category == Category.Security).ToList();

            return securityRecommendations;
        }
    }
}
