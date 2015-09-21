using System.Xml.Linq;
using CrownPeak.SearchG2.Result;
using SolrNet;
using SolrNet.Impl;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Executes queries
	/// </summary>
	/// <typeparam name="T">Document type</typeparam>
	internal class CrownPeakQueryExecuter<T> : SolrQueryExecuter<T>, ICrownPeakQueryExecuter<T>
	{
		private readonly ICrownPeakResponseParser<T> _parser;
		private readonly ISolrConnection _connection;

		/// <summary>
		/// Make a new CrownPeakQueryExecuter
		/// </summary>
		/// <param name="resultParser"></param>
		/// <param name="connection"></param>
		/// <param name="querySerializer"></param>
		/// <param name="facetQuerySerializer"></param>
		/// <param name="mlthResultParser"></param>
		public CrownPeakQueryExecuter(ISolrAbstractResponseParser<T> resultParser,
			ISolrConnection connection, ISolrQuerySerializer querySerializer,
			ISolrFacetQuerySerializer facetQuerySerializer,
			ISolrMoreLikeThisHandlerQueryResultsParser<T> mlthResultParser)
			: base(resultParser, connection, querySerializer, facetQuerySerializer, mlthResultParser)
		{
			_parser = resultParser as ICrownPeakResponseParser<T>;
			_connection = connection;
		}

		/// <summary>
		/// Executes the query and returns results
		/// </summary>
		/// <returns>query results</returns>
		public new CrownPeakQueryResults<T> Execute(ISolrQuery q, SolrNet.Commands.Parameters.QueryOptions options)
		{
			var param = GetAllParameters(q, options);
			var results = new CrownPeakQueryResults<T>();
			var r = _connection.Get(Handler, param);
			var xml = XDocument.Parse(r);
			_parser.Parse(xml, results);
			return results;
		}
	}
}
