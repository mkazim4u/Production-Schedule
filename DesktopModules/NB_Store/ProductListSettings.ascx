<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProductListSettings.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.ProductListSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="nbs" TagName="CatMenuSettings" Src="CategoryMenuSettings.ascx"%>
<dnn:SectionHead ID="dshBasic" CssClass="Head" runat="server" Text="Basic Settings" Section="tblBasic" ResourceKey="BasicSettings" IncludeRule="True" IsExpanded="True" />
<table width="500" border="0" align="center" cellSpacing="5" id="tblBasic" runat="server">
    <tr>
		<TD width="150px"><dnn:label id="plDefaultCategory" runat="server" controlname="plDefaultCategory" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlDefaultCategory" runat="server"></asp:DropDownList>
	    </td>
	</tr>
	<tr>
        <td>
            <dnn:label id="plModuleTitle" runat="server" controlname="plModuleTitle" suffix=":"></dnn:label>
        </td>
        <td>        
            <asp:DropDownList id="lstModuleTitle" runat="server">
                <asp:ListItem Value="0">Default</asp:ListItem>
                <asp:ListItem Value="1">Product Title</asp:ListItem>
                <asp:ListItem Value="2">Category Name</asp:ListItem>
                <asp:ListItem Value="3">Product Title / Category Name</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
	<tr>
        <td>
            <dnn:label id="plBrowseMode" runat="server" controlname="plBrowseMode" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:RadioButtonList ID="lstBrowseMode" runat="server">
                <asp:ListItem Value="0">Enabled</asp:ListItem>
                <asp:ListItem Value="1">Disabled</asp:ListItem>
                <asp:ListItem Value="2">Featured Only, Browsing Enabled</asp:ListItem>
                <asp:ListItem Value="3">Featured Only, Browsing Disabled</asp:ListItem>
            </asp:RadioButtonList><!-- asp:ListItem Value="4">Initially Featured, Browse all Enabled</asp:ListItem-->
        </td>
    </tr>
    <tr>
		<td><dnn:label id="plExcludeFeatured" runat="server" controlname="plExcludeFeatured" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:CheckBox ID="chkExcludeFeatured" runat="server" />
	    </td>
	</tr>

    <tr>
		<td><dnn:label id="plExcludeProduct" runat="server" controlname="plExcludeProduct" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:CheckBox ID="chkExcludeProduct" runat="server" />
	    </td>
	</tr>

    <tr>
		<td><dnn:label id="plPageSize" runat="server" controlname="plPageSize" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:TextBox ID="txtPageSize" runat="server"></asp:TextBox>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plColumns" runat="server" controlname="plColumns" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:TextBox ID="txtColumns" runat="server"></asp:TextBox>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plLayout" runat="server" controlname="plLayout" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlLayout" runat="server">
                <asp:ListItem Selected="True" Value="0">Table</asp:ListItem>
                <asp:ListItem Value="1">Flow</asp:ListItem>
            </asp:DropDownList>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plDirection" runat="server" controlname="plDirection" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlDirection" runat="server">
                <asp:ListItem Selected="True" Value="0">Horizontal</asp:ListItem>
                <asp:ListItem Value="1">Vertical</asp:ListItem>
            </asp:DropDownList>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plDefaultOrder" runat="server" controlname="plDefaultOrder" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlDefaultOrder" runat="server">
                <asp:ListItem Value="man">Manufacturer</asp:ListItem>
                <asp:ListItem Value="name">Name</asp:ListItem>
                <asp:ListItem Value="ref">Ref</asp:ListItem>
                <asp:ListItem Value="price">Price</asp:ListItem>
                <asp:ListItem Value="cdate">Created Date</asp:ListItem>
            </asp:DropDownList>
	    </td>
	</tr>
    <tr>
        <td>
            <dnn:label id="plDefaultOrderDESC" runat="server" controlname="plDefaultOrderDESC" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkDefaultOrderDESC" runat="server" />
        </td>
    </tr>	
    <tr>
        <td>
            <dnn:label id="plCascadeResults" runat="server" controlname="plCascadeResults" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkCascadeResults" runat="server" />
        </td>
    </tr>	
