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

Namespace NEvoWeb.Modules.NB_Store

    Public Class PromoController

#Region "NB_Store_Promo Public Methods"

        Public Sub DeletePromo(ByVal PromoID As Integer)
            DataProvider.Instance().DeleteNB_Store_Promo(PromoID)
        End Sub

        Public Function GetPromo(ByVal PromoID As Integer) As NB_Store_PromoInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Promo(PromoID), GetType(NB_Store_PromoInfo)), NB_Store_PromoInfo)
        End Function

        Public Function GetPromoList(ByVal PortalID As Integer, ByVal PromoType As String, ByVal SearchText As String, Optional ByVal GetActiveOnly As Boolean = False) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_PromoList(PortalID, PromoType, SearchText, GetActiveOnly), GetType(NB_Store_PromoInfo))
        End Function

        Public Sub UpdateObjPromo(ByVal objInfo As NB_Store_PromoInfo)
            DataProvider.Instance().UpdateNB_Store_Promo(objInfo.PromoID, objInfo.PortalID, objInfo.ObjectID, objInfo.PromoName, objInfo.PromoType, objInfo.Range1, objInfo.Range2, objInfo.RangeStartDate, objInfo.RangeEndDate, objInfo.PromoAmount, objInfo.PromoPercent, objInfo.Disabled, objInfo.PromoCode, objInfo.PromoGroup, objInfo.PromoUser, objInfo.QtyRange1, objInfo.QtyRange2, objInfo.PromoEmail, objInfo.XMLData, objInfo.MaxUsagePerUser, objInfo.MaxUsage)
        End Sub

        Public Function GetPromoCodeUsage(ByVal PortalID As Integer, ByVal PromoCode As String) As Integer
            Return CType(DataProvider.Instance().GetNB_Store_PromoCodeUsage(PortalID, PromoCode), Integer)
        End Function


#End Region

#Region "NB_Store_SaleRates Public Methods"

        Public Sub DeleteSaleRates(ByVal ItemID As Integer)
            DataProvider.Instance().DeleteNB_Store_SaleRates(ItemID)
        End Sub

        Public Sub ClearSaleRates(ByVal PortalID As Integer, ByVal ModelID As Integer)
            DataProvider.Instance().ClearNB_Store_SaleRates(PortalID, ModelID)
        End Sub

        Public Function GetSaleRates(ByVal ItemID As Integer) As NB_Store_SaleRatesInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_SaleRates(ItemID), GetType(NB_Store_SaleRatesInfo)), NB_Store_SaleRatesInfo)
        End Function

        Public Function GetSaleRatesByCacheKey(ByVal CacheKey As String) As NB_Store_SaleRatesInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_SaleRatesByCacheKey(CacheKey), GetType(NB_Store_SaleRatesInfo)), NB_Store_SaleRatesInfo)
        End Function

        Public Function GetSaleRatesList(ByVal PortalID As Integer, ByVal ModelID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_SaleRatesList(PortalID, ModelID), GetType(NB_Store_SaleRatesInfo))
        End Function

        Public Sub UpdateObjSaleRates(ByVal objInfo As NB_Store_SaleRatesInfo)
            DataProvider.Instance().UpdateNB_Store_SaleRates(objInfo.ItemID, objInfo.CacheKey, objInfo.PortalID, objInfo.RoleName, objInfo.CategoryID, objInfo.ModelID, objInfo.SalePrice)
        End Sub


#End Region

