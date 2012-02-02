<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SNR_Registration.ascx.vb"
    Inherits="SNR_Registration" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadFormDecorator ID="rfd" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
<asp:ValidationSummary ID="vs" runat="server" ValidationGroup="vg" />
<table id="tblRegister" border="0" cellpadding="0" cellspacing="0" width="95%" runat="server">
    <tr>
        <td width="15%">
            <asp:Label ID="lblUserName" Text="User Name" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:TextBox ID="txtUserName" Text="" runat="server" MaxLength="50" ValidationGroup="vg"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUserName"
                Display="None" ErrorMessage="User Name is required." ToolTip="User Name is required."
                ValidationGroup="vg">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revUserName" runat="server" Display="Dynamic"
                ValidationGroup="vg" ControlToValidate="txtUserName" Text="«" Width="100%" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                ErrorMessage="Please Enter Valid Email Address As UserName" />
        </td>
    </tr>
    <tr>
        <td width="15%">
            <asp:Label ID="Label1" Text="First Name" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:TextBox ID="txtFirstName" Text="" runat="server" MaxLength="50" ValidationGroup="vg"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ControlToValidate="txtFirstName"
                ErrorMessage="First Name is required." ToolTip="First Name is required." ValidationGroup="vg">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td width="15%">
            <asp:Label ID="Label2" Text="Last Name" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:TextBox ID="txtLastName" Text="" runat="server" MaxLength="50" ValidationGroup="vg"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" ControlToValidate="txtLastName"
                ErrorMessage="Last Name is required." ToolTip="Last Name is required." ValidationGroup="vg">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td width="15%">
            <asp:Label ID="Label3" Text="Display Name" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:TextBox ID="txtDisplayName" Text="" runat="server"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td width="15%">
            <asp:Label ID="Label6" Text="Email Address" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:TextBox ID="txtEmailAddress" runat="server" Text="" ValidationGroup="vg"></asp:TextBox>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtEmailAddress"
                ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="vg">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td width="15%">
            <asp:Label ID="Label4" Text="Password" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" ValidationGroup="vg"></asp:TextBox>
        </td>
        <td width="60%">
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword"
                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="vg">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regexpName" runat="server" ErrorMessage="Password must be between 8 and 10 characters, contain at least one digit and one alphabetic character, and must not contain special characters."
                Display="None" ValidationGroup="vg" ControlToValidate="txtPassword" ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$" />
        </td>
    </tr>
    <tr>
        <td width="15%">
            <asp:Label ID="Label5" Text="Confirm Password" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:TextBox ID="txtConfirmPassword" Text="" runat="server" TextMode="Password" ValidationGroup="vg"></asp:TextBox>
        </td>
        <td width="60%">
            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="txtPassword"
                Display="None" ControlToValidate="txtConfirmPassword" ErrorMessage="The Password and Confirmation Password must match."
                ValidationGroup="vg"></asp:CompareValidator>
            <%--            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="It must be between 8 and 10 characters, contain at least one digit and one alphabetic character, and must not contain special characters."
                ControlToValidate="txtConfirmPassword" ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$" />
            --%>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblCreateAccountResults" Text="" runat="server"></asp:Label>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td colspan="2">
        </td>
        <td>
            <asp:Button ID="btnRegister" runat="server" Text="Register" CausesValidation="true"
                ValidationGroup="vg" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" />
        </td>
    </tr>
</table>
<asp:Label ID="lblAccountCreated" Text="" runat="server" Visible="false" ForeColor="Black"
    Font-Bold="true"></asp:Label>
