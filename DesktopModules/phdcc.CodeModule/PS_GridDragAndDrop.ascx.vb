Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports FFDataLayer
Imports Telerik.Web.UI
Imports Telerik.OpenAccess
Imports FFDataLayer.FF_Group
Partial Class PS_GridDragAndDrop
    Inherits System.Web.UI.UserControl

    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffGroup As New FFDataLayer.FF_Group


    Protected Sub grdPendingOrders_NeedDataSource(ByVal source As Object, ByVal e As GridNeedDataSourceEventArgs)

        'Dim dbContext As New DBContext
        'Dim ffGroup As FFDAL.FFDAL.FF_Group
        Dim query = dbContext.GetAll(Of FFDataLayer.FF_Group)()
        grdPendingOrders.DataSource = query.ToList
        PendingOrdersStore = query.ToList

    End Sub

    Protected Property PendingOrdersStore() As IList(Of FF_Group)
        Get
            Try
                Dim obj As Object = Session("PendingOrders_VB")
                If obj Is Nothing Then
                    obj = GetOrders()
                    Session("PendingOrders_VB") = obj
                End If
                Return DirectCast(obj, IList(Of FF_Group))
            Catch ex As Exception
                Session("PendingOrders_VB") = Nothing
            End Try
            Return New List(Of FF_Group)
        End Get
        Set(ByVal value As IList(Of FF_Group))
            Session("PendingOrders_VB") = value
        End Set
    End Property

    Protected Property ShippedOrdersStore() As IList(Of FF_Group)
        Get
            Try
                Dim obj As Object = Session("ShippedOrders_VB")
                If obj Is Nothing Then
                    obj = New List(Of FF_Group)()
                    Session("ShippedOrders_VB") = obj
                End If
                Return DirectCast(obj, IList(Of FF_Group))
            Catch ex As Exception
                Session("ShippedOrders_VB") = Nothing
            End Try
            Return New List(Of FF_Group)
        End Get
        Set(ByVal value As IList(Of FF_Group))
            Session("ShippedOrders_VB") = value
        End Set
    End Property

    Protected Sub grdShippedOrders_NeedDataSource(ByVal source As Object, ByVal e As GridNeedDataSourceEventArgs)

        grdShippedOrders.DataSource = ShippedOrdersStore

    End Sub

    Protected Sub grdPendingOrders_RowDrop(ByVal sender As Object, ByVal e As GridDragDropEventArgs)

        If String.IsNullOrEmpty(e.HtmlElement) Then
            If e.DraggedItems(0).OwnerGridID = grdPendingOrders.ClientID Then


                ' items are drag from pending to shipped grid
                If (e.DestDataItem Is Nothing AndAlso ShippedOrdersStore.Count = 0) OrElse (e.DestDataItem IsNot Nothing AndAlso e.DestDataItem.OwnerGridID = grdShippedOrders.ClientID) Then
                    Dim shippedOrders As IList(Of FF_Group)
                    Dim pendingOrders As IList(Of FF_Group)

                    shippedOrders = ShippedOrdersStore
                    pendingOrders = PendingOrdersStore

                    Dim destinationIndex As Int32 = -1
                    If (e.DestDataItem IsNot Nothing) Then
                        Dim order As FF_Group = GetOrder(shippedOrders, DirectCast(e.DestDataItem.GetDataKeyValue("Id"), Integer))
                        destinationIndex = IIf(order IsNot Nothing, shippedOrders.IndexOf(order), -1)
                    End If

                    For Each draggedItem As GridDataItem In e.DraggedItems

                        Dim x As GridDataItem = draggedItem
                        Dim hidID As HiddenField = draggedItem.FindControl("hidID")
                        Dim nID As Integer = hidID.Value

                        Dim tmpOrder As FF_Group = GetOrder(shippedOrders, nID)

                        If tmpOrder IsNot Nothing Then
                            If destinationIndex > -1 Then
                                If e.DropPosition = GridItemDropPosition.Below Then
                                    destinationIndex += 1
                                End If
                                shippedOrders.Insert(destinationIndex, tmpOrder)
                            Else
                                shippedOrders.Add(tmpOrder)
                            End If
                            pendingOrders.Remove(tmpOrder)
                        End If
                    Next

                    ShippedOrdersStore = shippedOrders
                    PendingOrdersStore = pendingOrders
                    grdPendingOrders.Rebind()
                    grdShippedOrders.Rebind()
                ElseIf e.DestDataItem IsNot Nothing AndAlso e.DestDataItem.OwnerGridID = grdPendingOrders.ClientID Then

                    'reorder items in pending grid
                    Dim pendingOrders As IList(Of FF_Group)
                    pendingOrders = PendingOrdersStore

                    Dim order As FF_Group = GetOrder(pendingOrders, DirectCast(e.DestDataItem.GetDataKeyValue("Id"), Integer))
                    Dim destinationIndex As Integer = pendingOrders.IndexOf(order)

                    If ((e.DropPosition = GridItemDropPosition.Above) _
                            AndAlso (e.DestDataItem.ItemIndex > e.DraggedItems(0).ItemIndex)) Then
                        destinationIndex = (destinationIndex - 1)
                    End If
                    If ((e.DropPosition = GridItemDropPosition.Below) _
                            AndAlso (e.DestDataItem.ItemIndex < e.DraggedItems(0).ItemIndex)) Then
                        destinationIndex = (destinationIndex + 1)
                    End If

                    Dim ordersToMove As New List(Of FF_Group)()
                    For Each draggedItem As GridDataItem In e.DraggedItems
                        Dim tmpOrder As FF_Group = GetOrder(pendingOrders, DirectCast(draggedItem.GetDataKeyValue("Id"), Integer))
                        If tmpOrder IsNot Nothing Then
                            ordersToMove.Add(tmpOrder)
                        End If
                    Next

                    For Each orderToMove As FF_Group In ordersToMove
                        pendingOrders.Remove(orderToMove)
                        pendingOrders.Insert(destinationIndex, orderToMove)
                    Next
                    PendingOrdersStore = pendingOrders
                    grdPendingOrders.Rebind()

                    Dim destinationItemIndex As Integer = destinationIndex - (grdPendingOrders.PageSize * grdPendingOrders.CurrentPageIndex)
                    e.DestinationTableView.Items(destinationItemIndex).Selected = True
                End If
            End If
        End If

    End Sub

    Protected Sub grdShippedOrders_RowDrop(ByVal sender As Object, ByVal e As GridDragDropEventArgs)
        If Not String.IsNullOrEmpty(e.HtmlElement) AndAlso e.HtmlElement = "trashCan" Then
            Dim shippedOrders As IList(Of FF_Group)
            shippedOrders = ShippedOrdersStore
            Dim deleted As Boolean = False

            For Each draggedItem As GridDataItem In e.DraggedItems
                Dim tmpOrder As FF_Group = GetOrder(shippedOrders, DirectCast(draggedItem.GetDataKeyValue("Id"), Integer))

                If tmpOrder IsNot Nothing Then
                    deleted = True
                    shippedOrders.Remove(tmpOrder)
                End If
            Next
            If (deleted) Then
                'msg.Visible = True
            End If
            ShippedOrdersStore = shippedOrders
            grdShippedOrders.Rebind()
        End If
    End Sub

    Protected Function GetOrders() As IList(Of FF_Group)

        'Dim dbContext As New DBContext
        Dim query = dbContext.GetAll(Of FFDataLayer.FF_Group)()

        Return query.ToList

    End Function

    Private Shared Function GetOrder(ByVal ordersToSearchIn As IEnumerable(Of FF_Group), ByVal groupId As Integer) As FF_Group

        For Each FF_Group As FF_Group In ordersToSearchIn
            If FF_Group.ID = groupId Then
                Return FF_Group
            End If
        Next

        Return Nothing

    End Function



End Class

Public Class Group

    Private _groupId As Int64
    Private _groupName As String

    Public Sub New(ByVal groupId As Integer, ByVal groupName As String)

        _groupId = groupId
        _groupName = groupName

    End Sub

    Public ReadOnly Property GroupID() As Integer
        Get
            Return _groupId
        End Get
    End Property

    Public ReadOnly Property GroupName() As String
        Get
            Return _groupName
        End Get
    End Property

End Class




