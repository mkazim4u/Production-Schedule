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



Imports System.Runtime.Remoting
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store


    Public MustInherit Class CalcDiscountInterface

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As CalcDiscountInterface = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            Dim handle As ObjectHandle
            Dim Prov As String()
            Dim ProviderName As String

            ProviderName = GetStoreSetting(CType(HttpContext.Current.Items("PortalSettings"), DotNetNuke.Entities.Portals.PortalSettings).PortalId, "calcdiscount.provider")
            If ProviderName <> "" Then
                Prov = Split(ProviderName, ",")
                handle = Activator.CreateInstance(Prov(0), Prov(1))
                objProvider = DirectCast(handle.Unwrap, CalcDiscountInterface)
            End If
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As CalcDiscountInterface
            Return objProvider
        End Function

#End Region

        Public MustOverride Function getCartTotals(ByVal PortalID As Integer, ByVal CartID As String, ByVal objCartTotals As CartTotals) As CartTotals

    End Class

End Namespace
