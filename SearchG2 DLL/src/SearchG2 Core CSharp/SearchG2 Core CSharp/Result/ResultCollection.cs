using System;
using System.Collections.Generic;
using System.Linq;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a collection of <see cref="IResult"/> objects from a
	/// <see cref="Search{T}"/> on a CrownPeak SearchG2 collection.
	/// </summary>
	/// <typeparam name="T">The type of result for this collection. Must inherit from
	/// <see cref="ResultBase"/>.</typeparam>
	public class ResultCollection<T> : SearchCollection<T>, IResultCollection<T>
		where T : ResultBase
	{
		/// <summary>The starting ordinal for the collection of <see cref="IResult"/>.</summary>
		public int Start { get; internal set; }
		/// <summary>The total number of results that match this query.</summary>
		public int TotalCount { get; internal set; }
		/// <summary>The time taken (in ms) for the operation to execute.</summary>
		public int TimeTaken { get; internal set; }
		/// <summary>The maximum score for any of the results.</summary>
		public double? MaxScore { get; internal set; }
		/// <summary>Collection of <see cref="Suggestion"/> objects that are available.</summary>
		public SuggestionCollection Suggestions { get; internal set; }
		/// <summary>Collection of <see cref="Facet"/> objects that are available.</summary>
		public FacetCollection Facets { get; internal set; }
		/// <summary>Results that are specific to CrownPeak.</summary>
		public CrownPeakResults CrownPeak { get; internal set; }

		private ResultCollection()
		{
			Start = 0;
			TotalCount = 0;
			TimeTaken = 0;
			Suggestions = new SuggestionCollection();
			CrownPeak = null;
		}

		internal ResultCollection(CrownPeakQueryResults<T> results)
			: this(results, 0)
		{
		}

		internal ResultCollection(CrownPeakQueryResults<T> results, int start)
			: this()
		{
			Start = start;
			TotalCount = results.NumFound;
			TimeTaken = results.Header.QTime;
			CrownPeak = results.CrownPeak;

			var dict = new Dictionary<string, T>();
			int ordinal = start;
			foreach (var result in results)
			{
				result.Ordinal = ordinal++;
				if (results.Highlights != null)
				{
					result.Highlights = new HighlightCollection(results.Highlights[result.Id]);
				}
				if (results.MaxScore.HasValue && results.MaxScore.Value > 0 && result.Score.HasValue)
				{
					result.NormalizedScore = result.Score.Value / results.MaxScore.Value;
				}
				dict.Add(result.Id, result);
			}
			SetDictionary(dict);

			if (results.SpellChecking != null)
			{
				Suggestions = new SuggestionCollection(results.SpellChecking);
			}
			if (results.FacetFields != null)
			{
				Facets = new FacetCollection(results.FacetFields);
			}
		}
	}
}
