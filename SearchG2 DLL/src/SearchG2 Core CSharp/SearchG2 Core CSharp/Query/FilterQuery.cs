using System;
using System.Linq;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Class that wraps a single filter query operation when executing a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or a <see cref="CrownPeak.SearchG2.Search{T}"/>
	/// on a CrownPeak SearchG2 collection.
	/// </summary>
	public class FilterQuery
	{
		/// <summary>The field that this <see cref="FilterQuery"/> applies to.</summary>
		public string Field { get; set; }
		/// <summary>The value that this <see cref="Field"/> must match.</summary>
		public string Value { get; set; }

		/// <summary>
		/// Create a new <see cref="FilterQuery"/> object with an empty 
		/// <see cref="Field"/> and <see cref="Value"/>.
		/// </summary>
		public FilterQuery() : this("", "")
		{ }

		/// <summary>
		/// Create a new <see cref="FilterQuery"/> object with provided
		/// <see cref="Field"/> and <see cref="Value"/>.
		/// </summary>
		/// <param name="field">The <see cref="Field"/> that this <see cref="FilterQuery"/> applies to.</param>
		/// <param name="value">The <see cref="Value"/> that the <see cref="Field"/> must match.</param>
		public FilterQuery(string field, string value)
		{
			Field = field;
			Value = value;
		}

		/// <summary>
		/// Create a new <see cref="FilterQuery"/> object with a string containing both the 
		/// <see cref="Field"/> and the <see cref="Value"/>.
		/// </summary>
		/// <param name="filterQuery"><see cref="Field"/> and <see cref="Value"/> in the format
		/// field:value.</param>
		public FilterQuery(string filterQuery)
			: this()
		{
			if (filterQuery != null && filterQuery.IndexOf(':') >= 0)
			{
				string[] temp = filterQuery.Split(":".ToCharArray(), StringSplitOptions.None);
				Field = temp[0];
				Value = string.Join(":", temp.Skip(1).Take(temp.Length - 1));
			}
		}

		/// <summary>
		/// Return a string that represents the current <see cref="FilterQuery"/>.
		/// </summary>
		/// <returns><see cref="Field"/> and <see cref="Value"/> in the format field:value.</returns>
		public override string ToString()
		{
			// TODO: escape : in the value
			return string.Concat(Field, ":", Value);
		}

		/// <summary>
		/// Determines whether two specified <see cref="FilterQuery"/> objects have the same value.
		/// </summary>
		/// <param name="obj">The object to compare to this instance</param>
		/// <returns>True if the two objects match, or false if not.</returns>
		public override bool Equals(object obj)
		{
			if (obj is FilterQuery)
			{
				// Implement value equality for our class
				FilterQuery other = (FilterQuery) obj;
				return this.Field == other.Field && this.Value == other.Value;
			}
			return base.Equals(obj);
		}

		/// <summary>
		/// Returns this hash code for this instance.
		/// </summary>
		/// <returns>The hash code for this instance.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
