using System.Threading.Tasks;
using AzureMonitor.Responses;

namespace AzureMonitor.Interfaces
{
    public interface IAlertsService
    {
        Task<AlertsResponse> GetAlerts();
    }
}
