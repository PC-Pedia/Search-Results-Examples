using System.Collections.Generic;
using System.Linq;

namespace CrownPeak.SearchG2.Result
{
	// TODO: find a better way to contain the field type
	/// <summary>
	/// Class that wraps a single filter query operation when executing a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> operation on a 
	/// CrownPeak SearchG2 collection.
	/// </summary>
	public class PlainResult : IReadOnlyList<IField>
	{
		/// <summary>The unique id of this result.</summary>
		public string Id { get; internal set; }
		/// <summary>Collection of <see cref="Field"/> objects that are in this result.</summary>
		public FieldCollection Fields { get; internal set; }
		/// <summary>Collection of <see cref="Highlight"/> objects that apply to this result.</summary>
		public HighlightCollection Highlights { get; internal set; }

		private PlainResult()
		{
			Fields = new FieldCollection();
			Highlights = new HighlightCollection();
			Id = string.Empty;
		}

		internal PlainResult(Dictionary<string, object> solrResult) : this()
		{
			Fields = new FieldCollection(solrResult);
			if (Fields.ContainsKey("id"))
			{
				Id = Fields["id"].Value.ToString();
			}
		}

		internal void SetHighlights(SolrNet.Impl.HighlightedSnippets highlights)
		{
			Highlights = new HighlightCollection(highlights);

			// Merge the highlights into the fields as well
			foreach (var h in highlights)
			{
				if (Fields.ContainsKey(h.Key))
				{
					Fields[h.Key].Highlights = h.Value.ToArray();
				}
			}
		}

		/// <summary>Get the collection of keys (field names) in this result.</summary>
		public IEnumerable<string> Keys
		{
			get { return Fields.Keys; }
		}

		/// <summary>Get the <see cref="IField"/> from the collection by its index.</summary>
		/// <param name="index">The index of the item to retrieve.</param>
		/// <returns><see cref="IField"/></returns>
		public IField this[int index]
		{
			get { return Fields[index]; }
		}

		/// <summary>Get the <see cref="IField"/> from the collection by its name.</summary>
		/// <param name="key">The name of the item to retrieve.</param>
		/// <returns><see cref="IField"/></returns>
		public IField this[string key]
		{
			get { return Fields[key]; }
		}

		/// <summary>
		/// Get the count of the number of <see cref="IField"/> objects in this collection.
		/// </summary>
		public int Count
		{
			get { return Fields.Count; }
		}

		/// <summary>
		/// Test whether the collection contains a <see cref="IField"/> with the specified key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>True if a <see cref="IField"/> with the key is found, or false if not.</returns>
		public bool ContainsKey(string key)
		{
			return Fields.ContainsKey(key);
		}

		/// <summary>
		/// Get an <see cref="IEnumerator{IField}"/> for the current collection.
		/// </summary>
		/// <returns><see cref="IEnumerator{IField}"/></returns>
		public IEnumerator<IField> GetEnumerator()
		{
			return Fields.Values.GetEnumerator();
		}

		/// <summary>
		/// Get an <see cref="System.Collections.IEnumerator"/> for the current collection.
		/// </summary>
		/// <returns><see cref="System.Collections.IEnumerator"/></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Fields.Values.GetEnumerator();
		}
	}
}
