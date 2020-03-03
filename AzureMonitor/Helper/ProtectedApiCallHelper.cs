using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureMonitor.Helper
{
    public class ProtectedApiCallHelper
    {
        public ProtectedApiCallHelper(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected HttpClient HttpClient { get; private set; }

        public async Task<string> CallWebApiAndProcessResultASync(string webApiUrl, string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                var defaultRequetHeaders = HttpClient.DefaultRequestHeaders;
                if (defaultRequetHeaders.Accept == null || defaultRequetHeaders.Accept.All(m => m.MediaType != "application/json"))
                {
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                defaultRequetHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                var response = await HttpClient.GetAsync(webApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return json;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to call the Web Api: {response.StatusCode}");
                    string content = await response.Content.ReadAsStringAsync();

                    // Note that if you got reponse.Code == 403 and reponse.content.code == "Authorization_RequestDenied"
                    // this is because the tenant admin as not granted consent for the application to call the Web API
                    Console.WriteLine($"Content: {content}");
                }
                Console.ResetColor();
            }

            return null;
        }
    }
}
