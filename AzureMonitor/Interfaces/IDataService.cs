using System.Collections.Generic;
using System.Threading.Tasks;
using AzureMonitor.Models.Jira;

namespace AzureMonitor.Interfaces
{
    public interface IDataService
    {
        Task<int> SaveProjects(IEnumerable<JiraProject> projects);
        Task<int> SaveIssues(IEnumerable<JiraIssue> issues);
        Task<int> SaveWorklogs(IEnumerable<JiraIssue> issues);
        Task<int> SaveUsers(IEnumerable<JiraUser> users);
    }
}
