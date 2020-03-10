using System.Collections.Generic;
using System.Threading.Tasks;
using AzureMonitor.Models.Jira;
using AzureMonitor.Utils;

namespace AzureMonitor.Interfaces
{
    public interface IJiraService
    {
        Task<JiraBatchDataHeader> GetEpicIssuesHeader(string projectId = null);

        Task<JiraBatchDataHeader> GetIssuesUnderEpicHeader(string epicKey, string projectId = null);

        Task<JiraBatchDataHeader> GetIssuesWithoutEpicHeader(string projectId = null);

        Task<IEnumerable<JiraIssue>> GetEpicIssuesBatch(JiraBatchDataHeader jiraBatchDataHeader);
    }
}