</table>
<dnn:SectionHead ID="dshTemplates" CssClass="Head" runat="server" Text="Templates" Section="tblTemplates" ResourceKey="TemplateSettings" IncludeRule="True" IsExpanded="False" />
<table width="500" border="0" align="center" cellSpacing="5" id="tblTemplates" runat="server">
    <tr>
		<td><dnn:label id="plTemplate" runat="server" controlname="plTemplate" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlTemplate" runat="server">
            </asp:DropDownList>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plAlterTemplate" runat="server" controlname="plAlterTemplate" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlAlterTemplate" runat="server">
            </asp:DropDownList>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plDetailTemplate" runat="server" controlname="plDetailTemplate" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlDetailTemplate" runat="server">
            </asp:DropDownList>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plStaticTemplates" runat="server" controlname="plStaticTemplates" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:CheckBox ID="chkStaticTemplates" runat="server" />
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plStaticListView" runat="server" controlname="plStaticListView" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:CheckBox ID="chkStaticListView" runat="server" />
	    </td>
	</tr>
	
	    <tr>
		<td><dnn:label id="plHeaderText" runat="server"  suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlHeaderText" runat="server">
            </asp:DropDownList>
	    </td>
	</tr>
    <tr>
		<td><dnn:label id="plListHeaderText" runat="server" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlListHeaderText" runat="server">
            </asp:DropDownList>
	    </td>
	</tr>
    <tr>
		<td><dnn:label id="plDetailHeaderText" runat="server" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlDetailHeaderText" runat="server">
            </asp:DropDownList>
	    </td>
	</tr>

	
</table>
<dnn:SectionHead ID="dshTabs" CssClass="Head" runat="server" Text="Tabs / Pages" Section="tblTabs" ResourceKey="PageSettings" IncludeRule="True" IsExpanded="False"  />
<table width="500" border="0" align="center" cellSpacing="5" id="tblTabs" runat="server">
    <tr>
        <td>
            <dnn:label id="plRedirectToCart" runat="server" controlname="plRedirectToCart" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkRedirectToCart" runat="server" />
        </td>
    </tr>	
	<tr>
		<td>
			<dnn:label id="plTabList" runat="server" controlname="lstTabs" suffix=":"></dnn:label>
		</td>
		<td>
		<asp:dropdownlist id="lstTabs" runat="server" AutoPostBack="True"></asp:dropdownlist>
		</td>
	</tr>
    <tr>
        <td>
            <dnn:label id="plchkTabDefaultRedirect" runat="server" controlname="plchkTabDefaultRedirect" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkTabDefaultRedirect" runat="server" />
        </td>
    </tr>	
	<tr>
		<td>
			<dnn:label id="plTabDefaultRedirect" runat="server" controlname="ddlTabDefaultRedirect" suffix=":"></dnn:label>
		</td>
		<td>
		<asp:dropdownlist id="ddlTabDefaultRedirect" runat="server" AutoPostBack="True"></asp:dropdownlist>
		</td>
	</tr>
	<tr>
		<td>
			<dnn:label id="plProductTabList" runat="server" controlname="lstProductTabs" suffix=":"></dnn:label>
		</td>
		<td>
		<asp:dropdownlist id="lstProductTabs" runat="server" AutoPostBack="True"></asp:dropdownlist>
		</td>
	</tr>
