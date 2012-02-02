' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2008 SARL NevoWeb.  www.nevoweb.com. BSD License.
' Author: D.C.Lee
' ------------------------------------------------------------------------
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' ------------------------------------------------------------------------
' This copyright notice may NOT be removed, obscured or modified without written consent from the author.
' --- End copyright notice --- 


Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class AdminOrders
        Inherits BaseAdminModule

        Private _OrdId As Integer
        Private _UsrId As Integer
        Private _StatusList As Hashtable
        Private _EditData As String
        Private _RecalcCart As Boolean = False

#Region "Private events"

        Private Sub Page_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            _EditData = ""
            If Not (Request.QueryString("ed") Is Nothing) Then
                _EditData = Request.QueryString("ed")
            End If

            _RecalcCart = False
            If Not (Request.QueryString("rc") Is Nothing) Then
                _RecalcCart = True
            End If

        End Sub



        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not (Request.QueryString("OrdId") Is Nothing) Then
                    _OrdId = Int32.Parse(Request.QueryString("OrdId"))
                Else
                    _OrdId = -1
                End If
                If Not (Request.QueryString("uid") Is Nothing) Then
                    _UsrId = Int32.Parse(Request.QueryString("uid"))
                Else
                    _UsrId = -1
                End If

                ' If the call request has come from the client admin control, offer return option.
                If _UsrId > 0 And Not (Request.QueryString("rtnctl") Is Nothing) Then
                    cmdReturn.Visible = True
                Else
                    cmdReturn.Visible = False
                End If

                'clear order edit return url
                If GetStoreSettingBoolean(PortalId, "orderedititems.flag") Then
                    If getAdminCookieValue(PortalId, "OrderEditRtnURL") <> "" Then
                        'must have added a product to the cart, so don't re-build from order
                        _RecalcCart = True
                        setAdminCookieValue(PortalId, "OrderEditRtnURL", "")
                    End If
                    cartlist1.Visible = True
                    cartlist1.HideTotals = True
                    cartlist1.NoUpdates = False
                    cartlist1.ResourceFile = LocalResourceFile
                    cartlist1.AllowPriceEdit = GetStoreSettingBoolean(PortalId, "ordereditprice.flag")
                End If


                If Not Page.IsPostBack Then
                    If _OrdId = -1 Then
                        populateOrderList(_UsrId)
                    Else
                        populateOrderView(_OrdId)
                    End If

                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dgOrderList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgOrderList.ItemCommand
            Dim OrdId As Integer = Int32.Parse(e.CommandArgument.ToString)

            If e.CommandName = "gotocart" Then
                Dim CartTabID As String
                CartTabID = GetStoreSetting(PortalId, "checkout.tab")
                CurrentCart.CreateCartFromOrder(PortalId, OrdId, False)
                If IsNumeric(CartTabID) Then
                    Response.Redirect(NavigateURL(CInt(CartTabID)))
                End If
            End If

        End Sub

        Private Sub dgOrderList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgOrderList.ItemDataBound
            Dim itemInfo As NB_Store_OrdersInfo = CType(e.Item.DataItem, NB_Store_OrdersInfo)
            Dim objOCtrl As New OrderController
            Dim objAInfo As NB_Store_AddressInfo

            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                If _StatusList.ContainsKey(itemInfo.OrderStatusID) Then
                    e.Item.Cells(5).Text = _StatusList.Item(itemInfo.OrderStatusID)
                End If
                'go get the name (This could be a performace issue.) 
                objAInfo = objOCtrl.GetOrderAddress(itemInfo.BillingAddressID)
                If Not objAInfo Is Nothing Then
                    e.Item.Cells(3).Text = objAInfo.AddressName & " " & objAInfo.AddressName2
                End If

                If (itemInfo.OrderStatusID <> 60 And itemInfo.OrderStatusID <> 80 And itemInfo.OrderStatusID <> 90) Then
                    e.Item.Cells(7).Visible = False
                End If

            End If
        End Sub

        Private Sub dgOrderList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgOrderList.PageIndexChanged
            dgOrderList.CurrentPageIndex = e.NewPageIndex
            populateOrderList(_UsrId)
        End Sub

        Private Sub dgOrderList_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgOrderList.EditCommand
            Dim OrdId As Integer = Int32.Parse(e.CommandArgument.ToString)
            If ModuleConfiguration.ModuleControl.ControlType = SecurityAccessLevel.Edit Then
                If Not (Request.QueryString("uid") Is Nothing) And Not (Request.QueryString("rtnctl") Is Nothing) Then
                    Response.Redirect(EditUrl("OrdId", OrdId.ToString, "AdminOrders", "uid=" & Request.QueryString("uid"), "rtnctl=" & Request.QueryString("rtnctl")))
                Else
                    Response.Redirect(EditUrl("OrdId", OrdId.ToString, "AdminOrders"))
                End If
            Else
                Response.Redirect(NavigateURL(TabId, "", "OrdId=" & OrdId.ToString))
            End If
        End Sub

        Private Sub cmdReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturn.Click
            If ModuleConfiguration.ModuleControl.ControlType = SecurityAccessLevel.Edit Then
                If Not (Request.QueryString("uid") Is Nothing) And Not (Request.QueryString("rtnctl") Is Nothing) Then
                    Response.Redirect(EditUrl("uid", Request.QueryString("uid"), Request.QueryString("rtnctl")))
                Else
                    Response.Redirect(NavigateURL())
                End If
            Else
                Response.Redirect(NavigateURL())
            End If
        End Sub

        Private Sub cmdReturn2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturn2.Click
            If ModuleConfiguration.ModuleControl.ControlType = SecurityAccessLevel.Edit Then
                RedirectAfterEdit()
            Else
                Response.Redirect(NavigateURL())
            End If
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            updateOrder()
            RedirectAfterEdit()
        End Sub

        Private Sub cmdPrintOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrintOrder.Click
            Response.Write("<script>window.open('" & NavigateURL(TabId, "PrintOrder", "mid=" & ModuleId.ToString, "ORID=" & _OrdId.ToString & "&SkinSrc=" & QueryStringEncode("[G]" & Skins.SkinInfo.RootSkin & "/" & glbHostSkinFolder & "/" & "No Skin"), "ContainerSrc=" & QueryStringEncode("[G]" & Skins.SkinInfo.RootContainer & "/" & glbHostSkinFolder & "/" & "No Container"), "dnnprintmode=true") & "','_blank')</script>")
            populateOrderView(_OrdId)
        End Sub

        Private Sub cmdPrintreceipt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrintreceipt.Click
            Response.Write("<script>window.open('" & NavigateURL(TabId, "PrintOrder", "mid=" & ModuleId.ToString, "ORID=" & _OrdId.ToString & "&inv=1&SkinSrc=" & QueryStringEncode("[G]" & Skins.SkinInfo.RootSkin & "/" & glbHostSkinFolder & "/" & "No Skin"), "ContainerSrc=" & QueryStringEncode("[G]" & Skins.SkinInfo.RootContainer & "/" & glbHostSkinFolder & "/" & "No Container"), "dnnprintmode=true") & "','_blank')</script>")
            populateOrderView(_OrdId)
        End Sub


        Private Sub cmdSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
            If IsDate(txtFromDate.Text) Then
                setStoreCookieValue(PortalId, "SearchOrderDates", "From", txtFromDate.Text, 0)
            End If
            If IsDate(txtToDate.Text) Then
                setStoreCookieValue(PortalId, "SearchOrderDates", "To", txtToDate.Text, 0)
            End If
            dgOrderList.CurrentPageIndex = 0
            populateOrderList(_UsrId)
        End Sub

        Private Sub ddlSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSearch.SelectedIndexChanged
            dgOrderList.CurrentPageIndex = 0
            populateOrderList(_UsrId)
        End Sub

        Private Sub cmdSendShipEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSendShipEmail.Click
            updateOrder()
            SendOrderEmail("ordershipped.email")
            RedirectAfterEmail()
        End Sub

        Private Sub cmdSendValidateOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSendValidateOrder.Click
            updateOrder()
            SendOrderEmail("ordervalidated.email")
            RedirectAfterEmail()
        End Sub

        Private Sub cmdSendReceiptEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSendReceiptEmail.Click
            updateOrder()
            SendOrderEmail("orderreceipt.email")
            RedirectAfterEmail()
        End Sub

        Private Sub cmdSendAmendEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSendAmendEmail.Click
            updateOrder()
            SendOrderEmail("orderamended.email")
            RedirectAfterEmail()
        End Sub

        Private Sub cmdReOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReOrder.Click
            CurrentCart.CreateCartFromOrder(PortalId, _OrdId, True)
            Dim CatTabID As Integer = GetStoreSettingInt(PortalId, "checkout.tab", GetCurrentCulture)
            Response.Redirect(NavigateURL(CatTabID))
        End Sub

        Private Sub cmdEditOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEditOrder.Click
            Response.Redirect(GetEditUrl(_OrdId))
        End Sub

        Private Sub cartlist1_RecalculateCart() Handles cartlist1.RecalculateCart
            updateOrder()
            If Not (Request.QueryString("uid") Is Nothing) And Not (Request.QueryString("rtnctl") Is Nothing) Then
                Response.Redirect(EditUrl("OrdId", _OrdId.ToString, "AdminOrders", "uid=" & Request.QueryString("uid"), "rtnctl=" & Request.QueryString("rtnctl"), "ed=1", "rc=1"))
            Else
                Response.Redirect(EditUrl("OrdId", _OrdId.ToString, "AdminOrders", "ed=1", "rc=1"))
            End If
        End Sub

        Private Sub cmdAddProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddProduct.Click
            updateOrder()
            Dim rtnURL As String = GetEditUrl(_OrdId)
            Dim StoreTabID As String = GetStoreSetting(PortalId, "store.tab")
            setAdminCookieValue(PortalId, "OrderEditRtnURL", rtnURL)
            If IsNumeric(StoreTabID) Then
                Response.Redirect(NavigateURL(CInt(StoreTabID)))
            End If
        End Sub

        Private Sub cmdCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
            Dim objOCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo

            objOInfo = objOCtrl.CreateEmptyOrder(PortalId, UserId)

            Response.Redirect(GetEditUrl(objOInfo.OrderID))

        End Sub


