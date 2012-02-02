<%@ Control Language="VB" AutoEventWireup="true" CodeFile="PS_CreateEditJob.ascx.vb" Inherits="PS_CreateEditJob" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="DNN" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<link href="module.css" rel="stylesheet" type="text/css" />

<style type="text/css">
    .rcbJobTemplateleftDiv
    {
        float: left;
        text-align: center;
    }
    .rcbJobTemplaterightDiv
    {
        float: right;
        text-align: center;
    }
</style>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">

        function requestStart(sender, eventArgs) {


            if (eventArgs.get_eventTarget().indexOf("btnImport") != -1 || eventArgs.get_eventTarget().indexOf("btnCancel") != -1)
                eventArgs.set_enableAjax(false);


        }

        function CloseCustomerWindow() {

            GetCustomerWindow().close();
        }


        function CloseBasketWindow() {

            GetBasketWindow().close();
            return false;
        }

        function CloseEventLogWindow() {

            GetEventLogWindow().close();


        }

        //        function pageLoad() {
        //            $addHandler(document, "keydown", onKeyDown);
        //        }


        //        function onKeyDown(e) {
        //            if (!e) e = window.event;
        //            if ((e.keyCode == 27)) {
        //                CloseCustomerWindow();
        //            }
        //        }



        function GetEventLogWindow() {

            var oWindowViewJobLog = $find('<%= rwJobEventLog.ClientID %>');
            return oWindowViewJobLog;
        }

        function GetCustomerWindow() {

            var oWindowCust = $find('<%= rwCustomer.ClientID %>');

            return oWindowCust;
        }

        function GetBasketWindow() {

            var oWindowBasket = $find('<%= rwBasket.ClientID %>');

            return oWindowBasket;
        }



        function showWindow() {

            //alert("kazim")

            //            alert        Sys.Application.remove_load(showWindow);
            var oWindowCust = $find('<%= rwCustomer.ClientID %>');
            alert(GetCustomerWindow());
            GetCustomerWindow().show();
            //            oWindowCust.show();
        }


        //        function GetRadWindow() {
        //            var oWindow = null;
        //            if (window.rwCustomer) oWindow = window.rwCustomer; //Will work in Moz in all cases, including clasic dialog
        //            else if (window.frameElement.rwCustomer) oWindow = window.frameElement.rwCustomer; //IE (and Moz az well)
        //            return oWindow;
        //        }


        function confirmbox() {

            var con = confirm("Are you sure want to delete this job?");
            

            if (con == true) {

                return true;

            }

            else {

                return false;

            }

        }
  

    </script>
</telerik:RadCodeBlock>

