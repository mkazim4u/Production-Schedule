<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CategoryMenu.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.CategoryMenu" %>
<div class="CategoryMenuWrapper">
<asp:PlaceHolder ID="phRootHead" runat="server"></asp:PlaceHolder>
    <asp:DataList ID="dlRootMenu" runat="server">
        <ItemTemplate>
            <asp:PlaceHolder ID="phCatLink" runat="server"></asp:PlaceHolder>
        </ItemTemplate>
        <SeparatorTemplate>
         </SeparatorTemplate>
    </asp:DataList>
   <asp:PlaceHolder ID="phSecSep1" runat="server"></asp:PlaceHolder>
   <asp:Label ID="lblBreadcrumbs" runat="server" Text="Breadcrumbs"></asp:Label>
   <asp:PlaceHolder ID="phSecSep2" runat="server"></asp:PlaceHolder>
   <asp:PlaceHolder ID="phSubHead" runat="server"></asp:PlaceHolder>
   <asp:DataList ID="dlCategoryMenu" runat="server" RepeatDirection="Horizontal">
        <ItemTemplate>
            <asp:PlaceHolder ID="phCatLink" runat="server"></asp:PlaceHolder>
        </ItemTemplate>
        <SeparatorTemplate>        
         </SeparatorTemplate>
    </asp:DataList>
   <asp:PlaceHolder ID="phSecSep3" runat="server"></asp:PlaceHolder>
   <asp:PlaceHolder ID="phTreeHead" runat="server"></asp:PlaceHolder>
   <asp:PlaceHolder ID="phTreeMenu" runat="server"></asp:PlaceHolder>
</div>