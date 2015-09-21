using System.Collections.Generic;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Interface to describe a collection of results from a 
	/// <see cref="PlainSearch"/> or a 
	/// <see cref="Search{T}"/> operation.
	/// </summary>
	/// <typeparam name="T">The type of results that we wish to hold in this collection.</typeparam>
	public interface IResultCollection<T> : IReadOnlyDictionary<string, T>
	{
		/// <summary>The total number of results that match this query.</summary>
		int TotalCount { get; }
		/// <summary>The time taken (in ms) for the operation to execute.</summary>
		int TimeTaken { get; }
		/// <summary>The maximum score for any of the results.</summary>
		double? MaxScore { get; }
		/// <summary>Collection of <see cref="Facet"/> objects that are available.</summary>
		FacetCollection Facets { get; }
		/// <summary>Collection of <see cref="Suggestion"/> objects that are available.</summary>
		SuggestionCollection Suggestions { get; }
		/// <summary>Results that are specific to CrownPeak</summary>
		CrownPeakResults CrownPeak { get; }
	}
}
