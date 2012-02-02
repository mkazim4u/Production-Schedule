<%@ Control Language="VB" AutoEventWireup="true" CodeFile="PS_CreateEditCustomer.ascx.vb"
    Inherits="PS_CreateEditCustomer" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="DNN" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<link href="module.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    
</style>
<fieldset id="fsMain" runat="server">
    <legend class="fieldsetLegend" id="fslgndMain" runat="server">Create or Edit Customer</legend>
    <br />
    <asp:Label ID="Label1" runat="server" Text="Customer code" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbCustomerCode" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    &nbsp;<asp:RequiredFieldValidator ID="rfvCustomerCode" runat="server" ErrorMessage="required!"
        ControlToValidate="tbCustomerCode"></asp:RequiredFieldValidator>
    <br />
    <asp:Label ID="Label2" runat="server" Text="Customer name" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbCustomerName" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    &nbsp;<asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" ErrorMessage="required!"
        ControlToValidate="tbCustomerName"></asp:RequiredFieldValidator>
    <br />
    <asp:Label ID="Label3" runat="server" Text="Addr 1" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbAddr1" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    &nbsp;<asp:RequiredFieldValidator ID="rfvAddr1" runat="server" ErrorMessage="required!"
        ControlToValidate="tbAddr1"></asp:RequiredFieldValidator>
    <br />
    <asp:Label ID="Label4" runat="server" Text="Addr 2" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbAddr2" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    &nbsp;<asp:LinkButton ID="lnkbtnAddAddr3" runat="server">add Addr 3</asp:LinkButton>
    <br />
    <div id="divAddr3" runat="server" visible="false">
        <asp:Label ID="lblLegendAddr3" runat="server" Text="Addr 3" CssClass="fieldsetLabel" />
        <asp:TextBox ID="tbAddr3" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
        <br />
    </div>
    <asp:Label ID="lblLegendTown" runat="server" Text="Town/City" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbTown" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    &nbsp;<asp:RequiredFieldValidator ID="rfvTown" runat="server" ErrorMessage="required!"
        ControlToValidate="tbTown"></asp:RequiredFieldValidator>
    <br />
    <asp:Label ID="lblLegendPostcode" runat="server" Text="Post code" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbPostcode" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    &nbsp;<asp:LinkButton ID="lnkbtnNonUKAddress" runat="server">non-UK address</asp:LinkButton>
    <br />
    <div id="divCountry" runat="server" visible="false">
        <asp:Label ID="Label6" runat="server" Text="Country" CssClass="fieldsetLabel" />
        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
        &nbsp;<asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="required!"
            InitialValue="0" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
        <br />
    </div>
    <br />
    <div id="divMultipleContacts" runat="server" visible="false">
        <asp:Label ID="lblLegendDummy" runat="server" Text="" CssClass="fieldsetLabel" />
        <asp:LinkButton ID="lnkbtnPrevContact" runat="server"><< prev contact</asp:LinkButton>
        &nbsp;
        <asp:LinkButton ID="lnkbtnNextContact" runat="server">next contact >></asp:LinkButton>
        &nbsp;
        <asp:Button ID="btnRemoveContact" runat="server" Text="Remove Contact" CssClass="fieldsetControlWidth fieldsetLine" />
        <br />
    </div>
    <asp:Label ID="lblLegendContactName" runat="server" Text="Contact Name" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbContactName" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    &nbsp;<asp:Button ID="btnAddAnotherContact" runat="server" Text="Add Another Contact" />
    <br />
    <asp:Label ID="lblLegendTel" runat="server" Text="Contact Phone" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbContactTelephone" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    <br />
    <asp:Label ID="lblLegendMobile" runat="server" Text="Contact Mobile" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbContactMobile" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    <br />
    <asp:Label ID="lblLegendEmailAddr" runat="server" Text="Contact Email Addr" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbContactEmailAddr" runat="server" CssClass="fieldsetControlWidth fieldsetLine" />
    <br />
    <asp:Label ID="lblLegendNotes" runat="server" Text="Contact Notes" CssClass="fieldsetLabel" />
    <asp:TextBox ID="tbContactNotes" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
        Rows="3" TextMode="MultiLine" />
    <br />
    <asp:Label ID="lblDummy1" runat="server" Text="" CssClass="fieldsetLabel" />
    <asp:Button ID="btnUpdateCustomer" runat="server" Text="Update Customer" CssClass="fieldsetControlWidth fieldsetLine" />
    <br />
</fieldset>
<fieldset id="fsImport" runat="server">
    <legend class="fieldsetLegend" id="fslgndSelectStockSystemCustomer" runat="server">Select
        Stock System Customer</legend>
    <br />
    <asp:Label ID="lblLegendStockSystemCustomer" runat="server" Text="Stock system customer"
        CssClass="fieldsetLabel" />
    <telerik:RadComboBox ID="RadComboBoxStockSystemCustomer" runat="server" CssClass="fieldsetControlWidth fieldsetLine"
        AutoPostBack="True" />
    <telerik:RadToolTip ID="RadToolTipStockSystemCustomer" runat="server" TargetControlID="RadComboBoxStockSystemCustomer"
        RelativeTo="Element" HideDelay="3000" HideEvent="LeaveTargetAndToolTip" ShowCallout="true"
        Position="TopRight" Width="250px" Height="100px">
        <div id="divTooltipStockSystemCustomer" class="tooltip">
            <asp:Label ID="Label5" runat="server" Text="Select Customers From Stock Database"
                 />
<%--            <asp:LinkButton ID="lnkbtnCreateNormal" runat="server">Normal</asp:LinkButton>
            - a standard job<br />
            <asp:LinkButton ID="lnkbtnCreateTemplate" runat="server">Template</asp:LinkButton>
            - a job template that can be used to create normal or recurring jobs<br />
            <asp:LinkButton ID="lnkbtnCreateRecurring" runat="server">Recurring</asp:LinkButton>
            - a job that is automatically created to a defined schedule<br />
--%>        </div>
    </telerik:RadToolTip>
    <br />
</fieldset>
<asp:HiddenField ID="hidExternalCustomerKey" runat="server" />
