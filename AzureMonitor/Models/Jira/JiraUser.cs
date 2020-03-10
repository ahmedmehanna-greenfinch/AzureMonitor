using System.Collections.Generic;
using AzureMonitor.Interfaces;

namespace AzureMonitor.Models.Jira
{
    public class JiraUser : IModel
    {
        public JiraUser()
        {
            Groups = new HashSet<JiraUserGroups>();
        }

        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string SelfUri { get; set; }
        public bool IsGreenfinchStaff { get; set; }

        public virtual ICollection<JiraUserGroups> Groups { get; set; }

        public object Id => UserName;
    }
}
