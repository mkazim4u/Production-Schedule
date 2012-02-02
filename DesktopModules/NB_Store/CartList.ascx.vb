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

    Public Class CartList
        Inherits Framework.UserControlBase

        Private _CartID As String = ""
        Private _PortalID As Integer
        Private _NoUpdates As Boolean = True
        Private cartTotal As Decimal = 0
        Private itemCount As Integer = 0
        Private _TaxOption As String = "1"
        Private _ResourceFile As String
        Private _ShipMethodID As Integer = -1
        Private _ShippingHidden As Boolean = False
        Private _ShowDiscountCol As Boolean = False
        Private _HideColumns As New Hashtable
        Private _HideTotals As Boolean = False
        Private _AllowPriceEdit As Boolean = False
        Private _HideHeader As Boolean = False

        Public Event CartIsEmpty()
        Public Event RecalculateCart()
        Public Event ValidateCart()

#Region "Public Properties"

        Public Property ResourceFile() As String
            Get
                Return _ResourceFile
            End Get
            Set(ByVal Value As String)
                _ResourceFile = Value
            End Set
        End Property

        Public Property CartID() As String
            Get
                Return _CartID
            End Get
            Set(ByVal Value As String)
                _CartID = Value
            End Set
        End Property

        Public Property PortalID() As String
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As String)
                _PortalID = Value
            End Set
        End Property

        Public Property NoUpdates() As Boolean
            Get
                Return _NoUpdates
            End Get
            Set(ByVal Value As Boolean)
                _NoUpdates = Value
            End Set
        End Property

        Public Property TaxOption() As String
            Get
                Return _TaxOption
            End Get
            Set(ByVal Value As String)
                _TaxOption = Value
            End Set
        End Property

        Public Property ShipMethodID() As String
            Get
                Return _ShipMethodID
            End Get
            Set(ByVal Value As String)
                _ShipMethodID = Value
            End Set
        End Property

        Public Property ShippingHidden() As Boolean
            Get
                Return _ShippingHidden
            End Get
            Set(ByVal Value As Boolean)
                _ShippingHidden = Value
            End Set
        End Property

        Public Property ShowDiscountCol() As Boolean
            Get
                Return _ShowDiscountCol
            End Get
            Set(ByVal Value As Boolean)
                _ShowDiscountCol = Value
            End Set
        End Property

        Public WriteOnly Property HideColumn() As Integer
            Set(ByVal Value As Integer)
                If Not _HideColumns.ContainsKey(Value) Then
                    _HideColumns.Add(Value, Value)
                End If
            End Set
        End Property

        Public Property HideTotals() As Boolean
            Get
                Return _HideTotals
            End Get
            Set(ByVal Value As Boolean)
                _HideTotals = Value
            End Set
        End Property

        Public Property AllowPriceEdit() As Boolean
            Get
                Return _AllowPriceEdit
            End Get
            Set(ByVal Value As Boolean)
                _AllowPriceEdit = Value
            End Set
        End Property

        Public Property HideHeader() As Boolean
            Get
                Return _HideHeader
            End Get
            Set(ByVal Value As Boolean)
                _HideHeader = Value
            End Set
        End Property

#End Region

