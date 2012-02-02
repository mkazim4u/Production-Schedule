<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_Quotation.ascx.vb"
    Inherits="PS_Quotation" %>
<%@ Register TagPrefix="DNN" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    .lblTotalCost
    {
        width: 95%;
        display: none;
    }
    
    .lblTotalSelling
    {
        position: relative;
        margin: 10px;
        padding: 9px;
        left: 250px;
    }
</style>
<style type="text/css">
    .DetailTable_Default Caption
    {
        background-color: #b0c4de !important;
    }
</style>
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

        function RowDblClick(sender, eventArgs) {
            editedRow = eventArgs.get_itemIndexHierarchical();
            $find("<%= rgQuotation.MasterTableView.ClientID %>").editItem(editedRow);
        }


       

    </script>
    <script type="text/javascript">

        var sumInput = null;
        var tempValue = 0.0;

        function Load(sender, args) {
            sumInput = sender;
        }
        function Blur(sender, args) {
            sumInput.set_value(tempValue + sender.get_value());
        }
        function Focus(sender, args) {
            tempValue = sumInput.get_value() - sender.get_value();
        } 
                    
    </script>
</telerik:RadCodeBlock>
<%--<asp:Button ID="clrFilters" runat="server" Text="Clear filters"></asp:Button>--%>
<div id="divCopyQuotation_Outer" runat="server">
    <DNN:SectionHead runat="server" ID="shCopyQuotation" IsExpanded="false" Section="divCopyQuotation_Inner"
        Text="Copy / Create Quotation" IncludeRule="true" Visible="true" />
    <div id="divCopyQuotation_Inner" runat="server">
        <asp:Label ID="lblFilter" runat="server" Text="Select Filter" CssClass="fieldsetControlStyle">
        </asp:Label>
        &nbsp;&nbsp;
        <telerik:RadComboBox ID="rcbFilter" runat="server" Skin="Vista" Height="100px" Width="200px"
            AutoPostBack="true" MarkFirstMatch="True" HighlightTemplatedItems="True" DropDownWidth="200px">
            <Items>
                <telerik:RadComboBoxItem Text="- Please Select -" Value="-1" />
                <telerik:RadComboBoxItem Text="Tariff" Value="0" Selected="true"  />
                <telerik:RadComboBoxItem Text="Quotations" Value="1" />
            </Items>
        </telerik:RadComboBox>
        <br />
        <br />
        <telerik:RadGrid ID="rgFilter" runat="server" Width="95%" AutoGenerateColumns="false"
            AllowFilteringByColumn="false" AllowPaging="true" AllowSorting="true" Skin="Office2007">
            <PagerStyle Mode="NumericPages"></PagerStyle>
            <MasterTableView CommandItemDisplay="None" CurrentResetPageIndexAction="SetPageIndexToFirst"
                Dir="LTR" Frame="Border" TableLayout="Auto" Name="Quotation">
                <Columns>
                    <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="Copy" Visible="false"
                        ItemStyle-Width="50px" ConfirmText="Do you want to create a new Quotation ?"
                        ConfirmTitle="Copy & Create" HeaderText="Copy" HeaderTooltip="Copy" CommandName="Copy"
                        ItemStyle-HorizontalAlign="Center" ImageUrl="~/Portals/0/Images/Copy.jpg">
                    </telerik:GridButtonColumn>
                    <%-- <ItemStyle Width="30%" />--%>
                    <telerik:GridBoundColumn SortExpression="JobID" HeaderText="Job ID" HeaderButtonType="TextButton"
                        DataField="JobID" UniqueName="JobID" ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn>
                        <ItemTemplate>
                            <asp:Label ID="lblTariffName" runat="server" Text='<%# Eval("TariffName") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="80%">
                                <tr>
                                    <td id="tbSearch" style="width: 150px" runat="server">
                                        <asp:Image ID="imgSearch" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Portals/0/Images/Search.png" />
                                    </td>
                                </tr>
                            </table>
                            <telerik:RadToolTip ID="rttMore" runat="server" TargetControlID="tbSearch" RelativeTo="Element"
                                Skin="Office2007" ShowCallout="true" HideDelay="4000" HideEvent="LeaveTargetAndToolTip"
                                Position="TopCenter" Width="200px" Height="90px">
                                <fieldset id="fsMain" runat="server">
                                    <legend class="fieldsetLegend" id="fslgndMain" runat="server">Select Filter </legend>
                                    Search by Quotation/Tariff <b>name / number</b>
                                    <telerik:RadTextBox ID="tbSearchByQuotationName" EmptyMessage="Leave empty for all Quotations or enter a Quotation Name / Job No (Press Enter Key for search)"
                                        Width="450px" runat="server" CssClass="txt" OnTextChanged="btnGoSearchByQuotationName_Click" />
                                    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnGoSearchByQuotationName">
                                        <asp:Button ID="btnGoSearchByQuotationName" runat="server" Text="Go" OnClick="btnGoSearchByQuotationName_Click" />
                                    </asp:Panel>
                                </fieldset>
                            </telerik:RadToolTip>
                        </HeaderTemplate>
                        <%-- <ItemStyle Width="30%" />--%>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn SortExpression="TariffName" HeaderText="Quotation Name"
                        Visible="false" HeaderButtonType="TextButton" DataField="TariffName" UniqueName="TariffName"
                        ReadOnly="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Locked Date" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <asp:Label ID="lblLockedDate" runat="server" Text='<%# Eval("LockedDateTime") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--                    <telerik:GridBoundColumn SortExpression="LockedDateTime" HeaderText="Locked Date/Time" DataFormatString="{0:dd-MMM-yyyy}" 
                        HeaderButtonType="TextButton" DataField="LockedDateTime" UniqueName="LockedDateTime"
                        ReadOnly="true">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hidID" Value='<%# Eval("ID") %>' runat="server" />
                            <asp:HiddenField ID="hidCustomerID" Value='<%# Eval("CustomerID") %>' runat="server" />
                            <asp:HiddenField ID="hidJobID" Value='<%# Eval("JobID") %>' runat="server" />
                            <asp:HiddenField ID="hidRecordType" Value='<%# Eval("RecordType") %>' runat="server" />
                            <asp:HiddenField ID="hidLockedDateTime" Value='<%# Eval("LockedDateTime") %>' runat="server" />
                        </ItemTemplate>
                        <%-- <ItemStyle Width="30%" />--%>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <PagerStyle Mode="NumericPages" />
            <ExportSettings>
                <Pdf FontType="Subset" PaperSize="Letter" />
                <Excel Format="Html" />
                <Csv ColumnDelimiter="Comma" RowDelimiter="NewLine" />
            </ExportSettings>
        </telerik:RadGrid>
        <%--</asp:UpdatePanel>--%>
    </div>
