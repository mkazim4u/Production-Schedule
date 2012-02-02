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



Imports System.Runtime.Remoting
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports System.Xml

Namespace NEvoWeb.Modules.NB_Store

    Public Class GatewayWrapper

        Private _objGateTable As New Hashtable
        Private ProviderList As String()

        Public Sub New()
        End Sub

        Public Sub BuildGatewayArray(ByVal PortalID As Integer)
            'create provder for all selected gateways

            If _objGateTable.Count = 0 Then

                Dim ProvKey As String()
                Dim ProviderClassList As String

                ProviderClassList = GetStoreSetting(PortalID, "gateway.provider", "XX")

                If GetStoreSettingBoolean(PortalID, "debug.mode") Then
                    UpdateLog("ProviderClassList = " & ProviderClassList)
                End If

                ProviderList = Split(ProviderClassList, ";")

                For lp As Integer = 0 To ProviderList.GetUpperBound(0)

                    If GetStoreSettingBoolean(PortalID, "debug.mode") Then
                        UpdateLog("ProviderList" & lp.ToString & " = " & ProviderList(lp))
                    End If

                    If ProviderList(lp) <> "" Then
                        ProvKey = Split(ProviderList(lp), ",")
                        If GetStoreSettingBoolean(PortalID, "debug.mode") Then
                            UpdateLog("ProvKey = " & ProvKey(0))
                        End If
                        _objGateTable.Add(ProvKey(0), GatewayInterface.Instance(ProvKey(0), ProvKey(1)))
                    End If
                Next

            End If

        End Sub


        Public Function GetButtonHtml(ByVal PortalID As Integer, ByVal OrderID As Integer, ByVal Lang As String, ByVal GatewayRef As String) As String
            Dim Gateway As GatewayInterface
            Dim hTable As Hashtable
            BuildGatewayArray(PortalID)
            hTable = GetAvailableGatewaysTable(PortalID)
            Gateway = CType(_objGateTable(CType(hTable.Item(GatewayRef), NB_Store_GatewayInfo).assembly), GatewayInterface)
            Return Gateway.GetButtonHtml(PortalID, OrderID, Lang)
        End Function


        Public Function GetButtonHtml(ByVal PortalID As Integer, ByVal OrderID As Integer, ByVal Lang As String) As String
            Dim strRtn As String = ""
            Dim Gateway As GatewayInterface
            Dim HtmlSep As String = System.Web.HttpUtility.HtmlDecode(GetStoreSettingText(PortalID, "gatewaysep.template", GetCurrentCulture))
            Dim ProvKey As String()

            BuildGatewayArray(PortalID)

            For lp As Integer = 0 To ProviderList.GetUpperBound(0)
                If ProviderList(lp) <> "" Then
                    ProvKey = Split(ProviderList(lp), ",")
                    Gateway = CType(_objGateTable(ProvKey(0)), GatewayInterface)
                    strRtn &= Gateway.GetButtonHtml(PortalID, OrderID, Lang)
                    strRtn &= HtmlSep
                End If
            Next

            If strRtn.Length > HtmlSep.Length Then
                strRtn = strRtn.Substring(0, (strRtn.Length - HtmlSep.Length))
            End If

            Return strRtn
        End Function


        Public Sub AutoResponse(ByVal PortalID As Integer, ByVal Request As System.Web.HttpRequest)
            Dim Gateway As GatewayInterface
            Dim objCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo
            Dim ordID As String

            BuildGatewayArray(PortalID)

            ordID = Request.QueryString("ordID")

            If IsNumeric(ordID) Then
                objOInfo = objCtrl.GetOrder(ordID)
                If Not objOInfo Is Nothing Then
                    ' use GatewayProvider field to tempoary store gateway needed
                    Gateway = CType(_objGateTable(objOInfo.GatewayProvider), GatewayInterface)
                    Gateway.AutoResponse(PortalID, Request)
                End If
            Else
                If _objGateTable.Count > 1 Then
                    UpdateLog("TO SUPPORT MULTIPLE GATEWAYS USE, '.../ordID/[ORDERID]/...' IN THE AUTORESPONSE URL")
                Else
                    Dim keys As ICollection = _objGateTable.Keys
                    Dim keysArray(_objGateTable.Count - 1) As String
                    keys.CopyTo(keysArray, 0)
                    Gateway = CType(_objGateTable.Item(keysArray(0).ToString), GatewayInterface)
                    Gateway.AutoResponse(PortalID, Request)
                End If
            End If
        End Sub

        Public Function GetCompletedHtml(ByVal PortalID As Integer, ByVal UserID As Integer, ByVal Request As System.Web.HttpRequest, ByVal OrderID As Integer) As String
            Dim strRtn As String = "No Order found. OrderID=" & OrderID.ToString
            Dim Gateway As GatewayInterface
            Dim objCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo

            BuildGatewayArray(PortalID)

            objOInfo = objCtrl.GetOrder(OrderID)
            If Not objOInfo Is Nothing Then
                ' use GatewayProvider field to tempoary store gateway needed
                Gateway = CType(_objGateTable(objOInfo.GatewayProvider), GatewayInterface)
                strRtn = Gateway.GetCompletedHtml(PortalID, UserID, Request)
            End If

            Return strRtn
        End Function

        Public Function GetBankClick(ByVal PortalID As Integer, ByVal Request As System.Web.HttpRequest) As Boolean
            Dim blnRtn As Boolean = False
            Dim Gateway As GatewayInterface
            Dim ProvKey As String()

            BuildGatewayArray(PortalID)

            For lp As Integer = 0 To ProviderList.GetUpperBound(0)
                If ProviderList(lp) <> "" Then
                    ProvKey = Split(ProviderList(lp), ",")
                    Gateway = CType(_objGateTable(ProvKey(0)), GatewayInterface)
                    If Gateway.GetBankClick(PortalID, Request) Then
                        'update order with provider key
                        Dim OrdID As Integer = CurrentCart.GetCurrentCart(PortalID).OrderID
                        Dim objCtrl As New OrderController
                        Dim objOInfo As NB_Store_OrdersInfo
                        objOInfo = objCtrl.GetOrder(OrdID)
                        If Not objOInfo Is Nothing Then
                            objOInfo.GatewayProvider = ProvKey(0) ' use this field to tempoary store gateway needed
                            objCtrl.UpdateObjOrder(objOInfo)
                            blnRtn = True
                            Exit For
                        End If
                    End If
                End If
            Next

            Return blnRtn

        End Function


        Public Sub SetBankRemotePost(ByVal PortalID As Integer, ByVal OrderID As Integer, ByVal Lang As String, ByVal Request As System.Web.HttpRequest)
            Dim Gateway As GatewayInterface
            Dim objCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo

            BuildGatewayArray(PortalID)

            objOInfo = objCtrl.GetOrder(OrderID)
            If Not objOInfo Is Nothing Then
                ' use GatewayProvider field to tempoary store gateway needed
                Gateway = CType(_objGateTable(objOInfo.GatewayProvider), GatewayInterface)
                Gateway.SetBankRemotePost(PortalID, OrderID, Lang, Request)
            End If

        End Sub
    End Class

End Namespace
