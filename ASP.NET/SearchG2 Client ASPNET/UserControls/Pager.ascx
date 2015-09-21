<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="CrownPeak.SearchG2.Client.UserControls.Pager" %>
<asp:Repeater runat="server" ID="Repeater1">
	<ItemTemplate>
		<asp:LinkButton runat="server" CssClass='<%# "page " + (Container.DataItem.ToString().Equals(CurrentPage.ToString()) ? "current" : "") %>' Text='<%# Container.DataItem %>'
				OnClick="Pager_Click" CommandName="CurrentPage" CommandArgument='<%# Container.DataItem %>'/>
	</ItemTemplate>
	<SeparatorTemplate> | </SeparatorTemplate>
</asp:Repeater>