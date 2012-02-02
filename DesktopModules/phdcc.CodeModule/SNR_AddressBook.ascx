<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SNR_AddressBook.ascx.vb"
    Inherits="SNR_AddressBook" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">

    function showwindow(sender, eventargs) {

        sender.center();
    }
    
</script>
<telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server" />
<telerik:RadListView ID="rlvAddress" runat="server" ItemPlaceholderID="AddressContainer"
    AllowMultiItemSelection="true" Skin="Vista" PageSize="9" AllowPaging="true" DataKeyNames="ID">
    <LayoutTemplate>
        <fieldset style="padding: 5px;">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td width="100%">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnAddAddress" runat="server" CommandName="Insert" Visible='<%# Container.InsertItemPosition = RadListViewInsertItemPosition.None %>'
                                        Text="Add new Address" OnClick="btnAddAddress_Click" />
                                </td>
                                <td align="center" width="82%">
                                    <asp:Panel ID="pnl" runat="server" DefaultButton="btnSearch">
                                        <b>Search</b>
                                        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnSearch" runat="server" Text="Go" OnClick="btnSearch_Click" />
                                    </asp:Panel>
                                </td>
                                <td align="right" width="100%">
                                    <asp:Button ID="btnToggle" runat="server" Text="Shared" OnClick="btnToggle_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="AddressContainer" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadDataPager ID="RadDataPager1" runat="server" PagedControlID="rlvAddress"
                            PageSize="9">
                            <Fields>
                                <telerik:RadDataPagerButtonField FieldType="FirstPrev" />
                                <telerik:RadDataPagerButtonField FieldType="Numeric" />
                                <telerik:RadDataPagerButtonField FieldType="NextLast" />
                                <telerik:RadDataPagerPageSizeField PageSizeText="Page size: " />
                                <telerik:RadDataPagerTemplatePageField>
                                    <PagerTemplate>
                                        <div style="float: right">
                                            <b>Items
                                                <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.Owner.StartRowIndex+1%>" />
                                                to
                                                <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# IIF(Container.Owner.TotalRowCount > (Container.Owner.StartRowIndex+Container.Owner.PageSize), Container.Owner.StartRowIndex+Container.Owner.PageSize, Container.Owner.TotalRowCount) %>" />
                                                of
                                                <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.Owner.TotalRowCount%>" />
                                                <br />
                                            </b>
                                        </div>
                                    </PagerTemplate>
                                </telerik:RadDataPagerTemplatePageField>
                            </Fields>
                        </telerik:RadDataPager>
                    </td>
                </tr>
            </table>
        </fieldset>
    </LayoutTemplate>
    <ItemTemplate>
        <fieldset style="float: left; width: 270px; height: 210px; padding: 5px; margin: 5px;">
            <legend style="padding: 5px;">
                <asp:LinkButton ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName") %>'
                    CommandName="Select" Font-Bold="true"></asp:LinkButton>
            </legend>
            <table cellpadding="0" cellspacing="0px" width="100%">
                <tr>
                    <td width="50%">
                        <b>Contact</b>
                    </td>
                    <td>
                        <asp:Label ID="lblContactName" Text='<%# Bind("ContactName")%>' runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Address 1</b>
                    </td>
                    <td>
                        <asp:Label ID="lblAddress1" Text='<%# Bind("Address1")%>' runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Address 2</b>
                    </td>
                    <td>
                        <asp:Label ID="lblAddress2" Text='<%# Bind("Address2")%>' runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Country</b>
                    </td>
                    <td>
                        <asp:Label ID="lblCountry" Text='<%# Bind("CountryKey")%>' runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>City</b>
                    </td>
                    <td>
                        <asp:Label ID="lblCity" Text='<%# Bind("City")%>' runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Region</b>
                    </td>
                    <td>
                        <asp:Label ID="lblRegionCode" Text='<%# Bind("Region")%>' runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Post Code</b>
                    </td>
                    <td>
                        <asp:Label ID="lblPostalCode" Text='<%# Bind("PostCode")%>' runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Phone</b>
                    </td>
                    <td>
                        <asp:Label ID="lblPhone" Text='<%# Bind("Phone")%>' runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Mobile</b>
                    </td>
                    <td>
                        <asp:Label ID="lblMobile" Text='<%# Bind("Mobile")%>' runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Email</b>
                    </td>
                    <td>
                        <asp:Label ID="lblEmail" Text='<%# Bind("Email")%>' runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:LinkButton ID="lnkbtnDelete" runat="server" CausesValidation="True" CommandName="Delete"
                            ToolTip="Delete" OnClientClick="javascript:if(!confirm('Are you sure you want to delete this address ?')){return false;}"
                            CommandArgument='<%# Eval("ID")%>'>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Portals/3/Images/Delete.png" />
                        </asp:LinkButton>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="True" CommandName="Edit"
                            ToolTip="Edit" CommandArgument='<%# Eval("ID")%>'>
                            <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Portals/3/Images/Edit.gif" />
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </fieldset>
    </ItemTemplate>
</telerik:RadListView>
<telerik:RadWindow ID="rwAddress" runat="server" Title="Add New Address" Height="500px"
    OnClientBeforeShow="showwindow" Skin="Vista" EnableShadow="true" Width="350px"
    ReloadOnShow="true" ShowContentDuringLoad="false" Behaviors="Move,Pin,Resize"
    Visible="false" DestroyOnClose="true">
    <ContentTemplate>
        <table id="tblRw" cellpadding="1px" cellspacing="1px" border="0" width="100%">
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary ID="vs" ValidationGroup="vg" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFirstName" Text="Contact Name" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtContactName" Text="" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCompanyName" Text="Company Name" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCompanyName" Text="" runat="server" MaxLength="100" ValidationGroup="vg"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCompnayName" runat="server" ControlToValidate="txtCompanyName"
                        Display="None" ErrorMessage="Please Enter Company Name." ToolTip="Please Enter Company Name"
                        ValidationGroup="vg">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAddress" Text="Address 1" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAddress1" Text="" runat="server" MaxLength="100" ValidationGroup="vg"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddress1"
                        Display="None" ErrorMessage="Please Enter Address 1." ToolTip="Please Enter Address 1"
                        ValidationGroup="vg">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="Address 2" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAddress2" Text="" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" Text="Country" runat="server"></asp:Label>
                </td>
                <td>
                    <telerik:RadComboBox ID="rcbCountry" runat="server" Skin="Default" Height="100px"
                        Width="200px" DataTextField="CountryName" DataValueField="CountryKey" AllowCustomText="true"
                        MarkFirstMatch="True" HighlightTemplatedItems="True" DropDownWidth="200px" EmptyMessage="- Select Country -">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCity" Text="City" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCity" Text="" runat="server" MaxLength="100" ValidationGroup="vg"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
                        Display="None" ErrorMessage="Please Enter City." ToolTip="Please Enter City"
                        ValidationGroup="vg">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRegionCode" Text="Region" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRegion" Text="" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPostalCode" Text="Post Code" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPostalCode" Text="" runat="server" ValidationGroup="vg"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPostalCode" runat="server" ControlToValidate="txtPostalCode"
                        Display="None" ErrorMessage="Please Enter Post Code." ToolTip="Please Enter Post Code"
                        ValidationGroup="vg">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPhone" Text="Phone" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPhone" Text="" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMobile" Text="Mobile" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMobile" Text="" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEmail" Text="Email" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" Text="" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:CheckBox ID="chkIsGlobal" Text="Is Global" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" ValidationGroup="vg" />
                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="false"
                        CommandName="Cancel" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</telerik:RadWindow>
