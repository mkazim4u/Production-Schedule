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

Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Data
Imports System.Xml

Imports DotNetNuke
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Class AdminDashboard
        Inherits BaseAdminModule

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                Dim MsgText As String = getLangCache(PortalId, "dashboardmsgtext", GetCurrentCulture)
                Dim debugMode As Boolean = GetStoreSettingBoolean(PortalId, "debug.mode", "XX")

                If MsgText = "" Or debugMode Then
                    MsgText = GetMsgText(PortalId, "dashboard.text", "")
                    PlaceHolder1.Controls.Add(New LiteralControl(System.Web.HttpUtility.HtmlDecode(MsgText)))
                    setLangCache(PortalId, "dashboardmsgtext", GetCurrentCulture, MsgText, 120)
                Else
                    PlaceHolder1.Controls.Add(New LiteralControl(System.Web.HttpUtility.HtmlDecode(MsgText)))
                End If

            Catch exc As Exception        'Module failed to load
                'Maybe an error with SQl report, so continue...
            End Try
        End Sub



#End Region


    End Class

End Namespace
