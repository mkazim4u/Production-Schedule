<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_AddADUsersToPS.ascx.vb"
    Inherits="PS_AddADUsersToPS" %>
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
                    <asp:HiddenField ID="hidFFUserID" Value='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                        runat="server" />
                    <asp:HiddenField ID="hidRedirectToFirstTime" Value='<%# DataBinder.Eval(Container.DataItem, "RedirectToFirstTime")%>'
                        runat="server" />
                    <asp:HiddenField ID="hidRedirectTo" Value='<%# DataBinder.Eval(Container.DataItem, "RedirectTo")%>'
                        runat="server" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn>
                <ItemTemplate>
                    <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit" ImageUrl="~/Portals/0/Images/Edit.gif"
                        CommandName="Edit" />
                    &nbsp;
                    <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" OnClientClick="javascript:if(!confirm('Are you sure you want to delete this record?')){return false;}"
                        ImageUrl="~/Portals/0/Images/Delete.png" CommandName="Delete" />
                </ItemTemplate>
                <ItemStyle Width="50px" />
            </telerik:GridTemplateColumn>
            <%--            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                EditText="Edit" UpdateImageUrl="~/Portals/0/Images/Update.gif" ItemStyle-Width="40px"
                EditImageUrl="~/Portals/0/Images/Edit.gif" CancelImageUrl="~/Portals/0/Images/Cancel.gif">
            </telerik:GridEditCommandColumn>--%>
            <telerik:GridTemplateColumn HeaderText="User Name">
                <ItemTemplate>
                    <asp:Label ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "username")%>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Role Name">
                <ItemTemplate>
                    <asp:Label ID="lblRoleName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "roleid")%>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Redirect To First Time">
                <ItemTemplate>
                    <asp:Label ID="lblRedirectToFirstTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedirectToFirstTime")%>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Redirect To">
                <ItemTemplate>
                    <asp:Label ID="lblRedirectTo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RedirectTo")%>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridDateTimeColumn SortExpression="CreatedOn" HeaderText="Created On" HeaderButtonType="TextButton"
                DataFormatString="{0:dd-MMM-yyyy}" DataField="CreatedOn" UniqueName="CreatedOn"
                ReadOnly="true">
            </telerik:GridDateTimeColumn>
            <telerik:GridTemplateColumn HeaderText="Created By">
                <ItemTemplate>
                    <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridCheckBoxColumn HeaderText="Is First Time" DataType="System.Boolean"
                DataField="IsFirstTime" ShowSortIcon="false">
            </telerik:GridCheckBoxColumn>
            <telerik:GridCheckBoxColumn HeaderText="Is AD User" DataType="System.Boolean" DataField="IsADUser"
                ShowSortIcon="false">
            </telerik:GridCheckBoxColumn>
            <%--            <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                ConfirmText="Are you sure you to delete this Record ?" ItemStyle-Width="20px"
                CommandName="Delete" ImageUrl="~/Portals/0/Images/Delete.png">
            </telerik:GridButtonColumn>--%>
            <%--            <telerik:GridTemplateColumn >
                <ItemTemplate>
                    <asp:CheckBox ID="chkIsFirstTime" runat="server" AutoPostBack="true" OnCheckedChanged="chkIsFirstTime_CheckedChanged" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>--%>
        </Columns>
        <EditFormSettings EditFormType="Template">
            <FormTemplate>
                <asp:Label ID="lblUserType" runat="server" Text="User Type"></asp:Label>
                <asp:RadioButtonList ID="rblUserType" RepeatLayout="Flow" RepeatDirection="Horizontal"
                    runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblUserType_OnSelectedIndexChanged">
                    <asp:ListItem Text="Normal User" Value="0" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Active Directory User" Value="1"></asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <br />
                <asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadComboBox ID="rcbUser" runat="server" Skin="Vista" Filter="StartsWith">
                </telerik:RadComboBox>
                <br />
                <br />
                <asp:Label ID="lblRoleName" runat="server" Text="Role Name"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadComboBox ID="rcbRole" runat="server" Skin="Vista">
                </telerik:RadComboBox>
                <br />
                <br />
                <asp:Label ID="lblParentPage" runat="server" Text="Parent Page"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadComboBox ID="rcbParentTabs" runat="server" Skin="Vista" AutoPostBack="true"
                    OnSelectedIndexChanged="rcbParentTabs_OnSelectedIndexChanged">
                </telerik:RadComboBox>
                <br />
                <br />
                <asp:Label ID="lblFirstTimeRedirectPage" runat="server" Text="First Time Redirect Page"></asp:Label>
                
                <%--                <telerik:RadTextBox ID="rtbRedirectToFirstTime" runat="server" EmptyMessage="Enter First Time Redirect Page Name"
                    Width="200px" Skin="Vista">
                </telerik:RadTextBox>--%>
                <telerik:RadComboBox ID="rcbRedirectToFirstTime" runat="server" Skin="Vista">
                </telerik:RadComboBox>
                <br />
                <br />
                <asp:Label ID="lblRedirectTo" runat="server" Text="Redirect To"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <%--                <telerik:RadTextBox ID="rtbRedirectTo" runat="server" EmptyMessage="Enter Redirect Page Name"
                    Width="200px" Skin="Vista">
                </telerik:RadTextBox>--%>
                <telerik:RadComboBox ID="rcbRedirectTo" runat="server" Skin="Vista">
                </telerik:RadComboBox>
                <br />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkIsFirstTime" runat="server" Text="Is First Time"></asp:CheckBox>
                <br />
                <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName='<%# IIf( DataBinder.Eval(Container, "OwnerTableView.IsItemInserted"), "PerformInsert", "Update") %>'
                    CssClass="lnkButton" ToolTip="Update">
                    <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                    CssClass="lnkButton" ToolTip="Cancel">
                    <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" /></asp:LinkButton>
                <asp:HiddenField ID="hidFFUserID" Value='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                    runat="server" />
                <asp:HiddenField ID="hidIsADUser" Value='<%# DataBinder.Eval(Container.DataItem, "IsADUser")%>'
                    runat="server" />
                <asp:HiddenField ID="hidParentTabID" Value='<%# DataBinder.Eval(Container.DataItem, "ParentTabID")%>'
                    runat="server" />
                <%--                <asp:HiddenField ID="hidIsADUser" Value='<%# DataBinder.Eval(Container.DataItem, "IsADUser")%>'
                    runat="server" />--%>
            </FormTemplate>
        </EditFormSettings>
    </MasterTableView>
</telerik:RadGrid>
<%--Text='<%# IIf( DataBinder.Eval(Container, "OwnerTableView.IsItemInserted"), "Insert", "Update") %>'>--%>