#Region "get Sale Price Methods"

        Public Function GetSalePrice(ByVal objMInfo As NB_Store_ModelInfo, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo) As Decimal
            Dim objPCtrl As New ProductController
            Dim aryList As ArrayList
            Dim rtnPrice As Double = -1
            Dim SalePrice As Double = 0

            aryList = objPCtrl.GetCategoriesAssigned(objMInfo.ProductID)

            For Each objCInfo As NB_Store_ProductCategoryInfo In aryList
                SalePrice = GetSalePrice(objMInfo, UserInfo, objCInfo.CategoryID)
                If (SalePrice >= 0) And (SalePrice < rtnPrice) Then
                    rtnPrice = SalePrice
                Else
                    If rtnPrice = -1 Then
                        rtnPrice = SalePrice
                    End If
                End If
            Next

            Return rtnPrice
        End Function

        Public Function GetSalePrice(ByVal objMInfo As NB_Store_ModelInfo, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo, ByVal CategoryId As Integer) As Decimal
            Dim CacheKey As String
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim PromoRoles As String()
            Dim rtnPrice As Double = -1
            Dim strList As String = ""

            objSInfo = objSCtrl.GetSetting(objMInfo.PortalID, "promo.roles", "None")
            If Not objSInfo Is Nothing Then
                strList = "," & objSInfo.SettingValue
                PromoRoles = strList.Split(","c)
            Else
                PromoRoles = Split("", ",")
            End If

            'check if the sales cache exist, if not create it.
            If DataCache.GetCache("SalePrice" & objMInfo.PortalID.ToString) Is Nothing Then
                createSalePriceCache(objMInfo.PortalID, -1)
            End If

            For lp As Integer = 0 To PromoRoles.GetUpperBound(0)

                If UserInfo.IsInRole(PromoRoles(lp)) Or PromoRoles(lp) = "" Then

                    CacheKey = "SalePrice" & objMInfo.PortalID & "*" & PromoRoles(lp) & "*" & CategoryId & "*" & objMInfo.ModelID
                    If Not DataCache.GetCache(CacheKey) Is Nothing Then
                        If IsNumeric(DataCache.GetCache(CacheKey)) Then
                            If CDbl(DataCache.GetCache(CacheKey)) < rtnPrice Then
                                rtnPrice = CDbl(DataCache.GetCache(CacheKey))
                            Else
                                If rtnPrice = -1 Then
                                    rtnPrice = CDbl(DataCache.GetCache(CacheKey))
                                End If
                            End If
                        End If
                    End If

                    'check against all category sale price cache
                    If CategoryId > -1 Then
                        CacheKey = "SalePrice" & objMInfo.PortalID & "*" & PromoRoles(lp) & "*-1*" & objMInfo.ModelID
                        If Not DataCache.GetCache(CacheKey) Is Nothing Then
                            If IsNumeric(DataCache.GetCache(CacheKey)) Then
                                If CDbl(DataCache.GetCache(CacheKey)) < rtnPrice Then
                                    rtnPrice = CDbl(DataCache.GetCache(CacheKey))
                                Else
                                    If rtnPrice = -1 Then
                                        rtnPrice = CDbl(DataCache.GetCache(CacheKey))
                                    End If
                                End If
                            End If
                        End If
                    End If

                End If

            Next

            Return rtnPrice
        End Function

#End Region