#Region "Events"


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim Stage As String = "1"
            If Not (Request.QueryString("stg") Is Nothing) Then
                Stage = Request.QueryString("stg")
            End If

            If Not Page.IsPostBack And Stage <> "4" Then
                If _NoUpdates Then
                    cmdRecalculate.Visible = False
                End If

                Dim CTotals As CartTotals = CurrentCart.GetCalulatedTotals(PortalID)
                If Not CTotals Is Nothing Then
                    If CTotals.OrderTotal = 0 Then
                        _HideTotals = True
                        HideColumn = 2
                        HideColumn = 6
                        HideColumn = 4
                    End If
                End If

                If Not Page.IsPostBack Then
                    If _ShipMethodID < 0 Then
                        _ShipMethodID = GetDefaultShipMethod(PortalID)
                    End If
                    PopulateCartList()
                End If

                For Each di As DictionaryEntry In _HideColumns
                    If di.Value < dgCartList.Columns.Count Then
                        dgCartList.Columns(di.Value).Visible = False
                    End If
                Next

                If _HideHeader Then
                    dgCartList.ShowHeader = False
                End If

                If _HideTotals Then
                    lblTaxAmount.Visible = False
                    lblTax.Visible = False
                End If

            End If
        End Sub

        Private Sub dgCartList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCartList.ItemDataBound
            Dim itemInfo As NB_Store_CartListInfo = CType(e.Item.DataItem, NB_Store_CartListInfo)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim lbl As Label = DirectCast(e.Item.FindControl("lblUnitCost"), Label)
                If Not lbl Is Nothing Then
                    lbl.Text = FormatToStoreCurrency(PortalID, itemInfo.UnitCost)
                End If
                Dim lbl2 As Label = DirectCast(e.Item.FindControl("lblDiscount"), Label)
                If Not lbl2 Is Nothing Then
                    lbl2.Text = FormatToStoreCurrency(PortalID, (itemInfo.Discount * itemInfo.Quantity))
                End If
                lbl = DirectCast(e.Item.FindControl("lblSubTotal"), Label)
                If Not lbl Is Nothing Then
                    lbl.Text = FormatToStoreCurrency(PortalID, itemInfo.SubTotal)
                    cartTotal += itemInfo.SubTotal
                    itemCount += itemInfo.Quantity
                End If
                If _NoUpdates Then
                    Dim txt As TextBox = DirectCast(e.Item.FindControl("txtQty"), TextBox)
                    txt.Enabled = False
                    Dim chk As CheckBox = DirectCast(e.Item.FindControl("chkRemove"), CheckBox)
                    chk.Enabled = False
                    Dim txt2 As TextBox = DirectCast(e.Item.FindControl("txtUnitCost"), TextBox)
                    txt2.Enabled = False
                End If
                If _AllowPriceEdit Then
                    Dim txt3 As TextBox = DirectCast(e.Item.FindControl("txtUnitCost"), TextBox)
                    txt3.Visible = True
                End If
            End If
            If e.Item.ItemType = ListItemType.Footer And Not _HideTotals Then
                Dim lblCount As Label = CType(e.Item.FindControl("lblQtyCount"), Label)
                If Not (lblCount Is Nothing) Then
                    lblCount.Text = itemCount.ToString
                End If
                Dim lblTotal As Label = CType(e.Item.FindControl("lblTotal"), Label)
                If Not (lblTotal Is Nothing) Then
                    lblTotal.Text = FormatToStoreCurrency(PortalID, cartTotal)
                End If

                Dim Totals As CartTotals
                Totals = CurrentCart.GetCalulatedTotals(PortalID, _ShipMethodID)

                Dim lblValue As Label
                Dim lblLabel As Label

                lblValue = CType(e.Item.FindControl("lblOrdTotal"), Label)
                If Not (lblValue Is Nothing) Then
                    lblValue.Text = FormatToStoreCurrency(PortalID, Totals.OrderTotal)
                End If

                lblValue = CType(e.Item.FindControl("lblDiscountTotal"), Label)
                lblLabel = CType(e.Item.FindControl("lblDiscount"), Label)
                If Not (lblLabel Is Nothing) And Not (lblValue Is Nothing) Then
                    assignDiscount(lblValue, lblLabel, Totals)
                End If

                lblValue = CType(e.Item.FindControl("lblTotalAfterDiscount"), Label)
                lblLabel = CType(e.Item.FindControl("lblAfterDiscount"), Label)
                If Not (lblLabel Is Nothing) And Not (lblValue Is Nothing) Then
                    assignAfterDiscount(lblValue, lblLabel, Totals)
                End If

                lblValue = CType(e.Item.FindControl("lblShippingTotal"), Label)
                lblLabel = CType(e.Item.FindControl("lblShipping"), Label)
                If Not (lblLabel Is Nothing) And Not (lblValue Is Nothing) Then
                    If ShippingHidden Then
                        lblValue.Visible = False
                        lblLabel.Visible = False
                    Else
                        assignShipping(lblValue, lblLabel, Totals)
                    End If
                End If

                lblValue = CType(e.Item.FindControl("lblAppliedTaxAmount"), Label)
                lblLabel = CType(e.Item.FindControl("lblAppliedTax"), Label)
                If Not (lblLabel Is Nothing) And Not (lblValue Is Nothing) Then
                    assignTax(lblValue, lblLabel, Totals)
                End If

                lblValue = CType(e.Item.FindControl("lblOutstandingAmount"), Label)
                lblLabel = CType(e.Item.FindControl("lblOutstanding"), Label)
                If Not (lblLabel Is Nothing) And Not (lblValue Is Nothing) Then
                    If Totals.Balance <= 0 Then
                        lblValue.Visible = False
                        lblLabel.Visible = False
                    Else
                        lblValue.Text = FormatToStoreCurrency(PortalID, Totals.Balance)
                    End If
                End If

            End If
            If e.Item.ItemType = ListItemType.Footer And _HideTotals Then
                e.Item.Visible = False
            End If

        End Sub

        Private Sub cmdRecalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRecalculate.Click
            UpdateDataGridQty()
            RaiseEvent RecalculateCart()
        End Sub

