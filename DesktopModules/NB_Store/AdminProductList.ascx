<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminProductList.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminProductList" %>
<%@ Register TagPrefix="dnntc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="nbs" Namespace="NEvoWeb.Modules.NB_Store" Assembly="NEvoweb.DNN.Modules.NB_Store" %>
<asp:Panel ID="pnlSearchFocus" runat="server" DefaultButton="cmdSearch">
<div class="NBright_ButtonDiv">
<asp:LinkButton ID="cmdReturn" runat="server"  
    cssclass="NBright_ReturnButton" text="Return."></asp:LinkButton>
<asp:LinkButton ID="cmdAdd" runat="server"  
    cssclass="NBright_AddButton" resourcekey="cmdAdd" text="Add"></asp:LinkButton>
</div>
<div class="NBright_SelectDiv">
            	<asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList>&nbsp;
                <asp:TextBox ID="txtSearch" runat="server" Width="169px"></asp:TextBox>
                <asp:LinkButton ID="cmdSearch" cssclass="NBright_CommandButton" resourcekey="cmdSearch" runat="server">Search</asp:LinkButton>
</div>
</asp:Panel>
<asp:Panel ID="pnlGrid" runat="server">
    <asp:PlaceHolder ID="phPadding" runat="server"></asp:PlaceHolder>
    <asp:DataGrid id="dgProducts" runat="server" AutoGenerateColumns="False" gridlines="None" cellpadding="2"
            PageSize="25" Width="100%" AllowPaging="False">
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
			<Columns>
				<dnntc:ImageCommandColumn KeyField="ProductID" ShowImage="True" ImageURL="~/images/edit.gif" CommandName="Edit"
					EditMode="Command" >
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>
			<asp:BoundColumn DataField="ProductID" HeaderText="ID" Visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="ProductRef" HeaderText="Ref"></asp:BoundColumn>
			<asp:BoundColumn DataField="ProductName" HeaderText="Name"></asp:BoundColumn>
			<asp:BoundColumn DataField="Featured" HeaderText=""></asp:BoundColumn>
			<asp:BoundColumn DataField="Archived" HeaderText=""></asp:BoundColumn>
			<asp:BoundColumn DataField="IsDeleted" HeaderText=""></asp:BoundColumn>
			<asp:BoundColumn DataField="IsHidden" HeaderText=""></asp:BoundColumn>
				<dnntc:ImageCommandColumn KeyField="ProductID" ShowImage="True" ImageURL="~/images/copy.gif" CommandName="Copy"
					EditMode="Command" HeaderText="Copy">
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>
			</Columns>
			<PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
		</asp:DataGrid>
</asp:Panel>
<asp:Panel ID="pnlList" runat="server">
    <asp:DataList ID="dlProducts" runat="server" RepeatDirection="Horizontal" ></asp:DataList>
</asp:Panel>
<asp:Label ID="lblLineBreak" runat="server" Text="<br/>"></asp:Label>
<nbs:AdminPagingControl id="ctlPagingControl" runat="server" pagesize="25" BorderWidth="0"></nbs:AdminPagingControl>
