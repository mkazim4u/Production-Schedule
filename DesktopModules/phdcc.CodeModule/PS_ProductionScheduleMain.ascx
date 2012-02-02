<%@ Control Language="VB" AutoEventWireup="true" CodeFile="PS_ProductionScheduleMain.ascx.vb"
    Inherits="PS_ProductionScheduleMain" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<link href="module.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .radtooltip_Default, .radtooltip_Default td
    {
        background-color: #ffff9e !important;
    }
</style>
<telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
    <script type="text/javascript">
        function clickButton(e, buttonid) {
            var evt = e ? e : window.event;
            var bt = document.getElementById(buttonid);

            if (bt) {
                if (evt.keyCode == 13) {
                    bt.click();
                    return false;
                }
            }
        }

        function requestStart(sender, eventArgs) {


            if (eventArgs.get_eventTarget().indexOf("lnkbtnPrintJob") != -1)
                eventArgs.set_enableAjax(false);


        }

    </script>
</telerik:RadCodeBlock>
<telerik:RadAjaxLoadingPanel ID="alpProductionSechduleMain" runat="server" Height="75px"
    MinDisplayTime="5" Width="75px">
    <asp:Image ID="Image1" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel ID="rapPSMain" RequestQueueSize="5" runat="server" Width="100%"
    OnAjaxRequest="rapPSMain_AjaxRequest" EnableOutsideScripts="True" HorizontalAlign="NotSet"
    ScrollBars="None" LoadingPanelID="alpProductionSechduleMain" ClientEvents-OnRequestStart="requestStart">
    <table id="tblSelection" width="100%">
        <tr>
            <td style="width: 250px">
                <asp:ValidationSummary ID="vs" ValidationGroup="vg" runat="server" ForeColor="Red" />
                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="rdpToDate"
                    ForeColor="Red" ControlToCompare="rdpFromDate" Operator="GreaterThanEqual" Type="Date"
                    ErrorMessage="Please Enter Valid Date Range<br />">
                </asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 400px">
                <b>Jobs By</b>
                <asp:CheckBox ID="cbByCustomer" Text="Cust." runat="server" AutoPostBack="True" />
                <asp:CheckBox ID="cbByAccountHandler" Text="Acct. Handler" runat="server" AutoPostBack="True" />
                <asp:CheckBox ID="cbByDate" Text="date" runat="server" AutoPostBack="True" />
                <%--                <asp:CheckBox ID="cbMore" Text="more" runat="server" AutoPostBack="True" />--%>
            </td>
            <td id="tdCustomerSelector" runat="server" visible="false">
                <telerik:RadComboBox ID="rcbCustomer" runat="server" AutoPostBack="True" />
            </td>
            <td id="tdAccountHandlerSelector" runat="server" visible="false">
                <telerik:RadComboBox ID="rcbAccountHandler" runat="server" AutoPostBack="True" />
            </td>
            <td id="tdDateSelector" runat="server" visible="true">
                <table style="width: 300px">
                    <tr>
                        <td style="width: 150px">
                            <telerik:RadDatePicker ID="rdpFromDate" runat="server" DateInput-DateFormat="ddd d-MMM-yyyy"
                                Width="150px" DateInput-EmptyMessage="from">
                                <Calendar ID="Calendar1" runat="server">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#ccccff">
                                        </telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                            <telerik:RadToolTip ID="rttFromDate" runat="server" TargetControlID="rdpFromDate"
                                CssClass="tooltipBackColor" RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip"
                                ShowCallout="true" Position="TopRight" Width="150px" Height="100px">
                                <div id="divFromDate" class="tooltip">
                                </div>
                            </telerik:RadToolTip>
                        </td>
                        <td style="width: 30px">
                            <telerik:RadDatePicker ID="rdpToDate" runat="server" DateInput-DateFormat="ddd d-MMM-yyyy"
                                Width="150px" DateInput-EmptyMessage="to">
                                <Calendar ID="Calendar2" runat="server">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#ccccff">
                                        </telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                            </telerik:RadDatePicker>
                            <telerik:RadToolTip ID="rttToDate" runat="server" TargetControlID="rdpToDate" CssClass="tooltipBackColor"
                                RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
                                Position="TopRight" Width="150px" Height="100px">
                                <div id="divToDate" class="tooltip">
                                </div>
                            </telerik:RadToolTip>
                        </td>
                    </tr>
                </table>
            </td>
            <td id="tdGoButton" runat="server" style="align: center">
                <asp:Button ID="btnGo" runat="server" Text="Go" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvJobs" runat="server" Width="100%" CellPadding="2" AutoGenerateColumns="False"
        CssClass="gridviewSpacing gvJobs" AllowPaging="true" ShowHeaderWhenEmpty="true">
        <Columns>
            <asp:TemplateField>
                <ItemStyle Width="70px" />
                <ItemTemplate>
                    <asp:LinkButton ID="lnkbtnEditJob" runat="server" CommandArgument='<%# Eval("JobKey")%>'
                        ToolTip="Edit" OnClick="lnkbtnEditJob_Click">
                        <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Portals/0/Images/Edit.gif" />
                    </asp:LinkButton>
                    <asp:LinkButton ID="lnkbtnPrintJob" runat="server" CommandArgument='<%# Eval("JobKey")%>'
                        ToolTip="Print" OnClick="lnkbtnPrintJob_Click">
                        <asp:Image ID="imgPrint" runat="server" ImageUrl="~/Portals/0/Images/Print.gif" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <%--            <asp:TemplateField HeaderText="Job No">
            <ItemStyle Width="100px" />
            <ItemTemplate >                
                <asp:Label ID="lblJobKey" runat="server" Text='<%# Eval("JobKey")%>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>
            --%>
            <asp:BoundField HeaderText="Job ID" DataField="JobKey" ReadOnly="true" Visible="true"
                ItemStyle-Width="70px" />
            <asp:TemplateField HeaderText="Job" SortExpression="JobName">
                <ItemStyle Width="300px" />
                <ItemTemplate>
                    <asp:Label ID="lblJobName" runat="server" Text='<%# IIf(FF_GLOBALS.bDebugMode, Eval("JobName") & " (" & Eval("JobKey") & ")", Eval("JobName")) %>' />
                    <telerik:RadToolTip ID="RadToolTipJobName" runat="server" TargetControlID="lblJobName"
                        Animation="Resize" EnableEmbeddedBaseStylesheet="false" RelativeTo="Element"
                        HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true" Position="TopRight"
                        Width="350px" Height="100px">
                        <div id="divJobName" class="radtooltip_Default">
                            <table>
                                <tr>
                                    <td>
                                        Job name:
                                    </td>
                                    <td>
                                        <%# Eval("JobName")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Created :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblJobCreatedOn" Text='<%# IsValidDate(Eval("CreatedOn")) %>' runat="server"></asp:Label>
                                        <%--<%# Format(Eval("CreatedOn"),"d-MMM-yyyy")%>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Collateral due :
                                    </td>
                                    <td>
                                        <%--<%# Format(Eval("CollateralDueOn"), "d-MMM-yyyy")%>--%>
                                        <asp:Label ID="lblCollateralDueOn" Text='<%# IsValidDate(Eval("CollateralDueOn")) %>'
                                            runat="server"></asp:Label>
                                        <%--<% IsValidDate(Bind("CollateralDueOn"))%>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Deadline :
                                    </td>
                                    <td>
                                        <%--<% IsValidDate(Bind("DeadlineOn"))%>--%>
                                        <asp:Label ID="lblDeadlineOn" Text='<%# IsValidDate(Eval("DeadlineOn")) %>' runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Completed :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCompletedOn" Text='<%# IsValidDate(Eval("CompletedOn")) %>' runat="server"></asp:Label>
                                        <%--<%# IsValidDate(Eval("CompletedOn"))%>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnCopyJobToClipboard" CommandArgument='<%# Eval("JobKey") %>' runat="server"
                                            Text="Copy Job to Clipboard" OnClick="btnCopyJobToClipboard_Click" />
                                        <asp:Button ID="btnNewJobFromClipboard" CommandArgument='<%# Eval("JobKey") %>' runat="server"
                                            Text="New Job From Clipboard" OnClick="btnNewJobFromClipboard_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblJobInfo" runat="server" />
                        </div>
                    </telerik:RadToolTip>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status">
                <HeaderTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td id="tbSearch" style="width: 250px" runat="server">
                                <asp:Label ID="lblHeader" Text="Status" runat="server"></asp:Label>
                                <%--                                <asp:Label ID="lblFilterJobs" runat="server" Text="Filter"></asp:Label>--%>
                                <asp:Image ID="imgSearch" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Portals/0/Images/Search.png" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="50%">
                                    <tr>
                                        <td align="left">
                                            <telerik:RadToolTip ID="rttMore" runat="server" TargetControlID="tbSearch" CssClass="radtooltip_Default"
                                                RelativeTo="Element" ShowCallout="true" HideDelay="4000" HideEvent="LeaveTargetAndToolTip"
                                                EnableEmbeddedBaseStylesheet="false" Position="TopCenter" Width="200px" Height="90px">
                                                <fieldset id="fsMain" runat="server">
                                                    <legend class="fieldsetLegend" id="fslgndMain" runat="server">Select Filter </legend>
                                                    <asp:CheckBox ID="chkShowUncompletedJobs" Text="Show <b>Uncompleted</b> Jobs" OnCheckedChanged="chkShowUncompletedJobs_CheckedChanged"
                                                        CssClass="searchtxt" runat="server" AutoPostBack="true" />
                                                    <br />
                                                    <asp:CheckBox ID="chkShowCompletedJobs" Text="Show <b>Completed</b> Jobs" OnCheckedChanged="chkShowCompletedJobs_CheckedChanged"
                                                        CssClass="searchtxt" runat="server" AutoPostBack="true" />
                                                    <br />
                                                    <asp:CheckBox ID="chkShowRecentlyCompletedJobs" Text="Show <b>Recently Completed</b> Jobs"
                                                        OnCheckedChanged="chkShowRecentlyCompletedJobs_CheckedChanged" CssClass="searchtxt"
                                                        runat="server" AutoPostBack="true" />
                                                    <br />
                                                    <hr />
                                                    <%--                                                <asp:CheckBox ID="cbIncludeNotCancelled" Text="Show <b>Cancelled</b> Jobs"
                                                    OnCheckedChanged="cbIncludeNotCancelled_CheckedChanged" CssClass="searchtxt" Checked="true"
                                                    AutoPostBack="true" runat="server" />
                                                <br />
                                                    --%>
                                                    <asp:CheckBox ID="cbIncludeCancelled" Text="Show <b>cancelled</b> jobs" OnCheckedChanged="cbIncludeCancelled_CheckedChanged"
                                                        CssClass="searchtxt" runat="server" AutoPostBack="true" />
                                                    <br />
                                                    <asp:CheckBox ID="cbIncludeTemplates" Text="Show <b>Templates</b>" runat="server"
                                                        AutoPostBack="true" OnCheckedChanged="cbIncludeTemplates_CheckedChanged" CssClass="searchtxt" />
                                                    <hr />
                                                    Search by job <b>name / number</b>
                                                    <telerik:RadTextBox ID="tbSearchByJobName" EmptyMessage="Leave empty for all jobs or enter a Job Name / Job No (Press Enter Key for search)"
                                                        Width="450px" runat="server" CssClass="txt" OnTextChanged="tbSearchByJobName_TextChanged" />
                                                    <asp:Button ID="btnGoSearchByJobName" runat="server" Text="Go" OnClick="btnGoSearchByJobName_Click" />
                                                    <%--Style="display: none"--%>
                                                    <br />
                                                    <asp:CheckBox ID="chkIndependentSearch" Text="<b>Independent Search</b>" runat="server"
                                                        AutoPostBack="true" OnCheckedChanged="chkIndependentSearch_CheckedChanged" CssClass="searchtxt" />
                                                    <br />
                                                    <asp:CheckBox ID="cbIgnoreDateCustomer" Text="ignore date and customer settings"
                                                        Checked="true" runat="server" />
                                                    <br />
                                                </fieldset>
                                            </telerik:RadToolTip>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <%--                <asp:CheckBox ID="cbUncompletedJobsOnly" runat="server" Checked='<%# pbShowUncompletedJobsOnly() %>'
                    Font-Size="XX-Small" Visible='<%# SetUncompletedJobsOnlyCheckboxVisibility() %>'
                    Text="show uncompleted jobs only" AutoPostBack="True" OnCheckedChanged="cbUncompletedJobsOnly_CheckedChanged" />
                    --%>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblJobStatus" runat="server" Text='<%# FF_JobState.GetJobStateSummary(Container.DataItem) %>' />
                    <div id="divJobStatus" class="tooltip">
                        <telerik:RadToolTip ID="RadToolTipJobStatus" runat="server" CssClass="tooltipBackColor"
                            ManualClose="false" TargetControlID="lblJobStatus" IsClientID="false" ShowEvent="OnMouseOver"
                            RelativeTo="Element" HideDelay="4000" HideEvent="ManualClose" ShowCallout="true"
                            Position="TopRight" Width="550px" Height="200px">
