using System;
using System.Collections.Generic;
using System.Net;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Exceptions;
using CrownPeak.SearchG2.Query;
using CrownPeak.SearchG2.Result;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Exceptions;

namespace CrownPeak.SearchG2
{
	/// <summary>
	/// <para>Class to support a search on the CrownPeak SearchG2 infrastructure, 
	/// using a specified type to contain each result.</para>
	/// <seealso cref="PlainSearch"/>
	/// </summary>
	/// <typeparam name="T">The type you wish to use for each result. T must inherit from
	/// <see cref="ResultBase"/>.</typeparam>
	public class Search<T> where T : ResultBase
	{
// ReSharper disable once StaticFieldInGenericType
		private static Settings _settings;

		/// <summary>
		/// Create a new <see cref="Search{T}"/> object with the specified settings
		/// </summary>
		/// <param name="settings">The <see cref="Settings"/> to be used</param>
		public Search(Settings settings)
		{
			_settings = settings;
		}

		private static ICrownPeakOperations<T> Solr
		{
			get
			{
				return SolrInitializer<T>.Solr(_settings.Collection, _settings.Endpoint, _settings.Timeout, _settings.Certificate);
			}
		}

		/// <summary>
		/// Execute a search that returns all of the results in the specified collection.
		/// </summary>
		/// <returns><see cref="ResultCollection{T}"/></returns>
		public ResultCollection<T> Execute()
		{
			return Execute("*:*");
		}

		/// <summary>
		/// Execute a search that returns the results that match the specified query text,
		/// using a default set of query options.
		/// </summary>
		/// <param name="q">The query string to execute. See 
		/// <see href="https://wiki.apache.org/solr/SolrQuerySyntax"/>.</param>
		/// <returns><see cref="ResultCollection{T}"/></returns>
		public ResultCollection<T> Execute(string q)
		{
			try
			{
				return new ResultCollection<T>(Solr.Query(q, DefaultQueryOptions.BasicOptions));
			}
			catch (Exception ex)
			{
				Utils.ThrowException(ex);
				return null;
			}
		}

		/// <summary>
		/// Execute a search that returns the results that match the specified query text,
		/// using a specified set of <see cref="QueryOptions"/>
		/// </summary>
		/// <param name="q">The query string to execute. See 
		/// <see href="https://wiki.apache.org/solr/SolrQuerySyntax"/>.</param>
		/// <param name="options">The <see cref="QueryOptions"/> to use for this query.</param>
		/// <returns><see cref="ResultCollection{T}"/></returns>
		public ResultCollection<T> Execute(string q, QueryOptions options)
		{
			try
			{
				return new ResultCollection<T>(Solr.Query(q, options.SolrOptions), options.Start);
			}
			catch (Exception ex)
			{
				Utils.ThrowException(ex);
				return null;
			}
		}
	}
}
