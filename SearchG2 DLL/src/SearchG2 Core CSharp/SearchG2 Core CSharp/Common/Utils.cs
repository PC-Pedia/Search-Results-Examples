using System;
using CrownPeak.SearchG2.Attributes;
using CrownPeak.SearchG2.Exceptions;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Attributes;
using SolrNet.Exceptions;
using SolrNet.Impl;
using SolrNet.Mapping;

namespace CrownPeak.SearchG2.Common
{
	internal static class Utils
	{
		public static void AddMapper(Type t)
		{
			// Inject our mapper
			// See https://github.com/mausch/SolrNet/blob/master/Documentation/Overriding-mapper.md#built-in-container
			// Document says to use new Container(Startup.Container)
			// but that causes a KeyNotFound exception when getting the ISolrOperations<T> instance
			// This way works for me!
			var container = Startup.Container; //new Container(Startup.Container);
			container.RemoveAll<IReadOnlyMappingManager>();
			container.Register<IReadOnlyMappingManager>(c => MakeMapper(t));
			ServiceLocator.SetLocatorProvider(() => container);
		}

		public static void AddConfiguration(IEndPointConfiguration config)
		{
			try
			{
				var container = Startup.Container;
				container.RemoveAll<IEndPointConfiguration>();
				container.Register<IEndPointConfiguration>(c => config);
				ServiceLocator.SetLocatorProvider(() => container);
			}
			catch (Exception)
			{
				// Ignore exceptions here - just means something already registered this type
			}
		}

		public static void ThrowException(Exception ex)
		{
			if (ex is SolrConnectionException)
			{
				if (ex.Message.Contains("The operation has timed out"))
				{
					throw new SearchG2TimeoutException("The operation has timed out", ex);
				}
				else if (ex.Message.Contains("SyntaxError: Cannot parse"))
				{
					throw new SearchG2QueryException("Cannot parse query", ex);
				}
				else
				{
					throw new SearchG2ConnectionException("Unable to connect to index", ex);
				}
			}
			else
			{
				throw new SearchG2Exception("Unknown error", ex);
			}
		}

		private static MappingManager MakeMapper(Type t)
		{
			// See https://github.com/mausch/SolrNet/blob/master/Documentation/Overriding-mapper.md
			var mapper = new MappingManager();
			foreach (var prop in t.GetProperties())
			{
				foreach (Attribute attribute in prop.GetCustomAttributes(true))
				{
					if (attribute is FieldAttribute)
					{
						var fa = attribute as FieldAttribute;
						string fieldName = string.IsNullOrWhiteSpace(fa.FieldName) ? prop.Name : fa.FieldName;
						mapper.Add(prop, fieldName, fa.Boost);
					}
					else if (attribute is SolrFieldAttribute)
					{
						var fa = attribute as SolrFieldAttribute;
						string fieldName = string.IsNullOrWhiteSpace(fa.FieldName) ? prop.Name : fa.FieldName;
						mapper.Add(prop, fieldName, fa.Boost);
					}
					if (attribute is UniqueKeyAttribute)
					{
						var uka = attribute as UniqueKeyAttribute;
						mapper.SetUniqueKey(prop);
					}
					if (attribute is SolrUniqueKeyAttribute)
					{
						var uka = attribute as SolrUniqueKeyAttribute;
						mapper.SetUniqueKey(prop);
					}
				}
			}
			return mapper;
		}
	}
}
