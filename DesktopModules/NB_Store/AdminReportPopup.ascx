<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminReportPopup.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminReportPopup" %>
<asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" EnableViewState="False"></asp:Label>
<br />
    <asp:DataGrid id="dgResults" runat="server" AutoGenerateColumns="True" gridlines="None" cellpadding="1" Width="100%" AllowPaging="False" EnableViewState="False">
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
    </asp:DataGrid>
<br />
