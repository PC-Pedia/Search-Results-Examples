using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CrownPeak.SearchG2.Query;

namespace CrownPeak.SearchG2.Client.Models
{
	public class Query
	{
		public string QueryText { get; set; }
		public int Page { get; set; }
		public string LoggingId { get; set; }
		public string FilterQueries { get; set; }
		public bool HideSuggestions { get; set; }
	}
}