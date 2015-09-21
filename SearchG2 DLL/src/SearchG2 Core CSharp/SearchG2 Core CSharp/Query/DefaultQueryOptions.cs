using System.Collections.Generic;
using CrownPeak.SearchG2.Common;
using SolrNet.Commands.Parameters;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Default query options to use when querying CrownPeak SearchG2.
	/// </summary>
	internal static class DefaultQueryOptions
	{
		/// <summary>
		/// Basic options that will be applied when running a <see cref="CrownPeak.SearchG2.Search{T}"/>
		/// or a <see cref="CrownPeak.SearchG2.PlainSearch"/> if no other options are supplied.
		/// </summary>
		public static SolrNet.Commands.Parameters.QueryOptions BasicOptions
		{
			get
			{
				var parms = new List<KeyValuePair<string, string>>()
				{
						new KeyValuePair<string, string>("defType", "edismax"),
#if DEBUG
						new KeyValuePair<string, string>("echoParams", "explicit"),
#else
						new KeyValuePair<string, string>("echoParams", "none"),
#endif
				};
				var qf = AppConfiguration.ReadString("query.fields");
				if (!string.IsNullOrEmpty(qf))
				{
					parms.Add(new KeyValuePair<string, string>("qf", qf));
				}

				return new SolrNet.Commands.Parameters.QueryOptions()
				{
					Rows = AppConfiguration.ReadInt("result.rows", 10),
					Start = 0,
					Fields = new[] { "*", "score" },
					ExtraParams = parms
				};
			}
		}

		/// <summary>
		/// Facet parameters that will be applied when running a 
		/// <see cref="CrownPeak.SearchG2.Search{T}"/>or a 
		/// <see cref="CrownPeak.SearchG2.PlainSearch"/> if faceting is enabled but 
		/// no other options are supplied.
		/// </summary>
		public static FacetParameters FacetParameters
		{
			get
			{
				return new FacetParameters()
				{
					Limit = AppConfiguration.ReadInt("result.facets.limit", 5)
				};
			}
		}

		/// <summary>
		/// Highlighting parameters that will be applied when running a 
		/// <see cref="CrownPeak.SearchG2.Search{T}"/>or a 
		/// <see cref="CrownPeak.SearchG2.PlainSearch"/> if highlighting is enabled but 
		/// no other options are supplied.
		/// </summary>
		public static HighlightingParameters HighlightingParameters
		{
			get
			{
				return new HighlightingParameters()
				{
					Fields = new[] { "*" }, //"url", "title", "content*" },
					BeforeTerm = AppConfiguration.ReadString("result.highlight.prefix", "<b>"),
					AfterTerm = AppConfiguration.ReadString("result.highlight.suffix", "</b>"),
					Fragsize = AppConfiguration.ReadInt("result.highlight.fragmentsize", 100),
					Snippets = AppConfiguration.ReadInt("result.highlight.snippets", 3),
				};
			}
		}

		/// <summary>
		/// Extra parameters that will be applied when running a 
		/// <see cref="CrownPeak.SearchG2.Search{T}"/>or a 
		/// <see cref="CrownPeak.SearchG2.PlainSearch"/> if highlighting is enabled.
		/// </summary>
		public static IEnumerable<KeyValuePair<string, string>> HighlightingExtraParameters
		{
			get
			{
				return new[]
				{
					// We want the whole title and url highlighted, not snippets
					// See http://wiki.apache.org/solr/HighlightingParameters
					new KeyValuePair<string, string>("f.title.hl.fragsize", "50000"),
					new KeyValuePair<string, string>("f.url.hl.fragsize", "50000"),
				};
			}
		}

		/// <summary>
		/// SpellChecking parameters that will be applied when running a 
		/// <see cref="CrownPeak.SearchG2.Search{T}"/>or a 
		/// <see cref="CrownPeak.SearchG2.PlainSearch"/> if spellchecking is enabled but 
		/// no other options are supplied.
		/// </summary>
		public static SpellCheckingParameters SpellCheckingParameters
		{
			get
			{
				return new SpellCheckingParameters()
				{
					Count = AppConfiguration.ReadInt("spellcheck.count", 3),
					Collate = AppConfiguration.ReadBoolean("spellcheck.collate", true),
					OnlyMorePopular = AppConfiguration.ReadBoolean("spellcheck.onlymorepopular", true)
				};
			}
		}

		/// <summary>
		/// Extra parameters that will be applied when running a 
		/// <see cref="CrownPeak.SearchG2.Search{T}"/>or a 
		/// <see cref="CrownPeak.SearchG2.PlainSearch"/> if spellchecking is enabled.
		/// </summary>
		public static IEnumerable<KeyValuePair<string, string>> SpellCheckingExtraParameters
		{
			get
			{
				return new[]
				{
					new KeyValuePair<string, string>("spellcheck", "on"),
//					new KeyValuePair<string, string>("spellcheck.collate", "true"),
//					new KeyValuePair<string, string>("spellcheck.collateExtendedResults", "true")
				};
			}
		}
	}
}
