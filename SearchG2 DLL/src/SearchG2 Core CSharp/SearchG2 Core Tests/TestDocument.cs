using System;
using System.Collections.Generic;
using System.Linq;
using CrownPeak.SearchG2.Attributes;

namespace CrownPeak.SearchG2.Tests
{
	public class TestDocument : CrownPeak.SearchG2.Result.ResultBase
	{
		[Field]
		public string url { get; set; }
		[Field("title")]
		public string Title { get; set; }
		[Field("content")]
		public string Content { get; set; }
		[Field("type")]
		public string Type { get; set; }
		[Field("*")]
		public IDictionary<string, object> OtherFields { get; set; }
	}
}
