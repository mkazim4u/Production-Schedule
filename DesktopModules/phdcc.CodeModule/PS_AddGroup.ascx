﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_AddGroup.ascx.vb"
    Inherits="PS_AddGroup" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls"
    Assembly="DotNetNuke" %>

<script type="text/javascript">
    function RowDblClick(sender, eventArgs) {
        sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
    }
</script>
<link href="module.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    /*Telerik RadGrid WebBlue Skin*/
    
    /*global*/
    
    .lnkButton
    {
        float: right;
        display: block;
        padding-left: 5px;
        padding-right: 5px;
    }
    
    
    .RadGrid_WebBlue
    {
        border: 1px solid #768ca5;
        background: #fff;
        color: #000;
    }
    
    .RadGrid_WebBlue, .RadGrid_WebBlue .rgMasterTable, .RadGrid_WebBlue .rgDetailTable, .RadGrid_WebBlue .rgGroupPanel table, .RadGrid_WebBlue .rgCommandRow table, .RadGrid_WebBlue .rgEditForm table, .RadGrid_WebBlue .rgPager table, .GridToolTip_WebBlue
    {
        font: 12px/16px "segoe ui" ,arial,sans-serif;
    }
    
    .RadGrid_WebBlue .rgHeader:first-child, .RadGrid_WebBlue th.rgResizeCol:first-child, .RadGrid_WebBlue .rgFilterRow > td:first-child, .RadGrid_WebBlue .rgRow > td:first-child, .RadGrid_WebBlue .rgAltRow > td:first-child
    {
        border-left-width: 0;
        padding-left: 8px;
    }
    
    .RadGrid_WebBlue .rgAdd, .RadGrid_WebBlue .rgRefresh, .RadGrid_WebBlue .rgEdit, .RadGrid_WebBlue .rgDel, .RadGrid_WebBlue .rgFilter, .RadGrid_WebBlue .rgPagePrev, .RadGrid_WebBlue .rgPageNext, .RadGrid_WebBlue .rgPageFirst, .RadGrid_WebBlue .rgPageLast, .RadGrid_WebBlue .rgExpand, .RadGrid_WebBlue .rgCollapse, .RadGrid_WebBlue .rgSortAsc, .RadGrid_WebBlue .rgSortDesc, .RadGrid_WebBlue .rgUpdate, .RadGrid_WebBlue .rgCancel, .RadGrid_WebBlue .rgUngroup, .RadGrid_WebBlue .rgExpXLS, .RadGrid_WebBlue .rgExpDOC, .RadGrid_WebBlue .rgExpPDF, .RadGrid_WebBlue .rgExpCSV
    {
        background-image: url('~/Portals/0/Images/sprite.gif');
    }
    
    /*header*/
    
    .RadGrid_WebBlue .rgHeaderDiv
    {
        background: #dae2e8 0 -7050px repeat-x url('~/Portals/0/Images/sprite.gif');
    }
    .rgTwoLines .rgHeaderDiv
    {
        background-position: 0 -6550px;
    }
    
    .RadGrid_WebBlue .rgHeader, .RadGrid_WebBlue th.rgResizeCol
    {
        border: 1px solid;
        border-color: #98acbf #728ba1 #3d556c #455f77;
        border-top-width: 0;
        background: 0 -2300px repeat-x #718ca1 url('~/Portals/0/Images/sprite.gif');
    }
    
    .RadGrid_WebBlue th.rgSorted
    {
        border-color: #7c93a8 #758ea4 #334d65 #39556e;
        background-color: #5c7990;
        background-position: 0 -2600px;
    }
    
    .RadGrid_WebBlue .rgHeader, .RadGrid_WebBlue .rgHeader a
    {
        color: #fff;
    }
    
    /*rows*/
    
    .RadGrid_WebBlue .rgRow td, .RadGrid_WebBlue .rgAltRow td, .RadGrid_WebBlue .rgEditRow td, .RadGrid_WebBlue .rgFooter td
    {
        border-style: solid;
        border-width: 0 1px 1px;
    }
    
    .RadGrid_WebBlue .rgRow td
    {
        border-color: #fff #fff #fff #dae2e8;
    }
    
    .RadGrid_WebBlue .rgAltRow
    {
        background: #dae2e8;
    }
    
    .RadGrid_WebBlue .rgAltRow td
    {
        border-color: #dae2e8;
    }
    
    .RadGrid_WebBlue .rgRow .rgSorted
    {
        border-bottom-color: #f0f0f0;
        background-color: #f0f0f0;
    }
    
    .RadGrid_WebBlue .rgSelectedRow .rgSorted, .RadGrid_WebBlue .rgActiveRow .rgSorted, .RadGrid_WebBlue .rgHoveredRow .rgSorted, .RadGrid_WebBlue .rgEditRow .rgSorted
    {
        background-color: transparent;
    }
    
    .RadGrid_WebBlue .rgRow a, .RadGrid_WebBlue .rgAltRow a, .RadGrid_WebBlue .rgEditRow a, .RadGrid_WebBlue .rgFooter a, .RadGrid_WebBlue .rgEditForm a
    {
        color: #0e3d4f;
    }
    
    .RadGrid_WebBlue .rgSelectedRow
    {
        background: #7bbbcf 0 -3900px repeat-x url('~/Portals/0/Images/sprite.gif');
    }
    * + html .RadGrid_WebBlue .rgSelectedRow .rgSorted
    {
        background-color: #7bbbcf;
    }
    * html .RadGrid_WebBlue .rgSelectedRow .rgSorted
    {
        background-color: #7bbbcf;
    }
    
    .RadGrid_WebBlue .rgActiveRow, .RadGrid_WebBlue .rgHoveredRow
    {
        background: #bfe3f6 0 -2900px repeat-x url('~/Portals/0/Images/sprite.gif');
    }
    * + html .RadGrid_WebBlue .rgActiveRow .rgSorted, * + html .RadGrid_WebBlue .rgHoveredRow .rgSorted
    {
        background-color: #bfe3f6;
    }
    * html .RadGrid_WebBlue .rgActiveRow .rgSorted, * html .RadGrid_WebBlue .rgHoveredRow .rgSorted
    {
        background-color: #bfe3f6;
    }
    
    .RadGrid_WebBlue .rgEditRow
    {
        background: #fff 0 -4900px repeat-x url('~/Portals/0/Images/sprite.gif');
    }
    * + html .RadGrid_WebBlue .rgEditRow .rgSorted
    {
        background-color: #fff;
    }
    * html .RadGrid_WebBlue .rgEditRow .rgSorted
    {
        background-color: #fff;
    }
    
    .RadGrid_WebBlue .rgSelectedRow td, .RadGrid_WebBlue .rgActiveRow td, .RadGrid_WebBlue .rgHoveredRow td, .RadGrid_WebBlue .rgEditRow td
    {
        border-left-width: 0;
        border-right-width: 0;
        padding-left: 8px;
        padding-right: 8px;
    }
    
    .RadGrid_WebBlue .rgSelectedRow td, .RadGrid_WebBlue .rgSelectedRow td.rgSorted
    {
        border-bottom-color: #133c44;
    }
    
    .RadGrid_WebBlue .rgActiveRow td, .RadGrid_WebBlue .rgHoveredRow td, .RadGrid_WebBlue .rgActiveRow td.rgSorted, .RadGrid_WebBlue .rgHoveredRow td.rgSorted
    {
        border-bottom-color: #5d9fb7;
    }
    
    .RadGrid_WebBlue .rgEditRow td, .RadGrid_WebBlue .rgEditRow td.rgSorted
    {
        border-color: #fff #fff #768ca5 #fff;
    }
    
    .RadGrid_WebBlue .rgDrag
    {
        background-image: url('Grid/rgDrag.gif');
    }
    
    /*footer*/
    
    .RadGrid_WebBlue .rgFooterDiv, .RadGrid_WebBlue .rgFooter
    {
        background: #dae2e8;
    }
    
    .RadGrid_WebBlue .rgFooter td
    {
        border-top-width: 1px;
        border-color: #a2b3c7 #dae2e8 #fff #dae2e8;
    }
    
    /*status*/
    
    .RadGrid_WebBlue .rgPager .rgStatus
    {
        border: 1px solid;
        border-color: #a2b3c7 #9cb6c5 #fff #9cb6c5;
        border-left-width: 0;
    }
    
    .RadGrid_WebBlue .rgStatus div
    {
        background-image: url('Common/loading_small.gif');
    }
    
    /*pager*/
    
    .RadGrid_WebBlue .rgPager
    {
        background: #dae2e8;
    }
    
    .RadGrid_WebBlue td.rgPagerCell
    {
        border: 1px solid;
        border-color: #a2b3c7 #fff #fff;
        border-right-width: 0;
    }
    
    .RadGrid_WebBlue .rgInfoPart
    {
        color: #506175;
    }
    
    .RadGrid_WebBlue .rgInfoPart strong
    {
        color: #000;
    }
    
    .RadGrid_WebBlue .rgPageFirst
    {
        background-position: 0 -550px;
    }
    .RadGrid_WebBlue .rgPageFirst:hover
    {
        background-position: 0 -600px;
    }
    .RadGrid_WebBlue .rgPagePrev
    {
        background-position: 0 -700px;
    }
    .RadGrid_WebBlue .rgPagePrev:hover
    {
        background-position: 0 -750px;
    }
    .RadGrid_WebBlue .rgPageNext
    {
        background-position: 0 -850px;
    }
    .RadGrid_WebBlue .rgPageNext:hover
    {
        background-position: 0 -900px;
    }
    .RadGrid_WebBlue .rgPageLast
    {
        background-position: 0 -1000px;
    }
    .RadGrid_WebBlue .rgPageLast:hover
    {
        background-position: 0 -1050px;
    }
    
    .RadGrid_WebBlue .rgPager .rgPagerButton
    {
        border-color: #4e667e #476077 #425c71;
        background: #d6e1e7 repeat-x 0 -1550px url('~/Portals/0/Images/sprite.gif');
        color: #0d202b;
        font: 12px/12px "segoe ui" ,arial,sans-serif;
    }
    
    .RadGrid_WebBlue .rgNumPart a:hover, .RadGrid_WebBlue .rgNumPart a:hover span, .RadGrid_WebBlue .rgNumPart a.rgCurrentPage, .RadGrid_WebBlue .rgNumPart a.rgCurrentPage span
    {
        background: no-repeat url('~/Portals/0/Images/sprite.gif');
    }
    
    .RadGrid_WebBlue .rgNumPart a
    {
        color: #000;
    }
    
    .RadGrid_WebBlue .rgNumPart a:hover
    {
        background-position: 100% -1250px;
        color: #0e3d4f;
    }
    
    .RadGrid_WebBlue .rgNumPart a:hover span
    {
        background-position: 0 -1150px;
        cursor: pointer;
    }
    
    .RadGrid_WebBlue .rgNumPart a.rgCurrentPage, .RadGrid_WebBlue .rgNumPart a.rgCurrentPage:hover
    {
        background-position: 100% -1450px;
        color: #0053a5;
    }
    
    .RadGrid_WebBlue .rgNumPart a.rgCurrentPage span, .RadGrid_WebBlue .rgNumPart a.rgCurrentPage:hover span
    {
        background-position: 0 -1350px;
    }
    
    /*sorting, reordering*/
    
    .RadGrid_WebBlue .rgHeader .rgSortAsc
    {
        background-position: 3px -247px;
        height: 10px;
    }
    
    .RadGrid_WebBlue .rgHeader .rgSortDesc
    {
        background-position: 3px -197px;
        height: 10px;
    }
    
    .GridReorderTop_WebBlue, .GridReorderBottom_WebBlue
    {
        background: 0 0 no-repeat url('~/Portals/0/Images/sprite.gif');
    }
    
    .GridReorderBottom_WebBlue
    {
        background-position: 0 -50px;
    }
    
    /*filtering*/
    
    .RadGrid_WebBlue .rgFilterRow
    {
        background: #dae2e8;
    }
    
    .RadGrid_WebBlue .rgFilterRow td
    {
        border: 1px solid;
        border-top-width: 0;
        border-color: #dae2e8 #dae2e8 #a2b3c7;
    }
    
    .RadGrid_WebBlue .rgFilter
    {
        background-position: 0 -300px;
    }
    
    .RadGrid_WebBlue .rgFilter:hover
    {
        background-position: 0 -350px;
    }
    
    .RadGrid_WebBlue .rgFilterActive, .RadGrid_WebBlue .rgFilterActive:hover
    {
        background-position: 0 -400px;
    }
    
    .RadGrid_WebBlue .rgFilterBox
    {
        border-color: #768ca5;
        font: 12px "segoe ui" ,arial,sans-serif;
        color: #000;
    }
    
    /*filter context menu*/
    
    .RadMenu_WebBlue .rgHCMClear, .RadMenu_WebBlue .rgHCMFilter
    {
        border-color: #4e667e #476078 #425c71;
        background: #d7e2e7 center -23px repeat-x url('FormDecorator/ButtonSprites.gif');
        color: #0d202b;
        font-family: "segoe ui" ,arial,sans-serif;
        -moz-border-radius: 2px;
        -webkit-border-radius: 2px;
        border-radius: 2px;
    }
    
    .RadMenu_WebBlue .rgHCMClear:hover, .RadMenu_WebBlue .rgHCMFilter:hover
    {
        border-color: #5d9fb7;
        background-position: center -67px;
        background-color: #bee3f6;
        color: #0e3d4f;
    }
    
    /*context menu*/
    
    .GridContextMenuWebBlue .rmLeftImage
    {
        background-image: url('../Common/Grid/contextMenu.gif');
    }
    
    .GridContextMenuWebBlue .rgHCMSortAsc .rmLeftImage
    {
        background-position: 0 0;
    }
    
    .GridContextMenuWebBlue .rgHCMSortDesc .rmLeftImage
    {
        background-position: 0 -40px;
    }
    
    .GridContextMenuWebBlue .rgHCMUnsort .rmLeftImage
    {
        background-position: 0 -80px;
    }
    
    .GridContextMenuWebBlue .rgHCMGroup .rmLeftImage
    {
        background-position: 0 -120px;
    }
    
    .GridContextMenuWebBlue .rgHCMUngroup .rmLeftImage
    {
        background-position: 0 -160px;
    }
    
    .GridContextMenuWebBlue .rgHCMCols .rmLeftImage
    {
        background-position: 0 -200px;
    }
    
    .GridContextMenuWebBlue .rgHCMFilter .rmLeftImage
    {
        background-position: 0 -240px;
    }
    
    .GridContextMenuWebBlue .rgHCMUnfilter .rmLeftImage
    {
        background-position: 0 -280px;
    }
    
    /*grouping*/
    
    .RadGrid_WebBlue .rgGroupPanel
    {
        border: 0;
        border-bottom: 1px solid #3d556c;
        background: #dfeeff;
    }
    
    .RadGrid_WebBlue .rgGroupPanel td
    {
        border: 0;
        padding: 3px;
        vertical-align: middle;
    }
    
    .RadGrid_WebBlue .rgGroupPanel td td
    {
        padding: 0;
    }
    
    .RadGrid_WebBlue .rgGroupPanel .rgSortAsc
    {
        background-position: 4px -144px;
    }
    
    .RadGrid_WebBlue .rgGroupPanel .rgSortDesc
    {
        background-position: 4px -94px;
    }
    
    .RadGrid_WebBlue .rgUngroup
    {
        background-position: 0 -6500px;
    }
    
    .RadGrid_WebBlue .rgGroupItem
    {
        border: 1px solid #506175;
        background: #ebf7ff;
        color: #0053a5;
    }
    
    .RadGrid_WebBlue .rgGroupHeader
    {
        background: #8ea3b9;
        font-size: 1.1em;
        line-height: 21px;
        color: #fff;
    }
    
    .RadGrid_WebBlue .rgGroupHeader td
    {
        padding: 0 8px;
    }
    
    .RadGrid_WebBlue td.rgGroupCol, .RadGrid_WebBlue td.rgExpandCol
    {
        background: #8ea3b9 none;
        border-color: #8ea3b9;
    }
    
    .RadGrid_WebBlue .rgExpand
    {
        background-position: 5px -496px;
    }
    
    .RadGrid_WebBlue .rgCollapse
    {
        background-position: 3px -444px;
    }
    
    /*editing*/
    
    .RadGrid_WebBlue .rgEditForm
    {
        border-bottom: 1px solid #768ca5;
    }
    
    .RadGrid_WebBlue .rgUpdate
    {
        background-position: 0 -1800px;
    }
    
    .RadGrid_WebBlue .rgCancel
    {
        background-position: 0 -1850px;
    }
    
    /*hierarchy*/
    
    .RadGrid_WebBlue .rgDetailTable
    {
        border-color: #768ca5;
    }
    
    /*command row*/
    
    .RadGrid_WebBlue .rgCommandRow
    {
        background: #495a70 0 -2099px repeat-x url('~/Portals/0/Images/sprite.gif');
        color: #fff;
    }
    
    .RadGrid_WebBlue .rgCommandCell
    {
        border: 1px solid #212f41;
        padding: 0;
    }
    
    .RadGrid_WebBlue .rgCommandTable td
    {
        border: 0;
        padding: 2px 7px;
    }
    
    .RadGrid_WebBlue .rgCommandTable
    {
        border: 1px solid;
        border-color: #63758a #3d4b5b #58697d;
    }
    
    .RadGrid_WebBlue .rgCommandRow a
    {
        color: #fff;
        text-decoration: none;
    }
    
    .RadGrid_WebBlue .rgAdd
    {
        margin-right: 3px;
        background-position: 0 -1650px;
    }
    
    .RadGrid_WebBlue .rgRefresh
    {
        margin-right: 3px;
        background-position: 0 -1600px;
    }
    
    .RadGrid_WebBlue .rgEdit
    {
        background-position: 0 -1700px;
    }
    
    .RadGrid_WebBlue .rgDel
    {
        background-position: 0 -1750px;
    }
    
    .RadGrid_WebBlue .rgExpPDF
    {
        background-image: url('../Portals/0/Images/ExportTopdf.gif');
    }
    
    .RadGrid_WebBlue .rgExpPDF
    {
        float: right;
        background-position: 0 0;
        display: block;
    }
    
    
    /*multirow select*/
    
    .GridRowSelector_WebBlue
    {
        background: #3d556c;
    }
    
    /*row drag n drop*/
    
    .GridItemDropIndicator_WebBlue
    {
        border-top: 1px dashed #3d556c;
    }
    
    /*tooltip*/
    
    .GridToolTip_WebBlue
    {
        border: 1px solid #768ca5;
        padding: 3px;
        background: #dae2e8;
        color: #000;
    }
    
    /*rtl*/
    
    .RadGridRTL_WebBlue .rgHeader:first-child, .RadGridRTL_WebBlue th.rgResizeCol:first-child, .RadGridRTL_WebBlue .rgFilterRow > td:first-child, .RadGridRTL_WebBlue .rgRow > td:first-child, .RadGridRTL_WebBlue .rgAltRow > td:first-child
    {
        border-left-width: 1px;
        padding-left: 7px;
    }
    
    .RadGridRTL_WebBlue .rgPageFirst
    {
        background-position: 0 -1000px;
    }
    .RadGridRTL_WebBlue .rgPageFirst:hover
    {
        background-position: 0 -1050px;
    }
    .RadGridRTL_WebBlue .rgPagePrev
    {
        background-position: 0 -850px;
    }
    .RadGridRTL_WebBlue .rgPagePrev:hover
    {
        background-position: 0 -900px;
    }
    .RadGridRTL_WebBlue .rgPageNext
    {
        background-position: 0 -700px;
    }
    .RadGridRTL_WebBlue .rgPageNext:hover
    {
        background-position: 0 -750px;
    }
    .RadGridRTL_WebBlue .rgPageLast
    {
        background-position: 0 -550px;
    }
    .RadGridRTL_WebBlue .rgPageLast:hover
    {
        background-position: 0 -600px;
    }
    
    .RadGridRTL_WebBlue .rgExpand
    {
        background-position: -20px -496px;
    }
