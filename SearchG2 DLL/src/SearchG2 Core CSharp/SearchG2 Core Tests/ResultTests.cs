using System;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Query;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolrNet;

namespace CrownPeak.SearchG2.Tests
{
	[TestClass]
	public class ResultTests
	{
		private PlainSearch PlainSearch;
		private Search<TestDocument> Search;

		[TestInitialize]
		public void SetupSearch()
		{
			//try
			//{
			//	var container = Startup.Container;
			//	container.Register<IEndPointConfiguration>(c => new TestEndPointConfiguration());
			//	ServiceLocator.SetLocatorProvider(() => container);
			//}
			//catch (Exception)
			//{
			//	// Ignore
			//}
			PlainSearch = new PlainSearch(new Settings("www.crownpeak.com"));
			Search = new Search<TestDocument>(new Settings("www.crownpeak.com"));
		}

		[TestMethod]
		public void SearchAllFindsAllContent()
		{
			var results = PlainSearch.Execute();
			Assert.IsTrue(results.Count == 10, "Expected 10 results, found " + results.Count);
			Assert.IsTrue(results.TotalCount == 98, "Expected 98 total results, found " + results.TotalCount);
		}

		[TestMethod]
		public void GenericSearchAllFindsAllContent()
		{
			var results = Search.Execute();
			Assert.IsTrue(results.Count == 10, "Expected 10 results, found " + results.Count);
			Assert.IsTrue(results.TotalCount == 98, "Expected 98 total results, found " + results.TotalCount);
		}
	}
}