#Region "get Cart Level Methods"

        Public Function getCartLevelDiscount(ByVal PortalID As Integer, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo, ByVal AllowMultiDis As Boolean) As Decimal
            Dim rtnDiscount As Decimal = 0
            Dim objInfo As NB_Store_CartItemsInfo
            Dim aryList As ArrayList
            Dim tQty As Integer = 0
            Dim tPrice As Decimal = 0
            Dim ApplyDiscount As Boolean = False
            Dim strTestList As String()

            'get current cart totals
            aryList = CurrentCart.GetCurrentCartItems(PortalID)
            For Each objInfo In aryList
                If AllowMultiDis Then
                    tPrice += ((objInfo.Quantity * objInfo.UnitCost) + (objInfo.Quantity * objInfo.Discount))
                Else
                    tPrice += ((objInfo.Quantity * objInfo.UnitCost))
                End If
                tQty += objInfo.Quantity
            Next

            'loop through the active cart level discount
            aryList = GetPromoList(PortalID, "CAR", "", True)
            For Each objPromoInfo As NB_Store_PromoInfo In aryList
                ApplyDiscount = True
                'check qty range
                If (tQty < objPromoInfo.QtyRange1 Or tQty > objPromoInfo.QtyRange2) And _
                    Not (objPromoInfo.QtyRange1 = 0 And objPromoInfo.QtyRange2 = 0) Then
                    ApplyDiscount = False
                End If
                'check price range
                If tPrice < objPromoInfo.Range1 Or tPrice > objPromoInfo.Range2 And _
                Not (objPromoInfo.Range1 = 0 And objPromoInfo.Range2 = 0) Then
                    ApplyDiscount = False
                End If
                'check user roles
                If Not UserInfo.IsInRole(objPromoInfo.PromoGroup) And objPromoInfo.PromoGroup <> "" Then
                    ApplyDiscount = False
                End If
                'check user email
                If objPromoInfo.PromoEmail <> "" Then
                    ApplyDiscount = False
                    strTestList = objPromoInfo.PromoEmail.Split(","c)
                    For lp As Integer = 0 To strTestList.GetUpperBound(0)
                        If strTestList(lp) = UserInfo.Email Then
                            ApplyDiscount = True
                        End If
                    Next
                End If
                'check username
                If objPromoInfo.PromoUser <> "" Then
                    ApplyDiscount = False
                    strTestList = objPromoInfo.PromoUser.Split(","c)
                    For lp As Integer = 0 To strTestList.GetUpperBound(0)
                        If strTestList(lp) = UserInfo.Username Then
                            ApplyDiscount = True
                        End If
                    Next
                End If
                If ApplyDiscount = True Then
                    Dim SaleP As Decimal = 0
                    If objPromoInfo.PromoAmount > 0 Then
                        SaleP = tPrice - objPromoInfo.PromoAmount
                    Else
                        SaleP = tPrice
                    End If
                    If objPromoInfo.PromoPercent <> 0 Then
                        SaleP = (SaleP / 100) * (100 - objPromoInfo.PromoPercent)
                    End If
                    If AllowMultiDis Then
                        rtnDiscount += (SaleP - tPrice)
                    Else
                        If (SaleP - tPrice) < rtnDiscount Then
                            rtnDiscount = (SaleP - tPrice)
                        End If
                    End If

                End If
            Next

            If rtnDiscount > 0 Then rtnDiscount = 0

            Return rtnDiscount
        End Function

        Public Sub ClearCartItemDiscount(ByVal PortalID As Integer)
            Dim aryList As ArrayList
            Dim objCCtrl As New CartController

            aryList = CurrentCart.GetCurrentCartItems(PortalID)
            For Each objInfo As NB_Store_CartItemsInfo In aryList
                objInfo.Discount = 0
                objCCtrl.UpdateObjCartItem(objInfo)
            Next
        End Sub

#End Region

