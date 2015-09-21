using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrownPeak.SearchG2;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Query;

namespace CrownPeak.SearchG2.Client.Controllers
{
	public class AutocompleteController : Controller
	{
		[HttpGet]
		public JsonResult Autocomplete(string query)
		{
			var search = new Suggest(new Settings("www.crownpeak.com"));
			var searcher = search.Execute(query);
			if (searcher.Count > 0)
			{
				return Json(searcher[0].Options.Select(o => new { Value = o, Label = o }), JsonRequestBehavior.AllowGet);
			}
			return null;
		}
	}
}
