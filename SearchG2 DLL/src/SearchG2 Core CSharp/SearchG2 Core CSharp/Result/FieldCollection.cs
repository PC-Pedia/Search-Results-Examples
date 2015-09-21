using System.Collections.Generic;
using System.Linq;
using CrownPeak.SearchG2.Query;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class that wraps a collection of <see cref="Facet"/> results from a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> on a CrownPeak SearchG2 collection.
	/// </summary>
	public class FieldCollection : SearchCollection<Field>
	{
		internal FieldCollection()
		{
			SetDictionary(new Dictionary<string, Field>());
		}

		internal FieldCollection(Dictionary<string, object> dictionary)
		{
			Dictionary<string, Field> d = dictionary.Keys.ToDictionary(key => key, key => new Field(key, dictionary[key]));
			SetDictionary(d);
		}
	}
}
