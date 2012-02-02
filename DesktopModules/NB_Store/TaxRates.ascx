<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TaxRates.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.TaxRates" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table class="NBright_ContentDiv"><tr><td>
<asp:Panel ID="pnlEdit" runat="server">
<div class="NBright_ButtonDiv">
<asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server"  cssclass="NBright_SaveButton">Update</asp:linkbutton>&nbsp;
</div>
	<asp:panel id="pnlSetup" runat="server">
<div class="NBright_EditDiv">
	<asp:TextBox id="txtTDefault" Width="67px" runat="server"></asp:TextBox>
	<asp:CompareValidator id="cvTaxDefault" runat="server" ErrorMessage="*" Type="Double" ControlToValidate="txtTDefault" Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
	<asp:Label id="lblTDefaultID" runat="server" Visible="False">Tax Default ID</asp:Label>
    <dnn:label id="plTDefault" runat="server" controlname="txtTDefault" suffix=":"></dnn:label>
    <br />
	<asp:TextBox id="txtShipTax" Width="67px" runat="server"></asp:TextBox>
	<asp:CompareValidator id="cvShipTax" runat="server" ErrorMessage="*" Type="Double" ControlToValidate="txtShipTax" Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
	<asp:Label id="lblShipTaxID" runat="server" Visible="False">Ship Tax ID</asp:Label>
    <dnn:label id="plShipTax" runat="server" controlname="txtShipTax" suffix=":"></dnn:label>
    <br />
	<asp:Label id="lblTOptionsID" runat="server" Visible="False">Tax Options ID</asp:Label>
	<asp:RadioButtonList id="rblTaxOptions" Width="392px" runat="server">
		<asp:ListItem Value="1" Selected="True">No Tax</asp:ListItem>
		<asp:ListItem Value="2">Tax included in Unit Cost</asp:ListItem>
		<asp:ListItem Value="3">Tax NOT included in Unit Cost</asp:ListItem>
	</asp:RadioButtonList>    
</div>
</asp:panel><asp:panel id="pnlProduct" runat="server">
<div class="NBright_SelectDiv">
    <asp:TextBox ID="txtSearch" runat="server" Width="169px"></asp:TextBox>
    <asp:LinkButton ID="cmdSearch" cssclass="NBright_CommandButton" resourcekey="cmdSearch" runat="server">Search</asp:LinkButton>
</div>
</asp:panel><asp:panel id="pnlCategory" runat="server">
<div class="NBright_SelectDiv NBright_ButtonDiv">
	<asp:DropDownList id="ddlCategory" runat="server"></asp:DropDownList>
	<asp:LinkButton id="cmdAddCategory" cssclass="NBright_AddButton" runat="server" resourcekey="cmdAddCategory">Add</asp:LinkButton>
</div>
</asp:panel><asp:panel id="pnlRange" runat="server">
		<asp:datagrid id="grdRange"  width="100%" gridlines="None" cellpadding="2" runat="server" showfooter="True" 
            autogeneratecolumns="False" AllowPaging="True" PageSize="20">
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
			<Columns>
				<asp:ButtonColumn Text="&lt;img src='/images/delete.gif' alt='Delete' border='0' /&gt;" CommandName="Delete"></asp:ButtonColumn>
				<asp:BoundColumn Visible="False" DataField="ItemId" HeaderText="Key"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="ObjectId" HeaderText="ObjectId"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="TaxType" HeaderText="TaxType"></asp:BoundColumn>
				<asp:BoundColumn DataField="TaxDesc"></asp:BoundColumn>
				<asp:TemplateColumn>
					<HeaderTemplate>
						<asp:Label id="lbldgPercentHeader" resourcekey="dgPercent" Runat="server" cssclass="NormalBold">Percent</asp:Label>
					</HeaderTemplate>
					<ItemTemplate>
						<asp:TextBox id="txtTaxPercent" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TaxPercent") %>'>
						</asp:TextBox>
				<asp:CompareValidator id="cvTaxPercent" runat="server" ErrorMessage="*" Type="Double" ControlToValidate="txtTaxPercent" Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn>
					<HeaderTemplate>
						<asp:Label ID="lbldgDisableHeader" Runat="server" cssclass="NormalBold" resourcekey="dgDisable">Disable</asp:Label>
					</HeaderTemplate>
					<ItemTemplate>
						<asp:CheckBox id=chkDisable runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Disable") %>'>
						</asp:CheckBox>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:datagrid>
</asp:panel>

</asp:Panel>
</td></tr>
</table>
