using System.Collections.Generic;
using CrownPeak.SearchG2.Result;
using SolrNet;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Consolidating interface, exposes all operations
	/// </summary>
	/// <typeparam name="T">Document type</typeparam>
	internal interface ICrownPeakOperations<T> : ISolrOperations<T>
	{
		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="q">query to execute</param>
		/// <returns>query results</returns>
		new CrownPeakQueryResults<T> Query(string q);

		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="q"></param>
		/// <param name="orders"></param>
		/// <returns></returns>
		new CrownPeakQueryResults<T> Query(string q, ICollection<SolrNet.SortOrder> orders);

		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="q"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		new CrownPeakQueryResults<T> Query(string q, SolrNet.Commands.Parameters.QueryOptions options);

		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="q"></param>
		/// <returns></returns>
		new CrownPeakQueryResults<T> Query(ISolrQuery q);

		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="query"></param>
		/// <param name="orders"></param>
		/// <returns></returns>
		new CrownPeakQueryResults<T> Query(ISolrQuery query, ICollection<SolrNet.SortOrder> orders);
	}
}
