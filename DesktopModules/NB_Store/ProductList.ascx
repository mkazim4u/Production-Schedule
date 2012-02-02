<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProductList.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.ProductList" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="nbs" TagName="CatMenu" Src="CategoryMenu.ascx"%>
<asp:PlaceHolder ID="phProductModuleHeader" runat="server"></asp:PlaceHolder>
<asp:Panel ID="pnlProductList" runat="server">
<asp:PlaceHolder ID="phProductListHeader" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="plhCatMsg" runat="server"></asp:PlaceHolder>
    <div class="NBrightWrapperProdCatMenu">
    <nbs:CatMenu id="CategoryMenu" runat="server"></nbs:CatMenu>	
    </div>
    <asp:DataList ID="dlProductList" runat="server">
        <ItemTemplate>
        </ItemTemplate>
    </asp:DataList>
<asp:Label ID="lblLineBreak" runat="server" Text="<br/>"></asp:Label>
<dnn:pagingcontrol id=ctlPagingControl runat="server"></dnn:pagingcontrol>
</asp:Panel>
<asp:Panel ID="pnlSPO" runat="server">
<asp:PlaceHolder ID="phProductDetailHeader" runat="server"></asp:PlaceHolder>
    <asp:DataList ID="dlProductDetail" runat="server">
        <ItemTemplate>
        </ItemTemplate>
        <SeparatorTemplate>
        <div></div>
         </SeparatorTemplate>
    </asp:DataList>
    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
</asp:Panel>