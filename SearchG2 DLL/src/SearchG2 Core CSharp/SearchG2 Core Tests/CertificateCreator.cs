using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CrownPeak.SearchG2.Tests
{
	public class CertificateCreator
	{
		public static X509Certificate LoadCertificate(string thumbprint)
		{
			if (string.IsNullOrWhiteSpace(thumbprint)) return null;

			var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			store.Open(OpenFlags.ReadOnly);
			return store.Certificates.Cast<X509Certificate2>()
					.FirstOrDefault(cert => string.Equals(cert.Thumbprint, thumbprint, StringComparison.CurrentCultureIgnoreCase));
		}
	}
}
