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

    Partial Public Class RemotePost
        Inherits BaseModule

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim objCInfo As NB_Store_CartInfo = CurrentCart.GetCurrentCart(PortalId)
            If objCInfo.BankTransID <> objCInfo.OrderID Then
                objCInfo.BankTransID = objCInfo.OrderID
                Dim SIPShtml As String = ""
                SIPShtml = CurrentCart.GetCurrentCart(PortalId).BankHtmlRedirect
                If SIPShtml = "" Then
                    SIPShtml = Session("BankHtmlRedirect")
                    Session("BankHtmlRedirect") = ""
                End If
                System.Web.HttpContext.Current.Response.Clear()
                System.Web.HttpContext.Current.Response.Write(SIPShtml)
                System.Web.HttpContext.Current.Response.End()
            Else
                DisplayMsgText(PortalId, plhMsg, "paymentinprocess.text", "Payment already being processed!!")
            End If
        End Sub

    End Class

End Namespace
