using System;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Exceptions;
using CrownPeak.SearchG2.Query;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolrNet;

namespace CrownPeak.SearchG2.Tests
{
	[TestClass]
	public class SuggestTests
	{
		// ------------------------------------------------------------------------------------
		// NOTE: these tests depend on which set of servers returns the results. Expect errors!
		// ------------------------------------------------------------------------------------

		private Suggest Suggest;

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

			Suggest = new Suggest(new Settings("www.crownpeak.com"));
		}

		[TestMethod]
		public void SingleWordSuggest()
		{
			var result = Suggest.Execute("cro");
			Assert.AreEqual(1, result.Count, "Expecting 1 result, got " + result.Count);
			Assert.IsTrue(result[0].Options.Length == 5, "Expecting 5 options, got " + result[0].Options.Length);
			Assert.IsTrue(result[0].Options[0] == "crownpeak", "Expecting \"crownpeak\" but found \"" + result[0].Options[0] + "\"");
			Assert.IsTrue(result[0].Options[1] == "crownpeak’s", "Expecting \"crownpeak’s\" but found \"" + result[0].Options[1] + "\"");
		}

		[TestMethod]
		public void TwoWordSuggest()
		{
			var result = Suggest.Execute("tedious cro");
			Assert.IsTrue(result.Count == 1, "Expecting 1 result, got " + result.Count);
			Assert.IsTrue(result[0].Options.Length == 5, "Expecting 5 options, got " + result[0].Options.Length);
			Assert.IsTrue(result[0].Options[0] == "tedious crownpeak", "Expecting \"tedious crownpeak\" but found \"" + result[0].Options[0] + "\"");
			Assert.IsTrue(result[0].Options[1] == "tedious crownpeak's", "Expecting \"tedious crownpeak's\" but found \"" + result[0].Options[1] + "\"");
		}

		[TestMethod]
		[ExpectedException(typeof(SearchG2ConnectionException), "An exception should have been thrown but was not")]
		public void RestrictedSuggestFailsWithoutCertificate()
		{
			var Suggest2 = new Suggest(new Settings("authoring-hss.cp-access.com", "https://searchg2-restricted.crownpeak.net/", null));
			var result = Suggest2.Execute("a");
			Assert.AreEqual(0, result.Count, "Expected 0 results, found " + result.Count);
		}

		[TestMethod]
		public void RestrictedSuggestWorksWithCertificate()
		{
			var Suggest2 = new Suggest(new Settings("authoring-hss.cp-access.com", "https://searchg2-restricted.crownpeak.net/", CertificateCreator.LoadCertificate("E63D2DCEB03E981968F373F5C419E043D9394AF9")));
			var result = Suggest2.Execute("a");
			Assert.AreEqual(1, result.Count, "Expected 4 results, found " + result.Count);
			Assert.AreEqual(4, result[0].Options.Length, "Expecting 4 options, got " + result[0].Options.Length);
		}


		[TestMethod]
		public void ZeroOutputSuggest()
		{
			var result = Suggest.Execute("bah");
			Assert.IsTrue(result.Count == 0, "Expecting no results, got " + result.Count);
		}
	}
}
