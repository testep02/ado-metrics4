using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;

namespace ado_metrics4
{
    public class WorkItemData
    {
        private readonly WorkItem workItem;
        private readonly List<WorkItem> workItemRevisions;

        public List<WorkItem> WorkItemRevisions
        {
            get
            {
                return workItemRevisions;
            }
        }

        public WorkItemData(WorkItem workItem, List<WorkItem> workItemRevisions)
        {
            this.workItem = workItem;
            this.workItemRevisions = workItemRevisions;
        }

        public List<WorkItem> GetUniqueWorkItemStates()
        {
            List<WorkItem> uniqueStates = workItemRevisions
                .GroupBy(p => p.Fields["System.BoardColumn"])
                .Select(g => g.First())
                .ToList();
            
            return uniqueStates;
        }
    }
}
