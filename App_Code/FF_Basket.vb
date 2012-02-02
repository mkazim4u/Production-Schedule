Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Data

Partial Public Class FF_Basket

    ''' <summary>
    ''' Replace selected but not picked items in Basket table
    ''' </summary>
    Public Shared Sub RefreshBasket(ByVal nJobId As Int32, ByVal nCustomerKey As Int32, ByVal dictItems As Dictionary(Of Int32, Int32))
        Call DNNDB.Query("DELETE FROM FF_Basket WHERE IsPicked = 0 AND CustomerKey = " & nCustomerKey)
        For Each kv As KeyValuePair(Of Int32, Int32) In dictItems
            Dim ffBasket As New FF_Basket
            ffBasket.JobId = nJobId
            ffBasket.CustomerKey = nCustomerKey
            ffBasket.LogisticProductKey = kv.Key
            ffBasket.Qty = kv.Value
            ffBasket.Add()
        Next
    End Sub

    ''' <summary>
    ''' Add picked items with pick datetime
    ''' </summary>
    Public Shared Sub AddPickedItems(ByVal nJobId As Int32, ByVal nCustomerKey As Int32, ByVal dictItems As Dictionary(Of Int32, Int32))
        For Each kv As KeyValuePair(Of Int32, Int32) In dictItems
            Dim ffBasket As New FF_Basket
            ffBasket.CustomerKey = nCustomerKey
            ffBasket.JobId = nJobId
            ffBasket.LogisticProductKey = kv.Key
            ffBasket.Qty = kv.Value
            ffBasket.IsPicked = True
            ffBasket.PickDateTime = DateTime.Now
            ffBasket.Add()
        Next
    End Sub

    ''' <summary>
    ''' Retrieve picked items for job
    ''' </summary>
    Public Shared Function GetPickedItems(ByVal nJobId As Int32) As DataTable
        GetPickedItems = DNNDB.Query("SELECT FROM FF_Basket WHERE IsPicked = 1 AND JobId = " & nJobId)
    End Function

    ''' <summary>
    ''' Retrieve all unpicked items for job, irrespective of customer
    ''' </summary>
    Public Shared Function GetUnpickedItems(ByVal nJobId As Int32) As DataTable
        GetUnpickedItems = DNNDB.Query("SELECT FROM FF_Basket WHERE IsPicked = 0 AND JobId = " & nJobId)
    End Function

    ''' <summary>
    ''' Retrieve all unpicked items for job by customer
    ''' </summary>
    Public Shared Function GetUnpickedItems(ByVal nJobId As Int32, ByVal nCustomerKey As Int32) As DataTable
        GetUnpickedItems = DNNDB.Query("SELECT * FROM FF_Basket WHERE IsPicked = 0 AND JobId = " & nJobId & " AND CustomerKey = " & nCustomerKey)
    End Function

    ''' <summary>
    ''' Remove unpicked items from basket when customer is changed
    ''' </summary>
    Public Shared Sub RemoveUnpickedItemsForJob(ByVal nJobId As Int32)
        Call DNNDB.Query("DELETE FROM FF_Basket WHERE IsPicked = 0 AND JobKey = " & nJobId)
    End Sub

    ''' <summary>
    ''' Total number of picked items irrespective of customer (since customer may have changed)
    ''' </summary>
    Public Shared Function GetTotalPickedItems(ByVal nJobId As Int32) As Integer
        GetTotalPickedItems = DNNDB.Query("SELECT COUNT (*) FROM FF_Basket WHERE IsPicked = 1 AND JobKey = " & nJobId)(0)(0)
    End Function

    ''' <summary>
    ''' Total number of picked items irrespective of customer (since customer may have changed)
    ''' </summary>
    Public Shared Function GetTotalUnpickedItems(ByVal nJobId As Int32) As Integer
        GetTotalUnpickedItems = DNNDB.Query("SELECT COUNT (*) FROM FF_Basket WHERE IsPicked = 0 AND JobKey = " & nJobId)(0)(0)
    End Function

    ''' <summary>
    ''' Total number of unpicked items for job for this customer
    ''' </summary>
    Public Shared Function GetTotalUnpickedItems(ByVal nJobId As Int32, ByVal nCustomerKey As Int32) As Integer
        GetTotalUnpickedItems = DNNDB.Query("SELECT COUNT (*) FROM FF_Basket WHERE IsPicked = 0 AND JobId = " & nJobId & " AND CustomerKey = " & nCustomerKey)(0)(0)
    End Function

End Class
