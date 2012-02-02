<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SNR_Orders.ascx.vb" Inherits="SNR_Orders"
    EnableViewState="true" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    div.RadGrid_Vista, div.RadGrid_Vista .rgMasterTable, div.RadGrid_Vista .rgDetailTable, div.RadGrid_Vista .rgGroupPanel table, div.RadGrid_Vista .rgCommandRow table, div.RadGrid_Vista .rgEditForm table, div.RadGrid_Vista .rgPager table, span.GridToolTip_Vista
    {
        font-family: Verdana;
        font-size: xx-small;
    }
    
    .viewWrap
    {
        padding: 15px;
        background: #2291b5 0 0 url(Img/bluegradient.gif) repeat-x;
    }
    
    .contactWrap
    {
        padding: 10px 15px 15px 15px;
        background: #fff;
        color: #333;
    }
    
    .contactWrap td
    {
        padding: 0 20px 0 0;
    }
    
    .contactWrap td td
    {
        padding: 3px 20px 3px 0;
    }
    
    .contactWrap img
    {
        border: 1px solid #05679d;
    }
    
    .tabStrip
    {
        position: absolute;
        top: 79px;
        left: 101px;
    }
</style>
<telerik:RadFormDecorator ID="rfd" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
<table id="tblSearch" width="100%">
    <tr>
        <td align="left" width="15%">
            <asp:CheckBox ID="chkSearchByShop" runat="server" AutoPostBack="true" Text="Search By Shop" />
        </td>
        <td id="tdShop" align="right" runat="server" visible="false">
            <asp:Label ID="lblShop" Text="" runat="server"></asp:Label>
            <telerik:RadComboBox ID="rcbPortal" Height="100px" runat="server" Skin="Vista" AutoPostBack="true"
                Width="120px" DataTextField="PortalName" DataValueField="PortalID">
            </telerik:RadComboBox>
        </td>
    </tr>
</table>
<%--<div id="divSearch">
    <asp:CheckBox ID="chkSearchByShop" runat="server" AutoPostBack="true" Text="Search By Shop" />
    <div id="divSearchByShop" runat="server" visible="false" align="right">
        <asp:Label ID="lblShop" Text="" runat="server"></asp:Label>
        <telerik:RadComboBox ID="rcbPortal" Height="100px" runat="server" Skin="Vista" AutoPostBack="true"
            Width="120px" DataTextField="PortalName" DataValueField="PortalID">
        </telerik:RadComboBox>
    </div>
</div>
--%>
<%--<telerik:RadAjaxLoadingPanel ID="alpSNROrders" runat="server" Height="75px" MinDisplayTime="5"
    Width="75px">
    <asp:Image ID="Image1" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/3/Images/LoadingAjax.gif" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel ID="rapSNROrders" RequestQueueSize="5" runat="server" Width="100%"
    EnableOutsideScripts="True" HorizontalAlign="NotSet" ScrollBars="None" LoadingPanelID="alpSNROrders">--%>
