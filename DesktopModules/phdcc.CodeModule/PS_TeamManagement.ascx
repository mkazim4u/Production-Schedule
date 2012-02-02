<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_TeamManagement.ascx.vb"
    Inherits="PS_TeamManagement" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="DNN" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls"
    Assembly="DotNetNuke" %>
<link href="module.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    div.radtooltip_Default
    {
        border: solid 1px Red !important;
    }
</style>
<telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
    <script type="text/javascript">





        //indicates whether the user is currently dragging a listbox item
        var listBoxDragInProgress = false;

        //indicates whether the user is currently dragging a tree node
        var treeViewDragInProgress = false;

        //select the hovered listbox item if the user is dragging a node
        function onListBoxMouseOver(sender, args) {
            if (listBoxDragInProgress) {
                args.get_item().select();
            }

            //            var item = args.get_element();

            //            alert(item + "1")
            //            if (item != null) {
            //                
            //                //var node = nodeElem;
            //                alert(item + "2")
            //                var tooltipManager = $find("<%= RadToolTipManager.ClientID%>");

            //                //If the user hovers the image before the page has loaded, there is no manager created
            //                if (!tooltipManager) return;

            //                //Find the tooltip for this element if it has been created 
            //                var tooltip = tooltipManager.getToolTipByElement(item);

            //Create a tooltip if no tooltip exists for such element
            //                if (!tooltip) {
            //                    alert(tooltipManager)
            //                    tooltip = tooltipManager.createToolTip(item);
            //                    alert(tooltip)
            //                    //tooltip.set_value(item.get_item().get_value());
            //                    tooltip.show();
            //                }
            //            }






        }

        //select the hovered tree node if the user is dragging a listbox item
        function onTreeViewMouseOver(sender, args) {
            if (listBoxDragInProgress) {
                args.get_node().select();
            }

            ////////////////  radtooltip code for move over ///////////////
            var nodeElem = args.get_node();
            if (nodeElem.get_level() != 0) {
                var node = nodeElem.get_textElement();

                //                var tooltipManager = $find("<%= RadToolTipManager.ClientID%>");

                //                //If the user hovers the image before the page has loaded, there is no manager created
                //                if (!tooltipManager) return;

                //                //Find the tooltip for this element if it has been created 
                //                var tooltip = tooltipManager.getToolTipByElement(node);

                //                //Create a tooltip if no tooltip exists for such element
                //                if (!tooltip) {

                //                    
                //                    tooltip = tooltipManager.createToolTip(node);
                //                    tooltip.set_value(nodeElem.get_text());
                //                    tooltip.show();
                //                }
            }


        }

        //unselect the item if the user is dragging a node
        function onListBoxMouseOut(sender, args) {
            if (treeViewDragInProgress) {
                args.get_item().unselect();
            }
        }

        //unselect the node if the user is dragging a listbox item
        function onTreeViewMouseOut(sender, args) {
            if (listBoxDragInProgress) {
                args.get_node().unselect();
            }
        }

        //indicate that the user started dragging a listbox item
        function onListBoxDragStart(sender, args) {
            listBoxDragInProgress = true;
        }

        //indicate that the user started dragging a tree node
        function onTreeViewDragStart(sender, args) {
            treeViewDragInProgress = true;
        }

        //handle the drop of the listbox item
        function onListBoxDropping(sender, args) {
            //indicate that the user stopped dragging
            listBoxDragInProgress = false;
            document.body.style.cursor = "";
            //restore the cursor to the default state
            document.body.style.cursor = "";

            //get the html element on which the item is dropped
            var target = args.get_htmlElement();

            //if dropped on the listbox itself return.
            if (isOverElement(target, "<%= lstUsers.ClientID %>")) {
                return;
            }
            //check if dropped on the treeview
            var target = isOverElement(target, "<%= tvTeams.ClientID %>");

            //if not cancel the dropping event so it does not postback
            if (!target) {
                args.set_cancel(true);
                return;
            }

            //the item was dropped on the treeview - set the htmlElement. 
            //it can be later accessed via the HtmlElementID property of the RadListBox
            args.set_htmlElement(target);
        }

        //handle the drop of the tree node
        function onTreeViewDropping(sender, args) {
            //indicate that the user stopped dragging
            treeViewDragInProgress = false;

            //restore the cursor to the default state
            document.body.style.cursor = "";

            //get the html element on which the node is dropped
            var target = args.get_htmlElement();

            //if dropped on the treeview itself return.
            if (isOverElement(target, "<%= tvTeams.ClientID %>")) {
                return;
            }
            //check if dropped on the listbox
            var target = isOverElement(target, "<%= lstUsers.ClientID %>");

            //if not cancel the dropping event so it does not postback
            if (!target) {
                args.set_cancel(true);
                return;
            }

            //the node was dropped on the listbox - set the htmlElement. 
            //it can be later accessed via the HtmlElementID property of the RadTreeNodeDragDropEventArgs
            args.set_htmlElement(target);
        }

        //chech if a given html element is a child of an element with the specified id
        function isOverElement(target, id) {
            while (target) {
                if (target.id == id)
                    break;

                target = target.parentNode;
            }
            return target;
        }

        function checkDropTargets(target) {
            if (isOverElement(target, "<%= lstUsers.ClientID %>") || isOverElement(target, "<%= tvTeams.ClientID %>")) {
                //if the mouse is over the treeview or listbox - set the cursor to default
                document.body.style.cursor = "";
            } else {
                //else set the cursor to "no-drop" to indicate that dropping over this html element is not supported
                document.body.style.cursor = "no-drop";
            }
        }

        //update the cursor if the user is dragging the item over supported drop target - listbox or treeview
        function onListBoxDragging(sender, args) {
            checkDropTargets(args.get_htmlElement());
        }

        //update the cursor if the user is dragging the node over supported drop target - listbox or treeview
        function onTreeViewDragging(sender, args) {
            checkDropTargets(args.get_htmlElement());
        }


        function CloseActiveToolTip() {
            var tooltip = Telerik.Web.UI.RadToolTip.getCurrent();
            if (tooltip) tooltip.hide();
        }



        function AdjustSize(sender, args) {
            sender.autoSize(true);
        }

    </script>
    <script type="text/javascript" language="javascript">

        function Reset() {


            document.getElementById("<%= tbTeamName.ClientID %>").value = "";
            document.getElementById("<%= tbTeamDescription.ClientID %>").value = "";
            document.getElementById("divValidationSummary").style.visibility = 'hidden';
        }

        function ShowValidationDiv() {

            document.getElementById("divValidationSummary").style.display = 'block';
            document.getElementById("divValidationSummary").style.visibility = 'visible';
        }


       
    </script>
