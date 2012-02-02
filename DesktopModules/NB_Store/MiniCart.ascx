<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MiniCart.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.MiniCart" %>
<%@ Register TagPrefix="nbs" TagName="CartList" Src="CartList.ascx" %>
<div class="MiniCartWrapper">
<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
<nbs:CartList runat="server" id="cartlist1" />
<asp:PlaceHolder id="PlaceHolder2" runat="server"></asp:PlaceHolder>
</div>
