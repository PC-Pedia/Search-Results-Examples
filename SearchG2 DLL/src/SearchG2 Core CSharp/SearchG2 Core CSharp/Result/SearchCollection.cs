using System.Collections.Generic;
using System.Linq;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>Base class for collections of fields relating to search results</summary>
	/// <typeparam name="T">Type of field used as the value for the collection</typeparam>
	public abstract class SearchCollection<T> : IReadOnlyList<T>,
																							IReadOnlyDictionary<string, T>
	{
		private Dictionary<string, T> _items;

		/// <summary>Allow derived classes to set the backing collection.</summary>
		/// <param name="dictionary">The dictionary to use as the backing collection.</param>
		protected void SetDictionary(Dictionary<string, T> dictionary)
		{
			_items = dictionary ?? new Dictionary<string, T>();
		}

		#region IReadOnlyList<T> Implementation
		/// <summary>Get a count of the number of in this collection.</summary>
		public int Count
		{
			get { return _items.Count; }
		}

		/// <summary>Get an <see cref="IEnumerator{T}"/> for this collection.</summary>
		/// <returns><see cref="IEnumerator{T}"/></returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return _items.Values.GetEnumerator();
		}

		/// <summary>Get an <see cref="System.Collections.IEnumerator"/> for this collection.</summary>
		/// <returns><see cref="System.Collections.IEnumerator"/></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _items.Values.GetEnumerator();
		}

		/// <summary>Get the item at the specified index.</summary>
		/// <param name="index">The index of the item to retrieve.</param>
		/// <returns>The item at the specified index.</returns>
		public T this[int index]
		{
			get { return _items[_items.Keys.ToArray()[index]]; }
		}
		#endregion

		#region IReadOnlyDictionary<T> Implementation
		/// <summary>Test if this collection contains an item with the specified key.</summary>
		/// <param name="key">The key of the item to test.</param>
		/// <returns>True if the item is found within the collection, or false if not.</returns>
		public bool ContainsKey(string key)
		{
			return _items.ContainsKey(key);
		}

		/// <summary>Get an <see cref="IEnumerable{String}"/> of the keys in this collection.</summary>
		public IEnumerable<string> Keys
		{
			get { return _items.Keys; }
		}

		/// <summary>Try to retrieve the item with the specified key from the collection.</summary>
		/// <param name="key">The key of the item to retrieve.</param>
		/// <param name="value">The item.</param>
		/// <returns>True if the item is found within the collection, or false if not.</returns>
		public bool TryGetValue(string key, out T value)
		{
			return _items.TryGetValue(key, out value);
		}

		/// <summary>Get an <see cref="IEnumerable{T}"/> of the values in this collection.</summary>
		public IEnumerable<T> Values
		{
			get { return _items.Values; }
		}

		/// <summary>Get the item with the specified key.</summary>
		/// <param name="key">The key of the item to retrieve.</param>
		/// <returns>The item with the specified key.</returns>
		public T this[string key]
		{
			get
			{
				if (_items.ContainsKey(key))
					return _items[key];
				return default(T);
			}
		}

		/// <summary>Get an <see cref="IEnumerator{T}"/> of  
		/// <see cref="KeyValuePair{String,T}"/> for this collection.</summary>
		/// <returns><see cref="IEnumerator{T}"/> of <see cref="KeyValuePair{String,T}"/>.</returns>
		IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator()
		{
			foreach (KeyValuePair<string, T> pair in _items)
				yield return pair;
		}
		#endregion
	}
}
