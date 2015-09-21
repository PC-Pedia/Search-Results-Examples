using SolrNet;

namespace CrownPeak.SearchG2.Query
{
	/// <summary>
	/// Class that wraps a single sort order request when executing a
	/// <see cref="CrownPeak.SearchG2.PlainSearch"/> or a <see cref="CrownPeak.SearchG2.Search{T}"/>
	/// on a CrownPeak SearchG2 collection.
	/// </summary>
	public class SortOrder
	{
		/// <summary>The field to sort by.</summary>
		public string Field { get; set; }
		/// <summary>The <see cref="SortDirection"/> for this <see cref="SortOrder"/>.</summary>
		public SortDirection Direction { get; set; }

		/// <summary>
		/// Create a new <see cref="SortOrder"/> object with an empty 
		/// <see cref="Field"/> and <see cref="Direction"/> = Ascending.
		/// </summary>
		public SortOrder()
			: this(string.Empty, SortDirection.Asc)
		{ }

		/// <summary>
		/// Create a new <see cref="SortOrder"/> object with provided
		/// <see cref="Field"/> and <see cref="Direction"/>.
		/// </summary>
		/// <param name="field">The <see cref="Field"/> field to sort by.</param>
		/// <param name="direction">The <see cref="SortDirection"/> to sort in.</param>
		public SortOrder(string field, SortDirection direction)
		{
			Field = field;
			Direction = direction;
		}

		/// <summary>
		/// Convert the current <see cref="SolrNet.SortOrder"/> into a format for Solr queries.
		/// </summary>
		internal SolrNet.SortOrder ToSolrSortOrder()
		{
			return new SolrNet.SortOrder(Field, Direction == SortDirection.Asc ? Order.ASC : Order.DESC);
		}
	}

	/// <summary>Available directions for a <see cref="SortOrder"/> parameter.</summary>
	public enum SortDirection
	{
		/// <summary>Sort the field in ascending order.</summary>
		Asc,
		/// <summary>Sort the field in descending order.</summary>
		Desc
	}
}
