<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Clients.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.Clients" %>
<%@ Register TagPrefix="dnntc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<div class="NBright_SelectDiv">
<asp:TextBox ID="txtSearch" runat="server" Width="251px"></asp:TextBox>
<asp:LinkButton ID="cmdSearch" cssclass="NBright_CommandButton" resourcekey="cmdSearch" runat="server">Search</asp:LinkButton>
</div> 
    <asp:DataGrid ID="dgUserList" runat="server" AutoGenerateColumns="False" showfooter="true" width="100%"  gridlines="None" cellpadding="2" 
    AllowPaging="True" PageSize="25">
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />        
        <Columns>
		<dnntc:ImageCommandColumn KeyField="UserID" ShowImage="True" ImageURL="~/images/edit.gif" CommandName="Edit"
			EditMode="Command">
			<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
			<EditItemTemplate></EditItemTemplate>
			<ItemStyle HorizontalAlign="Center"></ItemStyle>
			<HeaderTemplate></HeaderTemplate>
			<ItemTemplate></ItemTemplate>
		</dnntc:ImageCommandColumn>
        <asp:BoundColumn DataField="UserID" HeaderText="UserID" headerstyle-cssclass="NormalBold">
<HeaderStyle CssClass="NormalBold"></HeaderStyle>
            </asp:BoundColumn>
        <asp:BoundColumn DataField="UserName" HeaderText="UserName" headerstyle-cssclass="NormalBold">
<HeaderStyle CssClass="NormalBold"></HeaderStyle>
            </asp:BoundColumn>
        <asp:BoundColumn DataField="DisplayName" HeaderText="DisplayName" headerstyle-cssclass="NormalBold">
<HeaderStyle CssClass="NormalBold"></HeaderStyle>
            </asp:BoundColumn>
        <asp:BoundColumn DataField="Email" HeaderText="Email" headerstyle-cssclass="NormalBold">
<HeaderStyle CssClass="NormalBold"></HeaderStyle>
            </asp:BoundColumn>
        </Columns>
    </asp:DataGrid>
            