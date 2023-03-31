using System;
namespace ado_metrics4
{
	public class WorkItemBoardColumn
	{
		public int WorkItemId
		{
			get;
		}

		public string WorkItemBoardColumnName
		{
			get;
		}

		public DateTime WorkItemStateChangeDate
		{
			get;
		}

		public WorkItemBoardColumn()
		{
		}
	}
}

