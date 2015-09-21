using System.Security.Cryptography.X509Certificates;

namespace CrownPeak.SearchG2.Common
{
	/// <summary>
	/// Default implementation of <see cref="IEndPointConfiguration"/>, connecting to
	/// http://searchg2.crownpeak.net/.
	/// </summary>
	internal class EndPointConfiguration : IEndPointConfiguration
	{
		/// <summary>
		/// Get the EndPoint url.
		/// </summary>
		public string Url
		{
			get { return "http://searchg2.crownpeak.net/"; }
		}

		/// <summary>
		/// Get the timeout to be used for this EndPoint
		/// </summary>
		public int Timeout
		{
			get { return 5000; }
		}

		/// <summary>
		/// Get the certificate to be sent for requests to this EndPoint.
		/// </summary>
		public X509Certificate Certificate
		{
			get { return null; }
		}
	}
}