#End Region

#Region "Private methods"

        Private Function GetEditUrl(ByVal OrderID As Integer) As String
            If Not (Request.QueryString("uid") Is Nothing) And Not (Request.QueryString("rtnctl") Is Nothing) Then
                Return EditUrl("OrdId", OrderID.ToString, "AdminOrders", "uid=" & Request.QueryString("uid"), "rtnctl=" & Request.QueryString("rtnctl"), "ed=1")
            Else
                Return EditUrl("OrdId", OrderID.ToString, "AdminOrders", "ed=1")
            End If
        End Function

        Private Sub RedirectAfterEdit()
            'clear cart for editor
            CurrentCart.DeleteCart(PortalId)

            If plhOrder.Visible Then
                If Not (Request.QueryString("uid") Is Nothing) And Not (Request.QueryString("rtnctl") Is Nothing) Then
                    Response.Redirect(EditUrl("uid", Request.QueryString("uid"), "AdminOrders", "rtnctl=" & Request.QueryString("rtnctl")))
                Else
                    Response.Redirect(EditUrl("AdminOrders"))
                End If
            Else
                If Not (Request.QueryString("uid") Is Nothing) And Not (Request.QueryString("rtnctl") Is Nothing) Then
                    Response.Redirect(EditUrl("uid", Request.QueryString("uid"), "AdminOrders", "rtnctl=" & Request.QueryString("rtnctl"), "OrdId=" & _OrdId))
                Else
                    Response.Redirect(EditUrl("OrdId", _OrdId, "AdminOrders"))
                End If
            End If
        End Sub

        Private Sub RedirectAfterEmail()
            If Request.QueryString("ctl") <> "" Then
                Response.Redirect(EditUrl("OrdId", _OrdId.ToString, Request.QueryString("ctl")))
            Else
                Response.Redirect(NavigateURL(TabId, "", "OrdId=" & _OrdId.ToString))
            End If
        End Sub

        Private Sub SendOrderEmail(ByVal EmailTemplateName As String)
            Dim objCtrl As New OrderController
            Dim objInfo As NB_Store_OrdersInfo
            objInfo = objCtrl.GetOrder(_OrdId)
            If Not objInfo Is Nothing Then
                SendEmailToClient(PortalId, GetClientEmail(PortalId, objInfo), "", objInfo, EmailTemplateName, GetClientLang(PortalId, objInfo))
                UpdateLog("EMAILSENT : " & EmailTemplateName)
            End If
        End Sub


        Private Sub populateOrderList(ByVal UsrID As Integer)
            Dim objCtrl As New OrderController
            Dim aryList As ArrayList = Nothing

            cmdCreate.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdCreateMsg", LocalResourceFile) & "');")
            If Not IsManager(PortalId, UserInfo) Then
                cmdCreate.Visible = False
            End If


            If ModuleConfiguration.ModuleControl.ControlType = SecurityAccessLevel.View Then
                cmdReturn.Visible = False
                ddlSearch.Visible = False
            End If

            pnlOrderEdit.Visible = False
            pnlOrderList.Visible = True

            hypFromDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtFromDate)
            hypFromDate.Text = ""
            hypFromDate.ImageUrl = "~/images/calendar.png"

            hypToDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtToDate)
            hypToDate.Text = ""
            hypToDate.ImageUrl = "~/images/calendar.png"

            Localization.LocalizeDataGrid(dgOrderList, LocalResourceFile)

            If Not Page.IsPostBack Then
                populateStatusList(ddlSearch, "0", Localization.GetString("SelectAll", LocalResourceFile), "")
            End If

            If Not IsEditor(PortalId, UserInfo) Then
                'only allow users to see their own orders
                UsrID = UserInfo.UserID
            End If

            'don't show list if not logged on
            If UserInfo.UserID >= 0 Then
                If Not String.IsNullOrEmpty(txtOrderNbr.Text) Then
                    If IsNumeric(txtOrderNbr.Text) Then
                        aryList = New ArrayList()
                        Dim objOrder As NB_Store_OrdersInfo = objCtrl.GetOrder(CInt(txtOrderNbr.Text))
                        If Not objOrder Is Nothing Then
                            If objOrder.UserID = UsrID Or IsEditor(PortalId, UserInfo) Then
                                If objOrder IsNot Nothing Then aryList.Add(objOrder)
                            End If
                        End If
                    End If
                Else
                    If Not IsDate(txtFromDate.Text) Or Not IsDate(txtToDate.Text) Then
                        Dim cookieFrom As String = getStoreCookieValue(PortalId, "SearchOrderDates", "From")
                        Dim cookieTo As String = getStoreCookieValue(PortalId, "SearchOrderDates", "To")

                        If IsDate(cookieFrom) Then
                            txtFromDate.Text = cookieFrom
                        Else
                            txtFromDate.Text = DateAdd(DateInterval.Month, -1, Today).ToString("d")
                        End If

                        If IsDate(cookieTo) Then
                            txtToDate.Text = cookieTo
                        Else
                            txtToDate.Text = Today.ToString("d")
                        End If

                    End If
                    Dim SelectStatus As Integer
                    If Not ddlSearch.Visible Then
                        SelectStatus = 0
                    Else
                        SelectStatus = CInt(ddlSearch.SelectedValue)
                    End If

                    aryList = objCtrl.GetOrderList(PortalId, UsrID, CDate(txtFromDate.Text), CDate(txtToDate.Text), SelectStatus, txtFilter.Text)
                End If


                If Not aryList Is Nothing Then

                    If Not IsManager(PortalId, UserInfo) Then
                        'remove any orders with no order number
                        Dim lp As Integer
                        For lp = (aryList.Count - 1) To 0 Step -1
                            If DirectCast(aryList(lp), NB_Store_OrdersInfo).OrderNumber = "" Then
                                aryList.RemoveAt(lp)
                            End If
                        Next
                    End If

                    _StatusList = getStatusList(GetCurrentCulture)

                    dgOrderList.DataSource = aryList
                    dgOrderList.DataBind()

                End If

            End If


        End Sub

        Private Sub populateOrderView(ByVal OrderID As Integer)
            Dim objOCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo = Nothing
            Dim MsgText As String = ""

            pnlOrderEdit.Visible = True
            pnlOrderList.Visible = False

            cmdSendShipEmail.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdSendShipEmailMsg", LocalResourceFile) & "');")
            cmdSendValidateOrder.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdSendValidateOrderMsg", LocalResourceFile) & "');")
            cmdSendreceiptEmail.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdSendreceiptEmailMsg", LocalResourceFile) & "');")
            cmdSendAmendEmail.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdSendAmendEmailMsg", LocalResourceFile) & "');")
            cmdSendAmendEmail.Visible = False


            MsgText = GetStoreSettingText(PortalId, "orderview.text", GetCurrentCulture, UserInfo)

            objOInfo = objOCtrl.GetOrder(OrderID)
            If Not objOInfo Is Nothing Then

                'get order details and change tokens
                Dim objTR As New TokenStoreReplace(objOInfo, GetCurrentCulture)
                MsgText = objTR.DoTokenReplace(MsgText)

                If GetClientEmail(PortalId, objOInfo) = "" Then
                    cmdSendReceiptEmail.Visible = False
                    cmdSendShipEmail.Visible = False
                    cmdSendValidateOrder.Visible = False
                    cmdSendAmendEmail.Visible = False
                End If
                '--------------- display the order -------------------------------
                plhOrder.Controls.Add(New LiteralControl(Server.HtmlDecode(MsgText)))


                If IsEditor(PortalId, UserInfo) Or objOInfo.UserID = UserInfo.UserID Then
                    Dim objAInfo As NB_Store_AddressInfo

                    '--------------- display the edit order options -------------------------------
                    If _EditData <> "" Then
                        pnlEdit.Visible = True
                        pnlUpdate.Visible = False
                        cmdEditOrder.Visible = False
                        plhOrder.Visible = False
                        cmdSendReceiptEmail.Visible = False
                        cmdSendShipEmail.Visible = False
                        cmdSendValidateOrder.Visible = False
                        cmdReOrder.Visible = False
                        cmdPrintOrder.Visible = False
                        cmdPrintreceipt.Visible = False

                        'Billing Address
                        objAInfo = objOCtrl.GetOrderAddress(objOInfo.BillingAddressID)
                        If Not objAInfo Is Nothing Then
                            EditBAddrForm.TemplateName = "rab_editaddress.template"
                            EditBAddrForm.AddressDataInfo = objAInfo
                        End If

                        'Shipping Address
                        objAInfo = objOCtrl.GetOrderAddress(objOInfo.ShippingAddressID)
                        If Not objAInfo Is Nothing Then
                            EditSAddrForm.TemplateName = "rab_editaddress.template"
                            EditSAddrForm.AddressDataInfo = objAInfo
                        End If

                        txtEditNoteMsg.Text = objOInfo.NoteMsg

                        If IsManager(PortalId, UserInfo) Then
                            If objOInfo.AlreadyPaid > 0 Then
                                txtAlreadyPaid.Text = objOInfo.AlreadyPaid
                                txtAlreadyPaid.Text = CDbl(txtAlreadyPaid.Text).ToString
                            Else
                                txtAlreadyPaid.Text = 0
                            End If
                        Else
                            txtAlreadyPaid.Visible = False
                            lblAlreadyPaid.Visible = False
                        End If

                        If GetStoreSettingBoolean(PortalId, "orderedititems.flag") Then
                            'Order details.
                            If Not _RecalcCart Then
                                cartlist1.CartID = CurrentCart.CreateCartFromOrder(PortalId, objOInfo.OrderID, False)
                            Else
                                cartlist1.CartID = CurrentCart.getCartID(PortalId)
                            End If
                            cartlist1.PopulateCartList()
                            cmdAddProduct.Visible = True
                        Else
                            cartlist1.Visible = False
                            cmdAddProduct.Visible = False
                        End If

                        txtOrderEmail.Text = GetClientEmail(PortalId, objOInfo)

                    Else
                        'check if user edit allowed,
                        cmdEditOrder.Visible = False
                        If objOInfo.OrderStatusID < 50 Or objOInfo.OrderStatusID >= 80 Then ' only allow change for some status.
                            If GetStoreSettingBoolean(PortalId, "orderedit.flag") Then
                                cmdEditOrder.Visible = True
                                If IsEditor(PortalId, UserInfo) Then
                                    'offer the email ammeded order option to the manager/editor
                                    If objOInfo.OrderGUID <> "" And GetClientEmail(PortalId, objOInfo) <> "" Then
                                        cmdSendAmendEmail.Visible = True
                                    End If
                                Else
                                    If objOInfo.UserID = UserInfo.UserID Then
                                        If GetStoreSettingBoolean(PortalId, "ordereditclient.flag") Then
                                            cmdEditOrder.Visible = True
                                        Else
                                            cmdEditOrder.Visible = False
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                End If

            End If

            '--------------- security display ------------------------------
            If IsEditor(PortalId, UserInfo) Then

                '--------------- display the update options -------------------------------

                pnlUpdate.Visible = True
                cmdUpdate.Visible = True

                If GetStoreSettingText(PortalId, "receiptprint.text", GetCurrentCulture) = "" Then
                    cmdSendReceiptEmail.Visible = False
                    cmdPrintreceipt.Visible = False
                End If

                If objOInfo Is Nothing Then
                    objOInfo = objOCtrl.GetOrder(OrderID)
                End If
                If Not objOInfo Is Nothing Then

                    hypShipDate.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtShipDate)
                    hypShipDate.Text = ""
                    hypShipDate.ImageUrl = "~/images/calendar.png"

                    populateStatusList(ddlStatus, "", "", objOInfo.OrderStatusID)
                    If Not objOInfo.ShipDate = Null.NullDate Then
                        txtShipDate.Text = objOInfo.ShipDate.ToString("d")
                    Else
                        txtShipDate.Text = ""
                    End If

                    txtTrackCode.Text = objOInfo.TrackingCode

                End If

            Else
                pnlUpdate.Visible = False
                cmdUpdate.Visible = False
                cmdPrintreceipt.Visible = False
                'check user if order owner, if not hide order
                If UserInfo.UserID <> objOInfo.UserID Then
                    pnlOrderEdit.Visible = False
                End If
            End If


        End Sub

        Private Sub updateOrder()
            Dim objOCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo = Nothing

            objOInfo = objOCtrl.GetOrder(_OrdId)
            If Not objOInfo Is Nothing Then
                If ddlStatus.Visible Then
                    objOInfo.OrderStatusID = ddlStatus.SelectedValue
                    objOInfo.TrackingCode = txtTrackCode.Text
                    If objOInfo.OrderStatusID = 40 And objOInfo.OrderNumber = "" Then
                        'payment is OK, so update ordernumber if blank
                        Dim UsrID As Integer = objOInfo.UserID
                        If UsrID = -1 Then UsrID = 0
                        objOInfo.OrderNumber = Format(PortalId, "00") & "-" & UsrID.ToString("0000#") & "-" & objOInfo.OrderID.ToString("0000#") & "-" & objOInfo.OrderDate.ToString("yyyyMMdd")
                    End If
                    If IsDate(txtShipDate.Text) Then
                        objOInfo.ShipDate = CDate(txtShipDate.Text)
                    End If
                End If

                '-------------------------------------------------------------------------
                If pnlEdit.Visible Then

                    'create audit trail
                    Dim objExport As New Export
                    Dim strXML As String = objExport.GetOrderXML(objOInfo.OrderID)
                    UpdateLog(UserId, strXML)

                    Dim objAddr As NB_Store_AddressInfo

                    objAddr = EditBAddrForm.AddressDataInfo
                    objAddr.OrderID = objOInfo.OrderID
                    objAddr.UserID = UserId
                    objAddr.CreatedDate = Now
                    objAddr.CreatedByUser = UserId

                    If objOInfo.BillingAddressID > 0 Then
                        objAddr.AddressID = objOInfo.BillingAddressID
                        objOCtrl.UpdateObjOrderAddress(objAddr)
                    Else
                        objAddr = objOCtrl.UpdateObjOrderAddress(objAddr)
                        objOInfo.BillingAddressID = objAddr.AddressID
                    End If

                    objAddr = EditSAddrForm.AddressDataInfo
                    objAddr.OrderID = objOInfo.OrderID
                    objAddr.UserID = UserId
                    objAddr.CreatedDate = Now
                    objAddr.CreatedByUser = UserId

                    If objOInfo.ShippingAddressID = objOInfo.BillingAddressID Then
                        objAddr.AddressID = -1
                        objAddr = objOCtrl.UpdateObjOrderAddress(objAddr)
                        objOInfo.ShippingAddressID = objAddr.AddressID
                    Else
                        If objOInfo.ShippingAddressID > 0 Then
                            objAddr.AddressID = objOInfo.ShippingAddressID
                            objOCtrl.UpdateObjOrderAddress(objAddr)
                        Else
                            objAddr = objOCtrl.UpdateObjOrderAddress(objAddr)
                            objOInfo.ShippingAddressID = objAddr.AddressID
                        End If
                    End If

                    If txtEditNoteMsg.Visible Then objOInfo.NoteMsg = txtEditNoteMsg.Text

                    If txtOrderEmail.Visible Then objOInfo.Email = txtOrderEmail.Text

                    If cartlist1.Visible Then

                        'update details
                        CurrentCart.CreateOrderDetails(PortalId, objOInfo.OrderID)

                        'update total
                        Dim objCInfo As NB_Store_CartInfo = CurrentCart.GetCurrentCart(PortalId)
                        objCInfo.ShipMethodID = objOInfo.ShipMethodID
                        objOInfo = CurrentCart.CreateOrderTotals(objOInfo, objCInfo)

                        objOInfo.OrderGUID = System.Guid.NewGuid.ToString

                        objOInfo.NoteMsg &= vbCrLf & Now.ToString & " - " & Localization.GetString("OrderAmended", LocalResourceFile) & " "

                    End If

                    If txtAlreadyPaid.Visible Then
                        If IsNumeric(txtAlreadyPaid.Text) Then
                            If CDec(txtAlreadyPaid.Text) >= 0 Then
                                objOInfo.AlreadyPaid = CDec(txtAlreadyPaid.Text)
                            End If
                        End If
                    End If

                    If objOInfo.AlreadyPaid < objOInfo.Total Then
                        objOInfo.OrderStatusID = 90
                    End If

                    Dim ElapsedHours As Double
                    ElapsedHours = CDbl(GetStoreSettingInt(PortalId, "elapsedorderhours.limit", "None"))
                    If ElapsedHours > 0 Then
                        objOInfo.ElapsedDate = DateAdd(DateInterval.Hour, ElapsedHours, Today)
                    End If

                End If
            objOCtrl.UpdateObjOrder(objOInfo)

            If Not EventInterface.Instance() Is Nothing Then
                EventInterface.Instance.EditOrderUpdate(PortalId, objOInfo.OrderID)
            End If

            End If

        End Sub



#End Region

    End Class

End Namespace
