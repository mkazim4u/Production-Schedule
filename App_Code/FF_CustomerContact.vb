Imports Telerik.Web.UI

Partial Public Class FF_CustomerContact

    Public Shared Function GetFirstContactID(ByVal nCustomerID As Integer) As Integer
        GetFirstContactID = 0
        Dim nCustomerContacts = GetAllContactIDs(nCustomerID)
        If nCustomerContacts.Length > 0 Then
            GetFirstContactID = nCustomerContacts(0)
        End If
    End Function

    Public Shared Function GetAllContactIDs(ByVal nCustomerID As Int32) As Integer()
        GetAllContactIDs = Nothing
        Dim oDT As DataTable
        Dim sSQL As String = "SELECT [id] FROM FF_CustomerContact WHERE CustomerKey = " & nCustomerID & " ORDER BY Name"
        oDT = DNNDB.Query(sSQL)
        Dim nCustomerContacts(oDT.Rows.Count - 1) As Integer
        For i As Integer = 0 To oDT.Rows.Count - 1
            nCustomerContacts(i) = oDT.Rows(i)(0)
        Next
        GetAllContactIDs = nCustomerContacts
    End Function

    'Public Shared Function GetAllContacts(ByVal nCustomerID As Int32) As DataTable
    '    GetAllContacts = Nothing
    '    Dim oDT As DataTable
    '    Dim sSQL As String = "SELECT * FROM FF_CustomerContact WHERE CustomerKey = " & nCustomerID & " ORDER BY Name"
    '    oDT = DNNDB.Query(sSQL)

    'End Function

    Public Shared Function GetAllContacts(ByVal nCustomerID As Int32) As DataTable
        GetAllContacts = DNNDB.Query("SELECT * FROM FF_CustomerContact WHERE CustomerKey = " & nCustomerID & " ORDER BY Name")
    End Function

    Public Shared Function GetContactCount(ByVal nCustomerID As Int32) As Int32
        GetContactCount = DNNDB.Query("SELECT [id] FROM FF_CustomerContact WHERE CustomerKey = " & nCustomerID).Rows.Count
    End Function

    Public Shared Function GetContactName(ByVal nCustomerContactID As Int32) As Int32
        GetContactName = DNNDB.Query("SELECT Name FROM FF_CustomerContact WHERE [id] = " & nCustomerContactID).Rows.Count
    End Function

End Class

