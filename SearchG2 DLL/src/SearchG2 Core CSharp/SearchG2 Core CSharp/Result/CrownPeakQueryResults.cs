using SolrNet;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Query results returned from a search using CrownPeak SearchG2
	/// </summary>
	/// <typeparam name="T">The type of document to retrieve</typeparam>
	internal class CrownPeakQueryResults<T> : SolrQueryResults<T>
	{
		/// <summary>
		/// Results that are specific to CrownPeak search
		/// </summary>
		public CrownPeakResults CrownPeak { get; internal set; }
	}
}
