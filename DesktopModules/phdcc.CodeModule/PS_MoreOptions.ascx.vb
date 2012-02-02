Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.PortalSecurity
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports FFDataLayer
Imports Telerik.Web.UI


Partial Class PS_MoreOptions
    Inherits System.Web.UI.UserControl

    Dim gsSQL As String
    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private ri As New DotNetNuke.Security.Roles.RoleInfo
    Private rgi As New DotNetNuke.Security.Roles.RoleGroupInfo
    Private uc As New DotNetNuke.Entities.Users.UserController
    Private userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
    Private userInfo As DotNetNuke.Entities.Users.UserInfo
    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffRecipients As New FFDataLayer.FF_PickRecipient
    Private ffJob As New FFDataLayer.FF_Job

    Public Property psFilterText() As String
        Get
            Dim o As Object = ViewState("psFilterText")
            If o Is Nothing Then
                Return Nothing
            End If
            Return DirectCast(o, String)
        End Get
        Set(ByVal Value As String)
            ViewState("psFilterText") = Value
        End Set
    End Property

    Public Property pdtJobEventLog() As DataTable
        Get
            Dim o As Object = ViewState("pdtJobEventLog")
            If o Is Nothing Then
                Return Nothing
            End If
            Return DirectCast(o, DataTable)
        End Get
        Set(ByVal Value As DataTable)
            ViewState("pdtJobEventLog") = Value
        End Set
    End Property
    Property pbInludeDate() As Boolean
        Get
            Dim o As Object = ViewState("pbInludeDate")
            If o Is Nothing Then
                Return True
            End If
            Return CBool(o)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("pbInludeDate") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call ContractAll()
            Call BindTemplateGridview()
            Call SetControls()
            Call PopulateUsersDropDown()

            pdtJobEventLog = Nothing
            'cbDebugMode.Checked = FF_GLOBALS.bDebugMode
        End If
    End Sub
    Protected Sub SetControls()

        If pbInludeDate = False Then
            rdpFromDate.Enabled = False
            rdpToDate.Enabled = False
            chkIsInclude.Checked = False
        Else
            rdpFromDate.Enabled = True
            rdpToDate.Enabled = True
            chkIsInclude.Checked = True
        End If

    End Sub
    Protected Sub BindTemplateGridview()
        gsSQL = "SELECT ID 'Ref', JobName 'Template' FROM FF_Job WHERE IsCreated = 1 AND IsTemplate = 1 AND IsCancelled = 0 AND IsDeleted = 0 ORDER BY JobName"
        Dim oDT As DataTable = DNNDB.Query(gsSQL)
        gvTemplates.DataSource = oDT
        gvTemplates.DataBind()
    End Sub

    Protected Sub BindCustomerGridview()
        gsSQL = "SELECT ID, CustomerCode, CustomerName FROM FF_Customer WHERE IsActive = 1 AND IsDeleted = 0 ORDER BY CustomerCode"
        Dim oDT As DataTable = DNNDB.Query(gsSQL)
        gvCustomers.DataSource = oDT
        gvCustomers.DataBind()
    End Sub

    Protected Sub BindScheduledJobGridview()
        Dim sSQL As String
        sSQL = "SELECT ID, JobName, IsIntervalInMonths, Interval, LastRun, NextRun FROM FF_Job j INNER JOIN FF_ScheduledJob sj ON j.id = sj.JobId WHERE IsDeleted = 0 ORDER BY JobName"
        Dim oDT As DataTable = DNNDB.Query(sSQL)
        gvScheduledJobs.DataSource = oDT
        gvScheduledJobs.DataBind()
    End Sub

    Protected Function BindAuditTrialGridview() As DataTable
        Dim sqlParam(4) As SqlParameter
        sqlParam(0) = New SqlClient.SqlParameter("@JobId", SqlDbType.BigInt)

        If tbSearchByJobNO.Text.Trim() = String.Empty Then
            sqlParam(0).Value = 0
        Else
            sqlParam(0).Value = Convert.ToInt64(tbSearchByJobNO.Text.Trim())
        End If

        If rcbUsers.SelectedValue <> "-1" Then
            sqlParam(1) = New SqlClient.SqlParameter("@UserId", SqlDbType.BigInt)
            sqlParam(1).Value = rcbUsers.SelectedValue
        Else
            sqlParam(1) = New SqlClient.SqlParameter("@UserId", SqlDbType.BigInt)
            sqlParam(1).Value = DBNull.Value

        End If
        
        If chkIsInclude.Checked Then

            sqlParam(2) = New SqlClient.SqlParameter("@AuditTrialDateFrom", SqlDbType.DateTime)
            sqlParam(2).Value = rdpFromDate.SelectedDate
            sqlParam(3) = New SqlClient.SqlParameter("@AuditTrialDateTo", SqlDbType.DateTime)
            sqlParam(3).Value = rdpToDate.SelectedDate
            pbInludeDate = True
        Else
            sqlParam(2) = New SqlClient.SqlParameter("@AuditTrialDateFrom", SqlDbType.DateTime)
            sqlParam(2).Value = DBNull.Value
            sqlParam(3) = New SqlClient.SqlParameter("@AuditTrialDateTo", SqlDbType.DateTime)
            sqlParam(3).Value = DBNull.Value
            pbInludeDate = False
        End If

        If rcbRecordType.SelectedValue <> "-1" Then
            sqlParam(4) = New SqlClient.SqlParameter("@RecordType", SqlDbType.Char)
            sqlParam(4).Value = rcbRecordType.SelectedValue
            'ShowHideColumn(True)
        Else
            sqlParam(4) = New SqlClient.SqlParameter("@RecordType", SqlDbType.Char)
            sqlParam(4).Value = DBNull.Value
            'ShowHideColumn(False)

        End If


        SetControls()
        Dim oDT As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllEventsForJob", sqlParam)
        pdtJobEventLog = oDT
        gvJobEventLog.Rebind()
        Return pdtJobEventLog
    End Function
    Protected Sub ShowHideColumn(ByVal enable As Boolean)
        gvJobEventLog.Columns(2).HeaderText = "Job No"
        gvJobEventLog.Columns(2).Visible = enable
    End Sub
    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))
    End Sub

    Protected Sub btnAddTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddTemplate.Click
        Dim sQueryParams(0) As String
        sQueryParams(0) = "type=template"
        'sQueryParams(1) = "template"
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
    End Sub

    Protected Sub btnExpandAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpandAll.Click
        Call ExpandAll()
    End Sub

    Protected Sub btnContractAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContractAll.Click
        Call ContractAll()
    End Sub

    Protected Sub ExpandAll()
        shManageCustomers.IsExpanded = True
        shManageTemplates.IsExpanded = True
        shManageScheduledJobs.IsExpanded = True
        shManageAuditTrail.IsExpanded = True
    End Sub

    Protected Sub ContractAll()
        shManageCustomers.IsExpanded = False
        shManageTemplates.IsExpanded = False
        shManageScheduledJobs.IsExpanded = False
        shManageAuditTrail.IsExpanded = False
    End Sub

    Protected Sub btnCreateTemplateFromJob_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateTemplateFromJob.Click

        rwJob.Visible = True
        BindJobCombo()

    End Sub

    Protected Sub BindJobCombo()

        Dim rgJob As RadGrid = TryCast(rcbJob.Items(0).FindControl("rgJob"), RadGrid)
        rgJob.Rebind()

        'Dim dt As New DataTable()

        'dt.Columns.Add("JobID")
        'dt.Columns.Add("JobName")
        'Dim dr As DataRow = dt.NewRow() ' for empty gridview'
        'dr("jobName") = "empty"
        'dr("JobID") = 1
        'dt.Rows.Add(dr)
        'rcbJob.DataSource = dt
        'rcbJob.DataBind()


    End Sub

    Protected Sub rapJob_AjaxRequest(ByVal sender As Object, ByVal e As AjaxRequestEventArgs)

        'Dim rgJob As RadGrid = TryCast(rcbJob.Items(0).FindControl("rgJob"), RadGrid)
        'If e.Argument.ToString() <> String.Empty Then
        '    psFilterText = e.Argument
        '    rgJob.Rebind()
        'End If

    End Sub

    Protected Sub rgJob_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)

        Dim rgJob As Telerik.Web.UI.RadGrid = source

        'Dim IJobs = (From Jobs In dbContext.FF_Jobs Where Jobs.IsCreated = True And Jobs.IsCancelled = False And Jobs.IsTemplate = False And Jobs.ID.ToString() Like psFilterText & "%"
        '             Select Jobs).ToList

        Dim IJobs = (From Jobs In dbContext.FF_Jobs Where Jobs.IsCreated = True And Jobs.IsCancelled = False And Jobs.IsTemplate = False
                     Select Jobs Order By Jobs.CreatedOn Descending).ToList

        rgJob.DataSource = IJobs


    End Sub

    Protected Sub rcbJobTemplate_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbJob.SelectedIndexChanged

        Dim rgJob As Telerik.Web.UI.RadGrid = rcbJob.Items(0).FindControl("rgJob")
        Dim nJobId As Integer

        If rgJob.SelectedItems.Count > 0 Then


            Dim item As GridDataItem = rgJob.SelectedItems(0)


            nJobId = item("ID").Text

            rcbJob.Text = item("ID").Text + " - " + item("JobName").Text

           

        End If


        Dim paramSqlJobId(0) As SqlParameter

        paramSqlJobId(0) = New SqlClient.SqlParameter("@JobId", SqlDbType.BigInt)
        paramSqlJobId(0).Value = nJobId
        Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetJobByJobId", paramSqlJobId)

        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            dvJob.DataSource = dt
            dvJob.DataBind()
        Else
            WebMsgBox.Show("Job Not Found")
        End If

        dvJob.DataSource = dt
        dvJob.DataBind()




    End Sub

    Protected Sub rgJob_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)


        If (TypeOf e.Item Is GridDataItem) Or (e.Item.ItemType = GridItemType.AlternatingItem) Then

            Dim item As GridDataItem = e.Item

            Dim nUserId As Integer = Convert.ToInt64(item("CreatedBy").Text)

            Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, nUserId)

            item("CreatedBy").Text = userInfo.Username


        End If

    End Sub

    Protected Sub lnkCreateTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim lb As LinkButton = sender
        Dim nJobId = lb.CommandArgument
        Dim sQueryParams(1) As String
        sQueryParams(0) = "type=TemplateFromJob"
        sQueryParams(1) = "jobId=" & nJobId.ToString()
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)

    End Sub

    'Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click

    'Dim paramSqlJobId(0) As SqlParameter
    'Dim nJobId As Integer = tbJobNo.Text.Trim()
    'paramSqlJobId(0) = New SqlClient.SqlParameter("@JobId", SqlDbType.BigInt)
    'paramSqlJobId(0).Value = nJobId
    'Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetJobByJobId", paramSqlJobId)
    'If dt IsNot Nothing And dt.Rows.Count <> 0 Then
    '    dvJob.DataSource = dt
    '    dvJob.DataBind()
    'Else
    '    WebMsgBox.Show("Job Not Found")
    'End If

    'End Sub

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


    Protected Sub btnAddCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCustomer.Click
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_CUSTOMER)
    End Sub

    Protected Sub btnImportCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportCustomer.Click
        Dim sQueryParams(1) As String
        sQueryParams(0) = "mode=import"
        'sQueryParams(1) = "import"
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_CUSTOMER, sQueryParams)
    End Sub

    Protected Sub btnShowScheduledJobs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowScheduledJobs.Click
        WebMsgBox.Show("Under Construction.")
        'Call BindScheduledJobGridview()
    End Sub

    'Protected Sub btnShowAuditTrail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowAuditTrail.Click



    'End Sub

    Protected Sub btnAddScheduledJob_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddScheduledJob.Click
        WebMsgBox.Show("Under Construction.")
    End Sub

    Protected Sub btnShowCustomers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowCustomers.Click
        Call BindCustomerGridview()
    End Sub

    Protected Sub lnkbtnEditScheduledJob_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub lnkbtnRemoveScheduledJob_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub lnkbtnEditCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender
        Dim sQueryParams(1) As String
        sQueryParams(0) = "customer=" + lb.CommandArgument
        'sQueryParams(1) = lb.CommandArgument
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_CUSTOMER, sQueryParams)
    End Sub

    Protected Sub lnkbtnRemoveCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub lnkbtnEditTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim b As LinkButton = sender
        Dim nJobId As Integer = b.CommandArgument
        Dim sQueryParams(1) As String
        sQueryParams(0) = "job=" & nJobId
        NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
    End Sub

    Protected Sub lnkbtnRemoveTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub gvTemplates_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvTemplates.RowDeleting

        Dim gvr As GridViewRow = gvTemplates.Rows(e.RowIndex)
        Dim lnkbtnEditTemplate As LinkButton = gvr.FindControl("lnkbtnEditTemplate")
        Dim nTemplateid As Integer = lnkbtnEditTemplate.CommandArgument
        Dim sTeamName As String = gvr.Cells(1).Text


        Call DNNDB.Query("Update FF_Job set IsDeleted = 1 WHERE IsTemplate = 1 and ID = " & nTemplateid)

        Dim ffJob As New FF_Job(nTemplateid)
        Dim ffAuditTrial As New FF_AuditTrail(ffJob, FF_GLOBALS.DB_DELETE)

        Call BindTemplateGridview()

    End Sub


    'Protected Sub cbDebugMode_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbDebugMode.CheckedChanged
    '    FF_GLOBALS.bDebugMode = cbDebugMode.Checked
    'End Sub
    Protected Sub btnSearchAuditTrial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchAuditTrial.Click
        BindAuditTrialGridview()
    End Sub
    Protected Sub rcbRecordType_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbRecordType.SelectedIndexChanged
        BindAuditTrialGridview()
    End Sub
    Protected Sub rcbUsers_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbUsers.SelectedIndexChanged
        BindAuditTrialGridview()
    End Sub

    Protected Sub PopulateUsersDropDown()

        'Dim sqlParam(0) As SqlParameter
        'sqlParam(0) = New SqlClient.SqlParameter("@RoleGroupName", SqlDbType.VarChar)
        'sqlParam(0).Value = FF_GLOBALS.ROLE_GROUP_NAME
        'Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetRoleGroupIdByName", sqlParam)
        'If dt IsNot Nothing And dt.Rows.Count <> 0 Then
        '    Dim nRoleGroupId = dt.Rows(0)("RoleGroupId")
        '    Dim roleList As ArrayList = rc.GetRolesByGroup((DNN.GetPMB(Me).PortalId), nRoleGroupId)
        '    For Each role As DotNetNuke.Security.Roles.RoleInfo In roleList
        '        Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, role.RoleName)
        '        For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
        '            Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, user.UserID)
        '            Dim findUser As RadComboBoxItem = rcbUsers.FindItemByText(userInfo.Username)
        '            If rcbUsers.Items.Contains(findUser) = False Then
        '                rcbUsers.Items.Add(New RadComboBoxItem(userInfo.Username, userInfo.UserID))
        '            End If
        '        Next
        '    Next
        'End If

        Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllUsersFromPSRole")
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            For Each dr As DataRow In dt.Rows
                If rcbUsers.Items.FindItemByText(dr("UserName")) Is Nothing Then
                    rcbUsers.Items.Add(New RadComboBoxItem(dr("UserName"), dr("UserId")))
                End If
            Next

        End If


        rcbUsers.Items.Insert(0, New RadComboBoxItem("- Please Select -", "-1"))

    End Sub

    Protected Sub gvJobEventLog_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles gvJobEventLog.NeedDataSource
        If (ViewState("pdtJobEventLog") Is Nothing) Then
            gvJobEventLog.DataSource = BindAuditTrialGridview()
        Else
            gvJobEventLog.DataSource = pdtJobEventLog
        End If
    End Sub



    Protected Sub rgRecipients_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgRecipients.NeedDataSource

        'Dim ffGroup As FFDAL.FFDAL.FF_Group
        Dim IRecipients = (From Recipients In dbContext.FF_PickRecipients
                          Select Recipients).ToList
        rgRecipients.DataSource = IRecipients

    End Sub
    Protected Sub Insert(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If TypeOf e.Item Is GridEditFormInsertItem And e.Item.IsInEditMode Then

            Dim rtbShortcut As RadTextBox = e.Item.FindControl("rtbShortcut")
            Dim rtbCtcName As RadTextBox = e.Item.FindControl("rtbCtcName")
            Dim rtbCneeName As RadTextBox = e.Item.FindControl("rtbCneeName")
            Dim rtbCneeAddr1 As RadTextBox = e.Item.FindControl("rtbCneeAddr1")
            Dim rtbCneeAddr2 As RadTextBox = e.Item.FindControl("rtbCneeAddr2")
            Dim rtbState As RadTextBox = e.Item.FindControl("rtbState")
            Dim rtbTown As RadTextBox = e.Item.FindControl("rtbTown")
            Dim rtbPostCode As RadTextBox = e.Item.FindControl("rtbPostCode")

            Dim ddlCountry As DropDownList = e.Item.FindControl("ddlCountry")

            ffRecipients.Shortcut = rtbShortcut.Text.Trim()
            ffRecipients.CneeCtcName = rtbCtcName.Text.Trim()
            ffRecipients.CneeName = rtbCneeName.Text.Trim()
            ffRecipients.CneeAddr1 = rtbCneeAddr1.Text.Trim()
            ffRecipients.CneeAddr2 = rtbCneeAddr2.Text.Trim()
            ffRecipients.CneeState = rtbState.Text.Trim()
            ffRecipients.CneeTown = rtbTown.Text.Trim()
            ffRecipients.CneePostCode = rtbPostCode.Text.Trim()
            ffRecipients.CneeCountryKey = Convert.ToInt32(ddlCountry.SelectedValue)
            ffRecipients.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffRecipients.CreatedOn = DateTime.Now

            dbContext.Add(ffRecipients)
            dbContext.SaveChanges()

        End If


    End Sub
    Protected Sub Update(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If e.Item.IsInEditMode Then

            Dim rtbShortcut As RadTextBox = e.Item.FindControl("rtbShortcut")
            Dim rtbCtcName As RadTextBox = e.Item.FindControl("rtbCtcName")
            Dim rtbCneeName As RadTextBox = e.Item.FindControl("rtbCneeName")
            Dim rtbCneeAddr1 As RadTextBox = e.Item.FindControl("rtbCneeAddr1")
            Dim rtbCneeAddr2 As RadTextBox = e.Item.FindControl("rtbCneeAddr2")
            Dim rtbState As RadTextBox = e.Item.FindControl("rtbState")
            Dim rtbTown As RadTextBox = e.Item.FindControl("rtbTown")
            Dim rtbPostCode As RadTextBox = e.Item.FindControl("rtbPostCode")

            Dim ddlCountry As DropDownList = e.Item.FindControl("ddlCountry")
            Dim hidRecipientID As HiddenField = e.Item.FindControl("hidRecipientID")

            ffRecipients = dbContext.GetObjectByKey(New Telerik.OpenAccess.ObjectKey(ffRecipients.GetType().Name, hidRecipientID.Value))

            ffRecipients.Shortcut = rtbShortcut.Text.Trim()
            ffRecipients.CneeCtcName = rtbCtcName.Text.Trim()
            ffRecipients.CneeName = rtbCneeName.Text.Trim()
            ffRecipients.CneeAddr1 = rtbCneeAddr1.Text.Trim()
            ffRecipients.CneeAddr2 = rtbCneeAddr2.Text.Trim()
            ffRecipients.CneeState = rtbState.Text.Trim()
            ffRecipients.CneeTown = rtbTown.Text.Trim()
            ffRecipients.CneePostCode = rtbPostCode.Text.Trim()
            ffRecipients.CneeCountryKey = Convert.ToInt32(ddlCountry.SelectedValue)
            ffRecipients.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffRecipients.CreatedOn = DateTime.Now

            dbContext.SaveChanges()


        End If
    End Sub
    Protected Sub BindCountry(ByVal e As Telerik.Web.UI.GridItemEventArgs)

        Dim ddlCountry As DropDownList = e.Item.FindControl("ddlCountry")

        Call FF_GLOBALS.PopulateComboboxCountry(ddlCountry)

    End Sub
    Protected Sub rgRecipients_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgRecipients.ItemDataBound

        If TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode Then

            If e.Item.OwnerTableView.IsItemInserted Then

                BindCountry(e)

            Else

                BindCountry(e)

                Dim hidRecipientID As HiddenField = e.Item.FindControl("hidRecipientID")

                Dim nID As Int64 = hidRecipientID.Value
                ffRecipients = dbContext.GetObjectByKey(New Telerik.OpenAccess.ObjectKey(ffRecipients.GetType().Name, nID))

                Dim rtbShortcut As RadTextBox = e.Item.FindControl("rtbShortcut")
                Dim rtbCtcName As RadTextBox = e.Item.FindControl("rtbCtcName")
                Dim rtbCneeName As RadTextBox = e.Item.FindControl("rtbCneeName")
                Dim rtbCneeAddr1 As RadTextBox = e.Item.FindControl("rtbCneeAddr1")
                Dim rtbCneeAddr2 As RadTextBox = e.Item.FindControl("rtbCneeAddr2")
                Dim rtbState As RadTextBox = e.Item.FindControl("rtbState")
                Dim rtbTown As RadTextBox = e.Item.FindControl("rtbTown")
                Dim rtbPostCode As RadTextBox = e.Item.FindControl("rtbPostCode")

                Dim ddlCountry As DropDownList = e.Item.FindControl("ddlCountry")

                rtbShortcut.Text = ffRecipients.Shortcut
                rtbCtcName.Text = ffRecipients.CneeCtcName
                rtbCneeName.Text = ffRecipients.CneeName
                rtbCneeAddr1.Text = ffRecipients.CneeAddr1
                rtbCneeAddr2.Text = ffRecipients.CneeAddr2
                rtbState.Text = ffRecipients.CneeState
                rtbTown.Text = ffRecipients.CneeTown
                rtbPostCode.Text = ffRecipients.CneePostCode
                ddlCountry.SelectedValue = ffRecipients.CneeCountryKey

            End If

        ElseIf (TypeOf e.Item Is GridDataItem) Or (e.Item.ItemType = GridItemType.AlternatingItem) Then

            Dim lblCneeCountryKey As Label = e.Item.FindControl("lblCneeCountryKey")
            Dim nCountryKey As Integer = Convert.ToInt64(lblCneeCountryKey.Text)
            Dim sSQL As String = "SELECT SUBSTRING(CountryName, 1, 25) CountryName, CountryKey FROM Country WHERE DeletedFlag = 0 and CountryKey = " + nCountryKey.ToString()
            Dim oDT As DataTable = SprintDB.Query(sSQL)

            If oDT IsNot Nothing And oDT.Rows.Count <> 0 Then

                lblCneeCountryKey.Text = oDT.Rows(0)("CountryName")

            End If

        End If

    End Sub
    Protected Sub rgRecipients_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgRecipients.ItemCommand

        If e.CommandName = "PerformInsert" Then
            Insert(e)
        ElseIf e.CommandName = "Update" Then
            Update(e)
        End If


    End Sub





End Class