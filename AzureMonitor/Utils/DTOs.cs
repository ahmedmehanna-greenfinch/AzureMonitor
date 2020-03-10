using System;

namespace AzureMonitor.Utils
{
    /// <summary>
    ///     DTO used in Core Jira Issue import logic
    /// </summary>
    public class ProjectSaveIssueDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectKey { get; set; }
    }

    /// <summary>
    ///     Object used in batch processing of Issues and Worklogs
    /// </summary>
    public class JiraBatchDataHeader
    {
        public JiraBatchDataHeader(string query,
            int totalItemsCount,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int batchStartAtIndex = 0,
            int batchEndAtIndex = 0,
            int batchNumber = 0)
        {
            BatchEndAtIndex = batchEndAtIndex;
            BatchNumber = batchNumber;
            StartDate = startDate;
            EndDate = endDate;
            BatchStartAtIndex = batchStartAtIndex;
            Query = query;
            TotalItemsCount = totalItemsCount;
        }

        /// <summary>
        ///     The current batch last item's index in the overall set of Jira items to retrieve
        /// </summary>
        public int BatchEndAtIndex { get; }

        /// <summary>
        ///     Number of this batch in the sequence of batches fetching the queried Jira items
        /// </summary>
        public int BatchNumber { get; }

        /// <summary>
        ///     The current batch first item's index in the overall set of Jira items to retrieve
        /// </summary>
        public int BatchStartAtIndex { get; }

        /// <summary>
        ///     The end date in the JQL query date range. Can be set to anything if query is not filtering by date range and will
        ///     be ignored
        /// </summary>
        public DateTime? EndDate { get; }

        /// <summary>
        ///     The Jira JQL string of the batch query
        /// </summary>
        public string Query { get; }

        /// <summary>
        ///     The start date in the JQL query date range. Can be set to anything if query is not filtering by date range and will
        ///     be ignored
        /// </summary>
        public DateTime? StartDate { get; }

        /// <summary>
        ///     Total items count returned by the JQL query
        /// </summary>
        public int TotalItemsCount { get; }
    }
}
