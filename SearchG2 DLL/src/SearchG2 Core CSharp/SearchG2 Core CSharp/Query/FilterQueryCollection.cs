using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Class that wraps a set of <see cref="FilterQuery"/> objects to be used when executing a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or a <see cref="CrownPeak.SearchG2.Search{T}"/>
	/// on a CrownPeak SearchG2 collection.
	/// </summary>
	public class FilterQueryCollection : ICollection<FilterQuery>
	{
		private List<FilterQuery> _filterQueries;

		/// <summary>
		/// Create a new, empty <see cref="FilterQueryCollection"/> object.
		/// </summary>
		public FilterQueryCollection()
			: this(new FilterQuery[] { })
		{ }

		/// <summary>
		/// Create a new <see cref="FilterQueryCollection"/> object from an
		/// <see cref="IEnumerable{FilterQuery}"/>.
		/// </summary>
		/// <param name="filterQueries"><see cref="IEnumerable{FilterQuery}"/> containing
		/// the filter queries to be applied.</param>
		public FilterQueryCollection(IEnumerable<FilterQuery> filterQueries)
		{
			_filterQueries = filterQueries.ToList();
		}

		/// <summary>
		/// Create a new <see cref="FilterQueryCollection"/> object from a string
		/// containing zero or more <see cref="FilterQuery"/> in the format
		/// field:value[;field:value...].
		/// </summary>
		/// <param name="filterQueries"><see cref="string"/> containing the filter queries 
		/// to be applied, in the format field:value[;field:value...].</param>
		public FilterQueryCollection(string filterQueries)
			: this()
		{
			if (filterQueries != null)
			{
				_filterQueries = filterQueries.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
					.Select(f => new FilterQuery(f)).ToList();
			}
		}

		/// <summary>
		/// Return a string that represents the current <see cref="FilterQueryCollection"/>.
		/// </summary>
		/// <returns><see cref="FilterQuery"/> in the format field:value[;field:value...].</returns>
		public override string ToString()
		{
			return string.Join(";", _filterQueries.Select(f => f.ToString()).ToArray());
		}

		/// <summary>
		/// Get the numbered <see cref="FilterQuery"/> from this collection.
		/// </summary>
		/// <param name="index">The index to retrieve.</param>
		/// <returns><see cref="FilterQuery"/></returns>
		public FilterQuery this[int index]
		{
			get { return _filterQueries[index]; }
		}

		/// <summary>
		/// Get the count of <see cref="FilterQuery"/> contained in this collection.
		/// </summary>
		public int Count
		{
			get { return _filterQueries.Count; }
		}

		/// <summary>
		/// Get an <see cref="IEnumerator{T}"/> for this collection.
		/// </summary>
		/// <returns><see cref="IEnumerator{T}"/></returns>
		public IEnumerator<FilterQuery> GetEnumerator()
		{
			return _filterQueries.GetEnumerator();
		}

		/// <summary>
		/// Get an <see cref="IEnumerator"/> for this collection.
		/// </summary>
		/// <returns><see cref="IEnumerator"/></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _filterQueries.GetEnumerator();
		}

		/// <summary>
		/// Add a <see cref="FilterQuery"/> to the collection.
		/// </summary>
		/// <param name="item"><see cref="FilterQuery"/> to be added.</param>
		public void Add(FilterQuery item)
		{
			_filterQueries.Add(item);
		}

		/// <summary>
		/// Remove all <see cref="FilterQuery"/> objects from the collection.
		/// </summary>
		public void Clear()
		{
			_filterQueries.Clear();
		}

		/// <summary>
		/// Test if the collection contains a specific <see cref="FilterQuery"/> object.
		/// </summary>
		/// <param name="item"><see cref="FilterQuery"/> to be checked.</param>
		/// <returns>True if it is contained in the collection, or false if not.</returns>
		public bool Contains(FilterQuery item)
		{
			return _filterQueries.Contains(item);
		}

		/// <summary>
		/// Copies the entire set of <see cref="FilterQuery"/> to a one-dimensional array,
		/// starting at the specified index.
		/// </summary>
		/// <param name="array">The array to copy to</param>
		/// <param name="arrayIndex">The array index to begin copying to.</param>
		public void CopyTo(FilterQuery[] array, int arrayIndex)
		{
			_filterQueries.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Whether this collection is read only or not
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Remove the specified <see cref="FilterQuery"/> from the collection.
		/// </summary>
		/// <param name="item"><see cref="FilterQuery"/> to be removed.</param>
		/// <returns>True if the item is successfully removed, or false if not.</returns>
		public bool Remove(FilterQuery item)
		{
			return _filterQueries.Remove(item);
		}
	}
}
