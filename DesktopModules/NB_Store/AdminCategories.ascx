<%@ Control Language="vb" Inherits="NEvoWeb.Modules.NB_Store.AdminCategories" AutoEventWireup="false"
    Explicit="True" CodeBehind="AdminCategories.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="nwb" TagName="SelectLang" Src="controls/SelectLang.ascx" %>
<%@ Register TagPrefix="nwb" TagName="ShowSelectLang" Src="controls/ShowSelectLang.ascx" %>
<%@ Register TagPrefix="nbs" Namespace="NEvoWeb.Modules.NB_Store" Assembly="NEvoweb.DNN.Modules.NB_Store" %>
<%@ Register TagPrefix="nbs" TagName="ProdList" Src="~/DesktopModules/NB_Store/AdminProductList.ascx" %>
<table class="NBright_ContentDiv"><tr><td>
            <asp:Panel ID="pnlList" runat="server">
                <nbs:ProdList id="productlist" runat="server">
                </nbs:ProdList>
            </asp:Panel>
            <asp:Panel ID="pnlCategory" runat="server">

<table>
    <tr>
        <td valign="top" width="250px" >        
            <div class="NBright_ButtonDiv">
                <asp:LinkButton CssClass="NBright_AddButton" ID="cmdAdd" resourcekey="cmdAdd" runat="server"
                    Text="Add"></asp:LinkButton>
            </div>
            <asp:PlaceHolder ID="phTreeMenu" runat="server"></asp:PlaceHolder>
        </td>
        <td valign="top">
        
            <asp:Panel ID="pnlEdit" runat="server">
            

        
                <nwb:SelectLang ID="selectlang" runat="server"></nwb:SelectLang>
                <div class="NBright_ButtonDiv">
                    <asp:LinkButton CssClass="NBright_SaveButton" ID="cmdUpdate" resourcekey="cmdUpdate"
                        runat="server" Text="Update"></asp:LinkButton>&nbsp;
                    <asp:LinkButton CssClass="NBright_CancelButton" ID="cmdCancel" resourcekey="cmdCancel"
                        runat="server" Text="Cancel" CausesValidation="False"></asp:LinkButton>&nbsp;
                    <asp:LinkButton CssClass="NBright_DeleteButton" ID="cmdDelete" resourcekey="cmdDelete"
                        runat="server" Text="Delete" CausesValidation="False"></asp:LinkButton>
                </div>
                <p>
                    <asp:Label ID="lblErrorDelete" runat="server" Font-Bold="true" ForeColor="red" resourcekey="ErrorDelete"
                        Text="Unable to delete category. Category has dependant categories or Products."
                        Visible="false"></asp:Label>
                </p>
    <div id="tabs" runat="server" class="NBright_tabs">
            <a id="buttontabs-1" href="#"><asp:Label ID="lblGeneral" runat="server" Text="General" resourcekey="lblGeneral"></asp:Label></a>
            <a id="buttontabs-2" href="#"><asp:Label ID="lblProducts" runat="server" Text="Products" resourcekey="lblProducts"></asp:Label></a>
    </div>
        <div id="tabs-1" >
                <table width="500" border="0" align="center" cellspacing="2">
                    <tr valign="top">
                        <td class="NormalBold" nowrap="nowrap">
                            <dnn:label id="labelCategoryName" runat="server" controlname="labelCategoryName"
                                suffix=":"></dnn:label>
                        </td>
                        <td class="Normal" nowrap="nowrap">
                            <asp:TextBox ID="txtCategoryName" runat="server" Width="200" MaxLength="50" CssClass="NormalTextBox"></asp:TextBox><nwb:ShowSelectLang
                                ID="ShowSelectLang" runat="server"></nwb:ShowSelectLang>
                            <asp:RequiredFieldValidator ID="valReqCategoryName" runat="server" ControlToValidate="txtCategoryName"
                                Display="Dynamic" ErrorMessage="* Category name is required." resourcekey="valReqCategoryName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="NormalBold" nowrap="nowrap">
                            <dnn:label id="labelCategoryDescription" runat="server" controlname="labelCategoryDescription"
                                suffix=":"></dnn:label>
                        </td>
                        <td class="Normal" nowrap="nowrap">
                            <asp:TextBox ID="txtDescription" runat="server" Width="350" MaxLength="500" CssClass="NormalTextBox"></asp:TextBox><nwb:ShowSelectLang
                                ID="ShowSelectLang1" runat="server"></nwb:ShowSelectLang>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="NormalBold" nowrap="nowrap">
                            <dnn:label id="labelOrder" runat="server" controlname="labelOrder" suffix=":"></dnn:label>
                        </td>
                        <td class="Normal" nowrap="nowrap">
                            <asp:TextBox ID="txtOrder" runat="server" Width="35" MaxLength="50" CssClass="NormalTextBox"></asp:TextBox>
                            <asp:CompareValidator ID="validatorOrder" runat="server" ErrorMessage="Error! Please enter a valid order."
                                resourcekey="validatorOrder" Type="integer" ControlToValidate="txtOrder" Operator="DataTypeCheck"
                                Display="Dynamic"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="validatorRequireOrder" runat="server" ControlToValidate="txtOrder"
                                Display="Dynamic" ErrorMessage="* Order is required." resourcekey="validatorRequireOrder"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="NormalBold" nowrap="nowrap">
                            <dnn:label id="labelParentCategory" runat="server" controlname="labelParentCategory"
                                suffix=":"></dnn:label>
                        </td>
                        <td class="Normal" nowrap="nowrap">
                            <asp:DropDownList ID="ddlParentCategory" runat="server">
                            </asp:DropDownList>
                            <br />
                            <asp:Label ID="lblRecursionWarning" runat="server" Font-Bold="true" ForeColor="red"
                                resourcekey="lblRecursionWarning" Text="Recursive category relationship detected.  Please specify a <br/>different parent category."
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="NormalBold" nowrap="nowrap">
                            <dnn:label id="plProductTemplate" runat="server" controlname="plProductTemplate"
                                suffix=":"></dnn:label>
                        </td>
                        <td class="Normal" nowrap="nowrap">
                            <asp:DropDownList ID="ddlProductTemplate" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="NormalBold" nowrap="nowrap">
                            <dnn:label id="plListItemTemplate" runat="server" controlname="plListItemTemplate"
                                suffix=":"></dnn:label>
                        </td>
                        <td class="Normal" nowrap="nowrap">
                            <asp:DropDownList ID="ddlListItemTemplate" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="NormalBold" nowrap="nowrap">
                            <dnn:label id="plListAltItemTemplate" runat="server" controlname="plListAltItemTemplate"
                                suffix=":"></dnn:label>
                        </td>
                        <td class="Normal" nowrap="nowrap">
                            <asp:DropDownList ID="ddlListAltItemTemplate" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="NormalBold" nowrap="nowrap">
                            <dnn:label id="labelArchived" runat="server" controlname="labelArchived" suffix=":"></dnn:label>
                        </td>
                        <td class="Normal" nowrap="nowrap">
                            <asp:CheckBox ID="chkArchived" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="NormalBold" nowrap="nowrap">
                            <dnn:label id="labelHide" runat="server" controlname="labelHide" suffix=":"></dnn:label>
                        </td>
                        <td class="Normal" nowrap="nowrap">
                            <asp:CheckBox ID="chkHide" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="NBright_ButtonDiv">
                            <asp:FileUpload ID="cmdBrowse" runat="server" />
                                <asp:LinkButton ID="cmdAddImage" runat="server" CssClass="NBright_AddButton" Resourcekey="AddImage">Add</asp:LinkButton>
                                <asp:LinkButton ID="cmdRemove" runat="server" CssClass="NBright_CancelButton" Resourcekey="cmdRemove">Remove Image</asp:LinkButton>
                            </div>
                            <asp:Image ID="imgCat" runat="server" />
                        </td>
                    </tr>
                </table>
                <dnn:SectionHead ID="dshMsgBox" CssClass="Head" runat="server" Text="Message" Section="divMsgBox"
                    ResourceKey="labelMessage" IncludeRule="True" IsExpanded="False" />
                <div id="divMsgBox" runat="server">
                    <nwb:ShowSelectLang ID="ShowSelectLang2" runat="server"></nwb:ShowSelectLang>
                    <dnn:label id="labelMessage" runat="server" controlname="labelMessage" suffix=":"></dnn:label><br />
                    <dnn:TextEditor id="txtMessage" runat="server" width="100%" height="350"></dnn:TextEditor>
                </div>
                </div>
                <div id="tabs-2">
