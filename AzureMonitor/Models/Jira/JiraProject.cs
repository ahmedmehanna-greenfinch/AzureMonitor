using System.Collections.Generic;
using AzureMonitor.Interfaces;

namespace AzureMonitor.Models.Jira
{
    public class JiraProject : IModel
    {
        public JiraProject()
        {
            Issues = new HashSet<JiraIssue>();
            BillableCategories = new HashSet<BillableCategory>();
        }


        public int ProjectId { get; set; }
        public string Key { get; set; }
        public string Category { get; set; }
        public string Lead { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public bool IsBillable { get; set; }

        public virtual ICollection<JiraIssue> Issues { get; set; }
        public virtual ICollection<BillableCategory> BillableCategories { get; set; }

        public object Id => ProjectId;
    }
}
