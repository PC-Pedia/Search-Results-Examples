using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using HttpWebAdapters;
using HttpWebAdapters.Adapters;

namespace CrownPeak.SearchG2.Common
{
	internal class CrownPeakWebRequestFactory : IHttpWebRequestFactory
	{
		private readonly X509Certificate _certificate;

		public CrownPeakWebRequestFactory() : this(null)
		{ }

		public CrownPeakWebRequestFactory(X509Certificate certificate)
		{
			_certificate = certificate;
		}

		public IHttpWebRequest Create(Uri url)
		{
			var req = (HttpWebRequest) WebRequest.Create(url);
			if (_certificate != null)
			{
				// If we're sending a certificate, we need TLS1.2 and to pass the callback check
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
				// Plus we add the certificate to the request's list of certificates
				req.ClientCertificates.Add(_certificate);
			}
			return new HttpWebRequestAdapter(req);
		}
	}
}