<telerik:RadGrid ID="rgOrders" runat="server" AutoGenerateColumns="False" Skin="Vista"
    AllowSorting="false" AllowPaging="True" PageSize="10" GridLines="None" ShowGroupPanel="false">
    <MasterTableView AllowMultiColumnSorting="false" Name="Orders" DataKeyNames="OrderID"
        AllowFilteringByColumn="false">
        <Columns>
            <telerik:GridBoundColumn DataField="OrderID" HeaderText="Order ID" SortExpression="OrderID"
                AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" UniqueName="OrderID">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PortalID" HeaderText="Portal ID" SortExpression="PortalID"
                Visible="false" UniqueName="PortalID" HeaderStyle-HorizontalAlign="Center">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="OrderNumber" HeaderText="Order Number" SortExpression="OrderNumber"
                AllowFiltering="false" ReadOnly="true" UniqueName="OrderNumber" HeaderStyle-HorizontalAlign="Center">
            </telerik:GridBoundColumn>
            <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                AllowFiltering="false">
                <HeaderTemplate>
                    <table id="tblOrderStatus" border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td id="tbSearch" style="width: 250px" runat="server">
                                <asp:Label ID="lblHeader" Text="Status" runat="server"></asp:Label>
                                <asp:Image ID="imgSearch" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Portals/3/Images/Search.png" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="tblStatus">
                                    <tr>
                                        <td align="left">
                                            <telerik:RadToolTip ID="rttMore" runat="server" TargetControlID="tbSearch" RelativeTo="Element"
                                                ShowCallout="true" HideDelay="4000" HideEvent="LeaveTargetAndToolTip" Position="TopCenter"
                                                Width="300px" Height="90px">
                                                <fieldset id="fsMain" runat="server">
                                                    <legend class="fieldsetLegend" id="fslgndMain" runat="server">Select Filter </legend>
                                                    <asp:CheckBox ID="chkAwaitingAuthorization" Text="Show <b>Awaiting Authorization</b> Orders"
                                                        OnCheckedChanged="chkAwaitingAuthorization_CheckedChanged" CssClass="searchtxt"
                                                        runat="server" AutoPostBack="true" />
                                                    <br />
                                                    <asp:CheckBox ID="chkApprove" Text="Show <b>Approve</b> Orders" OnCheckedChanged="chkApprove_CheckedChanged"
                                                        CssClass="searchtxt" runat="server" AutoPostBack="true" />
                                                    <br />
                                                    <asp:CheckBox ID="chkAwaitingFulfilment" Text="Show <b>Awaiting Fulfilment</b> Orders"
                                                        OnCheckedChanged="chkAwaitingFulfilment_CheckedChanged" CssClass="searchtxt"
                                                        runat="server" AutoPostBack="true" />
                                                    <br />
                                                    <asp:CheckBox ID="chkFulfilled" Text="Show <b>Fulfilled</b> Orders" OnCheckedChanged="chkFulfilled_CheckedChanged"
                                                        CssClass="searchtxt" runat="server" AutoPostBack="true" />
                                                    <br />
                                                    <asp:CheckBox ID="chkCancelled" Text="Show <b>Cancelled</b> Orders" OnCheckedChanged="chkCancelled_CheckedChanged"
                                                        CssClass="searchtxt" runat="server" AutoPostBack="true" />
                                                </fieldset>
                                            </telerik:RadToolTip>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblStatus" Text="" runat="server"></asp:Label>
                    <asp:HiddenField ID="hidOrderStatusID" runat="server" Value='<%# Bind("OrderStatusID")%>' />
                </ItemTemplate>
                <ItemStyle Width="30%" />
            </telerik:GridTemplateColumn>
            <telerik:GridBoundColumn DataField="OrderDate" HeaderText="Order Date" SortExpression="Order Date"
                AllowFiltering="false" DataFormatString="{0:dd-MMM-yyyy}" UniqueName="OrderDate">
            </telerik:GridBoundColumn>
            <telerik:GridTemplateColumn HeaderText="Shop Name" UniqueName="PortalID" DataField="PortalID"
                SortExpression="PortalID">
                <ItemTemplate>
                    <asp:Image ID="imgPortal" runat="server" ImageAlign="AbsBottom" />
                    <asp:Label ID="lblPortalName" runat="server" Text='<%# Eval("PortalID") %>'></asp:Label>
                </ItemTemplate>
                <%-- <FilterTemplate>
                    <telerik:RadComboBox ID="rcbPortal" Height="200px" AppendDataBoundItems="true" SelectedValue='<%# TryCast(Container,GridItem).OwnerTableView.GetColumn("PortalID").CurrentFilterValue %>'
                        runat="server" OnClientSelectedIndexChanged="TitleIndexChanged" DataTextField="PortalName"
                        DataValueField="PortalID">
                        <Items>
                            <telerik:RadComboBoxItem Text="All" />
                        </Items>
                    </telerik:RadComboBox>
                    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                        <script type="text/javascript">
                            function TitleIndexChanged(sender, args) {
                                var tableView = $find("<%# TryCast(Container,GridItem).OwnerTableView.ClientID %>");
                                tableView.filter("PortalID", args.get_item().get_value(), "EqualTo");

                            }
                        </script>
                    </telerik:RadScriptBlock>
                </FilterTemplate>--%>
            </telerik:GridTemplateColumn>
        </Columns>
        <NestedViewSettings>
            <ParentTableRelation>
                <telerik:GridRelationFields MasterKeyField="OrderID" />
            </ParentTableRelation>
        </NestedViewSettings>
        <NestedViewTemplate>
            <asp:Panel ID="NestedViewPanel" runat="server" CssClass="viewWrap">
                <div class="contactWrap">
                    <asp:HiddenField ID="hidOrderID" runat="server" Value='<%# Eval("OrderID")%>' />
                    <asp:HiddenField ID="hidPortalID" runat="server" Value='<%# Eval("PortalID")%>' />
                    <asp:HiddenField ID="hidShippingAddressID" runat="server" Value='<%# Eval("ShippingAddressID")%>' />
                    <asp:HiddenField ID="hidBillingAddressID" runat="server" Value='<%# Eval("BillingAddressID")%>' />
                    <asp:HiddenField ID="hidOrderStatusID" runat="server" Value='<%# Eval("OrderStatusID")%>' />
                    <telerik:RadTabStrip runat="server" ID="TabStip1" MultiPageID="Multipage1" SelectedIndex="0"
                        Orientation="HorizontalTop" Skin="WebBlue">
                        <Tabs>
                            <telerik:RadTab runat="server" Text="Order Info" PageViewID="PageView1" Width="100px">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Customer Info" PageViewID="PageView2">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Order Log" PageViewID="PageView3">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage runat="server" ID="Multipage1" SelectedIndex="0" RenderSelectedPageOnly="false">
                        <telerik:RadPageView runat="server" ID="PageView1">
                            <fieldset style="padding: 10px;">
                                <legend style="padding: 5px;"><b>Order Info:&nbsp; &nbsp;<%#Eval("OrderNumber") %></b></legend>
                                <b>Items</b>
                                <br />
                                <br />
                                <telerik:RadGrid ID="rgItems" runat="server" Width="100%" AutoGenerateColumns="false"
                                    ShowFooter="true" AllowFilteringByColumn="false" AllowPaging="false" AllowSorting="false"
                                    Skin="Vista">
                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                    <MasterTableView CommandItemDisplay="None" CurrentResetPageIndexAction="SetPageIndexToFirst"
                                        Name="Items">
                                        <Columns>
                                            <telerik:GridBoundColumn SortExpression="ProductName" HeaderText="ProductName" HeaderButtonType="TextButton"
                                                DataField="ProductName" UniqueName="ProductName">
                                                <ItemStyle Width="60%" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="UnitCost" HeaderText="Price" HeaderButtonType="TextButton"
                                                DataField="UnitCost" UniqueName="UnitCost">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="Quantity" HeaderText="Quantity" HeaderButtonType="TextButton"
                                                DataField="Quantity" UniqueName="Quantity">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridCalculatedColumn HeaderText="Total" DataType="System.Double" UniqueName="TotalCostPrice"
                                                FooterStyle-Font-Bold="true" DataFields="UnitCost,Quantity" Aggregate="Sum" Expression="{0}*{1}"
                                                DataFormatString="{0:###,##0.00}" FooterAggregateFormatString="{0:£###,##0.00}" />
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <br />
                                <br />
                                <asp:Label ID="lblRejectStatus" Text="Order has been rejected" runat="server" Visible="false"
                                    Font-Bold="true" ForeColor="Red"></asp:Label>
                                <div id="divOrderStatus" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnApprove" Text="Approve" runat="server" Width="200px" OnClick="btnApprove_Click" />
                                                <asp:Button ID="btnReject" Text="Reject" runat="server" Width="200px" OnClick="btnReject_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divActions" runat="server">
                                    <b>Current Order Status</b>
                                    <br />
                                    <br />
                                    <telerik:RadGrid ID="rgActions" runat="server" Width="100%" AutoGenerateColumns="false"
                                        OnItemCommand="rgActions_ItemCommand" OnItemDataBound="rgActions_ItemDataBound"
                                        OnNeedDataSource="rgActions_NeedDataSource" AllowFilteringByColumn="false" AllowPaging="false"
                                        AllowSorting="false" Skin="Vista">
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                        <MasterTableView CommandItemDisplay="None" CurrentResetPageIndexAction="SetPageIndexToFirst"
                                            Name="Actions">
                                            <Columns>
                                                <telerik:GridBoundColumn SortExpression="OrderStatusID" HeaderText="Order Status ID"
                                                    HeaderButtonType="TextButton" DataField="OrderStatusID" UniqueName="OrderStatusID"
                                                    ReadOnly="true" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="OrderStatusText" HeaderText="Status" HeaderButtonType="TextButton"
                                                    DataField="OrderStatusText" UniqueName="OrderStatusText">
                                                    <ItemStyle Width="88%" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="chkStatus">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkStatus" Checked="false" AutoPostBack="true" runat="server" OnCheckedChanged="chkStatus_CheckedChanged" />
                                                        <asp:HiddenField ID="hidOSID" runat="server" Value='<%# Bind("OrderStatusID")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </fieldset>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="PageView2">
                            <fieldset style="padding: 10px;">
                                <legend style="padding: 5px;"><b>Customer Info:&nbsp; &nbsp;<%# Eval("OrderNumber")%></b></legend>
                                <table id="tblAddress" width="100%" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td width="30%">
                                                                <b>Shipping Address</b>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                First Name:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblS_FirstName" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Last Name:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblS_LastName" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Company:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblS_Company" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Address:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblS_Address" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                City:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblS_City" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Region:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblS_Region" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                PostalCode:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblS_PostalCode" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Country:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblS_CountryCode" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Phone:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblS_Phone" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Mobile No:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblS_MobileNo" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <%--<td width="50%" style="display: none;">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <b>Billing Address</b>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                First Name:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblB_FirstName" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Last Name:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblB_LastName" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Company:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblB_Company" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Address:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblB_Address" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                City:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblB_City" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Region:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblB_Region" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                PostalCode:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblB_PostalCode" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Country:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblB_CountryCode" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Phone:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblB_Phone" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Mobile No:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblB_MobileNo" Text="" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>--%>
                                        </tr>
                                    </tbody>
                                </table>
                            </fieldset>
                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="PageView3">
                            <fieldset style="padding: 10px;">
                                <legend style="padding: 5px;"><b>Order Log:&nbsp; &nbsp;<%#Eval("OrderNumber") %></b></legend>
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td width="100%">
                                            <telerik:RadGrid ID="rgOrderLog" runat="server" Width="100%" AutoGenerateColumns="false"
                                                PageSize="10" OnItemDataBound="rgOrderLog_ItemDataBound" OnNeedDataSource="rgOrderLog_NeedDataSource"
                                                AllowFilteringByColumn="false" AllowPaging="true" AllowSorting="false" Skin="Vista">
                                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                                <MasterTableView CommandItemDisplay="None" CurrentResetPageIndexAction="SetPageIndexToFirst"
                                                    Name="Order Log">
                                                    <Columns>
                                                        <telerik:GridBoundColumn SortExpression="ID" HeaderText="ID" HeaderButtonType="TextButton"
                                                            DataField="ID" UniqueName="ID" ReadOnly="true" Visible="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn SortExpression="SourceID" HeaderText="SourceID" HeaderButtonType="TextButton"
                                                            DataField="SourceID" UniqueName="SourceID" ReadOnly="true" Visible="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn SortExpression="AuditEvent" HeaderText="Audit Event" HeaderButtonType="TextButton"
                                                            DataField="AuditEvent" UniqueName="AuditEvent">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn SortExpression="ChangeDetail" HeaderText="Status" HeaderButtonType="TextButton"
                                                            DataField="ChangeDetail" UniqueName="ChangeDetail">
                                                            <ItemStyle Width="50%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Updated By">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn SortExpression="CreatedOn" HeaderText="Updated On" DataFormatString="{0:dd-MMM-yyyy}"
                                                            HeaderButtonType="TextButton" DataField="CreatedOn" UniqueName="CreatedOn">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                </div>
            </asp:Panel>
        </NestedViewTemplate>
    </MasterTableView>
    <PagerStyle Mode="NumericPages"></PagerStyle>
    <ClientSettings />
</telerik:RadGrid>
<%--</telerik:RadAjaxPanel>--%>
