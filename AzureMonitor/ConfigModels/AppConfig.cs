namespace AzureMonitor.ConfigModels
{
    public class AppConfig
    {
        public AzureConfig AzureConfig { get; set; }

        public JiraConfig JiraConfig { get; set; }

        public AlertsConfig AlertsConfig { get; set; }

        public RecommendationsConfig RecommendationsConfig { get; set; }
    }
}
