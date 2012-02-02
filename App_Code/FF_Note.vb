Imports Microsoft.VisualBasic

Partial Public Class FF_Note

    Public Shared Function GetAllNotes(ByVal nJobId As Integer, ByRef Myself As Control) As String
        GetAllNotes = String.Empty
        Dim pmb As DotNetNuke.Entities.Modules.PortalModuleBase
        pmb = DNN.GetPMB(Myself)
        Dim nPortalId As Integer = pmb.PortalId
        Dim sbNotes As New StringBuilder
        Dim oDT As DataTable = DNNDB.Query("SELECT * FROM FF_Note WHERE JobID = " & nJobId & " ORDER BY [id]")
        For Each dr As DataRow In oDT.Rows
            Dim nCreatedBy As Integer = dr("CreatedBy")
            Dim dtCreatedOn As DateTime = dr("CreatedOn")
            Dim usercontroller As DotNetNuke.Entities.Users.UserController = New DotNetNuke.Entities.Users.UserController
            Try
                Dim userinfo As DotNetNuke.Entities.Users.UserInfo = usercontroller.GetUser(nPortalId, nCreatedBy)
                Dim sCreatedByDisplayName As String = userinfo.DisplayName
                sbNotes.Append("[" & sCreatedByDisplayName & " | " & dtCreatedOn.ToString("d-MMM-yyyy h:mm") & "]" & Environment.NewLine)
            Catch ex As Exception
                sbNotes.Append("[" & "Not logged in" & " | " & dtCreatedOn.ToString("d-MMM-yyyy h:mm") & "]" & Environment.NewLine)
            Finally
                sbNotes.Append(dr("Note") & Environment.NewLine & Environment.NewLine)
            End Try
        Next
        GetAllNotes = sbNotes.ToString
    End Function

End Class
