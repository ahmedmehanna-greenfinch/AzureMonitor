using System.Collections.Generic;
using System.Threading.Tasks;
using AzureMonitor.Responses;

namespace AzureMonitor.Interfaces
{
    public interface IAdvisorRecommendationsService
    {
        Task<RecommendationsResponse> GetAllRecommendations();
        Task<List<Recommendations>> GetSecurityRecommendations();
    }
}
