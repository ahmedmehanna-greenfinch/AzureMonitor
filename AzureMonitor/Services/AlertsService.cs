using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using AzureMonitor.Helper;
using AzureMonitor.Interfaces;
using AzureMonitor.Responses;
using Newtonsoft.Json;

namespace AzureMonitor.Services
{
    public class AlertsService: IAlertsService
    {
        private readonly IAppConfigService _appConfigService;
        private readonly IConfidentialClientApplication _app;

        public AlertsService(IAuthenticationService authenticationService, IAppConfigService appConfigService)
        {
            _appConfigService = appConfigService;
            _app = authenticationService.GetAzureConfidentialClientApplication();
        }

        public async Task<AlertsResponse> GetAlerts()
        {
            var result = await AuthenticationService.AcquireAzureTokenForClient(_app, _appConfigService.Config.AlertsConfig.Scopes);

            if (result != null)
            {
                var httpClient = new HttpClient();
                var apiCaller = new ProtectedApiCallHelper(httpClient);
                var url = _appConfigService.Config.AlertsConfig.GetFullUrl(_appConfigService.Config.AzureConfig.SubscriptionId);
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
