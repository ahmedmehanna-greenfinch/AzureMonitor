using System;
using System.Linq;
using System.Threading.Tasks;
using AzureMonitor.ConfigModels;
using AzureMonitor.Responses;
using AzureMonitor.Services;

namespace AzureMonitor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RunAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static async Task RunAsync()
        {
            AppConfig config = AppConfig.ReadFromJsonFile("appsettings.json");

            var app = AuthenticationService.GetConfidentialClientApplication(config);

            var alerts = await AlertsService.GetAlerts(app, config);
            var recommendations = await AdvisorRecommendationsService.GetAdvisorRecommendations(app, config);
            var securityRecommendations = recommendations.Recommendations
                .Where(r => r.Properties.Category == Category.Security).ToList();
        }
    }
}
