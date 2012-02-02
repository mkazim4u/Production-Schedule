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

    Public Class ShipController


#Region "NB_Store_ShippingRates Public Methods"

        Public Sub DeleteShippingRate(ByVal ItemId As Integer)
            DataProvider.Instance().DeleteNB_Store_ShippingRates(ItemId)
        End Sub

        Public Function GetShippingRate(ByVal PortalID As Integer, ByVal ItemId As Integer) As NB_Store_ShippingRatesInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_ShippingRates(PortalID, ItemId), GetType(NB_Store_ShippingRatesInfo)), NB_Store_ShippingRatesInfo)
        End Function

        Public Function GetShippingRateByObjID(ByVal PortalID As Integer, ByVal ObjectId As Integer, ByVal ShipType As String, ByVal ShipMethodID As Integer) As NB_Store_ShippingRatesInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_ShippingRatesByObjID(PortalID, ObjectId, ShipType, ShipMethodID), GetType(NB_Store_ShippingRatesInfo)), NB_Store_ShippingRatesInfo)
        End Function

        Public Function GetShippingRateList(ByVal PortalID As Integer, ByVal ShipType As String, ByVal ShipMethodID As Integer) As ArrayList
            Return GetShippingRateList(PortalID, ShipType, "", "", -1, ShipMethodID)
        End Function

        Public Function GetShippingRateList(ByVal PortalID As Integer, ByVal ShipType As String, ByVal Lang As String, ByVal Filter As String, ByVal ShipMethodID As Integer) As ArrayList
            Return GetShippingRateList(PortalID, ShipType, Lang, Filter, -1, ShipMethodID)
        End Function

        Public Function GetShippingRateList(ByVal PortalID As Integer, ByVal ShipType As String, ByVal Lang As String, ByVal Filter As String, ByVal CategoryID As Integer, ByVal ShipMethodID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ShippingRatess(PortalID, ShipType, Lang, Filter, CategoryID, ShipMethodID), GetType(NB_Store_ShippingRatesInfo))
        End Function

        Public Function GetShippingRateListByShipMethodID(ByVal PortalID As Integer, ByVal ShipMethodID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ShippingRatesListByShipMethodID(PortalID, ShipMethodID), GetType(NB_Store_ShippingRatesInfo))
        End Function

        Public Sub UpdateObjShippingRate(ByVal objInfo As NB_Store_ShippingRatesInfo)
            DataProvider.Instance().UpdateNB_Store_ShippingRates(objInfo.PortalID, objInfo.ItemId, objInfo.Range1, objInfo.Range2, objInfo.ObjectId, objInfo.ShipCost, objInfo.ShipType, objInfo.Disable, objInfo.Description, objInfo.ProductWeight, objInfo.ProductHeight, objInfo.ProductLength, objInfo.ProductWidth, objInfo.ShipMethodID)
        End Sub

        Public Sub ClearAllShippingRates(ByVal PortalID As Integer, ByVal ShipMethodID As Integer)
            Dim aryList As ArrayList

            aryList = GetShippingRateListByShipMethodID(PortalID, ShipMethodID)
            For Each objSRInfo In aryList
                DeleteShippingRate(objSRInfo.ItemId)
            Next

        End Sub

        Public Sub CopyAllShippingRates(ByVal PortalID As Integer, ByVal ShipMethodID As Integer, ByVal NewShipMethodID As Integer)
            Dim aryList As ArrayList

            aryList = GetShippingRateListByShipMethodID(PortalID, ShipMethodID)
            For Each objSRInfo As NB_Store_ShippingRatesInfo In aryList
                objSRInfo.ShipMethodID = NewShipMethodID
                objSRInfo.ItemId = -1
                UpdateObjShippingRate(objSRInfo)
            Next

        End Sub



#End Region

