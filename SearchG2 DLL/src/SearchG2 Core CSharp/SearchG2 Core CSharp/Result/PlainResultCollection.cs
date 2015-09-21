using System.Collections.Generic;
using System.Linq;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a collection of <see cref="PlainResult"/> from a
	/// <see cref="PlainSearch"/> on a CrownPeak SearchG2 collection.
	/// </summary>
	public class PlainResultCollection : SearchCollection<PlainResult>, IResultCollection<PlainResult>
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

		private PlainResultCollection()
		{
			Start = 0;
			TotalCount = 0;
			TimeTaken = 0;
			Suggestions = new SuggestionCollection();
			CrownPeak = null;
		}

		internal PlainResultCollection(CrownPeakQueryResults<Dictionary<string, object>> results)
			: this(results, 0)
		{
		}

		internal PlainResultCollection(CrownPeakQueryResults<Dictionary<string, object>> results, int start)
			: this()
		{
			Start = start;
			TotalCount = results.NumFound;
			TimeTaken = results.Header.QTime;
			CrownPeak = results.CrownPeak;

			var dict = new Dictionary<string, PlainResult>();
			foreach (var result in results)
			{
				PlainResult r = new PlainResult(result);
				if (results.Highlights != null)
				{
					r.SetHighlights(results.Highlights[r.Id]);
				}
				dict.Add(r.Id, r);
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

			if (results.MaxScore.HasValue && results.MaxScore.Value > 0)
			{
				foreach (var result in results.Where(r => r.ContainsKey("score")))
				{
					result.Add("normalizedScore", double.Parse(result["score"].ToString()) / results.MaxScore.Value);
				}
			}
		}
	}
}
