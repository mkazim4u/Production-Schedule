Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.PortalSecurity
Imports DotNetNuke.Security.Roles
Imports Telerik.Web.UI
Imports System.IO
Imports System.Collections.Generic

Partial Class PS_JobSummary
    Inherits System.Web.UI.UserControl

    Public Property sJobGuidId() As String
        Get
            Dim o As Object = ViewState("sJobGuidId")
            If o Is Nothing Then
                Return String.Empty
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("sJobGuidId") = Value
        End Set
    End Property

    Public Property pnJodId() As Int32
        Get
            Dim o As Object = ViewState("pnJodId")
            If o Is Nothing Then
                Return String.Empty
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As Int32)
            ViewState("pnJodId") = Value
        End Set
    End Property

    Public Property pdtJod() As DataTable
        Get
            Dim o As Object = ViewState("pdtJod")
            If o Is Nothing Then
                Return Nothing
            End If
            Return DirectCast(o, DataTable)
        End Get
        Set(ByVal Value As DataTable)
            ViewState("pdtJod") = Value
        End Set
    End Property



    Protected Function TryGetJobIDOrGUID() As String
        TryGetJobIDOrGUID = String.Empty
        If Request.Params.Count > 0 Then
            Try
                TryGetJobIDOrGUID = Request.Params("job")
            Catch
            End Try
        End If
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then


            sJobGuidId = Request.QueryString("job")

            Call InitializeControls()

            

        End If
    End Sub

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams), False)
    End Sub

  

    Public Sub InitializeControls()
        Call PopulateDetailView()
        Call PopulateJobStages()
        Call PopulateJobFiles()


    End Sub

    Public Sub PopulateDetailView()

        Dim sqlParamnJobGuidId(0) As SqlParameter
        sqlParamnJobGuidId(0) = New SqlClient.SqlParameter("@JobGuid", SqlDbType.NVarChar)
        sqlParamnJobGuidId(0).Value = sJobGuidId
        Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetJobByJobGuidId", sqlParamnJobGuidId)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            dvJob.DataSource = dt
            dvJob.DataBind()
            pdtJod = dt
            pnJodId = dt.Rows(0)("ID")
        End If

    End Sub

    Public Sub PopulateJobStages()

        If pnJodId > 0 Then

            Dim sqlParamnJobId(0) As SqlParameter
            sqlParamnJobId(0) = New SqlClient.SqlParameter("@JobId", SqlDbType.BigInt)
            sqlParamnJobId(0).Value = pnJodId
            Dim gvjobStates As GridView = dvJob.FindControl("gvJobStates")
            Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllStatesByJobID", sqlParamnJobId)
            If dt IsNot Nothing And dt.Rows.Count <> 0 Then
                gvjobStates.DataSource = dt
                gvjobStates.DataBind()
            End If


        End If

    End Sub

    Protected Sub PopulateJobFiles()

        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        Dim sourceFolder As String = pnJodId.ToString()
        Dim alFiles As New ArrayList
        For Each fName As String In Directory.GetFiles(ps.HomeDirectoryMapPath + FF_GLOBALS.PATH_TO_PS_USER_FILES + sourceFolder)
            alFiles.Add(Path.GetFileName(fName.ToString()))
        Next
        Dim gvjobFiles As GridView = dvJob.FindControl("gvJobFiles")
        gvjobFiles.DataSource = alFiles
        gvjobFiles.DataBind()
        'difFiles.GetFiles("*.gif")
        'Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        'Dim sDestinationFoder As String = pnJobID.ToString()
        'Dim sSourceFolder As String = ps.HomeDirectoryMapPath & FF_GLOBALS.PATH_TO_PS_USER_FILES & sFolderName
        'Dim oFolderController As New DotNetNuke.Services.FileSystem.FolderController
        'Dim objFileController As New Services.FileSystem.FileController
        'Dim objFolderInfo As Services.FileSystem.FolderInfo = oFolderController.GetFolder(ps.PortalId, sSourceFolder, True)
        'If objFolderInfo IsNot Nothing Then
        '    Dim dtFiles As DataTable = objFileController.GetAllFiles()
        '    If dtFiles IsNot Nothing Then
        '        For Each file As Services.FileSystem.FileInfo In dtFiles.Rows
        '            Dim objFileInfo As Services.FileSystem.FileInfo = objFileController.GetFile(file.FileName, ps.PortalId, objFolderInfo.FolderID)
        '            objFileInfo.Folder = sDestinationFoder
        '            If objFileInfo Is Nothing Then
        '                Dim filedId As Integer = objFileController.AddFile(objFileInfo)
        '            End If

        '        Next

        '    End If
        'End If

    End Sub


    Public Function IsValidDate(ByVal dtDate As DateTime) As String
        Dim dt As Date = dtDate.Date
        If dtDate = FF_GLOBALS.BASE_DATE_MONTH_FORMAT Then
            IsValidDate = String.Empty
        Else

            If dt = Date.Today Then
                IsValidDate = "Today"
            ElseIf dt > DateTime.Now.AddDays(7).Date Or dt < DateTime.Now.AddDays(-7).Date Then
                IsValidDate = dt.ToString("d-MMM-yyyy")
            ElseIf dt = DateTime.Now.AddDays(-1).Date Then
                IsValidDate = "Yesterday"
            ElseIf dt = DateTime.Now.AddDays(1).Date Then
                IsValidDate = "Tomorrow"
            Else
                IsValidDate = sWithinCurrentWeek(dtDate)
            End If

        End If


        Return IsValidDate

    End Function

    Private Function sWithinCurrentWeek(ByVal dtDate As DateTime) As String
        sWithinCurrentWeek = String.Empty
        Dim dtTargetDate = dtDate.Date
        Dim nTargetDayOfWeek As Int32 = dtTargetDate.DayOfWeek
        Dim dtToday As DateTime = Date.Today.Date
        Dim nTodayDayOfWeek As Int32 = dtToday.DayOfWeek

        If dtTargetDate > dtToday Then
            If nTargetDayOfWeek <= nTodayDayOfWeek Then
                sWithinCurrentWeek = "Next " & dtTargetDate.ToString("dddd")
            Else
                sWithinCurrentWeek = dtTargetDate.ToString("dddd")
            End If
        Else
            sWithinCurrentWeek = "Last " & dtTargetDate.ToString("dddd")
        End If
    End Function

End Class
