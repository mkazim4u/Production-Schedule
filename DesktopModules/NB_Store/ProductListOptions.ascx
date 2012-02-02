<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProductListOptions.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.ProductListOptions" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table width="500" border="0" align="center" cellSpacing="5" id="tblBasic" runat="server">
    <tr>
		<TD width="150px"><dnn:label id="plDefaultCategory" runat="server" controlname="plDefaultCategory" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlDefaultCategory" runat="server"></asp:DropDownList>
	    </td>
	</tr>
	<tr>
        <td>
            <dnn:label id="plModuleTitle" runat="server" controlname="plModuleTitle" suffix=":"></dnn:label>
        </td>
        <td>        
            <asp:DropDownList id="lstModuleTitle" runat="server">
                <asp:ListItem Value="0">Default</asp:ListItem>
                <asp:ListItem Value="1">Product Title</asp:ListItem>
                <asp:ListItem Value="2">Category Name</asp:ListItem>
                <asp:ListItem Value="3">Product Title / Category Name</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
		<td><dnn:label id="plPageSize" runat="server" controlname="plPageSize" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:TextBox ID="txtPageSize" runat="server"></asp:TextBox>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plDefaultOrder" runat="server" controlname="plDefaultOrder" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlDefaultOrder" runat="server">
                <asp:ListItem Value="man">Manufacturer</asp:ListItem>
                <asp:ListItem Value="name">Name</asp:ListItem>
                <asp:ListItem Value="ref">Ref</asp:ListItem>
                <asp:ListItem Value="price">Price</asp:ListItem>
                <asp:ListItem Value="cdate">Created Date</asp:ListItem>
            </asp:DropDownList>
	    </td>
	</tr>
    <tr>
        <td>
            <dnn:label id="plDefaultOrderDESC" runat="server" controlname="plDefaultOrderDESC" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkDefaultOrderDESC" runat="server" />
        </td>
    </tr>	
    <tr>
        <td>
            <dnn:label id="plRedirectToCart" runat="server" controlname="plRedirectToCart" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkRedirectToCart" runat="server" />
        </td>
    </tr>	
	<tr>
		<td>
			<dnn:label id="plCategoryMessage" runat="server" controlname="plCategoryMessage" suffix=":"></dnn:label>
		</td>
		<td>
		    <asp:RadioButtonList ID="rblCategoryMessage" runat="server">
                <asp:ListItem Value="1">Never</asp:ListItem>
                <asp:ListItem Value="2">Always</asp:ListItem>
                <asp:ListItem Value="3">Empty</asp:ListItem>
            </asp:RadioButtonList>
		</td>
	</tr>
    <tr>
        <td>
            <dnn:label id="plIncrementCart" runat="server" controlname="plIncrementCart" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkIncrementCart" runat="server" />
        </td>
    </tr>	
    <tr>
        <td>
            <dnn:label id="plSkipList" runat="server" controlname="plSkipList" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkSkipList" runat="server" />
        </td>
    </tr>	
    </table>
<div class="NBright_ButtonDiv">
    <asp:linkbutton cssclass="NBright_SaveButton" id="cmdUpdate" resourcekey="cmdUpdate" runat="server" text="Update"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="NBright_CancelButton" id="cmdCancel" resourcekey="cmdCancel" runat="server" text="Cancel" causesvalidation="False"></asp:linkbutton>&nbsp;
</div>

