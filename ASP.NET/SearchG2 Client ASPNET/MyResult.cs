using System;
using System.Collections.Generic;
using System.Linq;
using CrownPeak.SearchG2.Attributes;

namespace CrownPeak.SearchG2.Client
{
	public class MyResult : CrownPeak.SearchG2.Result.ResultBase
	{
		// See https://github.com/mausch/SolrNet/blob/master/Documentation/Mapping.md
		[Field("url")]
		public string Url { get; set; }
		[Field("title")]
		public string Title { get; set; }
		[Field("content")]
		public string Content { get; set; }
		[Field("language")]
		public string Language { get; set; }
		[Field("contentLength")]
		public string ContentLength { get; set; }
		[Field("type")]
		public string Type { get; set; }
		[Field("date")]
		public DateTime? Date { get; set; }
		[Field("*")]
		public IDictionary<string, object> OtherFields { get; set; }
	}
}
