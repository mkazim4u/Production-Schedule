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


    Public Class OrderController


#Region "NB_Store_OrderDetails Public Methods"

        Public Sub DeleteOrderDetail(ByVal OrderDetailID As Integer)
            DataProvider.Instance().DeleteNB_Store_OrderDetails(OrderDetailID)
        End Sub

        Public Function GetOrderDetail(ByVal OrderDetailID As Integer) As NB_Store_OrderDetailsInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_OrderDetails(OrderDetailID), GetType(NB_Store_OrderDetailsInfo)), NB_Store_OrderDetailsInfo)
        End Function

        Public Function GetOrderDetailList(ByVal OrderID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_OrderDetailss(OrderID), GetType(NB_Store_OrderDetailsInfo))
        End Function

        Public Sub UpdateObjOrderDetail(ByVal objInfo As NB_Store_OrderDetailsInfo)
            DataProvider.Instance().UpdateNB_Store_OrderDetails(objInfo.OrderDetailID, objInfo.OrderID, objInfo.Quantity, objInfo.UnitCost, objInfo.ModelID, objInfo.OptCode, objInfo.ItemDesc, objInfo.Discount, objInfo.Tax, objInfo.ProductURL, objInfo.PurchaseCost, objInfo.CartXMLInfo)
        End Sub

#End Region

#Region "NB_Store_Orders Public Methods"

        Public Function CreateEmptyOrder(ByVal PortalID As String, ByVal UserID As Integer) As NB_Store_OrdersInfo
            Dim objOInfo As New NB_Store_OrdersInfo
            Dim objAInfo As New NB_Store_AddressInfo

            objOInfo.PortalID = PortalID
            objOInfo.OrderDate = Now
            objOInfo.OrderID = -1
            objOInfo.OrderStatusID = 10
            objOInfo.UserID = UserID
            
            objOInfo = UpdateObjOrder(objOInfo)

            objAInfo.UserID = -1
            objAInfo.AddressID = -1
            objAInfo.OrderID = objOInfo.OrderID
            objAInfo.CreatedDate = Now
            objAInfo.CountryCode = GetMerchantCountryCode(PortalID)
            objAInfo = UpdateObjOrderAddress(objAInfo)

            objOInfo.BillingAddressID = objAInfo.AddressID
            objOInfo.ShippingAddressID = objAInfo.AddressID

            objOInfo = UpdateObjOrder(objOInfo)

            Return objOInfo

        End Function



        Public Sub DeleteOrder(ByVal OrderID As Integer)
            Dim objInfo As NB_Store_OrdersInfo
            objInfo = GetOrder(OrderID)
            If Not objInfo Is Nothing Then
                If Not objInfo.OrderIsPlaced Then ' never delete if the order has been placed.
                    'delete adresses attached to order
                    DeleteOrderAddress(objInfo.BillingAddressID)
                    If objInfo.BillingAddressID <> objInfo.ShippingAddressID Then
                        DeleteOrderAddress(objInfo.ShippingAddressID)
                    End If
                    'delete old order
                    DataProvider.Instance().DeleteNB_Store_Orders(OrderID)
                End If
            End If
        End Sub

        Public Sub PurgeArchivedOrders(ByVal PortalID As Integer)
            Dim aryList As ArrayList
            Dim objOInfo As NB_Store_OrdersInfo
            aryList = GetOrdersExportList(PortalID, 75)
            For Each objOInfo In aryList
                'set the payment flag to false so the delete will work
                objOInfo.OrderIsPlaced = False
                UpdateObjOrder(objOInfo)
                'now delete the order.
                DeleteOrder(objOInfo.OrderID)
            Next
        End Sub

        Public Function GetOrder(ByVal OrderID As Integer) As NB_Store_OrdersInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Orders(OrderID), GetType(NB_Store_OrdersInfo)), NB_Store_OrdersInfo)
        End Function

        Public Function GetOrderByGUID(ByVal OrderGUID As String) As NB_Store_OrdersInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_OrdersByGUID(OrderGUID), GetType(NB_Store_OrdersInfo)), NB_Store_OrdersInfo)
        End Function

        Public Function GetOrdersExportList(ByVal PortalID As Integer, ByVal StatusID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetOrdersExportList(PortalID, StatusID), GetType(NB_Store_OrdersInfo))
        End Function

        Public Function GetOrderList(ByVal PortalID As Integer, ByVal UsrID As Integer, ByVal FromDate As Date, ByVal ToDate As Date, ByVal StatusID As Integer, ByVal Filter As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_Orderss(PortalID, UsrID, FromDate, ToDate, StatusID, Filter), GetType(NB_Store_OrdersInfo))
        End Function

        Public Function GetOrderList(ByVal PortalID As Integer, ByVal UsrID As Integer) As ArrayList
            Return GetOrderList(PortalID, UsrID, DateAdd(DateInterval.Year, -2, Today), Today, -1, "")
        End Function

        Public Function GetOrderList(ByVal PortalID As Integer, ByVal UsrID As Integer, ByVal StatusID As Integer) As ArrayList
            Return GetOrderList(PortalID, UsrID, DateAdd(DateInterval.Year, -2, Today), Today, StatusID, "")
        End Function

        Public Function UpdateObjOrder(ByVal objInfo As NB_Store_OrdersInfo) As NB_Store_OrdersInfo
            Return CType(CBO.FillObject(DataProvider.Instance().UpdateNB_Store_Orders(objInfo.OrderID, objInfo.UserID, objInfo.PortalID, objInfo.OrderNumber, objInfo.OrderDate, objInfo.ShipDate, objInfo.ShippingAddressID, objInfo.BillingAddressID, objInfo.AppliedTax, objInfo.ShippingCost, objInfo.OrderIsPlaced, objInfo.OrderStatusID, objInfo.PayType, objInfo.CalculatedTax, objInfo.NoteMsg, objInfo.VATNumber, objInfo.Discount, objInfo.PromoCode, objInfo.Total, objInfo.Email, objInfo.BankAuthCode, objInfo.ShipMethodID, objInfo.TrackingCode, objInfo.Stg2FormXML, objInfo.Stg3FormXML, objInfo.AlreadyPaid, objInfo.OrderGUID, objInfo.ElapsedDate, objInfo.GatewayProvider, objInfo.CartXMLInfo), GetType(NB_Store_OrdersInfo)), NB_Store_OrdersInfo)
        End Function