</div>
<br />
<div id="divQuotation_Outer" runat="server">
    <DNN:SectionHead runat="server" ID="shQuotation" IsExpanded="true" Section="divQuotation_Inner"
        Text="Quotation" IncludeRule="true" Visible="true" />
    <div id="divQuotation_Inner" runat="server">
        <telerik:RadGrid ID="rgQuotation" runat="server" Width="95%" ShowStatusBar="true"
            Skin="Office2007" Font-Names="Verdana" AutoGenerateHierarchy="true" HierarchyLoadMode="ServerBind"
            AutoGenerateColumns="False" AllowSorting="True" AllowMultiRowSelection="False"
            AllowPaging="false" ShowFooter="True">
            <PagerStyle Mode="NumericPages"></PagerStyle>
            <MasterTableView Width="100%" DataKeyNames="ID" AllowMultiColumnSorting="True" CommandItemDisplay="Top"
                HierarchyDefaultExpanded="true" HierarchyLoadMode="ServerOnDemand" Name="Quotation"
                CssClass="DetailTable_Default">
                <Columns>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                        ItemStyle-Width="30px" EditImageUrl="~/Portals/0/Images/Edit.gif">
                        <ItemStyle BackColor="GreenYellow" />
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn SortExpression="ID" HeaderText="ID" HeaderButtonType="TextButton"
                        DataField="ID" UniqueName="ID" ReadOnly="true" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn SortExpression="TariffName" HeaderText="Quotation Name"
                        HeaderButtonType="TextButton" DataField="TariffName" UniqueName="TariffName"
                        ReadOnly="true">
                        <ItemStyle BackColor="GreenYellow" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn SortExpression="RecordType" HeaderText="Rec Type" HeaderButtonType="TextButton"
                        DataField="RecordType" UniqueName="RecordType" ReadOnly="true" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn SortExpression="JobID" HeaderText="Job No" HeaderButtonType="TextButton"
                        DataField="JobID" UniqueName="JobID" ReadOnly="true" Visible="false">
                    </telerik:GridBoundColumn>
                </Columns>
                <EditFormSettings EditFormType="Template" InsertCaption="New Tariff">
                    <FormTemplate>
                        <div>
                            <asp:Label ID="lblTariffName" runat="server" Text="Quotation Name" CssClass="fieldsetControlStyle">
                            </asp:Label>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txtTariffName" runat="server" Text='<%# Eval("TariffName") %>'>
                            </asp:TextBox>
                            <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName='<%# IIf( DataBinder.Eval(Container, "OwnerTableView.IsItemInserted"), "PerformInsert", "Update") %>'
                                CssClass="lnkButton" ToolTip="Update">
                                <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" />
                            </asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                CssClass="lnkButton" ToolTip="Cancel">
                                <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" />
                            </asp:LinkButton>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <DetailTables>
                    <telerik:GridTableView DataKeyNames="GroupID" HierarchyLoadMode="ServerBind" Width="100%"
                        HierarchyDefaultExpanded="true" CommandItemDisplay="Top" runat="server" Name="Groups"
                        CssClass="DetailTable_Default">
                        <Columns>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                ItemStyle-Width="40px" EditImageUrl="~/Portals/0/Images/Edit.gif">
                                <ItemStyle BackColor="Yellow" />
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn SortExpression="GroupID" HeaderText="GroupID" HeaderButtonType="TextButton"
                                DataField="GroupID" UniqueName="GroupID" ReadOnly="true" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="GroupName" HeaderText="Group Name" HeaderButtonType="TextButton"
                                DataField="GroupName" UniqueName="GroupName">
                                <ItemStyle Font-Bold="true" BackColor="Yellow" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="TariffID" HeaderText="TariffID" HeaderButtonType="TextButton"
                                DataField="TariffID" UniqueName="TariffID" ReadOnly="true" Visible="false">
                                <ItemStyle Width="0%" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="ShowTotal">
                                <ItemStyle Width="125px" HorizontalAlign="Left" BackColor="Yellow" />
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalCost" runat="server" Text="" Font-Bold="true" ForeColor="White"
                                        Font-Italic="true">
                                    </asp:Label>
                                    <%--left: 260px;
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    &nbsp;
                                    <asp:Label ID="lblTotalSelling" runat="server" Text="Total Selling" Font-Bold="true"
                                        Visible="false">
                                    </asp:Label>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <EditFormSettings EditFormType="Template" InsertCaption="New Group">
                            <FormTemplate>
                                <div>
                                    <asp:Label ID="lblGroupName" runat="server" Text="Group Name" CssClass="fieldsetControlStyle">
                                    </asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtGroupName" runat="server" Text='<%# Eval("groupname") %>'>
                                    </asp:TextBox>
                                    <br />
                                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName='<%# IIf( DataBinder.Eval(Container, "OwnerTableView.IsItemInserted"), "PerformInsert", "Update") %>'
                                        CssClass="lnkButton">
                                        <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" />
                                    </asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                        CssClass="lnkButton" ToolTip="Cancel">
                                        <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" />
                                    </asp:LinkButton>
                                </div>
                            </FormTemplate>
                        </EditFormSettings>
                        <CommandItemSettings AddNewRecordText="Add New Group" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
                            ShowRefreshButton="false" />
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="ID" Width="100%" Name="Items" CommandItemDisplay="Top"
                                AlternatingItemStyle-BackColor="SkyBlue" CssClass="DetailTable_Default!" EditMode="InPlace"
                                runat="server" HierarchyLoadMode="Client">
                                <Columns>
                                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                        UpdateImageUrl="~/Portals/0/Images/Update.gif" ItemStyle-Width="40px" EditImageUrl="~/Portals/0/Images/Edit.gif"
                                        CancelImageUrl="~/Portals/0/Images/Cancel.gif">
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridBoundColumn SortExpression="ID" HeaderText="ID" HeaderButtonType="TextButton"
                                        DataField="ID" UniqueName="ID" ReadOnly="true" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="ItemOrder" HeaderText="ItemOrder" HeaderButtonType="TextButton"
                                        DataField="ItemOrder" UniqueName="ItemOrder" ReadOnly="true" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <%--                            <telerik:GridBoundColumn SortExpression="ItemName" HeaderText="Item Name" DataField="ItemName"
                                UniqueName="ItemName">
                                UniqueName="ItemName">
                            </telerik:GridBoundColumn>--%>
                                    <telerik:GridTemplateColumn HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtItemName" runat="server" Text='<%# Eval("ItemName") %>' MaxLength="100">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtItemName" runat="server" ErrorMessage="!" Display="Dynamic"
                                                ControlToValidate="txtItemName">
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Rate" UniqueName="Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnits" runat="server" Text='<%# Eval("Units") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadNumericTextBox ID="rntbRate" ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true"
                                                IncrementSettings-InterceptMouseWheel="true" runat="server" Type="Number" Width="80px"
                                                DbValue='<%# Eval("Units") %>' NumberFormat-DecimalDigits="2" MinValue="1" IncrementSettings-Step="1"
                                                CssClass="fieldsetControlWidth fieldsetLine" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadNumericTextBox ID="rntbQuantity" ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true"
                                                IncrementSettings-InterceptMouseWheel="true" runat="server" Type="Number" Width="80px"
                                                DbValue='<%# Eval("Quantity") %>' NumberFormat-DecimalDigits="0" MinValue="0"
                                                IncrementSettings-Step="1" CssClass="fieldsetControlWidth fieldsetLine">
                                                <ClientEvents OnBlur="Blur" OnFocus="Focus" />
                                            </telerik:RadNumericTextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="C.P" UniqueName="CostPrice">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCostPrice" runat="server" Text='<%# Format(DataBinder.Eval(Container.DataItem,"CostPrice"),"###,##0.00") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadNumericTextBox ID="rntbCostPrice" ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true"
                                                IncrementSettings-InterceptMouseWheel="true" runat="server" Type="Number" Width="80px"
                                                DbValue='<%# Eval("CostPrice") %>' NumberFormat-DecimalDigits="2" MinValue="0"
                                                IncrementSettings-Step="1" CssClass="fieldsetControlWidth fieldsetLine" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="S.P" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSellingPrice" runat="server" Text='<%# Format(DataBinder.Eval(Container.DataItem,"SellingPrice"),"###,##0.00") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadNumericTextBox ID="rntbSellingPrice" ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true"
                                                IncrementSettings-InterceptMouseWheel="true" runat="server" Type="Number" Width="80px"
                                                DbValue='<%# Eval("SellingPrice") %>' NumberFormat-DecimalDigits="2" MinValue="0"
                                                IncrementSettings-Step="1" CssClass="fieldsetControlWidth fieldsetLine" />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridCalculatedColumn HeaderText="C.P Total" DataType="System.Double" UniqueName="TotalCostPrice"
                                        FooterStyle-Font-Bold="true" FooterStyle-ForeColor="White" DataFields="CostPrice,Units,Quantity"
                                        Expression="{0}/{1}*{2}" Aggregate="Sum" DataFormatString="{0:###,##0.00}" FooterAggregateFormatString="{0:£###,##0.00}" />
                                    <telerik:GridCalculatedColumn HeaderText="S.P Total " UniqueName="TotalSellingPrice"
                                        FooterStyle-ForeColor="White" FooterStyle-Font-Bold="true" Visible="false" DataFields="SellingPrice,Units,Quantity"
                                        Expression="{0}/{1}*{2}" DataType="System.Double" FooterAggregateFormatString="{0:£###,##0.00}"
                                        FooterText="S.P Total : " Aggregate="Sum" DataFormatString="{0:###,##0.00}" />
                                </Columns>
                                <CommandItemSettings AddNewRecordText="Add New Item" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
                                    ShowRefreshButton="false" />
                            </telerik:GridTableView>
                        </DetailTables>
                    </telerik:GridTableView>
                </DetailTables>
                <CommandItemSettings AddNewRecordText="Add new Quotation" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
                    ShowRefreshButton="false" ShowExportToPdfButton="true"   />
            </MasterTableView>
            <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true">
                <ClientEvents OnRowContextMenu="RowContextMenu" OnRowDblClick="RowDblClick" />
                <Selecting AllowRowSelect="true" />
                <KeyboardNavigationSettings EnableKeyboardShortcuts="true" AllowSubmitOnEnter="true"
                    AllowActiveRowCycle="true" CollapseDetailTableKey="LeftArrow" ExpandDetailTableKey="RightArrow" />
            </ClientSettings>
        </telerik:RadGrid>
        <br />
        <asp:Button ID="btnSaveQuotation" runat="server" Text="Save Quotation" CausesValidation="true" />
        &nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" />
    </div>
</div>
<telerik:RadContextMenu ID="rcMenu" runat="server" OnItemClick="rcMenu_ItemClick"
    EnableRoundedCorners="true" EnableShadows="true">
    <Items>
        <telerik:RadMenuItem Text="Add" />
        <telerik:RadMenuItem Text="Edit" />
        <telerik:RadMenuItem Text="Delete" />
        <telerik:RadMenuItem Text="Copy" />
        <telerik:RadMenuItem Text="Move Up" />
        <telerik:RadMenuItem Text="Move Down" />
    </Items>
</telerik:RadContextMenu>
<input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex" />
<input type="hidden" id="radGridClickedTableId" name="radGridClickedTableId" />
