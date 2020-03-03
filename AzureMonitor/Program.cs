using System;
using System.Threading.Tasks;
using AzureMonitor.Services;

namespace AzureMonitor
{
    class Program
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
        }

        /// <summary>
        /// Display the result of the Web API call
        /// </summary>
        /// <param name="result">Object to display</param>
        //private static void Display(JObject result)
        //{
        //    foreach (JProperty child in result.Properties().Where(p => !p.Name.StartsWith("@")))
        //    {
        //        Console.WriteLine($"{child.Name} = {child.Value}");
        //    }
        //}
    }
}
