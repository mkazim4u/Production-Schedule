<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_NBStoreRotator.ascx.vb"
    Inherits="PS_NBStoreRotator" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
 
    .rotWrapper
    {
        position:relative;
        top: -5;
        left:50px;
        border: 0px solid black;
    }
    
    .RotatorImage
    {
        position: relative;
        left:-25px;
        top:-25px;
        border: 1px solid #ccc;
        background-color: white;
        padding: 9px;
        margin-bottom: 5px;
        display: block;
        
    }
    
    .rotatorBackground
    {
        margin-left: auto;
        margin-right: auto;
        width: 605px;
        height: 93px;
        font-family: Arial;
        padding-top: 18px;
    }
    

</style>
<telerik:RadRotator ID="RadRotator1" RotatorType="CarouselButtons" Width="500px"
    Height="200px" ScrollDuration="500" FrameDuration="2000" ItemWidth="150px" ItemHeight="150px"
    CssClass="rotWrapper" ScrollDirection="Left,Right" runat="server">
    <ItemTemplate>
        <div>
            <asp:Image ID="Image1" runat="server" ImageUrl='<%# Container.DataItem %>' ImageAlign="Middle"
                CssClass="RotatorImage" Height="150px" Width="150px" />
        </div>
    </ItemTemplate>
</telerik:RadRotator>

<table id="tblPrintJobList" width="100%" border="0" cellpadding="0" cellspacing="0">
<tr>
<td class="Label DisableCartFlag DisablePricesFlag"><span class="Value">[Cart:OrderTotal]</span> Total</td>
<td class="WishListFlag"><a class="ViewWishList Button" href="/tabid/[Setting:wishlist.tab]/wishlist/Default.aspx">View Wishlist ([WishList:ItemCount])</a></td>
<td class="DisableCartFlag"><a class="Checkout Button" href="/tabid/[Setting:checkout.tab]/Default.aspx">Go to Checkout></a></td>

</tr>
 
    <li class="WishListFlag"><a class="ViewWishList Button" href="/tabid/[Setting:wishlist.tab]/wishlist/Default.aspx">View Wishlist ([WishList:ItemCount])</a></li>
    <li class="DisableCartFlag"><a class="Checkout Button" href="/tabid/[Setting:checkout.tab]/Default.aspx">Go to Checkout</a>
    <!-- <img src="/DesktopModules/NB_Store/img/cart.png" style="vertical-align:text-top" /></li> -->
    </li>

