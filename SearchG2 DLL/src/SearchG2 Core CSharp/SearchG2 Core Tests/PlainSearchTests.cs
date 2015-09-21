using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Exceptions;
using CrownPeak.SearchG2.Query;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolrNet;
using SolrNet.Impl;
using SolrNet.Utils;
using SortOrder = CrownPeak.SearchG2.Query.SortOrder;

namespace CrownPeak.SearchG2.Tests
{
	[TestClass]
	public class PlainSearchTests
	{
		private PlainSearch PlainSearch;
		private string _endpoint = ""; // "https://searchg2-restricted.crownpeak.net/";
		private X509Certificate _certificate = null; // CertificateCreator.LoadCertificate("E63D2DCEB03E981968F373F5C419E043D9394AF9");

		// TODO: Badly need some static data to test this

		[TestInitialize]
		public void SetupSearch()
		{
			//ClearContainer();
			//try
			//{
			//	var container = Startup.Container;
			//	container.RemoveAll<IEndPointConfiguration>();
			//	container.Register<IEndPointConfiguration>(c => new TestEndPointConfiguration());
			//	ServiceLocator.SetLocatorProvider(() => container);
			//}
			//catch (Exception)
			//{
			//	// Ignore
			//}
			PlainSearch = new PlainSearch(new Settings("www.crownpeak.com", _endpoint, _certificate)); // "www.crownpeak.com"));
		}

		[TestMethod]
		public void QueryAllFindsResults()
		{
			var results = PlainSearch.Execute();
			Assert.IsTrue(results.Count > 0, "No results found");
			Assert.IsTrue(results.Count == 10, "Too few/many results returned, expected 10 found " + results.Count);
			Assert.IsTrue(results.TotalCount == 182, "Too few/many results in total, expected 182 found " + results.TotalCount);
			Assert.IsTrue(results[0].ContainsKey("id"), "No id field returned");
		}

		[TestMethod]
		public void TestQueryFindsResults()
		{
			var results = PlainSearch.Execute("crownpeak");
			Assert.IsTrue(results.Count == 10, "Expected 10 results, got " + results.Count);
		}

		[TestMethod]
		public void BadgerQueryFindsNoResults()
		{
			var results = PlainSearch.Execute("badger");
			Assert.IsTrue(results.Count == 0, "Expected no results, got " + results.Count);
		}

		[TestMethod]
		public void RowSettingLimitsRows()
		{
			var results = PlainSearch.Execute("*:*", new QueryOptions() {Rows = 5});
			Assert.IsTrue(results.Count == 5, "Too few/many results returned");
		}

		[TestMethod]
		public void HighlightsSearchFindsHighlights()
		{
			var results = PlainSearch.Execute("tedious", new QueryOptions() { Rows = 1, Highlighting = true });
			Assert.IsTrue(results.Values.Count(r => r["url"].Highlights.Length > 0) == 0);
			Assert.IsTrue(results.Values.Count(r => r["content"].Highlights.Length > 0) == 1);
		}

		[TestMethod]
		public void FacetingWorksOnUrl()
		{
			var results = PlainSearch.Execute("crownpeak", new QueryOptions() { Rows = 10, FacetFields = new[] { "url" }});
			Assert.IsTrue(results.Facets["url"].Count == 5, "Expected 5 facets, found " + results.Facets["url"].Count);
			Assert.IsTrue(results.Facets["url"].First().Value == "com", "Expected first facet = com, found = " + results.Facets["url"].First().Value);
			Assert.IsTrue(results.Facets["url"].First().Count == 182, "Expected first facet count 182, found " + results.Facets["url"].First().Count);
		}

		[TestMethod]
		public void FilterQueriesLimitResults()
		{
			var fq = new FilterQueryCollection();
			fq.Add(new FilterQuery("url", "blog"));
			var results = PlainSearch.Execute("crownpeak", new QueryOptions() { Rows = 10, FilterQueries = fq });
			Assert.IsTrue(results.TotalCount == 87, "Expected 87 results, found " + results.TotalCount);
		}

