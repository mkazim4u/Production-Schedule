Imports Telerik.Web.UI
Imports System.Text
Imports System.Net
Imports System.IO
Imports DentonDataLayer


Partial Class SNR_ImportProductList
    Inherits System.Web.UI.UserControl

    Private dnnModel As New DentonDataLayer.EntitiesModel



    'Private ssCountry As New DentonDataLayer.Country



    Private productImages As String = "productimages"
    Private pc As New DotNetNuke.Entities.Portals.PortalController

    Private imagesPath As String = "http://www.sprintexpress.co.uk/common/prod_images/jpgs/"
    'Private ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
    'Private DestinationPath As String = ps.HomeDirectoryMapPath & productImages

    Public Property pdtProducts() As DataTable
        Get
            Dim o As Object = ViewState("pdtProducts")
            If o Is Nothing Then
                Return Nothing
            End If
            Return DirectCast(o, DataTable)
        End Get
        Set(ByVal Value As DataTable)
            ViewState("pdtProducts") = Value
        End Set
    End Property

    Public Property pdtShops() As DataTable
        Get
            Dim o As Object = ViewState("pdtShops")
            If o Is Nothing Then
                Return Nothing
            End If
            Return DirectCast(o, DataTable)
        End Get
        Set(ByVal Value As DataTable)
            ViewState("pdtShops") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Call InitializeControls()

        End If

    End Sub

    Protected Sub InitializeControls()

        rcbCategory.Items.Clear()
        rlbNBStoreProducts.Items.Clear()
        rlbStockProducts.Items.Clear()

        Call PopulateCustomerDropDown(rcbCustomer)
        Call PopulateCustomerDropDown(rcbCustomerImport)
        Call PopulatePortalsDropDown(rcbPortals)
        Call PopulateCategories()
        'Call BindShopGrid(-1)
        Call BindOrders()
        Call FillNbStoreProducts()


    End Sub

    Protected Sub PopulateCustomerDropDown(ByRef rcb As RadComboBox)

        rcb.Items.Clear()
        Dim sql As String = "SELECT CustomerAccountCode +  ' (' + CustomerName + ')' 'Customer', CustomerKey FROM Customer WHERE CustomerStatusId = 'ACTIVE' AND ISNULL(AccountHandlerKey, 0) > 0 ORDER BY CustomerAccountCode"
        Dim dt As DataTable = SprintDB.Query(sql)
        rcb.DataTextField = "Customer"
        rcb.DataValueField = "CustomerKey"
        rcb.DataSource = dt
        rcb.DataBind()

        rcb.Items.Insert(0, New RadComboBoxItem("- Select Customer -", "-1"))

    End Sub

    Protected Sub PopulatePortalsDropDown(ByRef rcb As RadComboBox)

        rcb.Items.Clear()
        Dim al As ArrayList = pc.GetPortals()

        rcb.Items.Add(New RadComboBoxItem("- Select Shop -", "-1"))

        For Each pi As PortalInfo In al

            Dim rcbItem As New RadComboBoxItem
            rcbItem.Text = pi.PortalName
            rcbItem.Value = pi.PortalID
            rcb.Items.Add(rcbItem)

        Next



    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Call AddShop()

    End Sub

    Protected Sub btnExportOrders_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportOrders.Click

        Call PickItems()

    End Sub

    Protected Sub BindOrders()

        Dim sql As String = "select * from NB_Store_Orders"
        Dim dtOrders As DataTable = DNNDB.Query(sql)
        If dtOrders IsNot Nothing And dtOrders.Rows.Count <> 0 Then

            gvOrders.DataSource = dtOrders
            gvOrders.DataBind()

        End If

    End Sub

    Protected Function PickItems() As Int32

        Dim sql As String = "select * from NB_Store_Orders where orderstatusid=80"
        Dim dtOrders As DataTable = DNNDB.Query(sql)
        If dtOrders IsNot Nothing And dtOrders.Rows.Count <> 0 Then

            gvOrders.DataSource = dtOrders
            gvOrders.DataBind()

        End If

        PickItems = 0
        Dim nBookingKey As Int32
        Dim nConsignmentKey As Int32
        Dim BookingFailed As Boolean
        Dim oConn As New SqlConnection
        'Dim nSprintAccountHandlerKey As Int32 = GetSprintAccountHandlerKey()
        Dim nSprintAccountHandlerKey As Int32 = 0
        If FF_SprintDBMode.GetDebugMode Then
            oConn.ConnectionString = SprintDB_Test.GetStockSystemConnectionString()
        Else
            oConn.ConnectionString = SprintDB.GetStockSystemConnectionString()
        End If



        For Each rowOrder As DataRow In dtOrders.Rows

            Dim oTrans As SqlTransaction
            Dim oCmdAddBooking As SqlCommand = New SqlCommand("spASPNET_StockBooking_Add3", oConn)
            oCmdAddBooking.CommandType = CommandType.StoredProcedure

            Dim dtCustAdd As DataTable = GetShippingAddressFromOrderId(rowOrder("orderid"))

            Dim param1 As SqlParameter = New SqlParameter("@UserProfileKey", SqlDbType.Int, 4)
            'param1.Value = nSprintAccountHandlerKey
            param1.Value = 0
            oCmdAddBooking.Parameters.Add(param1)

            Dim param2 As SqlParameter = New SqlParameter("@CustomerKey", SqlDbType.Int, 4)
            'param2.Value = FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem)
            param2.Value = GetCustomerKeyByPortalId(DNN.GetPMB(Me).PortalId)
            oCmdAddBooking.Parameters.Add(param2)

            Dim param2a As SqlParameter = New SqlParameter("@BookingOrigin", SqlDbType.NVarChar, 20)
            param2a.Value = "WEB_BOOKING"
            oCmdAddBooking.Parameters.Add(param2a)

            Dim param3 As SqlParameter = New SqlParameter("@BookingReference1", SqlDbType.NVarChar, 25)
            'param3.Value = "Fulfilment Job # " & pnJobID.ToString
            param3.Value = ""

            oCmdAddBooking.Parameters.Add(param3)

            Dim param4 As SqlParameter = New SqlParameter("@BookingReference2", SqlDbType.NVarChar, 25)
            'param4.Value = tbJobName.Text.Trim()
            param4.Value = ""
            oCmdAddBooking.Parameters.Add(param4)

            Dim param5 As SqlParameter = New SqlParameter("@BookingReference3", SqlDbType.NVarChar, 50)
            'param5.Value = txtRwBasketAIMSCustRef1.Text.Trim()
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
            param20.Value = GetCountryKeyByAddressId(rowOrder("shippingaddressid"))
            oCmdAddBooking.Parameters.Add(param20)

            Dim param21 As SqlParameter = New SqlParameter("@CnorCtcName", SqlDbType.NVarChar, 50)
            Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetCurrentUserInfo
            param21.Value = uiCurrentUser.DisplayName
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
            param25.Value = dtCustAdd.Rows(0)("AddressName")
            oCmdAddBooking.Parameters.Add(param25)

            Dim param26 As SqlParameter = New SqlParameter("@CneeAddr1", SqlDbType.NVarChar, 50)
            param26.Value = dtCustAdd.Rows(0)("Address1")
            oCmdAddBooking.Parameters.Add(param26)

            Dim param27 As SqlParameter = New SqlParameter("@CneeAddr2", SqlDbType.NVarChar, 50)
            param27.Value = dtCustAdd.Rows(0)("Address2")
            oCmdAddBooking.Parameters.Add(param27)

            Dim param28 As SqlParameter = New SqlParameter("@CneeAddr3", SqlDbType.NVarChar, 50)
            param28.Value = ""
            oCmdAddBooking.Parameters.Add(param28)

            Dim param29 As SqlParameter = New SqlParameter("@CneeTown", SqlDbType.NVarChar, 50)
            param29.Value = dtCustAdd.Rows(0)("city")
            oCmdAddBooking.Parameters.Add(param29)

            Dim param30 As SqlParameter = New SqlParameter("@CneeState", SqlDbType.NVarChar, 50)
            param30.Value = dtCustAdd.Rows(0)("city")
            oCmdAddBooking.Parameters.Add(param30)

            Dim param31 As SqlParameter = New SqlParameter("@CneePostCode", SqlDbType.NVarChar, 50)
            param31.Value = dtCustAdd.Rows(0)("postalcode")
            oCmdAddBooking.Parameters.Add(param31)

            Dim param32 As SqlParameter = New SqlParameter("@CneeCountryKey", SqlDbType.Int, 4)
            param32.Value = GetCountryKeyByAddressId(rowOrder("shippingaddressid"))
            oCmdAddBooking.Parameters.Add(param32)
            'If ddlRwBasketCountry.SelectedItem Is Nothing Or ddlRwBasketCountry.SelectedValue = "0" Then
            '    param32.Value = 222
            '    oCmdAddBooking.Parameters.Add(param32)
            'Else
            '    param32.Value = Convert.ToInt64(ddlRwBasketCountry.SelectedValue)
            '    oCmdAddBooking.Parameters.Add(param32)
            'End If

            Dim param33 As SqlParameter = New SqlParameter("@CneeCtcName", SqlDbType.NVarChar, 50)
            'param33.Value = (DNN.GetUserInfo(Me, rcbAccountHandler.SelectedValue)).DisplayName
            param33.Value = dtCustAdd.Rows(0)("AddressName")
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
                oConn.Open()
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted, "AddBooking")
                oCmdAddBooking.Connection = oConn
                oCmdAddBooking.Transaction = oTrans
                oCmdAddBooking.ExecuteNonQuery()
                nBookingKey = CLng(oCmdAddBooking.Parameters("@LogisticBookingKey").Value.ToString)
                nConsignmentKey = CInt(oCmdAddBooking.Parameters("@ConsignmentKey").Value.ToString)
                Dim nOrderStatusId As Integer = 110
                Dim sqlUpdateOrderStatus As String = "update nb_store_orders set orderstatusid = " & nOrderStatusId & " where orderid = " & rowOrder("orderid")
                DNNDB.Query(sqlUpdateOrderStatus)

                If nBookingKey > 0 Then
                    Dim dtOrderDetails As DataTable = GetOrderDetailsByOrderId(rowOrder("orderid"))
                    For Each drOrderDetails As DataRow In dtOrderDetails.Rows


                        'Dim nProductKey As Int32 = drOrderDetails("LogisticProductKey")
                        'Dim nPickQuantity As Int32 = drOrderDetails("QtyToPick")

                        Dim nProductKey As Int32 = GetProductKey(rowOrder("orderid"))
                        Dim nPickQuantity As Int32 = drOrderDetails("quantity")

                        Dim oCmdAddStockItem As SqlCommand = New SqlCommand("spASPNET_LogisticMovement_Add", oConn)
                        oCmdAddStockItem.CommandType = CommandType.StoredProcedure

                        Dim param51 As SqlParameter = New SqlParameter("@UserKey", SqlDbType.Int, 4)
                        'param51.Value = nSprintAccountHandlerKey
                        param51.Value = 0
                        oCmdAddStockItem.Parameters.Add(param51)

                        Dim param52 As SqlParameter = New SqlParameter("@CustomerKey", SqlDbType.Int, 4)
                        param52.Value = GetCustomerKeyByPortalId(DNN.GetPMB(Me).PortalId)
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
                    Next
                    Dim oCmdCompleteBooking As SqlCommand = New SqlCommand("spASPNET_LogisticBooking_Complete", oConn)
                    oCmdCompleteBooking.CommandType = CommandType.StoredProcedure

                    Dim param71 As SqlParameter = New SqlParameter("@LogisticBookingKey", SqlDbType.Int, 4)
                    param71.Value = nBookingKey

                    oCmdCompleteBooking.Parameters.Add(param71)
                    oCmdCompleteBooking.Connection = oConn
                    oCmdCompleteBooking.Transaction = oTrans
                    oCmdCompleteBooking.ExecuteNonQuery()
                Else
                    BookingFailed = True
                    WebMsgBox.Show("Booking key = 0")
                End If
                If Not BookingFailed Then
                    oTrans.Commit()
                    PickItems = nConsignmentKey
                Else
                    oTrans.Rollback("AddBooking")
                End If
            Catch ex As SqlException
                WebMsgBox.Show("Failed to submit order: " & ex.ToString)
                oTrans.Rollback("AddBooking")
            Finally
                oConn.Close()
            End Try

        Next



    End Function

    Protected Function GetProductKey(ByVal nOrderId As Int32) As Int32

        GetProductKey = 0

        Dim sql As String = "select productref from NB_Store_Products where ProductID  = (select productid from NB_Store_Model where ModelID  = (select modelid from nb_store_orderdetails where orderid = " & nOrderId & "))"
        Dim dt As DataTable = DNNDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            GetProductKey = Convert.ToInt32(dt.Rows(0)("productref"))
        End If

        Return GetProductKey

    End Function

    Protected Function GetOrderDetailsByOrderId(ByVal nOrderId As Int32) As DataTable

        GetOrderDetailsByOrderId = Nothing

        Dim sql As String = "select * from nb_store_orderdetails where orderid = " & nOrderId
        Dim dt As DataTable = DNNDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            GetOrderDetailsByOrderId = dt

        End If

        Return GetOrderDetailsByOrderId

    End Function

    Protected Function GetShippingAddressFromOrderId(ByVal nOrderId As Int32) As DataTable

        GetShippingAddressFromOrderId = Nothing
        Dim sql As String = "select * from nb_store_address where addressid = (select shippingaddressid from nb_store_orders where orderid = " & nOrderId & ")"
        Dim dt As DataTable = DNNDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            GetShippingAddressFromOrderId = dt
        End If

        Return GetShippingAddressFromOrderId

    End Function

    Protected Function GetCountryKeyByAddressId(ByVal nshippingAddId As Int32) As Int32
        GetCountryKeyByAddressId = "222"

        Dim countryCode As String = String.Empty
        Dim sqlNB As String = "select * from nb_store_address where addressid = " & nshippingAddId
        Dim dtAddress As DataTable = DNNDB.Query(sqlNB)
        If dtAddress IsNot Nothing And dtAddress.Rows.Count <> 0 Then

            countryCode = dtAddress.Rows(0)("countrycode").ToString()

        End If
        Dim sql As String = "select top 1 * from Logistics.dbo.Country where CountryId2 ='" & countryCode & "'"
        Dim dt As DataTable = SprintDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            GetCountryKeyByAddressId = Convert.ToInt32(dt.Rows(0)("countrykey"))
        End If

        Return GetCountryKeyByAddressId

    End Function

    Protected Function GetCustomerKeyByPortalId(ByVal nPortalId As Integer) As Int32

        GetCustomerKeyByPortalId = 0
        Dim sql As String = "select sprintcustomerkey from customerportalmapping where portalId = " & nPortalId
        Dim dt As DataTable = DNNDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            GetCustomerKeyByPortalId = Convert.ToInt64(dt.Rows(0)("sprintcustomerkey"))
        End If

        Return GetCustomerKeyByPortalId

    End Function

    Protected Sub AddShop()

        If IsShopUnAssigned() Then

            Dim ObjPC As New CustomerPortalMapping

            ObjPC.PortalID = rcbPortals.SelectedValue
            ObjPC.PortalName = rcbPortals.Text
            ObjPC.SprintCustomerKey = rcbCustomer.SelectedValue
            ObjPC.CustomerName = rcbCustomer.Text
            ObjPC.CreatedOn = DateTime.Now
            ObjPC.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ObjPC.Add()
            rapImportProducts.Alert(rcbPortals.Text & " assigned To " & rcbCustomer.Text)
            Call BindShopGrid(-1)
            'WebMsgBox.Show(rcbPortals.Text & " assigned To " & rcbCustomer.Text)
        Else
            rapImportProducts.Alert(rcbPortals.Text & " is already assigned To " & rcbCustomer.Text)
            'WebMsgBox.Show(rcbPortals.Text & " is already assigned To so" & rcbCustomer.Text)
        End If



    End Sub

    Protected Function IsShopUnAssigned() As Boolean

        IsShopUnAssigned = True

        Dim sql As String = "select * from CustomerPortalMapping where portalId = " & rcbPortals.SelectedValue
        Dim dt As DataTable = DNNDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            IsShopUnAssigned = False
        End If

        Return IsShopUnAssigned

    End Function

    Protected Function FetchShops() As DataTable

        Dim sb As New StringBuilder

        sb.Append("select cpm.ID, cpm.PortalID ,cpm.SprintCustomerKey , cpm.PortalName 'Portal' , cpm.CustomerName 'Customer' ")
        'sb.Append("(select c.CustomerName from Logistics.dbo.Customer c where c.CustomerKey = SprintCustomerKey ) 'customer', ")
        'sb.Append("(select pl.portalname from PortalLocalization pl where pl.PortalID = cpm.PortalID) 'portal' ")
        sb.Append("from CustomerPortalMapping cpm")



        Dim sql As String = sb.ToString

        pdtShops = DNNDB.Query(sql)

        'Call BindShopGrid(-1)

        Return pdtShops

    End Function

    Protected Sub BindShopGrid(ByVal nEditIndex As Integer)

        gvShop.EditIndex = nEditIndex
        gvShop.DataSource = FetchShops()
        gvShop.DataBind()

    End Sub

    Protected Sub btnImportProducts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportProducts.Click

        UpdateNBStore()

    End Sub

    Protected Sub FillNbStoreProducts()

        Dim sb As New StringBuilder
        Dim nPortalId As Integer = DNN.GetPMB(Me).PortalId
        sb.Append("select ProductName, productref, nb_store_products.productid from nb_store_productlang inner join nb_store_products on nb_store_productlang.ProductId = nb_store_products.ProductId ")
        sb.Append("where nb_store_products.portalid = " & nPortalId)

        Dim sql As String = sb.ToString()

        Dim dt As DataTable = DNNDB.Query(sql)

        If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            For Each dr As DataRow In dt.Rows

                Dim sProductName As String = dr("productname")
                Dim nProductId As Integer = dr("productid")
                'Dim nProductRef As Integer = dr("productref") '''' key in the stock system

                rlbNBStoreProducts.Items.Add(New RadListBoxItem(sProductName, nProductId))

            Next

        End If

    End Sub

    Protected Sub rcbPortals_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)

    End Sub

    'Protected Sub rcbCustomerImport_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)

    '    'Dim sql As String = "select portalId from CustomerPortalMapping where sprintCustomerKey = " & rcbCustomerImport.SelectedValue
    '    'Dim dt As DataTable = DNNDB.Query(sql)
    '    'If dt IsNot Nothing And dt.Rows.Count <> 0 Then
    '    '    Dim dr As DataRow = dt.Rows(0)
    '    '    Dim portalInfo As PortalInfo = pc.GetPortal(dr("portalId"))
    '    '    lblPortal.Text = portalInfo.PortalName
    '    'Else
    '    '    WebMsgBox.Show("Shop doesn't exist")
    '    'End If


    '    Dim sql As String = "select ModelRef 'sprintproductkey', modelname from nb_store_model sm inner join NB_Store_ModelLang sml on sml.ModelID = sm.ModelID where sm.ModelRef = '" & rcbCustomerImport.SelectedValue & "'"
    '    Dim dt As DataTable = DNNDB.Query(sql)
    '    If dt IsNot Nothing And dt.Rows.Count <> 0 Then
    '        Dim dr As DataRow = dt.Rows(0)
    '        Dim portalInfo As PortalInfo = pc.GetPortal(dr("portalId"))
    '        lblPortal.Text = portalInfo.PortalName
    '    Else
    '        WebMsgBox.Show("Shop doesn't exist")
    '    End If

    'End Sub

    Protected Sub lnkbtnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim lb As LinkButton = sender

    End Sub

    Protected Sub lnkbtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender
        Dim gvr As GridViewRow = lb.NamingContainer
        Dim rowIndex As Integer = gvr.RowIndex

        Call BindShopGrid(rowIndex)


    End Sub

    Protected Sub lnkbtnShopName_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'gdv = New DataView(pdtShop)
        'gdv.Sort = "ShopName ASC"
        'pdtShop = gdv.ToTable()
        'gvShop.DataSource = pdtShop
        'gvShop.DataBind()

    End Sub

    Protected Sub gvShop_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvShop.RowUpdating

        Dim gvr As GridViewRow = gvShop.Rows(e.RowIndex)
        Dim gvrcbCustomer As RadComboBox = gvr.FindControl("gvrcbCustomer")
        Dim gvrcbPortals As RadComboBox = gvr.FindControl("gvrcbPortals")
        Dim hidId As HiddenField = gvr.FindControl("hidID")

        Dim obj_CP As New CustomerPortalMapping()
        obj_CP.PortalID = gvrcbPortals.SelectedValue
        obj_CP.PortalName = gvrcbPortals.SelectedItem.Text
        obj_CP.SprintCustomerKey = gvrcbCustomer.SelectedValue
        obj_CP.CustomerName = gvrcbCustomer.SelectedItem.Text
        obj_CP.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
        obj_CP.CreatedOn = DateTime.Now
        obj_CP.Update(hidId.Value)

        Call BindShopGrid(-1)


    End Sub

    Protected Sub gvShop_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvShop.RowEditing

        Call BindShopGrid(e.NewEditIndex)

    End Sub

    Protected Sub gvShop_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvShop.RowDeleting

        Dim gvr As GridViewRow = gvShop.Rows(e.RowIndex)
        Dim hidShopId As HiddenField = gvr.FindControl("hidID")
        Dim nID As Integer = hidShopId.Value

        Dim obj_CP As New CustomerPortalMapping()
        obj_CP.Delete(nID)
        Call BindShopGrid(-1)


    End Sub

    Protected Sub gvShop_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvShop.RowDataBound

        If (e.Row.RowState And DataControlRowState.Edit) > 0 OrElse (e.Row.RowState And DataControlRowState.Insert) > 0 Then

            Dim gvrcbCustomer As RadComboBox = e.Row.FindControl("gvrcbCustomer")
            Dim gvrcbPortals As RadComboBox = e.Row.FindControl("gvrcbPortals")
            Dim imgUpdate As System.Web.UI.WebControls.Image = e.Row.FindControl("imgUpdate")
            Dim imgCancel As System.Web.UI.WebControls.Image = e.Row.FindControl("imgCancel")

            PopulateCustomerDropDown(gvrcbCustomer)
            PopulatePortalsDropDown(gvrcbPortals)

            gvrcbCustomer.SelectedValue = DirectCast(e.Row.DataItem, DataRowView)("SprintCustomerkey").ToString()
            gvrcbPortals.SelectedValue = DirectCast(e.Row.DataItem, DataRowView)("portalId").ToString()

            imgUpdate.ImageUrl = "~/portals/" & DNN.GetPMB(Me).PortalId & "/Images/Update.gif"
            imgCancel.ImageUrl = "~/portals/" & DNN.GetPMB(Me).PortalId & "/Images/Cancel.gif"

        ElseIf (e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowState = DataControlRowState.Normal Or e.Row.RowState = DataControlRowState.Alternate) Then

            Dim imgEdit As System.Web.UI.WebControls.Image = e.Row.FindControl("imgEdit")
            Dim imgDelete As System.Web.UI.WebControls.Image = e.Row.FindControl("imgDelete")

            imgEdit.ImageUrl = "~/portals/" & DNN.GetPMB(Me).PortalId & "/Images/Edit.gif"
            imgDelete.ImageUrl = "~/portals/" & DNN.GetPMB(Me).PortalId & "/Images/delete.png"

        End If


    End Sub

    Protected Sub gvShop_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvShop.RowCancelingEdit
        Call BindShopGrid(-1)
    End Sub
    Protected Sub gvrcbPortals_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)


    End Sub
    Protected Sub rcbCustomerImport_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)

        'rlbNBStoreProducts.Items.Clear()
        Dim sb As StringBuilder = New StringBuilder()
        'Dim sqlParamCustomer(0) As SqlParameter
        'sqlParamCustomer(0).Value = 579

        sb.Append("SELECT DISTINCT(lp.LogisticProductKey) 'SprintProductKey', lp.ProductCode 'ProductCode', lp.ProductDescription 'ProductDescription',")
        sb.Append("lp.ProductCategory 'Category',")
        sb.Append("lp.SubCategory 'SubCategory',")
        sb.Append("lp.UnitValue 'Price',")
        sb.Append("lp.UnitWeightGrams 'Weight',")
        sb.Append("QuantityAvailable = CASE ISNUMERIC((select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey))")
        sb.Append("WHEN 0 THEN 0 ELSE (select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey)")
        sb.Append("END,")
        sb.Append("lp.OriginalImage 'Image',")
        sb.Append("lp.ThumbNailImage 'ThumbNailImage'")
        sb.Append("FROM LogisticProduct AS lp ")
        sb.Append("LEFT OUTER JOIN LogisticProductLocation AS lpl ")
        sb.Append("ON lp.LogisticProductKey = lpl.LogisticProductKey ")
        sb.Append("INNER JOIN UserProductProfile As upp ")
        sb.Append("ON lp.LogisticProductKey = upp.ProductKey ")
        sb.Append("WHERE lp.CustomerKey = " & rcbCustomerImport.SelectedValue)
        sb.Append("AND lp.DeletedFlag = 'N' ")
        sb.Append("AND lp.ArchiveFlag = 'N'")

        Dim dt As DataTable = SprintDB.Query(sb.ToString())
        pdtProducts = dt

        If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            For Each dr As DataRow In dt.Rows
                Dim rlbItem As New RadListBoxItem
                rlbItem.Text = dr("ProductCode")
                rlbItem.Value = dr("SprintProductKey")
                rlbStockProducts.Items.Add(rlbItem)

            Next

        End If

    End Sub

    Protected Sub rcbCustomer_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)

        'Dim sb As StringBuilder = New StringBuilder()
        ''Dim sqlParamCustomer(0) As SqlParameter
        ''sqlParamCustomer(0).Value = 579

        'sb.Append("SELECT DISTINCT(lp.LogisticProductKey) 'SprintProductKey', lp.ProductCode 'ProductCode', lp.ProductDescription 'ProductDescription',")
        'sb.Append("lp.ProductCategory 'Category',")
        'sb.Append("lp.SubCategory 'SubCategory',")
        'sb.Append("lp.UnitValue 'Price',")
        'sb.Append("lp.UnitWeightGrams 'Weight',")
        'sb.Append("QuantityAvailable = CASE ISNUMERIC((select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey))")
        'sb.Append("WHEN 0 THEN 0 ELSE (select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey)")
        'sb.Append("END,")
        'sb.Append("lp.OriginalImage 'Image',")
        'sb.Append("lp.ThumbNailImage 'ThumbNailImage'")
        'sb.Append("FROM LogisticProduct AS lp ")
        'sb.Append("LEFT OUTER JOIN LogisticProductLocation AS lpl ")
        'sb.Append("ON lp.LogisticProductKey = lpl.LogisticProductKey ")
        'sb.Append("INNER JOIN UserProductProfile As upp ")
        'sb.Append("ON lp.LogisticProductKey = upp.ProductKey ")
        'sb.Append("WHERE lp.CustomerKey = " & rcbCustomer.SelectedValue)
        'sb.Append("AND lp.DeletedFlag = 'N' ")
        'sb.Append("AND lp.ArchiveFlag = 'N'")

        'Dim dt As DataTable = SprintDB.Query(sb.ToString())
        'pdtProducts = dt

        'If dt IsNot Nothing And dt.Rows.Count <> 0 Then

        '    For Each dr As DataRow In dt.Rows
        '        Dim rlbItem As New RadListBoxItem
        '        rlbItem.Text = dr("ProductCode")
        '        rlbItem.Value = dr("SprintProductKey")
        '        rlbStockProducts.Items.Add(rlbItem)

        '    Next

        'End If

    End Sub

    Protected Function IsProductExist(ByVal nProductId As Integer) As Boolean

        IsProductExist = False

        Dim sb As New StringBuilder
        Dim nPortalId As Integer = DNN.GetPMB(Me).PortalId
        sb.Append("select ProductName, productref, nb_store_products.productid from nb_store_productlang inner join nb_store_products on nb_store_productlang.ProductId = nb_store_products.ProductId ")
        sb.Append("where nb_store_products.portalid = " & nPortalId & "and nb_store_products.productid = " & nProductId)

        Dim sql As String = sb.ToString()
        Dim dt As DataTable = DNNDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            IsProductExist = True
        End If

        Return IsProductExist

    End Function

    Protected Sub PopulateCategories()

        Dim sb As New StringBuilder
        Dim nPortalId As Integer
        nPortalId = DNN.GetPMB(Me).PortalId
        sb.Append("select nb_store_categories.categoryid, categoryname from nb_store_categorylang ")
        sb.Append("inner join nb_store_categories on nb_store_categorylang.categoryid = nb_store_categories.categoryid ")
        sb.Append("where  portalid = " & nPortalId)

        Dim sql As String = sb.ToString()

        Dim dt As DataTable = DNNDB.Query(sql)

        If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            rcbCategory.DataSource = dt
            rcbCategory.DataTextField = "categoryname"
            rcbCategory.DataValueField = "categoryid"
            rcbCategory.DataBind()

        End If

        rcbCategory.Items.Insert(0, (New RadComboBoxItem("- Select Category -", "-1")))



    End Sub

    Protected Function GetCategory() As Integer

        Dim sb As New StringBuilder
        Dim nPortalId As Integer
        nPortalId = DNN.GetPMB(Me).PortalId
        sb.Append("select nb_store_categories.categoryid from nb_store_categorylang ")
        sb.Append("inner join nb_store_categories on nb_store_categorylang.categoryid = nb_store_categories.categoryid ")
        sb.Append("where  portalid = " & nPortalId)

        Dim sql As String = sb.ToString()

        Dim dt As DataTable = DNNDB.Query(sql)

        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            Dim nCategoryId As Integer = Convert.ToInt64(dt.Rows(0)("CategoryId"))
            GetCategory = nCategoryId
        Else
            GetCategory = 0
        End If

        Return GetCategory

    End Function

    Protected Sub UpdateNBStore()

        '''''''''''''''''''''''''''''''' Create Category '''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim retCategoryId As Integer

        retCategoryId = GetCategory()

        If rcbCategory.SelectedValue = "-1" And retCategoryId = 0 Then

            Dim spcCategoryUpdate(15) As SqlParameter

            spcCategoryUpdate(0) = New SqlClient.SqlParameter("@CategoryID", SqlDbType.BigInt)
            spcCategoryUpdate(0).Value = -1

            spcCategoryUpdate(1) = New SqlClient.SqlParameter("@Lang", SqlDbType.NChar)
            spcCategoryUpdate(1).Value = "en-US"

            spcCategoryUpdate(2) = New SqlClient.SqlParameter("@CategoryName", SqlDbType.NVarChar)
            spcCategoryUpdate(2).Value = "Products"

            spcCategoryUpdate(3) = New SqlClient.SqlParameter("@CategoryDesc", SqlDbType.NVarChar)
            spcCategoryUpdate(3).Value = "Products"

            spcCategoryUpdate(4) = New SqlClient.SqlParameter("@Message", SqlDbType.NText)
            spcCategoryUpdate(4).Value = ""

            spcCategoryUpdate(5) = New SqlClient.SqlParameter("@PortalID", SqlDbType.BigInt)
            spcCategoryUpdate(5).Value = DNN.GetPMB(Me).PortalId

            spcCategoryUpdate(6) = New SqlClient.SqlParameter("@Archived", SqlDbType.Bit)
            spcCategoryUpdate(6).Value = False

            spcCategoryUpdate(7) = New SqlClient.SqlParameter("@CreatedByUser", SqlDbType.BigInt)
            spcCategoryUpdate(7).Value = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID

            spcCategoryUpdate(8) = New SqlClient.SqlParameter("@CreatedDate", SqlDbType.DateTime)
            spcCategoryUpdate(8).Value = DateTime.Now

            spcCategoryUpdate(9) = New SqlClient.SqlParameter("@ParentCategoryID", SqlDbType.Int)
            spcCategoryUpdate(9).Value = 0

            spcCategoryUpdate(10) = New SqlClient.SqlParameter("@ListOrder", SqlDbType.Int)
            spcCategoryUpdate(10).Value = 1

            spcCategoryUpdate(11) = New SqlClient.SqlParameter("@ProductTemplate", SqlDbType.NVarChar)
            spcCategoryUpdate(11).Value = ""

            spcCategoryUpdate(12) = New SqlClient.SqlParameter("@ListItemTemplate", SqlDbType.NVarChar)
            spcCategoryUpdate(12).Value = ""

            spcCategoryUpdate(13) = New SqlClient.SqlParameter("@ListAltItemTemplate", SqlDbType.NVarChar)
            spcCategoryUpdate(13).Value = ""

            spcCategoryUpdate(14) = New SqlClient.SqlParameter("@Hide", SqlDbType.Bit)
            spcCategoryUpdate(14).Value = False

            spcCategoryUpdate(15) = New SqlClient.SqlParameter("@ImageURL", SqlDbType.BigInt)
            spcCategoryUpdate(15).Value = ""

            retCategoryId = DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_Categories_Update", spcCategoryUpdate).Rows(0)(0)

        ElseIf rcbCategory.SelectedValue = "-1" Then

            retCategoryId = GetCategory()

        Else

            retCategoryId = rcbCategory.SelectedValue

        End If


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        For Each item As RadListBoxItem In rlbNBStoreProducts.Items

            If Not IsProductExist(item.Value) Then

                'Dim sProductCode As String = item.Text
                'Dim sUserName As String = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username

                'Dim obj_CP As CustomerProduct = New CustomerProduct()

                'obj_CP.ProductID = item.Value
                'obj_CP.SprintCustomerKey = rcbCustomerImport.SelectedValue
                'obj_CP.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                'obj_CP.CreatedOn = DateTime.Now
                'obj_CP.Add()


                '''''''''''''''''''''''''''''''''''''''''' Products ''''''''''''''''''''''''''''''''''''''''''

                Dim spcProductUpdate(17) As SqlParameter

                spcProductUpdate(0) = New SqlClient.SqlParameter("@ProductId", SqlDbType.BigInt)
                spcProductUpdate(0).Value = -1

                spcProductUpdate(1) = New SqlClient.SqlParameter("@PortalId", SqlDbType.BigInt)
                spcProductUpdate(1).Value = DNN.GetPMB(Me).PortalId

                spcProductUpdate(2) = New SqlClient.SqlParameter("@TaxCategoryID", SqlDbType.BigInt)
                spcProductUpdate(2).Value = 0

                spcProductUpdate(3) = New SqlClient.SqlParameter("@Featured", SqlDbType.Bit)
                spcProductUpdate(3).Value = 0

                spcProductUpdate(4) = New SqlClient.SqlParameter("@Archived", SqlDbType.Bit)
                spcProductUpdate(4).Value = 0

                spcProductUpdate(5) = New SqlClient.SqlParameter("@CreatedByUser", SqlDbType.NVarChar)
                spcProductUpdate(5).Value = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username

                spcProductUpdate(6) = New SqlClient.SqlParameter("@CreatedDate", SqlDbType.DateTime)
                spcProductUpdate(6).Value = DateTime.Now

                spcProductUpdate(7) = New SqlClient.SqlParameter("@IsDeleted", SqlDbType.Bit)
                spcProductUpdate(7).Value = 0

                spcProductUpdate(8) = New SqlClient.SqlParameter("@ProductRef", SqlDbType.NVarChar)
                spcProductUpdate(8).Value = item.Value

                spcProductUpdate(9) = New SqlClient.SqlParameter("@Lang", SqlDbType.Char)
                spcProductUpdate(9).Value = "en-US"

                spcProductUpdate(10) = New SqlClient.SqlParameter("@Summary", SqlDbType.NVarChar)
                spcProductUpdate(10).Value = item.Text

                spcProductUpdate(11) = New SqlClient.SqlParameter("@Description", SqlDbType.NVarChar)
                spcProductUpdate(11).Value = item.Text

                spcProductUpdate(12) = New SqlClient.SqlParameter("@Manufacturer", SqlDbType.NVarChar)
                spcProductUpdate(12).Value = ""

                spcProductUpdate(13) = New SqlClient.SqlParameter("@ProductName", SqlDbType.NVarChar)
                spcProductUpdate(13).Value = item.Text

                spcProductUpdate(14) = New SqlClient.SqlParameter("@XMLdata", SqlDbType.Xml)
                spcProductUpdate(14).Value = item.Text

                spcProductUpdate(15) = New SqlClient.SqlParameter("@SEOName", SqlDbType.NVarChar)
                spcProductUpdate(15).Value = item.Text

                spcProductUpdate(16) = New SqlClient.SqlParameter("@TagWords", SqlDbType.NVarChar)
                spcProductUpdate(16).Value = item.Text

                spcProductUpdate(17) = New SqlClient.SqlParameter("@IsHidden", SqlDbType.Bit)
                spcProductUpdate(17).Value = 0

                Dim retProductId As Int64 = DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_Products_Update", spcProductUpdate).Rows(0)(0)



                ''''''''''''''''''''''''''''''''''''''''''''''''' Product Category '''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim spcCategoryProductUpdate(1) As SqlParameter

                spcCategoryProductUpdate(0) = New SqlClient.SqlParameter("@ProductId", SqlDbType.BigInt)
                spcCategoryProductUpdate(0).Value = retProductId

                spcCategoryProductUpdate(1) = New SqlClient.SqlParameter("@CategoryID", SqlDbType.BigInt)
                spcCategoryProductUpdate(1).Value = retCategoryId

                DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_ProductCategory_Update", spcCategoryProductUpdate)


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                '''''''''''''''''''''''''''''''' Product Price and Quantity '''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim spcProductModelUpdate(18) As SqlParameter

                Dim row() As DataRow = pdtProducts.Select("sprintProductkey=" & item.Value.ToString())
                Dim nQtyAvailable As Int64
                nQtyAvailable = Convert.ToInt64(row(0)("QuantityAvailable"))
                'If row IsNot Nothing And row.Count() <> 0 Then
                'Dim nPrice As Int64 = Convert.ToInt64(row(0)("Price"))
                'End If


                spcProductModelUpdate(0) = New SqlClient.SqlParameter("@ModelID", SqlDbType.BigInt)
                spcProductModelUpdate(0).Value = -1

                spcProductModelUpdate(1) = New SqlClient.SqlParameter("@ProductID", SqlDbType.BigInt)
                spcProductModelUpdate(1).Value = retProductId

                spcProductModelUpdate(2) = New SqlClient.SqlParameter("@ListOrder", SqlDbType.BigInt)
                spcProductModelUpdate(2).Value = 1

                spcProductModelUpdate(3) = New SqlClient.SqlParameter("@UnitCost", SqlDbType.Money)
                Dim dPrice As Decimal = Convert.ToDecimal(row(0)("Price"))
                spcProductModelUpdate(3).Value = dPrice

                spcProductModelUpdate(4) = New SqlClient.SqlParameter("@Barcode", SqlDbType.NVarChar)
                spcProductModelUpdate(4).Value = ""

                spcProductModelUpdate(5) = New SqlClient.SqlParameter("@ModelRef", SqlDbType.NVarChar)
                spcProductModelUpdate(5).Value = item.Value

                spcProductModelUpdate(6) = New SqlClient.SqlParameter("@Lang", SqlDbType.NChar)
                spcProductModelUpdate(6).Value = "en-US"

                spcProductModelUpdate(7) = New SqlClient.SqlParameter("@ModelName", SqlDbType.NVarChar)
                spcProductModelUpdate(7).Value = item.Text

                spcProductModelUpdate(8) = New SqlClient.SqlParameter("@QtyRemaining", SqlDbType.BigInt)
                spcProductModelUpdate(8).Value = nQtyAvailable

                spcProductModelUpdate(9) = New SqlClient.SqlParameter("@QtyTrans", SqlDbType.BigInt)
                spcProductModelUpdate(9).Value = 0

                spcProductModelUpdate(10) = New SqlClient.SqlParameter("@QtyTransDate", SqlDbType.DateTime)
                spcProductModelUpdate(10).Value = DateTime.Now

                spcProductModelUpdate(11) = New SqlClient.SqlParameter("@Deleted", SqlDbType.Bit)
                spcProductModelUpdate(11).Value = False

                spcProductModelUpdate(12) = New SqlClient.SqlParameter("@QtyStockSet", SqlDbType.BigInt)
                spcProductModelUpdate(12).Value = 0

                spcProductModelUpdate(13) = New SqlClient.SqlParameter("@DealerCost", SqlDbType.Money)
                spcProductModelUpdate(13).Value = dPrice

                spcProductModelUpdate(14) = New SqlClient.SqlParameter("@PurchaseCost", SqlDbType.Money)
                spcProductModelUpdate(14).Value = 0

                spcProductModelUpdate(15) = New SqlClient.SqlParameter("@XMLData", SqlDbType.Xml)
                spcProductModelUpdate(15).Value = ""

                spcProductModelUpdate(16) = New SqlClient.SqlParameter("@Extra", SqlDbType.NVarChar)
                spcProductModelUpdate(16).Value = ""

                spcProductModelUpdate(17) = New SqlClient.SqlParameter("@DealerOnly", SqlDbType.Bit)
                spcProductModelUpdate(17).Value = False

                spcProductModelUpdate(18) = New SqlClient.SqlParameter("@Allow", SqlDbType.BigInt)
                spcProductModelUpdate(18).Value = 0

                DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_Model_Update", spcProductModelUpdate)

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                ''''''''''''''''''''''''''''''''''''' Images '''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
                Dim DestinationPath As String = ps.HomeDirectoryMapPath & productImages
                Dim imageName As String = row(0)("Image").ToString()

                Dim spcProductImageUpdate(7) As SqlParameter

                spcProductImageUpdate(0) = New SqlClient.SqlParameter("@ImageID", SqlDbType.BigInt)
                spcProductImageUpdate(0).Value = -1

                spcProductImageUpdate(1) = New SqlClient.SqlParameter("@ProductID", SqlDbType.BigInt)
                spcProductImageUpdate(1).Value = retProductId

                spcProductImageUpdate(2) = New SqlClient.SqlParameter("@ImagePath", SqlDbType.NVarChar)
                spcProductImageUpdate(2).Value = DestinationPath & "\" & imageName

                spcProductImageUpdate(3) = New SqlClient.SqlParameter("@ListOrder", SqlDbType.Int)
                spcProductImageUpdate(3).Value = 1

                spcProductImageUpdate(4) = New SqlClient.SqlParameter("@Hidden", SqlDbType.Bit)
                spcProductImageUpdate(4).Value = False

                spcProductImageUpdate(5) = New SqlClient.SqlParameter("@Lang", SqlDbType.NChar)
                spcProductImageUpdate(5).Value = "en-US"

                spcProductImageUpdate(6) = New SqlClient.SqlParameter("@ImageDesc", SqlDbType.NVarChar)
                spcProductImageUpdate(6).Value = ""

                spcProductImageUpdate(7) = New SqlClient.SqlParameter("@ImageURL", SqlDbType.NVarChar)
                spcProductImageUpdate(7).Value = VirtualPathUtility.AppendTrailingSlash(ps.HomeDirectory) & productImages & "/" & imageName

                DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_ProductImage_Update", spcProductImageUpdate)

                '''''''''''''''''''''''''''''''''''''''''' Copy Image To Folder '''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim Url As String
                Url = imagesPath & row(0)("Image").ToString()
                Call CopyImageFromUrl(Url)

            End If

        Next

    End Sub

    Public Sub CreateFolder(folderName As String)

        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        Dim DestinationPath As String = ps.HomeDirectoryMapPath & productImages

        If Not Directory.Exists(DestinationPath) Then
            Directory.CreateDirectory(DestinationPath)
        End If

    End Sub

    Protected Sub CopyImageFromUrl(ByVal url As String)

        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        Dim DestinationPath As String = ps.HomeDirectoryMapPath & "productimages"

        Dim imageUrl As String = url
        Dim filename As String = imageUrl.Substring(imageUrl.LastIndexOf("/"c) + 1)
        Dim bytes As Byte() = GetBytesFromUrl(imageUrl)
        Call CreateFolder(productImages)
        WriteBytesToFile(DestinationPath + "/" + filename, bytes)

    End Sub

    Public Shared Sub WriteBytesToFile(fileName As String, content As Byte())

        Dim fs As New FileStream(fileName, FileMode.Create)
        Dim w As New BinaryWriter(fs)
        Try
            w.Write(content)
        Finally
            fs.Close()
            w.Close()
        End Try

    End Sub

    Public Shared Function GetBytesFromUrl(url As String) As Byte()

        Dim b As Byte()
        Dim myReq As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        Dim myResp As WebResponse = myReq.GetResponse()

        Dim stream As Stream = myResp.GetResponseStream()
        'int i;
        Using br As New BinaryReader(stream)
            'i = (int)(stream.Length);
            b = br.ReadBytes(500000)
            br.Close()
        End Using
        myResp.Close()
        Return b
    End Function

    Protected Sub btnImportCountry_Click(sender As Object, e As System.EventArgs) Handles btnImportCountry.Click

        Try

            'Dim ssModel As New SSDataLayer.EntitiesModel
            'Dim ISSCountry = (From Country In ssModel.Countries
            '                Select Country).ToList


            'For Each country As SSDataLayer.Country In ISSCountry

            '    Dim dnnCountry As New DentonDataLayer.List

            '    dnnCountry.Value = country.CountryId2
            '    dnnCountry.Text = country.CountryName
            '    dnnCountry.ParentID = 0
            '    dnnCountry.SortOrder = 0
            '    dnnCountry.Level = 0
            '    dnnCountry.DefinitionID = -1
            '    dnnCountry.PortalID = -1
            '    dnnCountry.SystemList = True
            '    dnnCountry.ListName = "Country"
            '    dnnModel.Add(dnnCountry)
            '    dnnModel.SaveChanges()

            'Next

            'Dim sql As String = "select * from country order by countrykey"

            'Dim dt As DataTable = SprintDB.Query(sql)

            'If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            '    For Each dr As DataRow In dt.Rows

            '        Dim dnnCountry As New DentonDataLayer.List

            '        dnnCountry.Value = dr("CountryId2")
            '        dnnCountry.Text = dr("CountryName")
            '        dnnCountry.ParentID = 0
            '        dnnCountry.SortOrder = 0
            '        dnnCountry.Level = 0
            '        dnnCountry.DefinitionID = -1
            '        dnnCountry.PortalID = -1
            '        dnnCountry.SystemList = True
            '        dnnCountry.ListName = "Country"
            '        dnnModel.Add(dnnCountry)
            '        dnnModel.SaveChanges()

            '    Next


            'End If

        Catch ex As Exception

            WebMsgBox.Show(ex.Message.ToString())

        End Try



        'dnnCountry.EntryID = country.CountryKey

        'dnnCountry.Value = country.CountryId2
        'dnnCountry.Text = country.CountryName
        'dnnCountry.ParentID = 0
        'dnnCountry.SortOrder = 0
        'dnnCountry.Level = 0
        'dnnCountry.DefinitionID = -1
        'dnnCountry.PortalID = -1
        'dnnCountry.SystemList = True
        'Dim ISSCountryList = (From ICoun In ssModel.Countries Order By ICoun.CountryKey Ascending
        '                     Select ICoun).ToList


    End Sub

    Protected Sub rgCountryList_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgCountryList.NeedDataSource

        Dim ICountry = (From Country In dnnModel.Lists
                     Select Country).ToList

        rgCountryList.DataSource = ICountry

    End Sub

End Class

'Dim sqlParam(18) As SqlParameter
'sqlParam(0) = 
'sb.Append("insert into dbo.[NB_Store_Products](PortalID,TaxCategoryID,Featured, Archived,CreatedByUser, CreatedDate, IsDeleted, ProductRef, ModifiedDate, IsHidden()) values(")
'sb.Append("@PortalID,@TaxCategoryID,@Featured,@Archived,@CreatedByUser,@CreatedDate,@IsDeleted,@ProductRef,getdate(),@IsHidden)")
'sb.Append("set @ProductID = @@Identity")
'cmd.Parameters.Add("@ProductId", SqlDbType.BigInt).Value = -1
'cmd.Parameters.Add("@PortalId", SqlDbType.BigInt).Value = DNN.GetPMB(Me).PortalId
'cmd.Parameters.Add("@TaxCategoryID", SqlDbType.BigInt).Value = 0
'cmd.Parameters.Add("@Featured", SqlDbType.Bit).Value = 0
'cmd.Parameters.Add("@Archived", SqlDbType.Bit).Value = 0
'cmd.Parameters.Add("@CreatedByUser", SqlDbType.NVarChar).Value = sUserName
'cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now
'cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit).Value = 0
'cmd.Parameters.Add("@ProductRef", SqlDbType.NVarChar).Value = sProductCode
'cmd.Parameters.Add("@Lang", SqlDbType.Char).Value = "en-US"
'cmd.Parameters.Add("@Summary", SqlDbType.NVarChar).Value = ""
'cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = ""
'cmd.Parameters.Add("@Manufacturer", SqlDbType.NVarChar).Value = ""
'cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = sProductCode
'cmd.Parameters.Add("@XMLdata", SqlDbType.NVarChar).Value = ""
'cmd.Parameters.Add("@SEOName", SqlDbType.NVarChar).Value = sProductCode
'cmd.Parameters.Add("@TagWords", SqlDbType.NVarChar).Value = sProductCode
'cmd.Parameters.Add("@IsHidden", SqlDbType.BigInt).Value = 0

'DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_Products_Update", cmd)


'DataProvider.Instance.ExecuteNonQuery("NEvoweb_NB_Store_Products_Update", sqlParamCollection)

'sb.Append(DNN.GetPMB(Me).PortalId & ", 0, 0, 0," & sUserName & "," & DateTime.Now & ", 0, " & sProductCode & "," & DateTime.Now & ", 0")

'DNNDB.Query(  


'Dim spcProductRelatedUpdate(10) As SqlParameter

'spcProductRelatedUpdate(0) = New SqlClient.SqlParameter("@RelatedID", SqlDbType.BigInt)
'spcProductRelatedUpdate(0).Value = -1

'spcProductRelatedUpdate(1) = New SqlClient.SqlParameter("@PortalId", SqlDbType.BigInt)
'spcProductRelatedUpdate(1).Value = rcbPortals.SelectedValue

'spcProductRelatedUpdate(2) = New SqlClient.SqlParameter("@ProductId", SqlDbType.BigInt)
'spcProductRelatedUpdate(2).Value = retProductId

'spcProductRelatedUpdate(3) = New SqlClient.SqlParameter("@DiscountAmt", SqlDbType.BigInt)
'spcProductRelatedUpdate(3).Value = 0

'spcProductRelatedUpdate(4) = New SqlClient.SqlParameter("@DiscountPercent", SqlDbType.BigInt)
'spcProductRelatedUpdate(4).Value = 0

'Dim row() As DataRow = pdtProducts.Select("sprintProductkey=" & item.Value.ToString())
'If row IsNot Nothing Then

'retProductId = Convert.ToInt64(row(0)("QuantityAvailable"))
'spcProductRelatedUpdate(5) = New SqlClient.SqlParameter("@ProductQty", SqlDbType.BigInt)
'spcProductRelatedUpdate(5).Value = Convert.ToInt64(row(0)("QuantityAvailable"))

'spcProductRelatedUpdate(6) = New SqlClient.SqlParameter("@MaxQty", SqlDbType.BigInt)
'spcProductRelatedUpdate(6).Value = Convert.ToInt64(row(0)("QuantityAvailable"))

'End If

'spcProductRelatedUpdate(7) = New SqlClient.SqlParameter("@RelatedType", SqlDbType.BigInt)
'spcProductRelatedUpdate(7).Value = -1

'spcProductRelatedUpdate(9) = New SqlClient.SqlParameter("@NotAvailable", SqlDbType.Bit)
'spcProductRelatedUpdate(9).Value = False

'spcProductRelatedUpdate(10) = New SqlClient.SqlParameter("@BiDirectional", SqlDbType.Bit)
'spcProductRelatedUpdate(10).Value = False

'DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_ProductRelated_Update", spcProductRelatedUpdate)
