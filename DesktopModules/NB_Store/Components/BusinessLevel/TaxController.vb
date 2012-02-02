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

    Public Class TaxController

#Region "NB_Store_TaxRates Public Methods"

        Public Sub DeleteTaxRate(ByVal ItemID As Integer)
            DataProvider.Instance().DeleteNB_Store_TaxRates(ItemID)
        End Sub

        Public Function GetTaxRate(ByVal PortalID As Integer, ByVal ItemID As Integer) As NB_Store_TaxRatesInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_TaxRates(PortalID, ItemID), GetType(NB_Store_TaxRatesInfo)), NB_Store_TaxRatesInfo)
        End Function

        Public Function GetTaxRateByObjID(ByVal PortalID As Integer, ByVal ObjectID As Integer, ByVal TaxType As String) As NB_Store_TaxRatesInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_TaxRatesByObjID(PortalID, ObjectID, TaxType), GetType(NB_Store_TaxRatesInfo)), NB_Store_TaxRatesInfo)
        End Function
        Public Function GetTaxRatesList(ByVal PortalID As Integer, ByVal TaxType As String) As ArrayList
            Return GetTaxRatesList(PortalID, TaxType, "", "")
        End Function

        Public Function GetTaxRatesList(ByVal PortalID As Integer, ByVal TaxType As String, ByVal Lang As String, ByVal Filter As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_TaxRatess(PortalID, TaxType, Lang, Filter), GetType(NB_Store_TaxRatesInfo))
        End Function

        Public Sub UpdateObjTaxRate(ByVal objInfo As NB_Store_TaxRatesInfo)
            DataProvider.Instance().UpdateNB_Store_TaxRates(objInfo.PortalID, objInfo.ItemID, objInfo.ObjectID, objInfo.TaxPercent, objInfo.TaxDesc, objInfo.TaxType, objInfo.Disable)
        End Sub

#End Region

    End Class

    Public Class TaxCalcController

#Region "Private Members"
        Private _TaxDefault As NB_Store_TaxRatesInfo
        Private _objTaxInfo As NB_Store_TaxRatesInfo
        Private _TaxOption As String
        Private _objTaxCtrl As TaxController
#End Region

#Region "Constructors"
        Public Sub New(ByVal PortalID As Integer)
            _objTaxCtrl = New TaxController
            _TaxDefault = getDefaultTaxValue(PortalID, "DTX")
            If _TaxDefault Is Nothing Then
                _TaxDefault = New NB_Store_TaxRatesInfo
                _TaxDefault.Disable = False
                _TaxDefault.ItemID = -1
                _TaxDefault.ObjectID = -1
                _TaxDefault.TaxDesc = "Tax Default"
                _TaxDefault.TaxPercent = 0
                _TaxDefault.TaxType = "DTX"
            End If
            _objTaxInfo = getDefaultTaxValue(PortalID, "OTX")
            If _objTaxInfo Is Nothing Then
                _TaxOption = "1"
            Else
                'change to int and them string to get correct format to test
                _TaxOption = CInt(_objTaxInfo.TaxPercent).ToString
            End If
        End Sub
#End Region

