using System;
using AzureMonitor.Interfaces;

namespace AzureMonitor.Models.Jira
{
    public class JiraWorkLog : IModel
    {
        public int WorkLogId { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string TimeSpent { get; set; }
        public long TimeSpentInSeconds { get; set; }

        // FK
        public int? IssueId { get; set; }

        public virtual JiraIssue Issue { get; set; }

        public object Id => WorkLogId;
    }
}
