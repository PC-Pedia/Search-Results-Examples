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
using SortOrder = CrownPeak.SearchG2.Query.SortOrder;

namespace CrownPeak.SearchG2.Tests
{
	[TestClass]
	public class GenericSearchTests
	{
		private Search<TestDocument> Search;
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
			//	container.Register<IEndPointConfiguration>(c => new TestEndPointConfiguration());
			//	ServiceLocator.SetLocatorProvider(() => container);
			//}
			//catch (Exception)
			//{
			//	// Ignore
			//}

			Search = new Search<TestDocument>(new Settings("www.crownpeak.com", _endpoint, _certificate));
		}

		[TestMethod]
		public void QueryAllFindsResults()
		{
			var results = Search.Execute();
			Assert.IsTrue(results.Count > 0, "No results found");
			Assert.IsTrue(results.Count == 10, "Too few/many results returned, expected 10 found " + results.Count);
			Assert.IsTrue(results.TotalCount == 182, "Too few/many results in total, expected 182 found " + results.TotalCount);
			Assert.IsNotNull(results[0].Id, "No id returned");
			Assert.IsNotNull(results[0].url, "No url returned");
			Assert.IsNotNull(results[0].Title, "No title returned");
			Assert.IsNotNull(results[0].Content, "No content returned");
			Assert.IsNotNull(results[0].Type, "No type returned");
		}

		[TestMethod]
		public void TestQueryFindsResults()
		{
			var results = Search.Execute("crownpeak");
			Assert.IsTrue(results.Count == 10, "Expected 10 results, got " + results.Count);
		}

		[TestMethod]
		public void BadgerQueryFindsNoResults()
		{
			var results = Search.Execute("badger");
			Assert.IsTrue(results.Count == 0, "Expected no results, got " + results.Count);
		}

		[TestMethod]
		public void RowSettingLimitsRows()
		{
			var results = Search.Execute("*:*", new QueryOptions() { Rows = 5 });
			Assert.IsTrue(results.Count == 5, "Too few/many results returned");
		}

		[TestMethod]
		public void HighlightsSearchFindsHighlights()
		{
			var results = Search.Execute("tedious", new QueryOptions() { Rows = 1, Highlighting = true });
			Assert.IsTrue(results.Values.Count(r => r.Highlights["url"] == null) == 1);
			Assert.IsTrue(results.Values.Count(r => r.Highlights["content"].Count > 0) == 1);
		}

		[TestMethod]
		public void FacetingWorksOnUrl()
		{
			var results = Search.Execute("crownpeak", new QueryOptions() { Rows = 10, FacetFields = new[] { "url" } });
			Assert.IsTrue(results.Facets["url"].Count == 5, "Expected 5 facets, found " + results.Facets["url"].Count);
			Assert.IsTrue(results.Facets["url"].First().Value == "com", "Expected first facet = com, found = " + results.Facets["url"].First().Value);
			Assert.IsTrue(results.Facets["url"].First().Count == 182, "Expected first facet count 182, found " + results.Facets["url"].First().Count);
		}

		[TestMethod]
		public void FilterQueriesLimitResults()
		{
			var fq = new FilterQueryCollection()
			{
				new FilterQuery("url", "blog")
			};
			var results = Search.Execute("crownpeak", new QueryOptions() { Rows = 10, FilterQueries = fq });
			Assert.IsTrue(results.TotalCount == 87, "Expected 87 results, found " + results.TotalCount);
		}

		[TestMethod]
		public void OrderByOrdersResults()
		{
			var results = Search.Execute("crownpeak", new QueryOptions() { Rows = 1, OrderBy = new[] { new SortOrder("title", SortDirection.Asc), } });
			Assert.IsTrue(results[0].url == "http://www.crownpeak.com/About-Us.aspx", "Unexpected first page found, got " + results[0].url);
			results = Search.Execute("crownpeak", new QueryOptions() { Rows = 1, OrderBy = new[] { new SortOrder("title", SortDirection.Desc), } });
			Assert.IsTrue(results[0].url == "http://www.crownpeak.com/Blog/Tags/Enterprise-I.T.-Issues.aspx", "Unexpected last page found, got " + results[0].url);
		}

