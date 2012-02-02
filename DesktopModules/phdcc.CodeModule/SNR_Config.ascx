<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SNR_Config.ascx.vb" Inherits="SNR_Config" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    div.RadGrid_Vista, div.RadGrid_Vista .rgMasterTable, div.RadGrid_Vista .rgDetailTable, div.RadGrid_Vista .rgGroupPanel table, div.RadGrid_Vista .rgCommandRow table, div.RadGrid_Vista .rgEditForm table, div.RadGrid_Vista .rgPager table, span.GridToolTip_Vista
    {
        font-family: Verdana;
        font-size: xx-small;
    }
</style>
<telerik:RadFormDecorator ID="rfd" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
<telerik:RadGrid ID="rgConfiguration" runat="server" AutoGenerateColumns="False"
    Skin="Vista" AllowSorting="false" AllowPaging="True" PageSize="10" GridLines="None"
    ShowGroupPanel="false">
    <MasterTableView AllowMultiColumnSorting="false" Name="Configuration" DataKeyNames="ID"
        CommandItemDisplay="Top" AllowFilteringByColumn="false">
        <CommandItemSettings AddNewRecordImageUrl="" />
        <Columns>
            <telerik:GridTemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Bind("ID") %>' CommandName="Edit">
                        <asp:Image ID="imgEdit" runat="server" />
                    </asp:LinkButton>
                    <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Bind("ID") %>'
                        CommandName="Delete" OnClientClick="javascript:if(!confirm('Are you sure you want to delete this record?')){return false;}">
                        <asp:Image ID="imgDelete" runat="server" />
                    </asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="60px" />
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Key">
                <ItemTemplate>
                    <asp:Label ID="lblKey" runat="server" Text='<%# Bind("ConfigKey") %>'></asp:Label>
                    <asp:HiddenField ID="hidID" runat="server" Value='<%# Bind("ID") %>' />
                    <asp:HiddenField ID="hidCreatedBy" runat="server" Value='<%# Bind("CreatedBy") %>' />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Value">
                <ItemTemplate>
                    <asp:Label ID="lblValue" runat="server" Text='<%# Bind("ConfigValue") %>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridBoundColumn SortExpression="CreatedOn" HeaderText="Created On" HeaderButtonType="TextButton"
                DataFormatString="{0:dd-MMM-yyyy}" DataField="CreatedOn" UniqueName="CreatedOn"
                ReadOnly="true">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn SortExpression="CreatedBy" HeaderText="Created By" HeaderButtonType="TextButton"
                DataField="CreatedBy" UniqueName="CreatedBy" ReadOnly="true">
            </telerik:GridBoundColumn>
        </Columns>
        <EditFormSettings EditFormType="Template">
            <FormTemplate>
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <asp:ValidationSummary ID="vs" runat="server" ValidationGroup="vg" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblKey" runat="server" Text="Key"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKey" runat="server" Text='<%# Bind("ConfigKey") %>' Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="KeyRequired" runat="server" ControlToValidate="txtKey"
                                ErrorMessage="Please Enter Key." ToolTip="Please Enter Key" ValidationGroup="vg">*</asp:RequiredFieldValidator>
                            <%--                            <asp:CustomValidator runat="server" ID="cusKey" ControlToValidate="txtKey" ValidationGroup="vg" 
                                OnServerValidate="cusKey_ServerValidate" ErrorMessage="Key Already Exists" />
                            --%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Value"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValue" runat="server" Text='<%# Bind("ConfigValue") %>' Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ValueRequired" runat="server" ControlToValidate="txtValue"
                                ErrorMessage="Please Enter Value." ToolTip="Please Enter Value" ValidationGroup="vg">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsAdmin" runat="server" Text="Is Admin ?" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnInsert" Text="Insert" CommandName="PerformInsert" runat="server" Visible="false" 
                                CommandArgument='<%# Bind("ID") %>' ValidationGroup="vg" />
                            <asp:Button ID="btnUpdate" Text="Update" CommandName="Update" runat="server" CommandArgument='<%# Bind("ID") %>'
                                ValidationGroup="vg" Visible="false"   />
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="false"
                                CommandName="Cancel" />
                        </td>
                    </tr>
            </FormTemplate>
        </EditFormSettings>
    </MasterTableView>
</telerik:RadGrid>
