namespace AzureMonitor.Models.Jira
{
    public class IssueCustomField
    {
        public int IssueId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public virtual JiraIssue Issue { get; set; }

        public object Id => new object[] { IssueId, Key };
    }
}
