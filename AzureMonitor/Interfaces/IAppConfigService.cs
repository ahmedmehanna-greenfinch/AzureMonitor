using AzureMonitor.ConfigModels;

namespace AzureMonitor.Interfaces
{
    public interface IAppConfigService
    {
        AppConfig Config { get; }
    }
}
