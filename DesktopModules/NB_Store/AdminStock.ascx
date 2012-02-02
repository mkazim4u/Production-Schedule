<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminStock.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminStock" %>
<%@ Register TagPrefix="nbs" Namespace="NEvoWeb.Modules.NB_Store" Assembly="NEvoweb.DNN.Modules.NB_Store" %>
<table class="NBright_ContentDiv"><tr><td>
<asp:Panel ID="pnlList" runat="server">
<div class="NBright_ButtonDiv">
                <asp:LinkButton ID="cmdUpdate" runat="server"  
                    cssclass="NBright_SaveButton" resourcekey="cmdUpdate" text="Update"></asp:LinkButton>
</div>
<div class="NBright_SelectDiv">
            <asp:DropDownList ID="cmbCategory" Runat="server" Width="200" DataTextField="CategoryPathName" DataValueField="CategoryID"></asp:DropDownList>&nbsp;
                <asp:TextBox ID="txtSearch" runat="server" Width="169px"></asp:TextBox>
                <asp:LinkButton ID="cmdSearch" cssclass="NBright_CommandButton" runat="server" resourcekey="cmdSearch">Search</asp:LinkButton>
</div>
    <asp:DataGrid id="dgModel" runat="server" AutoGenerateColumns="False" 
            CellPadding="2" GridLines="None" Width="100%" >
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
			<Columns>
			<asp:BoundColumn DataField="ModelID" HeaderText="ID" Visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="Lang" HeaderText="Lang" Visible="false"></asp:BoundColumn>
   <asp:TemplateColumn HeaderText="Product">
      <ItemTemplate>
          <asp:Label ID="lblProductName" runat="server"></asp:Label>        
      </ItemTemplate>
    </asp:TemplateColumn>

			<asp:BoundColumn DataField="ModelRef" HeaderText="Ref"></asp:BoundColumn>
	<asp:TemplateColumn HeaderText="Model">
      <ItemTemplate>
          <asp:Label ID="lblModelName" runat="server"></asp:Label>        
      </ItemTemplate>
    </asp:TemplateColumn>

                <asp:TemplateColumn HeaderText="UnitCost">
                    <ItemTemplate>
                        <asp:TextBox ID="txtUnitCost" runat="server"  Width="60"  MaxLength="10"
                            Text='<%# DataBinder.Eval(Container, "DataItem.UnitCost") %>'></asp:TextBox>
<asp:CompareValidator id="validatorUnitPrice" runat="server" ErrorMessage="Error! Please enter a valid price."
				resourcekey="validatorUnitPrice" Type="Double" ControlToValidate="txtUnitCost" Operator="DataTypeCheck"
				Display="Dynamic"></asp:CompareValidator>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DealerCost">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDealerCost" runat="server"  Width="60"  MaxLength="10"
                            Text='<%# DataBinder.Eval(Container, "DataItem.DealerCost") %>'></asp:TextBox>
<asp:CompareValidator id="validatorDealerCost" runat="server" ErrorMessage="Error! Please enter a valid price."
				resourcekey="validatorDealerCost" Type="Double" ControlToValidate="txtDealerCost" Operator="DataTypeCheck"
				Display="Dynamic"></asp:CompareValidator>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DealerOnly">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkDealerOnly" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "DealerOnly") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="PurchaseCost">
                    <ItemTemplate>
                        <asp:TextBox ID="txtPurchaseCost" runat="server"  Width="60"  MaxLength="10"
                            Text='<%# DataBinder.Eval(Container, "DataItem.PurchaseCost") %>'></asp:TextBox>
<asp:CompareValidator id="validatorPurchaseCost" runat="server" ErrorMessage="Error! Please enter a valid price."
				resourcekey="validatorPurchaseCost" Type="Double" ControlToValidate="txtPurchaseCost" Operator="DataTypeCheck"
				Display="Dynamic"></asp:CompareValidator>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="QtyRemaining">
                    <ItemTemplate>
                        <asp:TextBox ID="txtQtyRemaining" runat="server" Width="30"  MaxLength="6"
                            Text='<%# DataBinder.Eval(Container, "DataItem.QtyRemaining") %>'></asp:TextBox> - 
                        <asp:TextBox ID="txtMaxStock" runat="server" Width="30"  MaxLength="6"
                            Text='<%# DataBinder.Eval(Container, "DataItem.QtyStockSet") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
			</Columns>
		</asp:DataGrid>
<asp:Label ID="lblLineBreak" runat="server" Text="<br/>"></asp:Label>
<nbs:AdminPagingControl id="ctlPagingControl" runat="server" pagesize="25" BorderWidth="0"></nbs:AdminPagingControl>
</asp:Panel>
</td></tr>
</table>
