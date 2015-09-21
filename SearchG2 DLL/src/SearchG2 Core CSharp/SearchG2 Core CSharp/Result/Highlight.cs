using System.Collections.Generic;
using System.Linq;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a single highlight result from a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or a <see cref="CrownPeak.SearchG2.Search{T}"/>
	/// on a CrownPeak SearchG2 collection.
	/// </summary>
	public class Highlight : ICollection<string>
	{
		/// <summary>The separator of to use between each of multiple values.</summary>
		public string Separator { get; internal set; }

		private readonly ICollection<string> _this;
		private bool _isSingleValue = false;

		internal Highlight(string highlight)
			: this(new[] { highlight })
		{
			_isSingleValue = true;
		}

		internal Highlight(ICollection<string> highlight, string separator = "")
		{
			_this = highlight;
			Separator = separator;
		}

		/// <summary>
		/// Return a string that represents the current <see cref="Highlight"/>.
		/// </summary>
		/// <returns><see cref="string"/> prefixed, separated and suffixed by the <see cref="Separator"/>.</returns>
		public override string ToString()
		{
			if (_this.Count == 0) return string.Empty;
			return _isSingleValue ? _this.First() : Join(Separator, Separator, Separator);
		}

		/// <summary>
		/// Join the multiple values together with a specified prefix, separator and suffix.
		/// </summary>
		/// <param name="separator">The separator to use between values.</param>
		/// <param name="prefix">The prefix to use before the first value.</param>
		/// <param name="suffix">The suffix to use after the last value.</param>
		/// <returns><see cref="string"/> prefixed, separated and suffixed as required.</returns>
		public string Join(string separator, string prefix = "", string suffix = "")
		{
			return string.Concat(prefix, string.Join(separator, _this), suffix).Trim();
		}

		#region ICollection<string> implementation

		/// <summary>
		/// Add the <see cref="string"/> to the collection.
		/// </summary>
		/// <param name="item"><see cref="string"/> to add to the collection.</param>
		public void Add(string item)
		{
			_this.Add(item);
		}

		/// <summary>
		/// Remove all values from this collection
		/// </summary>
		public void Clear()
		{
			_this.Clear();
		}

		/// <summary>
		/// Test if the item is contained in the collection.
		/// </summary>
		/// <param name="item"><see cref="string"/> to be tested.</param>
		/// <returns>True if item is contained in the collection, or false if not.</returns>
		public bool Contains(string item)
		{
			return _this.Contains(item);
		}

		/// <summary>
		/// Copy the contents of the collection to the specified one-dimensional array, starting at the specified index.
		/// </summary>
		/// <param name="array">The destination array.</param>
		/// <param name="arrayIndex">The index at which to start copying.</param>
		public void CopyTo(string[] array, int arrayIndex)
		{
			_this.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Get the count of items in the collection.
		/// </summary>
		public int Count
		{
			get { return _this.Count(); }
		}

		/// <summary>
		/// Get whether this collection is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get { return _this.IsReadOnly; }
		}

		/// <summary>
		/// Remove the specified item from the collection.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>True if the item is removed, or false if not.</returns>
		public bool Remove(string item)
		{
			return _this.Remove(item);
		}

		/// <summary>
		/// Get an <see cref="IEnumerator{String}"/> for the current collection.
		/// </summary>
		/// <returns><see cref="IEnumerator{String}"/></returns>
		public IEnumerator<string> GetEnumerator()
		{
			return _this.GetEnumerator();
		}

		/// <summary>
		/// Get an <see cref="System.Collections.IEnumerator"/> for the current collection.
		/// </summary>
		/// <returns><see cref="System.Collections.IEnumerator"/></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _this.GetEnumerator();
		}

		#endregion
	}
}
