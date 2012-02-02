<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProductSearch.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.ProductSearch" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellspacing="0" cellpadding="0" summary="Search Input Table" border="0">
	<tr>
		<td><dnn:label id="plSearch" runat="server" controlname="cboModule" suffix=":"></dnn:label><asp:image id="imgSearch" runat="server"></asp:image></td>
		<td><asp:textbox id="txtSearch" runat="server" Wrap="False" maxlength="200"	cssclass="NBright_NormalTextBox"></asp:textbox></td>
		<td><asp:imagebutton id="imgGo" runat="server"></asp:imagebutton><asp:Button id="cmdGo" runat="server" Text="Go"></asp:Button></td>
	</tr>
</table>