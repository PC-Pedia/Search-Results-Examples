using System.Security.Cryptography.X509Certificates;

namespace CrownPeak.SearchG2.Common
{
	/// <summary>
	/// IEndPointConfiguration is the interface that is required for a
	/// connection to a SearchG2 end point.
	/// </summary>
	public interface IEndPointConfiguration
	{
		/// <summary>
		/// The url of the end point.
		/// </summary>
		string Url { get; }

		/// <summary>
		/// The timeout to be used for this end point.
		/// </summary>
		int Timeout { get; }

		/// <summary>
		/// The certificate to be sent for requests to this end point.
		/// </summary>
		X509Certificate Certificate { get; }
	}
}
