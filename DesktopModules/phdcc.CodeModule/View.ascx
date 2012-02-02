<%@ Control language="vb" Inherits="phdcc.CodeModule.View" CodeFile="View.ascx.vb" AutoEventWireup="false" Explicit="True" %>

<div id="divAdmin" visible="false" runat="server" enableviewstate="false">
	<table style="border:1px solid red; padding:5px; ">
	<tr>
	<th valign="top" style="font-size:larger; font-weight:bold;">phdcc.CodeModule:</th>
	<td>
		<asp:Label ID="lblInfo" Text="" runat="server" EnableViewState="false" />
	</td>
	</tr>
	</table>
	
	<asp:Label ID="lblNoSettings" runat="server" Text="Please specify your control in Settings" Visible="false" />
	<asp:Button ID="btnSettings" runat="server" Text="Settings" Visible="false" />

</div>

<asp:PlaceHolder ID="ViewControl" runat="server" />
<pre id="lblError" runat="server" style="color:Red;" Visible="false" />