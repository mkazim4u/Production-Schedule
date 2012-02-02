<%@ Control language="vb" Inherits="NEvoWeb.Modules.NB_Store.AdminReport" AutoEventWireup="false" Explicit="True" Codebehind="AdminReport.ascx.vb" %>
<%@ Register TagPrefix="nwb" TagName="RepList" Src="AdminReportList.ascx"%>
<%@ Register TagPrefix="nwb" TagName="RepEdit" Src="AdminReportEdit.ascx"%>
<%@ Register TagPrefix="nbs" TagName="CustomForm" Src="controls/CustomForm.ascx" %>
<table class="NBright_ContentDiv"><tr><td>
<asp:Panel ID="pnlList" runat="server">
<nwb:RepList id="AdminReportList" runat="server"></nwb:RepList>	
</asp:Panel>
<asp:Panel ID="pnlEdit" runat="server">
<nwb:RepEdit id="AdminReportEdit" runat="server"></nwb:RepEdit>	
</asp:Panel>
<asp:Panel ID="pnlForm" runat="server">
<nbs:CustomForm ID="frmReport" runat="server" ReportForm="True"></nbs:CustomForm>
<div class="NBright_ButtonDiv">
<asp:LinkButton ID="cmdFormReturn" runat="server" cssclass="NBright_ReturnButton" resourcekey="cmdReturn" text="Return"></asp:LinkButton>&nbsp;
<asp:LinkButton ID="cmdFormRun" runat="server" cssclass="NBright_AcceptButton" resourcekey="cmdRunReport" text="Run Report"></asp:LinkButton>
</div>
</asp:Panel>
<asp:Panel ID="pnlResults" runat="server">
<br />
<asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" EnableViewState="False"></asp:Label>
    <asp:DataGrid id="dgResults" runat="server" AutoGenerateColumns="True" gridlines="None" cellpadding="1" Width="100%" AllowPaging="False" EnableViewState="False">
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
    </asp:DataGrid>
    <br />
<div class="NBright_ButtonDiv">
    <asp:LinkButton ID="cmdResultsReturn" runat="server" cssclass="NBright_ReturnButton" resourcekey="cmdReturn" text="Return"></asp:LinkButton>
</div>
</asp:Panel>
</td></tr>
</table>

