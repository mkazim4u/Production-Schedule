<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_AssignRoleToUser.ascx.vb"
    Inherits="PS_AssignRoleToUser" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadGrid ID="rgFFUser" runat="server" Width="95%" ShowStatusBar="true" EnableEmbeddedSkins="false"
    CssClass="gridviewSpacing" AutoGenerateColumns="False" PageSize="10" AllowSorting="True"
    AllowMultiRowSelection="False" AllowPaging="True" GridLines="Both">
    <PagerStyle Mode="NumericPages"></PagerStyle>
    <MasterTableView Width="100%" DataKeyNames="ID" AllowMultiColumnSorting="True" Name="FF_Users"
        CommandItemDisplay="Top">
        <CommandItemSettings AddNewRecordText="Add/Edit FFUser" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
            ShowAddNewRecordButton="true" ShowRefreshButton="false" />
        <Columns>
            <telerik:GridTemplateColumn Visible="false">
                <ItemTemplate>
                    <asp:HiddenField ID="hidFFUserID" Value='<%# Bind("ID")%>' runat="server" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="User Name">
                <ItemTemplate>
                    <asp:Label ID="lblUserName" runat="server" Text='<% Eval("username") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <telerik:RadComboBox ID="rcbUsers" runat="server" Skin="Vista" AutoPostBack="True">
                    </telerik:RadComboBox>
                </EditItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Role Name">
                <ItemTemplate>
                    <asp:Label ID="lblRoleName" runat="server" Text='<% Eval("rolename") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <telerik:RadComboBox ID="rcbRoles" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
                        Skin="Vista" AutoPostBack="True">
                    </telerik:RadComboBox>
                </EditItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Redirect To First Time">
                <ItemTemplate>
                    <asp:Label ID="lblRedirectToFirstTime" runat="server" Text='<% Eval("RedirectToFirstTime") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <telerik:RadTextBox ID="rtbRedirectToFirstTime" runat="server" EmptyMessage="Enter First Time Redirect Page Name" />
                </EditItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Redirect To">
                <ItemTemplate>
                    <asp:Label ID="lblRedirectTo" runat="server" Text='<% Eval("RedirectTo") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <telerik:RadTextBox ID="rtbRedirectTo" runat="server" EmptyMessage="Enter Redirect Page Name" />
                </EditItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridDateTimeColumn SortExpression="CreatedOn" HeaderText="Created On" HeaderButtonType="TextButton"
                DataFormatString="{0:dd-MMM-yyyy}" DataField="CreatedOn" UniqueName="CreatedOn"
                ReadOnly="true" HeaderStyle-Font-Bold="true">
            </telerik:GridDateTimeColumn>
            <telerik:GridTemplateColumn HeaderText="Created By" HeaderStyle-Font-Bold="true">
                <ItemTemplate>
                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Is First Time" HeaderStyle-Font-Bold="true">
                <ItemTemplate>
                    <asp:CheckBox ID="chkIsFirstTime" runat="server" AutoPostBack="true" OnCheckedChanged="chkIsFirstTime_CheckedChanged">
                    </asp:CheckBox>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
        </Columns>
    </MasterTableView>
</telerik:RadGrid>