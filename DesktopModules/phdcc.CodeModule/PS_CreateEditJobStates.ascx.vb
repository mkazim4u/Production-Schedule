Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.PortalSecurity
Imports System.Data.SqlTypes
Imports System.Data.SqlDbType
Imports Telerik.Web.UI

Partial Class PS_CreateEditJobStates
    Inherits System.Web.UI.UserControl

    'Dim pdtJobStages As DataTable

    Dim gdv As DataView
    Dim gbFirstGridRowPruned As Boolean = False

    'gdv = New DataView(pdtJobStages, "Position = " & nPosition, "Position", DataViewRowState.CurrentRows)
    'gdv(0)("JobStateName") = gvr.Cells(1)

    ' check if storing data table in ViewState is actually necessary
#Region "Helper functions (HideAllPanels, ...)"

    Protected Sub HideAllPanels()

    End Sub

    Property psButtonSubmit() As String
        Get
            Dim o As Object = ViewState("Submit")
            If o Is Nothing Then
                Return ""
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("Submit") = Value
        End Set
    End Property

#End Region

#Region "Properties"

    Public ReadOnly Property BasePage As DotNetNuke.Framework.CDefault
        Get
            Return CType(Me.Page, DotNetNuke.Framework.CDefault)
        End Get
    End Property

    Property pdtJobStages() As DataTable
        Get
            Dim o As Object = ViewState("JobStages")
            If o Is Nothing Then
                Return Nothing
            End If
            Return o
        End Get
        Set(ByVal Value As DataTable)
            ViewState("JobStages") = Value
        End Set
    End Property

    Property pdtTempJobStages() As DataTable
        Get
            Dim o As Object = ViewState("pdtTempJobStages")
            If o Is Nothing Then
                Return Nothing
            End If
            Return o
        End Get
        Set(ByVal Value As DataTable)
            ViewState("pdtTempJobStages") = Value
        End Set
    End Property


    Property pnJobID() As Integer
        Get
            Dim o As Object = ViewState("JobID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("JobID") = Value
        End Set
    End Property

    Property pFF_Job() As FF_Job
        Get
            Dim o As Object = ViewState("FF_Job")
            If o Is Nothing Then
                Return Nothing
            End If
            Return o
        End Get
        Set(ByVal Value As FF_Job)
            ViewState("FF_Job") = Value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            pnJobID = GetJobIDFromQueryString()
            pdtJobStages = FF_JobState.FetchJobStages(pnJobID)

            Call SetTitles()
            Call AddChangeStatusColumn()
            Call BindGrid(-1)
            lblCreateEditJobStates.Text = lblCreateEditJobStates.Text + " - " + pnJobID.ToString()
        Else
            pdtJobStages = pdtJobStages
        End If
    End Sub

    Protected Sub SetTitles()
        Dim sTitle As String = String.Empty
        sTitle = FF_GLOBALS.PAGE_EDIT_JOB_STATES
        'lblCreateEditJobStates.Text = FF_GLOBALS.PAGE_EDIT_JOB_STATES
        Me.BasePage.Title = FF_GLOBALS.PAGE_TITLE_TEXT + " - " + sTitle
    End Sub

    Protected Sub AddChangeStatusColumn()

        Dim status As DataColumn = New DataColumn("status")
        status.DataType = System.Type.GetType("System.Int32")
        status.Unique = False
        status.Caption = "Status"
        status.DefaultValue = 1
        pdtJobStages.Columns.Add(status)

        'Dim ID As DataColumn = New DataColumn("ID")
        'ID.DataType = System.Type.GetType("System.Int32")
        'ID.Unique = False
        'ID.Caption = "ID"
        'pdtJobStages.Columns.Add(ID)

        pdtJobStages.PrimaryKey = New DataColumn() {pdtJobStages.Columns("ID")}



        'Dim status As DataColumn = pdtJobStages.Columns.Add("status", Type.GetType("System.Integer"))
        'status.AllowDBNull = False
    End Sub
    Protected Function GetJobIDFromQueryString() As String
        GetJobIDFromQueryString = String.Empty
        If Request.Params.Count > 0 Then
            Try
                GetJobIDFromQueryString = Request.Params("job")
            Catch
            End Try
        End If
    End Function


    Protected Sub lnkbtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender
        gdv = New DataView(pdtJobStages)
        gdv.Sort = "Position"

        Dim nIndex As Integer = gdv.Find(lb.CommandArgument)
        Call BindGrid(nIndex)
    End Sub

    Protected Sub lnkbtnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender
    End Sub

    Protected Sub lnkbtnAddAbove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender
        Call AddEntry(lb.CommandArgument - 1)
        Dim gvr As GridViewRow = lb.NamingContainer
        Dim rowIndex As Integer = gvr.RowIndex
        Dim tbJobStateName As TextBox = gvJobStates.Rows(rowIndex).Cells(0).FindControl("tbJobStateName")
        tbJobStateName.Focus()

    End Sub

    Protected Sub lnkbtnAddBelow_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender
        Call AddEntry(lb.CommandArgument + 1)
        Dim gvr As GridViewRow = lb.NamingContainer
        Dim rowIndex As Integer = gvr.RowIndex + 1
        Dim tbJobStateName As TextBox = gvJobStates.Rows(rowIndex).Cells(0).FindControl("tbJobStateName")
        tbJobStateName.Focus()
    End Sub

    Protected Sub AddEntry(ByVal nPosition As Integer)
        Dim dr As DataRow

        'dr = pdtJobStages.NewRow
        dr = pdtJobStages.NewRow
        dr("Position") = nPosition
        dr("JobStateName") = ""
        dr("JobID") = pnJobID
        dr("IsCompleted") = False
        dr("status") = 0

        pdtJobStages.Rows.Add(dr)
        'pdtJobStages.Rows.Add(dr)


        '''''''''''''''''''''''''''''' Temp Row '''''''''''''''''''''''''''''''''
        'Dim drTemp As DataRow

        'drTemp = pdtTempJobStages.NewRow
        'drTemp("Position") = nPosition
        'drTemp("JobStateName") = ""
        'drTemp("JobID") = pnJobID
        'drTemp("IsCompleted") = False
        'drTemp("status") = 0
        'pdtTempJobStages.Rows.Add(drTemp)

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        gdv = New DataView(pdtJobStages)
        gdv.Sort = "Position"
        pdtJobStages = gdv.ToTable

        Dim nNewPosition As Integer = 2
        Dim nEditRow As Integer = -1
        For i As Integer = 0 To pdtJobStages.Rows.Count - 1
            dr = pdtJobStages.Rows(i)
            If dr("Position") = nPosition Then
                nEditRow = i
            End If
            dr("Position") = nNewPosition
            nNewPosition += 2
        Next
        Call BindGrid(nEditRow)
        'pdtJobStages = pdtJobStages

    End Sub

    Protected Sub BindGrid(ByVal nEditIndex As Integer)
        gbFirstGridRowPruned = False
        gvJobStates.EditIndex = nEditIndex
        gvJobStates.DataSource = pdtJobStages
        gvJobStates.DataBind()
    End Sub

    Protected Sub gvJobStates_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvJobStates.RowCancelingEdit
        Call BindGrid(-1)
        btnSaveJobStages.Visible = True
    End Sub
    Protected Sub gvJobStates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvJobStates.RowDataBound
        Dim gvr As GridViewRow = e.Row
        Dim lb As LinkButton = Nothing
        Dim tb As TextBox = Nothing
        'Dim rdpJobStateUncompletedAlertDateTime As RadDatePicker
        Dim lbl As Label = Nothing
        'Dim hidJobStateUncompletedAlertDateTime As HiddenField
        If gvr.RowType = DataControlRowType.DataRow Then
            If Not gbFirstGridRowPruned Then
                lb = GetControl(gvr, "lnkbtnAddAbove")
                lb.Visible = False
                lb = GetControl(gvr, "lnkbtnEdit")
                lb.Visible = False
                lb = GetControl(gvr, "lnkbtnRemove")
                lb.Visible = False

                gbFirstGridRowPruned = True
            End If
            Try
                lbl = GetControl(gvr, "lblJobStateName")
                If lbl.Text = "Completed" Then
                    lb = GetControl(gvr, "lnkbtnAddBelow")
                    lb.Visible = False
                    lb = GetControl(gvr, "lnkbtnEdit")
                    lb.Visible = False
                    lb = GetControl(gvr, "lnkbtnRemove")
                    lb.Visible = False
                End If
            Catch
            End Try

            'lbl = GetControl(gvr, "lblJobStateUncompletedAlertDateTime")
            'If lbl IsNot Nothing Then
            '    If Not String.IsNullOrWhiteSpace(lbl.Text) Then
            '        Dim dt As DateTime = Date.Parse(lbl.Text)
            '        If dt = FF_GLOBALS.BASE_DATE Or lbl.Text.Contains("0001") Then
            '            lbl.Text = String.Empty
            '        Else
            '            'lbl.Text = dt.ToString("d-MMM-yyyy hh:mm")
            '            lbl.Text = dt.ToString("d-MMM-yyyy")
            '        End If
            '    End If
            'Else
            'tb = GetControl(gvr, "tbJobStateUncompletedAlertDateTime")
            'If tb IsNot Nothing Then
            '    If Not String.IsNullOrWhiteSpace(tb.Text) Then
            '        Dim dt As DateTime = Date.Parse(tb.Text)
            '        If dt = FF_GLOBALS.BASE_DATE Then
            '            tb.Text = String.Empty
            '        Else
            '            tb.Text = dt.ToString("d-MMM-yyyy hh:mm")

            '        End If
            '    End If
            'End If
            'hidJobStateUncompletedAlertDateTime = GetControl(gvr, "hidJobStateUncompletedAlertDateTime")
            'If IsDate(hidJobStateUncompletedAlertDateTime.Value) AndAlso Date.Parse(hidJobStateUncompletedAlertDateTime.Value) > FF_GLOBALS.BASE_DATE Then
            '    rdpJobStateUncompletedAlertDateTime = GetControl(gvr, "rdpJobStateUncompletedAlertDateTime")
            '    rdpJobStateUncompletedAlertDateTime.SelectedDate = hidJobStateUncompletedAlertDateTime.Value
            'End If
            'End If
        End If
    End Sub

    Protected Sub gvJobStates_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvJobStates.RowDeleting
        Dim gvr As GridViewRow = gvJobStates.Rows(e.RowIndex)
        Dim hidD As HiddenField = GetControl(gvr, "hidID")
        Dim drDeleted As DataRow
        Dim nID As Integer = hidD.Value
        'gdv = New DataView(pdtJobStages)
        'gdv.Sort = "ID"
        'Dim nIndex As Integer = gdv.Find(nID)
        'gdv.Delete(nIndex)
        drDeleted = pdtJobStages.Rows.Find(nID)
        If (drDeleted IsNot Nothing) Then
            drDeleted.Delete()
        End If
        Call BindGrid(-1)
    End Sub

    Protected Sub gvJobStates_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvJobStates.RowEditing
        Call BindGrid(e.NewEditIndex)
        Dim tbJobStateName As TextBox = gvJobStates.Rows(e.NewEditIndex).FindControl("tbJobStateName")
        tbJobStateName.Focus()
        btnSaveJobStages.Visible = False
    End Sub

    Protected Sub gvJobStates_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvJobStates.RowUpdating
        Dim gvr As GridViewRow = gvJobStates.Rows(e.RowIndex)
        Dim hidD As HiddenField = GetControl(gvr, "hidID")
        Dim hidCompleted As HiddenField = GetControl(gvr, "hidCompleted")
        Dim hidJobStateName As HiddenField = GetControl(gvr, "hidJobStateName")

        Dim nID As Integer = hidD.Value
        Dim bCompleted As Boolean = hidCompleted.Value
        Dim sJobStateName As String = hidJobStateName.Value
        Dim tb As TextBox
        Dim rdp As RadDatePicker

        If CType(GetControl(gvr, "tbJobStateName"), TextBox).Text.Trim = String.Empty Then
            WebMsgBox.Show("You must supply a name for the stage.")
            CType(GetControl(gvr, "tbJobStateName"), TextBox).Focus()
            Exit Sub
        End If


        gdv = New DataView(pdtJobStages, "ID = " & nID, "ID", DataViewRowState.CurrentRows)
        'gdv(0)("JobStateName") = gvr.Cells(1)

        gdv(0)("JobStateName") = (CType(GetControl(gvr, "tbJobStateName"), TextBox)).Text

        'If bCompleted = String.Empty Then

        '    Dim dr As DataRow = pdtTempJobStages.Select("IsCompleted=''")

        'End If



        'Dim drFind As DataRow = pdtJobStages.Rows.Find(nID)
        'If drFind IsNot Nothing Then
        '    drFind("IsCompleted") = String.Empty
        '    drFind("status") = 0
        'End If

        'rdp = CType(GetControl(gvr, "rdpJobStateUncompletedAlertDateTime"), RadDatePicker)
        'tb = CType(GetControl(gvr, "tbJobStateUncompletedAlertDateTime"), TextBox)
        'If String.IsNullOrWhiteSpace(tb.Text) Then
        'If rdp.SelectedDate Is Nothing Then
        'gdv(0)("JobStateUncompletedAlertDateTime") = Date.MinValue
        'ElseIf IsDate(tb.Text) Then
        '    gdv(0)("JobStateUncompletedAlertDateTime") = tb.Text
        'Else
        'WebMsgBox.Show(tb.Text & "is not a valid date")
        'gdv(0)("JobStateUncompletedAlertDateTime") = rdp.SelectedDate.ToString("d-MMM-yyyy")
        'gdv(0)("JobStateUncompletedAlertDateTime") = rdp.SelectedDate
        'End If
        'gdv(0)("JobStateOnCompletionNotify") = (CType(GetControl(gvr, "tbJobStateOnCompletionNotify"), TextBox)).Text
        'gdv(0)("JobStateOnCompletionAction") = (CType(GetControl(gvr, "tbJobStateOnCompletionAction"), TextBox)).Text
        Call BindGrid(-1)
        btnSaveJobStages.Visible = True
    End Sub

    Protected Function GetControl(ByRef gvr As GridViewRow, ByVal sControlName As String) As Control
        GetControl = Nothing
        Dim bFound As Boolean = False
        For Each tc As TableCell In gvr.Cells
            GetControl = tc.FindControl(sControlName)
            If GetControl IsNot Nothing Then
                Exit For
            End If
        Next
    End Function

    Protected Sub btnSaveJobStages_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveJobStages.Click

        'Dim alAdd As New ArrayList
        'Dim alUpdate As New ArrayList
        'alAdd.Add(gvr)

        Dim nStatus As Integer
        Dim sSQL As String


        For Each dr As DataRow In pdtJobStages.Rows

            If dr.RowState = DataRowState.Deleted Then

                Dim o As Object = dr.Item("Id", DataRowVersion.Original)
                Dim Id As Integer = CType(o, Integer)
                Dim sSQLPreamble As String = "Delete from FF_JobState where ID=" & Id
                Call DNNDB.Query(sSQLPreamble)

            Else

                nStatus = Convert.ToInt64(dr("status"))
                If nStatus = 1 Then
                    Dim sSQLPreamble As String = "Update FF_JobState set JobStateName='" & dr("JobStateName") & "' , " & "JobID=" & pnJobID & ", " & "Position=" & dr("Position") & " where id= " & dr("id")
                    sSQL = sSQLPreamble
                    Call DNNDB.Query(sSQL)
                ElseIf nStatus = 0 Then
                    'If nStatus = 0 Then
                    Dim sSQLPreamble As String = "INSERT INTO FF_JobState (JobStateName, JobID, Position, IsCompleted) VALUES ('"
                    sSQL = sSQLPreamble & dr("JobStateName") & "', '" & pnJobID & "', '" & dr("Position") & "' , 0)"
                    Call DNNDB.Query(sSQL)
                    'End If

                End If

            End If

        Next



        Dim sQueryParams(0) As String
        sQueryParams(0) = "job=" & pnJobID
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim sQueryParams(0) As String
        sQueryParams(0) = "job=" & pnJobID
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
    End Sub

    'Protected Sub NavigateTo(ByVal sPageName As String, ByVal sQueryStringKey As String, ByVal sQueryStringParam As String)
    '    Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
    '    Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
    '    Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
    '    Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryStringKey, sQueryStringParam))
    'End Sub

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))
    End Sub

End Class