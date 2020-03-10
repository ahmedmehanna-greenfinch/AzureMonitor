using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureMonitor.Interfaces;
using AzureMonitor.Models.Jira;
using AzureMonitor.Utils;

namespace AzureMonitor.Services
{
    public class BatchProcessService: IBatchProcessService
    {
        private readonly IJiraService _jiraService;

        public BatchProcessService(IJiraService jiraService)
        {
            _jiraService = jiraService;
        }

        /// <summary>
        ///     Processes Epic Issues in batches of hundred Issues
        /// </summary>
        /// <param name="projectId">The project id to process for</param>
        public async Task<List<JiraIssue>> ProcessEpics(string projectId = null)
        {
            var header = await _jiraService.GetEpicIssuesHeader(projectId);

            var batchHeaders = GenerateBatchHeaders(header);

            var epics = new List<JiraIssue>();

            for (var i = 0; i < batchHeaders.Count; i++)
            {
                var issueBatch = await _jiraService.GetEpicIssuesBatch(batchHeaders.ElementAt(i));

                epics.AddRange(issueBatch);

                //await _dataService.SaveIssues(issueBatch);
            }

            return epics;
        }

        public async Task<List<JiraIssue>> ProcessIssuesUnderEpic(string epicKey, string projectId = null)
        {
            var header = await _jiraService.GetIssuesUnderEpicHeader(epicKey, projectId);

            var batchHeaders = GenerateBatchHeaders(header);

            var issues = new List<JiraIssue>();

            for (var i = 0; i < batchHeaders.Count; i++)
            {
                var issueBatch = await _jiraService.GetEpicIssuesBatch(batchHeaders.ElementAt(i));

                issues.AddRange(issueBatch);

                //await _dataService.SaveIssues(issueBatch);
            }

            return issues;
        }


        public async Task<List<JiraIssue>> ProcessIssuesWithoutEpic(string projectId = null)
        {
            var header = await _jiraService.GetIssuesWithoutEpicHeader(projectId);

            var batchHeaders = GenerateBatchHeaders(header);

            var issues = new List<JiraIssue>();

            for (var i = 0; i < batchHeaders.Count; i++)
            {
                var issueBatch = await _jiraService.GetEpicIssuesBatch(batchHeaders.ElementAt(i));

                issues.AddRange(issueBatch);

                //await _dataService.SaveIssues(issueBatch);
            }

            return issues;
        }



        #region helpers

        /// <summary>
        ///     Generates batch header objects to be used in batch processing of Issues and Worklogs
        /// </summary>
        /// <param name="jiraBatchDataHeader">A <see cref="JiraBatchDataHeader" /> object with initial input data</param>
        /// <returns>A list of <see cref="JiraBatchDataHeader" /> objects with batch data</returns>
        private static List<JiraBatchDataHeader> GenerateBatchHeaders(JiraBatchDataHeader jiraBatchDataHeader)
        {
            var headerList = new List<JiraBatchDataHeader>();

            if (jiraBatchDataHeader.TotalItemsCount > 100)
            {
                var maxBatches = jiraBatchDataHeader.TotalItemsCount / 100 +
                                 (jiraBatchDataHeader.TotalItemsCount % 100 == 0 ? 0 : 1);
                foreach (var batchNumber in Enumerable.Range(0, maxBatches))
                {
                    var startIndex = batchNumber * 100;
                    var endIndex = startIndex + 99 < jiraBatchDataHeader.TotalItemsCount
                        ? startIndex + 99
                        : jiraBatchDataHeader.TotalItemsCount;
                    headerList.Add(
                        new JiraBatchDataHeader(jiraBatchDataHeader.Query,
                            jiraBatchDataHeader.TotalItemsCount,
                            jiraBatchDataHeader.StartDate,
                            jiraBatchDataHeader.EndDate,
                            startIndex,
                            endIndex,
                            batchNumber));
                }
            }
            else
            {
                headerList.Add(new JiraBatchDataHeader(jiraBatchDataHeader.Query,
                    jiraBatchDataHeader.TotalItemsCount,
                    jiraBatchDataHeader.StartDate,
                    jiraBatchDataHeader.EndDate,
                    0, 99));
            }

            return headerList;
        }

        #endregion // helpers
    }
}
