<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectLang.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.SelectLang" %>
<asp:DataList ID="dlLanguages" runat="server" RepeatColumns="5" BackColor="White">
    <ItemTemplate>
        <asp:HiddenField ID="hidCultureCode" runat="server" Value="" />
        <asp:LinkButton ID="cmdFlagLang" runat="server" commandname="Change"></asp:LinkButton>
    </ItemTemplate>
</asp:DataList>