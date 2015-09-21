using System.Linq;
using System.Xml.Linq;
using CrownPeak.SearchG2.Result;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Impl;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Parses a chunk of a query response
	/// </summary>
	/// <typeparam name="T">Document type</typeparam>
	internal class CrownPeakResponseParser<T> : ICrownPeakResponseParser<T>
	{
		/// <summary>
		/// Parses a chunk of a query response into the results object
		/// </summary>
		/// <param name="xml">query response</param>
		/// <param name="results">results object</param>
		public void Parse(XDocument xml, SolrQueryResults<T> results)
		{
			var parser = ServiceLocator.Current.GetInstance<ISolrResponseParser<T>>();
			parser.Parse(xml, results);
		}

		/// <summary>
		/// Parses a chunk of a query response into the results object
		/// </summary>
		/// <param name="xml">query response</param>
		/// <param name="results">results object</param>
		public void Parse(XDocument xml, AbstractSolrQueryResults<T> results)
		{
			var parser = ServiceLocator.Current.GetInstance<ISolrAbstractResponseParser<T>>();
			parser.Parse(xml, results);
		}

		/// <summary>
		/// Parses a chunk of a query response into the results object
		/// </summary>
		/// <param name="xml">query response</param>
		/// <param name="results">results object</param>
		public void Parse(XDocument xml, CrownPeakQueryResults<T> results)
		{
			// Call out to the base to do the initial parse of the results
			Parse(xml, results as AbstractSolrQueryResults<T>);

			// Get our logging id out of the results
			var responseElement = xml.Element("response");
			if (responseElement != null)
			{
				var lstElement = responseElement.Elements("lst").LastOrDefault();
				if (lstElement != null)
				{
					var strElement = lstElement.Element("str");
					if (strElement != null)
					{
						if (results.CrownPeak == null) results.CrownPeak = new CrownPeakResults(new CrownPeakLoggingResults());
						if (results.CrownPeak.Logging == null) results.CrownPeak.Logging = new CrownPeakLoggingResults();
						results.CrownPeak.Logging.Id = strElement.Value;
					}
				}
			}
		}
	}
}
