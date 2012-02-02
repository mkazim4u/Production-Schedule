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


Imports System
Imports System.Configuration
Imports System.Data
Imports System.Xml
Imports System.Web
Imports System.Collections.Generic

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Common.Utilities
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports DotNetNuke.Services.FileSystem

Namespace NEvoWeb.Modules.NB_Store

    Public Class CurrentCart

#Region "Cart Public Methods"

        Public Shared Function CreateOrder(ByVal UserID As Integer, ByVal PortalID As Integer, ByVal NoteMsg As String, ByVal OrderEmail As String, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo) As NB_Store_OrdersInfo
            Dim objOCtrl As New OrderController
            Dim objInfo As NB_Store_OrdersInfo
            Dim objCInfo As NB_Store_CartInfo
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim EstimatedShipDate As Date

            objSInfo = objSCtrl.GetSetting(PortalID, "ordershipdate.days", GetCurrentCulture)
            If objSInfo Is Nothing Then
                EstimatedShipDate = Nothing
            Else
                If IsNumeric(objSInfo.SettingValue) Then
                    EstimatedShipDate = DateAdd(DateInterval.Day, CInt(objSInfo.SettingValue), Today)
                Else
                    EstimatedShipDate = Nothing
                End If
            End If

            objCInfo = GetCurrentCart(PortalID)

            'check stock levels and product archive of cart.
            ValidateCart(PortalID, UserInfo)

            objInfo = objOCtrl.GetOrder(objCInfo.OrderID)
            'remove old order
            Dim OrdID As Integer = -1
            If Not objInfo Is Nothing Then
                'make sure order has not been payed.
                If Not objInfo.OrderIsPlaced Then
                    'delete old order
                    objOCtrl.DeleteOrder(objInfo.OrderID)
                    objInfo = New NB_Store_OrdersInfo
                Else
                    'allow order to be repayed (if edited)
                    OrdID = objInfo.OrderID

                    'create new order
                    If objInfo.OrderGUID = "" Then
                        'no guid so create new order
                        OrdID = -1
                        objInfo = New NB_Store_OrdersInfo
                    Else
                        ' GUID exists so may have been edited by manager, allow repayment.
                        If objInfo.OrderStatusID = 40 Then
                            'if the payment is OK, the order should not be repayed.
                            '  -- The status should be moved to 90 for edited orders which need extra payments. 
                            OrdID = -1
                            objInfo = New NB_Store_OrdersInfo
                        End If
                    End If

                End If

            Else
                objInfo = New NB_Store_OrdersInfo
            End If

            objInfo.BillingAddressID = -1
            objInfo.NoteMsg = NoteMsg
            objInfo.OrderDate = Now
            objInfo.OrderID = OrdID 'use ordid in case order has been edited.
            objInfo.OrderStatusID = 10
            objInfo.PayType = ""
            objInfo.PortalID = PortalID
            objInfo.PromoCode = objCInfo.PromoCode
            objInfo.ShipDate = EstimatedShipDate
            objInfo.ShippingAddressID = -1
            objInfo.UserID = UserID
            objInfo.VATNumber = objCInfo.VATNumber
            objInfo.Email = OrderEmail
            objInfo.ShipMethodID = objCInfo.ShipMethodID
            objInfo.TrackingCode = ""
            objInfo.ElapsedDate = Null.NullDate
            objInfo.CartXMLInfo = objCInfo.XMLInfo
            objInfo = CreateOrderTotals(objInfo, objCInfo)

            objInfo = objOCtrl.UpdateObjOrder(objInfo)

            'update details
            CreateOrderDetails(PortalID, objInfo.OrderID)

            'update cart orderid to link cart to new order
            Dim objCCtrl As New CartController
            objCInfo.OrderID = objInfo.OrderID
            objCCtrl.UpdateObjCart(objCInfo)

            If Not EventInterface.Instance() Is Nothing Then
                EventInterface.Instance.CreateOrder(PortalID, objInfo.OrderID)
            End If

            Return objInfo
        End Function

        Public Shared Function CreateOrderTotals(ByVal objInfo As NB_Store_OrdersInfo, ByVal objCInfo As NB_Store_CartInfo) As NB_Store_OrdersInfo
            Dim objCartTotals As CartTotals
            're-calc totals of cart to use in order.
            objCartTotals = GetCalulatedTotals(objInfo.PortalID, objCInfo.VATNumber, objCInfo.CountryCode, objCInfo.ShipType, objCInfo.ShipMethodID)

            'create new order
            objInfo.AppliedTax = objCartTotals.TaxAppliedAmt
            objInfo.CalculatedTax = objCartTotals.TaxAmt
            objInfo.Discount = objCartTotals.DiscountAmt
            objInfo.ShippingCost = objCartTotals.ShipAmt
            objInfo.Total = objCartTotals.OrderTotal

            Return objInfo
        End Function

        Public Shared Sub CreateOrderDetails(ByVal PortalID As Integer, ByVal OrderID As Integer)
            'update details
            Dim objOCtrl As New OrderController
            Dim aryList As ArrayList
            Dim objODInfo As NB_Store_OrderDetailsInfo
            Dim objMInfo As NB_Store_ModelInfo
            Dim objPCtrl As New ProductController

            'remove current order details.
            aryList = objOCtrl.GetOrderDetailList(OrderID)
            For Each objOD As NB_Store_OrderDetailsInfo In aryList
                objOCtrl.DeleteOrderDetail(objOD.OrderDetailID)
            Next

            aryList = GetCurrentCartItems(PortalID)
            For Each objCI As NB_Store_CartItemsInfo In aryList
                objODInfo = New NB_Store_OrderDetailsInfo
                objODInfo.Discount = objCI.Discount
                objODInfo.ItemDesc = objCI.ItemDesc
                objODInfo.ModelID = objCI.ModelID
                objODInfo.OptCode = objCI.OptCode
                objODInfo.OrderDetailID = -1
                objODInfo.OrderID = OrderID
                objODInfo.Quantity = objCI.Quantity
                objODInfo.Tax = objCI.Tax
                objODInfo.UnitCost = objCI.UnitCost
                objODInfo.ProductURL = objCI.ProductURL
                objODInfo.CartXMLInfo = objCI.XMLInfo
                objMInfo = objPCtrl.GetModel(objCI.ModelID, GetCurrentCulture)
                If objMInfo Is Nothing Then
                    objODInfo.PurchaseCost = 0
                Else
                    objODInfo.PurchaseCost = objMInfo.PurchaseCost
                End If
                objOCtrl.UpdateObjOrderDetail(objODInfo)
            Next
        End Sub

        Public Shared Function CreateCartFromOrder(ByVal PortalID As String, ByVal OrderID As Integer, ByVal CreateNewOrder As Boolean) As String
            Dim objOCtrl As New OrderController
            Dim objCCtrl As New CartController
            Dim objInfo As NB_Store_OrdersInfo
            Dim objCInfo As NB_Store_CartInfo
            Dim CartID As String

            'delete the current user cart
            CartID = getCartID(PortalID)
            objCCtrl.DeleteCart(CartID)

            'rebuild cart with order details
            objInfo = objOCtrl.GetOrder(OrderID)
            If Not objInfo Is Nothing Then
                Dim objBInfo As NB_Store_AddressInfo
                objBInfo = objOCtrl.GetOrderAddress(objInfo.BillingAddressID)

                objCInfo = New NB_Store_CartInfo
                objCInfo.BankHtmlRedirect = ""
                objCInfo.BankTransID = -1
                objCInfo.CartDiscount = objInfo.Discount
                objCInfo.CartID = CartID
                objCInfo.CountryCode = objBInfo.CountryCode
                objCInfo.DateCreated = Now
                If CreateNewOrder Then
                    objCInfo.OrderID = -1
                Else
                    objCInfo.OrderID = OrderID
                End If
                objCInfo.PortalID = PortalID
                objCInfo.PromoCode = ""
                objCInfo.ShipMethodID = GetDefaultShipMethod(PortalID)
                objCInfo.ShipType = ""
                objCInfo.UserID = -1
                objCInfo.VATNumber = ""
                objCInfo.XMLInfo = objInfo.CartXMLInfo

                objCCtrl.UpdateObjCart(objCInfo)

                'rebuild cart details
                Dim aryList As ArrayList
                aryList = objOCtrl.GetOrderDetailList(OrderID)

                Dim objCIInfo As NB_Store_CartItemsInfo
                For Each objOD As NB_Store_OrderDetailsInfo In aryList
                    objCIInfo = New NB_Store_CartItemsInfo

                    objCIInfo.CartID = CartID
                    objCIInfo.DateCreated = Now
                    objCIInfo.Discount = objOD.Discount
                    objCIInfo.ItemDesc = objOD.ItemDesc
                    objCIInfo.ModelID = objOD.ModelID
                    objCIInfo.OptCode = objOD.OptCode
                    objCIInfo.Quantity = objOD.Quantity
                    objCIInfo.Tax = objOD.Tax
                    objCIInfo.UnitCost = objOD.UnitCost
                    objCIInfo.ProductURL = objOD.ProductURL
                    objCIInfo.XMLInfo = objOD.CartXMLInfo

                    AddItemToCart(PortalID, objCIInfo)
                Next


            End If

            Return CartID

        End Function

        Public Shared Function GetCartItemTotal(ByVal PortalID As Integer) As Decimal
            Dim aryList As ArrayList
            Dim objInfo As NB_Store_CartItemsInfo
            Dim TotalAmt As Decimal = 0

            aryList = GetCurrentCartItems(PortalID)

            For Each objInfo In aryList
                TotalAmt += ((objInfo.Quantity * objInfo.UnitCost))
            Next

            If TotalAmt < 0 Then TotalAmt = 0

            Return TotalAmt
        End Function

        Public Shared Function GetCalulatedTotals(ByVal PortalID As Integer, Optional ByVal ShipMethodID As Integer = -1) As CartTotals
            Dim objCInfo As NB_Store_CartInfo
            objCInfo = GetCurrentCart(PortalID)
            Return GetCalulatedTotals(PortalID, objCInfo.VATNumber, objCInfo.CountryCode, objCInfo.ShipType, ShipMethodID, objCInfo)
        End Function

        Public Shared Function GetCalulatedTotals(ByVal PortalID As Integer, ByVal VATCode As String, ByVal CountryCode As String, ByVal ShipType As String, ByVal ShipMethodID As Integer) As CartTotals
            Return GetCalulatedTotals(PortalID, VATCode, CountryCode, ShipType, ShipMethodID, Nothing)
        End Function

        Public Shared Function GetCalulatedTotals(ByVal PortalID As Integer, ByVal VATCode As String, ByVal CountryCode As String, ByVal ShipType As String, ByVal ShipMethodID As Integer, ByVal objCart As NB_Store_CartInfo) As CartTotals
            Dim rtnCartTotals As CartTotals = Nothing
            Dim aryList As ArrayList
            Dim objInfo As NB_Store_CartItemsInfo
            Dim objShipCtrl As New ShipController
            Dim objTaxCalc As New TaxCalcController(PortalID)
            Dim objCTaxInfo As CartTaxInfo


            'check if we have a cartlevel CartCalcProvider
            If Not CalcCartInterface.Instance() Is Nothing Then
                rtnCartTotals = CalcCartInterface.Instance.getCartTotals(PortalID, getCartID(PortalID))
            End If
            If rtnCartTotals Is Nothing Then
                rtnCartTotals = New CartTotals

                aryList = GetCurrentCartItems(PortalID)

                For Each objInfo In aryList
                    rtnCartTotals.DiscountAmt += RoundToStoreCurrency(objInfo.Quantity * objInfo.Discount)
                    rtnCartTotals.TaxAmt += objInfo.Tax
                    rtnCartTotals.TotalAmt += RoundToStoreCurrency(objInfo.Quantity * objInfo.UnitCost)
                    rtnCartTotals.Qty += objInfo.Quantity
                Next
            End If

            'check if we have a cartlevel CartCalcProvider
            If Not CalcShipInterface.Instance() Is Nothing Then
                rtnCartTotals = CalcShipInterface.Instance.getCartTotals(PortalID, getCartID(PortalID), rtnCartTotals, ShipType, CountryCode, ShipMethodID)
            Else
                'calculate shipping based on shipping rules
                If ShipType = "NONE" Or rtnCartTotals.TotalAmt = 0 Then
                    rtnCartTotals.ShipAmt = 0
                Else
                    If ShipMethodID < 0 Then
                        ShipMethodID = GetDefaultShipMethod(PortalID)
                    End If
                    rtnCartTotals.ShipAmt = objShipCtrl.getShippingCost(PortalID, CountryCode, ShipMethodID)
                End If
            End If


            'check if we have a cartlevel CartCalcProvider
            If Not CalcTaxInterface.Instance() Is Nothing Then
                rtnCartTotals = CalcTaxInterface.Instance.getCartTotals(PortalID, getCartID(PortalID), rtnCartTotals, CountryCode, VATCode)
            Else
                'get applied tax based on if VAT code exists
                objCTaxInfo = objTaxCalc.getCartTaxDetails(PortalID, rtnCartTotals.ShipAmt, GetMerchantCountryCode(PortalID), VATCode)
                If objCTaxInfo.TaxOption = "3" Then
                    rtnCartTotals.TaxAppliedAmt = CDec(objCTaxInfo.TaxAmount) + CDec(objCTaxInfo.ShipTax)
                ElseIf objCTaxInfo.TaxOption = "2" Then
                    If CDec(objCTaxInfo.TaxAmount) < 0 Then
                        rtnCartTotals.TaxAppliedAmt = CDec(objCTaxInfo.TaxAmount)
                    Else
                        rtnCartTotals.TaxAppliedAmt = 0
                    End If
                Else
                    rtnCartTotals.TaxAppliedAmt = 0
                End If
                rtnCartTotals.TaxAmt = CDec(objCTaxInfo.TaxAmount) + CDec(objCTaxInfo.ShipTax)
            End If

            If Not CalcDiscountInterface.Instance() Is Nothing Then
                rtnCartTotals = CalcDiscountInterface.Instance.getCartTotals(PortalID, getCartID(PortalID), rtnCartTotals)
            Else
                'discount only applies to cart items, if more then make discount same as total of items
                'TODO: When Vouchers are enabled, this will need to change.
                If (rtnCartTotals.TotalAmt + rtnCartTotals.DiscountAmt) < 0 Then
                    rtnCartTotals.DiscountAmt = (rtnCartTotals.TotalAmt * -1)
                End If
            End If

            If rtnCartTotals.ShipAmt < 0 Then ' negitive shipping valid for special shipping, so don't include in total.
                rtnCartTotals.OrderTotal = rtnCartTotals.TotalAmt + rtnCartTotals.TaxAppliedAmt + rtnCartTotals.DiscountAmt
            Else
                rtnCartTotals.OrderTotal = rtnCartTotals.TotalAmt + rtnCartTotals.TaxAppliedAmt + rtnCartTotals.ShipAmt + rtnCartTotals.DiscountAmt
            End If

            If rtnCartTotals.OrderTotal < 0 Then
                rtnCartTotals.OrderTotal = 0
            End If

            If Not objCart Is Nothing Then
                If objCart.OrderID > 0 Then
                    Dim objO As NB_Store_OrdersInfo
                    Dim objOCtrl As New OrderController
                    objO = objOCtrl.GetOrder(objCart.OrderID)
                    If Not objO Is Nothing Then
                        If objO.AlreadyPaid > 0 Then
                            rtnCartTotals.Balance = rtnCartTotals.OrderTotal - objO.AlreadyPaid
                        End If
                    End If
                End If
            End If

            Return rtnCartTotals
        End Function

        Public Shared Function GetCurrentCartItems(ByVal PortalID As Integer) As ArrayList
            Dim objCtrl As New CartController
            Dim CartID As String
            Dim aryList As ArrayList

            CartID = getCartID(PortalID)

            aryList = objCtrl.GetCartItemList(CartID)

            Return aryList
        End Function

        Public Shared Function GetLastCartItem(ByVal PortalID As Integer) As NB_Store_CartItemsInfo
            Dim aryList As ArrayList

            aryList = CurrentCart.GetCurrentCartItems(PortalID)
            If aryList.Count >= 1 Then
                Return aryList.Item(aryList.Count - 1)
            Else
                Return Nothing
            End If

        End Function

        Public Shared Function GetCurrentCartItem(ByVal PortalID As Integer, ByVal CartItemID As Integer) As NB_Store_CartItemsInfo
            Dim objCtrl As New CartController
            Dim CartID As String
            Dim objInfo As NB_Store_CartItemsInfo
            CartID = getCartID(PortalID)

            objInfo = objCtrl.GetCartItem(CartItemID)

            Return objInfo
        End Function

        Public Shared Function IsCartEmpty(ByVal PortalID As Integer) As Boolean
            Dim objCtrl As New CartController
            Dim CartID As String
            CartID = getCartID(PortalID)
            If objCtrl.GetCartItemList(CartID).Count = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsCartAboveMinimum(ByVal PortalID As Integer) As Boolean
            Dim objCtrl As New NB_Store.SettingsController
            Dim objInfo As NB_Store_SettingsInfo
            Dim MinimumTotal As Double = -1

            objInfo = objCtrl.GetSetting(PortalID, "minimumcarttotal.limit", GetCurrentCulture)
            If Not objInfo Is Nothing Then
                If IsNumeric(objInfo.SettingValue) Then
                    MinimumTotal = CDbl(objInfo.SettingValue)
                End If
            End If
            If MinimumTotal > 0 Then
                If GetCartItemTotal(PortalID) < MinimumTotal Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return True
            End If
        End Function

        Public Shared Function GetCurrentCart(ByVal PortalID As Integer) As NB_Store_CartInfo
            Dim objCtrl As New CartController
            Dim objCInfo As NB_Store_CartInfo
            Dim CartID As String

            CartID = getCartID(PortalID)

            objCInfo = objCtrl.GetCart(CartID)
            If objCInfo Is Nothing Then
                objCInfo = New NB_Store_CartInfo
                objCInfo.CartID = CartID
                objCInfo.DateCreated = Now
                objCInfo.OrderID = -1
                objCInfo.PortalID = PortalID
                objCInfo.UserID = -1
                objCInfo.ShipMethodID = GetDefaultShipMethod(PortalID)
                objCtrl.UpdateObjCart(objCInfo)
            End If

            Return objCInfo
        End Function

        Public Shared Sub Save(ByVal objCInfo As NB_Store_CartInfo)
            Dim objCtrl As New CartController
            objCtrl.UpdateObjCart(objCInfo)
        End Sub

        Public Shared Sub SaveShipType(ByVal PortalID As Integer, ByVal ShipType As String)
            Dim objCtrl As New CartController
            Dim objCInfo As NB_Store_CartInfo = GetCurrentCart(PortalID)
            objCInfo.ShipType = ShipType
            objCtrl.UpdateObjCart(objCInfo)
        End Sub

        Public Shared Sub RemoveItemFromCart(ByVal ItemId As Integer)
            Dim objCtrl As New CartController
            objCtrl.DeleteCartItem(ItemId)
        End Sub

        Public Shared Sub UpdateCartItemQty(ByVal ItemId As Integer, ByVal NewQty As Integer)
            Dim objCtrl As New CartController
            Dim objCIInfo As NB_Store_CartItemsInfo
            objCIInfo = objCtrl.GetCartItem(ItemId)
            If Not objCIInfo Is Nothing Then
                objCIInfo.Quantity = NewQty
                objCIInfo.DateCreated = Now
                objCtrl.UpdateObjCartItem(objCIInfo)
            End If
        End Sub

        Public Shared Sub UpdateCartItemUnitCost(ByVal ItemId As Integer, ByVal UnitCost As Decimal)
            Dim objCtrl As New CartController
            Dim objCIInfo As NB_Store_CartItemsInfo
            objCIInfo = objCtrl.GetCartItem(ItemId)
            If Not objCIInfo Is Nothing Then
                objCIInfo.UnitCost = UnitCost
                objCIInfo.DateCreated = Now
                objCtrl.UpdateObjCartItem(objCIInfo)
            End If
        End Sub

        Public Shared Function CheckStockByCartItemID(ByVal PortalID As Integer, ByVal CartItemID As Integer, ByVal Qty As Integer) As Integer
            Dim objInfo As NB_Store_CartItemsInfo
            Dim rtnQty As Integer = Qty
            objInfo = CurrentCart.GetCurrentCartItem(PortalID, CartItemID)
            If Not objInfo Is Nothing Then
                rtnQty = CheckStock(objInfo.ModelID, Qty)
            End If
            Return rtnQty
        End Function

        Public Shared Function CheckStock(ByVal ModelID As Integer, ByVal Qty As Integer) As Integer
            Dim objCtrl As New ProductController
            Dim objMInfo As NB_Store_ModelInfo
            Dim rtnQty As Integer = Qty
            Dim QtyRemaining As Integer

            objMInfo = objCtrl.GetModel(ModelID, GetCurrentCulture)
            QtyRemaining = objMInfo.QtyRemaining

            'Get stock already in Portal carts.
            Dim blnLockOnCart As Boolean = GetStoreSettingBoolean(objMInfo.PortalID, "lockstockoncart")
            If blnLockOnCart And QtyRemaining > 0 Then
                Dim objCCtrl As New CartController
                Dim PortalQtyInCarts As Integer = 0
                'stock is locked when added to cart so get qty for all carts in portal
                PortalQtyInCarts = objCCtrl.GetCartModelQty(objMInfo.PortalID, objMInfo.ModelID)
                QtyRemaining = QtyRemaining - (PortalQtyInCarts - Qty)
                If QtyRemaining < 0 Then QtyRemaining = 0
            End If

            'stock control turned off if set to < 0
            If QtyRemaining >= 0 Then
                If QtyRemaining < Qty Then
                    'check for qty in transaction 
                    If Now < DateAdd(DateInterval.Minute, 10, objMInfo.QtyTransDate) Then
                        If objMInfo.QtyTrans > 0 Then
                            QtyRemaining = QtyRemaining - objMInfo.QtyTrans
                        End If
                    End If
                    rtnQty = QtyRemaining
                End If
            End If

            'check if model limit is applied
            If objMInfo.Allow >= 0 And GetStoreSettingBoolean(objMInfo.PortalID, "allowcartmodellimit.flag") Then
                If rtnQty > objMInfo.Allow Then
                    rtnQty = objMInfo.Allow
                End If
            End If

            Return rtnQty
        End Function

        Public Shared Sub AddItemToCart(ByVal PortalID As Integer, ByVal objCIInfo As NB_Store_CartItemsInfo, Optional ByVal Request As HttpRequest = Nothing)
            Dim objCtrl As New CartController
            Dim objCInfo As NB_Store_CartInfo
            Dim objTaxCalc As New TaxCalcController(PortalID)
            Dim objPTaxInfo As ProductTaxInfo

            If GetStoreSettingBoolean(PortalID, "singlepurchase.flag") Then
                'only allow 1 item in the cart, so remove the current one.
                CurrentCart.DeleteCartItems(PortalID)
                objCIInfo.Quantity = 1
            End If

            'create Cart
            objCInfo = GetCurrentCart(PortalID)

            objCIInfo.CartID = objCInfo.CartID

            'build product url
            If Not Request Is Nothing Then
                objCIInfo.ProductURL = Request.Url.OriginalString
            End If

            If objCIInfo.Quantity > 0 Then
                'calculate tax amounts for product
                objPTaxInfo = objTaxCalc.getProductTaxDetails(PortalID, objCIInfo)
                objCIInfo.Tax = objPTaxInfo.TaxAmount
                'only add if qty valid
                Dim StockQty As Integer = CheckStock(objCIInfo.ModelID, objCIInfo.Quantity)
                If StockQty > 0 Then
                    objCIInfo.Quantity = StockQty
                    Dim CartItemID As Integer = objCtrl.UpdateObjCartItem(objCIInfo)
                    If Not EventInterface.Instance() Is Nothing Then
                        EventInterface.Instance.AddItemToCart(PortalID, CartItemID)
                    End If
                End If
            End If
        End Sub

        Public Shared Sub AddItemToCart(ByVal PortalID As Integer, ByVal LItem As DataListItem, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo, Optional ByVal Request As HttpRequest = Nothing)
            AddItemToCart(PortalID, LItem, False, UserInfo, Request)
        End Sub

        Public Shared Sub AddItemToCart(ByVal PortalID As Integer, ByVal LItem As DataListItem, ByVal IncrementCart As Boolean, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo, Optional ByVal Request As HttpRequest = Nothing)
            Dim objCCtrl As New CartController
            Dim modelID As Integer = -1
            Dim objCIInfo As NB_Store_CartItemsInfo
            Dim Ctrl As Control
            Dim ddlOptionCtrls As New Collection
            Dim rblOptionCtrls As New Collection
            Dim txtOptionCtrls As New Collection
            Dim chkOptionCtrls As New Collection
            Dim fuCtrls As New Collection ' file upload controls
            Dim intQty As Integer = 1
            Dim txtOption As String

            Ctrl = LItem.FindControl("hfModel")
            If Not Ctrl Is Nothing Then
                If IsNumeric(DirectCast(Ctrl, HiddenField).Value) Then
                    modelID = CInt(DirectCast(Ctrl, HiddenField).Value)
                End If
            End If

            Ctrl = LItem.FindControl("ddlModel")
            If Ctrl Is Nothing Then
                Ctrl = LItem.FindControl("ddlModelsel")
            End If
            If Not Ctrl Is Nothing Then
                If Ctrl.Visible Then
                    If IsNumeric(DirectCast(Ctrl, DropDownList).SelectedValue) Then
                        modelID = CInt(DirectCast(Ctrl, DropDownList).SelectedValue)
                    End If
                End If
            End If

            Ctrl = LItem.FindControl("rblModel")
            If Ctrl Is Nothing Then
                Ctrl = LItem.FindControl("rblModelsel")
            End If
            If Not Ctrl Is Nothing Then
                If Ctrl.Visible Then
                    If IsNumeric(DirectCast(Ctrl, RadioButtonList).SelectedValue) Then
                        modelID = CInt(DirectCast(Ctrl, RadioButtonList).SelectedValue)
                    End If
                End If
            End If

            Ctrl = LItem.FindControl("ddlQty")
            If Not Ctrl Is Nothing Then
                If Ctrl.Visible Then
                    If IsNumeric(DirectCast(Ctrl, DropDownList).SelectedValue) Then
                        intQty = CInt(DirectCast(Ctrl, DropDownList).SelectedValue)
                    End If
                End If
            End If

            Ctrl = LItem.FindControl("txtQty")
            If Not Ctrl Is Nothing Then
                If Ctrl.Visible Then
                    If IsNumeric(DirectCast(Ctrl, TextBox).Text) Then
                        intQty = CInt(DirectCast(Ctrl, TextBox).Text)
                    End If
                End If
            End If


            'build list of option controls
            For Each Ctrl In LItem.Controls
                If TypeOf Ctrl Is DropDownList Then
                    If Ctrl.ID.StartsWith("option") And Ctrl.Visible = True Then
                        ddlOptionCtrls.Add(Ctrl)
                    End If
                End If
                If TypeOf Ctrl Is RadioButtonList Then
                    If Ctrl.ID.StartsWith("option") And Ctrl.Visible = True Then
                        rblOptionCtrls.Add(Ctrl)
                    End If
                End If
                If TypeOf Ctrl Is TextBox Then
                    If Ctrl.ID.StartsWith("option") And Ctrl.Visible = True Then
                        txtOptionCtrls.Add(Ctrl)
                    End If
                End If
                If TypeOf Ctrl Is CheckBox Then
                    If CType(Ctrl, CheckBox).Checked Then
                        If Ctrl.ID.StartsWith("chkoption") And Ctrl.Visible = True Then
                            chkOptionCtrls.Add(Ctrl)
                        End If
                    End If
                End If
                If TypeOf Ctrl Is FileUpload Then
                    If CType(Ctrl, FileUpload).FileName <> "" Then
                        If Ctrl.Visible = True Then
                            fuCtrls.Add(Ctrl)
                        End If
                    End If
                End If
            Next

            'build optionCode for cart
            Dim optCode As String = ""
            Dim ddl As DropDownList
            For Each ddl In ddlOptionCtrls
                optCode &= CType(ddl, DropDownList).SelectedValue & "-"
            Next
            Dim rbl As RadioButtonList
            For Each rbl In rblOptionCtrls
                optCode &= CType(rbl, RadioButtonList).SelectedValue & "-"
            Next
            Dim chk As CheckBox
            For Each chk In chkOptionCtrls
                optCode &= CType(chk, CheckBox).Attributes.Item("OptionValueID") & "-"
            Next

            'get text input and save to cart XML
            txtOption = ""
            Dim txt As TextBox
            Dim strXML As String = ""
            Dim optSep As String = GetStoreSettingText(PortalID, "optionseperator.text", GetCurrentCulture, False, True)
            If optSep = "" Then optSep = ","
            For Each txt In txtOptionCtrls
                strXML &= "<" & txt.ID.ToLower & ">" & txt.Text & "</" & txt.ID.ToLower & ">"
                If CType(txt, TextBox).ToolTip = "" Then
                    txtOption &= CType(txt, TextBox).Text & optSep
                Else
                    txtOption &= CType(txt, TextBox).ToolTip & "=" & CType(txt, TextBox).Text & optSep
                End If
            Next

            'do file upload and save path to cart XML
            For Each fu As FileUpload In fuCtrls
                Try
                    strXML &= "<" & fu.ID.ToLower & ">" & UploadOrderFile(fu) & "</" & fu.ID.ToLower & ">"
                Catch ex As Exception
                    strXML &= "<" & fu.ID.ToLower & ">ERROR</" & fu.ID.ToLower & ">"
                End Try
            Next

            If txtOption.EndsWith(optSep) Then
                txtOption = txtOption.Substring(0, (txtOption.Length - optSep.Length))
            End If
            If strXML <> "" Then
                strXML = "<root>" & strXML & "</root>"
            End If
            optCode = modelID.ToString & "-" & optCode
            optCode = optCode.TrimEnd("-"c)

            'validate optcode and get ItemDesc
            Dim OptCInfo As OptCodeInfo = GetOptCodeInfo(PortalID, optCode, UserInfo, txtOption, strXML)
            optCode = OptCInfo.OptCode

            Dim CartID As String
            CartID = getCartID(PortalID)

            objCIInfo = objCCtrl.GetCartItemByOptCode(CartID, optCode)
            If objCIInfo Is Nothing Or txtOption <> "" Then ' create new cart item if textbox data has been entered
                objCIInfo = New NB_Store_CartItemsInfo
                objCIInfo.OptCode = optCode
                objCIInfo.ItemDesc = OptCInfo.ItemDesc
                objCIInfo.DateCreated = Now
                objCIInfo.Discount = OptCInfo.Discount
                objCIInfo.ItemID = -1
                objCIInfo.ModelID = modelID
                objCIInfo.Quantity = intQty
                objCIInfo.Tax = 0
                objCIInfo.UnitCost = OptCInfo.UnitCost
                objCIInfo.XMLInfo = strXML
            Else
                objCIInfo.DateCreated = Now
                objCIInfo.Discount = OptCInfo.Discount
                If IncrementCart Then
                    objCIInfo.Quantity = objCIInfo.Quantity + intQty
                Else
                    objCIInfo.Quantity = intQty
                End If
                objCIInfo.Tax = 0
                objCIInfo.ItemDesc = OptCInfo.ItemDesc
                objCIInfo.UnitCost = OptCInfo.UnitCost
            End If

            If modelID >= 0 Then
                AddItemToCart(PortalID, objCIInfo, Request)
            End If

            'validate cart so cart level discount are calculated
            ValidateCart(PortalID, UserInfo)



        End Sub

        Private Shared Function UploadOrderFile(ByVal fu As FileUpload) As String
            Dim strMsg As String = ""
            Dim fs As New FileSystemUtils
            Dim objCtrl As New ProductController
            Dim HideFlag As Boolean = False
            Dim strGUID As String
            Dim DocPath As String = ""
            If fu.FileName <> "" Then

                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings

                CreateDir(PS, ORDERUPLOADFOLDER)

                Dim NewFileName As String = ""
                strGUID = Guid.NewGuid.ToString

                DocPath = PS.HomeDirectoryMapPath & ORDERUPLOADFOLDER & "\" & strGUID & System.IO.Path.GetExtension(fu.FileName)

                Dim strFolderpath As String = GetSubFolderPath(DocPath, PS.PortalId)

                FileSystemUtils.UploadFile(DocPath, fu.PostedFile)
                FileSystemUtils.MoveFile(DocPath & fu.FileName, DocPath, PS)

                Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo = FileSystemUtils.GetFolder(PS.PortalId, ORDERUPLOADFOLDER)
                If folderInfo.StorageLocation = FolderController.StorageLocationTypes.SecureFileSystem Then
                    DocPath = PS.HomeDirectoryMapPath & ORDERUPLOADFOLDER & "\" & strGUID & System.IO.Path.GetExtension(fu.FileName) & glbProtectedExtension
                End If


            End If
            Return DocPath
        End Function

        Public Shared Function getCartID(ByVal PortalID As Integer) As String
            Dim CartCookieName As String = "NB_Store_Portal_"
            Dim cartID As String = ""
            Dim cartCookie As HttpCookie = HttpContext.Current.Request.Cookies(CartCookieName & PortalID.ToString)
            If Not (cartCookie Is Nothing) Then
                cartID = cartCookie("CartID")
            Else
                cartID = System.Guid.NewGuid.ToString
                setCartID(PortalID, cartID)
            End If
            Return cartID
        End Function

        Private Shared Sub setCartID(ByVal PortalID As Integer, ByVal cartID As String)
            Dim CartCookieName As String = "NB_Store_Portal_"
            Dim cartCookie As HttpCookie = New HttpCookie(CartCookieName & PortalID.ToString)
            cartCookie("CartID") = cartID
            Dim cookieExpire As Integer = 30

            If GetStoreSetting(PortalID, "cookiecart.expire", "None") = "0" Then
                cartCookie.Expires = Nothing
            Else
                If IsNumeric(GetStoreSetting(PortalID, "cookiecart.expire", "None")) Then
                    cookieExpire = CInt(GetStoreSetting(PortalID, "cookiecart.expire", "None"))
                End If
                cartCookie.Expires = DateAdd(DateInterval.Day, cookieExpire, Today)
            End If

            HttpContext.Current.Response.Cookies.Add(cartCookie)
        End Sub

        Public Shared Function AdjustCartStockInModels(ByVal PortalID As Integer, ByVal aryList As ArrayList) As ArrayList
            Dim rtnList As New ArrayList
            Dim objMInfo As NB_Store_ModelInfo
            Dim blnLockOnCart As Boolean = False
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim CartQty As Integer
            Dim objCCtrl As New CartController

            objSInfo = objSCtrl.GetSetting(PortalID, "lockstockoncart", GetCurrentCulture)
            If Not objSInfo Is Nothing Then
                blnLockOnCart = CBool(objSInfo.SettingValue)
            End If

            For Each objMInfo In aryList
                If objMInfo.QtyRemaining >= 0 Then
                    If blnLockOnCart Then
                        'stock is locked when added to cart so get qty for all carts in portal
                        CartQty = objCCtrl.GetCartModelQty(PortalID, objMInfo.ModelID)
                    Else
                        CartQty = objCCtrl.GetCartModelQty(PortalID, objMInfo.ModelID, CurrentCart.getCartID(PortalID))
                    End If
                    objMInfo.QtyRemaining = objMInfo.QtyRemaining - CartQty
                    If objMInfo.QtyRemaining > 0 Then
                        rtnList.Add(objMInfo)
                    End If
                Else
                    rtnList.Add(objMInfo)
                End If
            Next
            Return rtnList
        End Function

        Public Shared Sub CancelOrder(ByVal PortalID As Integer)
            Dim objCtrl As New CartController
            Dim objOCtrl As New OrderController
            Dim objCInfo As NB_Store_CartInfo = GetCurrentCart(PortalID)
            Dim objOInfo As NB_Store_OrdersInfo

            objOInfo = objOCtrl.GetOrder(objCInfo.OrderID)
            If Not objOInfo Is Nothing Then
                If objOInfo.OrderNumber = "" Then ' only ever delete if no order number
                    objOCtrl.DeleteOrder(objCInfo.OrderID)
                Else
                    'change status of order
                    objOInfo.OrderStatusID = 30
                    objOCtrl.UpdateObjOrder(objOInfo)
                End If
            End If

            objCtrl.DeleteCart(objCInfo.CartID)
        End Sub

        Public Shared Sub DeleteCart(ByVal PortalID As Integer)
            Dim objCtrl As New CartController
            Dim objCInfo As NB_Store_CartInfo = GetCurrentCart(PortalID)
            objCtrl.DeleteCart(objCInfo.CartID)
        End Sub

        Public Shared Sub DeleteCartItems(ByVal PortalID As Integer)
            Dim objCtrl As New CartController
            Dim objCInfo As NB_Store_CartInfo = GetCurrentCart(PortalID)
            Dim aryList As ArrayList
            aryList = objCtrl.GetCartItemList(objCInfo.CartID)
            For Each obj As NB_Store_CartItemsInfo In aryList
                objCtrl.DeleteCartItem(obj.ItemID)
            Next
        End Sub

        Public Shared Sub ValidateCart(ByVal PortalID As Integer, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo)
            Dim aryCartItems As ArrayList
            Dim objPInfo As NB_Store_ProductsInfo
            Dim objMInfo As NB_Store_ModelInfo
            Dim objCInfo As NB_Store_CartItemsInfo
            Dim objPCtrl As New ProductController
            Dim objCCtrl As New CartController
            Dim objTaxCalc As New TaxCalcController(PortalID)
            Dim objPTaxInfo As ProductTaxInfo
            Dim objSTCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo

            aryCartItems = GetCurrentCartItems(PortalID)

            For Each objCInfo In aryCartItems
                objMInfo = objPCtrl.GetModel(objCInfo.ModelID, GetCurrentCulture)
                objPInfo = objPCtrl.GetProduct(objMInfo.ProductID, GetCurrentCulture)
                If objPInfo Is Nothing Or objMInfo Is Nothing Then
                    RemoveItemFromCart(objCInfo.ItemID)
                Else
                    If objPInfo.Archived Or objPInfo.IsDeleted Or objMInfo.Deleted Then
                        RemoveItemFromCart(objCInfo.ItemID)
                    Else
                        'Check for limitied Purchase category.
                        Dim blnFound As Boolean = True
                        Dim LimitedCategoryPurchase As String
                        LimitedCategoryPurchase = GetStoreSetting(PortalID, "categorypurchase.list")
                        If LimitedCategoryPurchase <> "" Then
                            Dim lst As String()
                            Dim aryList As ArrayList

                            lst = Split(LimitedCategoryPurchase, ",")

                            blnFound = False
                            For lp As Integer = 0 To lst.GetUpperBound(0)
                                aryList = objPCtrl.GetCategoriesAssigned(objPInfo.ProductID)
                                For Each objC As NB_Store_ProductCategoryInfo In aryList
                                    If IsNumeric(lst(lp)) Then
                                        If objC.CategoryID = CInt(lst(lp)) Then
                                            blnFound = True
                                            Exit For
                                        End If
                                    End If
                                Next
                            Next
                            'remove if not in list
                            If Not blnFound Then
                                RemoveItemFromCart(objCInfo.ItemID)
                            End If
                        End If

                        If blnFound Then

                            'Check Stock..
                            Dim StockQty As Integer = CheckStock(objCInfo.ModelID, objCInfo.Quantity)
                            If StockQty > 0 Then

                                Dim QtyLimit As Integer = 999999
                                objSInfo = objSTCtrl.GetSetting(PortalID, "productqty.limit", GetCurrentCulture)
                                If Not objSInfo Is Nothing Then
                                    If IsNumeric(objSInfo.SettingValue) Then
                                        QtyLimit = CInt(objSInfo.SettingValue)
                                        If StockQty > QtyLimit Then StockQty = QtyLimit
                                    End If
                                End If

                                objCInfo.Quantity = StockQty


                                're-calc unit cost with options.
                                Dim OptCInfo As OptCodeInfo = GetOptCodeInfo(PortalID, objCInfo.OptCode, UserInfo, "", objCInfo.XMLInfo)
                                If OptCInfo.OptCode = "" Then
                                    RemoveItemFromCart(objCInfo.ItemID)
                                Else

                                    'if order has guid then the model price could have been adjusted.
                                    ' therefore don't update the discount and unit cost.
                                    Dim objCart As NB_Store_CartInfo
                                    objCart = GetCurrentCart(PortalID)
                                    If objCart.OrderID > 0 Then
                                        Dim objOInfo As NB_Store_OrdersInfo
                                        Dim objOCtrl As New OrderController
                                        objOInfo = objOCtrl.GetOrder(objCart.OrderID)
                                        If Not objOInfo Is Nothing Then
                                            If objOInfo.OrderGUID = "" Then
                                                objCInfo.Discount = OptCInfo.Discount
                                                objCInfo.UnitCost = OptCInfo.UnitCost
                                            End If
                                        Else
                                            objCInfo.Discount = OptCInfo.Discount
                                            objCInfo.UnitCost = OptCInfo.UnitCost
                                        End If
                                    Else
                                        objCInfo.Discount = OptCInfo.Discount
                                        objCInfo.UnitCost = OptCInfo.UnitCost
                                    End If

                                    'calculate tax amounts for product
                                    objPTaxInfo = objTaxCalc.getProductTaxDetails(PortalID, objCInfo)
                                    objCInfo.Tax = objPTaxInfo.TaxAmount

                                    objCCtrl.UpdateObjCartItem(objCInfo)
                                    End If
                            Else
                                    RemoveItemFromCart(objCInfo.ItemID)
                            End If
                        End If
                    End If
                End If
            Next

            'calc discount
            Dim objCartInfo As NB_Store_CartInfo
            objCartInfo = GetCurrentCart(PortalID)

            Dim chk As String = GetStoreSetting(PortalID, "allowmultidiscount.flag", "None")
            If chk = "" Then chk = "false"
            Dim AllowMultiDis As Boolean = CBool(chk)

            calcCartLevelDiscount(PortalID, UserInfo, AllowMultiDis)

            calcCouponLevelDiscount(PortalID, UserInfo, objCartInfo.PromoCode, AllowMultiDis)

        End Sub


        Public Shared Function calcCartLevelDiscount(ByVal PortalID As Integer, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo, ByVal AllowMultiDis As Boolean) As Decimal
            Dim objPromoCtrl As New PromoController
            Dim objCCtrl As New CartController
            Dim objCInfo As NB_Store_CartInfo = GetCurrentCart(PortalID)

            objCInfo.CartDiscount = objPromoCtrl.getCartLevelDiscount(PortalID, UserInfo, AllowMultiDis)

            objCCtrl.UpdateObjCart(objCInfo)

            '-------------------------------------------------
            'aportion discount across products in cart.
            Dim detInfoList As ArrayList
            Dim CartDiscount As Decimal = (objCInfo.CartDiscount * -1)
            Dim TotalInCart As Decimal

            If CartDiscount > 0 Then

                detInfoList = CurrentCart.GetCurrentCartItems(PortalID)
                TotalInCart = CurrentCart.GetCartItemTotal(PortalID)

                AllocateAcrossCart(PortalID, detInfoList, CartDiscount, TotalInCart, AllowMultiDis)

            End If
            '-------------------------------------------------

            Return objCInfo.CartDiscount
        End Function

        Public Shared Function calcCouponLevelDiscount(ByVal PortalID As Integer, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo, ByVal CouponCode As String, ByVal AllowMultiDis As Boolean) As Decimal
            Dim objPromoCtrl As New PromoController
            Dim objCCtrl As New CartController
            Dim blnIsCartAmountDiscount As Boolean = False
            Dim CartDiscount As Decimal
            Dim detInfoList As ArrayList

            Dim objCInfo As NB_Store_CartInfo = GetCurrentCart(PortalID)


            CartDiscount = objPromoCtrl.getCouponLevelDiscount(PortalID, UserInfo, CouponCode)

            If CartDiscount < 0 Then
                If Not AllowMultiDis Then
                    'clear the previous cart level item discount, so items with no discount get cleared
                    objPromoCtrl.ClearCartItemDiscount(PortalID)
                    'recalcualte after the clear down
                    CartDiscount = objPromoCtrl.getCouponLevelDiscount(PortalID, UserInfo, CouponCode)
                End If
            End If


            If AllowMultiDis Then
                objCInfo.CartDiscount += CartDiscount
            Else
                If CartDiscount < objCInfo.CartDiscount Then
                    objCInfo.CartDiscount = CartDiscount
                End If
            End If

            objCCtrl.UpdateObjCart(objCInfo)

            '-------------------------------------------------
            'aportion discount across products in cart.
            Dim TotalInCart As Decimal = 0

            CartDiscount = CartDiscount * -1

            If CartDiscount > 0 Then

                detInfoList = objPromoCtrl.getCouponCartItems(PortalID, CouponCode)

                For Each objInfo As NB_Store_CartItemsInfo In detInfoList
                    TotalInCart += ((objInfo.Quantity * objInfo.UnitCost) + (objInfo.Quantity * objInfo.Discount))
                Next

                AllocateAcrossCart(PortalID, detInfoList, CartDiscount, TotalInCart, AllowMultiDis)

            End If
            '-------------------------------------------------

            Return objCInfo.CartDiscount
        End Function

        Private Shared Sub AllocateAcrossCart(ByVal PortalID As Integer, ByVal detInfoList As ArrayList, ByVal CartDiscount As Decimal, ByVal TotalInCart As Decimal, ByVal AllowMultiDis As Boolean)
            'aportion discount across products in cart.
            If TotalInCart > 0 Then
                Dim detInfo As NB_Store_CartItemsInfo
                Dim ApRatio As Decimal
                Dim ApAmt As Decimal
                Dim HoldDiscount As Decimal = CartDiscount
                Dim objCCtrl As New CartController
                Dim detInfoList2 As ArrayList

                detInfoList2 = detInfoList.Clone ' get a copy of the array for rounding after processing.

                ApRatio = (CartDiscount / TotalInCart)
                For Each detInfo In detInfoList
                    ApAmt = (ApRatio * (detInfo.UnitCost + detInfo.Discount))

                    If Not AllowMultiDis Then
                        If ((ApAmt * -1) < detInfo.Discount) Then
                            CartDiscount = CartDiscount + ((detInfo.Discount * -1) * detInfo.Quantity)
                            detInfo.Discount = (ApAmt * -1)
                            CartDiscount = CartDiscount - (ApAmt * detInfo.Quantity)
                        End If
                    Else
                        HoldDiscount = HoldDiscount - RoundToStoreCurrency(PortalID, (detInfo.Discount * detInfo.Quantity))
                        If CartDiscount < ApAmt Then
                            detInfo.Discount = detInfo.Discount - CartDiscount
                            CartDiscount = 0
                        Else
                            detInfo.Discount = detInfo.Discount - ApAmt
                            CartDiscount = CartDiscount - (ApAmt * detInfo.Quantity)
                        End If
                    End If

                    objCCtrl.UpdateObjCartItem(detInfo)
                Next

                If CartDiscount > 0 And AllowMultiDis Then
                    If detInfoList.Count > 0 Then
                        detInfo = CType(detInfoList.Item(0), NB_Store_CartItemsInfo)
                        detInfo.Discount = detInfo.Discount - CartDiscount
                        objCCtrl.UpdateObjCartItem(detInfo)
                    End If
                End If

                'Make adjustment for rounding errors based on currency.culture.
                Dim lp As Integer
                Dim CountDiscount As Decimal = 0

                lp = 0
                For Each detInfo In detInfoList2
                    lp += 1
                    CountDiscount = CountDiscount + RoundToStoreCurrency(PortalID, (detInfo.Discount * detInfo.Quantity))
                    If lp = detInfoList.Count Then
                        If (CountDiscount + HoldDiscount) <> 0 Then
                            detInfo.Discount = detInfo.Discount + (((HoldDiscount + CountDiscount) / detInfo.Quantity) * -1)
                            objCCtrl.UpdateObjCartItem(detInfo)
                        End If
                    End If
                Next


            End If
        End Sub

#End Region




    End Class




End Namespace
