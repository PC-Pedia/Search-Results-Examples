using System;
using System.Collections.Generic;
using CrownPeak.SearchG2.Common;
using CrownPeak.SearchG2.Exceptions;
using CrownPeak.SearchG2.Query;
using CrownPeak.SearchG2.Result;
using SolrNet;
using SolrNet.Exceptions;

namespace CrownPeak.SearchG2
{
	/// <summary>
	/// <para>Class to support a search on the CrownPeak SearchG2 infrastructure, 
	/// using a <see cref="Dictionary{String, Object}"/> to contain each result.</para>
	/// <seealso cref="Search{T}"/>
	/// </summary>
	public class PlainSearch
	{
// ReSharper disable once StaticFieldInGenericType
		/// <summary>Settings that were provided to the <see cref="PlainSearch(Settings)"/>.</summary>
		protected static Settings _settings;

		/// <summary>
		/// Create a new <see cref="PlainSearch"/> object with the specified settings
		/// </summary>
		/// <param name="settings">The <see cref="Settings"/> to be used</param>
		public PlainSearch(Settings settings)
		{
			_settings = settings;
		}

		private static ICrownPeakOperations<Dictionary<string, object>> Solr
		{
			get
			{
				return SolrInitializer<Dictionary<string, object>>.Solr(_settings.Collection, _settings.Endpoint, _settings.Timeout, _settings.Certificate);
			}
		}

		/// <summary>
		/// Execute a search that returns all of the results in the specified collection.
		/// </summary>
		/// <returns><see cref="PlainResultCollection"/></returns>
		public PlainResultCollection Execute()
		{
			return Execute("*:*");
		}

		/// <summary>
		/// Execute a search that returns the results that match the specified query text,
		/// using a default set of query options.
		/// </summary>
		/// <param name="q">The query string to execute. See 
		/// <see href="https://wiki.apache.org/solr/SolrQuerySyntax"/>.</param>
		/// <returns><see cref="PlainResultCollection"/></returns>
		public PlainResultCollection Execute(string q)
		{
			try
			{
				return new PlainResultCollection(Solr.Query(q, DefaultQueryOptions.BasicOptions));
			}
			catch (Exception ex)
			{
				Utils.ThrowException(ex);
				return null;
			}
		}

		/// <summary>
		/// Execute a search that returns the results that match the specified query text,
		/// using a specified set of <see cref="QueryOptions"/>.
		/// </summary>
		/// <param name="q">The query string to execute. See 
		/// <see href="https://wiki.apache.org/solr/SolrQuerySyntax"/>.</param>
		/// <param name="options">The <see cref="QueryOptions"/> to use for this query.</param>
		/// <returns><see cref="PlainResultCollection"/></returns>
		public PlainResultCollection Execute(string q, QueryOptions options)
		{
			try
			{
				return new PlainResultCollection(Solr.Query(q, options.SolrOptions), options.Start);
			}
			catch (Exception ex)
			{
				Utils.ThrowException(ex);
				return null;
			}
		}
	}
}
