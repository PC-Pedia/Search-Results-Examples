<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="CrownPeak.SearchG2.Client.Search" %>
<%@ Import Namespace="CrownPeak.SearchG2.Client" %>
<%@ Import Namespace="CrownPeak.SearchG2.Result" %>
<%@ Register Src="~/UserControls/Highlight.ascx" TagPrefix="uc1" TagName="Highlight" %>
<%@ Register Src="~/UserControls/Pager.ascx" TagPrefix="uc1" TagName="Pager" %>
<%@ Register Src="~/UserControls/FacetAndFilter.ascx" TagPrefix="uc1" TagName="FacetAndFilter" %>
<%@ Register Src="~/UserControls/Stars.ascx" TagPrefix="uc1" TagName="Stars" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>CrownPeak CrownPeakSearchG2 ASP.NET Example</title>
	<link rel="stylesheet" type="text/css" href="styles.css"/>
</head>
<body>
	<form id="form1" runat="server">
		<ajaxToolkit:ToolkitScriptManager runat="server">
		</ajaxToolkit:ToolkitScriptManager>
		<div>
			<asp:TextBox runat="server" ID="txtSearch" placeholder="Search" autofocus="autofocus" />
			<ajaxToolkit:AutoCompleteExtender runat="server"
				ServiceMethod="GetCompletionList" TargetControlID="txtSearch"
				MinimumPrefixLength="3" CompletionInterval="250" CompletionSetCount="5"
				ShowOnlyCurrentWordInCompletionListItem="false" />
			<asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
		</div>
		
		<asp:PlaceHolder runat="server" ID="phSearchResults" Visible="False">
			<asp:PlaceHolder runat="server" ID="phSearchResultsNoResults">
				<p>Sorry, no results found for <b><asp:Label runat="server" ID="lblQueryNoResults" /></b></p>
			</asp:PlaceHolder>
			
			<asp:PlaceHolder runat="server" ID="phSuggestions" Visible="False">
				<p>Did you mean: <asp:LinkButton runat="server" ID="lbSuggestionCollation" OnClick="Suggestion_Click" Text="Suggestion" /></p>
			</asp:PlaceHolder>
			
			<asp:PlaceHolder runat="server" ID="phSearchResultsResults" Visible="false">
				
				<p>Showing search results for: 
					"<asp:Label runat="server" ID="lblQuery" />"</p>
				<p>Your search term produced
					<asp:Label runat="server" ID="lblCount" />
					results. 
					<asp:PlaceHolder runat="server" ID="phResultFromTo">
						Results <asp:Label runat="server" ID="lblFrom" />
						to <asp:Label runat="server" ID="lblTo" /> are displayed.
					</asp:PlaceHolder></p>
				
				<uc1:Pager runat="server" id="rptPager" CurrentPage="1" OnPageChanged="Pager_PageChanged" />
				
				<uc1:FacetAndFilter runat="server" id="FacetAndFilter" OnFilterChanged="FacetAndFilter_OnFilterChanged" />

				<asp:Repeater runat="server" ID="rptResults">
					<ItemTemplate>
						<div class="result">
							<h4 runat="server">
								<uc1:Stars runat="server" id="Stars" Score='<%# Eval("NormalizedScore") %>' />
								<asp:Literal runat="server" Text='<%# (((MyResult)Container.DataItem).Ordinal + 1).ToString() + ". " + (((MyResult)Container.DataItem).Highlights["title"] ?? Eval("Title") ?? "[Untitled]") %>' />
							</h4>
							<p>
								<uc1:Highlight runat="server" id="Highlight" HighlightField="content" MaxLength="300" />
							</p>
							<asp:HyperLink runat="server" NavigateUrl='<%# Eval("Url") %>' Text='<%# ((MyResult)Container.DataItem).Highlights["url"] ?? Eval("Url") %>' />
						</div>
					</ItemTemplate>
				</asp:Repeater>
			</asp:PlaceHolder>
		</asp:PlaceHolder>

	</form>
</body>
</html>
