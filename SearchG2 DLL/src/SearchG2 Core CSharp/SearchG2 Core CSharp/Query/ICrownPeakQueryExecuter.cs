using CrownPeak.SearchG2.Result;
using SolrNet;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Executable query
	/// </summary>
	/// <typeparam name="T">Document type</typeparam>
	internal interface ICrownPeakQueryExecuter<T> : ISolrQueryExecuter<T>
	{
		/// <summary>
		/// Executes the query and returns results
		/// </summary>
		/// <returns>query results</returns>
		new CrownPeakQueryResults<T> Execute(ISolrQuery q, SolrNet.Commands.Parameters.QueryOptions options);
	}
}
