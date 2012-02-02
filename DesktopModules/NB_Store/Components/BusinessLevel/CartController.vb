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

    Public Class CartController


#Region "NB_Store_Cart Public Methods"

        Public Sub DeleteCart(ByVal CartID As String)
            DataProvider.Instance().DeleteNB_Store_Cart(CartID)
        End Sub

        Public Function GetCart(ByVal CartID As String) As NB_Store_CartInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Cart(CartID), GetType(NB_Store_CartInfo)), NB_Store_CartInfo)
        End Function

        Public Sub UpdateObjCart(ByVal objInfo As NB_Store_CartInfo)
            DataProvider.Instance().UpdateNB_Store_Cart(objInfo.CartID, objInfo.PortalID, objInfo.UserID, objInfo.DateCreated, objInfo.OrderID, objInfo.VATNumber, objInfo.PromoCode, objInfo.CountryCode, objInfo.ShipType, objInfo.BankTransID, objInfo.BankHtmlRedirect, objInfo.ShipMethodID, objInfo.CartDiscount, objInfo.XMLInfo)
        End Sub

        Public Function GetCartModelQty(ByVal PortalID As Integer, ByVal ModelID As Integer, Optional ByVal CartID As String = "") As Integer
            'If cartID blank then get search all portal carts
            Return DataProvider.Instance().GetCartModelQty(PortalID, ModelID, CartID)
        End Function

        Public Sub BuildFromSeedOrder(ByVal PortalID As Integer, ByVal OrderGUID As String)
            Dim objOCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo

            objOInfo = objOCtrl.GetOrderByGUID(OrderGUID)
            If Not objOInfo Is Nothing Then
                CurrentCart.CreateCartFromOrder(PortalID, objOInfo.OrderID, False)
            End If

        End Sub

#End Region

#Region "NB_Store_CartItems Public Methods"

        Public Sub DeleteCartItem(ByVal ItemID As Integer)
            DataProvider.Instance().DeleteNB_Store_CartItems(ItemID)
        End Sub

        Public Sub DeleteOldCarts(ByVal PortalID As Integer, ByVal CartMins As Integer, ByVal OrderMins As Integer)
            DataProvider.Instance().DeleteOldCarts(PortalID, CartMins, OrderMins)
        End Sub

        Public Function GetCartItem(ByVal ItemID As Integer) As NB_Store_CartItemsInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_CartItems(ItemID), GetType(NB_Store_CartItemsInfo)), NB_Store_CartItemsInfo)
        End Function
        Public Function GetCartItemByOptCode(ByVal CartID As String, ByVal OptCode As String) As NB_Store_CartItemsInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_CartItemsByOptCode(CartID, OptCode), GetType(NB_Store_CartItemsInfo)), NB_Store_CartItemsInfo)
        End Function

        Public Function GetCartItemList(ByVal CartID As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_CartItemss(CartID), GetType(NB_Store_CartItemsInfo))
        End Function

        Public Function GetCartList(ByVal CartID As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_CartList(CartID), GetType(NB_Store_CartListInfo))
        End Function

        Public Function UpdateObjCartItem(ByVal objInfo As NB_Store_CartItemsInfo) As Integer
            Return CType(DataProvider.Instance().UpdateNB_Store_CartItems(objInfo.ItemID, objInfo.CartID, objInfo.Quantity, objInfo.DateCreated, objInfo.UnitCost, objInfo.ModelID, objInfo.OptCode, objInfo.ItemDesc, objInfo.Discount, objInfo.Tax, objInfo.ProductURL, objInfo.XMLInfo), Integer)
        End Function

#End Region


    End Class

End Namespace