<div class="NBright_ButtonDiv">
<asp:LinkButton ID="cmdSelectProducts" runat="server" CssClass="NBright_CommandButton" resourcekey="cmdSelectProducts" Text="Select"></asp:LinkButton>&nbsp;
<asp:LinkButton ID="cmdClearProducts" runat="server" CssClass="NBright_CommandButton" resourcekey="cmdClearProducts" Text="Clear"></asp:LinkButton>&nbsp;
</div>
 <asp:DataGrid id="dgProducts" runat="server" AutoGenerateColumns="False" gridlines="None" cellpadding="2"  PageSize="20" Width="500" AllowPaging="False">
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
			<Columns>
			<asp:BoundColumn DataField="ProductID" HeaderText="ID" Visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="ProductRef" HeaderText="Ref"></asp:BoundColumn>
			<asp:BoundColumn DataField="ProductName" HeaderText="Name"></asp:BoundColumn>
				<dnn:ImageCommandColumn KeyField="ProductID" ShowImage="True" ImageURL="~/images/delete.gif" CommandName="Remove"
					EditMode="Command" HeaderText="">
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate></ItemTemplate>
				</dnn:ImageCommandColumn>
			</Columns>
			<PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
		</asp:DataGrid>		
		<br />
            <nbs:AdminPagingControl id="ctlPagingControl" runat="server" pagesize="20" BorderWidth="0"></nbs:AdminPagingControl>
                </div>
            </asp:Panel>            
            
        </td>
    </tr>
</table>
            </asp:Panel>
</td></tr>
</table>
