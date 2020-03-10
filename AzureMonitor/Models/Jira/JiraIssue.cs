using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AzureMonitor.Interfaces;

namespace AzureMonitor.Models.Jira
{
    public class JiraIssue : IModel
    {
        public JiraIssue()
        {
            Worklogs = new HashSet<JiraWorkLog>();
        }

        public int IssueId { get; set; }
        public string ParentIssueKey { get; set; }
        public string SecurityLevel { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Environment { get; set; }
        public string Assignee { get; set; }
        public string Key { get; set; }
        public string Priority { get; set; }
        public string ProjectName { get; set; }
        public string Reporter { get; set; }
        public string Resolution { get; set; }

        public string Labels { get; set; }
        public string FixVersion { get; set; }
        public string AffectedVersion { get; set; }
        public string Components { get; set; }

        public string Status { get; set; }
        public string Type { get; set; }
        public string EpicName { get; set; }
        public string EpicLink { get; set; }
        public string HKIBU { get; set; }
        public string SCBU { get; set; }
        public string SNBU { get; set; }
        public string CFBU { get; set; }
        public string Sprint { get; set; }
        public string TargetVersion { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? ResolutionDate { get; set; }

        // FK
        public int? ProjectId { get; set; }
        public int? BillableCategoryId { get; set; }

        public virtual BillableCategory BillableCategory { get; set; }
        public virtual JiraProject Project { get; set; }

        public virtual ICollection<IssueCustomField> CustomFields { get; set; }
        public virtual ICollection<JiraWorkLog> Worklogs { get; set; }

        [NotMapped]
        public object Id => IssueId;

        public override string ToString()
        {
            return $"{Key} - [{IssueId}]";
        }
    }
}
