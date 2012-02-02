Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.PortalSecurity

Partial Class PS_ClipboardNewJobFrom
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(FF_GLOBALS.COPY_JOB_TO_CLIPBOARD) IsNot Nothing Then
            Dim sQueryParams(1) As String
            sQueryParams(0) = "type=Clipboard"
            sQueryParams(1) = "template=" & Convert.ToInt64(Session(FF_GLOBALS.COPY_JOB_TO_CLIPBOARD))
            Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
        End If
        'If DotNetNuke.Services.Personalization.Personalization.GetProfile(DNN.GetModuleID(Me), "Clipboard") Is Nothing Then
        'If DotNetNuke.Services.Personalization.Personalization.GetProfile(DNN.GetModuleID(Me), "Clipboard") Is Nothing Then
        '    lblClipboard.Text = "Cannot create a job. The clipboard is empty."
        'Else
        '    Dim sQueryParams(1) As String
        'sQueryParams(0) = "template=" + DotNetNuke.Services.Personalization.Personalization.GetProfile(DNN.GetModuleID(Me).ToString(), "Clipboard")
        'sQueryParams(1) = DotNetNuke.Services.Personalization.Personalization.GetProfile(FF_GLOBALS.CLIPBOARD_MODULE_ID, "Clipboard")

        'End If
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