</table>
<dnn:SectionHead ID="dshOther" CssClass="Head" runat="server" Text="Other Settings" Section="Table2" ResourceKey="BasicSettings" IncludeRule="True" IsExpanded="False"  />
<table width="500" border="0" align="center" cellSpacing="5" id="Table2" runat="server">
	<tr>
		<td><dnn:label id="plThumbnailSize" runat="server" controlname="plThumbnailSize" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:TextBox ID="txtThumbnailSize" runat="server"></asp:TextBox>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plDetailThumbnailSize" runat="server" controlname="plDetailThumbnailSize" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:TextBox ID="txtDetailThumbnailSize" runat="server"></asp:TextBox>
	    </td>
	</tr>
	<tr>
		<td><dnn:label id="plGalleryThumbnailSize" runat="server" controlname="plGalleryThumbnailSize" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:TextBox ID="txtGalleryThumbnailSize" runat="server"></asp:TextBox>
	    </td>
	</tr>
    <tr>
        <td>
            <dnn:label id="plColWidth" runat="server" controlname="plColWidth" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtColWidth" runat="server" Width="200px"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td>
            <dnn:label id="plItemStyleCssClass" runat="server" controlname="plItemStyleCssClass" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtItemStyleCssClass" runat="server" Width="200px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plAlternatingItemStyleCssClass" runat="server" controlname="plAlternatingItemStyleCssClass" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtAlternatingItemStyleCssClass" runat="server" Width="200px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plCellPadding" runat="server" controlname="plCellPadding" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtCellPadding" runat="server" Width="40px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plCellSpacing" runat="server" controlname="plCellSpacing" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtCellSpacing" runat="server" Width="40px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plItemStyleHorizontalAlign" runat="server" controlname="plItemStyleHorizontalAlign" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:DropDownList ID="ddlItemStyleHorizontalAlign" runat="server">
                <asp:ListItem Value=""></asp:ListItem>
                <asp:ListItem Value="0">NotSet</asp:ListItem>
                <asp:ListItem Value="2">Centre</asp:ListItem>
                <asp:ListItem Value="4">Justify</asp:ListItem>
                <asp:ListItem Value="1">Left</asp:ListItem>
                <asp:ListItem Value="3">Right</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plItemStyleHeight" runat="server" controlname="plItemStyleHeight" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtItemStyleHeight" runat="server" Width="40px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plTableWidth" runat="server" controlname="plTableWidth" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtTableWidth" runat="server" Width="40px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plCssClass" runat="server" controlname="plCssClass" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtCssClass" runat="server" Width="200px"></asp:TextBox>
        </td>
    </tr>


	<tr>
		<td><dnn:label id="plCssBuyButton" runat="server" controlname="plCssBuyButton" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:TextBox ID="txtCssBuyButton" runat="server"></asp:TextBox>
	    </td>
	</tr>
	<tr>
		<td>
			<dnn:label id="plCategoryMessage" runat="server" controlname="plCategoryMessage" suffix=":"></dnn:label>
		</td>
		<td>
		    <asp:RadioButtonList ID="rblCategoryMessage" runat="server">
                <asp:ListItem Value="1">Never</asp:ListItem>
                <asp:ListItem Value="2">Always</asp:ListItem>
                <asp:ListItem Value="3">Empty</asp:ListItem>
            </asp:RadioButtonList>
		</td>
	</tr>
    <tr>
        <td>
            <dnn:label id="plSep1" runat="server" controlname="plSep" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSep" runat="server" Width="200px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plIncrementCart" runat="server" controlname="plIncrementCart" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkIncrementCart" runat="server" />
        </td>
    </tr>	
    <tr>
        <td>
            <dnn:label id="plIndexProducts" runat="server" controlname="plIndexProducts" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkIndexProducts" runat="server" />
        </td>
    </tr>	
    <tr>
        <td>
            <dnn:label id="plSkipList" runat="server" controlname="plSkipList" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkSkipList" runat="server" />
        </td>
    </tr>	
    <tr>
        <td>
            <dnn:label id="plReturnLimit" runat="server" controlname="plReturnLimit" suffix=":"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtReturnLimit" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
    </tr>
</table>
<dnn:SectionHead ID="dshSubMenu" CssClass="Head" runat="server" Text="Sub Menu Settings" Section="Table3" ResourceKey="SubMenuSettings" IncludeRule="True" IsExpanded="False"  />
<table width="500" border="0" align="center" cellSpacing="5" id="Table3" runat="server">
    <tr>
        <td>
            <dnn:label id="plShowSubMenu" runat="server" controlname="plHideSubMenu" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowSubMenu" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSubColumns" runat="server" controlname="plColumns" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSubMenuCols" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
	<tr>
		<td><dnn:label id="plSubThumbnailSize" runat="server" controlname="plSubThumbnailSize" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:TextBox ID="txtSubThumbnailSize" runat="server"></asp:TextBox>
	    </td>
	</tr> 
    <tr>
        <td>
            <dnn:label id="plCSS" runat="server" controlname="plCSS" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtCSS" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSubSelectCSS" runat="server" controlname="plSubSelectCSS" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSubSelectCSS" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSep" runat="server" controlname="plSep" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSubMenuSep" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSubLeftHtml" runat="server" controlname="plSubLeftHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSubLeftHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSubNameTemplate" runat="server" controlname="plSubNameTemplate"
                suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSubNameTemplate" runat="server" Width="300px" Height="100" TextMode="MultiLine"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSubRightHtml" runat="server" controlname="plSubLeftHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSubRightHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSubHeadHtml" runat="server" controlname="plSubHeadHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSubHeadHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
</table>