		[TestMethod]
		public void SpellCheckFindsOptions()
		{
			var results = Search.Execute("terios", new QueryOptions() { Rows = 1, SpellCheck = true });
			Assert.IsTrue(results.Suggestions.Count == 1, "Expected 1 results, got " + results.Suggestions.Count);
			Assert.IsTrue(results.Suggestions[0].Options.Length == 2, "Expected 2 option, got " + results.Suggestions[0].Options.Length);
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
				var s = new Search<TestDocument>(new Settings("a-collection-that-does-not-exist"));
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
			try
			{
				ClearContainer();
				var container = Startup.Container;
				container.Register<IEndPointConfiguration>(c => new TimeoutTestEndPointConfiguration());
				ServiceLocator.SetLocatorProvider(() => container);
			}
			catch (Exception)
			{
				// Ignore
			}
			try
			{
				var s = new Search<TestDocument>(new Settings("www.crownpeak.com", _endpoint, 1));
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
			var results = Search.Execute("crownpeak", new QueryOptions()
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
			var results = Search.Execute("something[broken)", new QueryOptions()
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
			var results = Search.Execute("crownpeak");
			Assert.IsTrue(results.CrownPeak.Logging.Id != string.Empty, "Expected to get a logging id");
			Assert.IsTrue(results.CrownPeak.Logging.Id.Length == 36, "Expected to get a 36 character logging id");
			Assert.IsTrue(new Regex("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}").IsMatch(results.CrownPeak.Logging.Id),
				"Expected to get a logging id that looks like a uuid");
		}

		[TestMethod]
		public void QueryWithNoLoggingReturnsNoLoggingId()
		{
			var results = Search.Execute("crownpeak", new QueryOptions()
			{
				LoggingEnabled = false
			});
			Assert.IsNull(results.CrownPeak, "Expected results.CrownPeak to be null");
		}

		[TestMethod]
		public void QueryReturnsMatchingLoggingId()
		{
			var results = Search.Execute("crownpeak");
			Assert.IsTrue(results.CrownPeak.Logging.Id != string.Empty, "Expected to get a logging id");
			Assert.IsTrue(results.CrownPeak.Logging.Id.Length == 36, "Expected to get a 36 character logging id");
			Assert.IsTrue(new Regex("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}").IsMatch(results.CrownPeak.Logging.Id),
				"Expected to get a logging id that looks like a uuid");
			string loggingId = results.CrownPeak.Logging.Id;

			results = Search.Execute("crownpeak2", new QueryOptions()
			{
				LoggingId = loggingId
			});
			Assert.IsTrue(results.CrownPeak.Logging.Id != string.Empty, "Expected to get a logging id second time");
			Assert.IsTrue(results.CrownPeak.Logging.Id.Length == 36, "Expected to get a 36 character logging id second time");
			Assert.IsTrue(new Regex("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}").IsMatch(results.CrownPeak.Logging.Id),
				"Expected to get a logging id that looks like a uuid second time");
			Assert.IsTrue(results.CrownPeak.Logging.Id == loggingId, "Expected second logging id to equal the first one");
		}

		[TestMethod]
		[ExpectedException(typeof(SearchG2ConnectionException), "An exception should have been thrown but was not")]
		public void RestrictedCollectionFailsWithoutCertificate()
		{
			var Search2 = new Search<TestDocument>(new Settings("authoring-hss.cp-access.com", "https://searchg2-restricted.crownpeak.net/", null));
			var results = Search2.Execute("*:*");
			Assert.AreEqual(results.TotalCount, 0, "Expected 0 results, found " + results.TotalCount);
		}

		[TestMethod]
		public void RestrictedCollectionWorksWithCertificate()
		{
			var Search2 = new Search<TestDocument>(new Settings("authoring-hss.cp-access.com", "https://searchg2-restricted.crownpeak.net/", CertificateCreator.LoadCertificate("E63D2DCEB03E981968F373F5C419E043D9394AF9")));
			var results = Search2.Execute("*:*");
			Assert.AreEqual(results.TotalCount, 13, "Expected 13 results, found " + results.TotalCount);
		}

		private void ClearContainer()
		{
			var container = Startup.Container;
			container.RemoveAll<IEndPointConfiguration>();
			container.RemoveAll<ISolrConnection>();
			container.RemoveAll<ISolrOperations<TestDocument>>();
			container.RemoveAll<ISolrDocumentActivator<TestDocument>>();
			container.RemoveAll<ISolrDocumentResponseParser<TestDocument>>();
			container.RemoveAll<ISolrAbstractResponseParser<TestDocument>>();
			container.RemoveAll<ISolrMoreLikeThisHandlerQueryResultsParser<TestDocument>>();
			container.RemoveAll<ISolrQueryExecuter<TestDocument>>();
			container.RemoveAll<ISolrDocumentSerializer<TestDocument>>();
			container.RemoveAll<ISolrBasicOperations<TestDocument>>();
			container.RemoveAll<ISolrBasicReadOnlyOperations<TestDocument>>();
			container.RemoveAll<ISolrReadOnlyOperations<TestDocument>>();
			container.RemoveAll<ISolrCoreAdmin>();
			ServiceLocator.SetLocatorProvider(() => container);
		}
	}
}
