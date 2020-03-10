namespace AzureMonitor.Models.Jira
{
    public class JiraUserGroups
    {
        public string UserName { get; set; }

        public virtual JiraUser User { get; set; }

        public int GroupId { get; set; }

        public virtual JiraGroup JiraGroup { get; set; }

        public object Id => new object[] { UserName, GroupId };
    }
}
