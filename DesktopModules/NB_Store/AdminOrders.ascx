<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminOrders.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminOrders" %>
<%@ Register TagPrefix="dnntc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="nbs" TagName="CustomForm" Src="controls/CustomForm.ascx" %>
<%@ Register TagPrefix="nbs" TagName="Address" Src="Address.ascx" %>
<%@ Register TagPrefix="nbs" TagName="CartList" Src="CartList.ascx" %>
<table class="NBright_ContentDiv"><tr><td>
<asp:Panel ID="pnlOrderList" runat="server">
<div class="NBright_ButtonDiv">
<asp:LinkButton ID="cmdReturn" runat="server" cssclass="NBright_ReturnButton" resourcekey="cmdReturn" text="Return"></asp:LinkButton>&nbsp;
<asp:LinkButton ID="cmdCreate" cssclass="NBright_CommandButton" runat="server" resourcekey="cmdCreate">Create</asp:LinkButton>
</div>
<div class="NBright_SelectDiv">
		<asp:Label ID="lblOrderNbr" runat="server" Text="No." resourcekey="lblOrderNbr"></asp:Label>&nbsp;
                <asp:TextBox id="txtOrderNbr" runat="server" MaxLength="20" Width="50px"></asp:TextBox>&nbsp;
		<asp:Label ID="lblName" runat="server" Text="Name" resourcekey="lblName"></asp:Label>&nbsp;
                <asp:TextBox id="txtFilter" runat="server" MaxLength="20"  Width="100px"></asp:TextBox>&nbsp;
                <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True"></asp:DropDownList>&nbsp;             
                <asp:Label ID="lblFrom" runat="server" Text="Label" resourcekey="lblFrom"></asp:Label>
                <asp:TextBox id="txtFromDate" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
				<asp:HyperLink id="hypFromDate" runat="server">HyperLink</asp:HyperLink>            
				<asp:CompareValidator id="cvFromDate" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFromDate"
						ErrorMessage="*"></asp:CompareValidator>&nbsp;
                <asp:Label ID="lblTo" runat="server" Text="Label" resourcekey="lblTo"></asp:Label>
                <asp:TextBox id="txtToDate" runat="server" MaxLength="20"  Width="80px"></asp:TextBox>
				<asp:HyperLink id="hypToDate" runat="server">HyperLink</asp:HyperLink>            
				<asp:CompareValidator id="cvToDate" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtToDate"
						ErrorMessage="*"></asp:CompareValidator>&nbsp;
            <asp:LinkButton ID="cmdSelect" cssclass="NBright_CommandButton" runat="server" resourcekey="cmdSelect">Select</asp:LinkButton>
</div>
    <asp:DataGrid ID="dgOrderList" runat="server" AutoGenerateColumns="False" showfooter="True" width="100%" 
    AllowPaging="True" PageSize="25" cellpadding="2" gridlines="None">
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
        <Columns>
        <asp:BoundColumn DataField="OrderID" Visible="false"/>
		<dnntc:ImageCommandColumn KeyField="OrderID" ShowImage="True" ImageURL="~/images/edit.gif" CommandName="Edit"
			EditMode="Command"><HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle><EditItemTemplate></EditItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle><HeaderTemplate></HeaderTemplate><ItemTemplate></ItemTemplate></dnntc:ImageCommandColumn>
        <asp:BoundColumn DataField="ShortOrderNumber" HeaderText="OrderNumber" headerstyle-cssclass="NormalBold">
<HeaderStyle CssClass="NormalBold"></HeaderStyle>
            </asp:BoundColumn>
          <asp:TemplateColumn HeaderText="" headerstyle-cssclass="NormalBold">
          <ItemTemplate>
          <asp:Label runat="server" ID="lblName" Text='<%# DataBinder.Eval(Container, "DataItem.Email") %>'></asp:Label>
          </ItemTemplate>
          </asp:TemplateColumn>            
        <asp:BoundColumn DataField="OrderDate" HeaderText="OrderDate" headerstyle-cssclass="NormalBold">
<HeaderStyle CssClass="NormalBold"></HeaderStyle>
            </asp:BoundColumn>
        <asp:BoundColumn DataField="OrderStatusID" HeaderText="OrderStatus" headerstyle-cssclass="NormalBold">
<HeaderStyle CssClass="NormalBold"></HeaderStyle>
            </asp:BoundColumn>

        <asp:BoundColumn DataField="Total" HeaderText="Total" headerstyle-cssclass="NormalBold" DataFormatString="{0:F}">
<HeaderStyle CssClass="NormalBold" HorizontalAlign="Right"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right" />
            </asp:BoundColumn>
		<dnntc:ImageCommandColumn KeyField="OrderID" ShowImage="True" ImageURL="~/images/cart.gif" CommandName="gotocart"
			EditMode="Command"><HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle><EditItemTemplate></EditItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle><HeaderTemplate></HeaderTemplate><ItemTemplate></ItemTemplate></dnntc:ImageCommandColumn>
        </Columns>
    </asp:DataGrid>

