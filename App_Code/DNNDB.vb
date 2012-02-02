Imports Microsoft.VisualBasic
Imports Microsoft.ApplicationBlocks.Data
Imports System.Data


Public Class DNNDB

    Public Shared Function Query(ByVal sQuery As String) As DataTable
        Query = New DataTable
        Dim dt As New DataTable
        Query.Load(CType(DataProvider.Instance().ExecuteSQL(sQuery), IDataReader))
    End Function
    Public Shared Function ExecuteStoredProcedure(ByVal sStoredProcedure As String, Optional ByVal parameters As SqlParameter() = Nothing) As DataTable
        ExecuteStoredProcedure = New DataTable
        ExecuteStoredProcedure.Load(CType(DataProvider.Instance().ExecuteReader(sStoredProcedure, parameters), IDataReader))
    End Function

    Public Shared Function Query(ByVal sQuery As String, ByVal sTextFieldName As String, ByVal sValueFieldName As String) As ListItemCollection
        Dim oListItemCollection As New ListItemCollection
        Dim oDataReader As SqlDataReader = CType(DataProvider.Instance().ExecuteSQL(sQuery), IDataReader)
        Dim sTextField As String
        Dim sValueField As String
        Try
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
            WebMsgBox.Show("Error in DNNDB.ExecuteQuery: " & ex.Message)
        End Try
        Query = oListItemCollection
    End Function

    Public Shared Function GetFromFormAndList(ByVal nModuleID As Integer, ByVal nFieldList As Integer()) As DataTable
        Dim dt As New DataTable
        Dim dtRowID As DataTable = DNNDB.Query("SELECT UserDefinedRowId FROM UserDefinedRows WHERE ModuleID = " & nModuleID & " ORDER BY UserDefinedRowId")
        Dim nRowCount As Integer = dtRowID.Rows.Count
        For i As Integer = 0 To nFieldList.Length - 1
            Dim dtFieldInfo As DataTable = DNNDB.Query("SELECT FieldTitle, FieldType FROM UserDefinedFields WHERE UserDefinedFieldId = " & nFieldList(i))
            Dim sFieldName As String = dtFieldInfo.Rows(0)("FieldTitle")
            Dim sDataType As String = dtFieldInfo.Rows(0)("FieldType")
            Select Case sDataType
                Case "String", "EMail", "Image", "LookUp", "URL", "UserLink", "CreatedBy", "ChangedBy"
                    dt.Columns.Add(New DataColumn(sFieldName, GetType(String)))
                Case "Int32"
                    dt.Columns.Add(New DataColumn(sFieldName, GetType(Int32)))
                Case "Date", "DateTime", "Time", "CreatedAt", "ChangedAt"
                    dt.Columns.Add(New DataColumn(sFieldName, GetType(DateTime)))
                Case "Boolean"
                    dt.Columns.Add(New DataColumn(sFieldName, GetType(Boolean)))
                Case "Currency", "Decimal"
                    dt.Columns.Add(New DataColumn(sFieldName, GetType(Double)))
                Case Else
                    Throw New Exception("Cannot currently handle type " & sDataType)
            End Select
        Next
        For nRow As Integer = 0 To nRowCount - 1
            dt.Rows.Add()
        Next
        Dim nColumnIndex As Integer = 0
        For nColumn As Integer = 0 To nFieldList.Length - 1
            For nRow As Integer = 0 To nRowCount - 1
                Try
                    dt.Rows(nRow).Item(nColumnIndex) = DNNDB.Query("SELECT FieldValue FROM UserDefinedData WHERE UserDefinedFieldID = " & nFieldList(nColumn) & " AND UserDefinedRowId = " & dtRowID(nRow)(0))(0)(0)
                Catch
                End Try
            Next
            nColumnIndex += 1
        Next
        GetFromFormAndList = dt
    End Function

End Class
