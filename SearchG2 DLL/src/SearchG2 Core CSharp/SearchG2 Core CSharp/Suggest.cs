using System;
using System.Collections.Generic;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Query;
using CrownPeak.SearchG2.Result;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Impl;

namespace CrownPeak.SearchG2
{
	/// <summary>
	/// Class to support a Suggest search on the CrownPeak SearchG2 infrastructure
	/// </summary>
	public class Suggest
	{
		private static bool _solrInitialized;
		private static Settings _settings;
		private static SolrQueryExecuter<Dictionary<string, object>> _queryExecuter;

		/// <summary>
		/// Create a new <see cref="Suggest"/> object with the specified settings
		/// </summary>
		/// <param name="settings">The <see cref="Settings"/> to be used</param>
		public Suggest(Settings settings)
		{
			_settings = settings;
		}

		private static ICrownPeakOperations<Dictionary<string, object>> Solr
		{
			get
			{
				var solr = SolrInitializer<Dictionary<string, object>>.Solr(_settings.Collection, _settings.Endpoint, _settings.Timeout, _settings.Certificate);

				var container = Startup.Container;

				container.RemoveAll<ICrownPeakQueryExecuter<Dictionary<string, object>>>();
				container.Register<ICrownPeakQueryExecuter<Dictionary<string, object>>>(c => new CrownPeakQueryExecuter<Dictionary<string, object>>(
					c.GetInstance<ICrownPeakResponseParser<Dictionary<string, object>>>(),
					c.GetInstance<ISolrConnection>(),
					c.GetInstance<ISolrQuerySerializer>(),
					c.GetInstance<ISolrFacetQuerySerializer>(),
					c.GetInstance<ISolrMoreLikeThisHandlerQueryResultsParser<Dictionary<string, object>>>())
				{
					Handler = "/suggest"
				});

				ServiceLocator.SetLocatorProvider(() => container);

				return ServiceLocator.Current.GetInstance<ICrownPeakOperations<Dictionary<string, object>>>();
			}
		}

		/// <summary>
		/// Execute a Suggest search that returns the results that start with the specified query text,
		/// using a default set of query options.
		/// </summary>
		/// <param name="q">The query string to execute. See 
		/// <see href="https://wiki.apache.org/solr/SolrQuerySyntax"/>.</param>
		/// <returns><see cref="SuggestionCollection"/></returns>
		public SuggestionCollection Execute(string q)
		{
			return Execute(q, DefaultQueryOptions.BasicOptions);
		}

		/// <summary>
		/// Execute a Suggest search that returns the results that start the specified query text,
		/// using a specified set of <see cref="QueryOptions"/>
		/// </summary>
		/// <param name="q">The query string to execute. See 
		/// <see href="https://wiki.apache.org/solr/SolrQuerySyntax"/>.</param>
		/// <param name="options">The <see cref="QueryOptions"/> to use for this query.</param>
		/// <returns><see cref="SuggestionCollection"/></returns>
		public SuggestionCollection Execute(string q, QueryOptions options)
		{
			return Execute(q, options.SolrOptions);
		}

		private SuggestionCollection Execute(string q, SolrNet.Commands.Parameters.QueryOptions options)
		{
			if (q == null) q = string.Empty;

			// Prepare our search string
			q = q.Trim();
			int pos = q.LastIndexOf(' ');
			string prefix = "";
			string queryTerm = q;
			if (pos >= 0)
			{
				prefix = q.Substring(0, pos + 1);
				queryTerm = q.Substring(pos + 1);
			}

			try
			{
				var suggestResults = Solr.Query(new SolrQuery(queryTerm), options);
				return new SuggestionCollection(suggestResults.SpellChecking, prefix);
			}
			catch (Exception ex)
			{
				Utils.ThrowException(ex);
				return null;
			}
		}
	}
}