#End Region

#Region "Methods"

        Public Sub UpdateQty()
            UpdateDataGridQty()
        End Sub

        Private Sub assignAfterDiscount(ByVal lblValue As Label, ByVal lblLabel As Label, ByVal Totals As CartTotals)
            If Totals.DiscountAmt = 0 Then
                lblValue.Visible = False
                lblLabel.Visible = False
            Else
                lblValue.Visible = True
                lblLabel.Visible = True
                If (Totals.TotalAmt + Totals.DiscountAmt) < 0 Then
                    lblValue.Text = FormatToStoreCurrency(PortalID, 0)
                Else
                    lblValue.Text = FormatToStoreCurrency(PortalID, Totals.TotalAmt + Totals.DiscountAmt)
                End If
            End If
        End Sub


        Private Sub assignDiscount(ByVal lblValue As Label, ByVal lblLabel As Label, ByVal Totals As CartTotals)
            If Totals.DiscountAmt = 0 Then
                lblValue.Visible = False
                lblLabel.Visible = False
                _ShowDiscountCol = False
            Else
                lblValue.Visible = True
                lblLabel.Visible = True
                lblValue.Text = FormatToStoreCurrency(PortalID, Totals.DiscountAmt)
            End If
            dgCartList.Columns(4).Visible = _ShowDiscountCol
        End Sub

        Private Sub assignShipping(ByVal lblShippingTotal As Label, ByVal lblShipping As Label, ByVal Totals As CartTotals)
            lblShippingTotal.Visible = True
            lblShipping.Visible = True
            lblShippingTotal.Text = FormatToStoreCurrency(PortalID, Totals.ShipAmt)
        End Sub

        Private Sub assignTax(ByVal lblAppliedTaxAmount As Label, ByVal lblAppliedTax As Label, ByVal Totals As CartTotals)
            lblTaxAmount.Visible = False
            lblTax.Visible = False
            lblAppliedTaxAmount.Visible = False
            lblAppliedTax.Visible = False

            Select Case _TaxOption
                Case "1" 'no tax
                    'don't display any tax fields
                Case "2" ' gross (tax included in cart total & shipping)
                    lblTaxAmount.Visible = True
                    lblTax.Visible = True
                    lblTaxAmount.Text = FormatToStoreCurrency(PortalID, Totals.TaxAmt)
                Case "3" ' net (tax added to cart total
                    lblAppliedTaxAmount.Visible = True
                    lblAppliedTax.Visible = True
                    lblAppliedTaxAmount.Text = FormatToStoreCurrency(PortalID, Totals.TaxAppliedAmt)
            End Select

        End Sub


        Private Sub UpdateDataGridQty()
            Dim dgi As DataGridItem
            Dim txt As TextBox
            Dim txt2 As TextBox
            Dim objCtrl As New CartController
            Dim strMsg As String = ""
            Dim QtyAvailable As Integer = 0
            Dim objSTCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim chk As CheckBox
            Dim NewPrice As Decimal

            For Each dgi In dgCartList.Items
                txt = DirectCast(dgi.FindControl("txtQty"), TextBox)
                If Not txt Is Nothing Then

                    'set qty to "0" if checkbox remove is true
                    chk = DirectCast(dgi.FindControl("chkRemove"), CheckBox)
                    If Not chk Is Nothing Then
                        If chk.Checked Then
                            txt.Text = "0"
                        End If
                    End If

                    If IsNumeric(txt.Text) And IsNumeric(dgi.Cells(0).Text) Then
                        If CInt(txt.Text) > 0 Then
                            QtyAvailable = CurrentCart.CheckStockByCartItemID(PortalID, CInt(dgi.Cells(0).Text), CInt(txt.Text))

                            Dim QtyLimit As Integer = 999999
                            objSInfo = objSTCtrl.GetSetting(PortalID, "productqty.limit", GetCurrentCulture)
                            If Not objSInfo Is Nothing Then
                                If IsNumeric(objSInfo.SettingValue) Then
                                    QtyLimit = CInt(objSInfo.SettingValue)
                                    If QtyAvailable > QtyLimit Then QtyAvailable = QtyLimit
                                End If
                            End If

                            If QtyAvailable < CInt(txt.Text) Then
                                strMsg = Localization.GetString("QtyAdjusted", _ResourceFile)
                            End If
                            CurrentCart.UpdateCartItemQty(CInt(dgi.Cells(0).Text), QtyAvailable)

                            'update price
                            NewPrice = -1
                            If _AllowPriceEdit Then
                                txt2 = DirectCast(dgi.FindControl("txtUnitCost"), TextBox)
                                If IsNumeric(txt2.Text) Then
                                    If CDec(txt2.Text) >= 0 Then
                                        NewPrice = CDec(txt2.Text)
                                    End If
                                End If
                                If NewPrice >= 0 Then
                                    CurrentCart.UpdateCartItemUnitCost(CInt(dgi.Cells(0).Text), NewPrice)
                                End If
                            End If

                        Else
                            CurrentCart.RemoveItemFromCart(CInt(dgi.Cells(0).Text))
                            If CurrentCart.IsCartEmpty(PortalID) Then
                                RaiseEvent CartIsEmpty()
                            End If
                        End If
                    End If
                End If
            Next
            lblMsg.Text = strMsg
            If strMsg <> "" Then lblMsg.Visible = True
            PopulateCartList()
        End Sub

        Public Sub PopulateCartList()
            PopulateCartList(-1)
        End Sub

        Public Sub PopulateCartList(ByVal OrderID As Integer)
            Dim objCtrl As New CartController
            Dim aryList As ArrayList

            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgCartList, _ResourceFile)

            RaiseEvent ValidateCart()

            aryList = objCtrl.GetCartList(_CartID)
            cartTotal = 0
            itemCount = 0

            dgCartList.Columns(4).Visible = _ShowDiscountCol

            If Not aryList Is Nothing Then
                dgCartList.DataSource = aryList
                dgCartList.DataBind()
            End If

            If _NoUpdates Then
                'get estimated ship date
                Dim objCInfo As NB_Store_CartInfo
                Dim objOInfo As NB_Store_OrdersInfo
                Dim objOCtrl As New OrderController
                lblEstShipDate.Visible = False
                objCInfo = objCtrl.GetCart(_CartID)
                If Not objCInfo Is Nothing Then
                    objOInfo = objOCtrl.GetOrder(objCInfo.OrderID)
                    If Not objOInfo Is Nothing Then
                        If objOInfo.ShipDate > Today Then
                            lblEstShipDate.Text = Replace(Localization.GetString("lblEstShipDate", _ResourceFile), "[TAG:ESTIMATEDDATE]", Common.GetMediumDate(objOInfo.ShipDate))
                            lblEstShipDate.Visible = True
                        End If
                    End If
                End If
            End If

        End Sub


#End Region

    End Class

End Namespace
