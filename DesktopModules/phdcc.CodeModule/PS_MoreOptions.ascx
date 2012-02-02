<%@ Control Language="VB" AutoEventWireup="true" CodeFile="PS_MoreOptions.ascx.vb"
    Inherits="PS_MoreOptions" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="DNN" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<link href="module.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .GridDataDiv_Vista
    {
        overflow-y: scroll !important;
    }
</style>
<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
    <script type="text/javascript">



        var isEnable = true;
        function ToggleDatePicker(enable) {
            var FromDatePicker = $find('<%= rdpFromDate.ClientID %>');
            var ToDatePicker = $find('<%= rdpToDate.ClientID %>');
            FromDatePicker.set_enabled(enable);
            ToDatePicker.set_enabled(enable);

        }




        function CloseJobWindow() {

            GetJobWindow().close();


        }

        function GetJobWindow() {

            var oWindowJob = $find('<%= rwJob.ClientID %>');
            return oWindowJob;
        }



        function Delete(id) {

            var JobId = id;

            if (confirm("Are you sure you want to delete this template " + JobId + "?")) {
                return true;
            }

            else {

                return false;
            }


        }



       



    </script>
</telerik:RadScriptBlock>
<asp:Button ID="btnExpandAll" runat="server" Text="Expand All" />&nbsp;<asp:Button
    ID="btnContractAll" runat="server" Text="Contract All" />
<br />
<br />
<DNN:SectionHead runat="server" ID="shManageTemplates" Section="fsManageTemplates"
    Text="Manage Templates" IncludeRule="true" Visible="true" />
<fieldset id="fsManageTemplates" runat="server">
    <legend>Manage Templates</legend>
    <br />
    <asp:GridView ID="gvTemplates" runat="server" Width="100%" CellPadding="2">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkbtnEditTemplate" runat="server" CommandArgument='<%# Eval("Ref")%>'
                        CommandName="edit" ToolTip="Edit" OnClick="lnkbtnEditTemplate_Click">
                        <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Portals/0/Images/Edit.gif" /></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="lnkbtnRemoveTemplate" runat="server" OnClick="lnkbtnRemoveTemplate_Click"
                        ToolTip="Delete" CommandName="delete" OnClientClick='<%#Eval("Ref", "return Delete({0});")%>'>
                        <asp:Image ID="imgDelete" runat="server" ImageUrl="~/Portals/0/Images/delete.png" /></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="btnAddTemplate" runat="server" Text="Add Template" />
    &nbsp;<asp:Button ID="btnCreateTemplateFromJob" runat="server" Text="Create Template from Job" />
    <br />
</fieldset>
<DNN:SectionHead runat="server" ID="shManageCustomers" Section="fsManageCustomers"
    Text="Manage Customers" IncludeRule="true" Visible="true" />
<fieldset id="fsManageCustomers" runat="server">
    <legend>Manage Customers</legend>
    <br />
    <asp:Button ID="btnShowCustomers" runat="server" Text="Show Customers" />
    <br />
    <br />
    <asp:GridView ID="gvCustomers" runat="server" Width="100%" AutoGenerateColumns="False"
        CellPadding="2">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkbtnEditCustomer" runat="server" CommandArgument='<%# Container.DataItem("id")%>'
                        ToolTip="Edit" OnClick="lnkbtnEditCustomer_Click">edit</asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="lnkbtnRemoveCustomer" runat="server" CommandArgument='<%# Container.DataItem("id")%>'
                        ToolTip="Delete" OnClick="lnkbtnRemoveCustomer_Click">remove</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" ReadOnly="True"
                SortExpression="CustomerCode" />
            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ReadOnly="True"
                SortExpression="CustomerName" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="btnAddCustomer" runat="server" Text="Add Customer" />
    &nbsp;<asp:Button ID="btnImportCustomer" runat="server" Text="Import Customer from Stock System" />
    <br />
</fieldset>
<DNN:SectionHead runat="server" ID="shManageRecipients" Section="fsManageRecipients"
    IsExpanded="false" Text="Manage Recipients" IncludeRule="true" Visible="true" />
