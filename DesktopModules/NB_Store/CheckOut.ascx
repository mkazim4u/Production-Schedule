<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CheckOut.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.CheckOut" %>
<%@ Register TagPrefix="nbs" TagName="CartList" Src="CartList.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="nbs" TagName="Address" Src="Address.ascx" %>
<%@ Register TagPrefix="nbs" TagName="CustomForm" Src="controls/CustomForm.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<div class="Checkout">
    <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
    <asp:Panel ID="pnlEmptyCart" runat="server">
        <asp:PlaceHolder ID="phEmptyCart" runat="server"></asp:PlaceHolder>
    </asp:Panel>
    <asp:Panel ID="pnlCart" runat="server">
        <nbs:CartList runat="server" id="cartlist1" />
        <div id="divShipCountry" runat="server" class="NBright_CartOptDiv">
            <dnn:label id="plShipCountry1" runat="server" controlname="plShipCountry" suffix=""></dnn:label>
            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div id="divShipMethod" runat="server" class="NBright_CartOptDiv">
            <asp:RadioButtonList ID="rblShipMethod" runat="server" AutoPostBack="true">
            </asp:RadioButtonList>
        </div>
        <div id="divVATCode" runat="server" class="NBright_CartOptDiv">
            <dnn:label id="plVATCode" runat="server" controlname="plVATCode" suffix=""></dnn:label>
            <asp:TextBox ID="txtVATCode" runat="server"></asp:TextBox>&nbsp;<asp:LinkButton ID="cmdVAT"
                runat="server" resourcekey="cmdVAT">OK</asp:LinkButton>
        </div>
        <div id="divPromoCode" runat="server" class="NBright_CartOptDiv">
            <dnn:label id="plPromoCode" runat="server" controlname="plPromoCode" suffix=""></dnn:label>
            <asp:TextBox ID="txtPromoCode" runat="server"></asp:TextBox>&nbsp;<asp:LinkButton
                ID="cmdPromo" runat="server" resourcekey="cmdPromo">OK</asp:LinkButton>
        </div>
        <div class="NBright_ClientButtonDiv">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td class="NBright_ClientButtonDivLeft">
                        <asp:LinkButton ID="cmdContShop" runat="server" CssClass="NBright_ClientButton" resourcekey="cmdContShop">Continue Shopping</asp:LinkButton>
                    </td>
                    <td class="NBright_ClientButtonDivRight">
                        <asp:LinkButton ID="cmdOrder" runat="server" CssClass="NBright_ClientButton" resourcekey="cmdOrder">Order</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlLogin" runat="server">
        <asp:PlaceHolder ID="phLogin" runat="server"></asp:PlaceHolder>
    </asp:Panel>
    <asp:Panel ID="pnlAddressDetails" runat="server">
        <asp:Panel ID="pnlAddress" runat="server">
            <fieldset>
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td class="SubHead" nowrap>
                            <dnn:label id="plEmail" runat="server" controlname="txtEmail" suffix=":" text="Email"></dnn:label>
                        </td>
                        <td valign="top" nowrap>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="NormalTextBox" Width="300px"
                                MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" CssClass="NormalRed"
                                Text="*" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Address" ValidationExpression='[^"\r\n]*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*'
                                Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="valEmail" runat="server" CssClass="NormalRed" ControlToValidate="txtEmail"
                                Text="*" ErrorMessage="Email Is Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td nowrap>
                                        <dnn:label id="plDefaultEmail" runat="server" controlname="plDefaultEmail" suffix=""
                                            text=""></dnn:label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkDefaultEmail" resourcekey="chkDefaultEmail" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            <fieldset>
                <legend>
                    <dnn:label id="lblBillingAddressTitle" runat="server" controlname="lblBillingAddressTitle"></dnn:label>
                </legend>
                <asp:Button ID="btnFill" Text="Show Address Book" runat="server" CausesValidation="false" />
                <br />
                <div id="divBillAddress" runat="server">
                    <nbs:address id="billaddress" runat="server"></nbs:address>
                </div>
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td nowrap>
                            <dnn:label id="plDefaultAddress" runat="server" controlname="plDefaultAddress" suffix=""
                                text=""></dnn:label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkDefaultAddress" resourcekey="chkDefaultAddress" runat="server" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div class="NBright_ShippingOptions">
                <fieldset>
                    <asp:RadioButton ID="radNone" runat="server" GroupName="radShipAddress" AutoPostBack="True">
                    </asp:RadioButton><dnn:label id="lblNone" runat="server" controlname="radNone"></dnn:label>
                    <asp:RadioButton ID="radBilling" runat="server" GroupName="radShipAddress" AutoPostBack="True">
                    </asp:RadioButton><dnn:label id="lblUseBillingAddress" runat="server" controlname="radBilling"></dnn:label>
                    <asp:RadioButton ID="radShipping" runat="server" GroupName="radShipAddress" AutoPostBack="True">
                    </asp:RadioButton><dnn:label id="lblUseShippingAddress" runat="server" controlname="radShipping"></dnn:label>
                </fieldset>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlShipAddress" runat="server">
            <fieldset>
                <legend>
                    <dnn:label id="lblShippingAddressTitle" runat="server" controlname="lblShippingAddressTitle"></dnn:label>
                </legend>
                <nbs:address id="shipaddress" runat="server"></nbs:address>
            </fieldset>
        </asp:Panel>
        <div class="NBright_ClientButtonDiv">
            <asp:CheckBox ID="chkSaveAddrCookie" resourcekey="chkSaveAddrCookie" runat="server"
                Checked="true" />
            <dnn:label id="plSaveAddrCookie" runat="server" controlname="plSaveAddrCookie" suffix=""
                text=""></dnn:label>
        </div>
        <p>
            <div id="divVATCode2" runat="server" class="NBright_CartOptDiv">
                <dnn:label id="plVATCode2" runat="server" controlname="plVATCode2" suffix="" resourcekey="plVATCode"></dnn:label>
                <asp:TextBox ID="txtVATCode2" runat="server"></asp:TextBox>
            </div>
            <asp:PlaceHolder ID="plhNoteMsg" runat="server"></asp:PlaceHolder>
            <nbs:CustomForm ID="Stg2Form" runat="server" DisplayTemplateName="stg2form.template"></nbs:CustomForm>
            <asp:TextBox CssClass="SpecialInstructions" ID="txtNoteMsg" runat="server" MaxLength="500"
                TextMode="MultiLine"></asp:TextBox>
            <p>
            </p>
            <div class="NBright_ClientButtonDiv">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="NBright_ClientButtonDivLeft">
                            <asp:LinkButton ID="cmdBack1" runat="server" CausesValidation="False" CssClass="NBright_ClientButton"
                                resourcekey="cmdBack">B</asp:LinkButton>
                        </td>
                        <td class="NBright_ClientButtonDivRight">
                            <asp:LinkButton ID="cmdNext1" runat="server" CssClass="NBright_ClientButton" resourcekey="cmdNext">N</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
        </p>
    </asp:Panel>
    <asp:Panel ID="pnlPromptPay" runat="server">
        <nbs:CartList runat="server" id="cartlist2" />
        <div id="divShipMethod2" runat="server" class="NBright_CartOptDiv">
            <asp:RadioButtonList ID="rblShipMethod2" runat="server" AutoPostBack="true">
            </asp:RadioButtonList>
        </div>
        <nbs:CustomForm ID="Stg3Form" runat="server" DisplayTemplateName="stg3form.template"></nbs:CustomForm>
        <asp:Panel ID="pnlGateway2" runat="server">
            <asp:DataList ID="dlGateway2" runat="server">
            </asp:DataList>
        </asp:Panel>
        <asp:Panel ID="pnlGateway1" runat="server">
            <fieldset>
                <legend>
                    <asp:Label ID="lblBankCard" runat="server" resourcekey="lblBankCard"></asp:Label>
                </legend>
                <table id="Table1" class="GateWays" cellspacing="0" cellpadding="4" border="0">
                    <tr>
                        <td valign="top">
                            <asp:PlaceHolder ID="plhGatewayMsg" runat="server"></asp:PlaceHolder>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td valign="top">
                            <strong>
                                <asp:LinkButton ID="lnkCheque" runat="server" CssClass="Cheque_Link">Label</asp:LinkButton></strong>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" align="center" class="CardGatewayButton">
                            <asp:PlaceHolder ID="plhGateway" runat="server"></asp:PlaceHolder>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td valign="middle" align="center" class="ManualGatewayButton">
                            <asp:ImageButton ID="imgBChq" runat="server" Visible="False"></asp:ImageButton>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
        <div class="NBright_ClientButtonDiv">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td class="NBright_ClientButtonDivLeft">
                        <asp:LinkButton ID="cmdBack2" runat="server" CssClass="NBright_ClientButton" resourcekey="cmdBack">B</asp:LinkButton>
                        <asp:LinkButton ID="cmdCancelOrder" runat="server" CssClass="NBright_ClientButton CancelOrderButton"
                            resourcekey="cmdCancelOrder">C</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlPayRtn" runat="server">
        <asp:PlaceHolder ID="plhPayRtn" runat="server"></asp:PlaceHolder>
    </asp:Panel>
