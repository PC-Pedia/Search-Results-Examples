<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RSS.aspx.cs" Inherits="CrownPeak.SearchG2.Client.RSS" %>
<rss version="2.0">
	<channel>
		<title>CrownPeak.com RSS example</title>
		<link>http://www.crownpeak.com</link>
		<description>CrownPeak - Web Experience Management for the Modern Marketer</description>
		<asp:Repeater runat="server" ID="rptItems">
			<ItemTemplate>
				<item>
					<title><%# (Eval("Title") ?? "[Untitled]").ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") %></title>
					<link><%# Eval("Url").ToString().Replace("&", "&amp;") %></link>
					<description><![CDATA[<%# Eval("Content") %>]]></description>
					<pubDate><%# ((DateTime)Eval("Date")).ToString("ddd',' d MMM yyyy HH':'mm':'ss ") + ((DateTime)Eval("date")).ToString("zzzz").Replace(":", "") %></pubDate>
				</item>
			</ItemTemplate>
		</asp:Repeater>
	</channel>
</rss>