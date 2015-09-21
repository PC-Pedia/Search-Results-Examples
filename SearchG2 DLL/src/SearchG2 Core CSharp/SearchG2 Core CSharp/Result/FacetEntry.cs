using System.Collections.Generic;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a single facet result entry from a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or a <see cref="CrownPeak.SearchG2.Search{T}"/>
	/// on a CrownPeak SearchG2 collection.
	/// </summary>
	public class FacetEntry
	{
		/// <summary>The key (field name) of this <see cref="FacetEntry"/>.</summary>
		public string Key { get; internal set; }
		/// <summary>The value of this <see cref="FacetEntry"/>.</summary>
		public string Value { get; internal set; }
		/// <summary>The count of instances of this <see cref="FacetEntry"/>.</summary>
		public int Count { get; internal set; }

		internal FacetEntry(string key, KeyValuePair<string, int> facet)
		{
			Key = key;
			Value = facet.Key;
			Count = facet.Value;
		}
	}
}
