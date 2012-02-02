<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_UserRoleMapping.ascx.vb"
    Inherits="PS_UserRoleMapping" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls"
    Assembly="DotNetNuke" %>
<link href="~/module.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    </style>

<script type="text/javascript">
             function selectallcheckbox(ischecked)
            {
            
                       
                for(i = 0; i < document.forms[0].elements.length; i++)
                {

                    elm = document.forms[0].elements[i]
                      
                    if (elm.type == 'checkbox')
                    {
                            elm.checked=ischecked;
                    }
                }

                
            }
</script>

<telerik:RadAjaxLoadingPanel ID="alpUserRoleMapping" runat="server" Height="75px"
    MinDisplayTime="5" Width="75px">
    <asp:Image ID="Image1" runat="server" AlternateText="Loading..." ImageUrl="~/DesktopModules/images/Loading.gif" />
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel ID="rapUserRoleMapping" runat="server" Width="100%" EnableOutsideScripts="True"
    HorizontalAlign="NotSet" ScrollBars="None" LoadingPanelID="alpUserRoleMapping">
    <table id="sub1" border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td width="22%">
                <asp:Label ID="lblUser" runat="server" Text="Role Name"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label14" runat="server" Text=":"></asp:Label>
            </td>
            <td>
                <telerik:RadComboBox ID="cmbRoleName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbRoleName_SelectedIndexChanged">
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td width="22%">
                <asp:Label ID="Label1" runat="server" Text="User Name"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
            </td>
            <td>
                <telerik:RadComboBox ID="cmbUserName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbUserName_SelectedIndexChanged" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:RadioButton ID="rbHighPrivilege" runat="server" Width="100%" Text="High Privilege User">
                </asp:RadioButton>
                <asp:RadioButton ID="rbStandardUser" runat="server" Width="100%" Text="Standard User">
                </asp:RadioButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Width="100%" Text="Select Roles For The Specific Application User">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" width="5%">
                            <asp:CheckBox ID="chkRole" runat="server" onclick="selectallcheckbox(this.checked);" />
                        </td>
                        <td align="left" width="70%">
                            Name
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="height: 200px; overflow: auto; width: 100%;">
                    <asp:DataList ID="dlRoleAssignment" runat="server" CellPadding="4" Width="97%" OnItemDataBound="dlRoleAssignment_ItemDataBound">
                        <ItemTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left" width="5%">
                                        <asp:CheckBox ID="chkRole" runat="server" />
                                    </td>
                                    <td align="left" width="75%">
                                        <asp:Label ID="lblRoleID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.RoleId") %>'>
                                        </asp:Label>
                                        <asp:Label ID="lblRoleName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RoleName") %>'>
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <AlternatingItemStyle CssClass="Datalist_AlternateItem" />
                        <ItemStyle CssClass="Datalist_Item" />
                        <SelectedItemStyle CssClass="Datalist_SelectItem" />
                    </asp:DataList>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSave" Text="Save" runat="server" CommandName="Save" OnClick="btnSave_Click">
                </asp:Button>
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                    CommandName="Cancel"></asp:Button>
            </td>
        </tr>
    </table>
</telerik:RadAjaxPanel>