#Region "get Coupon Level Methods"

        Public Function getCouponLevelDiscount(ByVal PortalID As Integer, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo, ByVal CouponCode As String) As Decimal
            Dim rtnDiscount As Decimal = 0
            Dim objInfo As NB_Store_CartItemsInfo
            Dim aryList As ArrayList
            Dim tQty As Integer = 0
            Dim tPrice As Decimal = 0
            Dim ApplyDiscount As Boolean = False
            Dim strTestList As String()
            Dim aryCList As ArrayList

            'get current cart totals
            ' only get the coupon cart items, so we calculate for seperate categories correctly.
            aryCList = getCouponCartItems(PortalID, CouponCode)
            For Each objInfo In aryCList
                tPrice += ((objInfo.Quantity * objInfo.UnitCost) + (objInfo.Quantity * objInfo.Discount))
                tQty += objInfo.Quantity
            Next

            'loop through the active cart level discount
            aryList = GetPromoList(PortalID, "COU", CouponCode, True)
            For Each objPromoInfo As NB_Store_PromoInfo In aryList
                ApplyDiscount = True
                'check qty range
                If (tQty < objPromoInfo.QtyRange1 Or tQty > objPromoInfo.QtyRange2) And _
                    Not (objPromoInfo.QtyRange1 = 0 And objPromoInfo.QtyRange2 = 0) Then
                    ApplyDiscount = False
                End If
                'check price range
                If tPrice < objPromoInfo.Range1 Or tPrice > objPromoInfo.Range2 And _
                Not (objPromoInfo.Range1 = 0 And objPromoInfo.Range2 = 0) Then
                    ApplyDiscount = False
                End If
                'check user roles
                If Not UserInfo.IsInRole(objPromoInfo.PromoGroup) And objPromoInfo.PromoGroup <> "" Then
                    ApplyDiscount = False
                End If
                'check user email
                If objPromoInfo.PromoEmail <> "" Then
                    ApplyDiscount = False
                    strTestList = objPromoInfo.PromoEmail.Split(","c)
                    For lp As Integer = 0 To strTestList.GetUpperBound(0)
                        If strTestList(lp) = UserInfo.Email Then
                            ApplyDiscount = True
                        End If
                    Next
                End If
                'check username
                If objPromoInfo.PromoUser <> "" Then
                    ApplyDiscount = False
                    strTestList = objPromoInfo.PromoUser.Split(","c)
                    For lp As Integer = 0 To strTestList.GetUpperBound(0)
                        If strTestList(lp) = UserInfo.Username Then
                            ApplyDiscount = True
                        End If
                    Next
                End If

                'check promocode code
                If objPromoInfo.PromoCode = CouponCode Then
                    ApplyDiscount = True
                Else
                    ApplyDiscount = False
                End If

                If (objPromoInfo.PromoCode <> "") AndAlso (objPromoInfo.PromoCode = CouponCode) AndAlso (objPromoInfo.MaxUsage > 0) Then
                    Dim intUsed As Integer = 0
                    intUsed = GetPromoCodeUsage(PortalID, CouponCode)
                    If intUsed >= objPromoInfo.MaxUsage Then 'code usage has been exceeded
                        ApplyDiscount = False
                    End If
                End If


                'check MaxUsagePerUser flag
                'added by Philipp Becker, dnnWerk
                If (objPromoInfo.PromoCode <> "") AndAlso (objPromoInfo.PromoCode = CouponCode) AndAlso (objPromoInfo.MaxUsagePerUser > 0) Then
                    Dim objOrderCtrl As New OrderController
                    'get all orders for current user
                    Dim aryOrders As ArrayList = objOrderCtrl.GetOrderList(PortalID, UserInfo.UserID)
                    Dim intUsed As Integer = 0
                    For Each objOrder As NB_Store_OrdersInfo In aryOrders
                        If objOrder.PromoCode = objPromoInfo.PromoCode And objOrder.OrderIsPlaced Then 'code has been used before
                            intUsed += 1
                        End If
                    Next
                    If intUsed >= objPromoInfo.MaxUsagePerUser Then 'code usage has been exceeded
                        ApplyDiscount = False
                    End If
                End If
                'end check MaxUsagePerUser flag

                If ApplyDiscount = True Then
                    Dim SaleP As Decimal = 0
                    If objPromoInfo.PromoAmount > 0 Then
                        SaleP = tPrice - objPromoInfo.PromoAmount
                    Else
                        SaleP = tPrice
                    End If
                    If objPromoInfo.PromoPercent <> 0 Then
                        SaleP = (SaleP / 100) * (100 - objPromoInfo.PromoPercent)
                    End If
                    rtnDiscount += (SaleP - tPrice)
                End If
            Next

            If rtnDiscount > 0 Then rtnDiscount = 0

            Return rtnDiscount
        End Function

        Public Function getCouponCartItems(ByVal PortalID As Integer, ByVal CouponCode As String) As ArrayList
            Return getCouponCartItems(PortalID, CouponCode, True)
        End Function


        Public Function getCouponCartItems(ByVal PortalID As Integer, ByVal CouponCode As String, ByVal IsInCategory As Boolean) As ArrayList
            Dim objInfo As NB_Store_CartItemsInfo
            Dim aryList As ArrayList
            Dim aryItemList As New ArrayList

            'loop through the active cart level discount
            aryList = GetPromoList(PortalID, "COU", CouponCode, True)
            For Each objPromoInfo As NB_Store_PromoInfo In aryList

                'check promocode code
                If objPromoInfo.PromoCode = CouponCode Then
                    Dim aryCList As ArrayList
                    Dim objPCtrl As New ProductController
                    Dim objMInfo As NB_Store_ModelInfo
                    aryCList = CurrentCart.GetCurrentCartItems(PortalID)
                    If objPromoInfo.ObjectID = -1 Then
                        ' no category seperation, so return all cart items
                        aryItemList = aryCList
                    Else
                        'only return cart items in category
                        For Each objInfo In aryCList
                            objMInfo = objPCtrl.GetModel(objInfo.ModelID, GetCurrentCulture)
                            If objPCtrl.IsInCategory(objMInfo.ProductID, objPromoInfo.ObjectID) = IsInCategory Then
                                aryItemList.Add(objInfo)
                            End If
                        Next
                    End If
                End If

            Next

            Return aryItemList
        End Function

