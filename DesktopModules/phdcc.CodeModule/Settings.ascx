<%@ Control Language="vb" AutoEventWireup="false" CodeFile="Settings.ascx.vb" Inherits="phdcc.CodeModule.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="0" cellpadding="2" border="0" summary="phdcc.CodeModule Settings Design Table">
    <tr>
        <td class="SubHead" style="width:150px">
			<dnn:label id="lblControlName" runat="server" controlname="txtControlName" suffix=":"></dnn:label>
		</td>
        <td valign="bottom" >
            <asp:textbox id="txtControlName" cssclass="NormalTextBox" width="200" runat="server" />
        </td>
    </tr>
    
    <tr>
    <td colspan="2">
	    <p style="font-size:smaller">
		Powered by <a href="http://www.phdcc.com/phdcc.CodeModule/" target="_blank">phdcc.CodeModule</a>
		<asp:Label ID="lblModuleVersion" runat="server" />.<br />
		Copyright &copy; 2007-2009 PHD Computer Consultants Ltd
		</p>
	</td>
    </tr>

</table>
