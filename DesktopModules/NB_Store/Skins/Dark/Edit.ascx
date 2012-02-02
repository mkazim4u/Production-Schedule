<%@ Control language="vb" CodeBehind="~/admin/Skins/skin.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<div id="ControlPanel" runat="server" visible="false"></div>
<table class="pagemaster editskin" border="0" cellspacing="0" cellpadding="0">
<tr>
<td valign="top" align="center" height="100%">
	<table width="960" border="0" cellspacing="0" cellpadding="0" height="100%">
	<tr><td valign="top" align="right" class="langpane"><dnn:LANGUAGE runat="server" id="dnnLANGUAGE" showMenu="False" showLinks="true" ItemTemplate="&lt;a href='[URL]' class='langfont' title='[CULTURE:NATIVENAME]'&gt;[CULTURE:NATIVENAME]&lt;/a&gt;" SelectedItemTemplate="&lt;a href='[URL]' class='langfont' title='[CULTURE:NATIVENAME]'&gt;[CULTURE:NATIVENAME]&lt;/a&gt;" SeparatorTemplate="&nbsp;|&nbsp;" /></td></tr>
  <tr><td id="ContentPane" class="contentpane" runat="server" valign="top" align="left" height="100%" ContainerType="G" ContainerName="_default" ContainerSrc="No Container.ascx"></td></tr>
  <tr><td class="copyright copypane" valign="top" align="left">Copyright &copy; Nevoweb Bright</td></tr>
	</table>
</td>
</tr>
</table>