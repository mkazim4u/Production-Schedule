<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CheckOutSettings.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.CheckOutSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table style="width: 100%;">
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
            <dnn:label id="plShowShipMethod" runat="server" controlname="plShowShipMethod" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowShipMethod" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plHideShipInCart" runat="server" controlname="plHideShipInCart" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideShipInCart" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plHideVAT" runat="server" controlname="plHideVAT" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideVAT" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plHidePromo" runat="server" controlname="plHidePromo" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHidePromo" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plHideCountry" runat="server" controlname="plHideCountry" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideCountry" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSkipCart" runat="server" controlname="plSkipCart" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkSkipCart" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plHideChq" runat="server" controlname="plHideChq" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideChq" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plStockChq" runat="server" controlname="plStockChq" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkStockChq" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plHideShip" runat="server" controlname="plHideShip" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideShip" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plNonUserOrder" runat="server" controlname="chkNonUserOrder" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkNonUserOrder" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plDisableLoginMsg" runat="server" controlname="plDisableLoginMsg" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkDisableLoginMsg" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSmoothLogin" runat="server" controlname="plSmoothLogin" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkSmoothLogin" runat="server" />
        </td>
    </tr>
	<tr>
		<td>
			<dnn:label id="plSmoothLoginTab" runat="server" controlname="plSmoothLoginTab" suffix=":"></dnn:label>
		</td>
		<td>
		<asp:dropdownlist id="ddlSmoothLoginTab" runat="server" AutoPostBack="True"></asp:dropdownlist>
		</td>
	</tr>
    <tr>
        <td>
            <dnn:label id="plMinimumValidate" runat="server" controlname="plMinimumValidate" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkMinimumValidate" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plHideExtraInfo" runat="server" controlname="plHideExtraInfo" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideExtraInfo" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plShowStageHeader" runat="server" controlname="plShowStageHeader" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowStageHeader" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plGateway" runat="server" controlname="plGateway" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBoxList ID="chkGateway" runat="server" RepeatColumns="3"></asp:CheckBoxList>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plChequeGateway" runat="server" controlname="plChequeGateway" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBoxList ID="chkLEncapGateway" runat="server" RepeatColumns="3"></asp:CheckBoxList>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plGatewayDisplay" runat="server" controlname="plGatewayDisplay" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:RadioButtonList ID="rblGatewayDisplay" runat="server" RepeatDirection="Horizontal" ></asp:RadioButtonList>
        </td>
    </tr>

	<TR>
		<TD>
			<dnn:label id="plTabList" runat="server" controlname="lstTabs" suffix=":"></dnn:label>
		</TD>
		<TD>
		<asp:dropdownlist id="lstTabs" runat="server" AutoPostBack="True"></asp:dropdownlist>
		</TD>
	</TR>
	<TR>
		<TD>
			<dnn:label id="plTabContShop" runat="server" controlname="lstTabs" suffix=":"></dnn:label>
		</TD>
		<TD>
		<asp:dropdownlist id="lstTabContShop" runat="server" AutoPostBack="True"></asp:dropdownlist>
		</TD>
	</TR>
</table>
