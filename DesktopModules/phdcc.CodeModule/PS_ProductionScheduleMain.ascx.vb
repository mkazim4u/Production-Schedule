Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.PortalSecurity
Imports DotNetNuke.Services.Personalization
Imports DotNetNuke.Entities.Users
Imports Telerik.Web.UI

Partial Class PS_ProductionScheduleMain

    Inherits System.Web.UI.UserControl


    Const QUERY_NON_TEMPLATE_JOBS As String = " ffj.IsCreated = 1 AND ffj.IsDeleted = 0 AND ffj.IsTemplate = 0 AND ffj.IsCancelled = 0"
    Const QUERY_TEMPLATE_JOBS As String = " ffj.IsCreated = 1 AND ffj.IsDeleted = 0 AND ffj.IsTemplate = 1 "
    Const QUERY_CANCELLED_JOBS As String = " ffj.IsCreated = 1 AND ffj.IsDeleted = 0 AND ffj.IsCancelled = 1 AND ffj.IsTemplate = 0 "
    Const QUERY_CREATED As String = " ffj.IsCreated = 1 AND ffj.IsDeleted = 0 "
    Const SORT_FIELD_ID As String = " ffj.ID "
    Const SORT_FIELD_DEADLINE As String = " ffj.DeadlineOn "
    Const DIRECTION_DESC As String = " DESC"
    Const DIRECTION_ASC As String = " ASC"
    Const USER_PREFERENCES As String = "UserPreferences"
    Const QUERY_ACCOUNT_HANDLER As String = " (select Users.Username from Users where Users.UserID = AccountHandlerKey ) as 'AccountHandler' "
    Private INNER_JOIN_USER_JOB_MAPPING As String = " Inner join FF_UserJobMapping UJP ON ffj.ID = UJP.JobID "
    Private SHOW_RECENTLY_COMPLETED_JOBS_QUERY As String = " ffj.CompletedOn >= '" & DateTime.Now.AddDays(-4).Date.ToString("dd-MMM-yyyy") & "' AND ffj.CompletedOn <= '" & DateTime.Now.AddDays(+1).Date.ToString("dd-MMM-yyyy") & "' "

    Private Shared _userPreferences As FF_UserPreferences

    Private Shared myInstance As PS_ProductionScheduleMain

    Private Shared m_session_prefix As String = "ProductionScheduleMain_"
    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private ri As New DotNetNuke.Security.Roles.RoleInfo
    Private rgi As New DotNetNuke.Security.Roles.RoleGroupInfo
    Private uc As New DotNetNuke.Entities.Users.UserController
    Private userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
    Private userInfo As DotNetNuke.Entities.Users.UserInfo

    
    Property psQuery() As String
        Get
            Dim o As Object = ViewState("JobQuery")
            If o Is Nothing Then
                Return QUERY_NON_TEMPLATE_JOBS
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("JobQuery") = Value
        End Set
    End Property

    Property psSortField() As String
        Get
            Dim o As Object = ViewState("SortField")
            If o Is Nothing Then
                Return SORT_FIELD_ID
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("SortField") = Value
        End Set
    End Property

    Property psDirection() As String
        Get
            Dim o As Object = ViewState("Direction")
            If o Is Nothing Then
                Return DIRECTION_DESC
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("Direction") = Value
        End Set
    End Property

    Property psSearchText() As String
        Get
            Dim o As Object = ViewState("psSearchText")
            If o Is Nothing Then
                Return String.Empty
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("psSearchText") = Value
        End Set
    End Property
    Property pbIndependentSearch() As Boolean
        Get
            Dim o As Object = ViewState("pbIndependentSearch")
            If o Is Nothing Then
                Return True
            End If
            Return CBool(o)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("pbIndependentSearch") = Value
        End Set
    End Property
    Property pbShowUncompletedJobsOnly() As Boolean
        Get
            Dim o As Object = ViewState("ShowUncompletedJobsOnly")
            If o Is Nothing Then
                Return True
            End If
            Return CBool(o)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("ShowUncompletedJobsOnly") = Value
        End Set
    End Property

    Property pbShowCompletedJobsOnly() As Boolean
        Get
            Dim o As Object = ViewState("ShowCompletedJobsOnly")
            If o Is Nothing Then
                Return True
            End If
            Return CBool(o)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("ShowCompletedJobsOnly") = Value
        End Set
    End Property

    Property pbShowRecentlyCompletedJobsOnly() As Boolean
        Get
            Dim o As Object = ViewState("ShowRecentlyCompletedJobsOnly")
            If o Is Nothing Then
                Return True
            End If
            Return CBool(o)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("ShowRecentlyCompletedJobsOnly") = Value
        End Set
    End Property

    Property pbIncludeTemplates() As Boolean
        Get
            Dim o As Object = ViewState("pbIncludeTemplates")
            If o Is Nothing Then
                Return True
            End If
            Return CBool(o)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("pbIncludeTemplates") = Value
        End Set
    End Property

    Property pbIncludeCancelled() As Boolean
        Get
            Dim o As Object = ViewState("cbIncludeCancelled")
            If o Is Nothing Then
                Return True
            End If
            Return CBool(o)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("cbIncludeCancelled") = Value
        End Set
    End Property

    Public Property pbiCurrentUserId() As Int32
        Get
            Dim o As Object = ViewState("iCurrentUserId")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("iCurrentUserId") = Value
        End Set
    End Property

    Public Property pbRowId() As Int32
        Get
            Dim o As Object = ViewState("RowId")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("RowId") = Value
        End Set
    End Property

    Public Property psJobGuidId() As String
        Get
            Dim o As Object = ViewState("psJobGuidId")
            If o Is Nothing Then
                Return 0
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("psJobGuidId") = Value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            FF_GLOBALS.bDebugMode = False
            _userPreferences = New FF_UserPreferences
            pbiCurrentUserId = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            'Call IsHighPrivilege()
            Call InitJobSelectionControlsVisibility()
            If Not LoadUserSettings() Then
                Call SetDefaultUserPreferences()
            Else
                Call SetUserPreferences()
            End If

            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()



        End If
    End Sub
    Protected Sub LoadUserRights()



    End Sub

    'Protected Function IsHighPrivilege() As Boolean

    '    If Not (DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.IsSuperUser) Then

    '        If GetProfilePropertyValue(pbiCurrentUserId, FF_GLOBALS.PS_PRIVILEGE_PROPERTY) Then
    '            IsHighPrivilege = True
    '            LoadAdminScreen()
    '        Else
    '            LoadUserScreen()
    '            IsHighPrivilege = False
    '        End If

    '    Else
    '        IsHighPrivilege = True
    '    End If

    'End Function

    Protected Function IsHighPrivilege() As Boolean
        IsHighPrivilege = False
        If Not (DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.IsSuperUser) Then
            Dim alRole As ArrayList = rc.GetUserRoles(DNN.GetPMB(Me).PortalId, pbiCurrentUserId)
            For Each roleInfo As DotNetNuke.Security.Roles.RoleInfo In alRole
                If roleInfo.RoleName = FF_GLOBALS.ROLE_MANAGER Or roleInfo.RoleName = FF_GLOBALS.ROLE_SUPER_USER Or roleInfo.RoleName = FF_GLOBALS.ROLE_ACCOUNT_HANDLER Then
                    IsHighPrivilege = True
                End If
            Next
        Else
            IsHighPrivilege = True
        End If

    End Function

    Protected Sub LoadUserScreen()

    End Sub

    Protected Sub LoadAdminScreen()

    End Sub
    Protected Sub SetDefaultUserPreferences()

        pbShowCompletedJobsOnly = False
        pbShowRecentlyCompletedJobsOnly = True
        pbShowUncompletedJobsOnly = True
        pbIndependentSearch = True
        pbIncludeCancelled = False
        pbIncludeTemplates = False

    End Sub

    Protected Sub SetUserPreferences()

        If _userPreferences IsNot Nothing Then
            pbShowUncompletedJobsOnly = _userPreferences.pShowUnCompletedJobs
            pbShowRecentlyCompletedJobsOnly = _userPreferences.pShowRecentlyCompletedJobs
            pbShowCompletedJobsOnly = _userPreferences.pShowCompletedJobs
            pbIncludeCancelled = _userPreferences.pShowCancelledJobs
            pbIncludeTemplates = _userPreferences.pShowTemplateJobs
        End If

    End Sub

    Protected Sub SetAllUnCheck()
        pbShowCompletedJobsOnly = False
        pbShowRecentlyCompletedJobsOnly = False
        pbShowUncompletedJobsOnly = False
        pbIncludeCancelled = False
        pbIncludeTemplates = False

    End Sub
    Protected Function LoadUserSettings() As Boolean

        Dim bFound As Boolean = False
        Dim persController As New PersonalizationController

        _userPreferences = Personalization.GetProfile(DNN.GetModuleID(Me).ToString(), USER_PREFERENCES)
        If _userPreferences IsNot Nothing Then
            bFound = True
        End If

        Return bFound
    End Function

    Protected Sub SetUserSettings()

        Dim persController As New PersonalizationController

        If _userPreferences IsNot Nothing Then
            Personalization.SetProfile(DNN.GetModuleID(Me).ToString(), USER_PREFERENCES, _userPreferences)
        End If



    End Sub

    Protected Function IsAllUncheck() As Boolean
        Dim check As Boolean = False
        If (pbIncludeCancelled = False And pbIncludeTemplates = False And pbShowCompletedJobsOnly = False And pbShowRecentlyCompletedJobsOnly = False And pbShowUncompletedJobsOnly = False) Then
            check = True
        End If

        Return check

    End Function

    Protected Sub BindJobsGridView()
        If psQuery <> String.Empty Then

            If IsHighPrivilege() Then
                INNER_JOIN_USER_JOB_MAPPING = String.Empty
            End If
            Dim sSQL As String
            sSQL = "SELECT distinct ffj.id 'JobKey', JobName, CustomerCode, AccountHandlerKey, CollateralDueOn, DeadlineOn, CompletedOn, dbo.FF_JobIsComplete(ffj.ID) 'IsCompleted', ffj.IsCancelled, ffj.CreatedOn, ffj.CreatedBy," & QUERY_ACCOUNT_HANDLER & "FROM FF_Job ffj left outer JOIN FF_Customer ffc ON ffj.CustomerKey = ffc.[id]" & INNER_JOIN_USER_JOB_MAPPING & " WHERE " & psQuery & " ORDER BY " & psSortField & psDirection
            Dim oDT As DataTable = DNNDB.Query(sSQL)
            If oDT IsNot Nothing Then
                gvJobs.DataSource = oDT
                gvJobs.DataBind()
                If oDT.Rows.Count <> 0 Then
                    btnPrintJobList.Visible = True
                    'Session.Add(m_session_prefix + "JobListReport", oDT)
                Else
                    'Dim dr As DataRow = oDT.NewRow() ' for empty gridview'
                    'dr("jobName") = "empty"
                    'dr("accounthandlerkey") = -1
                    'dr("IsCancelled") = 1
                    'dr("IsCompleted") = 1
                    ''dr("IsDeleted") = 1
                    'dr("CreatedOn") = DateTime.Now
                    'dr("CollateralDueOn") = DateTime.Now
                    'dr("DeadlineOn") = DateTime.Now
                    'dr("CompletedOn") = DateTime.Now
                    'dr("CreatedBy") = 1
                    'oDT.Rows.Add(dr)
                    'gvJobs.DataSource = oDT
                    'gvJobs.DataBind()
                    'gvJobs.Rows(0).Visible = False
                    'gvJobs.Rows(0).Controls.Clear()

                End If
            End If
        Else
            gvJobs.DataSource = Nothing
            gvJobs.DataBind()
            btnPrintJobList.Visible = False
        End If
    End Sub

    

    Protected Sub BindJobStateGridView(ByVal jobID As Integer)
        Dim gvJobStates As GridView
        gvJobStates = FindControlInHeirarchy(gvJobs, "gvJobStates")
        If gvJobStates IsNot Nothing Then
            gvJobStates.DataSource = FF_JobState.GetJobStages(jobID)
            gvJobStates.DataBind()
        End If
    End Sub

    Protected Sub InitJobSelectionControlsVisibility()
        tdCustomerSelector.Visible = False
        tdDateSelector.Visible = False
        tdGoButton.Visible = False
        'tdPrintJobButton.Visible = False
    End Sub

    Protected Sub SetQuery()

        Dim sCustomerSearchQuery As String = String.Empty
        Dim sDateSearchQuery As String = String.Empty
        Dim sAccountHandlerSearchQuery As String = String.Empty
        Dim sUserJobs As String = String.Empty

        If Not (IsHighPrivilege()) Then
            sUserJobs = " And UJP.UserId = " & pbiCurrentUserId
            'Else
            '    INNER_JOIN_USER_JOB_MAPPING = String.Empty
        End If


        If pbIndependentSearch And psSearchText <> String.Empty Then
            psQuery = QUERY_NON_TEMPLATE_JOBS + psSearchText
            Exit Sub
        End If

        If IsAllUncheck() Then
            psQuery = String.Empty
            Exit Sub
        End If

        If rcbCustomer.SelectedIndex > 0 Then
            sCustomerSearchQuery = " AND CustomerKey = " & rcbCustomer.SelectedValue
        End If

        If rcbAccountHandler.SelectedIndex > 0 Then
            sAccountHandlerSearchQuery = " AND AccountHandlerKey = " & rcbAccountHandler.SelectedValue

        End If


        If cbByDate.Checked Then
            Dim sDateClause As String = DateClause()
            If (sDateClause = "UNSET" Or sDateClause = "INVALID") Then
                sDateSearchQuery = String.Empty
            Else
                sDateSearchQuery = sDateClause
            End If
        End If

        If pbIncludeTemplates Then
            psQuery = QUERY_TEMPLATE_JOBS + sCustomerSearchQuery + sDateSearchQuery + sAccountHandlerSearchQuery
            pbShowCompletedJobsOnly = False
            pbShowRecentlyCompletedJobsOnly = False
            pbShowUncompletedJobsOnly = False
            pbIncludeCancelled = False

        End If

        If pbIncludeCancelled Then

            psQuery = QUERY_CANCELLED_JOBS + sCustomerSearchQuery + sDateSearchQuery + sAccountHandlerSearchQuery
            pbShowCompletedJobsOnly = False
            pbShowRecentlyCompletedJobsOnly = False
            pbShowUncompletedJobsOnly = False
            pbIncludeTemplates = False

        End If


        If pbShowUncompletedJobsOnly Then
            psQuery = " (dbo.FF_JobIsComplete(ffj.ID) = 0  AND " + QUERY_NON_TEMPLATE_JOBS + sCustomerSearchQuery + sAccountHandlerSearchQuery + sDateSearchQuery + psSearchText + sUserJobs + ")"

        End If

        If pbShowRecentlyCompletedJobsOnly Then
            If pbShowUncompletedJobsOnly Then
                psQuery += " OR (dbo.FF_JobIsComplete(ffj.ID) = 1 AND " + SHOW_RECENTLY_COMPLETED_JOBS_QUERY + " AND " + QUERY_NON_TEMPLATE_JOBS + sCustomerSearchQuery + sAccountHandlerSearchQuery + sDateSearchQuery + psSearchText + sUserJobs + ")"
            Else
                psQuery = "(dbo.FF_JobIsComplete(ffj.ID) = 1 AND " + SHOW_RECENTLY_COMPLETED_JOBS_QUERY + " AND " + QUERY_NON_TEMPLATE_JOBS + sCustomerSearchQuery + sAccountHandlerSearchQuery + sDateSearchQuery + psSearchText + sUserJobs + ")"
            End If
        End If

        If pbShowCompletedJobsOnly Then
            If pbShowRecentlyCompletedJobsOnly Or pbShowUncompletedJobsOnly Then
                psQuery += " OR (dbo.FF_JobIsComplete(ffj.ID) = 1 AND " + QUERY_NON_TEMPLATE_JOBS + sCustomerSearchQuery + sAccountHandlerSearchQuery + sDateSearchQuery + psSearchText + sUserJobs + ")"
            Else
                psQuery = "(dbo.FF_JobIsComplete(ffj.ID) = 1 AND " + QUERY_NON_TEMPLATE_JOBS + sCustomerSearchQuery + sAccountHandlerSearchQuery + sDateSearchQuery + psSearchText + sUserJobs + ")"
            End If
        End If


        gvJobs.PageIndex = 0
        psSortField = SORT_FIELD_ID

    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Call SetQuery()
        Call BindJobsGridView()
        Call HighLightUserJobStages()
    End Sub

    Protected Sub btnGoSearchByJobName_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        If psSearchText = String.Empty Then
            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()
            Exit Sub
        End If

        Dim nJobID As Integer
        If Integer.TryParse(psSearchText.Trim(), nJobID) Then
            psSearchText = " AND ffj.id = " & nJobID & ""
            Call SetQuery()
            psSearchText = String.Empty
        Else

            psSearchText = " AND ffj.JobName like '%" & psSearchText & "%'"
            Call SetQuery()
            psSearchText = String.Empty
        End If

        Call BindJobsGridView()
        Call HighLightUserJobStages()
    End Sub

    Protected Sub tbSearchByJobName_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim tbSearchByJobName As RadTextBox = sender
        psSearchText = tbSearchByJobName.Text.Trim()
    End Sub

    Protected Sub ClearDateAndCustomer()
        cbByCustomer.Checked = False
        cbByDate.Checked = False
        'tdPrintJobButton.Visible = False
        tdGoButton.Visible = False
    End Sub

    'Protected Sub lnkPrintJob_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Call SetQuery()
    '    Call BindJobsGridView()
    '    Call HighLightUserJobStages()
    'End Sub

    Protected Sub lnkbtnEditJob_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender
        Dim sQueryParams(1) As String
        sQueryParams(0) = "job=" & lb.CommandArgument
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
    End Sub

    Protected Sub lnkbtnPrintJob_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dim sQueryParams(0) As String
        'sQueryParams(0) = "ReportName=" + FF_GLOBALS.REPORT_JOB

        Dim lb As LinkButton = sender
        Session.Add(FF_GLOBALS.SESSION_JOB_ID, lb.CommandArgument)
        Dim printScript = String.Format("window.open('Reports.aspx?ReportName=" & FF_GLOBALS.REPORT_JOB & "')")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ReportPrint", printScript, True)
        'Call NavigateTo(FF_GLOBALS.PAGE_REPORTS, sQueryParams)
    End Sub


    Protected Function GetUserInitials(ByVal UserId As Integer) As String
        GetUserInitials = DNN.GetUserInitials(Me, UserId)
    End Function
    Protected Sub gvJobs_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvJobs.PageIndexChanging
        gvJobs.PageIndex = e.NewPageIndex
        Call BindJobsGridView()
        Call HighLightUserJobStages()

    End Sub

    Protected Sub GetAuditEventForStages()



    End Sub

    Protected Sub HighLightUserJobStages()

        Dim sqlParamCollection(1) As SqlParameter
        Dim sqlParamForJobStageEvent(0) As SqlParameter

        For Each gvRow As GridViewRow In gvJobs.Rows

            sqlParamCollection(0) = New SqlClient.SqlParameter("@UserId", SqlDbType.BigInt)
            sqlParamCollection(0).Value = pbiCurrentUserId
            sqlParamCollection(1) = New SqlClient.SqlParameter("@JobId", SqlDbType.BigInt)

            sqlParamCollection(1).Value = gvRow.Cells(1).Text

            Dim dtGetUserStages As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllStagesForUser", sqlParamCollection)


            Dim gvJobStates As GridView = gvRow.FindControl("gvJobStates")

            If gvJobStates IsNot Nothing Then

                For Each gvStageRow As GridViewRow In gvJobStates.Rows

                    Dim hidJobStateKey As HiddenField = gvStageRow.FindControl("hidJobStateKey")

                    If dtGetUserStages IsNot Nothing And dtGetUserStages.Rows.Count <> 0 Then
                        For Each dr As DataRow In dtGetUserStages.Rows

                            If hidJobStateKey.Value = dr("JobStateId") Then
                                Dim lblJobStateName As Label = gvStageRow.FindControl("lblJobStateName")
                                lblJobStateName.Font.Italic = True
                                lblJobStateName.Font.Bold = True
                            End If
                        Next

                    End If


                    sqlParamForJobStageEvent(0) = New SqlClient.SqlParameter("@JobStateId", SqlDbType.BigInt)
                    sqlParamForJobStageEvent(0).Value = hidJobStateKey.Value

                    Dim dtGetStagesAudit As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllEventsForJobStage", sqlParamForJobStageEvent)

                    If dtGetStagesAudit IsNot Nothing And dtGetStagesAudit.Rows.Count <> 0 Then

                        For Each dr As DataRow In dtGetStagesAudit.Rows

                            If hidJobStateKey.Value = dr("JobStateKey") Then
                                Dim lblJobStateName As Label = gvStageRow.FindControl("lblUserName")

                                Dim sMark As String = dr("ChangeDetail").ToString()
                                If sMark.Contains("True") Then
                                    sMark = FF_GLOBALS.JOB_STATE_MARK_COMPLETED
                                ElseIf sMark.Contains("False") Then
                                    sMark = FF_GLOBALS.JOB_STATE_MARK_UNCOMPLETED
                                End If
                                If lblJobStateName.Text.Length = 0 Then
                                    lblJobStateName.Text = dr("UserName") + " (" + sMark + ")"
                                Else
                                    lblJobStateName.Text = lblJobStateName.Text + ", " + dr("UserName") + " (" + sMark + ")"
                                End If


                            End If

                        Next

                    End If

                Next

            End If

        Next


    End Sub

    Protected Sub gvJobStates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim gvr As GridViewRow = e.Row
        If gvr.RowType = DataControlRowType.DataRow Then
            Dim cbIsCompleted As CheckBox
            cbIsCompleted = gvr.FindControl("cbIsCompleted")
            If cbIsCompleted.Checked Then
                gvr.BackColor = Color.SpringGreen
            Else
                gvr.BackColor = Color.White
            End If

        End If


    End Sub

    Protected Sub gvJobStates_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.Header Then

            Dim gvJobStates As GridView = sender

            Dim rowHeader As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
            rowHeader.ID = "gvJSheader1"
            rowHeader.BackColor = Color.Yellow
            Dim headerCell As New TableHeaderCell()
            headerCell.Text = pbRowId.ToString()
            headerCell.ColumnSpan = 3
            rowHeader.Cells.Add(headerCell)
            rowHeader.Visible = True
            gvJobStates.Controls(0).Controls.AddAt(0, rowHeader)

        End If
    End Sub


    Protected Sub gvJobs_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvJobs.RowDataBound




        Dim gvr As GridViewRow = e.Row

        If gvr.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(3).Attributes.Add("onmouseover", "this.style.backgroundColor='yellow';this.style.cursor='cursor';")
            e.Row.Cells(2).Attributes.Add("onmouseover", "this.style.backgroundColor='yellow';this.style.cursor='cursor';")

            Dim lbl As Label
            lbl = gvr.FindControl("lblJobInfo")
            lbl.Text = ""
            Dim hid As HiddenField
            hid = gvr.FindControl("hidIsCompleted")


            If hid.Value Then
                gvr.BackColor = Color.SpringGreen
                e.Row.Cells(2).Attributes.Add("onmouseout", "this.style.backgroundColor='springgreen';this.style.cursor='cursor';")
                e.Row.Cells(3).Attributes.Add("onmouseout", "this.style.backgroundColor='springgreen';this.style.cursor='cursor';")
            Else
                e.Row.Cells(2).Attributes.Add("onmouseout", "this.style.backgroundColor='white';this.style.cursor='cursor';")
                e.Row.Cells(3).Attributes.Add("onmouseout", "this.style.backgroundColor='white';this.style.cursor='cursor';")
            End If


            hid = gvr.FindControl("hidIsCancelled")
            If hid.Value <> String.Empty Then
                If hid.Value Then
                    gvr.BackColor = Color.Tomato
                End If
            End If

            Dim btnCopyJobToClipboard As Button = gvr.FindControl("btnCopyJobToClipboard")
            Dim btnNewJobFromClipboard As Button = gvr.FindControl("btnNewJobFromClipboard")
            If Not IsHighPrivilege() Then

                btnCopyJobToClipboard.Visible = False
                btnNewJobFromClipboard.Visible = False

            End If




            '''''''''''''''''''' Binding A nested Grid '''''''''''''''''''''''''''
            Dim JobId As Integer = CType(e.Row.DataItem, DataRowView).Row("JobKey")
            pbRowId = JobId
            'BindJobStateGridView(JobId) ''''''''''''' it was causing a great problem always remember
            Dim gvJobStates As GridView
            gvJobStates = gvr.FindControl("gvJobStates")
            Dim radToolTipUsers As RadToolTip
            radToolTipUsers = gvr.FindControl("RadToolTipUsers")
            If gvJobStates IsNot Nothing Then
                Dim dt As DataTable = FF_JobState.FetchJobStages(JobId)
                If dt IsNot Nothing <> dt.Rows.Count <> 0 Then
                    gvJobStates.DataSource = dt
                    gvJobStates.DataBind()
                    'gvJobStates.HeaderRow.Cells(0).Text = JobId
                Else
                    radToolTipUsers.Text = "No Resources Assigned To This Stage"
                End If


            End If

            Dim lnkbtnPrintJob As LinkButton = gvr.FindControl("lnkbtnPrintJob")
            If lnkbtnPrintJob IsNot Nothing Then
                'lnkbtnPrintJob.Attributes.Add("onclick", "window.open('Reports.aspx')")
                'lnkbtnPrintJob.Attributes.Add("onclick", "window.open('ReportViewer.aspx?JobID=" + JobId.ToString() + "&ReportName=" + FF_GLOBALS.REPORT_JOB + "');")
                'lnkbtnPrintJob.Attributes.Add("onclick", "window.open('Reports.aspx?ReportName=" + FF_GLOBALS.REPORT_JOB + "');")
            End If



        ElseIf gvr.RowType = DataControlRowType.Header Then            '''''''''''''' setting this values for Psquery ''''''''''''''''''''''


            e.Row.Cells(3).Attributes.Add("onmouseover", "this.style.backgroundColor='yellow';this.style.cursor='pointer';")
            e.Row.Cells(3).Attributes.Add("onmouseout", "this.style.backgroundColor='white';this.style.cursor='cursor';")

            Dim chkShowUncompletedJobs As CheckBox = gvr.FindControl("chkShowUncompletedJobs")
            Dim chkShowCompletedJobs As CheckBox = gvr.FindControl("chkShowCompletedJobs")
            Dim chkShowRecentlyCompletedJobs As CheckBox = gvr.FindControl("chkShowRecentlyCompletedJobs")
            Dim chkIndependentSearch As CheckBox = gvr.FindControl("chkIndependentSearch")

            Dim cbIncludeTemplates As CheckBox = gvr.FindControl("cbIncludeTemplates")
            Dim cbIncludeCancelled As CheckBox = gvr.FindControl("cbIncludeCancelled")
            'Dim cbIncludeNotCancelled As CheckBox = gvr.FindControl("cbIncludeNotCancelled")

            Dim tbSearchByJobName As RadTextBox = gvr.FindControl("tbSearchByJobName")
            Dim btnGoSearchByJobName As Button = gvr.FindControl("btnGoSearchByJobName")


            If tbSearchByJobName IsNot Nothing And btnGoSearchByJobName IsNot Nothing Then
                tbSearchByJobName.Attributes.Add("onkeypress", "return clickButton(event,'" + btnGoSearchByJobName.ClientID + "')")
            End If


            chkShowCompletedJobs.Checked = pbShowCompletedJobsOnly
            chkShowRecentlyCompletedJobs.Checked = pbShowRecentlyCompletedJobsOnly
            chkShowUncompletedJobs.Checked = pbShowUncompletedJobsOnly
            chkIndependentSearch.Checked = pbIndependentSearch

            cbIncludeTemplates.Checked = pbIncludeTemplates
            cbIncludeCancelled.Checked = pbIncludeCancelled


        End If

    End Sub

    Protected Function TranslateNullDate(ByVal dtDate As DateTime) As String
        If dtDate = FF_GLOBALS.BASE_DATE Then
            TranslateNullDate = "not set"
        Else
            TranslateNullDate = dtDate.ToString("d-MMM")
        End If
    End Function

    Public Function TranslateDate(ByVal dtDate As DateTime, ByVal DataItem As Object) As String
        Dim nId As Integer = DataBinder.Eval(DataItem, "JobKey")
        If FF_JobState.IsJobComplete(nId) Then
            TranslateDate = TranslateDate(dtDate, False)
        Else
            TranslateDate = TranslateDate(dtDate, True)
        End If
    End Function

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
    Public Function TranslateDate(ByVal dtDate As DateTime, ByVal bHighlightPassedDate As Boolean) As String
        Dim dt As Date = dtDate.Date
        Dim sTranslatedDate As String = String.Empty
        Dim dow As Int32 = dtDate.DayOfWeek
        If dtDate = FF_GLOBALS.BASE_DATE Then
            sTranslatedDate = "not set"
        Else
            If dt = Date.Today Then
                sTranslatedDate = "Today"
            ElseIf dt > DateTime.Now.AddDays(7).Date Or dt < DateTime.Now.AddDays(-7).Date Then
                sTranslatedDate = dt.ToString("d-MMM-yyyy")
            ElseIf dt = DateTime.Now.AddDays(-1).Date Then
                sTranslatedDate = "Yesterday"
            ElseIf dt = DateTime.Now.AddDays(1).Date Then
                sTranslatedDate = "Tomorrow"
            Else
                sTranslatedDate = sWithinCurrentWeek(dtDate)
            End If
        End If
        TranslateDate = sTranslatedDate
        If bHighlightPassedDate AndAlso dt < Date.Today Then
            TranslateDate = "<font color='red'>" & sTranslatedDate & "</font>"
        End If
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

    Protected Sub PopulateAccountHandler()

        FF_GLOBALS.PopulateAccountHandlerCombobox(rcbAccountHandler, Me)

    End Sub

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))
    End Sub

    Protected Sub ddlJobCount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlJobCount.SelectedIndexChanged
        gvJobs.PageSize = ddlJobCount.SelectedValue
        gvJobs.PageIndex = 0
        Call BindJobsGridView()
        Call HighLightUserJobStages()
    End Sub

    Protected Sub cbByDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbByDate.CheckedChanged
        Dim cb As CheckBox = sender
        If cb.Checked Then
            rdpFromDate.SelectedDate = Nothing
            rdpToDate.SelectedDate = Nothing
            tdDateSelector.Visible = True
            tdGoButton.Visible = True
            'tdPrintJobButton.Visible = True
        Else
            tdDateSelector.Visible = False
            tdGoButton.Visible = False
            'tdPrintJobButton.Visible = False
            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()
        End If
    End Sub
    Protected Sub btnPrintJobList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintJobList.Click

        PrintJobList()

    End Sub

    Protected Sub PrintJobList()

        'Dim sQueryString As String = String.Empty
        'Dim sCompleted As String = "Completed="
        'Dim sUnCompleted As String = "UnCompleted="
        'Dim sRecentlyCompleted As String = "RecentlyCompleted="
        'Dim sShowAll As String = "Completed=-1"

        'If pbShowCompletedJobsOnly Then
        '    sQueryString = sCompleted + "1"
        'Else
        '    sQueryString = sCompleted + "0"
        'End If
        'If pbShowUncompletedJobsOnly Then
        '    sQueryString += "&" + sUnCompleted + "0"
        'Else
        '    sQueryString += "&" + sUnCompleted + "1"
        'End If

        'If pbShowRecentlyCompletedJobsOnly Then
        '    sQueryString += "&" + sRecentlyCompleted + "1"
        'Else
        '    sQueryString += "&" + sRecentlyCompleted + "0"
        '    'sQueryString += "&Datefrom=" + DateTime.Now.AddDays(-5).Date + "&DateTo=" + DateTime.Now.Date
        'End If

        If pbShowCompletedJobsOnly Then
            Session.Add("Completed", "1")
        Else
            Session.Add("Completed", "0")
        End If
        If pbShowUncompletedJobsOnly Then
            Session.Add("UnCompleted", "0")
        Else
            Session.Add("UnCompleted", "1")
        End If

        If pbShowRecentlyCompletedJobsOnly Then
            Session.Add("RecentlyCompleted", "1")
        Else
            Session.Add("RecentlyCompleted", "0")
        End If

        If IsHighPrivilege() Then
            Session.Add("UserId", "0")
        Else
            Session.Add("UserId", DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID)
        End If



        'Dim sQueryParams(1) As String
        'sQueryParams(0) = "ReportName=" & FF_GLOBALS.REPORT_JOB_LIST

        'Dim lb As LinkButton = sender
        'Session.Add(FF_GLOBALS.SESSION_JOB_ID, lb.CommandArgument)

        'Dim printScript = String.Format("window.open('Reports.aspx?ReportName=" & FF_GLOBALS.REPORT_JOB & "')")
        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "ReportPrint", printScript, True)

        Dim printScript = String.Format("window.open('Reports.aspx?ReportName=" & FF_GLOBALS.REPORT_JOB_LIST & "')")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ReportPrint", printScript, True)



        'Call NavigateTo(FF_GLOBALS.PAGE_REPORTS, sQueryParams)

        '        Response.Redirect("Reports.aspx?" + "ReportName=" + FF_GLOBALS.REPORT_JOB_LIST + "&" + sQueryString)


    End Sub

    Protected Sub chkIndependentSearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb As CheckBox = sender
        If cb.Checked Then
            pbIndependentSearch = True
            pbIncludeCancelled = False
            pbIncludeTemplates = False
            pbShowCompletedJobsOnly = False
            pbShowRecentlyCompletedJobsOnly = False
            pbShowUncompletedJobsOnly = False
            Call UpdateUserPreferences()
            Call SetUserSettings()
            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()
        Else
            pbIndependentSearch = False
            Call UpdateUserPreferences()
            Call SetUserSettings()
            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()

        End If
    End Sub

    Protected Sub UpdateUserPreferences()
        If _userPreferences IsNot Nothing Then

            _userPreferences.pIndependentSearch = pbIndependentSearch
            _userPreferences.pShowCancelledJobs = pbIncludeCancelled
            _userPreferences.pShowTemplateJobs = pbIncludeTemplates
            _userPreferences.pShowCompletedJobs = pbShowCompletedJobsOnly
            _userPreferences.pShowUnCompletedJobs = pbShowUncompletedJobsOnly
            _userPreferences.pShowRecentlyCompletedJobs = pbShowRecentlyCompletedJobsOnly


        End If
    End Sub

    Protected Sub cbIncludeCancelled_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim cb As CheckBox = sender
        If Not cb.Checked Then
            pbIncludeCancelled = False
            Call UpdateUserPreferences()
            Call SetUserSettings()
            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()
        Else
            pbIncludeCancelled = True
            pbIncludeTemplates = False
            pbShowCompletedJobsOnly = False
            pbShowRecentlyCompletedJobsOnly = False
            pbShowUncompletedJobsOnly = False
            'tdGoButton.Visible = True
            Call UpdateUserPreferences()
            Call SetUserSettings()
            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()
        End If
    End Sub

    Protected Sub cbIncludeTemplates_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim cb As CheckBox = sender

        If cb.Checked Then
            pbIncludeTemplates = True
            pbShowCompletedJobsOnly = False
            pbShowUncompletedJobsOnly = False
            pbShowRecentlyCompletedJobsOnly = False
            pbIncludeCancelled = False
            Call UpdateUserPreferences()
            Call SetUserSettings()
            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()
            'tdGoButton.Visible = True
        Else
            pbIncludeTemplates = False
            Call UpdateUserPreferences()
            Call SetUserSettings()
            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()
        End If
    End Sub

    Protected Sub cbIsCompleted_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim cb As CheckBox = sender

        Dim gvJobStatesRow As GridViewRow = cb.NamingContainer

        Dim gvJobsRow As GridViewRow = gvJobStatesRow.Parent.NamingContainer.NamingContainer

        Dim RadToolTipJobStatus As RadToolTip = gvJobsRow.FindControl("RadToolTipJobStatus")

        'Dim strClientID As String = RadToolTipJobStatus.ClientID


        Dim nJobStateKey As Integer = cb.ValidationGroup
        Dim ffJobState As New FF_JobState(nJobStateKey)
        If cb.Checked Then
            If ffJobState.JobStateName = FF_GLOBALS.JOB_STATE_COMPLETED Then
                Call FF_JobState.SetAllStagesCompleted(ffJobState.JobID)
                Dim ffJob As New FF_Job(ffJobState.JobID)
                ffJob.CompletedOn = DateTime.Now
                ffJob.Update(ffJobState.JobID)

            End If
            ffJobState.IsCompleted = cb.Checked
            ffJobState.Update(nJobStateKey)
            Dim ffAuditEvent As New FF_AuditTrail(ffJobState, Me)
            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()
            Call NotificationEmailSend(ffJobState.JobID, ffJobState.id, ffJobState.Position)
        Else
            ffJobState.IsCompleted = cb.Checked
            ffJobState.Update(nJobStateKey)
            Dim ffAuditEvent As New FF_AuditTrail(ffJobState, Me)
            Call SetQuery()
            Call BindJobsGridView()
            Call HighLightUserJobStages()

        End If

        'RadToolTipJobStatus.Show()

        Dim script As String = "<script language='javascript' type='text/javascript'>function f(){$find('" + RadToolTipJobStatus.ClientID + "').show();}</script>"

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "key1", script, False)

        'Dim script As String = "<script language='javascript' type='text/javascript'>Sys.Application.add_load(showWindow);</script>"

        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "key1", "Sys.Application.add_load(function() {showWindow()});", True)


        'Dim sc As New ScriptManager

        'sc.RegisterClientScriptBlock. 

        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "key", script, True)

        'RadToolTipJobStatus.Show()Sys.Application.remove_load(f)Sys.Application.add_load(f);

        ' LOGGING THIS EVENT

        'Call SetJobStateTextBold(nJobStateKey, cb.Checked)

    End Sub
    Function GetProfilePropertyValue(ByVal userId As Integer, ByVal propertyName As String) As Boolean

        Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetUserById(DNN.GetPMB(Me).PortalId, userId)
        Dim ppc As DotNetNuke.Entities.Profile.ProfilePropertyDefinitionCollection = uiCurrentUser.Profile.ProfileProperties
        Dim ppd As DotNetNuke.Entities.Profile.ProfilePropertyDefinition = ppc.GetByName(propertyName)

        Return ppd.PropertyValue

    End Function

    Protected Function EmailBody(ByVal userInfo As UserInfo, ByVal sJobStateName As String) As String

        Dim sEmailBody As New StringBuilder
        Dim sFirstName As String = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.FirstName
        Dim sLastName As String = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.LastName
        Dim sUserId As String = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
        sEmailBody.Append("Dear " & userInfo.FirstName & " ( " & userInfo.Username & " ) ,")
        sEmailBody.Append("<br><br>")
        If sJobStateName = FF_GLOBALS.JOB_STATE_STARTED Then

            sEmailBody.Append("This is to inform you that " & sFirstName & " " & sLastName & " updated the status of job stage " & "<b>" & sJobStateName & "</b>")
            sEmailBody.Append(" for this job on " & DateTime.Now.ToString("dd-MMM-yyyy") & " at " & DateTime.Now.ToString("hh:mm ss"))
            sEmailBody.Append("<br><br>")
            sEmailBody.Append("You are assigned to this stage and/or the following stage.")
            sEmailBody.Append("<br><br>")
            sEmailBody.Append("Click ")
            Dim link As String = String.Format("<a href=""http://www.sprintexpress.net/ProductionSchedule/JobSummary.aspx?job={0}"">here</a>", psJobGuidId)
            sEmailBody.Append(link)
            sEmailBody.Append(" to view a summary of this job.")
            sEmailBody.Append("<br><br>")
            sEmailBody.Append("Thank you.")
            sEmailBody.Append("<br><br>")
            sEmailBody.Append("(This email was generated automatically. Please do not reply as replies are not monitored)")

        ElseIf sJobStateName = FF_GLOBALS.JOB_STATE_COMPLETED Then
            sEmailBody.Append("All intermediate stages of this job are now finished. The job should now be marked as COMPLETED.")
            sEmailBody.Append("<br><br>")
            sEmailBody.Append("Thank you.")
            sEmailBody.Append("<br><br>")
            sEmailBody.Append("(This email was generated automatically. Please do not reply as replies are not monitored)")

        Else
            'sEmailBody.Append("You have been assigned to <b>" & sJobStateName & "</b> and this is now ready to be worked on. Please Log In into your portal to check further detail.")
            sEmailBody.Append("This is to inform you that " & sFirstName & " " & sLastName & " updated the status of job stage " & "<b>" & sJobStateName & "</b>")
            sEmailBody.Append(" for this job on " & DateTime.Now.ToString("dd-MMM-yyyy") & " at " & DateTime.Now.ToString("hh:mm ss"))
            sEmailBody.Append("<br><br>")
            sEmailBody.Append("You are assigned to this stage and/or the following stage.")
            sEmailBody.Append("<br><br>")
            sEmailBody.Append("Click ")
            Dim link As String = String.Format("<a href=""http://www.sprintexpress.net/ProductionSchedule/JobSummary.aspx?job={0}"">here</a>", psJobGuidId)
            sEmailBody.Append(link)
            sEmailBody.Append(" to view a summary of this job.")
            sEmailBody.Append("<br><br>")
            sEmailBody.Append("Thank you.")
            sEmailBody.Append("<br><br>")
            sEmailBody.Append("(This email was generated automatically. Please do not reply as replies are not monitored)")

        End If

        Return sEmailBody.ToString()

    End Function


    Protected Sub NotificationEmailSend(ByVal nJobId As Integer, ByVal nJobStateId As Integer, ByVal nPosition As Integer)

        Dim nUserId As Integer
        Dim sJobStateName As String
        Dim sJobName As String
        Dim position As Integer
        Dim ffJobState As New FF_JobState(nJobStateId)
        Dim dtUsersInStages As DataTable
        Dim pnJobId As Integer = ffJobState.JobID
        Dim ffJob As New FF_Job(pnJobId)
        psJobGuidId = ffJob.JobGUID

        If FF_GLOBALS.JOB_STATE_STARTED = ffJobState.JobStateName Then

            dtUsersInStages = FF_UserJobMapping.FetchUserInJobStage(nJobId, nJobStateId)
            If dtUsersInStages IsNot Nothing And dtUsersInStages.Rows.Count <> 0 Then
                For Each dr As DataRow In dtUsersInStages.Rows
                    If dr IsNot Nothing Then
                        nUserId = dr("UserId")
                        sJobStateName = dr("JobStateName")
                        sJobName = dr("JobName")
                        position = dr("position")

                        Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserId)
                        If GetProfilePropertyValue(userInfo.UserID, FF_GLOBALS.PS_EMAIL_PROPERTY) Then
                            If dr("IsEmail") Then
                                If IsDBNull(dr("IsEmailSent")) Then
                                    'DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule", "JobId= " & nJobId & "UserId= " & nUserId & "JobStateId= " & nJobStateId & " Email From Production Schedule")
                                    'DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule - " & nJobId, "Dear " & userInfo.DisplayName & ", <br><br>You have been assigned to <b>JobNo = " & nJobId & ". </b>Please Log In into your portal to check further detail.<br><br>Thanking You")
                                    DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule - " & nJobId & " (" & sJobName & ")", EmailBody(userInfo, sJobStateName))
                                    FF_UserJobMapping.UpdateEmailSent(nJobId, userInfo.UserID, 1, nJobStateId)
                                ElseIf dr("IsEmailSent") = False Then
                                    'DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule", "JobId= " & nJobId & "UserId= " & nUserId & "JobStateId= " & nJobStateId & " Email From Production Schedule")
                                    DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule - " & nJobId & " (" & sJobName & ")", EmailBody(userInfo, sJobStateName))
                                    FF_UserJobMapping.UpdateEmailSent(nJobId, userInfo.UserID, 1, nJobStateId)
                                End If
                            End If
                        End If

                    End If
                Next
            End If
            ''''''''''''''''''''' sending email to next state '''''''''''''''''''''''''''''''''

            Dim sSql As String = "select top 1 Position from FF_JobState where  position not in (select Position from FF_JobState where Position <=" & nPosition & ") and JobID = " & nJobId & "order by position asc"
            Dim dtPosition As DataTable = DNNDB.Query(sSql)
            If dtPosition IsNot Nothing And dtPosition.Rows.Count <> 0 Then
                Dim nNextposition As Integer = dtPosition.Rows(0)("Position")
                'nJobStateId = nJobStateId + 1
                dtUsersInStages = FF_UserJobMapping.FetchUsersInPosition(nJobId, nNextposition)
                If dtUsersInStages IsNot Nothing And dtUsersInStages.Rows.Count <> 0 Then
                    For Each dr As DataRow In dtUsersInStages.Rows
                        If dr IsNot Nothing Then
                            nUserId = dr("UserId")
                            sJobStateName = dr("JobStateName")
                            sJobName = dr("JobName")

                            Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserId)
                            If GetProfilePropertyValue(userInfo.UserID, FF_GLOBALS.PS_EMAIL_PROPERTY) Then
                                If IsDBNull(dr("IsEmailSent")) Then
                                    'DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule", "Dear " & userInfo.FullName &  "\r You have been assigned to " & JobId= " & nJobId & "UserId= " & nUserId & "JobStateId= " & nJobStateId & " Email From Production Schedule")
                                    DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule - " & nJobId & " (" & sJobName & ")", EmailBody(userInfo, sJobStateName))
                                    FF_UserJobMapping.UpdateEmailSent(nJobId, userInfo.UserID, 1, nJobStateId)
                                ElseIf dr("IsEmailSent") = False Then
                                    'DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule", "JobId= " & nJobId & "UserId= " & nUserId & "JobStateId= " & nJobStateId & " Email From Production Schedule")
                                    DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule - " & nJobId & " (" & sJobName & ")", EmailBody(userInfo, sJobStateName))
                                    FF_UserJobMapping.UpdateEmailSent(nJobId, userInfo.UserID, 1, nJobStateId)
                                End If
                            End If

                        End If
                    Next
                End If

            End If


        End If

        If ffJobState.JobStateName <> FF_GLOBALS.JOB_STATE_STARTED And ffJobState.JobStateName <> FF_GLOBALS.JOB_STATE_COMPLETED Then

            Dim sSql As String = "select top 1 Position from FF_JobState where  position not in (select Position from FF_JobState where Position <=" & nPosition & ") and JobID = " & nJobId & "order by position asc"
            Dim dtPosition As DataTable = DNNDB.Query(sSql)
            If dtPosition IsNot Nothing And dtPosition.Rows.Count <> 0 Then
                Dim nNextposition As Integer = dtPosition.Rows(0)("Position")
                'nJobStateId = nJobStateId + 1
                dtUsersInStages = FF_UserJobMapping.FetchUsersInPosition(nJobId, nNextposition)
                If dtUsersInStages IsNot Nothing And dtUsersInStages.Rows.Count <> 0 Then
                    For Each dr As DataRow In dtUsersInStages.Rows
                        If dr IsNot Nothing Then
                            nUserId = dr("UserId")
                            sJobStateName = dr("JobStateName")
                            sJobName = dr("JobName")

                            Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserId)
                            If GetProfilePropertyValue(userInfo.UserID, FF_GLOBALS.PS_EMAIL_PROPERTY) Then
                                If IsDBNull(dr("IsEmailSent")) Then
                                    'DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule", "Dear " & userInfo.FullName &  "\r You have been assigned to " & JobId= " & nJobId & "UserId= " & nUserId & "JobStateId= " & nJobStateId & " Email From Production Schedule")
                                    DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule - " & nJobId & " (" & sJobName & ")", EmailBody(userInfo, sJobStateName))
                                    FF_UserJobMapping.UpdateEmailSent(nJobId, userInfo.UserID, 1, nJobStateId)
                                ElseIf dr("IsEmailSent") = False Then
                                    'DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule", "JobId= " & nJobId & "UserId= " & nUserId & "JobStateId= " & nJobStateId & " Email From Production Schedule")
                                    DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule - " & nJobId & " (" & sJobName & ")", EmailBody(userInfo, sJobStateName))
                                    FF_UserJobMapping.UpdateEmailSent(nJobId, userInfo.UserID, 1, nJobStateId)
                                End If
                            End If

                        End If
                    Next
                End If

            End If

        End If

        'If FF_GLOBALS.JOB_STATE_COMPLETED = ffJobState.JobStateName Then
        '    dtUsersInStages = FF_UserJobMapping.FetchUserInJobStage(nJobId, nJobStateId)
        '    If dtUsersInStages IsNot Nothing Then
        '        For Each dr As DataRow In dtUsersInStages.Rows
        '            If dr IsNot Nothing Then
        '                nUserId = dr("UserId")
        '                sJobStateName = dr("JobStateName")
        '                sJobName = dr("JobName")

        '                Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserId)
        '                If GetProfilePropertyValue(userInfo.UserID, FF_GLOBALS.PS_EMAIL_PROPERTY) Then
        '                    If IsDBNull(dr("IsEmailSent")) Then
        '                        'DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule", "JobId= " & nJobId & "UserId= " & nUserId & "JobStateId= " & nJobStateId & " Email From Production Schedule")
        '                        DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule - " & nJobId & " (" & sJobName & ")", EmailBody(userInfo, sJobStateName, bwStage))
        '                        FF_UserJobMapping.UpdateEmailSent(nJobId, userInfo.UserID, 1, nJobStateId)
        '                    ElseIf dr("IsEmailSent") = False Then
        '                        'DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule", "JobId= " & nJobId & "UserId= " & nUserId & "JobStateId= " & nJobStateId & " Email From Production Schedule")
        '                        DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, userInfo.Email, "Production Schedule - " & nJobId & " (" & sJobName & ")", EmailBody(userInfo, sJobStateName, bwStage))
        '                        FF_UserJobMapping.UpdateEmailSent(nJobId, userInfo.UserID, 1, nJobStateId)
        '                    End If
        '                End If

        '            End If
        '        Next
        '    End If
        'Else
        'nJobStateId = nJobStateId + 1
        'dtUsersInStages = FF_UserJobMapping.FetchUserInJobStage(nJobId, nJobStateId)
        'If dtUsersInStages IsNot Nothing And dtUsersInStages.Rows.Count <> 0 Then
        '    For Each dr As DataRow In dtUsersInStages.Rows
        '        If dr IsNot Nothing Then
        '            nUserId = dr("UserId")
        '            Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserId)
        '            If GetProfilePropertyValue(userInfo.UserID, "RPSEmail") Then
        '                If dr("IsEmail") Then
        '                    If dr("IsEmailSent") = 0 Then
        '                        DotNetNuke.Services.Mail.Mail.SendEmail("portal.notifier@gmail.com", userInfo.Email, "Production Schedule", "JobId= " & nJobId & "UserId= " & nUserId & "JobStateId= " & nJobStateId & " Email From Production Schedule")
        '                        FF_UserJobMapping.UpdateEmailSent(nJobId, userInfo.UserID, 1, nJobStateId)
        '                    End If
        '                End If
        '            End If

        '        End If
        '    Next
        'End If
        'End If


    End Sub
    '''''''''''''' not using this method but its good to find a control in any heirarchy '''''''''''
    Public Shared Function FindControlInHeirarchy(ByVal root As Control, ByVal controlId As String) As Control
        If root.ID = controlId Then
            Return root
        End If

        For Each control As Control In root.Controls
            Dim fintCtl As Control = FindControlInHeirarchy(control, controlId)
            If fintCtl IsNot Nothing Then
                Return fintCtl
            End If
        Next

        Return Nothing
    End Function


    Protected Sub SetJobStateTextBold(ByVal nJobStateKey As Integer, ByVal bChecked As Boolean)
        Dim gvr As GridViewRow
        Dim lblJobStateName As Label
        Dim hidJobStateKey As HiddenField
        For Each gvr In gvJobs.Rows
            If gvr.RowType = DataControlRowType.DataRow Then
                Dim gvJobState As GridView = gvr.FindControl("gvJobStates")
                For Each gridRow As GridViewRow In gvJobState.Rows
                    lblJobStateName = gridRow.FindControl("lblJobStateName")
                    hidJobStateKey = gridRow.FindControl("hidJobStateKey")
                    If lblJobStateName IsNot Nothing And hidJobStateKey IsNot Nothing Then
                        If hidJobStateKey.Value = nJobStateKey Then
                            lblJobStateName.Font.Bold = bChecked
                            gridRow.BackColor = Color.SpringGreen
                            Exit Sub
                        End If
                    End If
                Next
            End If
        Next
    End Sub
    Protected Sub cbByCustomer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbByCustomer.CheckedChanged
        Dim cb As CheckBox = sender
        If rcbCustomer.Items.Count = 0 Then
            Call FF_Customer.PopulateCustomerDropdown(rcbCustomer)
        End If
        tdCustomerSelector.Visible = cb.Checked
        rcbCustomer.SelectedIndex = 0
        Call SetQuery()
        Call BindJobsGridView()
        Call HighLightUserJobStages()
    End Sub

    Protected Sub cbByAccountHandler_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbByAccountHandler.CheckedChanged
        Dim cb As CheckBox = sender
        If rcbAccountHandler.Items.Count = 0 Then
            Call FF_GLOBALS.PopulateAccountHandlerCombobox(rcbAccountHandler, Me)
        End If
        tdAccountHandlerSelector.Visible = cb.Checked
        rcbAccountHandler.SelectedIndex = 0
        Call SetQuery()
        Call BindJobsGridView()
        Call HighLightUserJobStages()
    End Sub


    Protected Sub rcbCustomer_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbCustomer.SelectedIndexChanged
        Dim rcb As RadComboBox = o
        'If rcb.SelectedIndex > 0 Then
        Call SetQuery()
        Call BindJobsGridView()
        Call HighLightUserJobStages()
        'End If
    End Sub

    Protected Sub rcbAccountHandler_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbAccountHandler.SelectedIndexChanged

        'If rcb.SelectedIndex > 0 Then
        Call SetQuery()
        Call BindJobsGridView()
        Call HighLightUserJobStages()
        'End If
    End Sub


    Protected Function DateClause() As String

        If Not cbByDate.Checked Then
            DateClause = String.Empty
        ElseIf rdpFromDate.SelectedDate Is Nothing Or rdpToDate.SelectedDate Is Nothing Then
            DateClause = "UNSET"
        ElseIf rdpFromDate.SelectedDate > rdpToDate.SelectedDate Then
            DateClause = "INVALID"
        Else
            DateClause = " AND ffj.CreatedOn >= '" & Format(rdpFromDate.SelectedDate, "dd-MMM-yyyy") & "' AND ffj.CreatedOn <= '" & Format(rdpToDate.SelectedDate, "dd-MMM-yyyy") & "' "
        End If
    End Function

    Protected Sub lnkbtnCreatedColumn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        psSortField = SORT_FIELD_ID
        Call BindJobsGridView()
        Call HighLightUserJobStages()
    End Sub

    Protected Sub lnkbtnDeadlineColumn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        psSortField = SORT_FIELD_DEADLINE
        Call BindJobsGridView()
        Call HighLightUserJobStages()
    End Sub

    Protected Sub chkShowUncompletedJobs_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb As CheckBox = sender
        pbShowUncompletedJobsOnly = cb.Checked
        pbIncludeCancelled = False
        pbIncludeTemplates = False
        'If pboUserPreferences IsNot Nothing Then
        '    pboUserPreferences.pShowUnCompletedJobs = cb.Checked
        'End If
        Call UpdateUserPreferences()
        Call SetUserSettings()
        Call SetQuery()
        Call BindJobsGridView()
        Call HighLightUserJobStages()
    End Sub

    Protected Sub chkShowCompletedJobs_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb As CheckBox = sender
        'If pboUserPreferences IsNot Nothing Then
        '    pboUserPreferences.pShowCompletedJobs = cb.Checked
        'End If
        pbShowCompletedJobsOnly = cb.Checked
        pbIncludeCancelled = False
        pbIncludeTemplates = False
        Call UpdateUserPreferences()
        Call SetUserSettings()
        Call SetQuery()
        Call BindJobsGridView()
        Call HighLightUserJobStages()
    End Sub
    Protected Sub chkShowRecentlyCompletedJobs_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb As CheckBox = sender
        pbShowRecentlyCompletedJobsOnly = cb.Checked
        'If pboUserPreferences IsNot Nothing Then
        '    pboUserPreferences.pShowRecentlyCompletedJobs = cb.Checked
        'End If
        pbIncludeCancelled = False
        pbIncludeTemplates = False
        Call UpdateUserPreferences()
        Call SetUserSettings()
        Call SetQuery()
        Call BindJobsGridView()
        Call HighLightUserJobStages()
    End Sub
    Protected Sub rapPSMain_AjaxRequest(ByVal sender As Object, ByVal e As Telerik.Web.UI.AjaxRequestEventArgs)
    End Sub

    Protected Sub btnNewJobFromClipboard_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        If Session(FF_GLOBALS.COPY_JOB_TO_CLIPBOARD) IsNot Nothing Then
            Dim sQueryParams(1) As String
            sQueryParams(0) = "type=Clipboard"
            sQueryParams(1) = "template=" & Convert.ToInt64(Session(FF_GLOBALS.COPY_JOB_TO_CLIPBOARD))
            Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
        Else

            rapPSMain.Alert("Clipboard is empty.")

        End If

    End Sub

    Protected Sub btnCopyJobToClipboard_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim b As Button = sender
        'DotNetNuke.Services.Personalization.Personalization.SetProfile(FF_GLOBALS.CLIPBOARD_MODULE_ID, "Clipboard", b.CommandArgument.ToString)

        Dim b As Button = sender
        Session.Add(FF_GLOBALS.COPY_JOB_TO_CLIPBOARD, b.CommandArgument.ToString)
        rapPSMain.Alert("Job copied to clipboard.")
        'WebMsgBox.Show("Job copied to clipboard.")


        'Session("FF_Clipboard") = b.CommandArgument.ToString
        'Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        'Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        'Try
        '    Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(FF_GLOBALS.PAGE_NEW_JOB_FROM_CLIPBOARD, pi.PortalID)
        '    tInfo.IsVisible = True
        '    tInfo.DisableLink = False
        '    tabctrl.ClearCache(pi.PortalID)
        '    tabctrl.UpdateTab(tInfo)
        '    tabctrl.ClearCache(pi.PortalID)
        'Catch ex As Exception
        '    WebMsgBox.Show("Failed trying to set page " & FF_GLOBALS.PAGE_NEW_JOB_FROM_CLIPBOARD & " visible. Check page exists.")
        'End Try
    End Sub

    'Protected Sub TimerJobs_Tick(ByVal sender As Object, ByVal e As EventArgs)

    '    BindJobsGridView()

    'End Sub

    Public Shared Function GetInstance() As PS_ProductionScheduleMain
        If myInstance Is Nothing Then
            myInstance = New PS_ProductionScheduleMain()
        End If
        Return myInstance
    End Function

End Class
