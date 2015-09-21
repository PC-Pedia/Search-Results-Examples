namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class to wrap logging results that are specific to CrownPeak
	/// </summary>
	public class CrownPeakLoggingResults
	{
		/// <summary>
		/// The unique id associated with this logging operation
		/// </summary>
		public string Id { get; set; }

		internal CrownPeakLoggingResults()
			: this("")
		{ }

		internal CrownPeakLoggingResults(string id)
		{
			Id = id;
		}
	}
}
