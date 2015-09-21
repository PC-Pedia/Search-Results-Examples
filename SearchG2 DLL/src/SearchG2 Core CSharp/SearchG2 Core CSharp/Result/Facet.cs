using System.Collections.Generic;
using System.Linq;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a single facet result from a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or a <see cref="CrownPeak.SearchG2.Search{T}"/>
	/// on a CrownPeak SearchG2 collection.
	/// </summary>
	public class Facet : IReadOnlyList<FacetEntry>
	{
		/// <summary>The key (field name) of this <see cref="Facet"/>.</summary>
		public string Key { get; private set; }
		/// <summary><see cref="IEnumerable{FacetEntry}"/> containing the values for this <see cref="Facet"/>.</summary>
		public IEnumerable<FacetEntry> Values { get; private set; }

		internal Facet(string key, IEnumerable<FacetEntry> values)
		{
			Key = key;
			// We don't want zero-value facets
			Values = values.Where(v => v.Count > 0);
		}

		internal Facet(string key, IEnumerable<KeyValuePair<string, int>> values)
			: this(key, values.Select(v => new FacetEntry(key, v)))
		{ }

		/// <summary>
		/// Get the numbered <see cref="FacetEntry"/> from this collection.
		/// </summary>
		/// <param name="index">The index to retrieve.</param>
		/// <returns><see cref="FacetEntry"/></returns>
		public FacetEntry this[int index]
		{
			get { return Values.ToArray()[index]; }
		}

		/// <summary>
		/// Get the count of <see cref="FacetEntry"/> contained in this collection.
		/// </summary>
		public int Count
		{
			get { return Values == null ? 0 : Values.Count(); }
		}

		/// <summary>
		/// Get an <see cref="IEnumerator{T}"/> for this collection.
		/// </summary>
		/// <returns><see cref="IEnumerator{T}"/></returns>
		public IEnumerator<FacetEntry> GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		/// <summary>
		/// Get an <see cref="System.Collections.IEnumerator"/> for this collection.
		/// </summary>
		/// <returns><see cref="System.Collections.IEnumerator"/></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Values.GetEnumerator();
		}
	}
}
