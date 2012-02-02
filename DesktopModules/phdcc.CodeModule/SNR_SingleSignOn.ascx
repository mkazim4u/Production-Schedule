<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SNR_SingleSignOn.ascx.vb"
    Inherits="SNR_SingleSignOn" %>
<%--Username:
<asp:TextBox ID="UserName" runat="server"></asp:TextBox>
Password:
<asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
<br />
<asp:CheckBox ID="RememberMe" runat="server" Text="Remember Me" />
<br />
<asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" />
<br />
<asp:Label ID="InvalidCredentialsMessage" runat="server" ForeColor="Red" Text="Your username or password is invalid. Please try again." 

 Visible="False"></asp:Label>--%>
<asp:Panel ID="pnlLogin" runat="server">
    <asp:Login ID="Login" runat="server" Height="199px" Width="340px">
    </asp:Login>
</asp:Panel>
