using System;
using System.Collections.Generic;
using CrownPeak.SearchG2.Attributes;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Provide a base implentation of the <see cref="IResult"/> interface. When using
	/// <see cref="CrownPeak.SearchG2.Search{T}"/> you should inherit from this
	/// class and provide your new class to the search.
	/// </summary>
	public class ResultBase : IResult
	{
		/// <summary>The unique id of this <see cref="IResult"/>.</summary>
		[UniqueKey("id")]
		public virtual string Id { get; set; }
		/// <summary>The score of this <see cref="IResult"/>. Will be populated in results, but is optional for update.</summary>
		[Field("score")]
		public virtual double? Score { get; set; }
		/// <summary>The ordinal (index) of this <see cref="IResult"/> within the full set of results.</summary>
		public virtual int Ordinal { get; internal set; }
		/// <summary>
		/// The score of this <see cref="IResult"/>, normalized so that the highest score in any 
		/// <see cref="ResultCollection{IResult}"/> will always result in a normalized score of 1.
		/// </summary>
		public virtual double NormalizedScore { get; internal set; }
		/// <summary>Collection of <see cref="Highlight"/> objects for this result.</summary>
		public virtual HighlightCollection Highlights { get; internal set; }
	}
}
