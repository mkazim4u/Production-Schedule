<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_TariffItem.ascx.vb"
    Inherits="PS_TariffItem" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">

        function RowContextMenu(sender, eventArgs) {

            var menu;
            var ownerTable = eventArgs.get_tableView();

            menu = $find("<%= rcMenu.ClientID %>");

            document.getElementById("radGridClickedRowIndex").value = eventArgs.get_itemIndexHierarchical();

            document.getElementById("radGridClickedTableId").value = ownerTable._data.UniqueID;

            menu.show(eventArgs.get_domEvent());



        }
    </script>
</telerik:RadCodeBlock>
<%--<telerik:RadAjaxLoadingPanel ID="alpTariffItem" runat="server" Height="75px" MinDisplayTime="5"
    Width="75px">
    <asp:Image ID="Image2" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel ID="rapTariffItem" RequestQueueSize="5" runat="server" Width="100%"
    OnAjaxRequest="rapPSMain_AjaxRequest" EnableOutsideScripts="True" HorizontalAlign="NotSet"
    ScrollBars="None" LoadingPanelID="alpTariffItem">--%>
<telerik:RadGrid ID="rgTariffItem" runat="server" Width="95%" ShowStatusBar="true"
    AutoGenerateColumns="False" PageSize="20" AllowSorting="True" AllowMultiRowSelection="False"
    AllowPaging="True" GridLines="None">
    <PagerStyle Mode="NumericPages"></PagerStyle>
    <MasterTableView Width="100%" DataKeyNames="ID" AllowMultiColumnSorting="True" CommandItemDisplay="Top"
        Name="Tariff">
        <Columns>
            <%--<telerik:GridTemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                        CssClass="lnkButton" ToolTip="Edit">
                        <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Edit.gif" /></asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnCopy" runat="server" CausesValidation="False" CommandName="Copy"
                        CssClass="lnkButton" ToolTip="Copy">
                        <asp:Image ID="imgCopy" runat="server" ImageUrl="~/Portals/0/Images/Copy.jpg" /></asp:LinkButton>
                </ItemTemplate>
            </telerik:GridTemplateColumn>--%>
            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                ItemStyle-Width="10px" EditImageUrl="~/Portals/0/Images/Edit.gif">
            </telerik:GridEditCommandColumn>
            <%--            <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="CopyCommandColumn"
                ItemStyle-Width="20px" CommandName="Copy" ImageUrl="~/Portals/0/Images/copy.jpg">
            </telerik:GridButtonColumn>--%>
            <telerik:GridBoundColumn SortExpression="ID" HeaderText="ID" HeaderButtonType="TextButton"
                DataField="ID" UniqueName="ID" ReadOnly="true" Visible="false">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="TariffName" HeaderText="Tariff Name" HeaderButtonType="TextButton"
                DataField="TariffName" UniqueName="TariffName" ReadOnly="true">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="RecordType" HeaderText="Rec Type" HeaderButtonType="TextButton"
                DataField="RecordType" UniqueName="RecordType" ReadOnly="true" Visible="false">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="JobID" HeaderText="Job No" HeaderButtonType="TextButton"
                DataField="JobID" UniqueName="JobID" ReadOnly="true" Visible="false">
            </telerik:GridBoundColumn>
            <%--            <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="DeleteCommandColumn"
                ConfirmText="Are you sure you to delete this group ?" ItemStyle-Width="20px"
                CommandName="Delete" ImageUrl="~/Portals/0/Images/Delete.png">
            </telerik:GridButtonColumn>--%>
        </Columns>
        <EditFormSettings EditFormType="Template" InsertCaption="New Tariff">
            <FormTemplate>
                <div>
                    <asp:Label ID="lblTariffName" runat="server" Text="Tariff Name" CssClass="fieldsetControlStyle"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="txtTariffName" runat="server" Text='<%# Eval("TariffName") %>'></asp:TextBox>
                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName='<%# ToggleCommand() %>'
                        CssClass="lnkButton" ToolTip="Update">
                        <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                        CssClass="lnkButton" ToolTip="Cancel">
                        <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" /></asp:LinkButton>
                </div>
            </FormTemplate>
        </EditFormSettings>
        <DetailTables>
            <telerik:GridTableView DataKeyNames="GroupID" HierarchyLoadMode="Client" Width="100%"
                CommandItemDisplay="Top" runat="server" Name="Groups">
                <Columns>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                        ItemStyle-Width="20px" EditImageUrl="~/Portals/0/Images/Edit.gif">
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn SortExpression="GroupID" HeaderText="GroupID" HeaderButtonType="TextButton"
                        DataField="GroupID" UniqueName="GroupID" ReadOnly="true" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn SortExpression="GroupName" HeaderText="Group Name" HeaderButtonType="TextButton"
                        DataField="GroupName" UniqueName="GroupName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn SortExpression="TariffID" HeaderText="TariffID" HeaderButtonType="TextButton"
                        DataField="TariffID" UniqueName="TariffID" ReadOnly="true" Visible="false">
                    </telerik:GridBoundColumn>
                    <%--                    <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="DeleteCommandColumn"
                        ConfirmText="Are you sure you to delete this group ?" ItemStyle-Width="20px"
                        CommandName="Delete" ImageUrl="~/Portals/0/Images/Delete.png">
                    </telerik:GridButtonColumn>--%>
                </Columns>
                <EditFormSettings EditFormType="Template" InsertCaption="New Group">
                    <FormTemplate>
                        <div>
                            <asp:Label ID="lblGroupName" runat="server" Text="Group Name" CssClass="fieldsetControlStyle"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txtGroupName" runat="server" Text='<%# Eval("groupname") %>'></asp:TextBox>
                            <br />
                            <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName='<%# IIf( DataBinder.Eval(Container, "OwnerTableView.IsItemInserted"), "PerformInsert", "Update") %>'
                                CssClass="lnkButton">
                                <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                CssClass="lnkButton" ToolTip="Cancel">
                                <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" /></asp:LinkButton>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <CommandItemSettings AddNewRecordText="Add New Group" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
                    ShowRefreshButton="false" />
                <DetailTables>
                    <telerik:GridTableView DataKeyNames="ID" Width="100%" Name="Items" CommandItemDisplay="Top"
                        runat="server" HierarchyLoadMode="Client">
                        <Columns>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                ItemStyle-Width="20px" EditImageUrl="~/Portals/0/Images/Edit.gif">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn SortExpression="ID" HeaderText="ID" HeaderButtonType="TextButton"
                                DataField="ID" UniqueName="ID" ReadOnly="true" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="ItemOrder" HeaderText="ItemOrder" HeaderButtonType="TextButton"
                                DataField="ItemOrder" UniqueName="ItemOrder" ReadOnly="true" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="ItemName" HeaderText="Item Name" DataField="ItemName"
                                UniqueName="ItemName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="Units" HeaderText="Rate" DataField="Units"
                                UniqueName="Units">
                                <ItemStyle Width="20%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="CostPrice" HeaderText="Cost Price" DataField="CostPrice"
                                UniqueName="CostPrice">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="SellingPrice" HeaderText="Selling Price"
                                DataField="SellingPrice" UniqueName="SellingPrice">
                            </telerik:GridBoundColumn>
                            <%--                            <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="DeleteCommandColumn"
                                ConfirmText="Are you sure you to delete this group ?" ItemStyle-Width="20px"
                                CommandName="Delete" ImageUrl="~/Portals/0/Images/Delete.png">
                            </telerik:GridButtonColumn>--%>
                        </Columns>
                        <CommandItemSettings AddNewRecordText="Add New Item" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
                            ShowRefreshButton="false" />
                        <EditFormSettings EditFormType="Template" InsertCaption="New Group">
                            <FormTemplate>
                                <div>
                                    <asp:Label ID="lblItemName" runat="server" Text="Item Name" CssClass="fieldsetControlStyle"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtItemName" runat="server" Text='<%# Eval("ItemName") %>'></asp:TextBox>
                                    <br />
                                    <asp:Label ID="lblCostPrice" runat="server" Text="Cost Price" CssClass="fieldsetControlStyle"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <telerik:RadNumericTextBox ID="rntbCostPrice" MinValue="0" runat="server" MaxLength="6"
                                        DbValue='<%# Bind("CostPrice") %>' Width="160px" Type="Currency" Culture="English (United Kingdom)">
                                    </telerik:RadNumericTextBox>
                                    <br />
                                    <asp:Label ID="lblSellingPrice" runat="server" Text="Selling Price" CssClass="fieldsetControlStyle"></asp:Label>
                                    &nbsp;
                                    <telerik:RadNumericTextBox ID="rntbSellingPrice" MinValue="0" runat="server" MaxLength="6"
                                        DbValue='<%# Bind("SellingPrice") %>' Width="160px" Type="Currency" Culture="English (United Kingdom)">
                                    </telerik:RadNumericTextBox>
                                    <br />
                                    <asp:Label ID="lblRate" runat="server" Text="Rate" CssClass="fieldsetControlStyle"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="txtRate" runat="server" Text='<%# Eval("Units") %>'></asp:TextBox>
                                    <br />
                                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName='<%# IIf( DataBinder.Eval(Container, "OwnerTableView.IsItemInserted"), "PerformInsert", "Update") %>'
                                        CssClass="lnkButton" ToolTip="Update">
                                        <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                        CssClass="lnkButton" ToolTip="Cancel">
                                        <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" /></asp:LinkButton>
                                </div>
                            </FormTemplate>
                        </EditFormSettings>
                    </telerik:GridTableView>
                </DetailTables>
            </telerik:GridTableView>
        </DetailTables>
        <CommandItemSettings AddNewRecordText="Add new Tariff" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
            ShowRefreshButton="false" ShowExportToPdfButton="true" />
    </MasterTableView>
    <ClientSettings AllowKeyboardNavigation="true">
        <ClientEvents OnRowContextMenu="RowContextMenu" />
        <Selecting AllowRowSelect="true" />
        <KeyboardNavigationSettings EnableKeyboardShortcuts="true" AllowSubmitOnEnter="true"
            AllowActiveRowCycle="true" CollapseDetailTableKey="LeftArrow" ExpandDetailTableKey="RightArrow" />
    </ClientSettings>
</telerik:RadGrid>
<telerik:RadContextMenu ID="rcMenu" runat="server" OnItemClick="rcMenu_ItemClick"
    EnableRoundedCorners="true" EnableShadows="true">
    <Items>
        <telerik:RadMenuItem Text="Add" />
        <telerik:RadMenuItem Text="Edit" />
        <telerik:RadMenuItem Text="Delete" />
        <telerik:RadMenuItem Text="Move Up" />
        <telerik:RadMenuItem Text="Move Down" />
    </Items>
</telerik:RadContextMenu>
<%--</telerik:RadAjaxPanel>--%>
<input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex" />
<input type="hidden" id="radGridClickedTableId" name="radGridClickedTableId" />
