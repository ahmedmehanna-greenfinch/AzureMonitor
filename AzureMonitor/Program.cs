using System;
using System.Threading.Tasks;
using AzureMonitor.Interfaces;
using AzureMonitor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AzureMonitor
{
    internal class Program
    {
        public static ServiceProvider ServiceProvider;
        public static ILogger<Program> Log;

        static void Main(string[] args)
        {
            try
            {
                //setup our DI
                ServiceProvider = new ServiceCollection()
                    .AddLogging()
                    .AddSingleton<IAuthenticationService, AuthenticationService>()
                    .AddSingleton<IAppConfigService, AppConfigService>()
                    .AddSingleton<IAlertsService, AlertsService>()
                    .AddSingleton<IAdvisorRecommendationsService, AdvisorRecommendationsService>()
                    .AddSingleton<IJiraService, JiraService>()
                    .AddSingleton<IBatchProcessService, BatchProcessService>()
                    .BuildServiceProvider();

                Log = ServiceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

                Log.LogDebug("Starting application");

                RunAsync().GetAwaiter().GetResult();

                Log.LogDebug("All done!");
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
            //var alertService = ServiceProvider.GetService<IAlertsService>();
            //var recommendationsService = ServiceProvider.GetService<IAdvisorRecommendationsService>();
            var batchProcessService = ServiceProvider.GetService<IBatchProcessService>();

            //var alerts = await alertService.GetAlerts();
            //var securityRecommendations = await recommendationsService.GetSecurityRecommendations();

            var projectId = "Transom (Mainstay)";
            var epicKey = "MAIN-5733";

            //var epices = await batchProcessService.ProcessEpics(projectId);

            var issues = await batchProcessService.ProcessIssuesUnderEpic(epicKey, projectId);

            //var issuesWithoutEpic = await batchProcessService.ProcessIssuesWithoutEpic(projectId);

        }

    }
}