<asp:ValidationSummary ID="vs" runat="server" ValidationGroup="vg" ForeColor="Red" />
<fieldset id="fsMain" runat="server">
    <legend class="fieldsetLegend" id="fslgndMain">
        <asp:Label ID="lblCreateOrEditJob" runat="server" Text="Create or Edit Job" />
    </legend>
    <asp:Label ID="lblLegendCustomer" runat="server" Text="Customer" CssClass="fieldsetLabel" />
    <telerik:RadComboBox ID="rcbCustomer" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
        AutoPostBack="True" />
    <asp:LinkButton ID="lbAddCustomer" runat="server" CausesValidation="false" ToolTip="Edit">
        <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Portals/0/Images/Edit.gif" />
    </asp:LinkButton>
    <telerik:RadToolTip ID="rttCustomer" runat="server" TargetControlID="rcbCustomer"
        RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
        Position="TopRight" Width="350px" Height="200px">
        <div id="divCustomer" class="tooltip">
            <asp:Label ID="lblCustomerTooltipData" runat="server" />
        </div>
    </telerik:RadToolTip>
    <br />
    <div id="divCustomerContact" runat="server" visible="true">
        <asp:Label ID="lblLegendContact" runat="server" Text="Contact" CssClass="fieldsetLabel" />
        <telerik:RadComboBox ID="rcbCustomerContact" runat="server" AutoPostBack="true" CssClass="fieldsetControlWidth fieldsetLine"
            EnableViewState="true" />
        <asp:TextBox ID="tbContact" ReadOnly="true" runat="server" CssClass="fieldsetLine" />
        <telerik:RadToolTip ID="rttCustomerContact" runat="server" TargetControlID="rcbCustomerContact"
            RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
            Position="TopRight" Width="350px" Height="200px">
            <div id="divCustomerContactTooltip" class="tooltip">
                <asp:Label ID="lblCustomerContactTooltipData" runat="server" />
            </div>
        </telerik:RadToolTip>
        <br />
    </div>
    <asp:Label ID="lblLegendJobName" runat="server" Text="Job name" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbJobName" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
        ValidationGroup="vg" Width="350px" MaxLength="100" />
    <asp:RequiredFieldValidator ID="rfvJobName" ControlToValidate="tbJobName" runat="server"
        ForeColor="Red" ValidationGroup="vg" ErrorMessage="Please Enter Job Name" ToolTip="Please Enter Job Name"
        Text="«"></asp:RequiredFieldValidator>
    <br />
    <br />
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblLegendCollateralDue" runat="server" Text="Collateral due" CssClass="fieldsetLabel" />
                    <telerik:RadDatePicker ID="rdpCollateralDue" DateInput-DateFormat="ddd d-MMM-yyyy"
                        runat="server" CssClass="fieldsetControlWidth fieldsetLine">
                        <Calendar ID="Calendar2" runat="server">
                            <SpecialDays>
                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#ccccff">
                                </telerik:RadCalendarDay>
                            </SpecialDays>
                        </Calendar>
                    </telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLegendDeadline" runat="server" Text="Deadline" CssClass="fieldsetLabel" />
                    <telerik:RadDatePicker ID="rdpDeadlineOn" DateInput-DateFormat="ddd d-MMM-yyyy" runat="server"
                        CssClass="fieldsetControlWidth fieldsetLine">
                        <Calendar ID="Calendar1" runat="server">
                            <SpecialDays>
                                <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#ccccff">
                                </telerik:RadCalendarDay>
                            </SpecialDays>
                        </Calendar>
                    </telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLegendNumberOfItems" runat="server" Text="Number of items" CssClass="fieldsetLabel" />
                    <telerik:RadNumericTextBox ID="rntbNumberOfItems" ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true"
                        IncrementSettings-InterceptMouseWheel="true" runat="server" Type="Number" Width="80px"
                        NumberFormat-DecimalDigits="0" MinValue="0" IncrementSettings-Step="10" CssClass="fieldsetControlWidth fieldsetLine" />
                    <telerik:RadToolTip ID="rttNumberOfItems" runat="server" TargetControlID="rntbNumberOfItems"
                        RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
                        Position="TopRight" Width="300px" Height="80px">
                        <div id="divNumberOfItems" class="tooltip">
                            You can increment the number of items using the up/down arrow keys or the mouse
                            wheel.
                            <br />
                            Current increment is
                            <asp:Label ID="lblCurrentIncrement" Text="10" runat="server" />.
                            <br />
                            Increment by:
                            <asp:LinkButton ID="lnkbtnIncrementBy1" runat="server">1</asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="lnkbtnIncrementBy10" runat="server">10</asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="lnkbtnIncrementBy100" runat="server">100</asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="lnkbtnIncrementBy1000" runat="server">1000</asp:LinkButton>
                            <hr />
                            <asp:LinkButton ID="lnkbtnClearNumberOfItemsField" runat="server" CssClass="fieldsetControlWidth fieldsetLine">set number of items 0</asp:LinkButton>
                        </div>
                    </telerik:RadToolTip>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div id="divJobTemplate" runat="server">
        <asp:Label ID="lblLegendJobTemplate" runat="server" Text="Job template" CssClass="fieldsetLabel" />
        <telerik:RadComboBox ID="rcbJobTemplate" EmptyMessage="- please select -" runat="server"
            Width="300px" HighlightTemplatedItems="true" Skin="Vista" DataTextField="JobName"
            DataValueField="ID" AutoPostBack="True" AutoCompleteSeparator="true" Filter="Contains"
            AllowCustomText="true">
            <ItemTemplate>
                <div class="rcbJobTemplateleftDiv">
                    <%--'<asp:Label ID="lblJobTemplate" runat="server" CssClass="fieldsetLabel"></asp:Label>--%>
                    <asp:CheckBox ID="chkTemplate" runat="server" Checked="false" AutoPostBack="true"
                        Text='<%# Eval("JobName") %>' OnCheckedChanged="chkTemplate_OnCheckedChanged" />
                </div>
                &nbsp;
                <div class="rcbJobTemplaterightDiv">
                    <asp:CheckBox ID="chkStages" runat="server" Checked="false" AutoPostBack="true" Text='Stages'
                        OnCheckedChanged="chkStages_OnCheckedChanged" TextAlign="Right" />
                </div>
            </ItemTemplate>
            <%--            <Items>
                <telerik:RadComboBoxItem runat="server" Text="normal" Value="NORMAL" Selected="true" />
                <telerik:RadComboBoxItem runat="server" Text="template" Value="TEMPLATE" />
                <telerik:RadComboBoxItem runat="server" Text="recurring" Value="RECURRING" />
            </Items>--%>
        </telerik:RadComboBox>
        <telerik:RadToolTip ID="rttJobType" runat="server" TargetControlID="rcbJobTemplate"
            RelativeTo="Element" HideDelay="10000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
            Position="TopRight" Width="250px" Height="100px">
            <div id="divTooltipJobType" class="tooltip">
                Please select either a "Template" for a complete copy of that template or select
                "Stages" if you want just stages of that template.
            </div>
        </telerik:RadToolTip>
        <br />
    </div>
    <br />
    <div id="divJobStages" runat="server">
        <asp:Label ID="lblLegendJobState" runat="server" Text="Job stages" CssClass="fieldsetLabel" />
        <asp:GridView ID="gvJobStates" runat="server" AutoGenerateColumns="False" ShowHeader="False"
            Width="300px" CssClass="gridviewStage">
            <Columns>
                <asp:TemplateField HeaderText="JobState ID" Visible="False">
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("JobStateKey") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("JobStateKey") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="JobStateName">
                    <ItemTemplate>
                        <asp:Label ID="lblJobStateName" runat="server" Text='<%# Eval("JobStateName") %>' />
                        <div id="divUsersInStages" class="tooltip">
                            <telerik:RadToolTip ID="RadToolTipUsers" runat="server" TargetControlID="lblJobStateName"
                                RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
                                Position="TopRight" Width="350px" Height="200px">
                                <asp:GridView ID="gvUserJobMapping" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                    Width="345px" CssClass="gridviewToolTip" DataKeyNames="JobID">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserName" Text='<%# Eval("UserName")%>' runat="server"></asp:Label>
                                                <asp:HiddenField ID="hidIsJobStateID" Value='<%# Bind("JobStateID")%>' runat="server" />
                                                <asp:HiddenField ID="hidIsJobID" Value='<%# Bind("JobID")%>' runat="server" />
                                                <asp:HiddenField ID="hidIsUserID" Value='<%# Bind("UserID")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbIsEmail" runat="server" Text="Email" Checked='<%# Eval("IsEmail")%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </telerik:RadToolTip>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="200px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Label ID="lblDummy" runat="server" Text="" CssClass="fieldsetLabel" />
        <asp:LinkButton ID="lnkbtnEditJobStates" runat="server" CssClass="fieldsetControlWidth fieldsetLine">edit job stages</asp:LinkButton>
        &nbsp;
        <asp:LinkButton ID="lnkbtnEditJobResources" runat="server" CssClass="fieldsetControlWidth fieldsetLine">edit job resources</asp:LinkButton>
        <br />
    </div>
    <br />
    <br />
    <asp:Label ID="lblLegendInstructions" runat="server" Text="Instructions" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbInstructions" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
        Rows="4" TextMode="MultiLine" Width="350px" ReadOnly="false" MaxLength="1000" />
    <br />
    <br />
    <asp:Label ID="lblLegendInstructionsNotes" runat="server" Text="Notes" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbNotes" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
        Rows="4" TextMode="MultiLine" Width="350px" ReadOnly="true" MaxLength="1000" />
    <telerik:RadToolTip ID="rttInstructionsNotes" runat="server" TargetControlID="btnAddNote"
        ShowEvent="OnClick" RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip"
        ShowCallout="true" Position="TopRight" Width="250px" Height="100px">
        <div id="divTooltipInstructionsNotes" class="tooltip">
            <fieldset id="fsNewNote" runat="server">
                <legend class="fieldsetLegend" id="fslgndNewNote" runat="server">Add Note</legend>
                <asp:TextBox ID="tbNewNote" TextMode="MultiLine" Rows="6" runat="server" Width="100%"
                    Font-Names="Verdana" />
                <br />
                <asp:Button ID="btnSaveNote" runat="server" Text="Save Note" />
            </fieldset>
        </div>
    </telerik:RadToolTip>
    <div id="divAddNote" runat="server">
        <asp:Label ID="lblDummy3" runat="server" Text="" CssClass="fieldsetLabel" />
        <asp:Button ID="btnAddNote" runat="server" Text="Add Note" OnClientClick='return false;' />
    </div>
    <br />
    <br />
    <asp:Label ID="lblLegendMaterialsSupplied" runat="server" Text="Materials supplied"
        CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbMaterialsSupplied" runat="server" CssClass="fieldsetControlStyle"
        Rows="4" TextMode="MultiLine" Width="350px" />
    <br />
    <div id="divCost_Outer" runat="server">
        <DNN:SectionHead runat="server" ID="shCost" IsExpanded="false" Section="divCost_Inner"
            Text="Cost" IncludeRule="true" Visible="true" />
        <div id="divCost_Inner" runat="server">
            <asp:Label ID="lblLegendProductionCost" runat="server" Text="Production Cost" CssClass="fieldsetLabel" />
            <telerik:RadNumericTextBox ID="rntbProductionCost" MinValue="0" runat="server" MaxValue="999999"  
                ValidationGroup="vg" Width="350px" Type="Currency" Culture="English (United Kingdom)">
            </telerik:RadNumericTextBox>
            <%--                <asp:TextBox ID="tbProductionCost" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
                    ValidationGroup="vg" Width="350px" MaxLength="50" />
                <asp:RegularExpressionValidator ID="revtbSearchByJobNO" ControlToValidate="tbProductionCost"
                    ValidationGroup="vg" ForeColor="Red" Display="Dynamic" ErrorMessage="Please Enter Valid Production Cost"
                    ValidationExpression="(^([0-9]*\d*\d{1}?\d*)$)" runat="server" />
            --%>
            <br />
            <br />
            <asp:Label ID="lblLegendDistributionCost" runat="server" Text="Distribution Cost"
                CssClass="fieldsetLabel" />
            <telerik:RadNumericTextBox ID="rntbDistributionCost" MinValue="0" runat="server"
                Type="Currency" Culture="English (United Kingdom)" ValidationGroup="vg" MaxValue="999999"  
                Width="350px">
            </telerik:RadNumericTextBox>
            <%--                <asp:TextBox ID="tbDistributionCost" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
                    ValidationGroup="vg" Width="350px" MaxLength="50" />
                <asp:RegularExpressionValidator ID="revDistributionCost" ControlToValidate="tbDistributionCost"
                    ValidationGroup="vg" ForeColor="Red" Display="Dynamic" ErrorMessage="Please Enter Valid Distribution Cost"
                    ValidationExpression="(^([0-9]*\d*\d{1}?\d*)$)" runat="server" />--%>
            <hr />
        </div>
    </div>
    <br />
    <div id="divQuotation_Outer" runat="server">
        <DNN:SectionHead runat="server" ID="shQuotation" IsExpanded="false" Section="divQuotation_Inner"
            Text="Quotation" IncludeRule="true" Visible="true" />
        <div id="divQuotation_Inner" runat="server">
            <telerik:RadGrid ID="rgQuotation" runat="server" Width="95%" ShowStatusBar="true"
                EnableEmbeddedSkins="false" CssClass="gridviewSpacing" AutoGenerateColumns="False"
                PageSize="10" AllowSorting="True" AllowMultiRowSelection="False" AllowPaging="True"
                GridLines="Both">
                <PagerStyle Mode="NumericPages"></PagerStyle>
                <MasterTableView Width="100%" DataKeyNames="ID" AllowMultiColumnSorting="True" Name="Quotation"
                    CommandItemDisplay="Top">
                    <CommandItemSettings AddNewRecordText="Add/Edit Quotation" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
                        ShowAddNewRecordButton="true" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="hidQuotationID" Value='<%# Bind("ID")%>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn SortExpression="TariffName" HeaderText="Quotation Name"
                            HeaderStyle-Font-Bold="true" HeaderButtonType="TextButton" DataField="TariffName"
                            UniqueName="TariffName" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn SortExpression="CreatedOn" HeaderText="Created On" HeaderButtonType="TextButton"
                            DataFormatString="{0:dd-MMM-yyyy}" DataField="CreatedOn" UniqueName="CreatedOn"
                            ReadOnly="true" HeaderStyle-Font-Bold="true">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridTemplateColumn HeaderText="Created By" HeaderStyle-Font-Bold="true">
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Locked Date" HeaderStyle-Font-Bold="true">
                            <ItemTemplate>
                                <asp:Label ID="lblLockedDate" runat="server" Text='<%# Eval("LockedDateTime") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Is Locked" HeaderStyle-Font-Bold="true">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkLockedDate" runat="server" AutoPostBack="true" OnCheckedChanged="chkLockedDate_CheckedChanged">
                                </asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </div>
    <div>
    </div>
    <br />
    <div id="divCustomerReferences_Outer" runat="server">
        <DNN:SectionHead runat="server" ID="shCustomerReferences" IsExpanded="false" Section="divCustomerReferences_Inner"
            Text="Customer References" IncludeRule="true" Visible="true" />
        <div id="divCustomerReferences_Inner" runat="server">
            <asp:Label ID="lblLegendCustomerReference1" runat="server" Text="Customer Reference 1"
                CssClass="fieldsetLabel" />
            <asp:TextBox ID="tbCustRef1" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
                Width="350px" MaxLength="50" />
            <br />
            <asp:Label ID="lblLegendCustomerReference2" runat="server" Text="Customer Reference 2"
                CssClass="fieldsetLabel" />
            <asp:TextBox ID="tbCustRef2" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
                Width="350px" MaxLength="50" />
            <hr />
        </div>
    </div>
    <br />
    <div id="divMaterialsFromStock_Outer" runat="server">
        <DNN:SectionHead runat="server" ID="shMaterialFromStock" IsExpanded="false" Section="divMaterialsFromStock_Inner"
            Text="Materials to be picked from stock" IncludeRule="true" Visible="true" />
        <div id="divMaterialsFromStock_Inner" runat="server">
            <asp:Label ID="lblLegendSearchForStockItems" runat="server" Text="Search for stock items"
                CssClass="fieldsetLabel" />
            <telerik:RadTextBox ID="rtbSearchForStockItems" runat="server" EmptyMessage="Leave empty for all products or enter a search term"
                CssClass="fieldsetControlWidth fieldsetLine" Width="350px" /><asp:Button ID="btnSearchGo"
                    runat="server" Text="Go" />
            <telerik:RadToolTip ID="rttSearchForStockItems" runat="server" TargetControlID="rtbSearchForStockItems"
                RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
                Position="TopRight" Width="300px" Height="80px">
                <div id="div1" class="tooltip">
                    Separate multiple search terms with the pipe character <b>|</b> which is usually
                    found in the bottom left hand corner of your keyboard
                    <br />
                    <br />
                    e.g. <b>Envelopes | Bags</b>
                    <br />
                    <br />
                    Searches are case-insensitive.
                </div>
            </telerik:RadToolTip>
            <br />
            <asp:Label ID="lblLegendStockItems" runat="server" Text="Stock items" />
            <br />
            <asp:GridView ID="gvStockItems" runat="server" CssClass="gridviewSpacing gvStockItems"
                AutoGenerateColumns="False" Width="100%">
                <Columns>
                    <asp:BoundField DataField="ProductCode" HeaderText="Product code" ReadOnly="True"
                        SortExpression="ProductCode" />
                    <asp:BoundField DataField="ProductDescription" HeaderText="Description" ReadOnly="True"
                        SortExpression="ProductDescription" />
                    <asp:BoundField DataField="QtyAvailable" HeaderText="Qty Avl" ReadOnly="True" SortExpression="QtyAvailable" />
                    <asp:TemplateField HeaderText="Pick Qty">
                        <ItemTemplate>
                            <telerik:RadNumericTextBox ID="rntbPickQty" ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true"
                                IncrementSettings-InterceptMouseWheel="true" runat="server" Type="Number" Width="70px"
                                NumberFormat-DecimalDigits="0" IncrementSettings-Step="10" />
                            <asp:HiddenField ID="hidLogisticProductkey" runat="server" Value='<%# Eval("LogisticProductKey")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblStockItemsEmptyData" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="no products found"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
            <br />
            <asp:Label ID="Label1" runat="server" Text="" CssClass="fieldsetLabel" />
            <asp:Button ID="btnShowBasket" runat="server" Text=" Show Basket" CssClass="fieldsetControlWidth
        fieldsetLine" />
            &nbsp;
            <telerik:RadWindow ID="rwBasket" runat="server" Title="Basket" Width="500" Height="520"
                VisibleOnPageLoad="true" Behaviors="Move,Pin,Resize" InitialBehaviors="Move"
                KeepInScreenBounds="true" OffsetElementID="rdpDeadlineOn" Left="580" Visible="false"
                CssClass="rwBasket" VisibleStatusbar="false" DestroyOnClose="true">
                <Shortcuts>
                    <telerik:WindowShortcut CommandName="Close" Shortcut="Esc" />
                </Shortcuts>
                <ContentTemplate>
                    <asp:Button ID="btnClose" runat="server" Text="Close Basket" OnClientClick="return CloseBasketWindow();" />
                    &nbsp;
                    <asp:Button ID="btnRefreshBasket" runat="server" Text="Refresh Basket" CausesValidation="false" />
                    <%--                        <tr>
                            <td style="width: 75%">
                            </td>
                            <td style="width: 25%" align="right">
                                <asp:Image ID="imgBasketHelpButton" runat="server" ImageUrl="~/DesktopModules/phdcc.CodeModule/images/help.gif" />
                                <telerik:RadToolTip ID="rttBasketHelp" runat="server" TargetControlID="imgBasketHelpButton"
                                    RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
                                    Position="TopRight" Width="300px" Height="80px">
                                    <div id="divBasketHelp" class="tooltip">
                                        some help
                                    </div>
                                </telerik:RadToolTip>
                            </td>
                        </tr>
                    
                    </table>
                    <div style="text-align: center">--%>
                    <br />
                    <br />
                    <asp:GridView ID="gvBasket" runat="server" CellPadding="2" Width="95%" CssClass="gridviewSpacing gvBasket"
                        AutoGenerateColumns="False" HorizontalAlign="Center">
                        <Columns>
                            <asp:BoundField DataField="LogisticProductKey" ReadOnly="True" SortExpression="LogisticProductKey"
                                Visible="False" />
                            <asp:BoundField DataField="ProductCode" HeaderText="Product code" ReadOnly="True"
                                SortExpression="ProductCode" />
                            <asp:BoundField DataField="ProductDescription" HeaderText="Description" ReadOnly="True"
                                SortExpression="Description" />
                            <asp:BoundField DataField="QtyAvailable" HeaderText="Qty Available" ReadOnly="True"
                                SortExpression="QtyAvailable" />
                            <asp:BoundField DataField="QtyToPick" HeaderText="Qty to Pick" ReadOnly="True" SortExpression="QtyToPick" />
                        </Columns>
                    </asp:GridView>
                    <%--</div>--%>
                    <br />
                    <table id="tblPickRecipients" border="0" cellpadding="0" cellspacing="0" width="100%"
                        runat="server">
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="lblRwBasketShorcut" runat="server" Text="Shortcut" Font-Bold="true" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlRwBasketShortcut" runat="server" Skin="Vista" AutoPostBack="true">
                                </telerik:RadComboBox>
                                <%--<asp:DropDownList ID="ddlRwBasketShortcut" runat="server" AutoPostBack="true" CssClass="fieldsetControlStyle" />--%>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="30%">
                                &nbsp;
                                <asp:Label ID="lblRwBasketCustomerName" runat="server" Text="Customer name" Font-Bold="true" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRwBasketCustomerName" runat="server" Width="200px" CssClass="fieldsetControlStyle"
                                    MaxLength="50" />
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="required!"
                                    ControlToValidate="txtRwBasketCustomerName"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td width="30%">
                                &nbsp;
                                <asp:Label ID="Label9" runat="server" Text="Name" Font-Bold="true" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRwBasketName" runat="server" Width="200px" CssClass="fieldsetControlStyle"
                                    MaxLength="50" />
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="required!"
                                    ControlToValidate="txtRwBasketName"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="lblRwBasketAddr1" runat="server" Text="Addr 1" Font-Bold="true" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRwBasketAddr1" runat="server" Width="200px" CssClass="fieldsetControlStyle"
                                    MaxLength="50" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="required!"
                                    ControlToValidate="txtRwBasketAddr1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="lblRwBasketAddr2" runat="server" Text="Addr 2" Font-Bold="true" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRwBasketAddr2" runat="server" Width="200px" CssClass="fieldsetControlStyle"
                                    MaxLength="50" />
                                <%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="required!"
                                        ControlToValidate="txtRwBasketAddr2"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="Label11" runat="server" Text="AIMS Cust Ref 1" Font-Bold="true" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRwBasketAIMSCustRef1" runat="server" Width="200px" MaxLength="50"
                                    CssClass="fieldsetControlStyle" />
                                <%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="required!"
                                        ControlToValidate="txtRwBasketAIMSCustRef1"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="Label12" runat="server" Text="AIMS Cust Ref 2" Font-Bold="true" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRwBasketAIMSCustRef2" runat="server" Width="200px" CssClass="fieldsetControlStyle"
                                    MaxLength="50" />
                                <%--                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="required!"
                                        ControlToValidate="txtRwBasketAIMSCustRef2"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="lblRwBasketState" runat="server" Text="State" Font-Bold="true" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRwBasketState" runat="server" Width="200px" MaxLength="50" />
                                <%--                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="required!"
                                        ControlToValidate="txtRwBasketState"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="Label10" runat="server" Text="Town" Font-Bold="true" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRwBasketTown" runat="server" Width="200px" MaxLength="50" />
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="required!"
                                    ControlToValidate="txtRwBasketTown"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="lblRwbasketPostCode" runat="server" Text="Post code" Font-Bold="true" />
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRwbasketPostCode" runat="server" Width="200px" MaxLength="50" />
                                &nbsp;<asp:LinkButton ID="lnkRwBasketNonUKAddress" runat="server" CausesValidation="false">non-UK address</asp:LinkButton>
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="required!"
                                    ControlToValidate="txtRwbasketPostCode"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="trRwBasketCountry" runat="server" visible="false">
                            <td>
                                &nbsp;&nbsp;
                                <asp:Label ID="Label8" runat="server" Text="Country" Font-Bold="true" />
                            </td>
                            <td>
                            </td>
                            <td>
                                &nbsp;
                                <asp:DropDownList ID="ddlRwBasketCountry" runat="server" AutoPostBack="false" CssClass="fieldsetControlStyle"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    &nbsp;
                    <asp:Label ID="lblLegendSpecialInstructions" runat="server" Font-Bold="true" Text="Special Instructions:" />
                    <br />
                    <div style="text-align: center">
                        <asp:TextBox ID="tbSpecialInstructions" runat="server" Rows="2" TextMode="MultiLine"
                            Font-Names="Verdana" Width="95%" />
                    </div>
                    <br />
                    <br />
                    &nbsp;
                    <asp:Button ID="btnPickStockItems" runat="server" Text="Pick Items" />
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadToolTip ID="rttBasket" runat="server" TargetControlID="rwBasket" RelativeTo="Element"
                HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true" Position="TopRight"
                Width="300px" Height="80px">
                <div id="divBasket" class="tooltip">
                    Use the PIN icon in the top right hand corner to fix or detach the Basket window
                </div>
            </telerik:RadToolTip>
            <hr />
            <br />
            <%--            <asp:Label ID="lblLegendOtherMaterialFromStock" runat="server" Text="Other materials from stock"
                CssClass="fieldsetLabel" />
            <telerik:RadComboBox ID="rcbMaterialsFromStock" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
            --%>
        </div>
    </div>
    <br />
    <DNN:SectionHead runat="server" ID="shJobFiles" IsExpanded="false" Section="divJobFiles"
        Text="Job files" IncludeRule="true" Visible="true" />
    <div id="divJobFiles" runat="server">
        <DNN:SectionHead runat="server" ID="shUpload" IsExpanded="false" Section="divUpload"
            Text="Import job files" IncludeRule="true" Visible="true" />
        <div id="divUpload" runat="server">
            <table id="tabImport">
                <tr>
                    <td style="width: 35%">
                        &nbsp;
                    </td>
                    <td style="width: 65%">
                        <telerik:RadUpload ID="RadUpload" runat="server" OverwriteExistingFiles="false" ControlObjectsVisibility="None" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnImport" runat="server" Text="Import" />
                    </td>
                </tr>
            </table>
            <hr />
        </div>
        <DNN:SectionHead runat="server" ID="shDownloadJobFiles" IsExpanded="false" Section="divDownload"
            Text="Download job files" IncludeRule="true" Visible="true" />
        <div id="divDownload" runat="server" style="text-align: center">
            <asp:Label ID="lblLegendJobFiles" runat="server" Text="Job files" CssClass="fieldsetLabel" />
            <asp:GridView ID="gvJobFiles" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
                AutoGenerateColumns="False" Width="500px">
                <Columns>
                    <asp:BoundField DataField="Filename" HeaderText="File name" ReadOnly="True"></asp:BoundField>
                    <asp:BoundField DataField="Length" HeaderText="Length" ReadOnly="True" />
                    <asp:BoundField DataField="DateUploaded" HeaderText="Uploaded" ReadOnly="True" DataFormatString="{0:d-MMM-yy hh:mm}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnJobFileDownload" runat="server" CommandArgument='<%# Eval("Filename")%>'
                                OnClick="lnkbtnJobFileDownload_Click">download</asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton ID="lnkbtnJobFileDelete" runat="server" CommandArgument='<%# Eval("Filename")%>'
                                OnClientClick='return confirm("Are you sure you want to delete this file?");'
                                OnClick="lnkbtnJobFileDelete_Click">delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    no job files found
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
    <br />
    <asp:Label ID="lblLegendAccountHandler" runat="server" Text="Account Handler" CssClass="fieldsetLabel" />
    <telerik:RadComboBox ID="rcbAccountHandler" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    <asp:LinkButton ID="lnkbtnAccountHandlerMe" runat="server">me</asp:LinkButton>
    <br />
    <br />
    <asp:Label ID="lblDummy1" runat="server" Text="" CssClass="fieldsetLabel" />
    <asp:Button ID="btnUpdateJob" runat="server" Text="Update Job" ValidationGroup="vg" />
    &nbsp;
    <asp:Button ID="btnCancelJob" runat="server" Text="Cancel Job" OnClientClick="return confirmbox();"
        CausesValidation="false" />
    &nbsp;
    <asp:LinkButton ID="lnkbtnViewJobEventLog" runat="server" CausesValidation="false">view event log</asp:LinkButton>
    &nbsp;
    <asp:LinkButton ID="lnkbtnCopyJobToClipboard" runat="server">copy job to clipboard</asp:LinkButton>
    <%--        <telerik:RadToolTip ID="rttJobCopied" runat="server" RelativeTo="Element" ShowEvent="FromCode" Position="TopRight" Modal="true" 
            TargetControlID="lnkbtnCopyJobToClipboard" EnableShadow="true" Width="250px" Height="100px"
            HideEvent="ManualClose" ManualCloseButtonText="Close">
            <div id="div2" class="tooltip">
                <asp:Label ID="lblMessage" runat="server" Text="Message" Font-Bold="true" ></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblShowCopyJobToolTip" runat="server" Text=""  />
                <br /><br /><br />
                <br /><br /><br />
                <br /><br /><br />
            </div>
        </telerik:RadToolTip>--%>
    <br />
    <telerik:RadWindow ID="rwJobEventLog" runat="server" Title="Job Event Log" Width="350"
        Height="520" VisibleOnPageLoad="true" Behaviors="Move,Pin,Resize" InitialBehaviors="Move"
        KeepInScreenBounds="true" OffsetElementID="lnkbtnViewJobEventLog" Left="580"
        Visible="false" CssClass="rwBasket" DestroyOnClose="true">
        <Shortcuts>
            <telerik:WindowShortcut CommandName="Close" Shortcut="Esc" />
        </Shortcuts>
        <ContentTemplate>
            <div style="text-align: center">
                <table>
                    <tr>
                        <td style="width: 75%">
                            <asp:Button ID="btnJobEventLogCloseWindow" runat="server" Text="Close Window" OnClientClick="return CloseEventLogWindow();"
                                CausesValidation="false" />
                        </td>
                        <%--                                <td style="width: 25%" align="right">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/DesktopModules/phdcc.CodeModule/images/help.gif" />
                                </td>
                        --%>
                    </tr>
                </table>
                <asp:GridView ID="gvJobEventLog" runat="server" CellPadding="2" Width="95%" CssClass="gridviewSpacing gvBasket"
                    AutoGenerateColumns="True">
                    <Columns>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
        </ContentTemplate>
    </telerik:RadWindow>
    <telerik:RadWindow ID="rwCustomer" runat="server" Title="Add Customer" VisibleStatusbar="false"
        Behaviors="Move,Pin,Resize" InitialBehaviors="Pin" VisibleOnPageLoad="true" Height="500"
        Width="510" Left="150px" Visible="false" DestroyOnClose="true">
        <Shortcuts>
            <telerik:WindowShortcut CommandName="Close" Shortcut="Esc" />
        </Shortcuts>
        <ContentTemplate>
            <%--<div id="Add Customer" style="text-align: center">--%>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left" width="30%">
                        <asp:Button ID="btnCustomerCloseWindow" runat="server" Text="Close Window" CausesValidation="False"
                            OnClientClick="CloseCustomerWindow(); return false;" />
                        &nbsp;
                    </td>
                    <td id="tdbtnImportCustomer" runat="server">
                        <asp:Button ID="btnImportCustomer" runat="server" Text="Import Customer from Stock System"
                            CausesValidation="false" />
                    </td>
                    <td id="tdbtnAddCustomer" runat="server">
                        <asp:Button ID="btnAddCustomer" runat="server" Text="Add Customer" CausesValidation="false" />
                    </td>
                </tr>
            </table>
            <table id="tblImportCustomer" border="0" cellpadding="0" cellspacing="0" width="100%"
                runat="server">
                <tr>
                    <td>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td width="40%">
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblLegendStockSystemCustomer" runat="server" Text="Stock system customer"
                            Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadComboBox ID="RadComboBoxStockSystemCustomer" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
                            AutoPostBack="True" />
                        <telerik:RadToolTip ID="RadToolTipStockSystemCustomer" runat="server" TargetControlID="RadComboBoxStockSystemCustomer"
                            RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
                            Position="TopRight" Width="250px" Height="100px">
                            <div id="divTooltipStockSystemCustomer" class="tooltip">
                                <asp:Label ID="Label7" runat="server" Text="Select Customers From Stock Database" />
                            </div>
                        </telerik:RadToolTip>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <table id="tblAddCustomer" border="0" cellpadding="0" cellspacing="0" width="100%"
                runat="server">
                <tr>
                    <td width="26%" align="right">
                        <asp:Label ID="Label2" runat="server" Text="Customer code" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbCustomerCode" runat="server" Width="200px" CssClass="fieldsetControlStyle" />
                        &nbsp;<asp:RequiredFieldValidator ID="rfvCustomerCode" runat="server" ErrorMessage="required!"
                            ControlToValidate="tbCustomerCode"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label3" runat="server" Text="Customer name" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbCustomerName" runat="server" Width="200px" CssClass="fieldsetControlStyle" />
                        &nbsp;<asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" ErrorMessage="required!"
                            ControlToValidate="tbCustomerName"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label4" runat="server" Text="Addr 1" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbAddr1" runat="server" Width="200px" CssClass="fieldsetControlStyle" />
                        <asp:RequiredFieldValidator ID="rfvAddr1" runat="server" ErrorMessage="required!"
                            ControlToValidate="tbAddr1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label5" runat="server" Text="Addr 2" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbAddr2" runat="server" Width="200px" CssClass="fieldsetControlStyle" />
                        &nbsp;
                        <asp:LinkButton ID="lnkbtnAddAddr3" runat="server" CausesValidation="false">add Addr 3</asp:LinkButton>
                    </td>
                </tr>
                <tr id="trAdd3" runat="server" visible="false">
                    <td align="right">
                        <asp:Label ID="lblLegendAddr3" runat="server" Text="Addr 3" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbAddr3" runat="server" Width="200px" CssClass="fieldsetControlStyle" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblLegendTown" runat="server" Text="Town/City" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbTown" runat="server" Width="200px" />
                        &nbsp;<asp:RequiredFieldValidator ID="rfvTown" runat="server" ErrorMessage="required!"
                            ControlToValidate="tbTown"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblLegendPostcode" runat="server" Text="Post code" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbPostcode" runat="server" Width="200px" />
                        &nbsp;<asp:LinkButton ID="lnkbtnNonUKAddress" runat="server" CausesValidation="false">non-UK address</asp:LinkButton>
                    </td>
                </tr>
                <tr id="trCountry" runat="server" visible="false">
                    <td align="right">
                        <asp:Label ID="Label6" runat="server" Text="Country" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="fieldsetControlStyle fieldsetControlWidth" />
                        &nbsp;<asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="required!"
                            InitialValue="0" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="trMultipleContacts" runat="server" visible="false">
                    <td align="right">
                        <asp:Label ID="lblLegendDummy" runat="server" Text="" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkbtnPrevContact" runat="server"><< prev contact</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkbtnNextContact" runat="server">next contact >></asp:LinkButton>
                        &nbsp;
                        <asp:Button ID="btnRemoveContact" runat="server" Text="Remove Contact" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblLegendContactName" runat="server" Text="Contact Name" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbContactName" runat="server" Width="200px" CssClass="fieldsetControlStyle"
                            ValidationGroup="vgCustomer" />&nbsp;
                        <asp:RequiredFieldValidator ID="rfvtbContactName" runat="server" ErrorMessage="required!"
                            Display="Dynamic" ControlToValidate="tbContactName" ValidationGroup="vgCustomer"></asp:RequiredFieldValidator>
                        <asp:Button ID="btnAddAnotherContact" runat="server" Visible="false" Text="Add Contact" />
                    </td>
                </tr>
                <%--<tr>
                    <td  align="right">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td >
                        &nbsp;&nbsp;&nbsp;
                        
                    </td>
                </tr>--%>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblLegendTel" runat="server" Text="Contact Phone" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbContactTelephone" runat="server" Width="200px" CssClass="fieldsetControlStyle" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblLegendMobile" runat="server" Text="Contact Mobile" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbContactMobile" runat="server" Width="200px" CssClass="fieldsetControlStyle" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblLegendEmailAddr" runat="server" Text="Contact Email" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbContactEmailAddr" runat="server" Width="200px" CssClass="fieldsetControlStyle" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblLegendNotes" runat="server" Text="Contact Notes" Font-Bold="true" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="tbContactNotes" runat="server" Rows="3" TextMode="MultiLine" Width="200px"
                            CssClass="fieldsetControlStyle" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblUpdateCustomer" runat="server" Text="" />
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnUpdateCustomer" runat="server" Text="Update Customer" CausesValidation="true"
                            ValidationGroup="vgCustomer" />
                    </td>
                </tr>
            </table>
            <%--                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
            --%>
        </ContentTemplate>
    </telerik:RadWindow>
    <br />
</fieldset>
<asp:HiddenField ID="hidExternalCustomerKey" runat="server" />

<script type="text/javascript">
    function SB_ShowImage(value) {
        window.open("show_image.aspx?Image=" + value, "ProductImage", "top=10,left=10,width=610,height=610,status=no,toolbar=no,address=no,menubar=no,resizable=yes,scrollbars=yes");
    }
</script>

<%--<telerik:RadAjaxLoadingPanel ID="alpCreateEditJob" runat="server" Height="75px" MinDisplayTime="5"
    Width="75px">
    <asp:Image ID="Image2" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel ID="rapCreateEditJob" RequestQueueSize="5" runat="server" Width="100%"
    ClientEvents-OnRequestStart="requestStart" EnableOutsideScripts="True" HorizontalAlign="NotSet"
    ScrollBars="None" LoadingPanelID="alpCreateEditJob">--%>
<%--</telerik:RadAjaxPanel>--%>
