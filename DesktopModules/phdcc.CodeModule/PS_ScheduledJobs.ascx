<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_ScheduledJobs.ascx.vb"
    Inherits="PS_ScheduledJobs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="DNN" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<link href="module.css" rel="stylesheet" type="text/css" />
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript" language="javascript">

        function Reset() {


            document.getElementById("<%= tbJobNo.ClientID %>").value = "";
            document.getElementById("divValidationSummary").style.visibility = 'hidden';
            return false;
        }

        function ShowValidationDiv() {

            document.getElementById("divValidationSummary").style.display = 'block';
            document.getElementById("divValidationSummary").style.visibility = 'visible';
        }


        function Delete(id) {

            var JobId = id;

            if (confirm("Are you sure you want to delete this scheduled job " + JobId + "?")) {
                return true;
            }

            else {

                return false;
            }


        }

    </script>
</telerik:RadCodeBlock>
<%--<telerik:RadAjaxLoadingPanel ID="alpScheduledJobs" runat="server" Height="75px" MinDisplayTime="5"
    Width="75px">
    <asp:Image ID="Image2" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel ID="rapScheduledJobs" runat="server" Width="100%" EnableOutsideScripts="True"
    HorizontalAlign="NotSet" ScrollBars="None" LoadingPanelID="alpScheduledJobs">
--%>
    <fieldset id="fsMain" runat="server">
        <legend class="fieldsetLegend" id="fslgndMain" runat="server">Create or Edit Scheduled
            Job</legend>
        <br />
        <div id="divValidationSummary">
            <asp:ValidationSummary ID="vs" ValidationGroup="vg" runat="server" ForeColor="Red" />
        </div>
        <asp:Panel ID="pnlJob" runat="server" DefaultButton="btnSave">
            <asp:Label ID="lblJobId" Text="Job No" runat="server"></asp:Label>
            &nbsp;&nbsp;
            <asp:TextBox ID="tbJobNo" runat="server" MaxLength="10" ValidationGroup="vgRwJob" />
            <asp:RequiredFieldValidator ID="rfvtbContactName" runat="server" ErrorMessage="Please Enter Job No."
                Display="None" ControlToValidate="tbJobNo" ValidationGroup="vg"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revJobNo" ControlToValidate="tbJobNo" ForeColor="Red"
                ValidationGroup="vg" Display="None" ErrorMessage="Please Enter Valid Job Number"
                ValidationExpression="(^([0-9]*\d*\d{1}?\d*)$)" runat="server" />
            &nbsp;&nbsp;
            <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="vg"
                OnClientClick="ShowValidationDiv();" />
            &nbsp;&nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="Clear" CausesValidation="false" ValidationGroup="vg"
                OnClientClick="return Reset();" />
        </asp:Panel>
        <br />
        <br />
        <asp:GridView ID="gvScheduledJobs" runat="server" CellPadding="2" AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true" AllowSorting="true" Width="100%" AllowPaging="true"
            CssClass="gridviewSpacing gvScheduledJobs">
            <Columns>
                <asp:TemplateField ShowHeader="false">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Font-Bold="false" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtnEdit" runat="server" OnClick="lnkbtnEdit_Click" ToolTip="Edit"
                            CommandArgument='<%# Container.DataItemIndex %>' CommandName="edit">
                            <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Portals/0/Images/Edit.gif" /></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="lnkbtnRemove" runat="server" ToolTip="Delete" CommandName="delete"
                            OnClientClick='<%#Eval("JobID", "return Delete({0});")%>'>
                            <asp:Image ID="imgDelete" runat="server" ImageUrl="~/Portals/0/Images/delete.png" /></asp:LinkButton>
                        <asp:HiddenField ID="hidID" runat="server" Value='<%# Container.DataItem("ID")%>' />
                        <%--<asp:HiddenField ID="hidID" runat="server" Value='<%# Container.DataItemIndex %>' />--%>
                        <asp:HiddenField ID="hidJobID" runat="server" Value='<%# Container.DataItem("JobID")%>' />
                        <asp:HiddenField ID="hidCreatedBy" runat="server" Value='<%# Container.DataItem("CreatedBy")%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName="Update" 
                            ToolTip="Update" Text="">
                            <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ToolTip="Cancel" Text="">
                            <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" />
                        </asp:LinkButton>
                        <asp:HiddenField ID="hidID" runat="server" Value='<%# Container.DataItem("ID")%>' />
                        <asp:HiddenField ID="hidJobID" runat="server" Value='<%# Container.DataItem("JobID")%>' />
                        <asp:HiddenField ID="hidCreatedBy" runat="server" Value='<%# Container.DataItem("CreatedBy")%>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Job ID" DataField="JobId" Visible="true" ReadOnly="true" />
                <asp:TemplateField HeaderText="Last Run">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Font-Bold="false" />
                    <ItemTemplate>
                        <asp:Label ID="lblLastRun" runat="server" Text='<%# FF_Globals.IsValidDate(Eval("LastRun")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Next Run">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Font-Bold="false" />
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblNextRunDate" Text='<%# FF_Globals.IsValidDate(Eval("NextRun")) %>'> </asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadDatePicker ID="rdpNextRun" runat="server" SelectedDate='<%# Bind("NextRun", "{0:ddd d-MMM-yyyy}") %>'
                            DateInput-DateFormat="ddd d-MMM-yyyy" CssClass="fieldsetControlWidth fieldsetLine"
                            MinDate="1/1/1900" MaxDate="01/01/3000">
                            <Calendar ID="Calendar2" runat="server">
                                <SpecialDays>
                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="#ccccff">
                                    </telerik:RadCalendarDay>
                                </SpecialDays>
                            </Calendar>
                        </telerik:RadDatePicker>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Intervals">
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Font-Bold="false" />
                    <ItemTemplate>
                        <asp:CheckBox ID="chkDaily" Font-Bold="true" runat="server" Checked='<%# Bind("IsIntervalOnDaily") %>'
                            OnCheckedChanged="chkDaily_CheckedChanged" Text="Daily" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="chkWeekly" Font-Bold="true" runat="server" Checked='<%# Bind("IsIntervalInWeeks") %>'
                            OnCheckedChanged="chkWeekly_CheckedChanged" Text="Weekly" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="ChkMonthly" Font-Bold="true" runat="server" Checked='<%# Bind("IsIntervalInMonths") %>'
                            OnCheckedChanged="chkMonthly_CheckedChanged" Text="Monthly" AutoPostBack="true" />
                        &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Created By">
                    <ItemTemplate>
                        <asp:Label ID="lblCreatedBy" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Created" SortExpression="CreatedOn">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkbtnCreatedColumn" runat="server" OnClick="lnkbtnCreatedColumn_Click">Created</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCreatedOn" runat="server" Text='<%# FF_Globals.IsValidDate(Eval("CreatedOn")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>
<%--/telerik:RadAjaxPanel>--%>
<script type="text/javascript">
    function SB_ShowImage(value) {
        window.open("show_image.aspx?Image=" + value, "ProductImage", "top=10,left=10,width=610,height=610,status=no,toolbar=no,address=no,menubar=no,resizable=yes,scrollbars=yes");
    }
</script>