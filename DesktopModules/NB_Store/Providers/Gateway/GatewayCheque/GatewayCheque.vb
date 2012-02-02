' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2008 SARL NevoWeb.  www.nevoweb.com. All rights are reserved.
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
Imports System.Web
Imports System.Net
Imports System.IO
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports System.Collections.Specialized

Namespace NEvoWeb.Modules.NB_Store.Gateway

    Public Class GatewayCheque
        Inherits ChequeInterface

        Public Overrides Sub CompleteOrder(ByVal PortalID As Integer, ByVal OrderID As Integer, ByVal Lang As String, ByVal UpdateStock As Boolean)
            Dim ordID As Integer = CurrentCart.GetCurrentCart(PortalID).OrderID
            Dim ordNumber As String = ""
            Dim objOCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo

            objOInfo = objOCtrl.GetOrder(ordID)

            Dim UsrID As Integer = objOInfo.UserID
            If UsrID = -1 Then UsrID = 0
            ordNumber = Format(PortalID, "00") & "-" & UsrID.ToString("0000#") & "-" & objOInfo.OrderID.ToString("0000#") & "-" & objOInfo.OrderDate.ToString("yyyyMMdd")

            objOInfo.OrderNumber = ordNumber
            objOInfo.PayType = "CHQ"
            objOInfo.OrderStatusID = 80
            objOInfo.OrderIsPlaced = True

            If UpdateStock Then
                Dim objPCtrl As New ProductController
                'update stock tranction in progress
                objPCtrl.UpdateModelQtyTrans(objOInfo.OrderID)
                'remove qty in trans
                objPCtrl.UpdateStockLevel(objOInfo.OrderID)
            End If

            objOCtrl.UpdateObjOrder(objOInfo)

            Dim strEmailTemplate As String = GetStoreSetting(PortalID, "clientchqemail.name", Lang)
            If strEmailTemplate = "" Then strEmailTemplate = "chqpayment.email"
            SendEmailToClient(PortalID, GetClientEmail(PortalID, objOInfo), objOInfo.OrderNumber, objOInfo, strEmailTemplate, Lang)

            strEmailTemplate = GetStoreSetting(PortalID, "managerchqemail.name", Lang)
            If strEmailTemplate = "" Then strEmailTemplate = "chqpayment.email"
            SendEmailToManager(PortalID, objOInfo.OrderNumber, objOInfo, strEmailTemplate)


        End Sub

    End Class

End Namespace
