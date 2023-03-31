using System;
using System.Collections.Generic;

namespace ado_metrics4
{
	public class AdoQueries
	{
		private static readonly string _workItemQuery = $"Select [System.Id], " +
                                    "[Created Date], " +
                                    "[History], " +
                                    "[Microsoft.VSTS.Common.ActivatedDate], " +
                                    "[Closed Date], " +
                                    "[System.BoardColumn], " +
                                    "[System.ChangedDate], " +
                                    "[System.BoardLane]" +
                                    "From WorkItems " +
                                    "Where [Work Item Type] = 'User Story' " +
                                    "And [System.TeamProject] = '{0}' " +
                                    //"And [System.State] = 'Closed' " +
                                    "And [Assigned To] = 'Sourav Das' " +
                                    "And [Area Path] = 'OneLexmarkCloud\\ART-LCS\\Print 1' " +
                                    "Order By [State] Asc, [Changed Date] Desc";

        private static readonly List<string> _workItemFields = new List<string> {
            "System.Id",
            "System.CreatedDate",
            "Microsoft.VSTS.Common.ActivatedDate",
            "Microsoft.VSTS.Common.ClosedDate",
            "System.History",
            "System.BoardColumn",
            "System.BoardLane",
            "System.ChangedDate"
        };

        public static string WorkItemQuery
        {
            get
            {
                return _workItemQuery;
            }
        }

        public static List<string> WorkItemFields
        {
            get
            {
                return _workItemFields;
            }
        }

        public AdoQueries()
		{
		}
	}
}

