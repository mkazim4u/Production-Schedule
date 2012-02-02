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

    Partial Public Class MiniCart
        Inherits BaseModule

        Private ORID As Integer = -1
        Private _CartList As NEvoWeb.Modules.NB_Store.CartList

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Dim objSCtrl As New NB_Store.SettingsController
                Dim objInfo As NB_Store_SettingsTextInfo
                Dim MsgText As String = ""
                Dim templID As String = ""
                Dim CTotals As CartTotals = CurrentCart.GetCalulatedTotals(PortalId, GetDefaultShipMethod(PortalId))
                Dim MiniPosition As Integer

                MiniPosition = 0
                If IsNumeric(CType(Settings("ddlMiniPosition"), String)) Then
                    MiniPosition = CType(Settings("ddlMiniPosition"), Integer)
                End If


                If Not CTotals Is Nothing Then

                    If CTotals.Qty = 0 Then
                        If CType(Settings("ddlEmptyTemplate"), String) = "" Then
                            templID = "minicartempty.template"
                        Else
                            templID = CType(Settings("ddlEmptyTemplate"), String)
                        End If
                        cartlist1.Visible = False
                    Else
                        If CType(Settings("ddlFullTemplate"), String) = "" Then
                            templID = "minicart.template"
                        Else
                            templID = CType(Settings("ddlFullTemplate"), String)
                        End If

                        If CBool(Settings("chkShowFullCart")) Then
                            SetUpCartList()
                        Else
                            cartlist1.Visible = False
                        End If

                    End If


                    objInfo = objSCtrl.GetSettingsText(PortalId, templID, GetCurrentCulture)
                    If Not objInfo Is Nothing Then
                        If objInfo.SettingText <> "" Then
                            MsgText = objInfo.SettingText

                            'get order details and change tokens
                            Dim objTR As New TokenStoreReplace(CTotals, GetCurrentCulture, PortalId)
                            MsgText = objTR.DoTokenReplace(MsgText)
                        End If
                    End If

                End If

                If MiniPosition = 0 Then
                    PlaceHolder1.Controls.Add(New LiteralControl(Server.HtmlDecode(MsgText)))
                End If
                If MiniPosition = 1 Then
                    PlaceHolder2.Controls.Add(New LiteralControl(Server.HtmlDecode(MsgText)))
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub SetUpCartList()
            Dim objTaxCalc As New TaxCalcController(PortalId)
            Dim ShipMethodID As Integer = CurrentCart.GetCurrentCart(PortalId).ShipMethodID

            If ShipMethodID <= 0 Then
                ShipMethodID = GetDefaultShipMethod(PortalId)
            End If

            cartlist1.CartID = CurrentCart.GetCurrentCart(PortalId).CartID
            cartlist1.PortalID = PortalId
            cartlist1.ShipMethodID = ShipMethodID
            cartlist1.TaxOption = objTaxCalc.getTaxOption
            cartlist1.ResourceFile = LocalResourceFile
            cartlist1.ShowDiscountCol = CBool(Settings("chkShowDiscountCol"))
            cartlist1.ShippingHidden = True
            cartlist1.NoUpdates = Not CBool(Settings("chkAllowEdit"))
            cartlist1.HideTotals = CBool(Settings("chkHideTotal"))
            cartlist1.HideTotals = Settings("chkHideTotal")

            If CBool(Settings("chkHideHeader")) Then
                cartlist1.HideHeader = True
            End If

            If CBool(Settings("chkHideDescriptionCol")) Then
                cartlist1.HideColumn = 1
            End If
            If CBool(Settings("chkHidePriceCol")) Then
                cartlist1.HideColumn = 2
            End If
            If CBool(Settings("chkHideQtyCol")) Then
                cartlist1.HideColumn = 3
            End If
            If Not CBool(Settings("chkShowDiscountCol")) Then
                cartlist1.HideColumn = 4
            End If
            If CBool(Settings("chkHideRemoveCol")) Then
                cartlist1.HideColumn = 5
            End If
            If CBool(Settings("chkHideTotalCol")) Then
                cartlist1.HideColumn = 6
            End If

        End Sub

        Private Sub cartlist1_RecalculateCart() Handles cartlist1.RecalculateCart
            Response.Redirect(NavigateURL(TabId, "", getUrlCookieInfo(PortalId, "")))
        End Sub
    End Class

End Namespace