#End Region

#Region "Calculation Methods"

        Public Sub createSalePriceTable(ByVal PortalID As Integer, Optional ByVal ModelID As Integer = -1)

            'loop through all models, recalc saleprice
            Dim aryList As ArrayList
            Dim aryMList As ArrayList
            Dim objCtrl As New ProductController
            Dim objSCtrl As New SettingsController
            Dim CacheKey As String = ""
            Dim PromoRoles As New Hashtable
            Dim strPromoRoles As String = ""
            Dim objMInfo As NB_Store_ModelInfo

            'clear salerate cache
            aryList = GetSaleRatesList(PortalID, ModelID)
            For Each objInfo As NB_Store_SaleRatesInfo In aryList
                DataCache.RemoveCache(objInfo.CacheKey)

            Next

            ClearSaleRates(PortalID, ModelID)

            If ModelID = -1 Then
                aryMList = objCtrl.GetModelStockList(PortalID, "", GetCurrentCulture, -1, 1, 99999, True)
            Else
                'only do model specified
                objMInfo = objCtrl.GetModel(ModelID, GetCurrentCulture)
                aryMList = New ArrayList
                aryMList.Add(objMInfo)
            End If

            For Each objMInfo In aryMList
                'clear any produc level cache, so sale price is picked up.
                ClearProductCache(PortalID, objMInfo.ProductID)
                'get active promos, i.e. in daterange and not disabled
                aryList = GetPromoList(objMInfo.PortalID, "STO", "", True)
                For Each objPromoInfo As NB_Store_PromoInfo In aryList
                    CacheKey = "SalePrice" & objPromoInfo.PortalID & "*" & objPromoInfo.PromoGroup & "*" & objPromoInfo.ObjectID & "*" & objMInfo.ModelID
                    Dim objInfo As New NB_Store_SaleRatesInfo
                    objInfo.CacheKey = CacheKey
                    objInfo.CategoryID = objPromoInfo.ObjectID
                    objInfo.ItemID = -1
                    objInfo.ModelID = objMInfo.ModelID
                    objInfo.PortalID = objMInfo.PortalID
                    objInfo.RoleName = objPromoInfo.PromoGroup
                    objInfo.SalePrice = calcSalePrice(objMInfo, objPromoInfo)

                    UpdateObjSaleRates(objInfo)

                    If objInfo.RoleName <> "" Then
                        If Not PromoRoles.ContainsValue(objInfo.RoleName) Then
                            PromoRoles.Add(objInfo.RoleName, objInfo.RoleName)
                        End If
                    End If
                Next
            Next

            createSalePriceCache(PortalID, ModelID)

        End Sub

        Public Sub createSalePriceCache(ByVal PortalID As Integer, ByVal ModelID As Integer)
            Dim aryList As ArrayList
            Dim CacheKey As String = ""

            aryList = GetSaleRatesList(PortalID, ModelID)
            For Each objInfo As NB_Store_SaleRatesInfo In aryList
                DataCache.SetCache(objInfo.CacheKey, objInfo.SalePrice, DateAdd(DateInterval.Day, 1, Now))
            Next

            'set cache flag, so we know it exists in cache
            If ModelID = -1 Then ' only if we build the whole table.
                DataCache.SetCache("SalePrice" & PortalID, "TRUE", DateAdd(DateInterval.Day, 1, Now))
            End If
        End Sub

        Public Function calcSalePrice(ByVal objMInfo As NB_Store_ModelInfo, ByVal objPromoInfo As NB_Store_PromoInfo) As Decimal
            Dim SaleP As Decimal = 0
            If objPromoInfo.PromoAmount > 0 Then
                SaleP = objMInfo.UnitCost - objPromoInfo.PromoAmount
            Else
                SaleP = objMInfo.UnitCost
            End If
            If objPromoInfo.PromoPercent <> 0 Then
                SaleP = (SaleP / 100) * (100 - objPromoInfo.PromoPercent)
            End If
            Return SaleP
        End Function

#End Region

    End Class


End Namespace
