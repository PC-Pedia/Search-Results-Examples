using System;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Query;

namespace CrownPeak.SearchG2.Client
{
	public partial class RSS : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Query("*:*", 0);
		}

		protected void Query(string text, int start, FilterQueryCollection filterQueries = null)
		{
			var search = new Search<MyResult>(new Settings("www.crownpeak.com"));
			QueryOptions options = new QueryOptions()
			{
				Highlighting = false,
				Start = 0,
				Rows = 100000,
				SpellCheck = false,
				OrderBy = new[] { new SortOrder("date", CrownPeak.SearchG2.Query.SortDirection.Desc) },
				// Language = "en"
			};
			var cpResults = search.Execute(text, options);

			rptItems.DataSource = cpResults;
			rptItems.DataBind();
		}
	}
}