</style>
<telerik:RadGrid ID="rgGroup" runat="server" ShowStatusBar="true" AutoGenerateColumns="False"
    CssClass="RadGrid_WebBlue" ShowFooter="true" AllowSorting="True" AllowMultiRowSelection="true"
    AllowPaging="True" GridLines="None" AllowAutomaticDeletes="false" AllowAutomaticInserts="false"
    AllowAutomaticUpdates="false" OnNeedDataSource="rgGroup_NeedDataSource">
    <PagerStyle Mode="NumericPages"></PagerStyle>
    <MasterTableView DataKeyNames="ID" AllowMultiColumnSorting="True" CommandItemDisplay="Top"
        InsertItemDisplay="Top" InsertItemPageIndexAction="ShowItemOnFirstPage">
        <Columns>
            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                ItemStyle-Width="20px" EditImageUrl="~/Portals/0/Images/Edit.gif">
            </telerik:GridEditCommandColumn>
            <telerik:GridBoundColumn SortExpression="ID" HeaderText="ID" HeaderButtonType="TextButton"
                DataField="ID" UniqueName="ID" ReadOnly="true">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="GroupName" HeaderText="Group Name" HeaderButtonType="TextButton"
                DataField="GroupName" UniqueName="GroupName">
            </telerik:GridBoundColumn>
            <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                ConfirmText="Are you sure you to delete this group ?" ItemStyle-Width="20px"
                CommandName="Delete" ImageUrl="~/Portals/0/Images/Delete.png">
            </telerik:GridButtonColumn>
            <%--            <telerik:GridTemplateColumn>
                <asp:HiddenField ID="hidID" runat="server" Value='<%#  Eval("ID") %>' />
            </telerik:GridTemplateColumn>
            --%>
        </Columns>
        <CommandItemSettings AddNewRecordText="Add new Group" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
            ShowRefreshButton="false" ShowExportToPdfButton="true" />
        <EditFormSettings EditFormType="Template" InsertCaption="New Group">
            <FormTemplate>
                <div>
                    <asp:Label ID="lblGroupName" runat="server" Text="Group Name" CssClass="fieldsetControlStyle"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="txtGroupName" runat="server" Text='<%# Eval("GroupName") %>'></asp:TextBox>
                    <br />
                    <br />
                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName='<%# ToggleCommand() %>'
                        CssClass="lnkButton" ToolTip="Update">
                        <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                        CssClass="lnkButton" ToolTip="Cancel">
                        <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" /></asp:LinkButton>
                </div>
            </FormTemplate>
        </EditFormSettings>
    </MasterTableView>
    <ExportSettings HideStructureColumns="true">
        <Pdf PageTitle="Groups" PaperSize="A4" />
    </ExportSettings>
    <ClientSettings>
        <ClientEvents OnRowDblClick="RowDblClick" />
    </ClientSettings>
</telerik:RadGrid>
<%--</table>--%>