<%--                            <asp:UpdatePanel runat="server" ID="gvJobStatesUpdatePanel">
                                <ContentTemplate>--%>
                                    <asp:GridView ID="gvJobStates" runat="server" AutoGenerateColumns="False" ShowHeader="true"
                                        OnRowDataBound="gvJobStates_RowDataBound" Width="100%" OnRowCreated="gvJobStates_RowCreated">
                                        <HeaderStyle BackColor="SkyBlue" />
                                        <Columns>
                                            <asp:TemplateField SortExpression="JobStateName">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblHeader" Text="Stage" runat="server"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblJobStateName" runat="server" Text='<%# Eval("JobStateName") %>'
                                                        Font-Bold='<%# Eval("IsCompleted")%>' />
                                                    <asp:HiddenField ID="hidJobStateKey" runat="server" Value='<%# Eval("ID")%>' />
                                                    <asp:HiddenField ID="hidJobKey" runat="server" Value='<%# Eval("JobID")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="45%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label1" Text="Marked By" runat="server"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserName" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="45%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label2" Text="Completed" runat="server"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbIsCompleted" runat="server" AutoPostBack="true" 
                                                        Checked='<%# Eval("IsCompleted")%>' ValidationGroup='<%# Eval("ID")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%--                                    <asp:BoundField HeaderText="JobState ID" DataField="ID" ReadOnly="true" Visible="false" />
                                    <asp:BoundField HeaderText="Job ID" DataField="JobID" ReadOnly="true" Visible="false" />
                                            --%>
                                            <%--                                    <asp:TemplateField SortExpression="JobStateName"></asp:TemplateField>
                                    <asp:BoundField HeaderText="" DataField="JobID" ReadOnly="true" Visible="false" />
                                            --%>
                                            <%--<asp:TemplateField></asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
