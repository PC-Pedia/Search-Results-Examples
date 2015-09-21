using CrownPeak.SearchG2.Result;
using SolrNet;
using SolrNet.Impl;
using SolrNet.Schema;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Implements the basic Solr operations
	/// </summary>
	/// <typeparam name="T">Document type</typeparam>
	internal class CrownPeakBasicServer<T> : SolrBasicServer<T>, ICrownPeakBasicOperations<T>
	{
		private readonly ICrownPeakQueryExecuter<T> _queryExecuter;

		/// <summary>
		/// Execute the query and return the results
		/// </summary>
		/// <param name="query"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public new CrownPeakQueryResults<T> Query(ISolrQuery query, SolrNet.Commands.Parameters.QueryOptions options)
		{
			return _queryExecuter.Execute(query, options);
		}

		/// <summary>
		/// Make a new CrownPeakBasicServer instance
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="queryExecuter"></param>
		/// <param name="documentSerializer"></param>
		/// <param name="schemaParser"></param>
		/// <param name="headerParser"></param>
		/// <param name="querySerializer"></param>
		/// <param name="dihStatusParser"></param>
		/// <param name="extractResponseParser"></param>
		public CrownPeakBasicServer(ISolrConnection connection, ICrownPeakQueryExecuter<T> queryExecuter, ISolrDocumentSerializer<T> documentSerializer, ISolrSchemaParser schemaParser, ISolrHeaderResponseParser headerParser, ISolrQuerySerializer querySerializer, ISolrDIHStatusParser dihStatusParser, ISolrExtractResponseParser extractResponseParser)
			: base(connection, queryExecuter, documentSerializer, schemaParser, headerParser, querySerializer, dihStatusParser, extractResponseParser)
		{
			_queryExecuter = queryExecuter;
		}
	}
}
