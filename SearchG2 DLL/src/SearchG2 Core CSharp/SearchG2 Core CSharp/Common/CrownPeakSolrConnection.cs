using System.Security.Cryptography.X509Certificates;
using SolrNet;
using SolrNet.Impl;

namespace CrownPeak.SearchG2.Common
{
	internal class CrownPeakSolrConnection : SolrConnection, ISolrConnection
	{
		public CrownPeakSolrConnection(string serverURL) : base(serverURL)
		{ }

		public CrownPeakSolrConnection(string serverURL, X509Certificate certificate) : base(serverURL)
		{
			HttpWebRequestFactory = new CrownPeakWebRequestFactory(certificate);
		}
	}
}
