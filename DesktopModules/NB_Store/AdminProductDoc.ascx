<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminProductDoc.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminProductDoc" %>
<%@ Register TagPrefix="dnntc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="nwb" TagName="ShowSelectLang" Src="controls/ShowSelectLang.ascx"%>
<div class="NBright_ButtonDiv">
    <asp:Label ID="lblBrowse" runat="server" resourcekey="lblBrowse">Label</asp:Label>
    <asp:FileUpload ID="cmdBrowse" runat="server" />&nbsp;&nbsp;&nbsp;
    <asp:LinkButton ID="cmdAddDoc" runat="server" CssClass="NBright_AddButton" Resourcekey="Add">Add</asp:LinkButton>&nbsp;&nbsp;
                    <asp:TextBox ID="txtSearch" runat="server" Width="169px"></asp:TextBox>
                    <asp:LinkButton ID="cmdSearch" CssClass="NBright_CommandButton" resourcekey="cmdSearch"
                        runat="server">Search</asp:LinkButton>
</div>
    <asp:DataGrid ID="dgSelectDoc" runat="server" AutoGenerateColumns="False" CellPadding="2"
        GridLines="None" PageSize="25" Width="100%" AllowPaging="True">
        <HeaderStyle CssClass="NBright_HeaderStyle" />
        <FooterStyle CssClass="NBright_FooterStyle" />
        <EditItemStyle CssClass="NBright_EditItemStyle" />
        <SelectedItemStyle CssClass="NBright_SelectedItemStyle" />
        <PagerStyle CssClass="NBright_PagerStyle" Mode="NumericPages" />
        <AlternatingItemStyle CssClass="NBright_AlternatingItemStyle" />
        <ItemStyle CssClass="NBright_ItemStyle" />
        <Columns>
            <dnntc:ImageCommandColumn KeyField="" ShowImage="True" ImageURL="~/images/edit.gif"
                CommandName="Edit" EditMode="Command">
                <HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True"
                    HorizontalAlign="Center"></HeaderStyle>
                <EditItemTemplate>
                </EditItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                </ItemTemplate>
            </dnntc:ImageCommandColumn>
            <asp:BoundColumn DataField="FileName" HeaderText="Name"></asp:BoundColumn>
            <asp:BoundColumn DataField="DocDesc" HeaderText="Description"></asp:BoundColumn>
            <asp:BoundColumn DataField="DocPath" HeaderText="DocPath" Visible="false"></asp:BoundColumn>
            <asp:BoundColumn DataField="FileExt" HeaderText="FileExt" Visible="false"></asp:BoundColumn>
        </Columns>
    </asp:DataGrid>
<br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
    <asp:DataGrid ID="dgDocs" runat="server" AutoGenerateColumns="False" CellPadding="1"
        Width="100%" GridLines="None">
        <HeaderStyle CssClass="NBright_HeaderStyle" />
        <FooterStyle CssClass="NBright_FooterStyle" />
        <EditItemStyle CssClass="NBright_EditItemStyle" />
        <SelectedItemStyle CssClass="NBright_SelectedItemStyle" />
        <PagerStyle CssClass="NBright_PagerStyle" Mode="NumericPages" />
        <AlternatingItemStyle CssClass="NBright_AlternatingItemStyle" />
        <ItemStyle CssClass="NBright_ItemStyle" />
        <Columns>
            <asp:BoundColumn DataField="DocID" HeaderText="ID" Visible="false"></asp:BoundColumn>
            <dnntc:ImageCommandColumn KeyField="DocID" ShowImage="True" ImageURL="~/images/delete.gif"
                CommandName="Delete" EditMode="Command">
                <HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True"
                    HorizontalAlign="Center"></HeaderStyle>
                <EditItemTemplate>
                </EditItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                </ItemTemplate>
            </dnntc:ImageCommandColumn>
            <asp:TemplateColumn HeaderText="Hidden" Visible="false">
                <ItemTemplate>
                    <asp:CheckBox ID="chkHide" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Hidden") %>'>
                    </asp:CheckBox>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Name">
			<HeaderTemplate>
			<nwb:ShowSelectLang id="ShowSelectLang" runat="server"></nwb:ShowSelectLang>
			<asp:Label ID="nlName" runat="server" Text="Name."></asp:Label>
			</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="txtDocName" runat="server" Width="150px" MaxLength="50" Text='<%# System.Web.HttpUtility.UrlDecode(DataBinder.Eval(Container, "DataItem.FileName")) %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Description">
			<HeaderTemplate>
			<nwb:ShowSelectLang id="ShowSelectLang2" runat="server"></nwb:ShowSelectLang>
			<asp:Label ID="nlDescription" runat="server" Text="Description."></asp:Label>
			</HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="txtDocDesc" runat="server" Width="400px" MaxLength="200" Text='<%# DataBinder.Eval(Container, "DataItem.DocDesc") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="ListOrder">
                <ItemTemplate>
                    <asp:TextBox ID="txtListOrder" runat="server" Width="62px" MaxLength="3" Text='<%# DataBinder.Eval(Container, "DataItem.ListOrder") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>