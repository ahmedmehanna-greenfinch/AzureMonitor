using System.Collections.Generic;

namespace AzureMonitor.Models.Jira
{
    public class BillableCategory
    {
        public BillableCategory()
        {
            Budgets = new HashSet<BillableCategoryBudget>();
            Issues = new HashSet<JiraIssue>();
        }

        public int BillableCategoryId { get; set; }
        public string Name { get; set; }
        public int? ProjectId { get; set; }

        public virtual ICollection<JiraIssue> Issues { get; set; }
        public virtual ICollection<BillableCategoryBudget> Budgets { get; set; }
        public virtual JiraProject Project { get; set; }

        
        public object Id => BillableCategoryId;
    }
}
