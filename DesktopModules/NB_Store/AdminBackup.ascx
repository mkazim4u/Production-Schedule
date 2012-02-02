<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminBackup.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminBackup" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register assembly="DotNetNuke" namespace="DotNetNuke.UI.WebControls" tagprefix="cc1" %>
<table class="NBright_ContentDiv"><tr><td>
<asp:Panel ID="pnlBackup" runat="server">
<div class="NBright_ButtonDiv">
    <asp:LinkButton ID="cmdDoBackup" cssclass="NBright_CommandButton" runat="server" resourcekey="cmdDoBackup">Export</asp:LinkButton>
</div>   
<div class="NBright_EditDiv">
<table>
<tr>
<td>
<dnn:label id="plAllProducts" runat="server" controlname="plAllProducts" suffix=":" resourcekey="plAllProducts"></dnn:label>
</td>
<td>
<asp:RadioButton ID="rbAllProducts" runat="server"  GroupName="GRP1"/><asp:CheckBox ID="chkExpOrders" runat="server" text="Include All Orders in product export file" />
</td>
</tr>
<tr>
<td>
<dnn:label id="plProductImages" runat="server" controlname="plProductImages" suffix=":" resourcekey="plProductImages"></dnn:label>
</td>
<td>
<asp:RadioButton ID="rbProductImages" runat="server"  GroupName="GRP1"/>
</td>
</tr>
<tr>
<td>
<dnn:label id="plProductDocs" runat="server" controlname="plProductDocs" suffix=":" resourcekey="plProductDocs"></dnn:label>
</td>
<td>
<asp:RadioButton ID="rbProductDocs" runat="server"  GroupName="GRP1"/>
</td>
</tr>
<tr>
<td>
<dnn:label id="plAllOrders" runat="server" controlname="plAllOrders" suffix=":" resourcekey="plAllOrders"></dnn:label>
</td>
<td>
<asp:RadioButton ID="rbAllOrders" runat="server"  GroupName="GRP1"/>
</td>
</tr>
<tr>
<td>
<dnn:label id="plShipping" runat="server" controlname="plShipping" suffix=":" resourcekey="plShipping"></dnn:label>
</td>
<td>
<asp:RadioButton ID="rbShipping" runat="server"  GroupName="GRP1"/>
</td>
</tr>
<tr>
<td>
<dnn:label id="plPurgeStore" runat="server" controlname="plPurgeStore" suffix=":" resourcekey="plPurgeStore"></dnn:label>
</td>
<td>
<asp:RadioButton ID="rbPurgeStore" runat="server"  GroupName="GRP1"/>
</td>
</table>
</div>   
</asp:Panel> 
<asp:Panel ID="pnlRestore" runat="server">
    <div class="NBright_ButtonDiv">
        <asp:LinkButton ID="cmdDoImport" cssclass="NBright_CommandButton" runat="server" resourcekey="cmdDoImport">Import</asp:LinkButton>
    </div>    
<div class="NBright_EditDiv">
<table>
<tr>
<td>
<dnn:label id="plImportUpdate" runat="server" controlname="plImportUpdate" suffix=":" resourcekey="plImportUpdate"></dnn:label>
</td>
<td>
<asp:RadioButton ID="rbImportUpdate" runat="server"  GroupName="GRP1"/>
</td>
</tr>
<tr>
<td>
<dnn:label id="plImportNew" runat="server" controlname="plImportNew" suffix=":" resourcekey="plImportNew"></dnn:label>
</td>
<td>
<asp:RadioButton ID="rbImportNew" runat="server"  GroupName="GRP1"/>
</td>
</tr>
<tr>
<td>
<dnn:label id="plCreateCat" runat="server" controlname="plCreateCat" suffix=":" resourcekey="plCreateCat"></dnn:label>
</td>
<td>
<asp:CheckBox ID="chkCreateCat" runat="server" />
</td>
</tr>
<tr>
<td>
<dnn:label id="plArchiveProd" runat="server" controlname="chkArchiveProd" suffix=":" resourcekey="plArchiveProd"></dnn:label>
</td>
<td>
<asp:CheckBox ID="chkArchiveProd" runat="server" />
</td>
</tr>
<tr>
<td>
<dnn:label id="plXMLFile" runat="server" controlname="plXMLFile" suffix=":" resourcekey="plXMLFile"></dnn:label>
</td>
<td>
<asp:FileUpload ID="FileUploadXML" runat="server" />
</td>
</tr>
<tr>
<td>
<dnn:label id="plZipFile" runat="server" controlname="plZipFile" suffix=":" resourcekey="plZipFile"></dnn:label>
</td>
<td>
<asp:FileUpload ID="FileUploadZip" runat="server" />
</td>
</tr>
<tr>
<td>
<dnn:label id="plZipDocs" runat="server" controlname="plZipDocs" suffix=":" resourcekey="plZipDocs"></dnn:label>
</td>
<td>
<asp:FileUpload ID="FileUploadDocs" runat="server" />
</td>
</tr>
</table>
<hr />
<table>
<tr>
<td>
<dnn:label id="plImportShipping" runat="server" controlname="plImportShipping" suffix=":" resourcekey="plImportShipping"></dnn:label>
</td>
<td>
<asp:FileUpload ID="FileUploadShip" runat="server" />
</td>
</tr>
</table>
<hr />
<table>
<tr>
<td>
<dnn:label id="plImportModel" runat="server" controlname="plImportModel" suffix=":" resourcekey="plImportModel"></dnn:label>
</td>
<td>
<asp:FileUpload ID="FileUploadModel" runat="server" />
</td>
</tr>
</table>

</div>    
</asp:Panel> 
<asp:Label ID="lblMsg" runat="server" Text="" Font-Bold="True"></asp:Label>    
</td></tr>
</table>
