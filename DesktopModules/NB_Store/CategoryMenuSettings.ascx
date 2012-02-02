<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CategoryMenuSettings.ascx.vb"
    Inherits="NEvoWeb.Modules.NB_Store.CategoryMenuSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table>
	<tr>
		<td width="150px"><dnn:label id="plDefaultCategory" runat="server" controlname="plDefaultCategory" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:DropDownList ID="ddlDefaultCategory" runat="server">
            </asp:DropDownList>
	    </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plStaticCategory" runat="server" controlname="plStaticCategory" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkStaticCategory" runat="server" />
        </td>
    </tr>
</table>
<hr />
<table>
    <tr>
        <td>
            <dnn:label id="plHideBreadCrumb" runat="server" controlname="plHideBreadCrumb" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideBreadCrumb" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plHideBreadCrumbRoot" runat="server" controlname="plHideBreadCrumbRoot"
                suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideBreadCrumbRoot" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plBreadCrumbCSS" runat="server" controlname="plBreadCrumbCSS" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtBreadCrumbCSS" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plBreadCrumbSep" runat="server" controlname="plBreadCrumbSep" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtBreadCrumbSep" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
</table>
<hr />
<table>
    <tr>
        <td>
            <dnn:label id="plHideRootMenu" runat="server" controlname="plHideRootMenu" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideRootMenu" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plPatchWork" runat="server" controlname="plPatchWork" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkPatchWork" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plRootCSS" runat="server" controlname="plRootCSS" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtRootCSS" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plRootSelectCSS" runat="server" controlname="plRootSelectCSS" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtRootSelectCSS" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plRootSep" runat="server" controlname="plRootSep" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtRootSep" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plRootLeftHtml" runat="server" controlname="plRootLeftHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtRootLeftHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plRootNameTemplate" runat="server" controlname="plRootNameTemplate"
                suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtRootNameTemplate" runat="server" Width="300px" Height="100" TextMode="MultiLine"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plRootRightHtml" runat="server" controlname="plRootLeftHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtRootRightHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plRootHeadHtml" runat="server" controlname="plRootHeadHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtRootHeadHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
</table>
<hr />
<table>
    <tr>
        <td>
            <dnn:label id="plHideSubMenu" runat="server" controlname="plHideSubMenu" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideSubMenu" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plHideWhenRoot" runat="server" controlname="plHideWhenRoot" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkHideWhenRoot" runat="server" />
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
            <asp:TextBox ID="txtSep" runat="server" Width="250px"></asp:TextBox>
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
<hr />
<table>
    <tr>
        <td>
            <dnn:label id="plShowTreeMenu" runat="server" controlname="plShowTreeMenu" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowTreeMenu" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plTreeCSS" runat="server" controlname="plTreeCSS" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtTreeCSS" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plTreeSelectCSS" runat="server" controlname="plTreeSelectCSS" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtTreeSelectCSS" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plTreeLeftHtml" runat="server" controlname="plTreeLeftHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtTreeLeftHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plTreeNameTemplate" runat="server" controlname="plTreeNameTemplate"
                suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtTreeNameTemplate" runat="server" Width="300px" Height="100" TextMode="MultiLine"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plTreeRightHtml" runat="server" controlname="plTreeLeftHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtTreeRightHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plTreeHeadHtml" runat="server" controlname="plTreeHeadHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtTreeHeadHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
</table>
<hr />
<table>
    <tr>
        <td>
            <dnn:label id="plShowAccordionMenu" runat="server" controlname="plShowAccordionMenu" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkShowAccordionMenu" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plAccordionLeftHtml" runat="server" controlname="plAccordionLeftHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtAccordionLeftHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plAccordionNameTemplate" runat="server" controlname="plAccordionNameTemplate"
                suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtAccordionNameTemplate" runat="server" Width="300px" Height="100" TextMode="MultiLine"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plAccordionRightHtml" runat="server" controlname="plAccordionLeftHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtAccordionRightHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plAccordionHeadHtml" runat="server" controlname="plAccordionHeadHtml" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtAccordionHeadHtml" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
</table>
<hr />
<table>
    <tr>
        <td>
            <dnn:label id="plSkipBlankCat" runat="server" controlname="plSkipBlankCat" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkSkipBlankCat" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plViewProdHide" runat="server" controlname="plViewProdHide" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkViewProdHide" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plColumns" runat="server" controlname="plColumns" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtColumns" runat="server"></asp:TextBox>
        </td>
    </tr>
	<tr>
		<td><dnn:label id="plThumbnailSize" runat="server" controlname="plThumbnailSize" suffix=":"></dnn:label>
	    </td>
		<td>
            <asp:TextBox ID="txtThumbnailSize" runat="server"></asp:TextBox>
	    </td>
	</tr>
    <tr>
        <td>
            <dnn:label id="plSectionSep" runat="server" controlname="plSectionSep" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSectionSep" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSectionSep2" runat="server" controlname="plSectionSep2" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSectionSep2" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plSectionSep3" runat="server" controlname="plSectionSep3" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtSectionSep3" runat="server" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:label id="plProductTabList" runat="server" controlname="lstProductTabs" suffix=":">
            </dnn:label>
        </td>
        <td>
            <asp:DropDownList ID="lstProductTabs" runat="server" AutoPostBack="True">
            </asp:DropDownList>
        </td>
    </tr>
</table>