#End Region

#Region "NB_Store_OrderStatus Public Methods"

        Public Sub DeleteOrderStatus(ByVal OrderStatusID As Integer, ByVal Lang As String)
            DataProvider.Instance().DeleteNB_Store_OrderStatus(OrderStatusID, Lang)
        End Sub

        Public Function GetOrderStatus(ByVal OrderStatusID As Integer, ByVal Lang As String) As NB_Store_OrderStatusInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_OrderStatus(OrderStatusID, Lang), GetType(NB_Store_OrderStatusInfo)), NB_Store_OrderStatusInfo)
        End Function

        Public Function GetOrderStatusList(ByVal Lang As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_OrderStatuss(Lang), GetType(NB_Store_OrderStatusInfo))
        End Function

        Public Sub UpdateObjOrderStatus(ByVal objInfo As NB_Store_OrderStatusInfo)
            DataProvider.Instance().UpdateNB_Store_OrderStatus(objInfo.OrderStatusID, objInfo.Lang, objInfo.OrderStatusText, objInfo.ListOrder)
        End Sub

#End Region

#Region "NB_Store_Address Public Methods"

        Public Sub DeleteOrderAddress(ByVal AddressID As Integer)
            DataProvider.Instance().DeleteNB_Store_Address(AddressID)
        End Sub

        Public Function GetOrderAddress(ByVal AddressID As Integer) As NB_Store_AddressInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Address(AddressID), GetType(NB_Store_AddressInfo)), NB_Store_AddressInfo)
        End Function

        Public Function UpdateObjOrderAddress(ByVal objInfo As NB_Store_AddressInfo) As NB_Store_AddressInfo
            Return CType(CBO.FillObject(DataProvider.Instance().UpdateNB_Store_Address(objInfo.AddressID, objInfo.PortalID, objInfo.UserID, objInfo.AddressDescription, objInfo.AddressName, objInfo.Address1, objInfo.Address2, objInfo.City, objInfo.RegionCode, objInfo.CountryCode, objInfo.PostalCode, objInfo.Phone1, objInfo.Phone2, objInfo.PrimaryAddress, objInfo.CreatedByUser, objInfo.CreatedDate, objInfo.OrderID, objInfo.CompanyName, objInfo.Extra1, objInfo.Extra2, objInfo.Extra3, objInfo.Extra4), GetType(NB_Store_AddressInfo)), NB_Store_AddressInfo)
        End Function




#End Region




    End Class

End Namespace
