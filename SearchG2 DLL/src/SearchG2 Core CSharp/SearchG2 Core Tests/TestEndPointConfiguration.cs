using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CrownPeak.SearchG2.Common;

namespace CrownPeak.SearchG2.Tests
{
	public class TestEndPointConfiguration : IEndPointConfiguration
	{
		public string Url
		{
			get { return "http://searchg2.crownpeak.net/"; } // "http://searchg2.crownpeak.net/";
		}

		public int Timeout
		{
			get { return 5000; }
		}

		public X509Certificate Certificate
		{
			get { return null; }
		}
	}
}