#Region "NB_Store_ShippingMethod Public Methods"

        Public Sub DeleteShippingMethod(ByVal ShipMethodID As Integer)
            Dim objInfo As NB_Store_ShippingMethodInfo
            objInfo = GetShippingMethod(ShipMethodID)
            If Not objInfo Is Nothing Then
                ClearAllShippingRates(objInfo.PortalID, ShipMethodID)
                DataProvider.Instance().DeleteShippingMethod(ShipMethodID)
            End If
        End Sub

        Public Function GetShippingMethod(ByVal ShipMethodID As Integer) As NB_Store_ShippingMethodInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetShippingMethod(ShipMethodID), GetType(NB_Store_ShippingMethodInfo)), NB_Store_ShippingMethodInfo)
        End Function

        Public Function GetShippingMethodList(ByVal PortalID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetShippingMethodList(PortalID), GetType(NB_Store_ShippingMethodInfo))
        End Function

        Public Function GetShippingMethodEnabledList(ByVal PortalID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetShippingMethodEnabledList(PortalID), GetType(NB_Store_ShippingMethodInfo))
        End Function

        Public Function UpdateObjShippingMethod(ByVal objInfo As NB_Store_ShippingMethodInfo) As Integer
            Return DataProvider.Instance().UpdateShippingMethod(objInfo.ShipMethodID, objInfo.PortalID, objInfo.MethodName, objInfo.MethodDesc, objInfo.SortOrder, objInfo.TemplateName, objInfo.Disabled, objInfo.URLtracker)
        End Function

        Public Sub ClearAllShippingMethods(ByVal PortalID As Integer)
            Dim aryList As ArrayList
            aryList = GetShippingMethodList(PortalID)
            For Each objSMInfo As NB_Store_ShippingMethodInfo In aryList
                ClearAllShippingRates(PortalID, objSMInfo.ShipMethodID)
                DeleteShippingMethod(objSMInfo.ShipMethodID)
            Next
        End Sub

        Public Sub CopyShippingMethod(ByVal ShipMethodID As Integer)
            Dim objInfo As NB_Store_ShippingMethodInfo
            objInfo = GetShippingMethod(ShipMethodID)
            If Not objInfo Is Nothing Then
                Dim NewShipMethodID As Integer
                Dim OldShipMethodID As Integer
                OldShipMethodID = objInfo.ShipMethodID
                objInfo.ShipMethodID = -1
                objInfo.MethodName = objInfo.MethodName & " [Copy]"
                NewShipMethodID = UpdateObjShippingMethod(objInfo)
                CopyAllShippingRates(objInfo.PortalID, OldShipMethodID, NewShipMethodID)
            End If

        End Sub


#End Region

