using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrownPeak.SearchG2.Result
{
	/// <summary>
	/// Class to wrap results that are specific to CrownPeak
	/// </summary>
	public class CrownPeakResults
	{
		/// <summary>
		/// CrownPeak Logging results
		/// </summary>
		public CrownPeakLoggingResults Logging { get; internal set; }

		internal CrownPeakResults()
		{ }

		internal CrownPeakResults(CrownPeakLoggingResults logging)
		{
			Logging = logging;
		}
	}
}
