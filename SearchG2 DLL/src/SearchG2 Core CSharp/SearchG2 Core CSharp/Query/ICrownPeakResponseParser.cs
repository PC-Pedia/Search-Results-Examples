using System.Xml.Linq;
using CrownPeak.SearchG2.Result;
using SolrNet.Impl;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Parses a chunk of a query response
	/// </summary>
	/// <typeparam name="T">Document type</typeparam>
	interface ICrownPeakResponseParser<T> : ISolrResponseParser<T>
	{
		/// <summary>
		/// Parses a chunk of a query response into the results object
		/// </summary>
		/// <param name="xml">query response</param>
		/// <param name="results">results object</param>
		void Parse(XDocument xml, CrownPeakQueryResults<T> results);
	}
}
