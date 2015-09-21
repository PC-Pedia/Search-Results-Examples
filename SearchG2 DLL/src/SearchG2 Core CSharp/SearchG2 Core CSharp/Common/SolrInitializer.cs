using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using CrownPeak.SearchG2.Query;
using HttpWebAdapters;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Impl;
using SolrNet.Mapping.Validation;
using SolrNet.Schema;
using SolrNet.Utils;

namespace CrownPeak.SearchG2.Common
{
	internal static class SolrInitializer<T>
	{
		public static ICrownPeakOperations<T> Solr(string collection)
		{
			var config = ServiceLocator.Current.GetInstance<IEndPointConfiguration>();
			return Solr(collection, config.Url, config.Timeout, config.Certificate);
		}
		public static ICrownPeakOperations<T> Solr(string collection, string endpoint)
		{
			var config = ServiceLocator.Current.GetInstance<IEndPointConfiguration>();
			return Solr(collection, endpoint, config.Timeout, config.Certificate);
		}
		public static ICrownPeakOperations<T> Solr(string collection, string endpoint, int timeout)
		{
			var config = ServiceLocator.Current.GetInstance<IEndPointConfiguration>();
			return Solr(collection, endpoint, timeout, config.Certificate);
		}
		public static ICrownPeakOperations<T> Solr(string collection, string endpoint, X509Certificate certificate)
		{
			var config = ServiceLocator.Current.GetInstance<IEndPointConfiguration>();
			return Solr(collection, endpoint, config.Timeout, certificate);
		}
		public static ICrownPeakOperations<T> Solr(string collection, string endpoint, int timeout, X509Certificate certificate)
		{
			try
			{
				return ServiceLocator.Current.GetInstance<ICrownPeakOperations<T>>();
			}
			catch (Exception)
			{
				// Assume it's because this hasn't been created yet, so do that now
				//Utils.AddConfiguration(new EndPointConfiguration());
				Utils.AddMapper(typeof(T));

				// Register our helper interfaces and implementations
				var container = Startup.Container;

				container.RemoveAll<ICrownPeakResponseParser<T>>();
				container.Register<ICrownPeakResponseParser<T>>(c => new CrownPeakResponseParser<T>());

				container.RemoveAll<ICrownPeakQueryExecuter<T>>();
				container.Register<ICrownPeakQueryExecuter<T>>(c => new CrownPeakQueryExecuter<T>(
					c.GetInstance<ICrownPeakResponseParser<T>>(),
					c.GetInstance<ISolrConnection>(),
					c.GetInstance<ISolrQuerySerializer>(),
					c.GetInstance<ISolrFacetQuerySerializer>(),
					c.GetInstance<ISolrMoreLikeThisHandlerQueryResultsParser<T>>()));

				container.RemoveAll<ICrownPeakBasicOperations<T>>();
				container.Register<ICrownPeakBasicOperations<T>>(c => new CrownPeakBasicServer<T>(
					c.GetInstance<ISolrConnection>(),
					c.GetInstance<ICrownPeakQueryExecuter<T>>(),
					c.GetInstance<ISolrDocumentSerializer<T>>(),
					c.GetInstance<ISolrSchemaParser>(),
					c.GetInstance<ISolrHeaderResponseParser>(),
					c.GetInstance<ISolrQuerySerializer>(),
					c.GetInstance<ISolrDIHStatusParser>(),
					c.GetInstance<ISolrExtractResponseParser>()
				));

				container.RemoveAll<ICrownPeakOperations<T>>();
				container.Register<ICrownPeakOperations<T>>(c => new CrownPeakServer<T>(
					c.GetInstance<ICrownPeakBasicOperations<T>>(),
					c.GetInstance<IReadOnlyMappingManager>(),
					c.GetInstance<IMappingValidator>()
				));

				ServiceLocator.SetLocatorProvider(() => container);

				if (string.IsNullOrEmpty(endpoint))
				{
					var config = ServiceLocator.Current.GetInstance<IEndPointConfiguration>();
					endpoint = config.Url;
				}

				Startup.Init<T>(new CrownPeakSolrConnection(endpoint + collection, certificate) { Timeout = timeout });
				return ServiceLocator.Current.GetInstance<ICrownPeakOperations<T>>();
			}
		}
	}
}
