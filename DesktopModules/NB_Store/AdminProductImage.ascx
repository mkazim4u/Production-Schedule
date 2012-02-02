<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminProductImage.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminProductImage" %>
<%@ Register TagPrefix="dnntc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="nwb" TagName="ShowSelectLang" Src="controls/ShowSelectLang.ascx"%>
<div class="NBright_ButtonDiv">
<asp:label id="lblBrowse" runat="server" resourcekey="lblBrowse">Label</asp:label><asp:FileUpload ID="cmdBrowse" runat="server" />&nbsp;&nbsp;&nbsp;
 <asp:linkbutton id="cmdAddImage" runat="server" cssclass="NBright_AddButton" Resourcekey="Add">Add</asp:linkbutton>
</div>
<br />
<asp:label id="lblMsg" runat="server"></asp:label>
<asp:datagrid id="dgImages" runat="server" AutoGenerateColumns="False" CellPadding="1" Width="100%" GridLines="None">
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
		<Columns>
			<asp:BoundColumn DataField="ImageID" HeaderText="ID" Visible="false"></asp:BoundColumn>
				<dnntc:ImageCommandColumn KeyField="ImageID" ShowImage="True" ImageURL="~/images/delete.gif" CommandName="Delete"
					EditMode="Command">
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<HeaderTemplate></HeaderTemplate>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>
			<asp:TemplateColumn HeaderText="Hidden" Visible="false">
				<ItemTemplate>
					<asp:CheckBox id="chkHide" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Hidden") %>'></asp:CheckBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ImgProduct">
				<ItemTemplate>
					<asp:Image id="imgProduct" runat="server"></asp:Image>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Description">
			<HeaderTemplate>
			<nwb:ShowSelectLang id="ShowSelectLang" runat="server"></nwb:ShowSelectLang>
			<asp:Label ID="nlDescription" runat="server" Text="Description."></asp:Label>
			</HeaderTemplate>
				<ItemTemplate>
					<asp:TextBox id="txtImageDesc" runat="server" Width="400px" MaxLength="200" Text='<%# DataBinder.Eval(Container, "DataItem.ImageDesc") %>'></asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="ListOrder">
				<ItemTemplate>
					<asp:TextBox id="txtListOrder" runat="server" Width="62px" MaxLength="3" Text='<%# DataBinder.Eval(Container, "DataItem.ListOrder") %>'></asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:datagrid>
