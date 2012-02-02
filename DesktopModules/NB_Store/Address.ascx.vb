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

    Public MustInherit Class Address
        Inherits Framework.UserControlBase

#Region "Private Members"
        Private _CountryCode As String
        Private _NoValidate As Boolean = False
        Private _AddressDataInfo As NB_Store_AddressInfo

        Private _AddressDescription As String

        Private _AddressName As String
        Private _Address1 As String
        Private _Address2 As String
        Private _City As String
        Private _Region As String
        Private _PostalCode As String
        Private _Phone1 As String
        Private _Phone2 As String
        Private _TemplateName As String = ""
#End Region

#Region "Public Properties"


        Public WriteOnly Property CountryCode() As String
            Set(ByVal Value As String)
                _CountryCode = Value
            End Set
        End Property

        Public WriteOnly Property TemplateName() As String
            Set(ByVal Value As String)
                _TemplateName = Value
            End Set
        End Property

        Public Property NoValidate() As Boolean
            Get
                Return _NoValidate
            End Get
            Set(ByVal Value As Boolean)
                _NoValidate = Value
            End Set
        End Property

        Public Property AddressDataInfo() As NB_Store_AddressInfo
            Get
                Return GetAddressDataInfo()
            End Get
            Set(ByVal Value As NB_Store_AddressInfo)
                _AddressDataInfo = Value
                _CountryCode = _AddressDataInfo.CountryCode
            End Set
        End Property

#End Region

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If _TemplateName = "" Then _TemplateName = "checkoutaddress.template"
            Dim strData As String = GetStoreSettingText(PortalSettings.PortalId, _TemplateName, GetCurrentCulture)

            If strData <> "" Then
                dlAddress.ItemTemplate = New GenXMLTemplate(Server.HtmlDecode(strData))
            Else
                dlAddress.Visible = False
            End If

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not _AddressDataInfo Is Nothing Then
                Dim aryList As New ArrayList
                aryList.Add(_AddressDataInfo)

                dlAddress.DataSource = aryList
                dlAddress.DataBind()
            End If

            If Not Page.IsPostBack Then
                If dlAddress.Controls.Count >= 1 Then
                    Dim cbo As DropDownList = dlAddress.Controls(0).FindControl("cboCountry")
                    If Not cbo Is Nothing Then
                        populateCountryList(PortalSettings.PortalId, cbo, _CountryCode)
                    End If

                    If _NoValidate Then
                        For Each ctrl In dlAddress.Controls(0).Controls
                            If TypeOf ctrl Is RequiredFieldValidator Then
                                DirectCast(ctrl, RequiredFieldValidator).Enabled = False
                            End If
                            If TypeOf ctrl Is RegularExpressionValidator Then
                                DirectCast(ctrl, RegularExpressionValidator).Enabled = False
                            End If
                        Next
                    End If
                End If

            End If

        End Sub

        Private Function GetAddressDataInfo() As NB_Store_AddressInfo
            Dim objInfo As New NB_Store_AddressInfo

            objInfo = CType(populateGenObject(dlAddress, objInfo), NB_Store_AddressInfo)
            
            Return objInfo
        End Function



    End Class

End Namespace
