namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Interface to describe a single result from a <see cref="CrownPeak.SearchG2.Search{T}"/> 
	/// operation.
	/// </summary>
	public interface IResult
	{
		/// <summary>The ordinal (index) of this item within the full set of results.</summary>
		int Ordinal { get; }
		/// <summary>The unique id of this result.</summary>
		string Id { get; }
		/// <summary>The score of this result. Will be populated in results, but is optional for update.</summary>
		double? Score { get; }
		/// <summary>
		/// The score of this result, normalized so that the highest score in any result will
		/// always result in a normalized score of 1.
		/// </summary>
		double NormalizedScore { get; }
		/// <summary>Collection of <see cref="Highlight"/> objects for this result.</summary>
		HighlightCollection Highlights { get; }
	}
}
