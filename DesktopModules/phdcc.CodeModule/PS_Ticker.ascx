<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_Ticker.ascx.vb" Inherits="PS_Ticker" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadTicker AutoStart="true" runat="server" ID="Radticker1" Loop="true">
    <Items>
        <telerik:RadTickerItem>TURKEY: Stocks go up after a surge in investor confidence.</telerik:RadTickerItem>
        <telerik:RadTickerItem>ROMANIA: Privatization drives the stock market this week.</telerik:RadTickerItem>
        <telerik:RadTickerItem>BULGARIA: Government plans for a new wave of privatization.</telerik:RadTickerItem>
    </Items>
</telerik:RadTicker>