		[TestMethod]
		public void OrderByOrdersResults()
		{
			var results = PlainSearch.Execute("crownpeak", new QueryOptions() { Rows = 1, OrderBy = new[] { new SortOrder("title", SortDirection.Asc), } });
			Assert.IsTrue(results[0]["url"].ToString() == "http://www.crownpeak.com/About-Us.aspx", "Unexpected first page found, got " + results[0]["url"].ToString());
			results = PlainSearch.Execute("crownpeak", new QueryOptions() { Rows = 1, OrderBy = new[] { new SortOrder("title", SortDirection.Desc), } });
			Assert.IsTrue(results[0]["url"].ToString() == "http://www.crownpeak.com/Blog/Tags/Enterprise-I.T.-Issues.aspx", "Unexpected last page found, got " + results[0]["url"].ToString());
		}
		
		[TestMethod]
		public void SpellCheckFindsOptions()
		{
			var results = PlainSearch.Execute("terios", new QueryOptions() { Rows = 1, SpellCheck = true });
			Assert.IsTrue(results.Suggestions.Count == 1, "Expected 1 results, got " + results.Suggestions.Count);
			Assert.IsTrue(results.Suggestions[0].Options.Length == 2, "Expected 1 option, got " + results.Suggestions[0].Options.Length);
			Assert.IsTrue(results.Suggestions[0].Options[0] == "tedious", "Expected option = tedious, got " + results.Suggestions[0].Options[0]);
			Assert.IsTrue(results.Suggestions.Collation == "tedious");
		}

		[TestMethod]
		[ExpectedException(typeof(SearchG2ConnectionException), "An exception should have been thrown but was not")]
		public void InvalidCollectionReturnsError()
		{
			//try
			//{
			//	ClearContainer();
			//	var container = Startup.Container;
			//	container.Register<IEndPointConfiguration>(c => new TestEndPointConfiguration());
			//	ServiceLocator.SetLocatorProvider(() => container);
			//}
			//catch (Exception)
			//{
			//	// Ignore
			//}
			try
			{
				var s = new PlainSearch(new Settings("a-collection-that-does-not-exist"));
				var results = s.Execute("crownpeak");
				Assert.IsTrue(results.Count == 10, "Expected 10 results, got " + results.Count);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				// Restore the state
				SetupSearch();
			}
		}

		[TestMethod]
		[ExpectedException(typeof(SearchG2TimeoutException), "An exception should have been thrown but was not")]
		public void TimingOutReturnsTimeoutError()
		{
			//try
			//{
			//	ClearContainer();
			//	var container = Startup.Container;
			//	container.Register<IEndPointConfiguration>(c => new TimeoutTestEndPointConfiguration());
			//	ServiceLocator.SetLocatorProvider(() => container);
			//}
			//catch (Exception)
			//{
			//	// Ignore
			//}
			try
			{
				var s = new PlainSearch(new Settings("www.crownpeak.com", _endpoint, 1));
				var results = s.Execute("crownpeak");
				Assert.IsTrue(results.Count == 10, "Expected 10 results, got " + results.Count);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				// Restore the state
				SetupSearch();
			}
		}

		[TestMethod]
		[ExpectedException(typeof(SearchG2TimeoutException), "An exception should have been thrown but was not")]
		public void QueryTimeoutReturnsTimeoutError()
		{
			var results = PlainSearch.Execute("crownpeak", new QueryOptions()
			{
				Timeout = 1,
				Rows = 100
			});
			if (results.TimeTaken > 1) throw new SearchG2TimeoutException("The operation has timed out");
			Assert.IsTrue(results.Count < 10, "Expected fewer than 98 results, got " + results.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(SearchG2QueryException), "An exception should have been thrown but was not")]
		public void InvalidQueryReturnsQueryError()
		{
			var results = PlainSearch.Execute("something[broken)", new QueryOptions()
			{
				ExtraParameters = new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("defType", "lucene")
				}
			});
			Assert.IsTrue(results.Count == 10, "Expected 10 results, got " + results.Count);
		}

