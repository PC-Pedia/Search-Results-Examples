using CrownPeak.SearchG2.Result;
using SolrNet;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Solr operations without convenience overloads
	/// </summary>
	/// <typeparam name="T">Document type</typeparam>
	internal interface ICrownPeakBasicOperations<T> : ISolrBasicOperations<T>
	{
		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="query"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		new CrownPeakQueryResults<T> Query(ISolrQuery query, SolrNet.Commands.Parameters.QueryOptions options);
	}
}
