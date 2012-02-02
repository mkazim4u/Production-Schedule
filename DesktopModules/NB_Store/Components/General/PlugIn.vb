' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2010 SARL NevoWeb.  www.nevoweb.com. BSD License.
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

Imports System.Xml
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class PlugIn


        Public Shared Sub InstallStorePlugin(ByVal PortalID As Integer, ByVal PluginName As String, ByVal RootMenuId As String, ByVal text As String, ByVal image As String, ByVal ctl As String, ByVal ctlsrc As String, ByVal param As String, ByVal roles As String)
            Dim strXML As String = ""

            strXML = "<root>"
            strXML &= "<tabs>"
            strXML &= "<tab id=""" & RootMenuId & """>"
            strXML &= "<subtab text=""" & text & """ image=""" & image & """  ctl=""" & ctl & """ ctlsrc=""" & ctlsrc & """ param=""" & param & """ roles=""" & roles & """/>"
            strXML &= "</tab>"
            strXML &= "</tabs>"
            strXML &= "</root>"

            InstallStorePlugin(PortalID, PluginName, strXML)

        End Sub

        Public Shared Sub InstallStorePlugin(ByVal PortalID As Integer, ByVal PluginName As String, ByVal PluginCommand As String)
            'plugin  the control to nbstore backoffice.
            If Not PluginName.ToLower.EndsWith(".plugin") Then
                PluginName &= ".plugin"
            End If
            If GetStoreSetting(PortalID, PluginName, "None") = "" Then
                SetStoreSetting(PortalID, PluginName, PluginCommand, "None", True)
            End If

        End Sub

    End Class

End Namespace
