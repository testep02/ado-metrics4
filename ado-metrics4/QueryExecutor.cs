using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;

using ado_metrics4;

namespace adometrics
{
    public class QueryExecutor
    {
        private readonly Uri uri;
        private AdoConfiguration adoConfig = AdoConfiguration.Instance;

        public QueryExecutor()
        {
            Console.WriteLine("Creating QE Class");

            this.uri = new Uri("https://dev.azure.com/" + adoConfig.OrgName);
        }

        public async Task<IList<WorkItem>> QueryOpenBugs()
        {
            Console.WriteLine("Query Open Bugs called");

            var credentials = new VssBasicCredential(string.Empty, adoConfig.Pat);

            string backlogQuery = String.Format(AdoQueries.WorkItemQuery, adoConfig.Project);
            var wiql = new Wiql()
            {
                Query = backlogQuery,
            };

            using (var httpClient = new WorkItemTrackingHttpClient(this.uri, credentials))
            {
                Console.WriteLine("Getting Results");
                var result = await httpClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var ids = result.WorkItems.Select(item => item.Id).ToArray();

                if (ids.Length == 0)
                {
                    Console.WriteLine("No results found");
                    return Array.Empty<WorkItem>();
                }

                var fields = new[] {
                    "System.Id",
                    "System.CreatedDate",
                    "Microsoft.VSTS.Common.ActivatedDate",
                    "Microsoft.VSTS.Common.ClosedDate",
                    "System.History",
                    "System.BoardColumn",
                    "System.BoardLane",
                    "System.ChangedDate"
                };

                var workItems = await httpClient.GetWorkItemsAsync(ids, AdoQueries.WorkItemFields, result.AsOf).ConfigureAwait(false);                                

                Console.WriteLine("Finished getting the bugs");
                Console.WriteLine("Query Results: {0} items found", workItems.Count);

                return workItems;
            }

        }

        public async Task GetWorkItemRevisionsAsync(WorkItem workItem)
        {
            var credentials = new VssBasicCredential(string.Empty, adoConfig.Pat);
            List<WorkItem> workItemRevisions = new List<WorkItem>();

            using (var httpClient = new WorkItemTrackingHttpClient(this.uri, credentials))
            {
                workItemRevisions.AddRange(await httpClient.GetRevisionsAsync((int)workItem.Id));
                //WorkItemData workItemRevisions = new WorkItemData(workItem, await httpClient.GetRevisionsAsync((int)workItem.Id));

                var distinctStates = new WorkItemData(workItem, workItemRevisions);
                List<WorkItem> dsWorkItems = distinctStates.GetUniqueWorkItemStates();

                Console.WriteLine("There are {0} distinct states found.", dsWorkItems.Count);

                foreach (var wi in dsWorkItems)
                {
                    if (wi.Fields.Keys.Contains("System.BoardColumn") &&
                        wi.Fields.Keys.Contains("System.ChangedDate"))
                    {
                        Console.WriteLine("{0}\t{1}",
                            wi.Fields["System.BoardColumn"],
                            wi.Fields["System.ChangedDate"]);
                    }
                }

                //foreach (var wi in workItemRevisions.WorkItemRevisions)
                //{

                //}
            }
        }

        public async Task PrintOpenBugsAsync()
        {
            var workItems = await this.QueryOpenBugs().ConfigureAwait(false);

            Console.WriteLine("Query Results: {0} items found", workItems.Count);

            List<WorkItem> validItems = new List<WorkItem>();

            foreach (var workItem in workItems)
            {
                if(workItem.Fields.Keys.Contains("Microsoft.VSTS.Common.ActivatedDate") &&
                    workItem.Fields.Keys.Contains("Microsoft.VSTS.Common.ClosedDate"))
                {
                    validItems.Add(workItem);
                }
            }

            foreach (var workItem in validItems)
            {
                Console.WriteLine(
                    "{0}\t{1}\t{2}\t{3}",
                    workItem.Id,
                    workItem.Fields["System.BoardColumn"],
                    workItem.Fields["System.ChangedDate"],
                    workItem.Fields["System.BoardLane"]);
                await GetWorkItemRevisionsAsync(workItem);
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