</div>
<table id="tblAddressBook" runat="server" visible="false">
    <tr>
        <td>
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
                                                <%--                                                <asp:Button ID="btnAddAddress" runat="server" CommandName="Insert" Visible='<%# Container.InsertItemPosition = RadListViewInsertItemPosition.None %>'
                                                    Text="Add new Address" OnClick="btnAddAddress_Click" />--%>
                                                <asp:Button ID="btnAddAddress" runat="server" CommandName="Insert" Visible="false" 
                                                    Text="Add new Address" OnClick="btnAddAddress_Click" />
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
                            <asp:LinkButton ID="lnkCompanyName" runat="server" Text='<%#Eval("CompanyName") %>'
                                OnClick="lnkCompanyName_Click" CommandArgument='<%# Bind("ID") %>' CommandName="Select"
                                Font-Bold="true"></asp:LinkButton>
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
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lnkbtnDelete" runat="server" CausesValidation="True" CommandName="Delete"
                                        ToolTip="Delete" OnClientClick="javascript:if(!confirm('Are you sure you want to delete this address ?')){return false;}"
                                        CommandArgument='<%# Eval("ID")%>' Visible="false">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Portals/3/Images/Delete.png" />
                                    </asp:LinkButton>
                                </td>
                                <td align="right">
                                    <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="True" CommandName="Edit"
                                        ToolTip="Edit" CommandArgument='<%# Eval("ID")%>' Visible="false">
                                        <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Portals/3/Images/Edit.gif" />
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ItemTemplate>
            </telerik:RadListView>
        </td>
    </tr>
</table>
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
                    <asp:TextBox ID="TextBox1" Text="" runat="server"></asp:TextBox>
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
