using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy
{
	class Query
	{
		public string Columns { get; set; }
		public string Tables{ get; set; }
		public string Condition{ get; set; }
		public string GroupBy { get; set; }
		public Query(string columns, string tables, string condition = "", string group_by="")
		{
			Columns = columns;
			Tables = tables;
			Condition = condition;
			GroupBy = group_by;
		}
	}
}