		[TestMethod]
		public void QueryReturnsLoggingId()
		{
			var results = PlainSearch.Execute("crownpeak");
			Assert.IsTrue(results.CrownPeak.Logging.Id != string.Empty, "Expected to get a logging id");
			Assert.IsTrue(results.CrownPeak.Logging.Id.Length == 36, "Expected to get a 36 character logging id");
			Assert.IsTrue(new Regex("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}").IsMatch(results.CrownPeak.Logging.Id),
				"Expected to get a logging id that looks like a uuid");
		}

		[TestMethod]
		public void QueryWithNoLoggingReturnsNoLoggingId()
		{
			var results = PlainSearch.Execute("crownpeak", new QueryOptions()
			{
				LoggingEnabled = false
			});
			Assert.IsNull(results.CrownPeak, "Expected to get a null results.CrownPeak");
		}

		[TestMethod]
		public void QueryReturnsMatchingLoggingId()
		{
			var results = PlainSearch.Execute("crownpeak");
			Assert.IsTrue(results.CrownPeak.Logging.Id != string.Empty, "Expected to get a logging id");
			Assert.IsTrue(results.CrownPeak.Logging.Id.Length == 36, "Expected to get a 36 character logging id");
			Assert.IsTrue(new Regex("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}").IsMatch(results.CrownPeak.Logging.Id),
				"Expected to get a logging id that looks like a uuid");
			string loggingId = results.CrownPeak.Logging.Id;

			results = PlainSearch.Execute("crownpeak2", new QueryOptions()
			{
				LoggingEnabled = true,
				LoggingId = loggingId
			});
			Assert.IsTrue(results.CrownPeak.Logging.Id != string.Empty, "Expected to get a logging id second time");
			Assert.IsTrue(results.CrownPeak.Logging.Id.Length == 36, "Expected to get a 36 character logging id second time");
			Assert.IsTrue(new Regex("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}").IsMatch(results.CrownPeak.Logging.Id),
				"Expected to get a logging id that looks like a uuid second time");
			Assert.IsTrue(results.CrownPeak.Logging.Id == loggingId, "Expected secod logging id to equal the first one");
		}

		[TestMethod]
		[ExpectedException(typeof(SearchG2ConnectionException), "An exception should have been thrown but was not")]
		public void RestrictedCollectionFailsWithoutCertificate()
		{
			var Search2 = new PlainSearch(new Settings("authoring-hss.cp-access.com", "https://searchg2-restricted.crownpeak.net/", null));
			var results = Search2.Execute("*:*");
			Assert.AreEqual(results.TotalCount, 0, "Expected 0 results, found " + results.TotalCount);
		}

		[TestMethod]
		public void RestrictedCollectionWorksWithCertificate()
		{
			var Search2 = new PlainSearch(new Settings("authoring-hss.cp-access.com", "https://searchg2-restricted.crownpeak.net/", CertificateCreator.LoadCertificate("E63D2DCEB03E981968F373F5C419E043D9394AF9")));
			var results = Search2.Execute("*:*");
			Assert.AreEqual(results.TotalCount, 13, "Expected 13 results, found " + results.TotalCount);
		}

		private void ClearContainer()
		{
			var container = Startup.Container;
			container.RemoveAll<IEndPointConfiguration>();
			container.RemoveAll<ISolrConnection>();
			container.RemoveAll<ISolrOperations<Dictionary<string, object>>>();
			container.RemoveAll<ISolrDocumentActivator<Dictionary<string, object>>>();
			container.RemoveAll<ISolrDocumentResponseParser<Dictionary<string, object>>>();
			container.RemoveAll<ISolrAbstractResponseParser<Dictionary<string, object>>>();
			container.RemoveAll<ISolrMoreLikeThisHandlerQueryResultsParser<Dictionary<string, object>>>();
			container.RemoveAll<ISolrQueryExecuter<Dictionary<string, object>>>();
			container.RemoveAll<ISolrDocumentSerializer<Dictionary<string, object>>>();
			container.RemoveAll<ISolrBasicOperations<Dictionary<string, object>>>();
			container.RemoveAll<ISolrBasicReadOnlyOperations<Dictionary<string, object>>>();
			container.RemoveAll<ISolrReadOnlyOperations<Dictionary<string, object>>>();
			container.RemoveAll<ISolrCoreAdmin>();
			ServiceLocator.SetLocatorProvider(() => container);
		}
	}
}
