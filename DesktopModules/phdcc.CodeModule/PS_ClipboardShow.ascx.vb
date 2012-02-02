Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.PortalSecurity

Partial Class PS_ClipboardShow
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim nJobId As Int32 = 0
        'If DotNetNuke.Services.Personalization.Personalization.GetProfile(DNN.GetModuleID(Me), "Clipboard") Is Nothing Then
        If DotNetNuke.Services.Personalization.Personalization.GetProfile(FF_GLOBALS.CLIPBOARD_MODULE_ID, "Clipboard") Is Nothing Then
            lblClipboard.Text = "The clipboard is empty."
        Else
            lblClipboard.Text = DotNetNuke.Services.Personalization.Personalization.GetProfile(FF_GLOBALS.CLIPBOARD_MODULE_ID, "Clipboard")
        End If
    End Sub

#Region "Properties"

#End Region

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))
    End Sub

End Class