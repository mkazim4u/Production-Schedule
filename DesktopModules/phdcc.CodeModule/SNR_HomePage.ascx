<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SNR_HomePage.ascx.vb"
    Inherits="SNR_HomePage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    .radiobutton
    {
        vertical-align: middle;
    }
</style>
<table width="100%">
    <tr>
        <td align="center">
            <asp:UpdateProgress ID="UpdateProgressHome" runat="server">
                <ProgressTemplate>
                    <div align="center" style="font-weight: bold">
                        <asp:Image ID="imgAjaxImage" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/ClockAjaxLoading.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </td>
    </tr>
</table>
<table width="100%">
    <tr>
        <td>
            <asp:UpdatePanel runat="server" ID="UpdatePanelHome" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rbAllCountries" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="rbEurope" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="rbAsia" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="rbAmerica" EventName="CheckedChanged" />
                </Triggers>
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td width="25%">
                                <asp:RadioButton ID="rbAllCountries" runat="server" Checked="true" Text="All Countries"
                                    TabIndex="0" GroupName="Country" AutoPostBack="true"
                                    OnCheckedChanged="rbAllCountries_CheckChanged" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rbEurope" runat="server" Text="Europe, CIS, Africa" OnCheckedChanged="rbEurope_CheckChanged"
                                    TabIndex="1" GroupName="Country" AutoPostBack="true" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rbAsia" runat="server" Text="Middle-east, Asia, Australasia"
                                    TabIndex="2" GroupName="Country" AutoPostBack="true" OnCheckedChanged="rbAsia_CheckChanged" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rbAmerica" runat="server" Text="The Americas" OnCheckedChanged="rbAmerica_CheckChanged"
                                    TabIndex="3" GroupName="Country" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <b>Select Your Country :</b> &nbsp;
                                <telerik:RadComboBox ID="rcbCountry" runat="server" Skin="Default" Height="100px"
                                    Width="200px" DataTextField="CountryName" DataValueField="CountryKey" AllowCustomText="true"
                                    AutoPostBack="true" MarkFirstMatch="True" HighlightTemplatedItems="True" DropDownWidth="200px"
                                    OnSelectedIndexChanged="rcbCountry_SelectedIndexChanged" EmptyMessage="- Select Country -">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
