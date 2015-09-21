using System.Security.Cryptography.X509Certificates;

namespace CrownPeak.SearchG2.Common
{
	/// <summary>
	/// Wrap the required settings for operations to be executed on CrownPeak SearchG2.
	/// </summary>
	public class Settings
	{
		/// <summary>
		/// The endpoint to connect to.
		/// </summary>
		public string Endpoint { get; set; }
		/// <summary>
		/// The timeout for connection and query operations in milliseconds.
		/// </summary>
		public int Timeout { get; set; }
		/// <summary>
		/// The name of the collection to be operated on.
		/// </summary>
		public string Collection { get; set; }
		/// <summary>
		/// The certificate to be sent with requests.
		/// </summary>
		public X509Certificate Certificate { get; set; }

		/// <summary>Initialize a <see cref="Settings"/> object for a collection.</summary>
		/// <param name="collection">The collection name.</param>
		public Settings(string collection) : this(collection, string.Empty)
		{ }
		/// <summary>Initialize a <see cref="Settings"/> object for a collection.</summary>
		/// <param name="collection">The collection name.</param>
		/// <param name="endpoint">The endpoint.</param>
		public Settings(string collection, string endpoint)
			: this(collection, endpoint, null)
		{ }
		/// <summary>Initialize a <see cref="Settings"/> object for a collection.</summary>
		/// <param name="collection">The collection name.</param>
		/// <param name="endpoint">The endpoint.</param>
		/// <param name="certificate">The certificate to be sent with requests.</param>
		public Settings(string collection, string endpoint, X509Certificate certificate)
			: this(collection, endpoint, 5000, certificate)
		{ }
		/// <summary>Initialize a <see cref="Settings"/> object for a collection.</summary>
		/// <param name="collection">The collection name.</param>
		/// <param name="endpoint">The endpoint.</param>
		/// <param name="timeout">The connection and query timeout, in milliseconds.</param>
		public Settings(string collection, string endpoint, int timeout)
			: this(collection, endpoint, timeout, null)
		{ }
		/// <summary>Initialize a <see cref="Settings"/> object for a collection.</summary>
		/// <param name="collection">The collection name.</param>
		/// <param name="endpoint">The endpoint.</param>
		/// <param name="timeout">The connection and query timeout, in milliseconds.</param>
		/// <param name="certificate">The certificate to be sent with requests.</param>
		public Settings(string collection, string endpoint, int timeout, X509Certificate certificate)
		{
			// Make sure there's an endpoint configured before it tries to get used
			Utils.AddConfiguration(new EndPointConfiguration());

			Collection = collection;
			Endpoint = endpoint;
			Timeout = timeout;
			Certificate = certificate;
		}
	}
}
