<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_Item.ascx.vb" Inherits="PS_Item" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls"
    Assembly="DotNetNuke" %>
<script type="text/javascript">
    function RowDblClick(sender, eventArgs) {
        sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
    }
</script>
<link href="module.css" rel="stylesheet" type="text/css" />
<telerik:RadGrid ID="rgItem" runat="server" ShowStatusBar="true" AutoGenerateColumns="False"
    CssClass="RadGrid_WebBlue" ShowFooter="true" AllowSorting="True" AllowMultiRowSelection="true"
    AllowPaging="True" GridLines="None" AllowAutomaticDeletes="false" AllowAutomaticInserts="false"
    AllowAutomaticUpdates="false" OnNeedDataSource="rgItem_NeedDataSource">
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
            <telerik:GridBoundColumn SortExpression="ItemName" HeaderText="Item Name" DataField="ItemName"
                UniqueName="ItemName">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="Rate" HeaderText="Rate" DataField="Rate"
                UniqueName="Rate">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="Price" HeaderText="Price" DataField="Price"
                UniqueName="Price">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="GroupName" HeaderText="GroupName" DataField="GroupName"
                UniqueName="GroupName">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="Position" HeaderText="Position" DataField="Position"
                UniqueName="Position">
            </telerik:GridBoundColumn>
            <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                ConfirmText="Are you sure you to delete this Item ?" ItemStyle-Width="20px" CommandName="Delete"
                ImageUrl="~/Portals/0/Images/Delete.png">
            </telerik:GridButtonColumn>
        </Columns>
        <CommandItemSettings AddNewRecordText="Add new Item" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
            ShowRefreshButton="false" ShowExportToPdfButton="true" />
        <EditFormSettings EditFormType="Template" InsertCaption="New Item">
            <FormTemplate>
                <div>
                    <asp:Label ID="lblItemName" runat="server" Text="Item Name" CssClass="fieldsetControlStyle"></asp:Label>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="txtItemName" runat="server" Text='<%# Eval("ItemName") %>'></asp:TextBox>
                    <br />
                    <asp:Label ID="lblPrice" runat="server" Text="Price" CssClass="fieldsetControlStyle"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadNumericTextBox ID="rntbPrice" MinValue="0" runat="server" MaxLength="6"
                       Width="200px" Type="Currency" Culture="English (United Kingdom)">
                    </telerik:RadNumericTextBox>
                    <br />
                    <asp:Label ID="lblRate" runat="server" Text="Rate" CssClass="fieldsetControlStyle"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtRate" runat="server" Text='<%# Eval("Rate") %>'></asp:TextBox>
                    <br />
                    <asp:Label ID="lblPosition" runat="server" Text="Position" CssClass="fieldsetControlStyle"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtPosition" runat="server" Text='<%# Eval("Position") %>'></asp:TextBox>
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="Group" CssClass="fieldsetControlStyle"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadComboBox ID="rcbGroup" runat="server" Height="100px">
                    </telerik:RadComboBox>
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
        <Pdf PageTitle="Items" PaperSize="A4" />
    </ExportSettings>
    <ClientSettings>
        <ClientEvents OnRowDblClick="RowDblClick" />
    </ClientSettings>
</telerik:RadGrid>