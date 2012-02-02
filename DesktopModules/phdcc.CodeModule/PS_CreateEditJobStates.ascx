<%@ Control Language="VB" AutoEventWireup="true" CodeFile="PS_CreateEditJobStates.ascx.vb"
    Inherits="PS_CreateEditJobStates" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<link href="module.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    
</style>
<fieldset id="fsMain" runat="server">
    <legend class="fieldsetLegend" id="fslgndMain">
        <asp:Label ID="lblCreateEditJobStates" runat="server" Text="Create or Edit Job States" />
    </legend>
    <asp:ValidationSummary ID="vs" runat="server" ValidationGroup="vg" ForeColor="Red" />
    <telerik:RadAjaxLoadingPanel ID="alpCreateEditJobStates" runat="server" Height="75px"
        MinDisplayTime="5" Width="75px">
        <asp:Image ID="Image2" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="rapCreateEditJobStates" RequestQueueSize="5" runat="server"
        Width="100%" EnableOutsideScripts="True" HorizontalAlign="NotSet" ScrollBars="None"
        LoadingPanelID="alpCreateEditJobStates">
        <asp:GridView ID="gvJobStates" runat="server" Width="100%" AutoGenerateColumns="False"
            CellPadding="2" OnRowEditing="gvJobStates_RowEditing">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="10%" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkbtnAddAbove" runat="server" CommandArgument='<%# Container.DataItem("Position")%>'
                            OnClick="lnkbtnAddAbove_Click">add above</asp:LinkButton>
                        <br />
                        <asp:LinkButton ID="lnkbtnEdit" runat="server" CommandName="Edit" ToolTip="Edit">
                            <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Portals/0/Images/Edit.gif" /></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="lnkbtnRemove" runat="server" CommandName="Delete" ToolTip="Delete">
                            <asp:Image ID="imgDelete" runat="server" ImageUrl="~/Portals/0/Images/delete.png" /></asp:LinkButton>
                        <br />
                        <asp:LinkButton ID="lnkbtnAddBelow" runat="server" CommandArgument='<%# Container.DataItem("Position")%>'
                            OnClick="lnkbtnAddBelow_Click">add below</asp:LinkButton>
                        <asp:HiddenField ID="hidID" runat="server" Value='<%# Container.DataItem("ID")%>' />
                        <asp:HiddenField ID="hidCompleted" runat="server" Value='<%# Container.DataItem("IsCompleted")%>' />
                        <asp:HiddenField ID="hidJobStateName" runat="server" Value='<%# Container.DataItem("JobStateName")%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName="Update" ToolTip="Update">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel" ToolTip="Cancel">
                            <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" /></asp:LinkButton>
                        <asp:HiddenField ID="hidID" runat="server" Value='<%# Container.DataItem("ID")%>' />
                        <asp:HiddenField ID="hidCompleted" runat="server" Value='<%# Container.DataItem("IsCompleted")%>' />
                        <asp:HiddenField ID="hidJobStateName" runat="server" Value='<%# Container.DataItem("JobStateName")%>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Job stage" SortExpression="JobStateName">
                    <ItemStyle Width="80%" />
                    <ItemTemplate>
                        &nbsp;
                        <asp:Label ID="lblJobStateName" Font-Bold="true" runat="server" Text='<%# Bind("JobStateName") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        &nbsp;
                        <asp:TextBox ID="tbJobStateName" MaxLength="100" runat="server" Text='<%# Bind("JobStateName") %>'
                            ValidationGroup="vg" />
                        <asp:RequiredFieldValidator ID="rfvtbJobStateName" ControlToValidate="tbJobStateName"
                            runat="server" ForeColor="Red" ValidationGroup="vg" ErrorMessage="Please Enter Job Stage Name"
                            ToolTip="Please Enter Job Name"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <%--            <asp:TemplateField HeaderText="Alert if not completed by..." SortExpression="JobStateUncompletedAlertDateTime">
                <ItemTemplate>
                    &nbsp;
                    <asp:Label ID="lblJobStateUncompletedAlertDateTime" Font-Bold="true" runat="server"
                        Text='<%# Bind("JobStateUncompletedAlertDateTime") %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <telerik:RadDatePicker ID="rdpJobStateUncompletedAlertDateTime" DateInput-DateFormat="ddd d-MMM-yyyy"
                        runat="server" />
                    <asp:HiddenField ID="hidJobStateUncompletedAlertDateTime" runat="server" Value='<%# Bind("JobStateUncompletedAlertDateTime") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Alert on completion..." SortExpression="JobStateOnCompletionNotify">
                <ItemTemplate>
                    &nbsp;
                    <asp:Label ID="lblJobStateOnCompletionNotify" Font-Bold="true" runat="server" Text='<%# Bind("JobStateOnCompletionNotify") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    &nbsp;
                    <asp:TextBox ID="tbJobStateOnCompletionNotify" runat="server" MaxLength="50" Text='<%# Bind("JobStateOnCompletionNotify") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Add to work queue..." SortExpression="JobStateOnCompletionAction">
                <ItemTemplate>
                    &nbsp;
                    <asp:Label ID="lblJobStateOnCompletionAction" Font-Bold="true" runat="server" Text='<%# Bind("JobStateOnCompletionAction") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    &nbsp;
                    <asp:TextBox ID="tbJobStateOnCompletionAction" runat="server" MaxLength="50" Text='<%# Bind("JobStateOnCompletionAction") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
        <p>
            <asp:Button ID="btnSaveJobStages" runat="server" Text="Save Job Stages" CausesValidation="true" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" />
        </p>
    </telerik:RadAjaxPanel>
</fieldset>
<script type="text/javascript">
    function SB_ShowImage(value) {
        window.open("show_image.aspx?Image=" + value, "ProductImage", "top=10,left=10,width=610,height=610,status=no,toolbar=no,address=no,menubar=no,resizable=yes,scrollbars=yes");
    }
</script>
