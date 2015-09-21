<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FacetAndFilter.ascx.cs" Inherits="CrownPeak.SearchG2.Client.UserControls.FacetAndFilter" %>
<asp:HiddenField runat="server" ID="hidFilterQueries"/>
<asp:Repeater runat="server" ID="rptFilters">
	<HeaderTemplate><div class="filters">Active filters:<br/></HeaderTemplate>
	<ItemTemplate>
		<asp:Label runat="server" CssClass="filter" Text='<%# Eval("Field") + ": " + Eval("Value") %>'></asp:Label>
		<asp:LinkButton runat="server" CssClass="removefilter" CommandName="RemoveFilter" 
			OnClick="RemoveFilter_Click" CommandArgument='<%# Eval("Field") + ":" + Eval("Value") %>'>X</asp:LinkButton>
	</ItemTemplate>
	<FooterTemplate>
		<asp:PlaceHolder runat="server" ID="phRemoveAllFilters">
			<span class="removefilter removeall">(<asp:LinkButton runat="server" OnClick="RemoveAllFilters_Click">Remove All</asp:LinkButton>)</span>
		</asp:PlaceHolder>
		</div>
	</FooterTemplate>
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
