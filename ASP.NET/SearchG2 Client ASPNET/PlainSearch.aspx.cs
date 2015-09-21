using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Query;

namespace CrownPeak.SearchG2.Client
{
	public partial class PlainSearch : System.Web.UI.Page
	{
		private const int ROWS = 10;

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			phSearchResults.Visible = false;

			string text = txtSearch.Text.Trim();
			if (!string.IsNullOrEmpty(text))
			{
				Query(text, 0);
			}
		}

		protected void Pager_Click(object sender, EventArgs e)
		{
			int page = int.Parse(((LinkButton) sender).CommandArgument);
			hidPage.Value = page.ToString();
			Query(null, (page - 1) * ROWS, hidLoggingId.Value);
		}

		protected void Suggestion_Click(object sender, EventArgs e)
		{
			string suggestion = ((LinkButton) sender).CommandArgument;
			txtSearch.Text = suggestion;
			Query(suggestion, 0);
		}

		protected void AddFilter_Click(object sender, EventArgs e)
		{
			FilterQuery fq = new FilterQuery(((LinkButton)sender).CommandArgument);
			FilterQueryCollection existingQueries = new FilterQueryCollection(hidFilterQueries.Value);
			existingQueries.Add(fq);
			Query(null, 0, hidLoggingId.Value, existingQueries);
		}

		protected void RemoveFilter_Click(object sender, EventArgs e)
		{
			FilterQueryCollection existingQueries = new FilterQueryCollection(hidFilterQueries.Value);
			FilterQuery fq = new FilterQuery(((LinkButton)sender).CommandArgument);
			existingQueries.Remove(fq);
			Query(null, 0, hidLoggingId.Value, existingQueries);
		}

		protected void Query(string text, int start, string loggingId = null, FilterQueryCollection filterQueries = null)
		{
			if (text == null) text = txtSearch.Text.Trim();

			phSearchResults.Visible = true;
			lblQuery.Text = text;
			lblQueryNoResults.Text = text;

			if (filterQueries == null)
			{
				hidFilterQueries.Value = "";
				rptFilters.Visible = false;
			}
			else
			{
				hidFilterQueries.Value = filterQueries.ToString();
				rptFilters.Visible = filterQueries.Count > 0;
				rptFilters.DataSource = filterQueries;
				rptFilters.DataBind();
			}

			var search = new CrownPeak.SearchG2.PlainSearch(new Settings("www.crownpeak.com"));
			QueryOptions options = new QueryOptions()
			{
				Highlighting = true,
				Start = start,
				Rows = ROWS,
				SpellCheck = true,
				FacetFields = new[] { "url", "title" },
				FilterQueries = filterQueries,
				LoggingId = loggingId,
				// Language = "en" });
			};
			var resultSet = search.Execute(text, options);

			phSearchResultsNoResults.Visible = resultSet.Count == 0;
			phSearchResultsResults.Visible = !phSearchResultsNoResults.Visible;

			lblCount.Text = resultSet.TotalCount.ToString();
			lblFrom.Text = (start + 1).ToString();
			lblTo.Text = (start + resultSet.Count).ToString();

			phSuggestions.Visible = (resultSet.Suggestions.Count > 0);
			if (resultSet.Suggestions.Count > 0)
			{
				lbSuggestionCollation.Text = resultSet.Suggestions.Collation;
				lbSuggestionCollation.CommandArgument = resultSet.Suggestions.Collation;
			}

			if (resultSet.Facets != null && resultSet.Facets.Count > 0)
			{
				rptFacets.DataSource = resultSet.Facets;
				rptFacets.DataBind();
			}

			hidLoggingId.Value = "";
			var pages = Enumerable.Range(1, (int)Math.Ceiling((double)resultSet.TotalCount / ROWS));
			if (resultSet.CrownPeak != null && resultSet.CrownPeak.Logging != null)
			{
				hidLoggingId.Value = resultSet.CrownPeak.Logging.Id;
			}
			rptPager.DataSource = pages;
			rptPager.DataBind();
			rptPager.Visible = resultSet.TotalCount > ROWS;

			rptResults.DataSource = resultSet;
			rptResults.DataBind();
		}
	}
}