#Region "calculation functions"

        Public Function getTaxOption() As String
            Return _TaxOption
        End Function


        Public Function getCartTaxDetails(ByVal PortalID As Integer, ByVal ShipAmount As Decimal, ByVal MerchantCC As String, ByVal VATNumber As String) As CartTaxInfo
            Dim taxInfo As New CartTaxInfo
            Dim detInfo As NB_Store_CartItemsInfo
            Dim orderCtrl As OrderController = New OrderController
            Dim detInfoList As ArrayList
            Dim blnPayVAT As Boolean = checkPayVAT(MerchantCC, VATNumber, PortalID, CurrentCart.GetCurrentCart(PortalID).CountryCode)

            Dim taxProdInfo As ProductTaxInfo
            'get detail of order
            detInfoList = CurrentCart.GetCurrentCartItems(PortalID)
            taxInfo.CartID = CurrentCart.GetCurrentCart(PortalID).CartID
            taxInfo.TaxOption = _TaxOption
            taxInfo.TaxAmount = "0"
            taxInfo.TotalGROSS = "0"
            taxInfo.TotalNET = "0"
            taxInfo.ShipTax = "0"

            If _TaxOption <> "1" Then
                For Each detInfo In detInfoList
                    taxProdInfo = getProductTaxDetails(PortalID, detInfo)
                    If Not taxProdInfo Is Nothing Then
                        If blnPayVAT Then
                            taxInfo.TaxAmount = (CDec(taxInfo.TaxAmount) + CDec(taxProdInfo.TaxAmount)).ToString
                            taxInfo.TotalGROSS = (CDec(taxInfo.TotalGROSS) + CDec(taxProdInfo.TotalGROSS)).ToString
                            taxInfo.TotalNET = (CDec(taxInfo.TotalNET) + CDec(taxProdInfo.TotalNET)).ToString
                        Else
                            If _TaxOption = "2" Then
                                'remove any tax (included in unit price)
                                taxInfo.TaxAmount = (CDec(taxInfo.TaxAmount) + (CDec(taxProdInfo.TaxAmount) * -1)).ToString
                                taxInfo.TotalGROSS = (CDec(taxInfo.TotalGROSS) + CDec(taxProdInfo.TotalGROSS)).ToString
                                taxInfo.TotalNET = (CDec(taxInfo.TotalNET) + CDec(taxProdInfo.TotalNET)).ToString
                            End If
                        End If
                    End If

                Next
                If blnPayVAT Then
                    taxInfo.ShipTax = getShipTaxAmount(PortalID, ShipAmount).ToString
                Else
                    If _TaxOption = "2" Then
                        'remove any tax (included in unit price)
                        taxInfo.ShipTax = (getShipTaxAmount(PortalID, ShipAmount) * -1).ToString
                    Else
                        taxInfo.ShipTax = "0"
                    End If
                End If
            End If
            Return taxInfo

        End Function

        Private Function checkPayVAT(ByVal MerchantCC As String, ByVal VATNumber As String, ByVal PortalID As Integer, ByVal CountryCode As String) As Boolean
            'check if country exempt from tax
            Dim Exemptlist As String = GetStoreSetting(PortalID, "taxexempt.list", GetCurrentCulture)
            Dim Applieslist As String = GetStoreSetting(PortalID, "taxapplies.list", GetCurrentCulture)

            If Exemptlist <> "" Then
                If InStr(Exemptlist.ToLower, CountryCode.ToLower) > 0 Then
                    Return False
                End If
            End If

            If Applieslist <> "" Then
                If Not InStr(Applieslist.ToLower, CountryCode.ToLower) > 0 Then
                    Return False
                End If
            End If

            If VATNumber = "" Or IsNumeric(VATNumber) Or (Len(VATNumber) < 3) Then
                Return True
            Else
                If VATNumber.Substring(0, 2).ToUpper = MerchantCC Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

        Public Function getShipTaxAmount(ByVal PortalID As Integer, ByVal ShipAmount As Decimal) As Decimal
            Dim taxInfo As New ProductTaxInfo
            Dim objInfo As NB_Store_TaxRatesInfo

            objInfo = getDefaultTaxValue(PortalID, "STX")
            If Not objInfo Is Nothing Then
                Select Case _TaxOption
                    Case "1"
                        Return 0
                    Case "2" ' gross
                        Return ShipAmount - (ShipAmount / (100 + CDec(objInfo.TaxPercent))) * 100
                    Case "3" ' net
                        Return (ShipAmount * (1 + (CDec(objInfo.TaxPercent) / 100))) - ShipAmount
                End Select
            Else
                Return 0
            End If
        End Function

        Public Function getProductTaxDetails(ByVal PortalID As Integer, ByVal objCartDet As NB_Store_CartItemsInfo) As ProductTaxInfo
            Dim taxInfo As New ProductTaxInfo
            Dim objInfo As NB_Store_TaxRatesInfo
            Dim TaxUsed As NB_Store_TaxRatesInfo

            objInfo = _objTaxCtrl.GetTaxRateByObjID(PortalID, objCartDet.ModelID, "PRD")

            If objInfo Is Nothing Then
                TaxUsed = getCategoryTaxDetails(objCartDet.ModelID)
            Else
                If objInfo.Disable Then
                    TaxUsed = getCategoryTaxDetails(objCartDet.ModelID)
                Else
                    TaxUsed = objInfo
                End If
            End If

            Dim TotalUnitCost As Decimal
            TotalUnitCost = CDec((objCartDet.UnitCost + objCartDet.Discount) * objCartDet.Quantity)

            taxInfo.CartID = CurrentCart.GetCurrentCart(PortalID).CartID
            taxInfo.ModelID = objCartDet.ModelID
            taxInfo.TaxPercent = TaxUsed.TaxPercent
            taxInfo.TaxAmount = getTaxAmount(TotalUnitCost, CDec(TaxUsed.TaxPercent)).ToString
            taxInfo.TotalNET = getTaxNET(TotalUnitCost, CDec(TaxUsed.TaxPercent)).ToString
            taxInfo.TotalGROSS = getTaxGROSS(TotalUnitCost, CDec(TaxUsed.TaxPercent)).ToString

            Return taxInfo
        End Function

        Private Function getCategoryTaxDetails(ByVal ModelID As Integer) As NB_Store_TaxRatesInfo
            Dim objProdCtrl As New ProductController
            Dim objProduct As NB_Store_ProductsInfo
            Dim objModel As NB_Store_ModelInfo
            Dim objInfo As NB_Store_TaxRatesInfo

            objModel = objProdCtrl.GetModel(ModelID, GetCurrentCulture)
            If Not objModel Is Nothing Then
                objProduct = objProdCtrl.GetProduct(objModel.ProductID, GetCurrentCulture)
                If Not objProduct Is Nothing Then
                    objInfo = _objTaxCtrl.GetTaxRateByObjID(objProduct.PortalID, objProduct.TaxCategoryID, "CAT")
                    If Not objInfo Is Nothing Then
                        If objInfo.Disable Then
                            Return _TaxDefault
                        Else
                            Return objInfo
                        End If
                    End If
                End If
            End If
            Return _TaxDefault
        End Function

        Private Function getDefaultTaxValue(ByVal PortalID As Integer, ByVal TaxType As String) As NB_Store_TaxRatesInfo
            Dim list As ArrayList
            list = _objTaxCtrl.GetTaxRatesList(PortalID, TaxType, "", "")
            If list Is Nothing Then
                Return Nothing
            Else
                If list.Count = 0 Then
                    Return Nothing
                Else
                    Return CType(list.Item(0), NB_Store_TaxRatesInfo)
                End If
            End If

        End Function

        Private Function getTaxNET(ByVal CartProductCost As Decimal, ByVal TaxPercent As Decimal) As Decimal
            If CartProductCost <= 0 Then Return 0
            Select Case _TaxOption
                Case "1"
                    Return 0
                Case "2" 'gross
                    Return (CartProductCost / (100 + TaxPercent)) * 100
                Case "3" 'net
                    Return CartProductCost
            End Select
        End Function

        Private Function getTaxGROSS(ByVal CartProductCost As Decimal, ByVal TaxPercent As Decimal) As Decimal
            If CartProductCost <= 0 Then Return 0
            Select Case _TaxOption
                Case "1"
                    Return 0
                Case "2" 'gross
                    Return CartProductCost
                Case "3" 'net
                    Return (CartProductCost * (1 + (TaxPercent / 100)))
            End Select
        End Function

        Private Function getTaxAmount(ByVal CartProductCost As Decimal, ByVal TaxPercent As Decimal) As Decimal
            If CartProductCost <= 0 Then Return 0
            Select Case _TaxOption
                Case "1"
                    Return 0
                Case "2" ' gross
                    Return CartProductCost - (CartProductCost / (100 + TaxPercent)) * 100
                Case "3" ' net
                    Return (CartProductCost * (1 + (TaxPercent / 100))) - CartProductCost
            End Select
        End Function
#End Region


    End Class

End Namespace
