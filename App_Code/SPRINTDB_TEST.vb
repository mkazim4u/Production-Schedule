Imports Microsoft.VisualBasic

Public Class SprintDB_Test

    Private Shared _sConn As String
    Private Shared _sb As StringBuilder

    Public Shared Function GetConnectionString() As String
        GetConnectionString = _sConn
    End Function

    Public Shared Function GetDatabaseName() As String
        Const nStartPos As Int32 = 12
        Dim nEndPos As Int32 = _sConn.IndexOf(";")
        GetDatabaseName = _sConn.Substring(nStartPos, nEndPos - nStartPos)
    End Function

    Public Shared Sub SetConnectionString(ByVal sConn As String)
        ' SPRINT connection string must be set in root web.config, then
        ' this method must be called from global.asax Application_Start...     DB.SetConnectionString(System.Configuration.ConfigurationManager.ConnectionStrings("AIMSRootConnectionString").ConnectionString)
        _sConn = sConn
    End Sub

    Private Shared Sub CheckConnectionStringInitialised()
        If String.IsNullOrEmpty(_sConn) Then
            WebMsgBox.Show("SPRINTDB_Test.CheckConnectionStringInitialised: Connection string has not been initialised!")
            Throw New Exception("SPRINTDB_Test.CheckConnectionStringInitialised: Connection string has not been initialised!")
        End If
    End Sub

    Public Shared Function GetStockSystemConnectionString() As String
        GetStockSystemConnectionString = _sConn
    End Function

    Public Shared Function Query(ByVal sQuery As String) As DataTable
        Call CheckConnectionStringInitialised()
        Dim oDataTable As New DataTable
        Dim oConn As New SqlConnection(_sConn)
        Dim oAdapter As New SqlDataAdapter(sQuery, oConn)
        Dim oCmd As SqlCommand = New SqlCommand(sQuery, oConn)
        Try
            oAdapter.Fill(oDataTable)
            oConn.Open()
        Catch ex As Exception
            WebMsgBox.Show("Error in SPRINTDB_Test.ExecuteQueryToDataTable executing: " & sQuery & " : " & ex.Message)
        Finally
            oConn.Close()
        End Try
        Query = oDataTable
    End Function

    Public Shared Function Query(ByVal sQuery As String, ByVal sTextFieldName As String, ByVal sValueFieldName As String) As ListItemCollection
        Dim oListItemCollection As New ListItemCollection
        Dim oDataReader As SqlDataReader = Nothing
        Dim oConn As New SqlConnection(_sConn)
        Dim sTextField As String
        Dim sValueField As String
        Dim oCmd As SqlCommand = New SqlCommand(sQuery, oConn)
        Try
            oConn.Open()
            oDataReader = oCmd.ExecuteReader()
            If oDataReader.HasRows Then
                While oDataReader.Read
                    If Not IsDBNull(oDataReader(sTextFieldName)) Then
                        sTextField = oDataReader(sTextFieldName)
                    Else
                        sTextField = String.Empty
                    End If
                    If Not IsDBNull(oDataReader(sValueFieldName)) Then
                        sValueField = oDataReader(sValueFieldName)
                    Else
                        sValueField = String.Empty
                    End If
                    oListItemCollection.Add(New ListItem(sTextField, sValueField))
                End While
            End If
        Catch ex As Exception
            WebMsgBox.Show("Error in SPRINTDB_Test.ExecuteQueryToListItemCollection: " & ex.Message)
        Finally
            oConn.Close()
        End Try
        Query = oListItemCollection
    End Function

    Public Shared Sub StartNewQuery(ByVal sText As String)
        _sb.Length = 0
        _sb.Append(sText)
    End Sub

    Public Shared Sub AddToQuery(ByVal sText As String)
        _sb.Append(sText)
    End Sub

    Public Shared Function RunQuery(ByVal sText As String) As DataTable
        _sb.Append(sText)
        RunQuery = Query(_sb.ToString)
    End Function

    Public Shared Function RunQuery(ByVal sText As String, ByVal sTextFieldName As String, ByVal sValueFieldName As String) As ListItemCollection
        _sb.Append(sText)
        RunQuery = Query(_sb.ToString, sTextFieldName, sValueFieldName)
    End Function

    Public Shared Function Customer(ByVal sText As String) As DataTable
        Customer = Query("SELECT * FROM Customer WHERE CustomerKey = " & sText)
    End Function

End Class
