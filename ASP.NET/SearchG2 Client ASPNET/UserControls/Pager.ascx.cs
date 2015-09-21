using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CrownPeak.SearchG2.Client.UserControls
{
	public partial class Pager : System.Web.UI.UserControl
	{
		public object DataSource { get; set; }
		public int CurrentPage { get; set; }
		public string LoggingId { get; set; }
		public event EventHandler<PageChangedEventArgs> PageChanged;

		protected override void OnInit(EventArgs e)
		{
			Page.RegisterRequiresControlState(this);
			base.OnInit(e);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected override void OnDataBinding(EventArgs e)
		{
			Repeater1.DataSource = DataSource;
			base.OnDataBinding(e);
		}

		protected override void LoadControlState(object savedState)
		{
			var pagerState = savedState as PagerState;
			if (pagerState != null)
			{
				CurrentPage = pagerState.CurrentPage;
				LoggingId = pagerState.LoggingId;
			}
		}

		protected override object SaveControlState()
		{
			return new PagerState(CurrentPage, LoggingId);
		}

		protected void Pager_Click(object sender, EventArgs e)
		{
			LinkButton lb = sender as LinkButton;
			if (lb != null)
			{
				int n = int.Parse(lb.CommandArgument);
				if (n != CurrentPage)
				{
					CurrentPage = n;
					if (PageChanged != null)
						PageChanged(this, new PageChangedEventArgs(CurrentPage, n, LoggingId));
				}
			}
		}
	}

	public class PageChangedEventArgs : EventArgs
	{
		public int PreviousPage { get; set; }
		public int NewPage { get; set; }
		public string LoggingId { get; set; }

		public PageChangedEventArgs(int previousPage, int newPage, string loggingId)
		{
			PreviousPage = previousPage;
			NewPage = newPage;
			LoggingId = loggingId;
		}
	}

	[Serializable]
	class PagerState
	{
		public int CurrentPage { get; set; }
		public string LoggingId { get; set; }

		public PagerState(int currentPage, string loggingId)
		{
			CurrentPage = currentPage;
			LoggingId = loggingId;
		}
	}
}