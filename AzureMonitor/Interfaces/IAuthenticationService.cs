using Microsoft.Identity.Client;

namespace AzureMonitor.Interfaces
{
    public interface IAuthenticationService
    {
        IConfidentialClientApplication GetAzureConfidentialClientApplication();
    }
}
