using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrownPeak.SearchG2.Result;

namespace CrownPeak.SearchG2.Client.UserControls
{
	public partial class Highlight : System.Web.UI.UserControl
	{
		public IDataSource DataSource { get; set; }

		public string HighlightField { get; set; }
		public string TextField { get; set; }
		public string Separator { get; set; }
		public string Prefix { get; set; }
		public string Suffix { get; set; }
		public int MaxLength { get; set; }

		public Highlight()
		{
			Separator = " ... ";
			Prefix = "... ";
			Suffix = " ...";
			MaxLength = 0;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);
			ResultBase data = DataBinder.GetDataItem(this.NamingContainer) as ResultBase;
			if (data != null)
			{
				Repeater1.DataSource = data.Highlights[HighlightField];
				Repeater1.Visible = data.Highlights.ContainsKey(HighlightField);
				int maxLength = MaxLength > 0 ? MaxLength : int.MaxValue;
				Literal1.Text = DataBinder.Eval(data, string.IsNullOrWhiteSpace(TextField) ? HighlightField : TextField).ToString();
				if (Literal1.Text.Length > maxLength)
				{
					Literal1.Text = Literal1.Text.Substring(0, maxLength - Suffix.Length) + Suffix;
				}
				Literal1.Visible = !Repeater1.Visible;
			}
			Repeater1.DataBind();
			Literal1.DataBind();
		}
	}
}