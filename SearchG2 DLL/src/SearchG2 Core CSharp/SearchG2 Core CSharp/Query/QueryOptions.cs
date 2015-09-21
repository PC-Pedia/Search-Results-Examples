using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.SqlServer.Server;
using SolrNet;
using SolrNet.Mapping.Validation;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Class that wraps a set of query options to be used when executing a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or a <see cref="CrownPeak.SearchG2.Search{T}"/>
	/// on a CrownPeak SearchG2 collection.
	/// </summary>
	public class QueryOptions
	{
		/// <summary>The number of rows to return in a page of results. Defaults to 10.</summary>
		public int Rows { get; set; }
		/// <summary>The index of the first row to return. Defaults to 0.</summary>
		public int Start { get; set; }
		/// <summary>Whether to enable result hit highlighting. Defaults to false.</summary>
		public bool Highlighting { get; set; }
		/// <summary>Whether to enable spellchecking. Defaults to false.</summary>
		public bool SpellCheck { get; set; }
		/// <summary>A language filter query to be applied to the query. Defaults to empty string (no filter).</summary>
		public string Language { get; set; }
		/// <summary>A set of fields on which to facet. Defaults to null (no faceting).</summary>
		public IEnumerable<string> FacetFields { get; set; }
		/// <summary><see cref="FilterQueryCollection"/> to apply. Defaults to null (no filter).</summary>
		public FilterQueryCollection FilterQueries { get; set; }
		/// <summary>A set of <see cref="SortOrder"/> sort orders to apply. Defaults to Score DESC.</summary>
		public IEnumerable<SortOrder> OrderBy { get; set; }
		/// <summary>The timeout (in milliseconds) to allow for this query to execute. Defaults to 5000 (5 seconds).</summary>
		public int Timeout { get; set; }
		/// <summary>Whether to enable CrownPeak Search G2 Logging. Defaults to true.</summary>
		public bool LoggingEnabled { get; set; }
		/// <summary>The unique id to use this for this query. Defaults to empty string, so a new value will be created automatically.</summary>
		public string LoggingId { get; set; }
		/// <summary>Whether to enable CrownPeak Search G2 Logging of IP address. Defaults to true.</summary>
		public bool IpLoggingEnabled { get; set; }
		/// <summary>
		/// A collection of <see cref="KeyValuePair{String,String}"/> to apply in addition
		/// to the options specified above.
		/// </summary>
		public ICollection<KeyValuePair<string, string>> ExtraParameters { get; set; }

		/// <summary>
		/// Create a new <see cref="QueryOptions"/> with a default set of options.
		/// </summary>
		public QueryOptions()
		{
			Rows = DefaultQueryOptions.BasicOptions.Rows.Value;
			Start = 0;
			Timeout = 5000;
			Highlighting = false;
			SpellCheck = false;
			Language = string.Empty;
			LoggingEnabled = true;
			LoggingId = string.Empty;
			IpLoggingEnabled = true;
			ExtraParameters = new List<KeyValuePair<string, string>>();
		}

		/// <summary>
		/// Convert the current <see cref="QueryOptions"/> into a format for Solr queries.
		/// </summary>
		internal SolrNet.Commands.Parameters.QueryOptions SolrOptions
		{
			get
			{
				List<KeyValuePair<string, string>> parms = new List<KeyValuePair<string, string>>(ExtraParameters);

				SolrNet.Commands.Parameters.QueryOptions options = DefaultQueryOptions.BasicOptions;
				AddExtraParameters(ref parms, options.ExtraParams);
				options.Rows = Rows;
				options.Start = Start;

				if (Timeout > 0)
				{
					AddExtraParameters(ref parms, new [] { new KeyValuePair<string, string>("timeAllowed", Timeout.ToString() )});
				}

				if (Highlighting)
				{
					options.Highlight = DefaultQueryOptions.HighlightingParameters;
					AddExtraParameters(ref parms, DefaultQueryOptions.HighlightingExtraParameters);
				}

				if (FacetFields != null && FacetFields.Any())
				{
					options.Facet = DefaultQueryOptions.FacetParameters;
					options.Facet.Queries = FacetFields.Select(f => new SolrNet.SolrFacetFieldQuery(f) as ISolrFacetQuery).ToArray();
				}

				if (FilterQueries != null && FilterQueries.Any())
				{
					options.FilterQueries =
						FilterQueries.Select(f => new SolrQuery(f.ToString()) as ISolrQuery).ToArray();
				}

				if (OrderBy != null && OrderBy.Any())
				{
					options.OrderBy = OrderBy.Select(o => o.ToSolrSortOrder()).ToArray();
				}

				if (SpellCheck)
				{
					options.SpellCheck = DefaultQueryOptions.SpellCheckingParameters;
					AddExtraParameters(ref parms, DefaultQueryOptions.SpellCheckingExtraParameters);
				}

				if (!string.IsNullOrEmpty(Language))
				{
					options.FilterQueries.Add(new SolrQuery(string.Format("{0}:{1}", "language", Language)));
				}

				if (!LoggingEnabled)
				{
					parms.Add(new KeyValuePair<string, string>("cp.logging.enabled", "false"));
				}
				else
				{
					if (!IpLoggingEnabled)
					{
						parms.Add(new KeyValuePair<string, string>("cp.logging.storeip", "false"));
					}
					if (!string.IsNullOrWhiteSpace(LoggingId))
					{
						parms.Add(new KeyValuePair<string, string>("cp.logging.id", LoggingId));
					}
				}

				options.ExtraParams = parms;

				return options;
			}
		}


		private void AddExtraParameters(ref List<KeyValuePair<string, string>> parms, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			foreach (var p in parameters)
				parms.Add(p);
		}
	}
}
