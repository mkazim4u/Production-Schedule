
Partial Class PS_Chart
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub

    Protected Function GetCompletedJobs() As Integer

        Dim nCompletedJobs As Integer
        Dim sql As String = "SELECT count(*) 'count' from ff_job where dbo.FF_JobIsComplete(ff_job .ID) = 1"
        Dim dt As DataTable = DNNDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            nCompletedJobs = dt.Rows(0)("count")
        Else
            nCompletedJobs = dt.Rows(0)("count")
        End If

        Return nCompletedJobs

    End Function

    Protected Function GetUnCompletedJobs() As Integer

        Dim nUnCompletedJobs As Integer
        Dim sql As String = "SELECT count(*) 'count' from ff_job where dbo.FF_JobIsComplete(ff_job .ID) = 0"
        Dim dt As DataTable = DNNDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            nUnCompletedJobs = dt.Rows(0)("count")
        Else
            nUnCompletedJobs = dt.Rows(0)("count")
        End If

        Return nUnCompletedJobs

    End Function


End Class
