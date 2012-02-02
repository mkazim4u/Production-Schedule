Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports System.Linq
Imports System.Reflection
Imports Telerik.Web.UI


Partial Class SNR_Orders
    Inherits System.Web.UI.UserControl

    Private dbContext As New SNRDentonDBLayerDataContext
    Private snr_Audit_Trail As New SNR_AuditTrail
    Private strOrderApprove As String = "Approve"
    Private strOrderReject As String = "Reject"
    Private strOrderWaitingForFulfilment As String = "Waiting For Fulfilment"
    Private strFulfilled As String = "Fulfilled"
    Private uc As New DotNetNuke.Entities.Users.UserController
    Private pc As New DotNetNuke.Entities.Portals.PortalController
    Private pi As DotNetNuke.Entities.Portals.PortalInfo

    Public Property pnOrderID() As Integer
        Get
            Dim o As Object = ViewState("pnOrderID")
            If o Is Nothing Then
                Return -1
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("pnOrderID") = Value
        End Set
    End Property
    Property pnPortalID() As Integer
        Get
            Dim o As Object = ViewState("PortalID")
            If o Is Nothing Then
                Return -1
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("PortalID") = Value
        End Set
    End Property

    Property pnAwaitingAuhtorizationOrders() As Integer
        Get
            Dim o As Object = ViewState("AwaitingAuthorization")
            If o Is Nothing Then
                Return -1
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("AwaitingAuthorization") = Value
        End Set
    End Property

    Property pnApproveOrders() As Integer
        Get
            Dim o As Object = ViewState("Approve")
            If o Is Nothing Then
                Return -1
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("Approve") = Value
        End Set
    End Property
    Property pnAwaitingFulfilmentOrders() As Integer
        Get
            Dim o As Object = ViewState("AwaitingFulfilment")
            If o Is Nothing Then
                Return -1
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("AwaitingFulfilment") = Value
        End Set
    End Property
    Property pnFulfilledOrders() As Integer
        Get
            Dim o As Object = ViewState("Fulfilled")
            If o Is Nothing Then
                Return -1
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("Fulfilled") = Value
        End Set
    End Property
    Property pnCancelledOrders() As Integer
        Get
            Dim o As Object = ViewState("Cancelled")
            If o Is Nothing Then
                Return -1
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("Cancelled") = Value
        End Set
    End Property

    Protected Function GetGUID() As String
        GetGUID = String.Empty
        If Request.Params.Count > 0 Then
            Try
                GetGUID = Request.Params("Order")
            Catch
            End Try
        End If
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then



            BindShops()

        End If

    End Sub

    Private Sub BindShops()

        rcbPortal.DataSource = pc.GetPortals
        rcbPortal.DataBind()

        rcbPortal.Items.Insert(0, New RadComboBoxItem("- All - ", "-1"))

    End Sub

    Private Sub PerformSearch()

        Dim sqlparam(5) As SqlParameter
        sqlparam(0) = New System.Data.SqlClient.SqlParameter("@Portal_ID", SqlDbType.Int)
        sqlparam(0).Value = pnPortalID

        sqlparam(1) = New System.Data.SqlClient.SqlParameter("@Approve_OrderStatusID", SqlDbType.Int)
        sqlparam(1).Value = pnApproveOrders

        sqlparam(2) = New System.Data.SqlClient.SqlParameter("@AwatingFulfiment_OrderStatusID", SqlDbType.Int)
        sqlparam(2).Value = pnAwaitingFulfilmentOrders

        sqlparam(3) = New System.Data.SqlClient.SqlParameter("@AwatingAuthorization_OrderStatusID", SqlDbType.Int)
        sqlparam(3).Value = pnAwaitingAuhtorizationOrders

        sqlparam(4) = New System.Data.SqlClient.SqlParameter("@Cancelled_OrderStatusID", SqlDbType.Int)
        sqlparam(4).Value = pnCancelledOrders

        sqlparam(5) = New System.Data.SqlClient.SqlParameter("@Fulfilled_OrderStatusID", SqlDbType.Int)
        sqlparam(5).Value = pnFulfilledOrders

        rgOrders.DataSource = DNNDB.ExecuteStoredProcedure("SNR_DENTON_ORDER_SEARCH", sqlparam)
        rgOrders.DataBind()

    End Sub

    Protected Sub chkSearchByShop_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSearchByShop.CheckedChanged

        If chkSearchByShop.Checked Then

            tdShop.Visible = True

            rcbPortal.SelectedValue = "-1"

            pnPortalID = -1

            'Dim IOrders = dbContext.SNR_DENTON_ORDER_SEARCH(pnPortalID, pnApproveOrders, pnAwaitingFulfilmentOrders, pnAwaitingAuhtorizationOrders, pnCancelledOrders, pnFulfilledOrders).ToList

            'rgOrders.DataSource = IOrders
            'rgOrders.DataBind()

        Else

            rcbPortal.SelectedValue = "-1"
            tdShop.Visible = False
            pnPortalID = -1

            'Dim IOrders = dbContext.SNR_DENTON_ORDER_SEARCH(pnPortalID, pnApproveOrders, pnAwaitingFulfilmentOrders, pnAwaitingAuhtorizationOrders, pnCancelledOrders, pnFulfilledOrders).ToList

            'rgOrders.DataSource = IOrders
            'rgOrders.DataBind()

            PerformSearch()


        End If

    End Sub

    Protected Sub chkAwaitingAuthorization_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkAwaitingAuthorization As CheckBox = sender

        If chkAwaitingAuthorization.Checked Then
            pnAwaitingAuhtorizationOrders = FF_GLOBALS.ORDER_WAITING_FOR_AUTHORIZATION
        Else
            pnAwaitingAuhtorizationOrders = -1
        End If

        If tdShop.Visible Then

            pnPortalID = Convert.ToInt32(rcbPortal.SelectedValue)


        Else

            'pnPortalID = DNN.GetPMB(Me).PortalId
            pnPortalID = -1

        End If


        PerformSearch()

        'Dim IOrders = (dbContext.SNR_DENTON_ORDER_SEARCH(pnPortalID, pnApproveOrders, pnAwaitingFulfilmentOrders,
        '              pnAwaitingAuhtorizationOrders, pnCancelledOrders, pnFulfilledOrders)).ToList


        'rgOrders.DataSource = IOrders
        'rgOrders.DataBind()

    End Sub

    Protected Sub chkApprove_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkApprove As CheckBox = sender


        If chkApprove.Checked Then
            pnApproveOrders = FF_GLOBALS.ORDER_APPROVE
        Else
            pnApproveOrders = -1
        End If


        If tdShop.Visible Then

            pnPortalID = Convert.ToInt64(rcbPortal.SelectedValue)

        Else

            'pnPortalID = DNN.GetPMB(Me).PortalId
            pnPortalID = -1

        End If

        'Dim IOrders = dbContext.SNR_DENTON_ORDER_SEARCH(pnPortalID, pnApproveOrders, pnAwaitingFulfilmentOrders, pnAwaitingAuhtorizationOrders, pnCancelledOrders, pnFulfilledOrders).ToList

        'rgOrders.DataSource = IOrders
        'rgOrders.DataBind()

        PerformSearch()



    End Sub

    Protected Sub chkAwaitingFulfilment_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkAwaitingAuthorization As CheckBox = sender


        If chkAwaitingAuthorization.Checked Then
            pnAwaitingFulfilmentOrders = FF_GLOBALS.ORDER_WAITING_FOR_FULFIMENT
        Else
            pnAwaitingFulfilmentOrders = -1
        End If

        If tdShop.Visible Then

            pnPortalID = Convert.ToInt64(rcbPortal.SelectedValue)

        Else

            'pnPortalID = DNN.GetPMB(Me).PortalId
            pnPortalID = -1

        End If


        PerformSearch()

        'Dim IOrders = dbContext.SNR_DENTON_ORDER_SEARCH(pnPortalID, pnApproveOrders, pnAwaitingFulfilmentOrders, pnAwaitingAuhtorizationOrders, pnCancelledOrders, pnFulfilledOrders).ToList
        'rgOrders.DataSource = IOrders
        'rgOrders.DataBind()



    End Sub

    Protected Sub chkFulfilled_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkFulfilled As CheckBox = sender

        If chkFulfilled.Checked Then

            pnFulfilledOrders = FF_GLOBALS.ORDER_FULFILLED
        Else
            pnFulfilledOrders = -1

        End If

        If tdShop.Visible Then

            pnPortalID = Convert.ToInt64(rcbPortal.SelectedValue)

        Else

            'pnPortalID = DNN.GetPMB(Me).PortalId
            pnPortalID = -1

        End If

        'Dim IOrders = dbContext.SNR_DENTON_ORDER_SEARCH(pnPortalID, pnApproveOrders, pnAwaitingFulfilmentOrders, pnAwaitingAuhtorizationOrders, pnCancelledOrders, pnFulfilledOrders).ToList
        'rgOrders.DataSource = IOrders
        'rgOrders.DataBind()

        PerformSearch()



    End Sub

    Protected Sub chkCancelled_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkCancelled As CheckBox = sender


        If chkCancelled.Checked Then
            pnCancelledOrders = FF_GLOBALS.ORDER_REJECT
        Else
            pnCancelledOrders = -1
        End If

        If tdShop.Visible Then

            pnPortalID = Convert.ToInt64(rcbPortal.SelectedValue)

        Else

            'pnPortalID = DNN.GetPMB(Me).PortalId
            pnPortalID = -1

        End If

        PerformSearch()

        'Dim IOrders = dbContext.SNR_DENTON_ORDER_SEARCH(pnPortalID, pnApproveOrders, pnAwaitingFulfilmentOrders, pnAwaitingAuhtorizationOrders, pnCancelledOrders, pnFulfilledOrders).ToList
        'rgOrders.DataSource = IOrders
        'rgOrders.DataBind()

    End Sub

    Protected Sub rcbPortal_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rcbPortal.SelectedIndexChanged

        Dim nPortalID As Integer = Convert.ToInt64(rcbPortal.SelectedValue)

        pnPortalID = nPortalID

        'Dim IOrders = dbContext.SNR_DENTON_ORDER_SEARCH(pnPortalID, pnApproveOrders, pnAwaitingFulfilmentOrders, pnAwaitingAuhtorizationOrders, pnCancelledOrders, pnFulfilledOrders).ToList

        'Dim IOrders = From Order In dbContext.NB_Store_Orders
        '              Where Order.PortalID = nPortalID
        '              Select Order.OrderID, Order.OrderNumber, Order.OrderDate, Order.ShippingAddressID, Order.BillingAddressID, Order.OrderGUID, Order.OrderStatusID, Order.PortalID
        '              Order By OrderDate Descending


        'rgOrders.DataSource = IOrders
        'rgOrders.DataBind()

        PerformSearch()

    End Sub

    Protected Sub rgOrderLog_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)

        'Dim rgOrderLog As RadGrid = sender

        'Dim nvItem As GridNestedViewItem = rgOrderLog.NamingContainer.NamingContainer

        'Dim AuditTrail = From o In dbContext.SNR_AuditTrails
        '                 Where o.SourceID



    End Sub

    Protected Sub rgOrders_PreRender(ByVal source As Object, ByVal e As System.EventArgs) Handles rgOrders.PreRender
        'If Not rgOrders.MasterTableView.FilterExpression Is String.Empty Then
        '    RefreshCombos()
        'End If
    End Sub

    Protected Sub RefreshCombos()

        'Dim Orders = From o In dbContext.NB_Store_Orders
        '             Where rgOrders.MasterTableView.FilterExpression.ToString

        'SqlDataSource2.SelectCommand = SqlDataSource2.SelectCommand & " WHERE " & RadGrid1.MasterTableView.FilterExpression.ToString()
        'SqlDataSource3.SelectCommand = SqlDataSource3.SelectCommand & " WHERE " & RadGrid1.MasterTableView.FilterExpression.ToString()
        'SqlDataSource4.SelectCommand = SqlDataSource4.SelectCommand & " WHERE " & RadGrid1.MasterTableView.FilterExpression.ToString()
        'SqlDataSource6.SelectCommand = SqlDataSource6.SelectCommand & " WHERE " & RadGrid1.MasterTableView.FilterExpression.ToString()
        'rgOrders.MasterTableView.Rebind()

        'RadGrid1.MasterTableView.Rebind()
    End Sub

    Protected Sub rgActions_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        'If e.CommandName = RadGrid.FilterCommandName Then
        '    Dim filterPair As Pair = DirectCast(e.CommandArgument, Pair)

        '    Select Case filterPair.Second.ToString()
        '        Case "OrderDate"
        '            Me.startDate = (TryCast((TryCast(e.Item, GridFilteringItem))(filterPair.Second.ToString()).FindControl("FromOrderDatePicker"), RadDatePicker)).SelectedDate
        '            Me.endDate = (TryCast((TryCast(e.Item, GridFilteringItem))(filterPair.Second.ToString()).FindControl("ToOrderDatePicker"), RadDatePicker)).SelectedDate
        '            Exit Select
        '        Case "Freight"
        '            Me.startSlider = (TryCast((TryCast(e.Item, GridFilteringItem))(filterPair.Second.ToString()).FindControl("RadSlider1"), RadSlider)).SelectionStart
        '            Me.endSlider = (TryCast((TryCast(e.Item, GridFilteringItem))(filterPair.Second.ToString()).FindControl("RadSlider1"), RadSlider)).SelectionEnd
        '            Exit Select
        '        Case Else
        '            Exit Select
        '    End Select
        'End If

    End Sub

    Protected Sub rgActions_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs)

        Dim rgActions As RadGrid = sender

        Dim IActions = From Status In dbContext.NB_Store_OrderStatus
                       Where Status.OrderStatusID = 120 Or Status.OrderStatusID > 120 And Status.Lang = "XX"


        rgActions.DataSource = IActions



    End Sub

    Protected Sub rgOrders_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgOrders.NeedDataSource

        Dim IOrders = From Order In dbContext.NB_Store_Orders
                      Select Order.OrderID, Order.OrderNumber, Order.OrderDate, Order.ShippingAddressID, Order.BillingAddressID, Order.OrderGUID, Order.OrderStatusID, Order.PortalID
                      Order By OrderDate Descending


        If Not (String.IsNullOrEmpty(GetGUID)) Then

            IOrders = IOrders.Where(Function(o) o.OrderGUID = GetGUID())

        End If




        'Dim IOrders = From Orders In dbContext.NB_Store_Orders
        '               Join OrderDetail In dbContext.NB_Store_OrderDetails On Orders.OrderID Equals OrderDetail.OrderID
        '              Join Address In dbContext.NB_Store_Addresses On Orders.OrderID Equals Address.OrderID
        '              Select OrderID = Orders.OrderID, OrderNumber = Orders.OrderNumber, OrderStatusID = Orders.OrderStatusID, OrderDetailID = OrderDetail.OrderDetailID, ItemDesc = OrderDetail.ItemDesc,
        '              OrderDate = Orders.OrderDate, AddressName = Address.AddressName, Extra1 = Address.Extra1, CompanyName = Address.CompanyName,
        '              Address2 = Address.Address2, City = Address.City, RegionCode = Address.RegionCode, PostalCode = Address.PostalCode, CountryCode = Address.CountryCode,
        '              Phone1 = Address.Phone1, Phone2 = Address.Phone2


        rgOrders.DataSource = IOrders




    End Sub
    Protected Sub chkStatus_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkStatus As CheckBox = sender

        Dim nvItem As GridNestedViewItem = chkStatus.NamingContainer.NamingContainer.NamingContainer.NamingContainer

        Dim hidOrderID As HiddenField = nvItem.FindControl("hidOrderID")

        Dim hidBefore_OrderStatus As HiddenField = nvItem.FindControl("hidOrderStatusID")

        Dim nBefore_OrderStatus As Integer = hidBefore_OrderStatus.Value

        Dim nOrderID As Integer = hidOrderID.Value

        Dim hidOSID As HiddenField = chkStatus.FindControl("hidOSID")

        Dim nOrderStatusID As Integer = hidOSID.Value

        If chkStatus.Checked Then

            Dim orderStatus_Before = (From os In dbContext.NB_Store_OrderStatus
                     Where os.OrderStatusID = nBefore_OrderStatus
                     Select os).Single



            dbContext.ExecuteCommand("update nb_store_orders set nb_store_orders.orderstatusid = {0} where nb_store_orders.orderid = {1}", nOrderStatusID, nOrderID)

            Dim orderStatus_After = (From os In dbContext.NB_Store_OrderStatus
                    Where os.OrderStatusID = nOrderStatusID
                    Select os).Single


            snr_Audit_Trail.RecordType = FF_GLOBALS.RECORD_TYPE_ORDER
            snr_Audit_Trail.SourceID = nOrderID
            snr_Audit_Trail.CreatedOn = DateTime.Now
            snr_Audit_Trail.AuditEvent = FF_GLOBALS.AUDIT_EVENT_ORDER_UPDATED
            snr_Audit_Trail.ChangeDetail = "Change From " + orderStatus_Before.OrderStatusText + " To " + orderStatus_After.OrderStatusText
            snr_Audit_Trail.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            dbContext.SNR_AuditTrails.InsertOnSubmit(snr_Audit_Trail)
            dbContext.SubmitChanges()

            rgOrders.Rebind()

            ExpandOrderRow(nOrderID)

            'ChangeColor(nOrderID, nOrderStatusID)

        Else

            Dim orderStatus_Before = (From os In dbContext.NB_Store_OrderStatus
                     Where os.OrderStatusID = nBefore_OrderStatus
                     Select os).Single


            Dim nOrderStatus_After As Integer

            If nOrderStatusID = FF_GLOBALS.ORDER_WAITING_FOR_FULFIMENT Then

                nOrderStatus_After = FF_GLOBALS.ORDER_APPROVE

            ElseIf nOrderStatusID = FF_GLOBALS.ORDER_FULFILLED Then

                nOrderStatus_After = FF_GLOBALS.ORDER_WAITING_FOR_FULFIMENT

            ElseIf nOrderStatusID = FF_GLOBALS.ORDER_APPROVE Then

                nOrderStatus_After = FF_GLOBALS.ORDER_WAITING_FOR_AUTHORIZATION

            End If

            Dim orderStatus_After = (From os In dbContext.NB_Store_OrderStatus
                     Where os.OrderStatusID = nOrderStatus_After
                     Select os).Single

            snr_Audit_Trail.RecordType = FF_GLOBALS.RECORD_TYPE_ORDER
            snr_Audit_Trail.SourceID = nOrderID
            snr_Audit_Trail.CreatedOn = DateTime.Now
            snr_Audit_Trail.AuditEvent = FF_GLOBALS.AUDIT_EVENT_ORDER_UPDATED
            snr_Audit_Trail.ChangeDetail = "Change From " + orderStatus_Before.OrderStatusText + " To " + orderStatus_After.OrderStatusText
            snr_Audit_Trail.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            dbContext.SNR_AuditTrails.InsertOnSubmit(snr_Audit_Trail)
            dbContext.SubmitChanges()

            dbContext.ExecuteCommand("update nb_store_orders set nb_store_orders.orderstatusid = {0} where nb_store_orders.orderid = {1}", nOrderStatus_After, nOrderID)

            rgOrders.Rebind()

            ExpandOrderRow(nOrderID)

            'ChangeColor(nOrderID, orderStatus_After)


        End If





    End Sub

    Protected Sub ChangeColor(ByVal nOrderID As Integer, ByVal nOrderStatusID As Integer)

        For Each item As GridDataItem In rgOrders.Items

            If Convert.ToInt64(item("OrderID").Text) = nOrderID Then

                If nOrderStatusID = FF_GLOBALS.ORDER_FULFILLED Then

                    item.BackColor = Color.SpringGreen

                ElseIf nOrderStatusID = FF_GLOBALS.ORDER_REJECT Then

                    item.BackColor = Color.Tomato

                ElseIf nOrderStatusID = FF_GLOBALS.ORDER_WAITING_FOR_FULFIMENT Then

                    item.BackColor = Color.AliceBlue

                ElseIf nOrderStatusID = FF_GLOBALS.ORDER_APPROVE Then

                    item.BackColor = Color.Silver

                ElseIf nOrderStatusID = FF_GLOBALS.ORDER_WAITING_FOR_AUTHORIZATION Then

                    item.BackColor = Color.Yellow

                End If

            End If

        Next


    End Sub

    Protected Function GetCountryKeyByAddressId(ByVal nshippingAddId As Int32) As Int32

        Dim Address = (From add In dbContext.NB_Store_Addresses
                      Where add.AddressID = nshippingAddId).SingleOrDefault

        GetCountryKeyByAddressId = "222"

        Dim countryCode As String = Address.CountryCode
        Dim sqlNB As String = "select * from nb_store_address where addressid = " & nshippingAddId
        'Dim dtAddress As DataTable = nbQuery.GetDataTable(sqlNB)
        'If dtAddress IsNot Nothing And dtAddress.Rows.Count <> 0 Then

        '    countryCode = dtAddress.Rows(0)("countrycode").ToString()

        'End If
        Dim sql As String = "select top 1 * from Logistics.dbo.Country where CountryId2 ='" & countryCode & "'"
        Dim dt As DataTable = SprintDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            GetCountryKeyByAddressId = Convert.ToInt32(dt.Rows(0)("countrykey"))
        End If

        Return GetCountryKeyByAddressId

    End Function

    Protected Function GetProductKey(ByVal nOrderDetailId As Int32) As Int32

        GetProductKey = 0

        Dim sql As String = "select productref from NB_Store_Products where ProductID  = (select productid from NB_Store_Model where ModelID  = (select modelid from nb_store_orderdetails where orderdetailid = " & nOrderDetailId & "))"
        Dim dt As DataTable = DNNDB.Query(sql)
        Dim nProductID As Integer
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            If Integer.TryParse(dt.Rows(0)("productref"), nProductID) Then
                GetProductKey = Convert.ToInt32(dt.Rows(0)("productref"))
            Else
                GetProductKey = 0
            End If

        End If

        Return GetProductKey


    End Function

    Public Function PickOrder(ByVal nOrderID As Integer) As Int32


        PickOrder = 0
        Dim nBookingKey As Int32
        Dim nConsignmentKey As Int32
        Dim BookingFailed As Boolean
        Dim oConn As New SqlConnection

        Dim nSprintAccountHandlerKey As Int32 = 0


        Dim config = (From con In dbContext.SNR_Configurations
                      Where con.ConfigKey = FF_GLOBALS.CUSTOMER_ID_KEY
                     Select con).SingleOrDefault

        Dim Order = (From o In dbContext.NB_Store_Orders
                    Where o.OrderID = nOrderID
                    Select o.OrderID, o.OrderNumber, o.OrderDate, o.ShippingAddressID, o.BillingAddressID, o.OrderGUID,
                    o.OrderStatusID, o.PortalID
                    Order By OrderDate Descending).SingleOrDefault


        Dim nCustKey As Integer = config.ConfigValue

        Dim oTrans As SqlTransaction
        Dim oCmdAddBooking As SqlCommand = New SqlCommand("spASPNET_StockBooking_Add3", oConn)
        oCmdAddBooking.CommandType = CommandType.StoredProcedure

        Dim nShippindAddressID As Integer = Order.ShippingAddressID

        Dim Address = (From add In dbContext.NB_Store_Addresses
                       Where add.AddressID = nShippindAddressID
                       Select add).SingleOrDefault



        'Dim dtCustAdd As DataTable = GetShippingAddressFromOrderId(nOrderID)

        Dim param1 As SqlParameter = New SqlParameter("@UserProfileKey", SqlDbType.Int, 4)
        'param1.Value = nSprintAccountHandlerKey
        param1.Value = 0
        oCmdAddBooking.Parameters.Add(param1)

        Dim param2 As SqlParameter = New SqlParameter("@CustomerKey", SqlDbType.Int, 4)
        'param2.Value = FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem)
        'param2.Value = GetCustomerKeyByPortalId(DNN.GetPMB(Me).PortalId)
        param2.Value = nCustKey
        oCmdAddBooking.Parameters.Add(param2)

        Dim param2a As SqlParameter = New SqlParameter("@BookingOrigin", SqlDbType.NVarChar, 20)
        param2a.Value = "WEB_BOOKING"
        oCmdAddBooking.Parameters.Add(param2a)

        Dim param3 As SqlParameter = New SqlParameter("@BookingReference1", SqlDbType.NVarChar, 25)
        'param3.Value = "Fulfilment Job # " & pnJobID.ToString
        'cost centre code
        param3.Value = ""

        oCmdAddBooking.Parameters.Add(param3)

        Dim param4 As SqlParameter = New SqlParameter("@BookingReference2", SqlDbType.NVarChar, 25)
        'param4.Value = tbJobName.Text.Trim()
        param4.Value = ""
        oCmdAddBooking.Parameters.Add(param4)

        Dim param5 As SqlParameter = New SqlParameter("@BookingReference3", SqlDbType.NVarChar, 50)
        'param5.Value = txtRwBasketAIMSCustRef1.Text.Trim()
        'order id
        param5.Value = ""
        oCmdAddBooking.Parameters.Add(param5)

        Dim param6 As SqlParameter = New SqlParameter("@BookingReference4", SqlDbType.NVarChar, 50)
        'param6.Value = txtRwBasketAIMSCustRef2.Text.Trim()
        param6.Value = ""
        oCmdAddBooking.Parameters.Add(param6)

        Dim param6a As SqlParameter = New SqlParameter("@ExternalReference", SqlDbType.NVarChar, 50)
        'param6a.Value = GetJobGuid()
        param6a.Value = ""
        oCmdAddBooking.Parameters.Add(param6a)

        Dim param7 As SqlParameter = New SqlParameter("@SpecialInstructions", SqlDbType.NVarChar, 1000)
        'param7.Value = tbSpecialInstructions.Text
        param7.Value = ""
        oCmdAddBooking.Parameters.Add(param7)

        Dim param8 As SqlParameter = New SqlParameter("@PackingNoteInfo", SqlDbType.NVarChar, 1000)
        param8.Value = String.Empty
        oCmdAddBooking.Parameters.Add(param8)

        Dim param9 As SqlParameter = New SqlParameter("@ConsignmentType", SqlDbType.NVarChar, 20)
        param9.Value = "STOCK ITEM"
        oCmdAddBooking.Parameters.Add(param9)

        Dim param10 As SqlParameter = New SqlParameter("@ServiceLevelKey", SqlDbType.Int, 4)
        param10.Value = -1
        oCmdAddBooking.Parameters.Add(param10)
        Dim param11 As SqlParameter = New SqlParameter("@Description", SqlDbType.NVarChar, 250)
        param11.Value = "PRINTED MATTER - FREE DOMICILE"
        oCmdAddBooking.Parameters.Add(param11)

        Dim param13 As SqlParameter = New SqlParameter("@CnorName", SqlDbType.NVarChar, 50)
        param13.Value = "Sprint International"
        oCmdAddBooking.Parameters.Add(param13)

        Dim param14 As SqlParameter = New SqlParameter("@CnorAddr1", SqlDbType.NVarChar, 50)
        param14.Value = "UNIT 3 MERCURY CENTRE"
        oCmdAddBooking.Parameters.Add(param14)

        Dim param15 As SqlParameter = New SqlParameter("@CnorAddr2", SqlDbType.NVarChar, 50)
        param15.Value = "CENTRAL WAY"
        oCmdAddBooking.Parameters.Add(param15)

        Dim param16 As SqlParameter = New SqlParameter("@CnorAddr3", SqlDbType.NVarChar, 50)
        param16.Value = ""
        oCmdAddBooking.Parameters.Add(param16)

        Dim param17 As SqlParameter = New SqlParameter("@CnorTown", SqlDbType.NVarChar, 50)
        param17.Value = "FELTHAM"
        oCmdAddBooking.Parameters.Add(param17)

        Dim param18 As SqlParameter = New SqlParameter("@CnorState", SqlDbType.NVarChar, 50)
        param18.Value = "MIDDLESEX"
        oCmdAddBooking.Parameters.Add(param18)

        Dim param19 As SqlParameter = New SqlParameter("@CnorPostCode", SqlDbType.NVarChar, 50)
        param19.Value = "TW14 0RN"
        oCmdAddBooking.Parameters.Add(param19)

        Dim param20 As SqlParameter = New SqlParameter("@CnorCountryKey", SqlDbType.Int, 4)
        param20.Value = 222
        oCmdAddBooking.Parameters.Add(param20)

        Dim param21 As SqlParameter = New SqlParameter("@CnorCtcName", SqlDbType.NVarChar, 50)
        'Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetCurrentUserInfo
        param21.Value = "" ' Account Handler's Name
        oCmdAddBooking.Parameters.Add(param21)

        Dim param22 As SqlParameter = New SqlParameter("@CnorTel", SqlDbType.NVarChar, 50)
        param22.Value = ""
        oCmdAddBooking.Parameters.Add(param22)

        Dim param23 As SqlParameter = New SqlParameter("@CnorEmail", SqlDbType.NVarChar, 50)
        param23.Value = ""
        oCmdAddBooking.Parameters.Add(param23)

        Dim param24 As SqlParameter = New SqlParameter("@CnorPreAlertFlag", SqlDbType.Bit)
        param24.Value = 0
        oCmdAddBooking.Parameters.Add(param24)

        Dim param25 As SqlParameter = New SqlParameter("@CneeName", SqlDbType.NVarChar, 50)
        param25.Value = Address.AddressName
        oCmdAddBooking.Parameters.Add(param25)

        Dim param26 As SqlParameter = New SqlParameter("@CneeAddr1", SqlDbType.NVarChar, 50)
        param26.Value = Address.Address1
        oCmdAddBooking.Parameters.Add(param26)

        Dim param27 As SqlParameter = New SqlParameter("@CneeAddr2", SqlDbType.NVarChar, 50)
        param27.Value = Address.Address2
        oCmdAddBooking.Parameters.Add(param27)

        Dim param28 As SqlParameter = New SqlParameter("@CneeAddr3", SqlDbType.NVarChar, 50)
        param28.Value = ""
        oCmdAddBooking.Parameters.Add(param28)

        Dim param29 As SqlParameter = New SqlParameter("@CneeTown", SqlDbType.NVarChar, 50)
        param29.Value = Address.City
        oCmdAddBooking.Parameters.Add(param29)

        Dim param30 As SqlParameter = New SqlParameter("@CneeState", SqlDbType.NVarChar, 50)
        param30.Value = Address.City
        oCmdAddBooking.Parameters.Add(param30)

        Dim param31 As SqlParameter = New SqlParameter("@CneePostCode", SqlDbType.NVarChar, 50)
        param31.Value = Address.PostalCode
        oCmdAddBooking.Parameters.Add(param31)

        Dim param32 As SqlParameter = New SqlParameter("@CneeCountryKey", SqlDbType.Int, 4)

        Dim AddID = (From o In dbContext.NB_Store_Orders
                     Where o.OrderID = nOrderID
                     Select AddressID = nShippindAddressID).SingleOrDefault

        param32.Value = GetCountryKeyByAddressId(AddID)
        oCmdAddBooking.Parameters.Add(param32)

        Dim param33 As SqlParameter = New SqlParameter("@CneeCtcName", SqlDbType.NVarChar, 50)
        'param33.Value = (DNN.GetUserInfo(Me, rcbAccountHandler.SelectedValue)).DisplayName
        param33.Value = Address.AddressName
        oCmdAddBooking.Parameters.Add(param33)

        Dim param34 As SqlParameter = New SqlParameter("@CneeTel", SqlDbType.NVarChar, 50)
        param34.Value = ""
        oCmdAddBooking.Parameters.Add(param34)
        Dim param35 As SqlParameter = New SqlParameter("@CneeEmail", SqlDbType.NVarChar, 50)
        param35.Value = ""
        oCmdAddBooking.Parameters.Add(param35)
        Dim param36 As SqlParameter = New SqlParameter("@CneePreAlertFlag", SqlDbType.Bit)
        param36.Value = 0
        oCmdAddBooking.Parameters.Add(param36)
        Dim param37 As SqlParameter = New SqlParameter("@LogisticBookingKey", SqlDbType.Int, 4)
        param37.Direction = ParameterDirection.Output
        oCmdAddBooking.Parameters.Add(param37)
        Dim param38 As SqlParameter = New SqlParameter("@ConsignmentKey", SqlDbType.Int, 4)
        param38.Direction = ParameterDirection.Output
        oCmdAddBooking.Parameters.Add(param38)
        Try
            BookingFailed = False
            oConn.ConnectionString = SprintDB.GetConnectionString()
            oConn.Open()
            oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted, "AddBooking")
            oCmdAddBooking.Connection = oConn
            oCmdAddBooking.Transaction = oTrans
            oCmdAddBooking.ExecuteNonQuery()
            nBookingKey = CLng(oCmdAddBooking.Parameters("@LogisticBookingKey").Value.ToString)
            nConsignmentKey = CInt(oCmdAddBooking.Parameters("@ConsignmentKey").Value.ToString)

            If nBookingKey > 0 Then

                Dim nApproveOrderStatusId As Integer = Convert.ToInt64(ConfigurationManager.AppSettings("ORDER_APPROVE"))

                Dim IODetail = (From od In dbContext.NB_Store_OrderDetails
                                Where od.OrderID = nOrderID
                                Select OrderDetailID = od.OrderDetailID, Quantity = od.Quantity).ToList


                For Each OrderDetail In IODetail

                    Dim nOrderDetailId As Int64 = OrderDetail.OrderDetailID

                    Dim nProductKey As Int32 = GetProductKey(nOrderDetailId)

                    Dim nPickQuantity As Int32 = OrderDetail.Quantity

                    Dim oCmdAddStockItem As SqlCommand = New SqlCommand("spASPNET_LogisticMovement_Add", oConn)
                    oCmdAddStockItem.CommandType = CommandType.StoredProcedure

                    Dim param51 As SqlParameter = New SqlParameter("@UserKey", SqlDbType.Int, 4)
                    'param51.Value = nSprintAccountHandlerKey
                    param51.Value = 0 ' user key remains same
                    oCmdAddStockItem.Parameters.Add(param51)

                    Dim param52 As SqlParameter = New SqlParameter("@CustomerKey", SqlDbType.Int, 4)
                    param52.Value = nCustKey
                    oCmdAddStockItem.Parameters.Add(param52)

                    Dim param53 As SqlParameter = New SqlParameter("@LogisticBookingKey", SqlDbType.Int, 4)
                    param53.Value = nBookingKey
                    oCmdAddStockItem.Parameters.Add(param53)

                    Dim param54 As SqlParameter = New SqlParameter("@LogisticProductKey", SqlDbType.Int, 4)
                    param54.Value = nProductKey
                    oCmdAddStockItem.Parameters.Add(param54)

                    Dim param55 As SqlParameter = New SqlParameter("@LogisticMovementStateId", SqlDbType.NVarChar, 20)
                    param55.Value = "PENDING"
                    oCmdAddStockItem.Parameters.Add(param55)

                    Dim param56 As SqlParameter = New SqlParameter("@ItemsOut", SqlDbType.Int, 4)
                    param56.Value = nPickQuantity
                    oCmdAddStockItem.Parameters.Add(param56)

                    Dim param57 As SqlParameter = New SqlParameter("@ConsignmentKey", SqlDbType.Int, 8)
                    param57.Value = nConsignmentKey
                    oCmdAddStockItem.Parameters.Add(param57)

                    oCmdAddStockItem.Connection = oConn
                    oCmdAddStockItem.Transaction = oTrans
                    oCmdAddStockItem.ExecuteNonQuery()

                    Dim oCmdCompleteBooking As SqlCommand = New SqlCommand("spASPNET_LogisticBooking_Complete", oConn)
                    oCmdCompleteBooking.CommandType = CommandType.StoredProcedure

                    Dim param71 As SqlParameter = New SqlParameter("@LogisticBookingKey", SqlDbType.Int, 4)
                    param71.Value = nBookingKey

                    oCmdCompleteBooking.Parameters.Add(param71)
                    oCmdCompleteBooking.Connection = oConn
                    oCmdCompleteBooking.Transaction = oTrans
                    oCmdCompleteBooking.ExecuteNonQuery()

                Next
            Else
                BookingFailed = True
                'Log.WriteToFile("Booking Failed ")

                DotNetNuke.Services.Mail.Mail.SendEmail(
                FF_GLOBALS.PS_EMAIL_NOTIFIER, "mkazim4u@gmail.com", "Booking Failed - Invalid Product Reference Key - " & nOrderID,
                "Hi, Booking Failed -  Action Required.")


            End If

            If Not BookingFailed Then
                oTrans.Commit()
                PickOrder = nConsignmentKey
            Else
                oTrans.Rollback("AddBooking")
            End If
        Catch ex As SqlException

            oTrans.Rollback("AddBooking")

        Finally
            oConn.Close()

        End Try

    End Function

    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim EMAIL_ADMIN_DEPUTY_UK As String = (From con In dbContext.SNR_Configurations Where con.ConfigKey = "EMAIL_ADMIN_DEPUTY_UK" Select con.ConfigValue).SingleOrDefault.ToString
        Dim EMAIL_ADMIN_UK As String = (From con In dbContext.SNR_Configurations Where con.ConfigKey = "EMAIL_ADMIN_UK" Select con.ConfigValue).SingleOrDefault.ToString
        Dim EMAIL_FULFILMENT_UK As String = (From con In dbContext.SNR_Configurations Where con.ConfigKey = "EMAIL_FULFILMENT_UK" Select con.ConfigValue).SingleOrDefault.ToString

        'Dim EMAIL_ADMIN_DEPUTY_US As String = (From con In dbContext.SNR_Configurations Where con.ConfigKey = "EMAIL_ADMIN_DEPUTY_US" Select con).SingleOrDefault.ToString
        Dim EMAIL_ADMIN_US As String = (From con In dbContext.SNR_Configurations Where con.ConfigKey = "EMAIL_ADMIN_US" Select con.ConfigValue).SingleOrDefault.ToString
        'Dim EMAIL_FULFILMENT_US As String = (From con In dbContext.SNR_Configurations Where con.ConfigKey = "EMAIL_ADMIN_US" Select con).SingleOrDefault.ToString

        'Dim EMAIL_ADMIN_DEPUTY_ME As String = (From con In dbContext.SNR_Configurations Where con.ConfigKey = "EMAIL_ADMIN_DEPUTY_ME" Select con).SingleOrDefault.ToString
        Dim EMAIL_ADMIN_ME As String = (From con In dbContext.SNR_Configurations Where con.ConfigKey = "EMAIL_ADMIN_ME" Select con.ConfigValue).SingleOrDefault.ToString
        'Dim EMAIL_FULFILMENT_ME As String = (From con In dbContext.SNR_Configurations Where con.ConfigKey = "EMAIL_FULFILMENT_ME" Select con).SingleOrDefault.ToString


        Dim btnApprove As Button = sender

        Dim hidOrderID As HiddenField = btnApprove.NamingContainer.FindControl("hidOrderID")
        Dim hidOrderStatusID As HiddenField = btnApprove.NamingContainer.FindControl("hidOrderStatusID")

        Dim hidPortalID As HiddenField = btnApprove.NamingContainer.FindControl("hidPortalID")

        Dim rgActions As RadGrid = btnApprove.NamingContainer.FindControl("rgActions")

        'btnApprove.NamingContainer.FindControl("divOrderStatus").Visible = False

        'btnApprove.NamingContainer.FindControl("divActions").Visible = True

        Dim nPortalID As Integer = hidPortalID.Value
        Dim nOrderID As Integer = hidOrderID.Value
        Dim nOrderStatusID As Integer = hidOrderStatusID.Value

        pnOrderID = nOrderID


        Dim orderStatus = (From os In dbContext.NB_Store_OrderStatus
                          Where os.OrderStatusID = nOrderStatusID
                          Select os).Single


        Dim nConsignmentKey As Integer = PickOrder(pnOrderID)

        'pnStatusID = FF_GLOBALS.ORDER_APPROVE

        If nConsignmentKey > 0 Then

            If nPortalID = FF_GLOBALS.UK_PORTAL_ID Then

                If Not String.IsNullOrEmpty(EMAIL_ADMIN_UK) Then

                    DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, EMAIL_ADMIN_UK, "Consignment Key - For Order Number - " &
                                                            nOrderID, "Hi, <br>Consignment Key = " & nConsignmentKey)

                End If

            ElseIf nPortalID = FF_GLOBALS.US_PORTAL_ID Then

                If Not String.IsNullOrEmpty(EMAIL_ADMIN_US) Then

                    DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, EMAIL_ADMIN_US, "Consignment Key - For Order Number - " &
                                                            nOrderID, "Hi, <br>Consignment Key = " & nConsignmentKey)

                End If


            ElseIf nPortalID = FF_GLOBALS.ME_PORTAL_ID Then

                If Not String.IsNullOrEmpty(EMAIL_ADMIN_ME) Then

                    DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, EMAIL_ADMIN_ME, "Consignment Key - For Order Number - " &
                                                            nOrderID, "Hi, <br>Consignment Key = " & nConsignmentKey)

                End If


            End If

            dbContext.ExecuteCommand("update nb_store_orders set nb_store_orders.orderstatusid = {0} where nb_store_orders.orderid = {1}", FF_GLOBALS.ORDER_APPROVE, nOrderID)
            snr_Audit_Trail.RecordType = FF_GLOBALS.RECORD_TYPE_ORDER
            snr_Audit_Trail.SourceID = nOrderID
            snr_Audit_Trail.CreatedOn = DateTime.Now
            snr_Audit_Trail.AuditEvent = FF_GLOBALS.AUDIT_EVENT_ORDER_UPDATED
            snr_Audit_Trail.ChangeDetail = "Change From " + orderStatus.OrderStatusText + " To " + strOrderApprove
            snr_Audit_Trail.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            dbContext.SNR_AuditTrails.InsertOnSubmit(snr_Audit_Trail)
            dbContext.SubmitChanges()

        End If


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        rgOrders.Rebind()

        ExpandOrderRow(nOrderID)

        'rgActions.Rebind()

    End Sub

    Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim btnReject As Button = sender

        Dim hidOrderID As HiddenField = btnReject.NamingContainer.FindControl("hidOrderID")
        Dim hidOrderStatusID As HiddenField = btnReject.NamingContainer.FindControl("hidOrderStatusID")

        Dim lblRejectStatus As Label = btnReject.NamingContainer.FindControl("lblRejectStatus")


        Dim nOrderID As Integer = hidOrderID.Value
        Dim nOrderStatusID As Integer = hidOrderStatusID.Value

        pnOrderID = nOrderID

        Dim orderStatus = (From os In dbContext.NB_Store_OrderStatus
                         Where os.OrderStatusID = nOrderStatusID
                         Select os).Single


        dbContext.ExecuteCommand("update nb_store_orders set nb_store_orders.orderstatusid = {0} where nb_store_orders.orderid = {1}", FF_GLOBALS.ORDER_REJECT, nOrderID)

        lblRejectStatus.Visible = True

        snr_Audit_Trail.RecordType = FF_GLOBALS.RECORD_TYPE_ORDER
        snr_Audit_Trail.SourceID = nOrderID
        snr_Audit_Trail.CreatedOn = DateTime.Now
        snr_Audit_Trail.AuditEvent = FF_GLOBALS.AUDIT_EVENT_ORDER_UPDATED
        snr_Audit_Trail.ChangeDetail = "Changed From " + orderStatus.OrderStatusText + " To " + strOrderReject
        snr_Audit_Trail.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
        dbContext.SNR_AuditTrails.InsertOnSubmit(snr_Audit_Trail)
        dbContext.SubmitChanges()

        rgOrders.Rebind()

        ExpandOrderRow(nOrderID)


    End Sub

    Protected Sub rgOrders_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgOrders.ItemDataBound

        If e.Item.ItemType = GridItemType.Header Then

            Dim chkAwaitingAuthorization As CheckBox = e.Item.FindControl("chkAwaitingAuthorization")
            Dim chkApprove As CheckBox = e.Item.FindControl("chkApprove")
            Dim chkAwaitingFulfilment As CheckBox = e.Item.FindControl("chkAwaitingFulfilment")
            Dim chkFulfilled As CheckBox = e.Item.FindControl("chkFulfilled")
            Dim chkCancelled As CheckBox = e.Item.FindControl("chkCancelled")

            If pnApproveOrders > 0 Then
                chkApprove.Checked = True
            End If

            If pnAwaitingAuhtorizationOrders > 0 Then
                chkAwaitingAuthorization.Checked = True
            End If

            If pnAwaitingFulfilmentOrders > 0 Then
                chkAwaitingFulfilment.Checked = True
            End If

            If pnFulfilledOrders > 0 Then
                chkFulfilled.Checked = True
            End If

            If pnCancelledOrders > 0 Then
                chkCancelled.Checked = True
            End If

        ElseIf e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim lblStatus As Label = e.Item.FindControl("lblStatus")

            Dim hidStatusID As HiddenField = e.Item.FindControl("hidOrderStatusID")

            '''''''''''''''''''' Portal Name '''''''''''''''''''''''''''

            Dim lblPortalName As Label = e.Item.FindControl("lblPortalName")

            Dim imgPortal As System.Web.UI.WebControls.Image = e.Item.FindControl("imgPortal")

            Dim nPortalID As Integer = Convert.ToInt64(lblPortalName.Text)

            pi = pc.GetPortal(nPortalID)

            lblPortalName.Text = pi.PortalName




            If pi.PortalID = FF_GLOBALS.ME_PORTAL_ID Then

                imgPortal.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/Images/" + "UAE.gif"

            ElseIf pi.PortalID = FF_GLOBALS.US_PORTAL_ID Then

                imgPortal.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/Images/" + "USA.gif"

            ElseIf pi.PortalID = FF_GLOBALS.UK_PORTAL_ID Then

                imgPortal.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/Images/" + "UK.gif"

            End If



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim nStatusID As Integer = hidStatusID.Value
            Dim IOrderStatus = (From OrderStatus In dbContext.NB_Store_OrderStatus Where OrderStatus.OrderStatusID = nStatusID And OrderStatus.Lang = "XX"
                              Select orderstatustext = OrderStatus.OrderStatusText).SingleOrDefault

            lblStatus.Text = IOrderStatus


            If nStatusID = FF_GLOBALS.ORDER_FULFILLED Then

                e.Item.BackColor = Color.SpringGreen

            ElseIf nStatusID = FF_GLOBALS.ORDER_REJECT Then

                e.Item.BackColor = Color.Tomato

            ElseIf nStatusID = FF_GLOBALS.ORDER_WAITING_FOR_FULFIMENT Then

                e.Item.BackColor = Color.LightGreen

            ElseIf nStatusID = FF_GLOBALS.ORDER_APPROVE Then

                e.Item.BackColor = Color.SkyBlue

            ElseIf nStatusID = FF_GLOBALS.ORDER_WAITING_FOR_AUTHORIZATION Then

                e.Item.BackColor = Color.Yellow

            End If




            'Dim gridDataItem As GridDataItem = e.Item

            'Dim rcbPortal As RadComboBox = gridDataItem("PortalName").FindControl("rcbPortal")


            'rcbPortal.DataSource = pc.GetPortals()


        ElseIf e.Item.ItemType = Telerik.Web.UI.GridItemType.NestedView Then


            Dim hidOrderID As HiddenField = e.Item.FindControl("hidOrderID")
            Dim hidShippingAddressID As HiddenField = e.Item.FindControl("hidShippingAddressID")
            Dim hidBillingAddressID As HiddenField = e.Item.FindControl("hidBillingAddressID")
            Dim hidStatusID As HiddenField = e.Item.FindControl("hidOrderStatusID")
            Dim nStatusID As Integer = hidStatusID.Value


            Dim nOrderID As Integer = hidOrderID.Value
            Dim nShippingAddressID As Integer = hidShippingAddressID.Value
            Dim nBillingAddressID As Integer = hidBillingAddressID.Value

            pnOrderID = nOrderID

            ''''''''''''' Billing Addres ''''''''''''''''''''''''''

            'Dim IBillingAddress = (From Orders In dbContext.NB_Store_Orders Join Address In dbContext.NB_Store_Addresses
            '                      On Orders.BillingAddressID Equals Address.AddressID
            '                      Where Orders.OrderID = nOrderID And Orders.BillingAddressID = nBillingAddressID
            '                      Select Address).SingleOrDefault



            'Dim lblB_FirstName As Label = e.Item.FindControl("lblB_FirstName")
            'Dim lblB_LastName As Label = e.Item.FindControl("lblB_LastName")
            'Dim lblB_Company As Label = e.Item.FindControl("lblB_Company")
            'Dim lblB_Address As Label = e.Item.FindControl("lblB_Address")
            'Dim lblB_City As Label = e.Item.FindControl("lblB_City")
            'Dim lblB_Region As Label = e.Item.FindControl("lblB_Region")
            'Dim lblB_PostalCode As Label = e.Item.FindControl("lblB_PostalCode")
            'Dim lblB_CountryCode As Label = e.Item.FindControl("lblB_CountryCode")
            'Dim lblB_Phone As Label = e.Item.FindControl("lblB_Phone")
            'Dim lblB_MobileNo As Label = e.Item.FindControl("lblB_MobileNo")


            'If IBillingAddress IsNot Nothing Then

            '    lblB_FirstName.Text = IBillingAddress.AddressName
            '    lblB_LastName.Text = IBillingAddress.Extra1
            '    lblB_Company.Text = IBillingAddress.CompanyName
            '    lblB_Address.Text = IBillingAddress.Address2
            '    lblB_City.Text = IBillingAddress.City
            '    lblB_Region.Text = IBillingAddress.RegionCode
            '    lblB_PostalCode.Text = IBillingAddress.PostalCode
            '    lblB_CountryCode.Text = IBillingAddress.CountryCode
            '    lblB_Phone.Text = IBillingAddress.Phone1
            '    lblB_MobileNo.Text = IBillingAddress.Phone2


            'End If




            '''''''''''''''''''' Shipping Address '''''''''''''''''''''''''''

            Dim lblS_FirstName As Label = e.Item.FindControl("lblS_FirstName")
            Dim lblS_LastName As Label = e.Item.FindControl("lblS_LastName")
            Dim lblS_Company As Label = e.Item.FindControl("lblS_Company")
            Dim lblS_Address As Label = e.Item.FindControl("lblS_Address")
            Dim lblS_City As Label = e.Item.FindControl("lblS_City")
            Dim lblS_Region As Label = e.Item.FindControl("lblS_Region")
            Dim lblS_PostalCode As Label = e.Item.FindControl("lblS_PostalCode")
            Dim lblS_CountryCode As Label = e.Item.FindControl("lblS_CountryCode")
            Dim lblS_Phone As Label = e.Item.FindControl("lblS_Phone")
            Dim lblS_MobileNo As Label = e.Item.FindControl("lblS_MobileNo")

            Dim IShippingAddress = (From Orders In dbContext.NB_Store_Orders Join Address In dbContext.NB_Store_Addresses
            On Orders.ShippingAddressID Equals Address.AddressID
            Where Orders.OrderID = nOrderID And Orders.ShippingAddressID = nShippingAddressID
            Select Address).SingleOrDefault


            If IShippingAddress IsNot Nothing Then

                lblS_FirstName.Text = IShippingAddress.AddressName
                lblS_LastName.Text = IShippingAddress.Extra1
                lblS_Company.Text = IShippingAddress.CompanyName
                lblS_Address.Text = IShippingAddress.Address2
                lblS_City.Text = IShippingAddress.City
                lblS_Region.Text = IShippingAddress.RegionCode
                lblS_PostalCode.Text = IShippingAddress.PostalCode
                lblS_CountryCode.Text = IShippingAddress.CountryCode
                lblS_Phone.Text = IShippingAddress.Phone1
                lblS_MobileNo.Text = IShippingAddress.Phone2


            End If


            '''''''''''''''''''''''''''''''' Items ''''''''''''''''''''''''''''''''''''''''

            Dim rgItems As Telerik.Web.UI.RadGrid = e.Item.FindControl("rgItems")

            Dim IItems = (From OrderDetail In dbContext.NB_Store_OrderDetails Join Model In dbContext.NB_Store_Models On OrderDetail.ModelID Equals Model.ModelID
                         Join Products In dbContext.NB_Store_Products On Products.ProductID Equals Model.ProductID
                         Join ProdLang In dbContext.NB_Store_ProductLangs On Products.ProductID Equals ProdLang.ProductID
                         Where OrderDetail.OrderID = nOrderID
                         Select OrderDetail.OrderDetailID, ProdLang.ProductName, OrderDetail.UnitCost, OrderDetail.Quantity).Distinct

            rgItems.DataSource = IItems
            rgItems.DataBind()



            ''''''''''''''''''''''''''''''''' Actions ''''''''''''''''''''''''''''''''''''''''''

            Dim rgActions As Telerik.Web.UI.RadGrid = e.Item.FindControl("rgActions")

            Dim IActions = From Status In dbContext.NB_Store_OrderStatus
                           Where Status.OrderStatusID = 120 Or Status.OrderStatusID > 120

            rgActions.DataSource = IActions
            rgActions.DataBind()


            If nStatusID = FF_GLOBALS.ORDER_RECEIVED Then

                e.Item.FindControl("divOrderStatus").Visible = True
                e.Item.FindControl("divActions").Visible = False

            ElseIf nStatusID = FF_GLOBALS.ORDER_REJECT Then

                e.Item.FindControl("lblRejectStatus").Visible = True
                e.Item.FindControl("divOrderStatus").Visible = False
                e.Item.FindControl("divActions").Visible = False

            Else

                e.Item.FindControl("divOrderStatus").Visible = False
                e.Item.FindControl("divActions").Visible = True

            End If

            '''''''''''''''''''''''''''' Order Log '''''''''''''''''''''''''''''''''''


            Dim rgOrderLog As Telerik.Web.UI.RadGrid = e.Item.FindControl("rgOrderLog")

            Dim IOrderLog = From OLog In dbContext.SNR_AuditTrails
                           Where OLog.SourceID = nOrderID
                           Order By OLog.CreatedOn Descending

            rgOrderLog.DataSource = IOrderLog
            rgOrderLog.DataBind()



        End If


    End Sub

    Protected Sub rgOrderLog_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)

        If (TypeOf e.Item Is GridDataItem) Or (e.Item.ItemType = GridItemType.AlternatingItem) Then

            Dim item As GridDataItem = e.Item

            Dim txtCreatedBy As Label = item.FindControl("txtCreatedBy")

            Dim nUserId As Integer = Convert.ToInt64(txtCreatedBy.Text)

            Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, nUserId)

            If txtCreatedBy IsNot Nothing Then

                If userInfo IsNot Nothing Then

                    txtCreatedBy.Text = userInfo.Username

                End If

            End If




        End If


    End Sub

    Protected Sub rgActions_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then



            '''''''''''''''the problem is that i hv opened the nested grid but i didn't get the order id '''''''''''''

            'Dim nvItem As GridNestedViewItem = e.Item.NamingContainer.NamingContainer.NamingContainer

            'Dim hidOrderID As HiddenField = nvItem.FindControl("hidOrderID") 

            'Dim nOrderID As Integer = hidOrderID.Value

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim chkStatus As CheckBox = e.Item.FindControl("chkStatus")

            Dim hidOSID As HiddenField = e.Item.FindControl("hidOSID")

            Dim nOSID As Integer = hidOSID.Value

            'Dim nOrderID As Integer = pnOrderID

            If pnOrderID > 0 Then

                Dim Order = (From o In dbContext.NB_Store_Orders
                            Where o.OrderID = pnOrderID
                            Select OrderStatusID = o.OrderStatusID).Single

                If nOSID = Order.GetValueOrDefault Or nOSID < Order.GetValueOrDefault Then

                    chkStatus.Checked = True

                End If

            End If

        End If

    End Sub
    Protected Sub rgOrders_ItemCreated(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgOrders.ItemCreated

        'If TypeOf e.Item Is GridFilteringItem Then

        '    Dim rcbShop As RadComboBox = e.Item.FindControl("rcbPortal")

        '    rcbShop.DataSource = pc.GetPortals

        'End If

    End Sub



    Protected Sub ExpandOrderRow(ByVal nOrderID As Integer)

        For Each item As GridDataItem In rgOrders.Items

            If Convert.ToInt64(item("OrderID").Text) = nOrderID Then

                item.Expanded = True

            End If

        Next

        'rgOrders.Items.fi 

    End Sub




End Class