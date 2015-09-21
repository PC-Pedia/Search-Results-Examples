using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrownPeak.SearchG2;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Query;
using CrownPeak.SearchG2.Result;
using CrownPeak.SearchG2.Client.UserControls;

namespace CrownPeak.SearchG2.Client
{
	public partial class Search : System.Web.UI.Page
	{
		private const string COLLECTION = "www.crownpeak.com";
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

		protected void Suggestion_Click(object sender, EventArgs e)
		{
			string suggestion = ((LinkButton) sender).CommandArgument;
			txtSearch.Text = suggestion;
			// If they clicked on a suggestion, don't give them more - they want this one!
			Query(suggestion, 0, hideSuggestions: true);
		}

		protected void FacetAndFilter_OnFilterChanged(object sender, FilterChangedEventArgs e)
		{
			Query(null, 0, rptPager.LoggingId, filterQueries: e.Filters);
		}

		protected void Pager_PageChanged(object sender, PageChangedEventArgs e)
		{
			int page = e.NewPage;
			Query(null, (page - 1) * ROWS, e.LoggingId);
		}

		protected void Query(string text, int start, string loggingId = null, FilterQueryCollection filterQueries = null, bool hideSuggestions = false)
		{
			if (text == null) text = txtSearch.Text.Trim();

			phSearchResults.Visible = true;
			lblQuery.Text = text;
			lblQueryNoResults.Text = text;

			FacetAndFilter.FilterDataSource = filterQueries;
			FacetAndFilter.DataBind();

			var search = new Search<MyResult>(new Settings(COLLECTION));
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
			var cpResults = search.Execute(text, options);

			phSearchResultsNoResults.Visible = cpResults.Count == 0 && (filterQueries == null || filterQueries.Count == 0);
			phSearchResultsResults.Visible = !phSearchResultsNoResults.Visible;

			lblCount.Text = cpResults.TotalCount.ToString();
			lblFrom.Text = (start + 1).ToString();
			lblTo.Text = (start + cpResults.Count).ToString();
			phResultFromTo.Visible = start + cpResults.Count > 1;

			phSuggestions.Visible = (cpResults.Suggestions.Count > 0 && cpResults.Start == 0 && !hideSuggestions);
			if (cpResults.Suggestions.Count > 0)
			{
				lbSuggestionCollation.Text = cpResults.Suggestions.Collation;
				lbSuggestionCollation.CommandArgument = cpResults.Suggestions.Collation;
			}

			if (cpResults.Facets != null && cpResults.Facets.Count > 0)
			{
				FacetAndFilter.FacetDataSource = cpResults.Facets;
				FacetAndFilter.DataBind();
			}

			var pages = Enumerable.Range(1, (int) Math.Ceiling((double) cpResults.TotalCount/ROWS));
			rptPager.LoggingId = "";
			if (cpResults.CrownPeak != null && cpResults.CrownPeak.Logging != null)
			{
				rptPager.LoggingId = cpResults.CrownPeak.Logging.Id;
			}
			rptPager.DataSource = pages;
			rptPager.DataBind();
			rptPager.Visible = cpResults.TotalCount > ROWS;

			rptResults.DataSource = cpResults;
			rptResults.DataBind();
		}

		[System.Web.Services.WebMethod]
		[System.Web.Script.Services.ScriptMethod]
		public static string[] GetCompletionList(string prefixText, int count)
		{
			List<string> results = new List<string>();

			var autocompleter = new Suggest(new Settings(COLLECTION));
			var searchResults = autocompleter.Execute(prefixText);
			foreach (var suggestion in (IEnumerable<Suggestion>)searchResults)
			{
				foreach (var option in suggestion.Options)
				{
					results.Add(option);
					if (results.Count >= count) break;
				}
				if (results.Count >= count) break;
			}

			return results.ToArray();
		}
	}
}