<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_AddRecipients.ascx.vb"
    Inherits="PS_AddRecipients" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadGrid ID="rgRecipients" runat="server" Width="95%" ShowStatusBar="true"
    EnableEmbeddedSkins="false" CssClass="gridviewSpacing" AutoGenerateColumns="False"
    PageSize="10" AllowSorting="True" AllowMultiRowSelection="False" AllowPaging="True"
    GridLines="Both">
    <PagerStyle Mode="NumericPages"></PagerStyle>
    <MasterTableView Width="100%" DataKeyNames="ID" AllowMultiColumnSorting="True" Name="FF_Users"
        CommandItemDisplay="Top">
        <CommandItemSettings AddNewRecordText="Add/Edit Recepients" AddNewRecordImageUrl="~/Portals/0/Images/AddRecord.gif"
            ShowAddNewRecordButton="true" ShowRefreshButton="false" />
        <Columns>
            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                ItemStyle-Width="20px" EditImageUrl="~/Portals/0/Images/Edit.gif">
            </telerik:GridEditCommandColumn>
            <telerik:GridBoundColumn SortExpression="ID" HeaderText="ID" HeaderButtonType="TextButton"
                DataField="ID" UniqueName="ID" ReadOnly="true">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="ShortCut" HeaderText="ShortCut" HeaderButtonType="TextButton"
                DataField="ShortCut" UniqueName="ShortCut">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="CneeCtcName" HeaderText="CneeCtcName" HeaderButtonType="TextButton"
                DataField="CneeCtcName" UniqueName="CneeCtcName">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="CneeName" HeaderText="CneeName" HeaderButtonType="TextButton"
                DataField="CneeName" UniqueName="CneeName">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="CneeAddr1" HeaderText="CneeAddr1" HeaderButtonType="TextButton"
                DataField="CneeAddr1" UniqueName="CneeAddr1">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="CneeAddr2" HeaderText="CneeAddr2" HeaderButtonType="TextButton"
                DataField="CneeAddr2" UniqueName="CneeAddr2">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="CneeTown" HeaderText="CneeTown" HeaderButtonType="TextButton"
                DataField="CneeTown" UniqueName="CneeTown">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="CneeState" HeaderText="CneeState" HeaderButtonType="TextButton"
                DataField="CneeState" UniqueName="CneeState">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="CneePostCode" HeaderText="CneePostCode"
                HeaderButtonType="TextButton" DataField="CneePostCode" UniqueName="CneePostCode">
            </telerik:GridBoundColumn>
            <telerik:GridTemplateColumn HeaderText="Redirect To">
                <ItemTemplate>
                    <asp:Label ID="lblCneeCountryKey" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CneeCountryKey")%>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <%--            <telerik:GridBoundColumn SortExpression="CneeCountryKey" HeaderText="CneeCountryKey"
                HeaderButtonType="TextButton" DataField="CneeCountryKey" UniqueName="CneeCountryKey">
            </telerik:GridBoundColumn>--%>
            <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                ConfirmText="Are you sure you to delete this group ?" ItemStyle-Width="20px"
                CommandName="Delete" ImageUrl="~/Portals/0/Images/Delete.png">
            </telerik:GridButtonColumn>
        </Columns>
        <EditFormSettings EditFormType="Template">
            <FormTemplate>
                <asp:Label ID="lblShortcut" runat="server" Text="Shortcut"></asp:Label>
                <telerik:RadTextBox ID="rtbShortcut" runat="server" EmptyMessage="Please Enter Shortcut"
                    Width="200px" Skin="Vista">
                </telerik:RadTextBox>
                <br />
                <asp:Label ID="lblCtcName" runat="server" Text="Ctc Name"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadTextBox ID="rtbCtcName" runat="server" EmptyMessage="Please Enter CtcName"
                    Width="200px" Skin="Vista">
                </telerik:RadTextBox>
                <br />
                <asp:Label ID="lblCneeName" runat="server" Text="Cnee Name"></asp:Label>
                <telerik:RadTextBox ID="rtbCneeName" runat="server" EmptyMessage="Please Enter Cnee Name"
                    Width="200px" Skin="Vista">
                </telerik:RadTextBox>
                <br />
                <asp:Label ID="lblCneeAddr1" runat="server" Text="Cnee Addr1"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadTextBox ID="rtbCneeAddr1" runat="server" EmptyMessage="Please Enter Cnee Addr1"
                    Width="200px" Skin="Vista">
                </telerik:RadTextBox>
                <br />
                <asp:Label ID="lblCneeAddr2" runat="server" Text="Cnee Addr2"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadTextBox ID="rtbCneeAddr2" runat="server" EmptyMessage="Please Enter Cnee Addr2"
                    Width="200px" Skin="Vista">
                </telerik:RadTextBox>
                <br />
                <asp:Label ID="lblState" runat="server" Text="State"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadTextBox ID="rtbState" runat="server" EmptyMessage="Please Enter State"
                    Width="200px" Skin="Vista">
                </telerik:RadTextBox>
                <br />
                <asp:Label ID="lblCneeTown" runat="server" Text="Town"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadTextBox ID="rtbTown" runat="server" EmptyMessage="Please Enter Cnee Town"
                    Width="200px" Skin="Vista">
                </telerik:RadTextBox>
                <br />
                <asp:Label ID="lblCneePostCode" runat="server" Text="Cnee Post Code"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <telerik:RadTextBox ID="rtbPostCode" runat="server" EmptyMessage="Please Enter Cnee Post Code"
                    Width="200px" Skin="Vista">
                </telerik:RadTextBox>
                <br />
                <asp:Label ID="lblCneeCountry" runat="server" Text="Country"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="fieldsetControlStyle fieldsetControlWidth" />
                <%--                <telerik:RadComboBox ID="rcbCountry" runat="server" Skin="Vista">
                </telerik:RadComboBox>--%>
                <br />
                <asp:HiddenField ID="hidFFRecipientID" Value='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                    runat="server" />
                <br />
                <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName='<%# IIf( DataBinder.Eval(Container, "OwnerTableView.IsItemInserted"), "PerformInsert", "Update") %>'
                    CssClass="lnkButton" ToolTip="Update">
                    <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                    CssClass="lnkButton" ToolTip="Cancel">
                    <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" /></asp:LinkButton>
                <asp:HiddenField ID="hidRecepientsID" Value='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                    runat="server" />
            </FormTemplate>
        </EditFormSettings>
    </MasterTableView>
</telerik:RadGrid>