</asp:Panel>
<asp:Panel ID="pnlOrderEdit" runat="server">
    <div class="NBright_ButtonDiv">
    <asp:LinkButton ID="cmdEditOrder" runat="server" cssclass="NBright_EnterButton" resourcekey="cmdEditOrder" text="Edit"></asp:LinkButton>
    &nbsp;    
    <asp:LinkButton ID="cmdUpdate" runat="server" cssclass="NBright_SaveButton" resourcekey="cmdUpdate" text="Update"></asp:LinkButton>
    &nbsp;    
    <asp:LinkButton ID="cmdReturn2" runat="server" cssclass="NBright_ReturnButton" resourcekey="cmdReturn" text="Return"></asp:LinkButton>    
    &nbsp;    
    <asp:LinkButton ID="cmdReOrder" runat="server" cssclass="NBright_SaveButton" resourcekey="cmdReOrder" text="ReOrder"></asp:LinkButton>
    &nbsp;    
    <asp:LinkButton ID="cmdPrintOrder" runat="server" cssclass="NBright_PrintButton" resourcekey="cmdPrintOrder" text="Print Order"></asp:LinkButton>    
    &nbsp;    
    <asp:LinkButton ID="cmdPrintreceipt" runat="server" cssclass="NBright_PrintButton" resourcekey="cmdPrintreceipt" text="Print receipt"></asp:LinkButton>    
    </div>
    <asp:Label ID="lblPopupMsg" runat="server" Text="Label" resourcekey="lblPopupMsg"></asp:Label>   
    <asp:Panel ID="pnlUpdate" runat="server">
    <div class="NBright_ActionDiv">
                    <asp:LinkButton ID="cmdSendAmendEmail" runat="server" cssclass="NBright_ActionButton" resourcekey="cmdSendAmendEmail" text="Amended Email"></asp:LinkButton>    
                    <asp:LinkButton ID="cmdSendValidateOrder" runat="server" cssclass="NBright_ActionButton" resourcekey="cmdSendValidateOrder" text="Validate Order"></asp:LinkButton> 
                    <asp:LinkButton ID="cmdSendShipEmail" runat="server" cssclass="NBright_ActionButton" resourcekey="cmdSendShipEmail" text="Ship Email"></asp:LinkButton>    
                    <asp:LinkButton ID="cmdSendReceiptEmail" runat="server" cssclass="NBright_ActionButton" resourcekey="cmdSendReceiptEmail" text="receipt Email"></asp:LinkButton>    
</div>
<div  class="NBright_EditDiv">
                <asp:Label ID="lblShipDate" runat="server" Text="Label" resourcekey="lblShipDate"></asp:Label><asp:TextBox id="txtShipDate" runat="server" MaxLength="20"></asp:TextBox>
				<asp:HyperLink id="hypShipDate" runat="server">HyperLink</asp:HyperLink>&nbsp;
                <asp:Label ID="lblTrackCode" runat="server" Text="Label" resourcekey="lblTrackCode"></asp:Label><asp:TextBox id="txtTrackCode" runat="server" MaxLength="50"></asp:TextBox>&nbsp;
				<asp:CompareValidator id="CompareValidator1" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtShipDate"
						ErrorMessage="*"></asp:CompareValidator>
                <asp:Label ID="lblStatus" runat="server" Text="Label" resourcekey="lblStatus"></asp:Label><asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>                
</div>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
    <hr />
    <table>
    <tr>
    <td>
         <nbs:address id="EditBAddrForm" runat="server" NoValidate="true" Templatename="editaddress.template"></nbs:address>    
    </td>
    <td>
         <nbs:address id="EditSAddrForm" runat="server" NoValidate="true" Templatename="editaddress.template"></nbs:address>    
    </td>
    </tr>
<tr>
<td colspan="2">
    <asp:Label ID="lblOrderEmail" runat="server" Text="Email : " resourcekey="lblOrderEmail"></asp:Label><asp:TextBox ID="txtOrderEmail" runat="server" Width="200"></asp:TextBox>
</td>
</tr>    
<tr>
<td colspan="2">
<hr />
    <asp:TextBox ID="txtEditNoteMsg" runat="server" Width="600" Height="150" TextMode="MultiLine"></asp:TextBox>
</td>
</tr>
<tr>
<td colspan="2">
    <nbs:CartList runat="server" id="cartlist1" />
</td>
</tr>    
<tr>
<td><asp:LinkButton ID="cmdAddProduct" runat="server" cssclass="NBright_ActionButton" resourcekey="cmdAddProduct" text="Add"></asp:LinkButton> 
</td>
<td>
    <asp:Label ID="lblAlreadyPaid" runat="server" Text="Paid" resourcekey="lblAlreadyPaid"></asp:Label>
    <asp:TextBox ID="txtAlreadyPaid" runat="server" Width="90"></asp:TextBox>
</td>
</tr>
    </table>
    <hr />
    </asp:Panel>
    <asp:PlaceHolder ID="plhOrder" runat="server"></asp:PlaceHolder>
</asp:Panel>


</td></tr>
</table>