</telerik:RadCodeBlock>
<fieldset id="fsMain" runat="server">
    <legend class="fieldsetLegend" id="fslgndMain">
        <asp:Label ID="lblUserJobMapping" runat="server" Text="Manage Teams" />
    </legend>
    <telerik:RadAjaxLoadingPanel ID="alpUserJobMapping" runat="server" Height="75px"
        MinDisplayTime="5" Width="75px">
        <asp:Image ID="Image1" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="rapUserRoleAssignment" RequestQueueSize="5" runat="server"
        Width="100%" EnableOutsideScripts="True" HorizontalAlign="NotSet" ScrollBars="None"
        LoadingPanelID="alpUserJobMapping">
        <table id="head" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <table id="tblAddTeam" border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <DNN:SectionHead runat="server" ID="shAddTeam" IsExpanded="false" Section="divCreateTeam"
                                    Text="Create/Delete Team" IncludeRule="true" Visible="true" />
                                <div id="divValidationSummary">
                                    <asp:ValidationSummary ID="vs" ValidationGroup="vg" runat="server" ForeColor="Red" />
                                    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" />
                                </div>
                                <div id="divCreateTeam" runat="server">
                                    <br />
                                    <asp:Label ID="lblTeamName" runat="server" Text="Team Name" />
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="tbTeamName" Text="" runat="server" MaxLength="25" CssClass="fieldsetControlWidth"
                                        ValidationGroup="vg" />
                                    &nbsp;
                                    <asp:RequiredFieldValidator Display="None" ID="rfvTeamName" runat="server" ErrorMessage="Please Enter Team Name"
                                        ControlToValidate="tbTeamName" ValidationGroup="vg" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <br />
                                    <asp:Label ID="lblTeamDescription" runat="server" Text="Description" MaxLength="50" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="tbTeamDescription" Text="" runat="server" CssClass="fieldsetControlWidth"
                                        MaxLength="25" />
                                    &nbsp;
                                    <%--                                    <asp:RequiredFieldValidator Display="None" ID="rfvTeamDescription" runat="server"
                                        ErrorMessage="Please Enter Team Description" ControlToValidate="tbTeamDescription"
                                        ValidationGroup="vg" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    --%>
                                    <br />
                                    <br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="vg"
                                        OnClientClick="ShowValidationDiv();" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="Clear" CausesValidation="false" OnClientClick="return Reset();" />
                                    <br />
                                    <br />
                                    <asp:GridView ID="gvTeam" runat="server" CellPadding="2" AutoGenerateColumns="false"
                                        AllowSorting="true" Width="100%" AllowPaging="true" CssClass="gridviewSpacing gvTeam">
                                        <Columns>
                                            <asp:TemplateField ShowHeader="false">
                                                <HeaderStyle Font-Bold="true" />
                                                <ItemStyle Font-Bold="false" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnEditTeam" runat="server" OnClick="lnkbtnEditTeam_Click"
                                                        ToolTip="Edit" CommandArgument='<%# Container.DataItemIndex %>' CommandName="edit">
                                                        <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Portals/0/Images/Edit.gif" /></asp:LinkButton>
                                                    &nbsp;<asp:LinkButton ID="lnkbtnRemoveTeam" runat="server" OnClick="lnkbtnRemoveTeam_Click"
                                                        ToolTip="Delete" CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete this team ?');">
                                                        <asp:Image ID="imgDelete" runat="server" ImageUrl="~/Portals/0/Images/delete.png" />
                                                    </asp:LinkButton>
                                                    <asp:HiddenField ID="hidTeamID" runat="server" Value='<%# Container.DataItem("ID")%>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                        ToolTip="Update" Text="">
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
                                                    &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                        ToolTip="Cancel" Text="">
                                                        <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" />
                                                    </asp:LinkButton>
                                                    <asp:HiddenField ID="hidTeamID" runat="server" Value='<%# Container.DataItem("ID")%>' />
                                                    <asp:HiddenField ID="hidTeamName" runat="server" Value='<%# Container.DataItem("TeamName")%>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnTeamName" runat="server" OnClick="lnkbtnTeamName_Click">Team Name</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTeamName" runat="server" Text='<%# Bind("TeamName") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtTeamName" runat="server" Text='<%# Bind("TeamName") %>'
                                                        ValidationGroup="vg" MaxLength="50" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField HeaderText="Team Name" DataField="RoleName" Visible="true" ReadOnly="true" />--%>
                                            <asp:TemplateField HeaderText="Description">
                                                <HeaderStyle Font-Bold="true" />
                                                <ItemStyle Font-Bold="false" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTeamDescription" Font-Bold="true" runat="server" Text='<%# Bind("TeamDescription") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="tbTeamDescription" runat="server" Text='<%# Bind("TeamDescription") %>'
                                                        ValidationGroup="vg" MaxLength="50" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Created On" SortExpression="CreatedOn">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkbtnCreatedColumn" runat="server" OnClick="lnkbtnCreatedColumn_Click">Created</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreatedOn" runat="server" Text='<%# FF_Globals.IsValidDate(Eval("CreatedOn")) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--                                            <asp:BoundField HeaderText="Created On" DataField="CreatedOnDate" Visible="true"
                                                ReadOnly="true" DataFormatString="{0:d-MMM-yyyy}" />
                                            --%>
                                            <asp:BoundField HeaderText="Created By" DataField="UserName" Visible="true" ReadOnly="true" />
                                            <%--<asp:Label ID="Created" Font-Bold="true" runat="server" Text='<%# Bind("CreatedOnDate") %>' />--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <DNN:SectionHead runat="server" ID="shTeamManagement" IsExpanded="false" Section="divTeamManagement"
                        Text="Users / Teams" IncludeRule="true" Visible="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divTeamManagement" runat="server">
                        <table id="tblUserTeamManagement" border="0" cellpadding="0" cellspacing="0" width="100%">
                            <%--                        <tr>
                            <td width="20%">
                                <asp:CheckBox runat="server" ID="chkAllPSUsers" Text="Select All Users" AutoPostBack="true"
                                    OnCheckedChanged="chkAllPSUsers_OnCheckedChanged" />
                            </td>
                        </tr>--%>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="30%">
                                                <asp:Label ID="lblUser" runat="server" Text="Users"></asp:Label>
                                            </td>
                                            <td width="20%">
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="Teams"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadToolTipManager HideDelay="1" EnableViewState="false" RelativeTo="Element"
                                        ID="RadToolTipManager" runat="server" EnableShadow="true" OffsetX="15" HideEvent="LeaveToolTip"
                                        BackColor="#ffff66" Position="TopRight" OnAjaxUpdate="RadToolTipmanager_AjaxUpdate"
                                        Style="font-size: 18px; text-align: center; font-family: Arial;" RenderInPageRoot="true">
                                    </telerik:RadToolTipManager>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="20%">
                                                <telerik:RadListBox runat="server" ID="lstUsers" Sort="Ascending" EnableDragAndDrop="True"
                                                    Width="160px" AllowReorder="true" Height="200px" OnClientDropping="onListBoxDropping"
                                                    OnClientDragStart="onListBoxDragStart" OnClientMouseOver="onListBoxMouseOver"
                                                    BorderColor="#ffff66" BackColor="#ffff66" OnClientMouseOut="onListBoxMouseOut"
                                                    OnClientDragging="onListBoxDragging" OnDropped="lstUsers_ItemDropped">
                                                    <ButtonSettings ShowReorder="false" />
                                                </telerik:RadListBox>
                                            </td>
                                            <td width="20%">
                                                <div style="height: 200px; overflow: auto; width: 100%;">
                                                    <telerik:RadTreeView runat="server" ID="tvTeams" EnableDragAndDrop="True" OnClientMouseOver="onTreeViewMouseOver"
                                                        OnClientMouseOut="onTreeViewMouseOut" OnClientNodeDragStart="onTreeViewDragStart"
                                                        OnClientNodeDropping="onTreeViewDropping" OnNodeDrop="tvTeams_NodeDrop" OnClientNodeDragging="onTreeViewDragging"
                                                        OnContextMenuItemClick="tvTeams_ContextMenuItemClick">
                                                        <ContextMenus>
                                                            <telerik:RadTreeViewContextMenu runat="server" ID="HelpDeskMenu" ClickToOpen="True"
                                                                Skin="Vista">
                                                                <Items>
                                                                    <telerik:RadMenuItem Text="Remove From Team" Value="Remove">
                                                                    </telerik:RadMenuItem>
                                                                    <telerik:RadMenuItem Text="Remove From All Teams" Value="Remove All">
                                                                    </telerik:RadMenuItem>
                                                                </Items>
                                                            </telerik:RadTreeViewContextMenu>
                                                        </ContextMenus>
                                                    </telerik:RadTreeView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <%--            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                </td>
            </tr>--%>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
</fieldset>
