<%@ Control Language="VB" AutoEventWireup="true" CodeFile="PS_Cleanup.ascx.vb" Inherits="PS_Cleanup" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls" Assembly="DotNetNuke" %>

<link href="module.css" rel="stylesheet" type="text/css" />
<style type="text/css">
</style>
<asp:Button ID="btnCleanup" runat="server" Text="Cleanup" />
<br />
<br />
<asp:TextBox ID="tbLog" runat="server" Rows="10" TextMode="MultiLine" Width="100%"></asp:TextBox>

