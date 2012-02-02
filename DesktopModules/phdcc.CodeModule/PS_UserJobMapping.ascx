<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_UserJobMapping.ascx.vb"
    Inherits="PS_UserJobMapping" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls"
    Assembly="DotNetNuke" %>
<link href="module.css" rel="stylesheet" type="text/css" />
<telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
    <script type="text/javascript">



        //indicates whether the user is currently dragging a listbox item
        var listBoxDragInProgress = false;

        //indicates whether the user is currently dragging a tree node
        var treeViewDragInProgress = false;

        //select the hovered listbox item if the user is dragging a node
        function onListBoxMouseOver(sender, args) {
            if (treeViewDragInProgress) {
                args.get_item().select();
            }
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

                var tooltipManager = $find("<%= RadToolTipManager.ClientID%>");

                //If the user hovers the image before the page has loaded, there is no manager created
                if (!tooltipManager) return;

                //Find the tooltip for this element if it has been created 
                var tooltip = tooltipManager.getToolTipByElement(node);

                //Create a tooltip if no tooltip exists for such element
                if (!tooltip) {
                    tooltip = tooltipManager.createToolTip(node);
                    tooltip.set_value(nodeElem.get_text());
                    tooltip.show();
                }
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
            var target = isOverElement(target, "<%= tvUsersInJobStages.ClientID %>");

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
            if (isOverElement(target, "<%= tvUsersInJobStages.ClientID %>")) {
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
            if (isOverElement(target, "<%= lstUsers.ClientID %>") || isOverElement(target, "<%= tvUsersInJobStages.ClientID %>")) {
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



    </script>
</telerik:RadCodeBlock>
<fieldset id="fsMain" runat="server">
    <legend class="fieldsetLegend" id="fslgndMain">
        <asp:Label ID="lblUserJobMapping" runat="server" Text="Resource Management" />
    </legend>
<%--    <telerik:RadAjaxLoadingPanel ID="alpUserJobMapping" runat="server" Height="75px"
        MinDisplayTime="5" Width="75px">
        <asp:Image ID="Image1" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="rapUserJobMapping" RequestQueueSize="5" runat="server"
        Width="100%" EnableOutsideScripts="True" HorizontalAlign="NotSet" ScrollBars="None"
        LoadingPanelID="alpUserJobMapping">--%>
        <table id="sub1" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr valign="top">
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="30%">
                                <telerik:RadComboBox ID="cmbRoleName" EmptyMessage="- please select team -" runat="server"
                                    AutoCompleteSeparator="true" HighlightTemplatedItems="true" AllowCustomText="true">
                                    <%--                                    <Items>                                        
                                        <telerik:RadComboBoxItem Value="0" Text="-- Select All Users --">
                                    </Items>
                                    --%>
                                    <ItemTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkRole" AutoPostBack="true" Text=""  OnCheckedChanged="chkRole_OnCheckedChanged" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                            </td>
                            <td width="20%">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <%--                        <tr>
                            <td width="20%">
                                <asp:CheckBox runat="server" ID="chkAllPSUsers" Text="Select All PS Users" AutoPostBack="true"
                                    OnCheckedChanged="chkAllPSUsers_OnCheckedChanged" />
                            </td>
                        </tr>
                        --%>
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
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="30%">
                                <asp:Label ID="lblUser" runat="server" Text="Resources"></asp:Label>
                            </td>
                            <td width="20%">
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Stages"></asp:Label>
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
                                <telerik:RadListBox runat="server" ID="lstUsers" Sort="Ascending" EnableDragAndDrop="True" skinn="Vista"
                                    Width="160px" AllowReorder="true" Height="200px" OnClientDropping="onListBoxDropping"
                                    OnClientDragStart="onListBoxDragStart" OnClientMouseOver="onListBoxMouseOver"
                                    BorderColor="#ffff66" BackColor="#ffff66" OnClientMouseOut="onListBoxMouseOut"
                                    OnClientDragging="onListBoxDragging" OnDropped="lstUsers_ItemDropped">
                                    <ButtonSettings ShowReorder="false" />
                                </telerik:RadListBox>
                            </td>
                            <td width="20%">
                                <div style="height: 200px; overflow: auto; width: 100%;">
                                    <telerik:RadTreeView runat="server" ID="tvUsersInJobStages" EnableDragAndDrop="True"
                                        OnClientMouseOver="onTreeViewMouseOver" OnClientMouseOut="onTreeViewMouseOut"
                                        OnClientNodeDragStart="onTreeViewDragStart" OnClientNodeDropping="onTreeViewDropping"
                                        OnNodeDrop="tvUsersInJobStages_NodeDrop" OnClientNodeDragging="onTreeViewDragging"
                                        OnContextMenuItemClick="tvUsersInJobStages_ContextMenuItemClick">
                                        <ContextMenus>
                                            <telerik:RadTreeViewContextMenu runat="server" ID="HelpDeskMenu" ClickToOpen="True"
                                                Skin="Vista">
                                                <Items>
                                                    <telerik:RadMenuItem Text="Remove From Stage" Value="Remove">
                                                    </telerik:RadMenuItem>
                                                    <telerik:RadMenuItem Text="Remove From All Stages" Value="Remove All">
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
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                </td>
            </tr>
        </table>
    <%--</telerik:RadAjaxPanel>--%>
</fieldset>
