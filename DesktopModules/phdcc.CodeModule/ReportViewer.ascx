<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ReportViewer.ascx.vb"
    Inherits="ReportViewer" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=5.0.11.510, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls"
    Assembly="DotNetNuke" %>
<link href="module.css" rel="stylesheet" type="text/css" />
<div>
    <telerik:RadAjaxLoadingPanel ID="alpReportViewer" runat="server" Height="75px" MinDisplayTime="5"
        Width="75px">
        <asp:Image ID="Image1" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="rapReportViewer" RequestQueueSize="5" runat="server" Width="100%"
        EnableOutsideScripts="True" HorizontalAlign="NotSet" ScrollBars="None" LoadingPanelID="alpReportViewer">
        <table style="width: 100%" id="Table1" cellspacing="0" cellpadding="0" border="0">
            <tbody>
                <%--                <tr>
                    <td id="tdJobIDCombo" runat="server" visible="true">
                        <asp:Label ID="lblJobID" runat="server" Text="Job ID : "></asp:Label>
                        
                        <telerik:RadComboBox ID="cmbJobID" EmptyMessage="- please select Job ID -" runat="server"
                            AutoCompleteSeparator="true" HighlightTemplatedItems="true" AutoPostBack="True" MarkFirstMatch="true"
                            EnableLoadOnDemand="True" ShowMoreResultsBox="true" EnableVirtualScrolling="true" AllowCustomText="true"
                            Height="100px" >
                        </telerik:RadComboBox>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <telerik:ReportViewer ID="rptViewer" runat="server" Height="600px" Width="100%">
                        </telerik:ReportViewer>
<%--                        <telerik:ReportViewer >
                        </telerik:ReportViewer>
--%>                    </td>
                </tr>
            </tbody>
        </table>
    </telerik:RadAjaxPanel>
</div>
