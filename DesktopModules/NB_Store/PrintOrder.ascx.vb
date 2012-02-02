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


    Partial Public Class PrintOrder
        Inherits BaseModule

        Private ORID As Integer = -1

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Dim objSCtrl As New NB_Store.SettingsController
                Dim objOCtrl As New OrderController
                Dim objInfo As NB_Store_SettingsTextInfo
                Dim objOInfo As NB_Store_OrdersInfo = Nothing
                Dim MsgText As String = ""

                If Request.QueryString("ORID") <> "" Then
                    If IsNumeric(Request.QueryString("ORID").ToString) Then
                        ORID = CInt(Request.QueryString("ORID"))
                        If Request.QueryString("inv") <> "" Then
                            objInfo = objSCtrl.GetSettingsText(PortalId, "receiptprint.text", GetCurrentCulture)
                        Else
                            objInfo = objSCtrl.GetSettingsText(PortalId, "orderprint.text", GetCurrentCulture)
                        End If
                        If Not objInfo Is Nothing Then
                            If objInfo.SettingText <> "" Then
                                MsgText = objInfo.SettingText
                                'get order details and change tokens
                                objOInfo = objOCtrl.GetOrder(ORID)
                                If Not objOInfo Is Nothing Then
                                    Dim objTR As New TokenStoreReplace(objOInfo, GetCurrentCulture)
                                    MsgText = objTR.DoTokenReplace(MsgText)
                                End If
                            End If
                        End If

                        'check user security
                        If Not IsManager(PortalId, UserInfo) Then
                            If UserInfo.UserID <> objOInfo.UserID Then
                                MsgText = ""
                            End If
                        End If


                        PlaceHolder1.Controls.Add(New LiteralControl(Server.HtmlDecode(MsgText)))
                    End If
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

    End Class

End Namespace