#Region "Calculation Method"

        Public Function getShippingCost(ByVal PortalID As Integer, ByVal CountryCode As String, ByVal ShipMethodID As Integer) As Decimal
            Dim rtnCost As Decimal = 0
            Dim gotCost As Decimal
            Dim orderCtrl As OrderController = New OrderController
            Dim detInfoList As ArrayList
            Dim weiCost As Decimal
            Dim qtyCost As Decimal
            Dim prcCost As Decimal
            Dim weiCart As Decimal
            Dim CartTotal As Decimal

            CartTotal = CurrentCart.GetCartItemTotal(PortalID)

            If getFreeShipping(PortalID, CartTotal, ShipMethodID) Then
                Return 0
            End If

            'get detail of order
            detInfoList = CurrentCart.GetCurrentCartItems(PortalID)

            'get cost for Weight, Quantity or Price
            getWeightShipping(PortalID, detInfoList, weiCost, weiCart, ShipMethodID)
            qtyCost = getQuantityShipping(PortalID, detInfoList, ShipMethodID)
            prcCost = getPriceShipping(PortalID, CartTotal, ShipMethodID)

            'take lowest cost between price and qty
            If (prcCost > 0) And ((prcCost <= qtyCost) Or (qtyCost < 0)) And ((prcCost <= weiCost) Or (weiCost <= 0)) Then
                rtnCost = prcCost
            End If
            If (qtyCost > 0) And ((qtyCost <= weiCost) Or (weiCost <= 0)) And ((qtyCost <= prcCost) Or (prcCost < 0)) Then
                rtnCost = qtyCost
            End If

            'take weight cost if more than price and qty
            If weiCost > rtnCost Then
                rtnCost = weiCost
            End If

            'Add extra for products
            gotCost = getProductExtraShipping(PortalID, detInfoList, -1)
            rtnCost += gotCost

            'Add extra for country
            gotCost = getCountryExtraShipping(PortalID, CountryCode, weiCart, ShipMethodID)
            If rtnCost > 0 Or gotCost = 0 Then
                rtnCost += gotCost
            Else
                rtnCost = gotCost
            End If

            If GetDefaultShipMethod(PortalID) = ShipMethodID Then
                'zero shipping is valid for downloads on default shipmethod
                If rtnCost < 0 Then rtnCost = -1
            Else
                'if not default shipping then hide the shipping cost
                'return -1 to hide this ship method 
                If rtnCost <= 0 Then rtnCost = -1
            End If

            Return rtnCost
        End Function


        Private Function getQuantityShipping(ByVal PortalID As Integer, ByVal orderDetailInfoList As ArrayList, ByVal ShipMethodID As Integer) As Decimal
            Dim detInfo As NB_Store_CartItemsInfo
            Dim Qty As Integer
            Dim list As ArrayList
            Dim objInfo As NB_Store_ShippingRatesInfo
            Dim rtnCost As Decimal = -1

            For Each detInfo In orderDetailInfoList
                Qty = Qty + detInfo.Quantity
            Next

            list = GetShippingRateList(PortalID, "QTY", ShipMethodID)
            For Each objInfo In list
                If Not objInfo.Disable Then
                    If (CDec(objInfo.Range1) <= Qty) And (CDec(objInfo.Range2) >= Qty) Then
                        If IsNumeric(objInfo.ShipCost) Then
                            rtnCost = CDec(objInfo.ShipCost)
                            Exit For
                        End If
                    End If
                End If
            Next
            Return rtnCost
        End Function

        Private Function getPriceShipping(ByVal PortalID As Integer, ByVal Prc As Decimal, ByVal ShipMethodID As Integer) As Decimal
            Dim list As ArrayList
            Dim objInfo As NB_Store_ShippingRatesInfo
            Dim rtnCost As Decimal = -1

            list = GetShippingRateList(PortalID, "PRC", ShipMethodID)
            For Each objInfo In list
                If Not objInfo.Disable Then
                    If (CDec(objInfo.Range1) <= Prc) And (CDec(objInfo.Range2) >= Prc) Then
                        If IsNumeric(objInfo.ShipCost) Then
                            rtnCost = CDec(objInfo.ShipCost)
                            Exit For
                        End If
                    End If
                End If
            Next
            Return rtnCost
        End Function

        Private Sub getWeightShipping(ByVal PortalID As Integer, ByVal orderDetailInfoList As ArrayList, ByRef weightCost As Decimal, ByRef CartWeight As Decimal, ByVal ShipMethodID As Integer)
            Dim detInfo As NB_Store_CartItemsInfo
            Dim Wei As Decimal
            Dim list As ArrayList
            Dim objInfo As NB_Store_ShippingRatesInfo
            Dim rtnCost As Decimal = 0
            Dim weiDefault As Decimal

            objInfo = getDefaultValue(PortalID, "DPO", -1)
            If objInfo Is Nothing Then
                weiDefault = 0
            Else
                If IsNumeric(objInfo.ProductWeight) Then
                    weiDefault = CDec(objInfo.ProductWeight)
                End If
            End If

            For Each detInfo In orderDetailInfoList
                objInfo = GetShippingRateByObjID(PortalID, detInfo.ModelID, "PRD", -1)
                If objInfo Is Nothing Then
                    Wei = Wei + weiDefault * detInfo.Quantity
                Else
                    If IsNumeric(objInfo.ProductWeight) Then
                        Wei = Wei + CDec(objInfo.ProductWeight) * detInfo.Quantity
                    End If
                End If
            Next

            list = GetShippingRateList(PortalID, "WEI", ShipMethodID)
            For Each objInfo In list
                If Not objInfo.Disable Then
                    If (CDec(objInfo.Range1) <= Wei) And (CDec(objInfo.Range2) >= Wei) Then
                        If IsNumeric(objInfo.ShipCost) Then
                            rtnCost = CDec(objInfo.ShipCost)
                            Exit For
                        End If
                    End If
                End If
            Next
            weightCost = rtnCost
            CartWeight = Wei

        End Sub

        Private Function getProductExtraShipping(ByVal PortalID As Integer, ByVal orderDetailInfoList As ArrayList, ByVal ShipMethodID As Integer) As Decimal
            Dim detInfo As NB_Store_CartItemsInfo
            Dim objInfo As NB_Store_ShippingRatesInfo
            Dim rtnCost As Decimal = 0
            Dim htShipRate As New Hashtable
            Dim blnCalcBoxes As Boolean = False

            'load hashtable and check to see if we have boxes to calcuate.
            For Each detInfo In orderDetailInfoList
                objInfo = GetShippingRateByObjID(PortalID, detInfo.ModelID, "PRD", ShipMethodID)
                If Not objInfo Is Nothing Then
                    htShipRate.Add(detInfo.ItemID, objInfo)
                    If objInfo.Range1 > 0 Then
                        blnCalcBoxes = True
                    End If
                End If
            Next


            If blnCalcBoxes Then
                'boxes % found, so calcuated as boxes.
                Dim htBoxTypes As New Hashtable

                'get box type, each different shipcost is counted as a new type of box.
                For Each detInfo In orderDetailInfoList
                    objInfo = htShipRate.Item(detInfo.ItemID)
                    If Not objInfo Is Nothing Then
                        If Not htBoxTypes.ContainsKey("Box" & objInfo.ShipCost.ToString) Then
                            htBoxTypes.Add("Box" & objInfo.ShipCost.ToString, objInfo.ShipCost)
                        End If
                    End If
                Next

                'calc amount for each box type
                Dim BoxCost As Decimal = 0
                Dim BoxPercent As Decimal
                For Each di As DictionaryEntry In htBoxTypes
                    BoxPercent = 0
                    For Each detInfo In orderDetailInfoList
                        objInfo = htShipRate.Item(detInfo.ItemID)
                        If Not objInfo Is Nothing Then
                            If di.Key = ("Box" & objInfo.ShipCost.ToString) Then
                                If objInfo.Range1 > 100 Or objInfo.Range1 <= 0 Then objInfo.Range1 = 100 ' only allow percentage
                                BoxPercent = BoxPercent + (detInfo.Quantity * objInfo.Range1)
                            End If
                        End If
                    Next
                    If BoxPercent > 0 Then
                        BoxCost = BoxCost + (Math.Ceiling((BoxPercent / 100)) * CType(di.Value, Decimal))
                    End If
                Next

                rtnCost = BoxCost

            Else
                For Each detInfo In orderDetailInfoList
                    objInfo = htShipRate.Item(detInfo.ItemID)
                    If Not objInfo Is Nothing Then
                        If IsNumeric(objInfo.ShipCost) Then
                            rtnCost = rtnCost + (CDec(objInfo.ShipCost) * detInfo.Quantity)
                        End If
                    End If
                Next
            End If



            Return rtnCost
        End Function

        Private Function getCountryExtraShipping(ByVal PortalID As Integer, ByVal Country As String, ByVal CartWeight As Decimal, ByVal ShipMethodID As Integer) As Decimal
            Dim list As ArrayList
            Dim objInfo As NB_Store_ShippingRatesInfo
            Dim rtnCost As Decimal = 0

            objInfo = getDefaultValue(PortalID, "DCO", ShipMethodID)
            If objInfo Is Nothing Then
                rtnCost = 0
            Else
                If IsNumeric(objInfo.ShipCost) Then
                    rtnCost = CDec(objInfo.ShipCost)
                End If
            End If

            'get the default range for any country.  use XX as country
            list = GetShippingRateList(PortalID, "COU", ShipMethodID)
            For Each objInfo In list
                If Not objInfo.Disable Then
                    If objInfo.Description.StartsWith("XX - ") Then
                        If ((CDec(objInfo.Range1) <= CartWeight) And (CDec(objInfo.Range2) >= CartWeight)) Or (CDec(objInfo.Range1) = 0 And CDec(objInfo.Range2) = 0) Then
                            If IsNumeric(objInfo.ShipCost) Then
                                rtnCost = CDec(objInfo.ShipCost)
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next


            list = GetShippingRateList(PortalID, "COU", ShipMethodID)
            For Each objInfo In list
                If Not objInfo.Disable Then
                    If objInfo.Description.StartsWith(Country & " - ") Then
                        If ((CDec(objInfo.Range1) <= CartWeight) And (CDec(objInfo.Range2) >= CartWeight)) Or (CDec(objInfo.Range1) = 0 And CDec(objInfo.Range2) = 0) Then
                            If IsNumeric(objInfo.ShipCost) Then
                                rtnCost = CDec(objInfo.ShipCost)
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next


            Return rtnCost
        End Function

        Private Function getFreeShipping(ByVal PortalID As Integer, ByVal CartTotal As Decimal, ByVal ShipMethodID As Integer) As Boolean
            Dim objInfo As NB_Store_ShippingRatesInfo

            objInfo = getDefaultValue(PortalID, "FRE", ShipMethodID)
            If Not objInfo Is Nothing Then
                If IsNumeric(objInfo.ShipCost) Then
                    If CartTotal > objInfo.ShipCost And objInfo.ShipCost >= 0 Then
                        Dim notFRElist As String = GetStoreSetting(PortalID, "shipfree.list", GetCurrentCulture)
                        If notFRElist = "" Then
                            Return True
                        Else
                            If InStr(notFRElist.ToLower, CurrentCart.GetCurrentCart(PortalID).CountryCode.ToLower) > 0 Then
                                Return True
                            End If
                        End If
                    End If
                End If
            End If
            Return False
        End Function

        Private Function getDefaultValue(ByVal PortalID As Integer, ByVal ShipType As String, ByVal ShipMethodID As Integer) As NB_Store_ShippingRatesInfo
            Dim list As ArrayList
            list = GetShippingRateList(PortalID, ShipType, ShipMethodID)
            If list Is Nothing Then
                Return Nothing
            Else
                If list.Count = 0 Then
                    Return Nothing
                Else
                    Return CType(list.Item(0), NB_Store_ShippingRatesInfo)
                End If
            End If

        End Function
#End Region

    End Class

End Namespace
