using System.Linq;
using SolrNet.Impl;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a single result suggestion from a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or a <see cref="CrownPeak.SearchG2.Search{T}"/>
	/// on a CrownPeak SearchG2 collection.
	/// </summary>
	public class Suggestion
	{
		/// <summary>The query to which this suggestion refers.</summary>
		public string Query { get; private set; }
		/// <summary>The starting offset of this query within the overall query string.</summary>
		public int StartOffset { get; private set; }
		/// <summary>The ending offset of this query within the overall query string.</summary>
		public int EndOffset { get; private set; }
		/// <summary>Array of options that this suggestion provides.</summary>
		public string[] Options { get; private set; }

		internal Suggestion(string query, int startOffset, int endOffset, string[] options, string queryPrefix)
		{
			if (queryPrefix == null) queryPrefix = string.Empty;

			Query = queryPrefix + query;
			StartOffset = queryPrefix.Length + startOffset;
			EndOffset = queryPrefix.Length + endOffset;
			Options = options.Select(o => queryPrefix + o).ToArray();
		}

		internal Suggestion(string query, int startOffset, int endOffset, string[] options)
			: this(query, startOffset, endOffset, options, "")
		{ }

		internal Suggestion(SpellCheckResult suggestion)
			: this(suggestion.Query, suggestion.StartOffset, suggestion.EndOffset, suggestion.Suggestions.ToArray(), "")
		{ }

		internal Suggestion(SpellCheckResult suggestion, string prefix)
			: this(suggestion.Query, suggestion.StartOffset, suggestion.EndOffset, suggestion.Suggestions.ToArray(), prefix)
		{ }
	}
}
