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

Namespace NEvoWeb.Modules.NB_Store

    Public MustInherit Class GatewayInterface

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As GatewayInterface = Nothing
        Private Shared objArray As New Hashtable

        ' constructor
        Shared Sub New()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider(ByVal PortalID As Integer, ByVal ProviderAssembly As String, ByVal ProviderClass As String)
            Dim handle As ObjectHandle

            If ProviderAssembly = "" Then
                Dim Prov As String()
                Prov = Split(GetStoreSetting(PortalID, "gateway.provider", "XX"), ",")
                ProviderAssembly = Prov(0)
                ProviderClass = Prov(1)
            End If

            handle = Activator.CreateInstance(ProviderAssembly, ProviderClass)
            objProvider = DirectCast(handle.Unwrap, GatewayInterface)
            objArray.Add(PortalID.ToString & ProviderAssembly, objProvider)

        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As GatewayInterface
            Return Instance("", "")
        End Function

        Public Shared Shadows Function Instance(ByVal ProviderAssembly As String, ByVal ProviderClass As String) As GatewayInterface
            Dim PortalID As Integer
            PortalID = CType(HttpContext.Current.Items("PortalSettings"), DotNetNuke.Entities.Portals.PortalSettings).PortalId
            If Not objArray.ContainsKey(PortalID.ToString & ProviderAssembly) Then
                CreateProvider(PortalID, ProviderAssembly, ProviderClass)
            End If
            Return CType(objArray(PortalID.ToString & ProviderAssembly), GatewayInterface)
        End Function

#End Region

        Public MustOverride Function GetButtonHtml(ByVal PortalID As Integer, ByVal OrderID As Integer, ByVal Lang As String) As String

        Public MustOverride Sub AutoResponse(ByVal PortalID As Integer, ByVal Request As System.Web.HttpRequest)

        Public MustOverride Function GetCompletedHtml(ByVal PortalID As Integer, ByVal UserID As Integer, ByVal Request As System.Web.HttpRequest) As String

        Public MustOverride Function GetBankClick(ByVal PortalID As Integer, ByVal Request As System.Web.HttpRequest) As Boolean

        Public MustOverride Sub SetBankRemotePost(ByVal PortalID As Integer, ByVal OrderID As Integer, ByVal Lang As String, ByVal Request As System.Web.HttpRequest)

    End Class

End Namespace
