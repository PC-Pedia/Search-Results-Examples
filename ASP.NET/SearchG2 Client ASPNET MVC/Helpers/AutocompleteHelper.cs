using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CrownPeak.SearchG2.Client.Helpers
{
	public static class AutocompleteHelper
	{
		public static MvcHtmlString AutocompleteFor<TModel, TValue>(this HtmlHelper<TModel> helper,
			Expression<Func<TModel, TValue>> expression, string actionUrl)
		{
			return CreateAutocomplete(helper, expression, actionUrl, null, null);
		}

		public static MvcHtmlString AutocompleteFor<TModel, TValue>(this HtmlHelper<TModel> helper,
			Expression<Func<TModel, TValue>> expression, string actionUrl, bool? isRequired, string placeholder)
		{
			return CreateAutocomplete(helper, expression, actionUrl, placeholder, isRequired);
		}

		private static MvcHtmlString CreateAutocomplete<TModel, TValue>(HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string actionUrl, string placeholder, bool? isRequired)
		{
			var attributes = new Dictionary<string, object>
			{
				{"data-autocomplete", true},
				{"data-action", actionUrl}
			};

			if (!string.IsNullOrWhiteSpace(placeholder))
			{
				attributes.Add("placeholder", placeholder);
			}

			if (isRequired.HasValue && isRequired.Value)
			{
				attributes.Add("required", "required");
			}

			Func<TModel, TValue> method = expression.Compile();
			// These bits are commented out as we don't need them - our box has the same value as label
//			var value = method(helper.ViewData.Model);
			var baseProperty = ((MemberExpression) expression.Body).Member.Name;
//			var hidden = helper.Hidden(baseProperty, value);

//			attributes.Add("data-value-name", baseProperty);

//			var autocompleteName = baseProperty + "_autocomplete";
//			var textBox = helper.TextBox(autocompleteName, null, string.Empty, attributes);
			var textBox = helper.TextBox(baseProperty, null, string.Empty, attributes);

			var builder = new StringBuilder();
//			builder.AppendLine(hidden.ToHtmlString());
			builder.AppendLine(textBox.ToHtmlString());

			return new MvcHtmlString(builder.ToString());
		}
	}
}