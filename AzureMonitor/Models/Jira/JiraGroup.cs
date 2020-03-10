using System.Collections.Generic;

namespace AzureMonitor.Models.Jira
{
    public class JiraGroup
    {
        public JiraGroup()
        {
            Users = new HashSet<JiraUserGroups>();
        }

        public int GroupId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<JiraUserGroups> Users { get; set; }

        public object Id => GroupId;
    }
}
