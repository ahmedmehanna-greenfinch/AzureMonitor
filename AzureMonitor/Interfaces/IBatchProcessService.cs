using System.Collections.Generic;
using System.Threading.Tasks;
using AzureMonitor.Models.Jira;

namespace AzureMonitor.Interfaces
{
    public interface IBatchProcessService
    {
        Task<List<JiraIssue>> ProcessEpics(string projectId = null);

        Task<List<JiraIssue>> ProcessIssuesUnderEpic(string epicKey, string projectId = null);

        Task<List<JiraIssue>> ProcessIssuesWithoutEpic(string projectId = null);
    }
}
