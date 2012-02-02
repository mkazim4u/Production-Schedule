<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ShipRates.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.ShipRates" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table class="NBright_ContentDiv"><tr><td>
<asp:Panel ID="pnlEdit" runat="server">
<div class="NBright_ButtonDiv">
<asp:linkbutton id="cmdNew" runat="server" resourcekey="cmdNew" cssclass="NBright_AddButton" >Add New</asp:linkbutton>&nbsp;
<asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" cssclass="NBright_SaveButton" >Update</asp:linkbutton>&nbsp;
<asp:linkbutton id="cmdDelete" runat="server" resourcekey="cmdDelete" cssclass="NBright_DeleteButton" >Delete</asp:linkbutton>&nbsp;
&nbsp;&nbsp;<asp:Label ID="lblShipMethod" runat="server" Text="Method : " resourcekey="lblShipMethod"></asp:Label><asp:DropDownList id="ddlShipMethod" runat="server" AutoPostBack="true"></asp:DropDownList>
</div>
	<asp:Panel id="pnlProduct" runat="server">
<div class="NBright_EditDiv">
	<asp:TextBox id="txtPDefault" runat="server" Width="67px"></asp:TextBox>&nbsp;
	<dnn:label id="plPDefault" runat="server" suffix=":" controlname="txtPDefault"></dnn:label>
	<asp:Label id="lblPDefaultID" runat="server" Visible="False">Product Default ID</asp:Label>
    <asp:CompareValidator id="revPCDefault" runat="server" ErrorMessage="*" Type="Double" ControlToValidate="txtPDefault" Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
</div>
<div class="NBright_SelectDiv">
	<asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList>&nbsp;
    <asp:TextBox ID="txtSearch" runat="server" Width="169px"></asp:TextBox>
    <asp:LinkButton ID="cmdSearch" cssclass="NBright_CommandButton" resourcekey="cmdSearch" runat="server">Search</asp:LinkButton>
</div>
</asp:Panel><asp:Panel id="pnlCountry" runat="server">
<div class="NBright_EditDiv">
    <asp:TextBox id="txtCDefault" runat="server" Width="67px"></asp:TextBox>
    <asp:CompareValidator id="revCCDefault" runat="server" ErrorMessage="*" Type="Double" ControlToValidate="txtCDefault" Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
	<asp:Label id="lblCDefaultID" runat="server" Visible="False">Country Default ID</asp:Label>
    <dnn:label id="plCDefault" runat="server" suffix=":" controlname="txtCDefault"></dnn:label>
</div>
<div class="NBright_SelectDiv">
	<asp:DropDownList id="ddlCountry" runat="server"></asp:DropDownList>
	<asp:LinkButton id="cmdAddCounty" cssclass="NBright_CommandButton" resourcekey="cmdAddCounty" runat="server">Add</asp:LinkButton>
</div>
<dnn:label id="plRangeHelp" runat="server" suffix=":" controlname="txtCDefault"></dnn:label>
</asp:Panel><asp:Panel id="pnlRange" runat="server">
	<p>
		<asp:datagrid id="grdRange" runat="server" width="100%" gridlines="None" cellpadding="2" 
            autogeneratecolumns="False" showfooter="True" AllowPaging="True" 
            PageSize="20">
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
            <Columns>
                <asp:ButtonColumn CommandName="Delete" Text="&lt;img src='/images/delete.gif' alt='Delete' border='0' /&gt;"></asp:ButtonColumn>
                <asp:BoundColumn DataField="ItemId" HeaderText="Key" Visible="False">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ObjectId" HeaderText="ObjectId" Visible="False">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ShipType" HeaderText="ShipType" Visible="False">
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Description">
                </asp:BoundColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="lbldgFromHeader" Runat="server" resourcekey="dgFrom">Range1</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtRange1" runat="server" MaxLength="10" 
                            Text='<%# DataBinder.Eval(Container.DataItem, "Range1") %>' Width="100px"></asp:TextBox>
                        <asp:CompareValidator ID="cvRange1" runat="server" 
                            ControlToValidate="txtRange1" Display="Dynamic" ErrorMessage="*" 
                            Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="lbldgToHeader" Runat="server" 
                            resourcekey="dgTo">Range</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtRange2" runat="server" MaxLength="7" 
                            Text='<%# DataBinder.Eval(Container.DataItem, "Range2") %>' Width="100px">
                        </asp:TextBox>
                        <asp:CompareValidator ID="cvRange2" runat="server" 
                            ControlToValidate="txtRange2" Display="Dynamic" ErrorMessage="*" 
                            Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="lbldgCostHeader" Runat="server" 
                            resourcekey="dgCost">Cost</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtShipCost" runat="server" MaxLength="7" 
                            Text='<%# DataBinder.Eval(Container.DataItem, "ShipCost") %>' Width="100px">
                        </asp:TextBox>
                        <asp:CompareValidator ID="revShipCost" runat="server" 
                            ControlToValidate="txtShipCost" Display="Dynamic" ErrorMessage="*" 
                            Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="lbldgWeightHeader" Runat="server" 
                            resourcekey="dgWeight">Weight</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtWeight" runat="server" MaxLength="7" 
                            Text='<%# DataBinder.Eval(Container.DataItem, "ProductWeight") %>' Width="50px">
                        </asp:TextBox>
                        <asp:CompareValidator ID="cvWeight" runat="server" 
                            ControlToValidate="txtWeight" Display="Dynamic" ErrorMessage="*" 
                            Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="lbldgBox" Runat="server" resourcekey="dgBox">Box</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtBox" runat="server" MaxLength="10" 
                            Text='<%# DataBinder.Eval(Container.DataItem, "Range1") %>' Width="50px"></asp:TextBox>
                        <asp:CompareValidator ID="cvBox" runat="server" 
                            ControlToValidate="txtBox" Display="Dynamic" ErrorMessage="*" 
                            Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <asp:Label ID="lbldgDisableHeader" Runat="server" cssclass="NormalBold" 
                            resourcekey="dgDisable">Disable</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkDisable" runat="server" 
                            Checked='<%# DataBinder.Eval(Container.DataItem, "Disable") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
		</asp:datagrid></p>
</asp:Panel>
<asp:Panel id="pnlFree" runat="server">
<div class="NBright_EditDiv">
<asp:TextBox id="txtFreeShip" runat="server" Width="67px"></asp:TextBox>
<asp:CompareValidator id="CompareValidator1" runat="server" ErrorMessage="*" Type="Double" ControlToValidate="txtFreeShip" Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
<asp:Label ID="lblFreeShip" runat="server" Visible="false">FreeShipID</asp:Label>
<dnn:label id="plFreeShip" runat="server" suffix=":" controlname="txtPDefault"></dnn:label>
<br />
<asp:TextBox id="txtFreeShipList" runat="server" Width="200px"></asp:TextBox>
<dnn:label id="plFreeShipList" runat="server" suffix=":" controlname="txtFreeShipList"></dnn:label>
</div>
</asp:Panel>
</asp:Panel>
</td></tr>
</table>
