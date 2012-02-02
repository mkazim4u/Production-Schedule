<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MiniCartSettings.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.MiniCartSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table style="width: 100%;">
	<tr>
		<td><dnn:label id="plFullTemplate" runat="server" controlname="plFullTemplate" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlFullTemplate" runat="server"></asp:DropDownList>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plEmptyTemplate" runat="server" controlname="plEmptyTemplate" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlEmptyTemplate" runat="server"></asp:DropDownList>
	    </td>
	</tr>
    <tr>
        <td>
            <dnn:label id="plShowFullCart" runat="server" controlname="plShowFullCart" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowFullCart" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plHideHeader" runat="server" controlname="plHideHeader" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideHeader" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plMiniPosition" runat="server" controlname="plMiniPosition" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:DropDownList ID="ddlMiniPosition" runat="server"></asp:DropDownList>
        </td>
    </tr>
   <tr>
        <td>
            <dnn:label id="plShowDiscountCol" runat="server" controlname="plShowDiscountCol" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowDiscountCol" runat="server" />
        </td>
    </tr>
   <tr>
        <td>
            <dnn:label id="plAllowEdit" runat="server" controlname="plAllowEdit" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkAllowEdit" runat="server" />
        </td>
    </tr>
   <tr>
        <td>
            <dnn:label id="plHideTotal" runat="server" controlname="plHideTotal" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideTotal" runat="server" />
        </td>
    </tr>


   <tr>
        <td>
            <dnn:label id="plHideDescriptionCol" runat="server" controlname="plHideDescriptionCol" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideDescriptionCol" runat="server" />
        </td>
    </tr>
   <tr>
        <td>
            <dnn:label id="plHidePriceCol" runat="server" controlname="plHidePriceCol" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHidePriceCol" runat="server" />
        </td>
    </tr>
   <tr>
        <td>
            <dnn:label id="plHideQtyCol" runat="server" controlname="plHideQtyCol" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideQtyCol" runat="server" />
        </td>
    </tr>
   <tr>
        <td>
            <dnn:label id="plHideRemoveCol" runat="server" controlname="plHideRemoveCol" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideRemoveCol" runat="server" />
        </td>
    </tr>
   <tr>
        <td>
            <dnn:label id="plHideTotalCol" runat="server" controlname="plHideTotalCol" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideTotalCol" runat="server" />
        </td>
    </tr>

</table>