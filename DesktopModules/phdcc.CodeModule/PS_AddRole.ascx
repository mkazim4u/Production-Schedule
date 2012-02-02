<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_AddRole.ascx.vb" Inherits="PS_AddRole" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls"
    Assembly="DotNetNuke" %>

<link href="~/module.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    </style>
<table id="table1" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Role Name"></asp:Label>
            <asp:TextBox ID="txtRoleName" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Description"></asp:Label>
            <asp:TextBox ID="txtRoleDescription" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnSave" runat="server" Text="Save" Visible="true"/>
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
        </td>
    </tr>
</table>
