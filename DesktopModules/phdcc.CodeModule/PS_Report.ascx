<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_Report.ascx.vb" Inherits="PS_Report" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=5.0.11.510, Culture=neutral, PublicKeyToken=A9D7983DFCC261BE"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<script type="text/javascript">
    //<!--
    function setMenuItemsState(menuItems, treeNode) {
        for (var i = 0; i < menuItems.get_count(); i++) {
            var menuItem = menuItems.getItem(i);
            switch (menuItem.get_value()) {
                case "Copy":
                    formatMenuItem(menuItem, treeNode, 'Copy "{0}"');
                    break;
                case "Rename":
                    formatMenuItem(menuItem, treeNode, 'Rename "{0}"');
                    break;                
            }
        }
    }

    function formatMenuItem(menuItem, treeNode, formatString) {
        var nodeValue = treeNode.get_value();
        if (nodeValue && nodeValue.indexOf("_Private_") == 0) {
            menuItem.set_enabled(false);
        }
        else {
            menuItem.set_enabled(true);
        }
        
    }

    function onClientContextMenuShowing(sender, args) {
        var treeNode = args.get_node();
        treeNode.set_selected(true);
        setMenuItemsState(args.get_menu().get_items(), treeNode);
    }

   
</script>
<telerik:RadTreeView runat="server" ID="rtvReports" Skin="Vista" OnNodeClick="rtvReports_NodeClick"
    OnClientContextMenuShowing="onClientContextMenuShowing" OnContextMenuItemClick="rtvReports_ContextMenuItemClick">
    <ContextMenus>
        <telerik:RadTreeViewContextMenu runat="server" ID="Copy" ClickToOpen="True" Skin="Vista">
            <Items>
                <telerik:RadMenuItem Text="Copy To Favourites" Value="Copy">
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Remove" Value="Remove">
                </telerik:RadMenuItem>
            </Items>
        </telerik:RadTreeViewContextMenu>
    </ContextMenus>
    <Nodes>
        <telerik:RadTreeNode Text="Reports" Expanded="true" Value="_Private_Reports">
            <Nodes>
                <telerik:RadTreeNode Text="Cost Reports" Expanded="true" Value="_Private_CostReports">
                    <Nodes>
                        <telerik:RadTreeNode Text="Total Cost Report" Expanded="true">
                        </telerik:RadTreeNode>
                    </Nodes>
                </telerik:RadTreeNode>
                <telerik:RadTreeNode Text="Favourite Reports" Expanded="true" Value="_Private_FavouriteReports">
                </telerik:RadTreeNode>
            </Nodes>
        </telerik:RadTreeNode>
    </Nodes>
</telerik:RadTreeView>
<br />
<br />
<hr />
<asp:ValidationSummary ID="vs" runat="server" ValidationGroup="vg" />
<div id="divCostReport" runat="server" visible="false">
    <asp:Label ID="lblDateFrom" Text="Date From" runat="server"></asp:Label>
    <telerik:RadDatePicker ID="rdpFromDate" runat="server" DateInput-DateFormat="ddd d-MMM-yyyy"
        Skin="Vista" Width="150px" DateInput-EmptyMessage="from">
        <Calendar ID="Calendar1" runat="server" Skin="Vista">
            <SpecialDays>
                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#ccccff">
                    <ItemStyle BackColor="#CCCCFF"></ItemStyle>
                </telerik:RadCalendarDay>
            </SpecialDays>
        </Calendar>
        <DateInput DisplayDateFormat="ddd d-MMM-yyyy" DateFormat="ddd d-MMM-yyyy" EmptyMessage="from"
            runat="server">
        </DateInput>
        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
    </telerik:RadDatePicker>
    <telerik:RadToolTip ID="rttFromDate" runat="server" TargetControlID="rdpFromDate"
        CssClass="tooltipBackColor" RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip"
        ShowCallout="true" Position="TopRight" Width="150px" Height="100px">
        <div id="divFromDate" class="tooltip">
        </div>
    </telerik:RadToolTip>
    <asp:Button ID="btnGo" runat="server" Text="View Report" OnClick="btnGo_Click" Style="float: right" />
    <br />
    <asp:Label ID="lblDateTo" Text="Date To" runat="server"></asp:Label>
    &nbsp;&nbsp;&nbsp;
    <telerik:RadDatePicker ID="rdpToDate" runat="server" DateInput-DateFormat="ddd d-MMM-yyyy"
        Skin="Vista" Width="150px" DateInput-EmptyMessage="to">
        <Calendar ID="Calendar2" runat="server" Skin="Vista">
            <SpecialDays>
                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#ccccff">
                    <ItemStyle BackColor="#CCCCFF"></ItemStyle>
                </telerik:RadCalendarDay>
            </SpecialDays>
        </Calendar>
        <DateInput DisplayDateFormat="ddd d-MMM-yyyy" DateFormat="ddd d-MMM-yyyy" EmptyMessage="to"
            runat="server">
        </DateInput>
        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
    </telerik:RadDatePicker>
    <telerik:RadToolTip ID="rttToDate" runat="server" TargetControlID="rdpToDate" CssClass="tooltipBackColor"
        RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
        Position="TopRight" Width="150px" Height="100px">
        <div id="divToDate" class="tooltip">
        </div>
    </telerik:RadToolTip>
    <asp:CompareValidator ID="cv1" runat="server" ControlToValidate="rdpToDate" Display="Dynamic"
        ValidationGroup="vg" ForeColor="Red" ControlToCompare="rdpFromDate" Operator="GreaterThanEqual"
        Type="Date" ErrorMessage="Please Enter Valid Date Range<br />">
    </asp:CompareValidator>
    <br />
    <br />
</div>
<telerik:ReportViewer ID="rptViewer" runat="server" Height="600px" Width="100%">
</telerik:ReportViewer>
