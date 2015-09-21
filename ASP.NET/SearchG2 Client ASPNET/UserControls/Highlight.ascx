<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Highlight.ascx.cs" Inherits="CrownPeak.SearchG2.Client.UserControls.Highlight" %>
<asp:Repeater ID="Repeater1" runat="server">
	<HeaderTemplate>... </HeaderTemplate>
	<ItemTemplate><%# Container.DataItem %></ItemTemplate>
	<SeparatorTemplate> ... </SeparatorTemplate>
	<FooterTemplate> ...</FooterTemplate>
</asp:Repeater>
<asp:Literal ID="Literal1" runat="server"></asp:Literal>
