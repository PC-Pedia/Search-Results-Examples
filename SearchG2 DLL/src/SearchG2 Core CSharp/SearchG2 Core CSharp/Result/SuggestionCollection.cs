using System.Collections.Generic;
using System.Linq;
using SolrNet;
using SolrNet.Impl;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a collection of <see cref="Suggestion"/> results from a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or a <see cref="CrownPeak.SearchG2.Search{T}"/>
	/// on a CrownPeak SearchG2 collection.
	/// </summary>
	public class SuggestionCollection : SearchCollection<Suggestion>
	{
		/// <summary>The collation for this suggestion.</summary>
		public string Collation { get; internal set; }

		internal SuggestionCollection() : 
			this(new Suggestion[] {}, string.Empty)
		{ }

		private SuggestionCollection(IEnumerable<Suggestion> suggestions, string collation)
		{
			SetDictionary(suggestions.ToDictionary(s => s.Query, s => s));
			Collation = collation;
		}

		internal SuggestionCollection(SpellCheckResults results)
			: this(results, "")
		{ }

		internal SuggestionCollection(SpellCheckResults results, string prefix)
		{
			// Prefix may be provided by the Suggest class
			var suggestions = results.Select(result => new Suggestion(result, prefix));
			SetDictionary(suggestions.ToDictionary(s => prefix + s.Query, s => s));
			Collation = prefix + results.Collation;
		}
	}
}
