<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SNR_UserAuthorization.ascx.vb"
    Inherits="SNR_UserAuthorization" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadFormDecorator ID="rfd" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
<telerik:RadGrid ID="rgUsers" runat="server" AutoGenerateColumns="False" Skin="Vista"
    AlternatingItemStyle-BackColor="LightBlue" AllowSorting="false" AllowPaging="True"
    PageSize="10" GridLines="None" ShowGroupPanel="false">
    <MasterTableView AllowMultiColumnSorting="false" Name="Users" CommandItemDisplay="None">
        <Columns>
            <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name" SortExpression="UserName"
                AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" UniqueName="UserName">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="FirstName" HeaderText="First Name" SortExpression="FirstName"
                AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" UniqueName="FirstName">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="LastName" HeaderText="Last Name" SortExpression="LastName"
                AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" UniqueName="LastName">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Membership.CreatedDate" HeaderText="Created On" SortExpression="Membership.CreatedDate" DataFormatString="{0:dd-MMM-yyyy}" 
                AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" UniqueName="Membership.CreatedDate">
            </telerik:GridBoundColumn>
            <telerik:GridTemplateColumn HeaderText="Authorize?">
                <ItemTemplate>
                    <asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# Bind("Membership.Approved") %>' AutoPostBack="true" 
                        OnCheckedChanged="chkAuthorized_CheckedChanged" />
                    <asp:HiddenField ID="hidUserID" runat="server" Value='<%# Bind("UserID") %>' />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
        </Columns>
    </MasterTableView>
</telerik:RadGrid>