<fieldset id="fsManageRecipients" runat="server">
    <legend>Manage Recipients</legend>
    <telerik:RadGrid ID="rgRecipients" runat="server" Width="100%" ShowStatusBar="true"
        EnableEmbeddedSkins="false" AutoGenerateColumns="False" PageSize="10" AllowSorting="True"
        AllowMultiRowSelection="False" AllowPaging="True" GridLines="Both">
        <PagerStyle Mode="NumericPages"></PagerStyle>
        <MasterTableView Width="100%" DataKeyNames="ID" AllowMultiColumnSorting="True" Name="FF_Users"
            CommandItemDisplay="Top">
            <CommandItemSettings AddNewRecordText="Add/Edit Recipients" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
                ShowAddNewRecordButton="true" ShowRefreshButton="false" />
            <Columns>
                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                    ItemStyle-Width="20px" EditImageUrl="~/Portals/0/Images/Edit.gif">
                </telerik:GridEditCommandColumn>
                <telerik:GridBoundColumn SortExpression="ID" HeaderText="ID" HeaderButtonType="TextButton"
                    DataField="ID" UniqueName="ID" ReadOnly="true" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn SortExpression="ShortCut" HeaderText="ShortCut" HeaderButtonType="TextButton"
                    DataField="ShortCut" UniqueName="ShortCut">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn SortExpression="CneeCtcName" HeaderText="CneeCtcName" HeaderButtonType="TextButton"
                    DataField="CneeCtcName" UniqueName="CneeCtcName">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn SortExpression="CneeName" HeaderText="CneeName" HeaderButtonType="TextButton"
                    DataField="CneeName" UniqueName="CneeName">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn SortExpression="CneeAddr1" HeaderText="CneeAddr1" HeaderButtonType="TextButton"
                    DataField="CneeAddr1" UniqueName="CneeAddr1">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn SortExpression="CneeAddr2" HeaderText="CneeAddr2" HeaderButtonType="TextButton"
                    DataField="CneeAddr2" UniqueName="CneeAddr2">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn SortExpression="CneeTown" HeaderText="CneeTown" HeaderButtonType="TextButton"
                    DataField="CneeTown" UniqueName="CneeTown">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn SortExpression="CneeState" HeaderText="CneeState" HeaderButtonType="TextButton"
                    DataField="CneeState" UniqueName="CneeState">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn SortExpression="CneePostCode" HeaderText="CneePostCode"
                    HeaderButtonType="TextButton" DataField="CneePostCode" UniqueName="CneePostCode">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Country">
                    <ItemTemplate>
                        <asp:Label ID="lblCneeCountryKey" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CneeCountryKey")%>'></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <%--            <telerik:GridBoundColumn SortExpression="CneeCountryKey" HeaderText="CneeCountryKey"
                HeaderButtonType="TextButton" DataField="CneeCountryKey" UniqueName="CneeCountryKey">
            </telerik:GridBoundColumn>--%>
                <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                    ConfirmText="Are you sure you to delete this group ?" ItemStyle-Width="20px"
                    CommandName="Delete" ImageUrl="~/Portals/0/Images/Delete.png">
                </telerik:GridButtonColumn>
            </Columns>
            <EditFormSettings EditFormType="Template">
                <FormTemplate>
                    <asp:Label ID="lblShortcut" runat="server" Text="Shortcut"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadTextBox ID="rtbShortcut" runat="server" EmptyMessage="Please Enter Shortcut"
                        Width="200px" Skin="Vista">
                    </telerik:RadTextBox>
                    &nbsp;
                    <asp:RequiredFieldValidator ID="rfvrtbShortcut" runat="server" ErrorMessage="required!"
                        Display="Dynamic" ControlToValidate="rtbShortcut"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="lblCtcName" runat="server" Text="Ctc Name"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadTextBox ID="rtbCtcName" runat="server" EmptyMessage="Please Enter CtcName"
                        Width="200px" Skin="Vista">
                    </telerik:RadTextBox>
                    <br />
                    <asp:Label ID="lblCneeName" runat="server" Text="Cnee Name"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadTextBox ID="rtbCneeName" runat="server" EmptyMessage="Please Enter Cnee Name"
                        Width="200px" Skin="Vista">
                    </telerik:RadTextBox>
                    <br />
                    <asp:Label ID="lblCneeAddr1" runat="server" Text="Cnee Addr1"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadTextBox ID="rtbCneeAddr1" runat="server" EmptyMessage="Please Enter Cnee Addr1"
                        Width="200px" Skin="Vista">
                    </telerik:RadTextBox>
                    <br />
                    <asp:Label ID="lblCneeAddr2" runat="server" Text="Cnee Addr2"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadTextBox ID="rtbCneeAddr2" runat="server" EmptyMessage="Please Enter Cnee Addr2"
                        Width="200px" Skin="Vista">
                    </telerik:RadTextBox>
                    <br />
                    <asp:Label ID="lblState" runat="server" Text="State"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadTextBox ID="rtbState" runat="server" EmptyMessage="Please Enter State"
                        Width="200px" Skin="Vista">
                    </telerik:RadTextBox>
                    <br />
                    <asp:Label ID="lblCneeTown" runat="server" Text="Town"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadTextBox ID="rtbTown" runat="server" EmptyMessage="Please Enter Cnee Town"
                        Width="200px" Skin="Vista">
                    </telerik:RadTextBox>
                    <br />
                    <asp:Label ID="lblCneePostCode" runat="server" Text="Cnee Post Code"></asp:Label>
                    &nbsp;&nbsp;
                    <telerik:RadTextBox ID="rtbPostCode" runat="server" EmptyMessage="Please Enter Cnee Post Code"
                        Width="200px" Skin="Vista">
                    </telerik:RadTextBox>
                    <br />
                    <asp:Label ID="lblCneeCountry" runat="server" Text="Country"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="fieldsetControlStyle fieldsetControlWidth" />
                    <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="required!"
                        InitialValue="0" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                    <%--                <telerik:RadComboBox ID="rcbCountry" runat="server" Skin="Vista">
                </telerik:RadComboBox>--%>
                    <br />
                    <asp:HiddenField ID="hidFFRecipientID" Value='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                        runat="server" />
                    <br />
                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName='<%# IIf( DataBinder.Eval(Container, "OwnerTableView.IsItemInserted"), "PerformInsert", "Update") %>'
                        CssClass="lnkButton" ToolTip="Update">
                        <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                        CssClass="lnkButton" ToolTip="Cancel">
                        <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" /></asp:LinkButton>
                    <asp:HiddenField ID="hidRecipientID" Value='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                        runat="server" />
                </FormTemplate>
            </EditFormSettings>
        </MasterTableView>
    </telerik:RadGrid>
