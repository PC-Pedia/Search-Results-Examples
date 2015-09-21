using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrownPeak.SearchG2;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Query;
using CrownPeak.SearchG2.Client.Models;

namespace CrownPeak.SearchG2.Client.Controllers
{
	public class SearchController : Controller
	{
		private const int ROWS = 10;

		// GET: GenericSearch
		public ActionResult Index()
		{
			return View(); //new Query()); // We don't need the new model, as we're not doing anything with hidden fields
		}

		public ActionResult Search(string queryText, int? page, string loggingId, string filterQueries, bool? hideSuggestions)
		{
			if (!page.HasValue || page < 1) page = 1;

			// We might have a leading ; from our concatenation
			if (filterQueries != null && filterQueries.StartsWith(";")) filterQueries = filterQueries.Substring(1);
			var fqCollection = new FilterQueryCollection(filterQueries);
			ViewBag.FilterQueryCollection = fqCollection;
			ViewBag.Query = queryText;
			ViewBag.Page = page.Value;
			ViewBag.HideSuggestions = hideSuggestions.HasValue && hideSuggestions.Value;

			var search = new Search<SearchResult>(new Settings("www.crownpeak.com"));
			QueryOptions options = new QueryOptions()
			{
				Highlighting = true,
				Start = (page.Value - 1) * ROWS,
				Rows = ROWS,
				SpellCheck = true,
				FacetFields = new[] { "url", "title" },
				FilterQueries = new FilterQueryCollection(filterQueries),
				LoggingId = loggingId,
				// Language = "en" });
			};
			var results = search.Execute(queryText, options);

			ViewBag.Pager = null;
			ViewBag.LoggingId = "";
			if (results.CrownPeak != null && results.CrownPeak.Logging != null)
			{
				ViewBag.LoggingId = results.CrownPeak.Logging.Id;
			}
			if (results.TotalCount > ROWS)
			{
				// Make a simple pager
				ViewBag.Pager = Enumerable.Range(1, (int)Math.Ceiling((double)results.TotalCount / ROWS));
			}

			return View(results);
		}
	}
}