<%--                                </ContentTemplate>
                            </asp:UpdatePanel>
--%>                        </telerik:RadToolTip>
                    </div>
                    <asp:HiddenField ID="hidIsCompleted" Value='<%# Eval("IsCompleted")%>' runat="server" />
                    <asp:HiddenField ID="hidIsCancelled" Value='<%# Eval("IsCancelled")%>' runat="server" />

                    <%--<asp:HiddenField ID="hidrttClientId" Value= runat="server" />--%>

                </ItemTemplate>
                <ItemStyle Width="300px" />
            </asp:TemplateField>
            <asp:BoundField DataField="CustomerCode" HeaderText="Customer" ReadOnly="True" SortExpression="CustomerKey"
                ItemStyle-Width="100px" />
            <%--            <asp:BoundField DataField="AccountHandler" HeaderText="Acct Handler" ReadOnly="True"
                ItemStyle-Width="100px" HeaderStyle-Width="100px" ItemStyle-Wrap="true" SortExpression="AccountHandler" ItemStyle-CssClass=""  />
            --%>
            <asp:TemplateField HeaderText="Created" SortExpression="CreatedOn">
                <HeaderTemplate>
                    <asp:LinkButton ID="lnkbtnCreatedColumn" runat="server" OnClick="lnkbtnCreatedColumn_Click">Created</asp:LinkButton>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblCreatedOn" runat="server" Text='<%# TranslateDate(Eval("CreatedOn"),false) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Deadline" SortExpression="DeadlineDate">
                <HeaderTemplate>
                    <asp:LinkButton ID="lnkbtnDeadlineColumn" runat="server" OnClick="lnkbtnDeadlineColumn_Click">Deadline</asp:LinkButton>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblDeadline" runat="server" Text='<%# TranslateDate(Eval("DeadlineOn"), Container.DataItem) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <asp:Label ID="lblHeader" runat="server" ForeColor="Red" Text=" no jobs match the search criteria"></asp:Label>
            <%--            <telerik:RadToolTip ID="rttMore" runat="server" TargetControlID="lblHeader" CssClass="radtooltip_Default"
                RelativeTo="Element" HideDelay="4000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
                Position="TopCenter" Width="250px" Height="100px">
                <fieldset id="fsMore" runat="server">
                    <legend class="fieldsetLegend" id="fslgndMain" runat="server">Select Filter</legend>
                    <asp:CheckBox ID="chkShowUncompletedJobs" Text="Show <b>Uncompleted</b> Jobs" OnCheckedChanged="chkShowUncompletedJobs_CheckedChanged"
                        CssClass="searchtxt" runat="server" AutoPostBack="true" />
                    <br />
                    <asp:CheckBox ID="chkShowCompletedJobs" Text="Show <b>Completed</b> Jobs" OnCheckedChanged="chkShowCompletedJobs_CheckedChanged"
                        CssClass="searchtxt" runat="server" AutoPostBack="true" />
                    <br />
                    <asp:CheckBox ID="chkShowRecentlyCompletedJobs" Text="Show <b>Recently Completed</b> Jobs"
                        OnCheckedChanged="chkShowRecentlyCompletedJobs_CheckedChanged" CssClass="searchtxt"
                        runat="server" AutoPostBack="true" />
                    <br />
                    <hr />
                  
                    <asp:CheckBox ID="cbIncludeCancelled" Text="Show <b>cancelled</b> jobs" OnCheckedChanged="cbIncludeCancelled_CheckedChanged"
                        CssClass="searchtxt" runat="server" AutoPostBack="true" />
                    <br />
                    <asp:CheckBox ID="cbIncludeTemplates" Text="Show <b>Templates</b>" runat="server"
                        AutoPostBack="true" OnCheckedChanged="cbIncludeTemplates_CheckedChanged" CssClass="searchtxt" />
                    <hr />
                    Search by job <b>name / number</b>
                    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnGoSearchByJobName">
                        <telerik:RadTextBox ID="tbSearchByJobName" EmptyMessage="Leave empty for all jobs or enter a Job Name / Job No (Press Enter Key for search)"
                            Width="450px" runat="server" CssClass="txt" OnTextChanged="tbSearchByJobName_TextChanged" />
                        <asp:Button ID="btnGoSearchByJobName" runat="server" Text="Go" OnClick="btnGoSearchByJobName_Click" />
            --%>
            <%--Style="display: none" --%>
            <%--                        <br />
                        <asp:CheckBox ID="chkIndependentSearch" Text="<b>Independent Search</b>" runat="server"
                            Checked="true" Enabled="false" AutoPostBack="true" OnCheckedChanged="chkIndependentSearch_CheckedChanged"
                            CssClass="searchtxt" />
                        <br />
                        <asp:CheckBox ID="cbIgnoreDateCustomer" Text="ignore date and customer settings"
                            Checked="true" runat="server" />
                    </asp:Panel>
                    <br />
                </fieldset>
            </telerik:RadToolTip>
            --%>
        </EmptyDataTemplate>
    </asp:GridView>
<%--    <asp:Panel ID="pnlTimer" runat="server">
        <asp:Timer ID="timerJobs" runat="server" Interval="10000" OnTick="TimerJobs_Tick"></asp:Timer>
    </asp:Panel>
--%></telerik:RadAjaxPanel>
<br />
<table id="tblPrintJobList" width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            Show
            <asp:DropDownList ID="ddlJobCount" runat="server" AutoPostBack="True">
                <asp:ListItem Selected="True">10</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>100</asp:ListItem>
            </asp:DropDownList>
            &nbsp;jobs / page
        </td>
        <td align="right">
            <asp:Button ID="btnPrintJobList" runat="server" Text="Print Job List" />
        </td>
    </tr>
    <tr>
    </tr>
</table>
<script type="text/javascript">
    function SB_ShowImage(value) {
        window.open("show_image.aspx?Image=" + value, "ProductImage", "top=10,left=10,width=610,height=610,status=no,toolbar=no,address=no,menubar=no,resizable=yes,scrollbars=yes");
    }
</script>
