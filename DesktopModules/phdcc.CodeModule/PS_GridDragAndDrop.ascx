<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_GridDragAndDrop.ascx.vb"
    Inherits="PS_GridDragAndDrop" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">
                <!--
    function onRowDropping(sender, args) {
        if (sender.get_id() == "<%=grdPendingOrders.ClientID %>") {
            var node = args.get_destinationHtmlElement();
            if (!isChildOf('<%=grdShippedOrders.ClientID %>', node) && !isChildOf('<%=grdPendingOrders.ClientID %>', node)) {
                args.set_cancel(true);
            }
        }
        else {
            var node = args.get_destinationHtmlElement();
            if (!isChildOf('trashCan', node)) {
                args.set_cancel(true);
            }
            else {
                if (confirm("Are you sure you want to delete this order?"))
                    args.set_destinationHtmlElement($get('trashCan'));
                else
                    args.set_cancel(true);
            }
        }
    }

    function isChildOf(parentId, element) {
        while (element) {
            if (element.id && element.id.indexOf(parentId) > -1) {
                return true;
            }
            element = element.parentNode;
        }
        return false;
    }
                    -->
</script>
<style type="text/css">
    .leftGrid
    {
        width: 300px;
        border: 1px solid red;
        float: left;
    }
    
    .RightGrid
    {
        width: 300px;
        border: 1px solid red;
        float: right;
    }
</style>
<div class="leftGrid">
    <telerik:RadGrid runat="server" ID="grdPendingOrders" OnNeedDataSource="grdPendingOrders_NeedDataSource"
        AllowPaging="True" Width="350px" OnRowDrop="grdPendingOrders_RowDrop" AllowMultiRowSelection="true"
        PageSize="30" EnableHeaderContextMenu="true">
        <MasterTableView DataKeyNames="ID" TableLayout="Fixed">
            <Columns>
                <telerik:GridTemplateColumn>
                    <ItemTemplate>
                        <asp:HiddenField ID="hidID" runat="server" Value='<%# Eval("ID")%>' /> 
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridDragDropColumn HeaderStyle-Width="18px" Visible="true" />                                
            </Columns>            
        </MasterTableView>
        <ClientSettings AllowRowsDragDrop="True" AllowColumnsReorder="true" ReorderColumnsOnClient="true">
            <Resizing AllowColumnResize="true" />
            <Selecting AllowRowSelect="True" EnableDragToSelectRows="false" />
            <ClientEvents OnRowDropping="onRowDropping" />
            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
        </ClientSettings>
        <PagerStyle Mode="NumericPages" PageButtonCount="4" />
        
    </telerik:RadGrid>
</div>
<div class="RightGrid">
    <telerik:RadGrid runat="server" AllowPaging="True" ID="grdShippedOrders" OnNeedDataSource="grdShippedOrders_NeedDataSource"
        Width="350px" OnRowDrop="grdShippedOrders_RowDrop" AllowMultiRowSelection="true">
        <MasterTableView DataKeyNames="ID" Width="100%">
            <Columns>
                <telerik:GridDragDropColumn HeaderStyle-Width="18px" Visible="true" />
            </Columns>
            <NoRecordsTemplate>
                <div style="height: 30px; cursor: pointer;">
                    No items to view</div>
            </NoRecordsTemplate>
            <PagerStyle Mode="NumericPages" PageButtonCount="4" />
        </MasterTableView>
        <ClientSettings AllowRowsDragDrop="True">
            <Selecting AllowRowSelect="True" EnableDragToSelectRows="false" />
            <ClientEvents OnRowDropping="onRowDropping" />
        </ClientSettings>
    </telerik:RadGrid>
</div>
