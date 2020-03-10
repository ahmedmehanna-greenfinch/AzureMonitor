using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlassian.Jira;
using AzureMonitor.Interfaces;
using AzureMonitor.Models.Jira;
using AzureMonitor.Utils;

namespace AzureMonitor.Services
{
    public class JiraService : IJiraService
    {
        private static Jira _jira;

        public JiraService(IAppConfigService appConfigService)
        {
            var appConfigService1 = appConfigService;

            _jira = Jira.CreateRestClient(appConfigService1.Config.JiraConfig.Endpoint,
                appConfigService1.Config.JiraConfig.Username,
                appConfigService1.Config.JiraConfig.Password);
        }

        public async Task<JiraBatchDataHeader> GetEpicIssuesHeader(string projectId = null)
        {
            var jql = GetEpicIssuesQuery(projectId);

            return await GetHeader(jql);
        }

        public async Task<JiraBatchDataHeader> GetIssuesUnderEpicHeader(string epicKey, string projectId = null)
        {
            var jql = GetIssuesUnderEpicQuery(epicKey, projectId);

            return await GetHeader(jql);
        }

        public async Task<JiraBatchDataHeader> GetIssuesWithoutEpicHeader(string projectId = null)
        {
            var jql = GetIssuesWithoutEpicQuery(projectId);

            return await GetHeader(jql);
        }

        public async Task<IEnumerable<JiraIssue>> GetEpicIssuesBatch(JiraBatchDataHeader jiraBatchDataHeader)
        {
            var issuesBatch = await _jira.Issues.GetIssuesFromJqlAsync(jiraBatchDataHeader.Query, 100,
                jiraBatchDataHeader.BatchStartAtIndex);

            var processedIssues = new List<JiraIssue>();

            foreach (var i in issuesBatch.ToList())
            {
                var ji = i.Convert();
                await SetSubTaskEpicLink(ji);
                processedIssues.Add(ji);
            }

            return processedIssues;
        }

        #region Private Methods
        private async Task<JiraBatchDataHeader> GetHeader(string jql)
        {
            var firstIssue = await _jira.Issues.GetIssuesFromJqlAsync(jql, 0);

            var header = new JiraBatchDataHeader(jql, firstIssue.TotalItems);

            return header;
        }

        private async Task SetSubTaskEpicLink(JiraIssue issue)
        {
            if (issue.Type == "Sub-task")
            {
                var parentIssueResult = await _jira.Issues.GetIssuesFromJqlAsync($"issueKey = {issue.ParentIssueKey}", 1);
                if (parentIssueResult != null && parentIssueResult.Count() == 1)
                {
                    var parentIssue = parentIssueResult.FirstOrDefault().Convert();
                    issue.EpicLink = parentIssue.EpicLink;
                }
            }
        }

        private string GetEpicIssuesQuery(string projectId = null)
        {
            var query = string.IsNullOrEmpty(projectId) ? string.Empty : $"Project = '{projectId}' AND ";
            query += "Type = 'Epic' ";
            query += "ORDER BY Created DESC";

            return query;
        }

        private string GetIssuesUnderEpicQuery(string epicKey, string projectId = null)
        {
            var query = string.IsNullOrEmpty(projectId) ? string.Empty : $"Project = '{projectId}' AND ";

            query += string.IsNullOrEmpty(epicKey)
                ? $"'{CoreConstants.JiraEpicLink}' is EMPTY "
                : $"'{CoreConstants.JiraEpicLink}' = '{epicKey}' ";

            query += "ORDER BY Created DESC";

            return query;
        }

        private string GetIssuesWithoutEpicQuery(string projectId = null)
        {
            return GetIssuesUnderEpicQuery(null, projectId);
        }
        #endregion

    }
}
