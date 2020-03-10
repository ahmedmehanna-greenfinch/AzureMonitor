namespace AzureMonitor.Models.Jira
{
    public class BillableCategoryBudget
    {
        public int BillableCategoryBudgetId { get; set; }
        public decimal Days { get; set; }
        public string Note { get; set; }

        // FK
        public int? BillableCategoryId { get; set; }

        public virtual BillableCategory BillableCategory { get; set; }

        public object Id => BillableCategoryBudgetId;
    }
}
