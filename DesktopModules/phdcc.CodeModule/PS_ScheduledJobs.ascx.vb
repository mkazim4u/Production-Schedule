
Partial Class PS_ScheduledJobs
    Inherits System.Web.UI.UserControl
    Private nUserId As Integer = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
    Private ff_schJob As FF_ScheduledJob
    Private ff_Job As FF_Job
    Private gdv As DataView

    Public Property pdtScheduledJobs() As DataTable
        Get
            Dim o As Object = ViewState("pdtScheduledJobs")
            If o Is Nothing Then
                Return Nothing
            End If
            Return DirectCast(o, DataTable)
        End Get
        Set(ByVal Value As DataTable)
            ViewState("pdtScheduledJobs") = Value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Call FetchScheduledJobs()
            Call BindScheduledJobsGrid(-1)
        End If
    End Sub

    Protected Sub AddColumns()

        pdtScheduledJobs.PrimaryKey = New DataColumn() {pdtScheduledJobs.Columns("ID")}

    End Sub

    Protected Sub FetchScheduledJobs()

        pdtScheduledJobs = DNNDB.ExecuteStoredProcedure("GetAllFromFF_ScheduledJob")
        Call AddColumns()

    End Sub

    Protected Sub BindScheduledJobsGrid(ByVal nEditIndex As Integer)

        'pdtScheduledJobs = DNNDB.ExecuteStoredProcedure("GetAllFromFF_ScheduledJob")
        gvScheduledJobs.DataSource = pdtScheduledJobs
        gvScheduledJobs.EditIndex = nEditIndex
        gvScheduledJobs.DataBind()

    End Sub

    Protected Sub lnkbtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender
        Dim gvr As GridViewRow = lb.NamingContainer
        Dim rowIndex As Integer = gvr.RowIndex
    End Sub

    Protected Sub lnkbtnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim nJobId As Integer
        nJobId = tbJobNo.Text.Trim()
        ff_Job = New FF_Job(nJobId)

        If ff_Job.ID > 0 Then

            ff_schJob = New FF_ScheduledJob()
            ff_schJob.JobId = tbJobNo.Text.Trim()
            ff_schJob.CreatedBy = nUserId
            ff_schJob.Add()


            ' New Row for datatable 

            Dim dr As DataRow = pdtScheduledJobs.NewRow()
            dr("JobId") = tbJobNo.Text.Trim()
            dr("CreatedBy") = nUserId
            dr("LastRun") = FF_GLOBALS.BASE_DATE
            dr("NextRun") = FF_GLOBALS.BASE_DATE
            dr("CreatedOn") = DateTime.Now
            dr("IsIntervalOnDaily") = 0
            dr("IsIntervalInWeeks") = 0
            dr("IsIntervalInMonths") = 0
            pdtScheduledJobs.Rows.Add(dr)

            Call BindScheduledJobsGrid(-1)

            tbJobNo.Text = String.Empty

        Else
            WebMsgBox.Show("Job " + nJobId.ToString() + " doesn't exist in the system.")
            'rapScheduledJobs.Alert("Job " + nJobId.ToString() + " doesn't exist in the system.")

        End If

    End Sub


    Protected Sub gvScheduledJobs_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvScheduledJobs.PageIndexChanging
        gvScheduledJobs.PageIndex = e.NewPageIndex
        Call BindScheduledJobsGrid(-1)
    End Sub

    Protected Sub gvScheduledJobs_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvScheduledJobs.RowEditing

        Call BindScheduledJobsGrid(e.NewEditIndex)

    End Sub

    Protected Sub gvScheduledJobs_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvScheduledJobs.RowUpdating

        Dim gvr As GridViewRow = gvScheduledJobs.Rows(e.RowIndex)
        Dim rdpNextRun As Telerik.Web.UI.RadDatePicker = gvr.FindControl("rdpNextRun")

        If rdpNextRun.SelectedDate IsNot Nothing Then

            Dim hidSchId As HiddenField = gvr.FindControl("hidID")
            Dim nSchId As Integer = hidSchId.Value
            ff_schJob = New FF_ScheduledJob(nSchId)
            ff_schJob.NextRun = rdpNextRun.SelectedDate
            ff_schJob.Update(nSchId)

            Dim dr() As DataRow = pdtScheduledJobs.Select("id=" & nSchId, "")
            dr(0)("NextRun") = rdpNextRun.SelectedDate


        End If


        Call BindScheduledJobsGrid(-1)


    End Sub

    Protected Sub lnkbtnCreatedColumn_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        gdv = New DataView(pdtScheduledJobs)
        gdv.Sort = "CreatedOn DESC"
        pdtScheduledJobs = gdv.ToTable()
        gvScheduledJobs.DataSource = pdtScheduledJobs
        gvScheduledJobs.DataBind()

    End Sub

    Protected Sub gvScheduledJobs_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvScheduledJobs.RowDeleting

        Dim gvr As GridViewRow = gvScheduledJobs.Rows(e.RowIndex)
        Dim hidSchId As HiddenField = gvr.FindControl("hidID")
        Dim nSchId As Integer = hidSchId.Value
        ff_schJob = New FF_ScheduledJob()
        ff_schJob.Delete(nSchId)

        Dim dr() As DataRow = pdtScheduledJobs.Select("id=" & nSchId, "")
        pdtScheduledJobs.Rows.Remove(dr(0))
        Call BindScheduledJobsGrid(-1)

    End Sub

    Protected Sub gvScheduledJobs_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvScheduledJobs.RowCancelingEdit
        Call BindScheduledJobsGrid(-1)
    End Sub

    Protected Sub gvScheduledJobs_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvScheduledJobs.RowDataBound

        Dim gvr As GridViewRow = e.Row

        If gvr.RowType = DataControlRowType.DataRow Then

            Dim hidCreatedBy As HiddenField = gvr.FindControl("hidCreatedBy")
            Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, hidCreatedBy.Value)
            Dim lblUserName As Label = e.Row.FindControl("lblCreatedBy")
            lblUserName.Text = userInfo.Username

        End If

        If (e.Row.RowState And DataControlRowState.Edit) > 0 OrElse (e.Row.RowState And DataControlRowState.Insert) > 0 Then

            Dim rdpNextRun As Telerik.Web.UI.RadDatePicker = gvr.FindControl("rdpNextRun")

            If rdpNextRun.SelectedDate = FF_GLOBALS.BASE_DATE Then
                rdpNextRun.Clear()
            End If

        End If

    End Sub

    Protected Sub chkDaily_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim cb As CheckBox = sender
        Dim dtToday As Date = Date.Today.AddDays(+1)
        Dim hidId As HiddenField = cb.NamingContainer.FindControl("hidID")
        Dim chkWeekly As CheckBox = cb.NamingContainer.FindControl("chkWeekly")
        Dim chkMonthly As CheckBox = cb.NamingContainer.FindControl("chkMonthly")

        If cb.Checked Then


            ff_schJob = New FF_ScheduledJob(hidId.Value)
            ff_schJob.IsIntervalOnDaily = cb.Checked
            ff_schJob.IsIntervalInWeeks = False
            ff_schJob.IsIntervalInMonths = False
            ff_schJob.NextRun = dtToday
            ff_schJob.Update(hidId.Value)

            Dim dr() As DataRow = pdtScheduledJobs.Select("id=" & hidId.Value, "")
            dr(0)("IsIntervalOnDaily") = 1
            dr(0)("NextRun") = dtToday

        Else

            ff_schJob = New FF_ScheduledJob(hidId.Value)
            ff_schJob.IsIntervalOnDaily = cb.Checked
            ff_schJob.NextRun = dtToday
            ff_schJob.Update(hidId.Value)

            Dim dr() As DataRow = pdtScheduledJobs.Select("id=" & hidId.Value, "")
            dr(0)("IsIntervalOnDaily") = 0
            dr(0)("NextRun") = dtToday


        End If

        'FetchScheduledJobs()
        BindScheduledJobsGrid(-1)

    End Sub

    Protected Sub chkWeekly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb As CheckBox = sender

        Dim dtToday As Date = Date.Today.AddDays(+7)
        Dim hidId As HiddenField = cb.NamingContainer.FindControl("hidID")

        If cb.Checked Then

            ff_schJob = New FF_ScheduledJob(hidId.Value)
            ff_schJob.IsIntervalInWeeks = cb.Checked

            ff_schJob.IsIntervalOnDaily = False
            ff_schJob.IsIntervalInMonths = False
            ff_schJob.NextRun = dtToday
            ff_schJob.Update(hidId.Value)

            Dim dr() As DataRow = pdtScheduledJobs.Select("id=" & hidId.Value, "")
            dr(0)("IsIntervalInWeeks") = 1


        Else

            ff_schJob = New FF_ScheduledJob(hidId.Value)
            ff_schJob.IsIntervalInWeeks = cb.Checked
            ff_schJob.NextRun = dtToday
            ff_schJob.Update(hidId.Value)

            Dim dr() As DataRow = pdtScheduledJobs.Select("id=" & hidId.Value, "")
            dr(0)("IsIntervalInWeeks") = 0


        End If

        BindScheduledJobsGrid(-1)

    End Sub

    Protected Sub chkMonthly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb As CheckBox = sender

        Dim dtToday As Date = Date.Today.AddMonths(+1)
        Dim hidId As HiddenField = cb.NamingContainer.FindControl("hidID")

        If cb.Checked Then

            ff_schJob = New FF_ScheduledJob(hidId.Value)
            ff_schJob.IsIntervalInMonths = cb.Checked
            ff_schJob.IsIntervalInWeeks = False
            ff_schJob.IsIntervalOnDaily = False
            ff_schJob.NextRun = dtToday
            ff_schJob.Update(hidId.Value)

            Dim dr() As DataRow = pdtScheduledJobs.Select("id=" & hidId.Value, "")
            dr(0)("IsIntervalInMonths") = 1


        Else

            ff_schJob = New FF_ScheduledJob(hidId.Value)
            ff_schJob.IsIntervalInMonths = cb.Checked
            ff_schJob.NextRun = dtToday
            ff_schJob.Update(hidId.Value)

            Dim dr() As DataRow = pdtScheduledJobs.Select("id=" & hidId.Value, "")
            dr(0)("IsIntervalInMonths") = 0


        End If

        BindScheduledJobsGrid(-1)



    End Sub


End Class
