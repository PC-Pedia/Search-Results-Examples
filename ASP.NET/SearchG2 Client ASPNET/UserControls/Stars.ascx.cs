using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CrownPeak.SearchG2.Client.UserControls
{
	public partial class Stars : System.Web.UI.UserControl
	{
		private double _score = 0.0;

		public double Score
		{
			get { return _score; }
			set
			{
				_score = value;
				Background.ToolTip = (Math.Round(value * 50) / 10) + " stars out of 5";
				Bar.Width = Unit.Pixel((int)(value*80));
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{

		}
	}
}