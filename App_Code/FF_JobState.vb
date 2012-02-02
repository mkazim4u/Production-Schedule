Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Data

Partial Public Class FF_JobState
    Private Shared myInstance As FF_JobState

    Public Shared Function IsJobComplete(ByVal nJobKey As Int32) As Boolean
        Dim sSQL As String = "SELECT dbo.FF_JobIsComplete(" & nJobKey & ")"
        Dim bResult As Boolean = DNNDB.Query(sSQL)(0)(0)
        IsJobComplete = bResult
    End Function

    Public Shared Function GetJobStateSummary(ByVal DataItem As Object) As String
        Dim nId As Integer = DataBinder.Eval(DataItem, "JobKey")
        GetJobStateSummary = FF_JobState.GetJobStateSummary(nId)
    End Function

    Public Shared Function GetJobStateSummary(ByVal nJobID As Integer) As String
        Dim sSummary As String = String.Empty
        Dim oDT As DataTable = DNNDB.Query("SELECT * FROM FF_JobState WHERE JobID = " & nJobID & " ORDER BY Position")
        Dim nTotalRows As Integer = oDT.Rows.Count
        Dim nIsCompletedContiguousRows As Integer = 0
        Dim nIsCompletedTotalRows As Integer = 0
        Dim bIsContiguous As Boolean = True
        Dim bStageisCompleted As Boolean = False
        For Each dr As DataRow In oDT.Rows
            bStageisCompleted = dr("IsCompleted")
            If Not bStageisCompleted Then
                bIsContiguous = False
            End If
            If bStageisCompleted Then
                nIsCompletedTotalRows += 1
                If bIsContiguous Then
                    nIsCompletedContiguousRows += 1
                End If
            End If
        Next
        If nTotalRows = 0 Then
            sSummary = "<font color='red'>No stages specified!</font>"
        ElseIf nIsCompletedTotalRows = nTotalRows Then
            If nTotalRows = 2 Then
                sSummary = "Both stages completed"
            Else
                sSummary = "All " & nTotalRows.ToString & " stages completed"
            End If
            ElseIf nIsCompletedTotalRows = 0 Then
                sSummary = "Not started (" & nTotalRows.ToString & " stages)"
            Else
                If nIsCompletedContiguousRows = nIsCompletedTotalRows Then
                    sSummary = "First "
                End If
                sSummary += nIsCompletedTotalRows.ToString & " of " & nTotalRows.ToString & " stages completed"
            End If
            GetJobStateSummary = sSummary
    End Function

    Public Shared Function GetJobStateDetailForPopup(ByVal DataItem As Object) As String
        Dim nJobId As Integer = DataBinder.Eval(DataItem, "JobKey")
        Dim sbDetails As New StringBuilder
        Dim oDT As DataTable = DNNDB.Query("SELECT * FROM FF_JobState WHERE JobID = " & nJobID & " ORDER BY Position")
        Dim bStageisCompleted As Boolean = False
        For Each dr As DataRow In oDT.Rows
            sbDetails.Append("<table>")
            sbDetails.Append("<tr>")
            sbDetails.Append("<td style='width: 250px'>")
            sbDetails.Append(dr("JobStateName"))
            sbDetails.Append("</td>")
            sbDetails.Append("<td style='width: 50px'>")
            bStageisCompleted = dr("IsCompleted")
            If bStageisCompleted Then
                sbDetails.Append("COMPLETED")
            Else
                sbDetails.Append("OPEN")
            End If
            sbDetails.Append("</td>")
            sbDetails.Append("</tr>")
            sbDetails.Append("</table>")
        Next

        GetJobStateDetailForPopup = sbDetails.ToString
    End Function

    Public Shared Function GetJobStateDataView(ByVal nJobID As Integer) As DataView
        GetJobStateDataView = New DataView(DNNDB.Query("SELECT * FROM FF_JobState WHERE JobID = " & nJobID & " ORDER BY Position"), "", "Position", DataViewRowState.CurrentRows)
    End Function

    Public Function GetJobStateList(ByVal nJobID As Integer) As List(Of FF_JobState)
        GetJobStateList = Nothing
        Dim oDT As DataTable = DNNDB.Query("SELECT * FROM FF_JobState WHERE JobID = " & nJobID & " ORDER BY Position")
        For Each dr As DataRow In oDT.Rows
            Dim oFFJobState As New FF_JobState
            oFFJobState.JobStateName = dr("JobStateName")
            oFFJobState.JobStateOnCompletionAction = dr("JobStateOnCompletionAction")
            oFFJobState.JobStateOnCompletionNotify = dr("JobStateOnCompletionNotify")
            oFFJobState.JobID = dr("JobID")
            oFFJobState.Position = dr("Position")
            oFFJobState.IsCompleted = dr("IsCompleted")
            GetJobStateList.Add(oFFJobState)
        Next
    End Function

    Public Sub PutJobStateList(ByVal FFJobStateList As List(Of FF_JobState))
        Dim nPosition As Integer = 0
        If FFJobStateList.Count > 0 Then
            Dim oFFJobState As FF_JobState = FFJobStateList(0)
            Dim nJobID As Integer = oFFJobState.JobID
            Call DNNDB.Query("DELETE FROM FF_JobState WHERE JobID = " & nJobID)
            For Each o As FF_JobState In FFJobStateList
                oFFJobState = New FF_JobState(o.JobStateName, o.JobStateOnCompletionAction, o.JobStateOnCompletionNotify, nJobID, nPosition, o.IsCompleted)
                oFFJobState.Add()
            Next
            nPosition += 1
        End If
    End Sub

    Public Shared Sub CloneJobStateList(ByVal nSourceJobID As Integer, ByVal nDestinationJobID As Integer)
        Call DNNDB.Query("DELETE FROM FF_JobState WHERE JobID = " & nDestinationJobID)
        Dim oDT As DataTable = DNNDB.Query("SELECT ISNULL(JobStateName,'') 'JobStateName', ISNULL(JobStateOnCompletionAction,'') 'JobStateOnCompletionAction', ISNULL(JobStateOnCompletionNotify,'') 'JobStateOnCompletionNotify', Position FROM FF_JobState WHERE JobID = " & nSourceJobID)
        For Each dr As DataRow In oDT.Rows
            Dim sbSQL As New StringBuilder
            sbSQL.Append("INSERT INTO FF_JobState (JobStateName, JobStateOnCompletionAction, JobStateOnCompletionNotify, JobID, Position, IsCompleted) VALUES (")
            sbSQL.Append("'")
            sbSQL.Append(dr("JobStateName").ToString.Replace("'", "''"))
            sbSQL.Append("'")
            sbSQL.Append(",")
            sbSQL.Append("'")
            sbSQL.Append(dr("JobStateOnCompletionAction").ToString.Replace("'", "''"))
            sbSQL.Append("'")
            sbSQL.Append(",")
            sbSQL.Append("'")
            sbSQL.Append(dr("JobStateOnCompletionNotify").ToString.Replace("'", "''"))
            sbSQL.Append("'")
            sbSQL.Append(",")
            sbSQL.Append(nDestinationJobID)
            sbSQL.Append(",")
            sbSQL.Append(dr("Position"))
            sbSQL.Append(",")
            sbSQL.Append("0")
            sbSQL.Append(")")
            Call DNNDB.Query(sbSQL.ToString)
        Next
    End Sub

    Public Shared Sub InitJobStateList(ByVal JobID As Int32)
        Dim sbSQL As New StringBuilder
        sbSQL.Append("INSERT INTO FF_JobState (JobStateName, JobStateOnCompletionAction, JobStateOnCompletionNotify, JobID, Position, IsCompleted) VALUES (")
        sbSQL.Append("'")
        sbSQL.Append("Started")
        sbSQL.Append("'")
        sbSQL.Append(",")
        sbSQL.Append("'")
        sbSQL.Append("")
        sbSQL.Append("'")
        sbSQL.Append(",")
        sbSQL.Append("'")
        sbSQL.Append("")
        sbSQL.Append("'")
        sbSQL.Append(",")
        sbSQL.Append(JobID)
        sbSQL.Append(",")
        sbSQL.Append(2)
        sbSQL.Append(",")
        sbSQL.Append("0")
        sbSQL.Append(")")
        sbSQL.Append(" ")
        sbSQL.Append("INSERT INTO FF_JobState (JobStateName, JobStateOnCompletionAction, JobStateOnCompletionNotify, JobID, Position, IsCompleted) VALUES (")
        sbSQL.Append("'")
        sbSQL.Append("Completed")
        sbSQL.Append("'")
        sbSQL.Append(",")
        sbSQL.Append("'")
        sbSQL.Append("")
        sbSQL.Append("'")
        sbSQL.Append(",")
        sbSQL.Append("'")
        sbSQL.Append("")
        sbSQL.Append("'")
        sbSQL.Append(",")
        sbSQL.Append(JobID)
        sbSQL.Append(",")
        sbSQL.Append(4)
        sbSQL.Append(",")
        sbSQL.Append("0")
        sbSQL.Append(")")
        Call DNNDB.Query(sbSQL.ToString)
    End Sub

    Public Shared Function FetchJobStages(ByVal nJobId As Int32) As DataTable
        Dim oDT As DataTable = DNNDB.Query("SELECT ID, JobStateName, JobID, Position, IsCompleted From FF_JobState WHERE JobID = " & nJobId & " ORDER BY Position")
        If oDT.Rows.Count = 0 Then
            Call InitJobStateList(nJobId)
        End If
        Dim nPosition As Integer = 2
        For Each dr As DataRow In oDT.Rows
            dr("Position") = nPosition
            nPosition += 2
        Next
        FetchJobStages = oDT
    End Function
    Public Shared Function GetJobStages(ByVal nJobId As Int32) As DataTable
        Dim oDT As DataTable = DNNDB.Query("SELECT id , JobId, JobStateName, IsCompleted FROM FF_JobState WHERE JobID = " & nJobId & " ORDER BY id")
        Return oDT
    End Function

    Public Shared Sub SetAllStagesCompleted(ByVal nJobId As Int32)
        Dim sSQL As String = "UPDATE FF_JobState SET IsCompleted = 1 WHERE JobId = " & nJobId
        Call DNNDB.Query(sSQL)
    End Sub

    Public Shared Function GetInstance() As FF_JobState
        If myInstance Is Nothing Then
            myInstance = New FF_JobState()
        End If
        Return myInstance
    End Function


End Class
