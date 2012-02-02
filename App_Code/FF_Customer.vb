Imports Telerik.Web.UI
Imports System.Data

Partial Public Class FF_Customer

#Region "User methods"

    Public Shared Sub PopulateCustomerDropdown(ByRef rcbRadComboBoxCustomer As RadComboBox)
        Dim sSQL As String = "SELECT CustomerCode, ID FROM FF_Customer WHERE IsActive = 1 AND IsDeleted = 0 ORDER BY CustomerCode"
        Dim oDT As DataTable = DNNDB.Query(sSQL)
        rcbRadComboBoxCustomer.Items.Clear()
        rcbRadComboBoxCustomer.Items.Add(New RadComboBoxItem("- select customer -", 0))
        For Each dr As DataRow In oDT.Rows
            rcbRadComboBoxCustomer.Items.Add(New RadComboBoxItem(dr(0), dr(1)))
        Next
    End Sub

    Public Shared Sub PopulateAccountHandlerDropdown(ByRef rcbRadComboBoxCustomer As RadComboBox)

    End Sub

    Public Shared Sub PopulateCustomerDropdown(ByRef rcbRadComboBoxCustomer As RadComboBox, ByVal bIncludeExternalCode As Boolean)
        Dim sSQL As String = "SELECT CustomerCode, ID, ISNULL(ExternalCustomerKey,0) 'ExternalCustomerKey' FROM FF_Customer WHERE IsActive = 1 AND IsDeleted = 0 ORDER BY CustomerCode"
        Dim oDT As DataTable = DNNDB.Query(sSQL)
        rcbRadComboBoxCustomer.Items.Clear()
        rcbRadComboBoxCustomer.Items.Add(New RadComboBoxItem("- select customer -", "0,0"))
        For Each dr As DataRow In oDT.Rows
            Dim rcbi As New RadComboBoxItem
            rcbi.Text = dr(0)
            If bIncludeExternalCode Then
                rcbi.Value = dr(1) & "," & dr(2)
            Else
                rcbi.Value = dr(1)
            End If
            rcbRadComboBoxCustomer.Items.Add(rcbi)
        Next
    End Sub

    Public Shared Function GetRCBICustomerLocalKey(ByVal rcbi As RadComboBoxItem) As Integer
        GetRCBICustomerLocalKey = 0
        If rcbi IsNot Nothing Then
            If rcbi.Value.Contains(",") Then
                GetRCBICustomerLocalKey = rcbi.Value.Split(",")(0)
            Else
                GetRCBICustomerLocalKey = rcbi.Value
            End If
        End If
    End Function

    Public Shared Function GetRCBICustomerExternalKey(ByVal rcbi As RadComboBoxItem) As Integer
        If rcbi.Value.Contains(",") Then
            GetRCBICustomerExternalKey = rcbi.Value.Split(",")(1)
        Else
            GetRCBICustomerExternalKey = -1
        End If
    End Function

    Public Shared Function IsUniqueCustomerCode(ByVal sCustomerCode As String) As Boolean
        IsUniqueCustomerCode = False
        Dim sSQL As String
        sSQL = "SELECT CustomerCode FROM FF_Customer WHERE CustomerCode = '" & sCustomerCode.Replace("'", "''") & "'"
        If DNNDB.Query(sSQL).Rows.Count = 0 Then
            IsUniqueCustomerCode = True
        End If
    End Function

    Public Shared Function IsUniqueCustomerCode(ByVal sCustomerCode As String, ByVal nCustomerKey As Integer) As Boolean
        IsUniqueCustomerCode = False
        Dim sSQL As String
        sSQL = "SELECT CustomerCode FROM FF_Customer WHERE [id] <> " & nCustomerKey & " AND CustomerCode = '" & sCustomerCode.Replace("'", "''") & "'"
        If DNNDB.Query(sSQL).Rows.Count = 0 Then
            IsUniqueCustomerCode = True
        End If
    End Function

    Public Shared Function GetCustomerCode(ByVal nCustomerKey As Integer) As String
        GetCustomerCode = String.Empty
        If nCustomerKey <> 0 Then
            GetCustomerCode = DNNDB.Query("SELECT CustomerCode FROM FF_Customer WHERE [id] = " & nCustomerKey)(0)(0)
        End If

    End Function

#End Region


End Class
