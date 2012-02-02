<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CartList.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.CartList" %>
		<table cellSpacing="0" cellPadding="0" width="100%" border="0" class="NBright_CartList">
			<tr>
				<td>
    <asp:DataGrid ID="dgCartList" runat="server" AutoGenerateColumns="False" showfooter="True" width="100%" gridlines="none" cellpadding="5">
			<AlternatingItemStyle cssclass="NBright_AltCartItem" />
            <ItemStyle cssclass="NBright_CartItem" />
        <Columns>
    	<asp:BoundColumn DataField="ItemID" Visible="False"></asp:BoundColumn>
    	<asp:BoundColumn DataField="ItemDesc" HeaderText="Description" headerstyle-cssclass="NBright_cartheader"></asp:BoundColumn>
    	    <asp:TemplateColumn HeaderText="Price" headerstyle-cssclass="NBright_cartheader" headerstyle-horizontalalign="Center" itemstyle-horizontalalign="Center"	footerstyle-horizontalalign="Right">
          <ItemTemplate>
          <asp:Label runat="server" ID="lblUnitCost" Text='<%# DataBinder.Eval(Container, "DataItem.UnitCost") %>'></asp:Label>
          <asp:TextBox ID="txtUnitCost" runat="server" width="70px" MaxLength="10" Text='' Visible="false"></asp:TextBox>
          </ItemTemplate>
				  <footertemplate>
                        <table style="line-height:150%;white-space:nowrap" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblTotals" Runat="server" resourcekey="lblTotals">Total:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblDiscount" Runat="server" resourcekey="lblDiscount">Discount:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblAfterDiscount" Runat="server" resourcekey="lblAfterDiscount">Total After Discount:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblShipping" Runat="server" resourcekey="lblShipping">Shipping:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblAppliedTax" Runat="server" resourcekey="lblTax">Tax:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="NBright_OrderTotalLabel">
                                    <asp:Label ID="lblOrderTotal" Runat="server" resourcekey="lblOrderTotal">Order Total:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblOutstanding" Runat="server" resourcekey="lblOutstanding">Outstanding:</asp:Label>
                                </td>
                            </tr>
                        </table>					
				  </footertemplate>
</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Qty" headerstyle-cssclass="NBright_cartheader" headerstyle-horizontalalign="Center" itemstyle-horizontalalign="Center"	footerstyle-horizontalalign="Center">
                <ItemTemplate>
                    <asp:TextBox ID="txtQty" runat="server" width="30px" MaxLength="6" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity") %>'></asp:TextBox>
                </ItemTemplate>
        <footertemplate>
					<asp:label id="lblQtyCount" runat="server"></asp:label>				
				</footertemplate>
				<footerstyle verticalalign="Top"></footerstyle>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Discount" headerstyle-cssclass="NBright_cartheader" headerstyle-horizontalalign="Center" itemstyle-horizontalalign="Center"	footerstyle-horizontalalign="Center">
                <ItemTemplate>
                       <asp:Label runat="server" ID="lblDiscount" Text='<%# DataBinder.Eval(Container, "DataItem.Discount") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Remove" headerstyle-cssclass="NBright_cartheader" headerstyle-horizontalalign="Center" itemstyle-horizontalalign="Center"	footerstyle-horizontalalign="Center">
                <ItemTemplate>
                    <asp:CheckBox ID="chkRemove" runat="server" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Total" headerstyle-cssclass="NBright_cartheader" headerstyle-horizontalalign="Center" itemstyle-horizontalalign="Right"	footerstyle-horizontalalign="Right" >
                <ItemTemplate>
                    <asp:Label ID="lblSubTotal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SubTotal") %>'></asp:Label>
                </ItemTemplate>
				<footertemplate>
                        <table style="line-height:150%;white-space:nowrap" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblDiscountTotal" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblTotalAfterDiscount" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblShippingTotal" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblAppliedTaxAmount" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="NBright_OrderTotalValue">
                                    <asp:Label ID="lblOrdTotal" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblOutstandingAmount" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
				</footertemplate>
            </asp:TemplateColumn>
        </Columns>

    </asp:DataGrid>
    </td>
			</tr>
		</table>
			<table cellspacing="0" cellpadding="0" border="0" width="100%">
                            <tr>                              
                                <td align="right"><div class="NBright_RecalcButtonDiv"><asp:LinkButton ID="cmdRecalculate" runat="server" cssclass="NBright_RecalcButton" resourcekey="cmdRecalculate" Text="Recalculate" /></div></td>
                            </tr>
                            <tr>
                                <td align="right"><asp:Label ID="lblEstShipDate" runat="server" Text="" Visible="false"></asp:Label><b><asp:Label ID="lblMsg" runat="server" Text="" Visible="false"></asp:Label></b></td>
                            </tr>
                            <tr>
                                <td align="right"><asp:Label ID="lblTax" Runat="server" resourcekey="lblTax">Tax:</asp:Label><asp:Label ID="lblTaxAmount" runat="server" cssclass="Normal"></asp:Label></td>
                            </tr>
			</table>

		
