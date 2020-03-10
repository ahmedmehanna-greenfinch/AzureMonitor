using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlassian.Jira;
using AzureMonitor.Models.Jira;

namespace AzureMonitor.Utils
{
    /// <summary>
    ///     Jira to JTR object converter methods
    /// </summary>
    public static class ObjectConverter
    {
        /// <summary>
        ///     Converts a Jira Project into a JTR JiraProject
        /// </summary>
        /// <param name="project">An <see cref="Atlassian.Jira.Project"/> to convert into <see cref="JiraProject"/></param>
        /// <returns>A converted <see cref="JiraProject"/></returns>
        public static JiraProject Convert(this Atlassian.Jira.Project project)
        {
            var jtrJiraProject = new JiraProject
            {
                ProjectId = System.Convert.ToInt32(project.Id),
                Name = project.Name,
                Key = project.Key,
                Category = project.Category?.Name,
                Lead = project.Lead,
                Url = project.Url
            };

            return jtrJiraProject;
        }

        /// <summary>
        ///     Converts a Jira Issue into a JTR JiraIssue
        /// </summary>
        /// <param name="issue">An <see cref="Atlassian.Jira.Issue"/> to convert into <see cref="JiraIssue"/></param>
        /// <returns>A converted <see cref="JiraIssue"/></returns>
        public static JiraIssue Convert(this Atlassian.Jira.Issue issue)
        {
            {
                var jtrJiraIssue = new JiraIssue
                {
                    IssueId = System.Convert.ToInt32(issue.JiraIdentifier),
                    Description = issue.Description,
                    Summary = issue.Summary,
                    ParentIssueKey = issue.ParentIssueKey,
                    SecurityLevel = issue.SecurityLevel?.Name,
                    Environment = issue.Environment,
                    Assignee = issue.Assignee,
                    Key = issue.Key?.Value,
                    Priority = issue.Priority?.Name,
                    ProjectName = issue.Project,
                    Reporter = issue.Reporter,
                    Resolution = issue.Resolution?.Name,

                    Labels = string.Join(CoreConstants.CommaSeparator, issue.Labels),
                    FixVersion = string.Join(CoreConstants.CommaSeparator, issue.FixVersions),
                    AffectedVersion = string.Join(CoreConstants.CommaSeparator, issue.AffectsVersions),
                    Components = string.Join(CoreConstants.CommaSeparator, issue.Components),

                    Status = issue.Status?.Name,
                    Type = issue.Type?.Name,
                    Created = issue.Created,
                    DueDate = issue.DueDate,
                    Updated = issue.Updated,
                    ResolutionDate = issue.ResolutionDate,
                };

                jtrJiraIssue.EpicLink = issue[CoreConstants.JiraEpicLink]?.Value;
                jtrJiraIssue.EpicName = issue[CoreConstants.JiraEpicName]?.Value;

                jtrJiraIssue.HKIBU = issue.CustomFields.Get(CoreConstants.JiraHkiBusinessUnit);
                jtrJiraIssue.SCBU = issue.CustomFields.Get(CoreConstants.JiraScBusinessUnit);
                jtrJiraIssue.SNBU = issue.CustomFields.Get(CoreConstants.JiraSnBusinessUnit);
                jtrJiraIssue.CFBU = issue.CustomFields.Get(CoreConstants.JiraCfBusinessUnit);

                jtrJiraIssue.Sprint = issue[CoreConstants.JiraSprint]?.Value;
                jtrJiraIssue.TargetVersion = issue[CoreConstants.JiraTargetVersion]?.Value;

                // Extract business unit                
                ProcessBusinessUnitCustomField(jtrJiraIssue);

                return jtrJiraIssue;
            }
        }

        /// <summary>
        ///     Processes JTR Business Unit <see cref="IssueCustomField"/> for a given JiraIssue
        /// </summary>
        /// <param name="jiraIssue">A JTR Jira Issue to process</param>
        private static void ProcessBusinessUnitCustomField(JiraIssue jiraIssue)
        {
            string businessUnit = GetFirstPopulatedField(jiraIssue.HKIBU, jiraIssue.SCBU, jiraIssue.SNBU, jiraIssue.CFBU);

            if (businessUnit != null)
            {
                if (jiraIssue.CustomFields == null)
                {
                    jiraIssue.CustomFields = new List<IssueCustomField>();
                }

                jiraIssue.CustomFields.Add(new IssueCustomField()
                {
                    IssueId = jiraIssue.IssueId,
                    Key = CoreConstants.BusinessUnit,
                    Value = businessUnit,
                });
            }
        }

        /// <summary>
        ///     Gets a first non-null business unit from a given array of business units
        /// </summary>
        /// <param name="businessUnits">Business units to check for validity</param>
        /// <returns>The first valid business unit string or <see cref="null"/> if nothing is found</returns>
        private static string GetFirstPopulatedField(params string[] businessUnits)
        {
            foreach (var unit in businessUnits)
            {
                if (!string.IsNullOrWhiteSpace(unit))
                {
                    return unit;
                }
            }

            return null;
        }

        /// <summary>
        ///     Extracts a value from <see cref="Atlassian.Jira.CustomFieldValueCollection"/> given a field code
        /// </summary>
        /// <param name="fields">An <see cref="Atlassian.Jira.CustomFieldValueCollection"/> to process</param>
        /// <param name="fieldCode">The field identifier to process for</param>
        /// <returns>An extracted value or an empty string is nothing is found</returns>
        private static string Get(this Atlassian.Jira.CustomFieldValueCollection fields, string fieldCode)
        {
            try
            {
                return string.Join(CoreConstants.CommaSeparator,
                    fields[fieldCode] == null ? Enumerable.Empty<string>() : fields[fieldCode].Values);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts a Jira Worklog into a JTR JiraWorkLog
        /// </summary>
        /// <param name="worklog">An <see cref="Atlassian.Jira.Worklog"/> to convert into <see cref="JiraWorkLog"/></param>
        /// <returns>A converted <see cref="JiraWorkLog"/></returns>
        public static JiraWorkLog Convert(this Atlassian.Jira.Worklog worklog)
        {
            {
                var jtrJiraWorklog = new JiraWorkLog
                {
                    WorkLogId = System.Convert.ToInt32(worklog.Id),
                    Author = worklog.Author,
                    Comment = worklog.Comment,
                    CreateDate = worklog.CreateDate,
                    StartDate = worklog.StartDate,
                    UpdateDate = worklog.UpdateDate,
                    TimeSpent = worklog.TimeSpent,
                    TimeSpentInSeconds = worklog.TimeSpentInSeconds,
                };

                return jtrJiraWorklog;
            }
        }

        /// <summary>
        ///     Converts a Jira User into a JTR JiraUser
        /// </summary>
        /// <param name="jiraUser">An <see cref="Atlassian.Jira.JiraUser"/> to convert into <see cref="JiraUser"/></param>
        /// <returns>A converted <see cref="JiraUser"/></returns>
        public static Models.Jira.JiraUser Convert(this Atlassian.Jira.JiraUser jiraUser)
        {
            {
                var jtrJiraUser = new Models.Jira.JiraUser()
                {
                    UserName = jiraUser.AccountId,
                    DisplayName = jiraUser.DisplayName,
                    IsActive = jiraUser.IsActive,
                    SelfUri = jiraUser.Self
                };

                return jtrJiraUser;
            }
        }
    }
}
