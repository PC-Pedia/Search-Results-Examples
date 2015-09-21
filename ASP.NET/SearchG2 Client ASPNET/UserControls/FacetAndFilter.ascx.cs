using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrownPeak.SearchG2;
using CrownPeak.SearchG2.Query;

namespace CrownPeak.SearchG2.Client.UserControls
{
	public partial class FacetAndFilter : System.Web.UI.UserControl
	{
		public object FilterDataSource
		{
			get { return rptFilters.DataSource; }
			set { rptFilters.DataSource = value; }
		}

		public object FacetDataSource
		{
			get { return rptFacets.DataSource; }
			set { rptFacets.DataSource = value; }
		}
		public event EventHandler<FilterChangedEventArgs> FilterChanged;

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void AddFilter_Click(object sender, EventArgs e)
		{
			FilterQueryCollection existingQueries = new FilterQueryCollection(hidFilterQueries.Value);
			FilterQuery fq = new FilterQuery(((LinkButton)sender).CommandArgument);
			existingQueries.Add(fq);
			UpdateFilters(existingQueries);
		}

		protected void RemoveFilter_Click(object sender, EventArgs e)
		{
			FilterQueryCollection existingQueries = new FilterQueryCollection(hidFilterQueries.Value);
			FilterQuery fq = new FilterQuery(((LinkButton)sender).CommandArgument);
			existingQueries.Remove(fq);
			UpdateFilters(existingQueries);
		}

		protected void RemoveAllFilters_Click(object sender, EventArgs e)
		{
			UpdateFilters(new FilterQueryCollection());
		}

		protected override void OnDataBinding(EventArgs e)
		{
			rptFilters.DataBind();
			rptFacets.DataBind();
			base.OnDataBinding(e);
		}

		private void UpdateFilters(FilterQueryCollection filterQueries)
		{
			rptFilters.Visible = filterQueries.Count > 0;
			hidFilterQueries.Value = filterQueries.ToString();
			if (FilterChanged != null)
				FilterChanged(this, new FilterChangedEventArgs(filterQueries));
		}
	}

	public class FilterChangedEventArgs : EventArgs
	{
		public FilterQueryCollection Filters { get; set; }

		public FilterChangedEventArgs(FilterQueryCollection filters)
		{
			Filters = filters;
		}
	}
}