</fieldset>
<DNN:SectionHead runat="server" ID="shManageScheduledJobs" Section="fsManageScheduledJobs"
    Visible="false" Text="Manage Scheduled Jobs" IncludeRule="true" />
<fieldset id="fsManageScheduledJobs" runat="server" visible="false">
    <legend>Manage Scheduled Jobs</legend>
    <br />
    <asp:Button ID="btnShowScheduledJobs" runat="server" Text="Show Scheduled Jobs" />
    <br />
    <br />
    <asp:GridView ID="gvScheduledJobs" runat="server" Width="100%" CellPadding="2">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkbtnEditScheduledJob" runat="server" OnClick="lnkbtnEditScheduledJob_Click">edit</asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="lnkbtnRemoveScheduledJob" runat="server" OnClick="lnkbtnRemoveScheduledJob_Click">remove</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="btnAddScheduledJob" runat="server" Text="Add Scheduled Job" />
    <br />
</fieldset>
<DNN:SectionHead runat="server" ID="shManageAuditTrail" Section="fsManageAuditTrail"
    Text="Manage Audit Trail" IncludeRule="true" Visible="true" />
<fieldset id="fsManageAuditTrail" runat="server">
    <legend>Manage Audit Trail</legend>
    <br />
    <telerik:RadAjaxLoadingPanel ID="alpMoreOptions" runat="server" Height="75px" MinDisplayTime="5"
        Width="75px">
        <asp:Image ID="Image2" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="rapMoreOptions" RequestQueueSize="5" runat="server" Width="100%"
        EnableOutsideScripts="True" HorizontalAlign="NotSet" ScrollBars="None" LoadingPanelID="alpMoreOptions">
        <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearchAuditTrial">
            <asp:Label ID="lblJobNo" Text="Job No" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="tbSearchByJobNO" runat="server" CssClass="txt">
            </asp:TextBox>
            <asp:RegularExpressionValidator ID="vldtbSearchByJobNO" ControlToValidate="tbSearchByJobNO"
                ForeColor="Red" Display="Dynamic" ErrorMessage="Please Enter Valid Number" ValidationExpression="(^([0-9]*\d*\d{1}?\d*)$)"
                runat="server" />
            <br />
            <asp:Label ID="lbRecordType" Text="Record Type" runat="server"></asp:Label>
            &nbsp;
            <telerik:RadComboBox ID="rcbRecordType" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
                Width="150px" Height="100px" AutoPostBack="True">
                <Items>
                    <telerik:RadComboBoxItem runat="server" Text="- Please Select - " Value="-1" Selected="true" />
                    <telerik:RadComboBoxItem runat="server" Text="Job" Value="J" Selected="true" />
                    <telerik:RadComboBoxItem runat="server" Text="Stage" Value="S" />
                    <telerik:RadComboBoxItem runat="server" Text="M" Value="M" />
                </Items>
            </telerik:RadComboBox>
            <br />
            <asp:Label ID="lblUsers" Text="Users" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadComboBox ID="rcbUsers" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
                Width="150px" Height="100px" AutoPostBack="True">
            </telerik:RadComboBox>
            <br />
            <asp:Label ID="lblDateFrom" Text="Date From" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadDatePicker ID="rdpFromDate" runat="server" DateInput-DateFormat="ddd d-MMM-yyyy"
                Width="150px" DateInput-EmptyMessage="from">
                <Calendar ID="Calendar1" runat="server">
                    <SpecialDays>
                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#ccccff">
                        </telerik:RadCalendarDay>
                    </SpecialDays>
                </Calendar>
            </telerik:RadDatePicker>
            <br />
            <asp:Label ID="lblDateTo" Text="Date To" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadDatePicker ID="rdpToDate" runat="server" DateInput-DateFormat="ddd d-MMM-yyyy"
                Width="150px" DateInput-EmptyMessage="To">
                <Calendar ID="Calendar2" runat="server">
                    <SpecialDays>
                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#ccccff">
                        </telerik:RadCalendarDay>
                    </SpecialDays>
                </Calendar>
            </telerik:RadDatePicker>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="chkIsInclude" runat="server" Checked="true" Text="Include Date"
                CssClass="searchtxt" OnClick="ToggleDatePicker(this.checked);" />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSearchAuditTrial" runat="server" Text="Search" />
        </asp:Panel>
        <br />
        <telerik:RadGrid ID="gvJobEventLog" runat="server" CellPadding="2" Width="100%" AutoGenerateColumns="false"
            EnableEmbeddedSkins="false" ShowFooter="true" GridLines="Both" OnNeedDataSource="gvJobEventLog_NeedDataSource"
            AllowPaging="true">
            <MasterTableView Width="100%">
                <PagerStyle Mode="NextPrevAndNumeric" />
                <Columns>
                    <telerik:GridBoundColumn HeaderText="ID" DataField="ID" UniqueName="ID" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Source ID" DataField="SourceID" UniqueName="SourceID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Job No" DataField="SourceID" UniqueName="SourceID"
                        Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Date/Time" DataField="Date/Time" UniqueName="Date/Time"
                        DataFormatString="{0:ddd d-MMM-yyyy}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Event" DataField="Event" UniqueName="Event"
                        Display="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="Description" DataField="Description" UniqueName="Description">
                        <ItemStyle Width="350px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn HeaderText="User" DataField="User" UniqueName="User">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
            <%--<HeaderStyle Width="0px" />--%>
            <%--            <ClientSettings>
                <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing AllowColumnResize="false" EnableRealTimeResize="false" ResizeGridOnColumnResize="false">
                </Resizing>
            </ClientSettings>
            --%>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    <br />
    <%--<asp:Button ID="btnShowAuditTrail" runat="server" Text="Show Audit Trail" />--%>
    <telerik:RadWindow ID="rwJob" runat="server" Title="Create Template From Job" Width="600"
        Height="520" VisibleOnPageLoad="true" Behaviors="Move,Pin,Resize" InitialBehaviors="Pin"
        KeepInScreenBounds="true" OffsetElementID="btnCreateTemplateFromJob" Left="580"
        Visible="false" CssClass="rwBasket" DestroyOnClose="true">
        <Shortcuts>
            <telerik:WindowShortcut CommandName="Close" Shortcut="Esc" />
        </Shortcuts>
        <ContentTemplate>
            <%--<asp:Panel ID="pnlJob" runat="server" DefaultButton="btnGo">--%>
            <asp:Button ID="btnCloseJobWindow" runat="server" Text="Close" CausesValidation="false"
                OnClientClick="CloseJobWindow(); return false;" />
            <br />
            <br />
            <br />
            <asp:ValidationSummary ID="vs" runat="server" ValidationGroup="vgRwJob" ForeColor="Red" />
            <br />
            <asp:Label ID="lblJobId" Text="Select Job" runat="server"></asp:Label>
            &nbsp;&nbsp;
            <%--<asp:TextBox ID="tbJobNo" runat="server" Width="100px" MaxLength="10" ValidationGroup="vgRwJob" />--%>
            <%--            <telerik:RadAjaxLoadingPanel ID="alpJob" runat="server" Height="75px" MinDisplayTime="5"
                Width="75px">
                <asp:Image ID="Image1" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
            </telerik:RadAjaxLoadingPanel>
            <telerik:RadAjaxPanel ID="rapJob" RequestQueueSize="5" runat="server" Width="100%" 
                OnAjaxRequest="rapJob_AjaxRequest" EnableOutsideScripts="True" HorizontalAlign="NotSet"
                ScrollBars="None" LoadingPanelID="alpJob">--%>
            <telerik:RadComboBox ID="rcbJob" EmptyMessage="- please select job -" runat="server"
                Width="500px" Skin="Vista" AutoPostBack="True" AllowCustomText="true">
                <ItemTemplate>
                    <div>
                        <telerik:RadGrid ID="rgJob" runat="server" Width="100%" Skin="Vista" AutoGenerateColumns="False"
                            OnItemDataBound="rgJob_ItemDataBound" AllowSorting="True" AllowMultiRowSelection="False"
                            OnNeedDataSource="rgJob_NeedDataSource" GridLines="Both" ExpandAnimation-Type="None"
                            CollapseAnimation-Type="None">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <MasterTableView Width="100%" DataKeyNames="ID,JobName" Name="rgJob" CommandItemDisplay="None">
                                <Columns>
                                    <telerik:GridBoundColumn SortExpression="ID" HeaderText="ID" HeaderButtonType="TextButton"
                                        DataField="ID" UniqueName="ID" ReadOnly="true">
                                        <%--<ItemStyle Width="100px" />--%>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="JobName" HeaderText="Job Name" HeaderButtonType="TextButton"
                                        DataField="JobName" UniqueName="JobName">
                                        <%--<ItemStyle Width="200px" />--%>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridDateTimeColumn SortExpression="CreatedOn" HeaderText="Created On" HeaderButtonType="TextButton"
                                        DataFormatString="{0:dd-MMM-yyyy}" DataField="CreatedOn" UniqueName="CreatedOn">
                                        <%--<ItemStyle Width="100px" />--%>
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridBoundColumn SortExpression="CreatedBy" HeaderText="Created By" HeaderButtonType="TextButton"
                                        DataField="CreatedBy" UniqueName="CreatedBy">
                                        <%--<ItemStyle Width="100px" />--%>
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="400px" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>
                    &nbsp;
                </ItemTemplate>
                <Items>
                    <telerik:RadComboBoxItem runat="server" Text=" "></telerik:RadComboBoxItem>
                </Items>
            </telerik:RadComboBox>
            <%--            </telerik:RadAjaxPanel>--%>
            <%--                <asp:RequiredFieldValidator ID="rfvtbContactName" runat="server" ErrorMessage="Please Enter Job No."
                    Display="None" ControlToValidate="tbJobNo" ValidationGroup="vgRwJob"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revJobNo" ControlToValidate="tbJobNo" ForeColor="Red"
                    Display="None" ErrorMessage="Please Enter Valid Job Number" ValidationExpression="(^([0-9]*\d*\d{1}?\d*)$)"
                    runat="server" />
            --%>
            &nbsp;&nbsp;
            <%--<asp:Button ID="btnGo" runat="server" Text="Go" CausesValidation="true" ValidationGroup="vgRwJob" />--%>
            <%--</asp:Panel>--%>
            <br />
            <br />
            <asp:DetailsView ID="dvJob" runat="server" AutoGenerateRows="False" DataKeyNames="Id" style="margin-left:70px;"
                Width="86%">
                <HeaderTemplate>
                    <asp:Label ID="lblJobDetail" Text="Job Detail" runat="server" Font-Bold="true" align="center"></asp:Label>
                </HeaderTemplate>
                <HeaderStyle Font-Bold="true" BackColor="LightGray" HorizontalAlign="Center" />
                <Fields>
                    <asp:BoundField DataField="Id" HeaderText="Job No" SortExpression="Id" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="JobName" HeaderText="Job Name" ReadOnly="True" SortExpression="JobName"
                        HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" SortExpression="CustomerCode"
                        HeaderStyle-Font-Bold="true" />
                    <asp:TemplateField HeaderText="Created On" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <asp:Label ID="lblJobCreatedOn" Text='<%# IsValidDate(Eval("CreatedOn")) %>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Collateral due On" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <asp:Label ID="lblCollateralDueOn" Text='<%# IsValidDate(Eval("CollateralDueOn")) %>'
                                runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Deadline On" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <asp:Label ID="lblDeadlineOn" Text='<%# IsValidDate(Eval("DeadlineOn")) %>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Completed On" HeaderStyle-Font-Bold="true">
                        <ItemTemplate>
                            <asp:Label ID="lblCompletedOn" Text='<%# IsValidDate(Eval("CompletedOn")) %>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ProductionCost" HeaderText="Production Cost" SortExpression="ProductionCost"
                        DataFormatString="{0:c}" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="DistributionCost" HeaderText="Distribution Cost" SortExpression="DistributionCost"
                        DataFormatString="{0:c}" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="UserName" HeaderText="Account Handler" SortExpression="AccountHandler"
                        HeaderStyle-Font-Bold="true" />
                    <asp:TemplateField ShowHeader="false">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCreateTemplate" runat="server" CommandArgument='<%# Container.DataItem("Id")%>'
                                OnClick="lnkCreateTemplate_Click">Create Template</asp:LinkButton>
                            <%--                            <asp:LinkButton ID="lnkCreateTemplate" runat="server"
                                OnClick="lnkCreateTemplate_Click">Create Template</asp:LinkButton>
                            --%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Fields>
                <AlternatingRowStyle BackColor="LightGray" />
                <EmptyDataTemplate>
                    <div>
                        <asp:Label ID="lblEmptyRecord" Text="No Record Found" runat="server" Font-Bold="true"
                            align="center" ForeColor="red"></asp:Label>
                    </div>
                </EmptyDataTemplate>
            </asp:DetailsView>
            <br />
        </ContentTemplate>
    </telerik:RadWindow>
</fieldset>
<br />
<br />
<%--<asp:CheckBox ID="cbDebugMode" runat="server" AutoPostBack="True" Text="Debug mode" />--%>
