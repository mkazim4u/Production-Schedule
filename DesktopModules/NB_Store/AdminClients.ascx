<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminClients.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminClients" %>
<%@ Register TagPrefix="nbs" TagName="Clients" Src="Clients.ascx"%>
<table class="NBright_ContentDiv"><tr><td>
<asp:Panel ID="pnlList" runat="server">
<nbs:Clients runat="server" id="Clients1" />
</asp:Panel>
<asp:Panel ID="pnlEdit" runat="server">
    <div class="NBright_ButtonDiv">
    <asp:LinkButton ID="cmdUpdate" resourcekey="cmdUpdate" cssclass="NBright_SaveButton" runat="server">LinkButton</asp:LinkButton>&nbsp;
    <asp:LinkButton ID="cmdReturn2" runat="server" cssclass="NBright_ReturnButton" resourcekey="cmdReturn" text="Return"></asp:LinkButton>
    </div> 
<div class="NBright_ActionDiv">
                <asp:LinkButton ID="cmdResetPass" cssclass="NBright_ActionButton" runat="server" resourcekey="cmdResetPass">LinkButton</asp:LinkButton>&nbsp;
                <asp:LinkButton ID="cmdUnlock" cssclass="NBright_ActionButton" runat="server" resourcekey="cmdUnlock">LinkButton</asp:LinkButton>&nbsp;
                <asp:LinkButton ID="cmdViewOrders" cssclass="NBright_ActionButton" runat="server" resourcekey="cmdViewOrders">LinkButton</asp:LinkButton>
                <asp:LinkButton ID="cmdCreateOrder" cssclass="NBright_ActionButton" runat="server" resourcekey="cmdCreateOrder">LinkButton</asp:LinkButton>
</div>
<div class="NBright_EditDiv">
                <asp:Label ID="lblUserID" resourcekey="lblUserID" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="lblViewUserID" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="lblUserName" resourcekey="lblUserName" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="lblViewUserName" runat="server" Text=""></asp:Label>
<br />
                <asp:Label ID="lblFirstName" resourcekey="lblFirstName" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="lblViewFirstName" runat="server" Text=""></asp:Label>
<br />
                <asp:Label ID="lblLastName" resourcekey="lblLastName" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="lblViewLastName" runat="server" Text=""></asp:Label>
<br />
                <asp:Label ID="lblDisplayName" resourcekey="lblDisplayName" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="lblViewDisplayName" runat="server" Text=""></asp:Label>
<br />
                <asp:Label ID="lblEmail" resourcekey="lblEmail" runat="server" Text="Label"></asp:Label>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
</div>


</asp:Panel>
</td></tr>
</table>
