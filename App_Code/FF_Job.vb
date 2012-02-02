Imports Microsoft.VisualBasic
Imports System.Data

Partial Public Class FF_Job

    Public Sub New(ByVal GUID_GUID As String)
        Try
            GetJobByGUID(GUID_GUID)
        Catch
            Throw
        End Try
    End Sub

    Public Shared Function GetJobIDFromGUID(ByVal sGUID As String) As Int32
        Dim sSQL As String
        sSQL = "SELECT [id] FROM FF_Job WHERE JobGUID = '" & sGUID & "'"
        Dim oDataTable As DataTable = DNNDB.Query(sSQL)
        If oDataTable.Rows.Count > 0 Then
            If oDataTable.Rows.Count > 1 Then
                Throw New Exception("Duplicate GUIDs detected in Job table.")
            Else
                GetJobIDFromGUID = oDataTable(0)(0)
            End If
        Else
            GetJobIDFromGUID = 0
        End If
    End Function

    Public Function GetJobByGUID(ByVal sGUID As String) As Int32
        GetJobByGUID = GetJobByID(GetJobIDFromGUID(sGUID))
    End Function

    Public Function GetJobs() As DataTable
        GetJobs = Nothing
        Dim sSQL As String = "SELECT * FROM FF_Job WHERE IsCreated = 1 "

        sSQL += " ORDER BY [id]"
        GetJobs = DNNDB.Query(sSQL)
    End Function

    Public Shared Sub SetJobComplete(ByVal nJobId As Int32)
        Call DNNDB.Query("UPDATE FF_Job SET ") ' Hmm, not sure if we need this now
    End Sub

    ' NOTES
    ' Rename Load to GetJobByID
    ' make ID a readonly property
    ' make JobGUID a readonly property
    ' in Init             m_JobGUID = Guid.NewGuid.ToString
    ' in Init             m_DeadlineOn = DateTime.Parse(BASE_DATE)
    ' in Init             m_CompletedOn = DateTime.Parse(BASE_DATE)

End Class
