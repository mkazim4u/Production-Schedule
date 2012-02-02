Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.PortalSecurity
Imports System.IO

Partial Class PS_Cleanup
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
        End If
    End Sub

#Region "Helper functions (HideAllPanels, ...)"

    Protected Sub HideAllPanels()
    End Sub

#End Region

#Region "Properties"

#End Region

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))
    End Sub

    Protected Sub btnCleanup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCleanup.Click
        Dim sSQL As String = String.Empty
        Dim sFolderRoot As String = DNN.GetPMB(Me).PortalSettings.HomeDirectoryMapPath & FF_GLOBALS.PATH_TO_PS_USER_FILES
        Dim oDT As DataTable = DNNDB.Query("SELECT [id] FROM FF_Job WHERE IsCreated = 0")
        For Each dr As DataRow In oDT.Rows
            Dim nJobId As Integer = dr(0)
            Dim sDirectory As String = sFolderRoot & nJobId.ToString
            If Directory.Exists(sDirectory) Then
                RmDir(sDirectory)
                Call Log("Removed " & sDirectory)
            End If
            Call DNNDB.Query("DELETE FROM FF_JobState WHERE JobId = " & nJobId)
            Call DNNDB.Query("DELETE FROM FF_Note WHERE JobId = " & nJobId)
            Call DNNDB.Query("DELETE FROM FF_Event WHERE JobId = " & nJobId)
            Call DNNDB.Query("DELETE FROM FF_Job WHERE [id] = " & nJobId)
            Call Log("Removed references to " & nJobId)
        Next
    End Sub

    Protected Sub Log(ByVal sMessage As String)
        tbLog.Text += sMessage & Environment.NewLine
    End Sub

End Class