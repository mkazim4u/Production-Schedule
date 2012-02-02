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

Namespace NEvoWeb.Modules.NB_Store

    Public MustInherit Class ChequeInterface

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As ChequeInterface = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            Dim handle As ObjectHandle
            Dim objInfo As NB_Store_SettingsInfo
            Dim objSCtrl As New SettingsController
            Dim Prov As String()

            objInfo = objSCtrl.GetSetting(CType(HttpContext.Current.Items("PortalSettings"), DotNetNuke.Entities.Portals.PortalSettings).PortalId, "encapsulated.provider", "XX")
            Prov = Split(objInfo.SettingValue, ",")
            handle = Activator.CreateInstance(Prov(0), Prov(1))
            objProvider = DirectCast(handle.Unwrap, ChequeInterface)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As ChequeInterface
            Return objProvider
        End Function

#End Region

        Public MustOverride Sub CompleteOrder(ByVal PortalID As Integer, ByVal OrderID As Integer, ByVal Lang As String, ByVal UpdateStock As Boolean)

    End Class

End Namespace
