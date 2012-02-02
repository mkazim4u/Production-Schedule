<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminSettings.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnntc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="nwb" TagName="SelectLang" Src="controls/SelectLang.ascx" %>
<table class="NBright_ContentDiv"><tr><td>
<table>
    <tr>
        <td valign="top" width="250px">
            <div class="NBright_ButtonDiv">
                <asp:LinkButton CssClass="NBright_AddButton" ID="cmdAdd" resourcekey="cmdAdd" runat="server"
                    Text="Add"></asp:LinkButton>
            </div>
            <asp:PlaceHolder ID="phTreeMenu" runat="server"></asp:PlaceHolder>
        </td>
        <td valign="top" width="700px">
        
            <asp:DataList ID="dlListMenu" runat="server" RepeatColumns="3" >
  <itemtemplate>
    <%#Container.DataItem.ToString%>
  </itemtemplate>
  <alternatingitemtemplate>
    <%#Container.DataItem.ToString%>
  </alternatingitemtemplate>
              </asp:DataList>
<asp:Panel ID="pnlEdit" runat="server">
<nwb:SelectLang ID="selectlang" runat="server"></nwb:SelectLang>
<div class="NBright_ButtonDiv">
    <asp:linkbutton cssclass="NBright_SaveButton" id="cmdUpdate" resourcekey="cmdUpdate" runat="server"  text="Update"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="NBright_CancelButton" id="cmdCancel" resourcekey="cmdCancel" runat="server"  text="Cancel" causesvalidation="False"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="NBright_DeleteButton" id="cmdDelete" resourcekey="cmdDelete" runat="server"  text="Delete" causesvalidation="False"></asp:linkbutton>&nbsp;
</div>
<table width="100%" border="0" align="center" cellSpacing="5">
	<tr>
		<td class="NormalBold" nowrap="nowrap" align="right">
    <dnn:label id="labelGroupName" runat="server" controlname="labelGroupName" suffix=":"></dnn:label>
		</td>
		<td>
		<asp:dropdownlist id="ddlGroups" runat="server" AutoPostBack="False"></asp:dropdownlist>                
        </td>
    <td class="NormalBold" nowrap="nowrap" align="right">
    <dnn:label id="labelTemplName" runat="server" controlname="labelTemplName" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:TextBox ID="txtTemplName" Runat="server" Width="200" MaxLength="50" CssClass="NormalTextBox"></asp:TextBox>
      <asp:RequiredFieldValidator ID="valReqCategoryName" runat="server" ControlToValidate="txtTemplName"
                Display="Dynamic" ErrorMessage="* Name is required." 
            resourcekey="valReqCategoryName"></asp:RequiredFieldValidator>
    </td>
	</tr>
	<tr>
	<td colspan="4">
		<asp:CheckBox ID="chkHostOnly" runat="server" resourcekey="chkHostOnly" />
            <asp:CheckBox ID="chkTextBoxEdit" runat="server" AutoPostBack="True" 
                Text="Edit with TextBox" resourcekey="chkTextBoxEdit" />
            <asp:CheckBox ID="chkUseFileSystem" runat="server" AutoPostBack="True" 
                Text="Use FileSystem" resourcekey="chkUseFileSystem" />
	</td>
	</tr>
   <tr>
    	<td colspan="4">
        <asp:RadioButtonList ID="rblCtrlType" runat="server" 
            RepeatDirection="Horizontal">
            <asp:ListItem Selected="True">TextBox</asp:ListItem>
            <asp:ListItem Value="Boolean">True/False</asp:ListItem>
            <asp:ListItem Value="MultiLineTextBox">MultiLine TextBox</asp:ListItem>
            <asp:ListItem Value="WebsiteTab">Website Page</asp:ListItem>
        </asp:RadioButtonList>
	</td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" colspan="4" nowrap="nowrap"><dnn:label id="labelMessage" runat="server" controlname="labelMessage" suffix=":"></dnn:label>
    </td>
  </tr>
  <tr>
    <td class="Normal" colspan="4" nowrap="nowrap" >
    <dnn:TextEditor id="txtEditor" runat="server" width="100%" height="400"></dnn:TextEditor>
    <asp:TextBox ID="txtMessage" runat="server" width="100%"></asp:TextBox>
    <asp:RadioButtonList ID="rblSettingValue" runat="server"></asp:RadioButtonList>
    <asp:DropDownList ID="ddlTabs" runat="server"></asp:DropDownList>
    </td>
  </tr>
  <tr>
    <td class="Normal" colspan="4" nowrap="nowrap" >
    <asp:TextBox ID="txtHelp" runat="server" width="100%" TextMode="MultiLine" Height="120px" Enabled="false" CssClass="BackOfficeHelp"></asp:TextBox><br />
    </td>
  </tr>

</table>
</asp:Panel>
        </td>
    </tr>
</table>
</td></tr>
</table>
