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


Imports System
Imports System.Data
Imports System.Xml
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class BaseModule
        Inherits Entities.Modules.PortalModuleBase

        Public StoreInstallPath As String

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            StoreInstallPath = ControlPath
            AddTextToHead(GetStoreSettingText(PortalId, "pageheader.template", GetCurrentCulture, False, True), Page)
        End Sub

        Private Shared Sub AddTextToHead(ByVal TextToAdd As String, ByVal Page As System.Web.UI.Page)
            If TextToAdd <> "" Then
                Dim oTestForHeader As Control = Page.FindControl("NBStoreHeader")
                If oTestForHeader Is Nothing Then
                    Dim oMeta As New HtmlMeta
                    oMeta.ID = "NBStoreHeader"
                    oMeta.Attributes("content") = "nbstore"
                    Dim oHead As Control = Page.FindControl("Head")
                    Dim li As New Literal
                    li.Text = TextToAdd
                    If Not oHead Is Nothing Then
                        oHead.Controls.Add(oMeta)
                        oHead.Controls.Add(li)
                    End If
                End If
            End If
        End Sub
    End Class

End Namespace

