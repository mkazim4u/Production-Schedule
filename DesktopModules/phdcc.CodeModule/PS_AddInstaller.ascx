<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_AddInstaller.ascx.vb"
    Inherits="PS_AddInstaller" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls"
    Assembly="DotNetNuke" %>
<style type="text/css">
.msgblue
{
	color:blue;
}    
</style>
<table id="sub1" border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td style="height: 13px">
            <asp:Label ID="lblMessage" runat="server" Width="100%" CssClass="msgblue"></asp:Label>
        </td>
    </tr>
    <tr>
        <td width="30%">
            <asp:Label ID="Label1" runat="server" Text="Creating Tags For Production Schedule" />
        </td>
        <td>
            <asp:Button ID="btnCreateTags" runat="server" Text="Create Tags" Width="161px" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Create Production Schedule Pages" />
        </td>
        <td>
            <asp:Button ID="btnAddPages" runat="server" Text="Create Pages" Width="161px" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Delete Production Schedule Pages" />
        </td>
        <td>
            <asp:Button ID="btnDeletePages" runat="server" Text="Delete Pages" Width="161px" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            <Portal:TabPermissionsGrid ID="PermGrid" runat="server" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
