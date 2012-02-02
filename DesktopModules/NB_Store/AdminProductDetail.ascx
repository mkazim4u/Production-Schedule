<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminProductDetail.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminProductDetail" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="nwb" TagName="ShowSelectLang" Src="controls/ShowSelectLang.ascx"%>
<table width="100%" border="0" align="center" cellspacing="1">
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"  align="right"><dnn:label id="labelManufacturer" runat="server" controlname="labelManufacturer" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:TextBox ID="txtManufacturer" Runat="server" Width="300" MaxLength="50" CssClass="NormalTextBox"></asp:TextBox><nwb:ShowSelectLang id="ShowSelectLang2" runat="server"></nwb:ShowSelectLang>
    </td>
    <td class="NormalBold" nowrap="nowrap"  rowspan="6">
    <table>
    <tr>
    <td>
    <nwb:ShowSelectLang id="ShowSelectLang1" runat="server"></nwb:ShowSelectLang><dnn:label id="labelSummary" runat="server" controlname="labelSummary" suffix=":"></dnn:label>
    <asp:TextBox ID="txtSummary" Runat="server" Width="500" Height="60" MaxLength="1000" TextMode="MultiLine" CssClass="NormalTextBox"></asp:TextBox>    
    </td>
    </tr>
    <tr>
    <td>
    <nwb:ShowSelectLang id="ShowSelectLang5" runat="server"></nwb:ShowSelectLang><dnn:label id="plTagWords" runat="server" controlname="plTagWords" suffix=":"></dnn:label>
    <asp:TextBox ID="txtTagWords" Runat="server" Width="500" Height="40" MaxLength="255" TextMode="MultiLine" CssClass="NormalTextBox"></asp:TextBox>    
    </td>
    </tr>
    </table>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"  align="right"><dnn:label id="labelProductName" runat="server" controlname="labelProductName" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:TextBox ID="txtProductName" Runat="server" Width="300" MaxLength="150" CssClass="NormalTextBox"></asp:TextBox><nwb:ShowSelectLang id="ShowSelectLang" runat="server"></nwb:ShowSelectLang>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap" align="right">
        <dnn:label id="labelSEOName" runat="server" controlname="labelSEOName" suffix=" : ">
        </dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap">
        <asp:TextBox ID="txtSEOName" runat="server" Width="300" MaxLength="256" CssClass="NormalTextBox"></asp:TextBox><nwb:ShowSelectLang ID="ShowSelectLang4" runat="server"></nwb:ShowSelectLang>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"  align="right"><dnn:label id="labelProductRef" runat="server" controlname="labelProductRef" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:TextBox ID="txtProductRef" Runat="server" Width="300" MaxLength="50" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"  align="right"><dnn:label id="plTaxCategory" runat="server" controlname="plTaxCategory" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:DropDownList ID="cmbCategory" Runat="server" Width="200" DataTextField="CategoryPathName" DataValueField="CategoryID"></asp:DropDownList>
    </td>
<tr valign="top">
<td class="Normal" nowrap="nowrap" colspan="2">

<table cellspacing="0" cellpadding="0" border="0" width="100%">
<tr>
<td class="NormalBold" align="right">
    <dnn:label id="labelArchived" runat="server" controlname="labelArchived" suffix=":"></dnn:label>
</td>
<td>
    <asp:CheckBox ID="chkArchived" Runat="server"></asp:CheckBox>
</td>
<td class="NormalBold"  align="right">
<dnn:label id="labelFeatured" runat="server" controlname="labelFeatured" suffix=":"></dnn:label>
</td>
<td  align="left">
    <asp:CheckBox ID="chkFeatured" Runat="server"></asp:CheckBox>
</td>
<td class="NormalBold"  align="right">
<dnn:label id="labelDeleted" runat="server" controlname="labelDeleted" suffix=":"></dnn:label>
</td>
<td  align="left">
    <asp:CheckBox ID="chkDeleted" Runat="server"></asp:CheckBox>
</td>
<td class="NormalBold"  align="right">
<dnn:label id="plIsHidden" runat="server" controlname="plIsHidden" suffix=":"></dnn:label>
</td>
<td  align="left">
    <asp:CheckBox ID="chkIsHidden" Runat="server"></asp:CheckBox>
</td>

</tr>
</table>


</td>
</tr>
  </tr>
  <tr valign="top">
  <td colspan="3"><asp:DataList ID="dlXMLData" runat="server"></asp:DataList>
  </td>
  </tr>
  <tr valign="top">
  <td colspan="3">
  <dnn:sectionhead id="dshEditor" ResourceKey="labelDescription" runat="server" cssclass="NormalBold" text="Description" section="tblDescription" includerule="false" isexpanded="false"></dnn:sectionhead>
<table id="tblDescription" runat="server" cellspacing="0" cellpadding="0" border="0" width="100%">
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><hr />
      <nwb:ShowSelectLang id="ShowSelectLang3" runat="server"></nwb:ShowSelectLang><dnn:label id="labelDescription" runat="server" controlname="labelDescription" suffix=":"></dnn:label>
    </td>
  </tr>
  <tr>
    <td class="Normal" nowrap="nowrap">
        <dnn:TextEditor id="txtDescription" runat="server" width="100%" height="350"></dnn:TextEditor>
    </td>
  </tr>
</table>
  </td>
</tr>
</table>
