using System.Collections.Generic;
using System.Linq;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a collection of <see cref="Facet"/> results from a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or a <see cref="CrownPeak.SearchG2.Search{T}"/>
	/// on a CrownPeak SearchG2 collection.
	/// </summary>
	public class FacetCollection : IReadOnlyList<Facet>, IReadOnlyDictionary<string, Facet>
	{
		private Dictionary<string, Facet> _facets;

		internal FacetCollection() 
			: this(new Facet[] {})
		{ }

		internal FacetCollection(IEnumerable<Facet> facets)
		{
			_facets = facets.ToDictionary(f => f.Key, f => f);
		}

		internal FacetCollection(IDictionary<string, ICollection<KeyValuePair<string, int>>> facets)
		{
			_facets = facets.ToDictionary(f => f.Key, f => new Facet(f.Key, f.Value));
		}

		/// <summary>
		/// Get the numbered <see cref="Facet"/> from this collection.
		/// </summary>
		/// <param name="index">The index to retrieve.</param>
		/// <returns><see cref="Facet"/></returns>
		public Facet this[int index]
		{
			get { return _facets[_facets.Keys.ToArray()[index]]; }
		}

		/// <summary>
		/// Get the count of <see cref="Facet"/> contained in this collection.
		/// </summary>
		public int Count
		{
			get { return _facets.Count(); }
		}

		/// <summary>
		/// Get an <see cref="IEnumerator{T}"/> for this collection.
		/// </summary>
		/// <returns><see cref="IEnumerator{T}"/></returns>
		public IEnumerator<Facet> GetEnumerator()
		{
			return _facets.Values.GetEnumerator();
		}

		/// <summary>
		/// Get an <see cref="System.Collections.IEnumerator"/> for this collection.
		/// </summary>
		/// <returns><see cref="System.Collections.IEnumerator"/></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _facets.Values.GetEnumerator();
		}

		/// <summary>
		/// Test if the collection contains a specific key.
		/// </summary>
		/// <param name="key">The key to be checked.</param>
		/// <returns>True if it the key is contained in the collection, or false if not.</returns>
		public bool ContainsKey(string key)
		{
			return _facets.ContainsKey(key);
		}

		/// <summary>
		/// Get an <see cref="IEnumerable{String}"/> of the Keys in this collection.
		/// </summary>
		public IEnumerable<string> Keys
		{
			get { return _facets.Keys; }
		}

		/// <summary>
		/// Try to retrieve the value from the collection the corresponds to this key.
		/// </summary>
		/// <param name="key">The key to be checked.</param>
		/// <param name="value">The value retrieved from the collection.</param>
		/// <returns>True if the key was found in the collection, or false if not.</returns>
		public bool TryGetValue(string key, out Facet value)
		{
			return _facets.TryGetValue(key, out value);
		}

		/// <summary>
		/// Get an <see cref="IEnumerable{Facet}"/> of the <see cref="Facet"/> objects in this collection.
		/// </summary>
		public IEnumerable<Facet> Values
		{
			get { return _facets.Values; }
		}

		/// <summary>
		/// Get the named <see cref="Facet"/> from this collection.
		/// </summary>
		/// <param name="key">The key to retrieve.</param>
		/// <returns><see cref="Facet"/></returns>
		public Facet this[string key]
		{
			get { return _facets[key]; }
		}

		/// <summary>
		/// Get an <see cref="IEnumerator{T}"/> for this collection.
		/// </summary>
		/// <returns><see cref="IEnumerator{T}"/></returns>
		IEnumerator<KeyValuePair<string, Facet>> IEnumerable<KeyValuePair<string, Facet>>.GetEnumerator()
		{
			return _facets.GetEnumerator();
		}
	}
}
