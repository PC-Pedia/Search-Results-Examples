using System.Collections.Generic;
using CrownPeak.SearchG2.Result;
using SolrNet;
using SolrNet.Impl;
using SolrNet.Mapping.Validation;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Main component to interact with CrownPeak Search G2
	/// </summary>
	/// <typeparam name="T">Document type</typeparam>
	internal class CrownPeakServer<T> : SolrServer<T>, ICrownPeakOperations<T>
	{
		private readonly ICrownPeakBasicOperations<T> _basicServer;

		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public new CrownPeakQueryResults<T> Query(string query)
		{
			return Query(new SolrQuery(query), new SolrNet.Commands.Parameters.QueryOptions());
		}

		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="query"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public new CrownPeakQueryResults<T> Query(string query, SolrNet.Commands.Parameters.QueryOptions options)
		{
			return Query(new SolrQuery(query), options);
		}

		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public new CrownPeakQueryResults<T> Query(ISolrQuery query)
		{
			return Query(query, new SolrNet.Commands.Parameters.QueryOptions());
		}

		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="q"></param>
		/// <param name="orders"></param>
		/// <returns></returns>
		public new CrownPeakQueryResults<T> Query(string q, ICollection<SolrNet.SortOrder> orders)
		{
			return Query(new SolrQuery(q), orders);
		}

		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="query"></param>
		/// <param name="orders"></param>
		/// <returns></returns>
		public new CrownPeakQueryResults<T> Query(ISolrQuery query, ICollection<SolrNet.SortOrder> orders)
		{
			return Query(query, new SolrNet.Commands.Parameters.QueryOptions { OrderBy = orders });
		}

		/// <summary>
		/// Executes a query
		/// </summary>
		/// <param name="query"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public new CrownPeakQueryResults<T> Query(ISolrQuery query, SolrNet.Commands.Parameters.QueryOptions options)
		{
			return _basicServer.Query(query, options);
		}

		/// <summary>
		/// Make a new CrownPeakServer instance
		/// </summary>
		/// <param name="basicServer"></param>
		/// <param name="mappingManager"></param>
		/// <param name="_schemaMappingValidator"></param>
		public CrownPeakServer(ICrownPeakBasicOperations<T> basicServer, IReadOnlyMappingManager mappingManager, IMappingValidator _schemaMappingValidator)
			: base(basicServer, mappingManager, _schemaMappingValidator)
		{
			_basicServer = basicServer;
		}
	}
}
