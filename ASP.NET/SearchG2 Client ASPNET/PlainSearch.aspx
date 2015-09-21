<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlainSearch.aspx.cs" Inherits="CrownPeak.SearchG2.Client.PlainSearch" %>
<%@ Import Namespace="CrownPeak.SearchG2.Result" %>
<%@ Import Namespace="SolrNet.Impl" %>
<%@ Import Namespace="SolrNet.Utils" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>CrownPeak SearchG2 ASP.NET Example</title>
	<link rel="stylesheet" type="text/css" href="styles.css"/>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:TextBox runat="server" ID="txtSearch" placeholder="Search" autofocus="autofocus" />
			<asp:HiddenField runat="server" ID="hidPage" Value="1"/>
			<asp:HiddenField runat="server" ID="hidFilterQueries" Value=""/>
			<asp:HiddenField runat="server" ID="hidLoggingId" Value=""/>
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

				<asp:Repeater runat="server" ID="rptFilters" Visible="False">
					<HeaderTemplate><div class="filters">Active filters:<br/></HeaderTemplate>
					<ItemTemplate>
						<asp:Label runat="server" CssClass="filter" Text='<%# Eval("Field") + ": " + Eval("Value") %>'></asp:Label>
						<asp:LinkButton runat="server" CssClass="removefilter" CommandName="RemoveFilter" 
							OnClick="RemoveFilter_Click" CommandArgument='<%# Eval("Field") + ":" + Eval("Value") %>'>X</asp:LinkButton>
					</ItemTemplate>
					<FooterTemplate></div></FooterTemplate>
				</asp:Repeater>

				<p>Showing search results for: 
					"<asp:Label runat="server" ID="lblQuery" />"</p>
				<p>Your search term produced
					<asp:Label runat="server" ID="lblCount" />
					results. Results <asp:Label runat="server" ID="lblFrom" />
					to <asp:Label runat="server" ID="lblTo" /> are displayed.</p>
				
				<asp:Repeater runat="server" ID="rptPager">
					<ItemTemplate>
						<asp:LinkButton runat="server" CssClass='<%# "page " + (Container.DataItem.ToString().Equals(hidPage.Value) ? "current" : "") %>' Text='<%# Container.DataItem %>'
							 OnClick="Pager_Click" CommandName="CurrentPage" CommandArgument='<%# Container.DataItem %>'/>
					</ItemTemplate>
					<SeparatorTemplate> | </SeparatorTemplate>
				</asp:Repeater>
				

				<asp:Repeater runat="server" ID="rptFacets">
					<HeaderTemplate><div class="facets">Facets:<br/></HeaderTemplate>
					<ItemTemplate>
						<asp:Label runat="server" Text='<%# Eval("Key") %>'></asp:Label>: 
						<asp:Repeater runat="server" DataSource='<%# Eval("Values") %>'>
							<ItemTemplate>
								<asp:LinkButton runat="server" CssClass="facet" Text='<%# Eval("Value") %>'
									OnClick="AddFilter_Click" CommandName="Facet" CommandArgument='<%# Eval("Key") + ":" + Eval("Value") %>'></asp:LinkButton>
								(<asp:Label runat="server" Text='<%# Eval("Count") %>'></asp:Label>)</ItemTemplate>
							<SeparatorTemplate>, </SeparatorTemplate>
						</asp:Repeater>
					</ItemTemplate>
					<SeparatorTemplate><br/></SeparatorTemplate>
					<FooterTemplate></div></FooterTemplate>
				</asp:Repeater>

				<asp:Repeater runat="server" ID="rptResults">
					<ItemTemplate>
						<div class="result">
							<h4 runat="server">
								<asp:Literal runat="server" Text='<%# ((PlainResult)Container.DataItem).Highlights["title"] %>' Visible='<%# ((PlainResult)Container.DataItem).Highlights.ContainsKey("title") %>'></asp:Literal>
								<asp:Literal runat="server" Text='<%# ((PlainResult)Container.DataItem)["title"] %>' Visible='<%# !((PlainResult)Container.DataItem).Highlights.ContainsKey("title") && ((PlainResult)Container.DataItem).ContainsKey("title") %>'></asp:Literal>
								<asp:Literal runat="server" Text="[Untitled]" Visible='<%# !((PlainResult)Container.DataItem).Highlights.ContainsKey("title") && !((PlainResult)Container.DataItem).ContainsKey("title") %>'></asp:Literal>
							</h4>
							<p>
								<asp:Repeater runat="server" DataSource='<%# ((PlainResult)Container.DataItem).Highlights["content"] %>' Visible='<%# ((PlainResult)Container.DataItem).Highlights.ContainsKey("content") %>'>
									<HeaderTemplate>... </HeaderTemplate>
									<ItemTemplate><%# Container.DataItem %></ItemTemplate>
									<SeparatorTemplate> ... </SeparatorTemplate>
									<FooterTemplate> ...</FooterTemplate>
								</asp:Repeater>
								<asp:Literal runat="server" Text='<%# ((PlainResult)Container.DataItem)["content"].ToString().Length > 300 ? ((PlainResult)Container.DataItem)["content"].ToString().Substring(0, 297) + "..." : ((PlainResult)Container.DataItem)["content"].ToString() %>' Visible='<%# !((PlainResult)Container.DataItem).Highlights.ContainsKey("content") && ((PlainResult)Container.DataItem).ContainsKey("content") %>'></asp:Literal>
								<asp:Literal runat="server" Text="[No content]" Visible='<%# !((PlainResult)Container.DataItem).Highlights.ContainsKey("content") && !((PlainResult)Container.DataItem).ContainsKey("content") %>'></asp:Literal>
							</p>
							<asp:HyperLink runat="server" NavigateUrl='<%# ((PlainResult)Container.DataItem)["url"] %>' Text='<%# ((PlainResult)Container.DataItem).Highlights["url"] %>' Visible='<%# ((PlainResult)Container.DataItem).Highlights.ContainsKey("url") %>' />
							<asp:HyperLink runat="server" NavigateUrl='<%# ((PlainResult)Container.DataItem)["url"] %>' Text='<%# ((PlainResult)Container.DataItem)["url"] %>' Visible='<%# !((PlainResult)Container.DataItem).Highlights.ContainsKey("url") %>' />
						</div>
					</ItemTemplate>
				</asp:Repeater>
			</asp:PlaceHolder>
		</asp:PlaceHolder>

	</form>
</body>
</html>
