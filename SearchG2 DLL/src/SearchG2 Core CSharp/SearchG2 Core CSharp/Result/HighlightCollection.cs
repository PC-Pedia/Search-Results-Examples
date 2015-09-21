using System.Collections.Generic;
using System.Linq;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a collection of <see cref="Highlight"/> results from a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or
	/// <see cref="CrownPeak.SearchG2.Search{T}"/> on a CrownPeak SearchG2 collection.
	/// </summary>
	public class HighlightCollection : SearchCollection<Highlight>
	{
		internal HighlightCollection()
		{
			SetDictionary(new Dictionary<string, Highlight>());
		}

		internal HighlightCollection(IEnumerable<KeyValuePair<string, ICollection<string>>> highlights)
		{
			SetDictionary(highlights.ToDictionary(h => h.Key, h => new Highlight(h.Value)));
		}
	}
}
