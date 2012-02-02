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



    Partial Public Class ShipRates
        Inherits BaseAdminModule

        Private _ShipType As String
        Private _ShipMethodID As Integer


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                'Sample code to get data
                If Not Request.QueryString("spg") Is Nothing Then
                    Select Case Request.QueryString("spg").ToUpper
                        Case "QTY"
                            _ShipType = Request.QueryString("spg").ToUpper
                        Case "PRC"
                            _ShipType = Request.QueryString("spg").ToUpper
                        Case "WEI"
                            _ShipType = Request.QueryString("spg").ToUpper
                        Case "PRD"
                            _ShipType = Request.QueryString("spg").ToUpper
                        Case "COU"
                            _ShipType = Request.QueryString("spg").ToUpper
                        Case "FRE"
                            _ShipType = Request.QueryString("spg").ToUpper
                    End Select

                    _ShipMethodID = -1
                    If IsNumeric(Request.QueryString("smethod")) Then
                        _ShipMethodID = CInt(Request.QueryString("smethod"))
                    Else
                        _ShipMethodID = GetDefaultShipMethod(PortalId)
                    End If

                    If Not Page.IsPostBack Then
                        pnlFree.Visible = False
                        cmdDelete.Visible = False

                        populateShipMethodList(ddlShipMethod, PortalId, _ShipMethodID.ToString)
                        If ddlShipMethod.Items.Count = 0 Then
                            _ShipType = "ERR" ' No shipping method found
                        ElseIf ddlShipMethod.Items.Count = 1 Then
                            ddlShipMethod.Visible = False
                        End If

                        Select Case _ShipType.ToUpper
                            Case "QTY"
                                grdRange.Columns(4).Visible = False
                                grdRange.Columns(8).Visible = False
                                grdRange.Columns(9).Visible = False
                                pnlCountry.Visible = False
                                pnlProduct.Visible = False
                            Case "PRC"
                                grdRange.Columns(4).Visible = False
                                grdRange.Columns(8).Visible = False
                                grdRange.Columns(9).Visible = False
                                pnlCountry.Visible = False
                                pnlProduct.Visible = False
                            Case "WEI"
                                grdRange.Columns(4).Visible = False
                                grdRange.Columns(8).Visible = False
                                grdRange.Columns(9).Visible = False
                                pnlCountry.Visible = False
                                pnlProduct.Visible = False
                            Case "PRD"
                                grdRange.Columns(5).Visible = False
                                grdRange.Columns(6).Visible = False
                                grdRange.Columns(8).Visible = True
                                grdRange.Columns(9).Visible = True
                                pnlCountry.Visible = False
                                pnlProduct.Visible = True
                                cmdNew.Visible = False
                                lblShipMethod.Visible = False
                                ddlShipMethod.Visible = False
                                _ShipMethodID = -1
                                populateCategoryList(PortalId, ddlCategory, -1, "All Categories", "")
                                populatePDefault()
                            Case "COU"
                                'grdRange.Columns(5).Visible = False
                                'grdRange.Columns(6).Visible = False
                                grdRange.Columns(8).Visible = False
                                grdRange.Columns(9).Visible = False
                                pnlCountry.Visible = True
                                pnlProduct.Visible = False
                                cmdNew.Visible = False
                                loadCountryList()
                                populateCDefault()
                            Case "FRE"
                                pnlCountry.Visible = False
                                pnlProduct.Visible = False
                                pnlRange.Visible = False
                                pnlFree.Visible = True
                                cmdDelete.Visible = True
                                cmdNew.Visible = False
                                populateFDefault()
                            Case Else
                                pnlEdit.Visible = False
                        End Select
                        If _ShipType <> "PRD" Then
                            populateRangeDG()
                        End If
                    End If
                End If


            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
            AddNewRecord(-1, "")
            Response.Redirect(Request.Url.ToString)
        End Sub

        Private Sub grdRange_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdRange.DeleteCommand
            Dim objCtl As New ShipController
            Dim objInfo As NB_Store_ShippingRatesInfo
            objInfo = objCtl.GetShippingRate(PortalId, CInt(e.Item.Cells(1).Text))
            objCtl.DeleteShippingRate(CInt(e.Item.Cells(1).Text))
            If Not objInfo Is Nothing Then
                UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
            End If
            populateRangeDG()
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            updateRangeDG()
        End Sub

        Private Sub cmdAddCounty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddCounty.Click
            AddNewRecord(-1, ddlCountry.SelectedItem.Text)
            Response.Redirect(Request.Url.ToString)
        End Sub

        Private Sub grdRange_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdRange.PageIndexChanged
            grdRange.CurrentPageIndex = e.NewPageIndex
            populateRangeDG()
        End Sub

        Private Sub grdRange_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdRange.ItemDataBound
            If _ShipType = "PRD" Then
                Dim item As DataGridItem = e.Item
                If item.ItemType = ListItemType.Header Then
                    Dim dglbl As Label = DirectCast(e.Item.FindControl("lbldgCostHeader"), Label)
                    dglbl.Attributes("resourcekey") = "dgBoxCost"
                End If
                If item.ItemType = ListItemType.Item Or _
                    item.ItemType = ListItemType.AlternatingItem Or _
                    item.ItemType = ListItemType.SelectedItem Then

                    Dim dgtxtWeight As TextBox = DirectCast(e.Item.FindControl("txtWeight"), TextBox)
                    Dim dgtxtShipCost As TextBox = DirectCast(e.Item.FindControl("txtShipCost"), TextBox)
                    Dim dgtxtRange1 As TextBox = DirectCast(e.Item.FindControl("txtRange1"), TextBox)

                    If Not dgtxtWeight Is Nothing And Not dgtxtShipCost Is Nothing Then
                        Dim chk As CheckBox = DirectCast(e.Item.FindControl("chkDisable"), CheckBox)
                        If IsNumeric(dgtxtWeight.Text) And IsNumeric(dgtxtShipCost.Text) Then
                            If CDec(dgtxtWeight.Text) = 0 And CDec(dgtxtShipCost.Text) = 0 Then
                                chk.Visible = False
                                e.Item.Cells(0).Text = ""
                            End If
                        Else
                            chk.Visible = False
                            e.Item.Cells(0).Text = ""
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            grdRange.CurrentPageIndex = 0
            populateRangeDG()
        End Sub

        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
            deleteFDefault()
            Response.Redirect(EditUrl("smethod", GetSelectedShipMethod, "ShipRates", "spg=" & _ShipType), True)
        End Sub

        Private Sub ddlShipMethod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShipMethod.SelectedIndexChanged
            Response.Redirect(EditUrl("smethod", GetSelectedShipMethod, "ShipRates", "spg=" & _ShipType), True)
        End Sub

#Region "Procedures"

        Private Function GetSelectedShipMethod() As String
            If ddlShipMethod.Visible Then
                Return ddlShipMethod.SelectedValue
            Else
                If _ShipType.ToUpper = "PRD" Then
                    Return -1
                Else
                    Return GetDefaultShipMethod(PortalId)
                End If
            End If
        End Function

        Private Sub AddNewRecord(ByVal ObjectID As Integer, ByVal Description As String)
            Dim objCtrl As New ShipController
            Dim objInfo As New NB_Store_ShippingRatesInfo

            objInfo.ItemId = 0
            objInfo.ObjectId = ObjectID
            objInfo.Range1 = 0
            objInfo.Range2 = 0
            objInfo.ShipCost = 0
            objInfo.ShipType = _ShipType.ToUpper
            objInfo.Description = Description
            objInfo.ProductWeight = 0
            objInfo.ProductHeight = 0
            objInfo.ProductLength = 0
            objInfo.ProductWidth = 0
            objInfo.PortalID = PortalId
            objInfo.ShipMethodID = _ShipMethodID
            objCtrl.UpdateObjShippingRate(objInfo)
            UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
        End Sub

        Private Sub populateRangeDG()
            Dim objCtrl As New ShipController
            Dim list As ArrayList

            If _ShipType.ToUpper = "PRD" Then
                list = objCtrl.GetShippingRateList(PortalId, _ShipType.ToUpper, GetCurrentCulture(), txtSearch.Text, ddlCategory.SelectedValue, -1)
            Else
                list = objCtrl.GetShippingRateList(PortalId, _ShipType.ToUpper, GetCurrentCulture(), txtSearch.Text, _ShipMethodID)
            End If

            grdRange.DataSource = list
            grdRange.DataBind()

        End Sub

        Private Sub populateCDefault()
            Dim objCtrl As New ShipController
            Dim list As ArrayList
            Dim objInfo As New NB_Store_ShippingRatesInfo

            list = objCtrl.GetShippingRateList(PortalId, "DCO", GetCurrentCulture(), "", _ShipMethodID)
            If list Is Nothing Then
                txtCDefault.Text = "0"
                lblCDefaultID.Text = "0"
            Else
                If list.Count = 0 Then
                    txtCDefault.Text = "0"
                    lblCDefaultID.Text = "0"
                Else
                    For Each objInfo In list
                        txtCDefault.Text = objInfo.ShipCost.ToString
                        lblCDefaultID.Text = objInfo.ItemId.ToString
                    Next
                End If
            End If

        End Sub

        Private Sub populateFDefault()
            Dim objCtrl As New ShipController
            Dim list As ArrayList
            Dim objInfo As New NB_Store_ShippingRatesInfo

            list = objCtrl.GetShippingRateList(PortalId, "FRE", GetCurrentCulture(), "", _ShipMethodID)
            If list Is Nothing Then
                txtFreeShip.Text = "0"
                lblFreeShip.Text = "0"
            Else
                If list.Count = 0 Then
                    txtFreeShip.Text = "0"
                    lblFreeShip.Text = "0"
                Else
                    For Each objInfo In list
                        txtFreeShip.Text = objInfo.ShipCost.ToString
                        lblFreeShip.Text = objInfo.ItemId.ToString
                    Next
                End If
            End If

            txtFreeShipList.Text = GetStoreSetting(PortalId, "shipfree.list", "None", True)


        End Sub

        Private Sub deleteFDefault()
            Dim objCtrl As New ShipController
            Dim list As ArrayList
            Dim objInfo As New NB_Store_ShippingRatesInfo

            list = objCtrl.GetShippingRateList(PortalId, "FRE", GetCurrentCulture(), "", _ShipMethodID)
            If Not list Is Nothing Then
                For Each objInfo In list
                    objCtrl.DeleteShippingRate(objInfo.ItemId)
                    UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
                Next
            End If
        End Sub


        Private Sub populatePDefault()
            Dim objCtrl As New ShipController
            Dim list As ArrayList
            Dim objInfo As New NB_Store_ShippingRatesInfo

            list = objCtrl.GetShippingRateList(PortalId, "DPO", GetCurrentCulture(), "", -1)
            If list Is Nothing Then
                txtPDefault.Text = "0"
                lblPDefaultID.Text = "0"
            Else
                If list.Count = 0 Then
                    txtPDefault.Text = "0"
                    lblPDefaultID.Text = "0"
                Else
                    For Each objInfo In list
                        txtPDefault.Text = objInfo.ProductWeight
                        lblPDefaultID.Text = objInfo.ItemId.ToString
                    Next
                End If
            End If

        End Sub

        Private Sub updateRangeDG()
            Try
                ' Only Update if the Entered Data is Valid
                If Page.IsValid = True Then

                    Dim objCtl As New ShipController
                    Dim objInfo As NB_Store_ShippingRatesInfo
                    Dim objRow As DataGridItem

                    For Each objRow In grdRange.Items
                        objInfo = New NB_Store_ShippingRatesInfo
                        objInfo.ShipMethodID = _ShipMethodID
                        objInfo.PortalID = PortalId
                        objInfo.ItemId = CInt(objRow.Cells(1).Text)
                        objInfo.ObjectId = CInt(objRow.Cells(2).Text)
                        objInfo.ShipType = objRow.Cells(3).Text
                        If objRow.Cells(4).Visible Then
                            objInfo.Description = objRow.Cells(4).Text
                        Else
                            objInfo.Description = ""
                        End If
                        Try
                            If _ShipType.ToUpper = "PRD" Then
                                'use range1 field for box % on product, saves us creating extra DB field.
                                objInfo.Range1 = CDec(CType(objRow.FindControl("txtBox"), TextBox).Text)
                            Else
                                objInfo.Range1 = CDec(CType(objRow.FindControl("txtRange1"), TextBox).Text)
                            End If
                        Catch ex As Exception
                            objInfo.Range1 = 0
                        End Try
                        Try
                            objInfo.Range2 = CDec(CType(objRow.FindControl("txtRange2"), TextBox).Text)
                        Catch ex As Exception
                            objInfo.Range2 = 0
                        End Try
                        Try
                            objInfo.ShipCost = CDec(CType(objRow.FindControl("txtShipCost"), TextBox).Text)
                        Catch ex As Exception
                            objInfo.ShipCost = 0
                        End Try
                        Try
                            objInfo.ProductWeight = CDec(CType(objRow.FindControl("txtWeight"), TextBox).Text)
                        Catch ex As Exception
                            objInfo.ProductWeight = 0
                        End Try
                        objInfo.Disable = CType(objRow.FindControl("chkDisable"), CheckBox).Checked
                        If objInfo.ShipType = "PRD" Then
                            If CDec(objInfo.ShipCost) > 0 Or CDec(objInfo.ProductWeight) > 0 Then
                                'product weights don't have a shipping method.
                                objInfo.ShipMethodID = -1
                                objCtl.UpdateObjShippingRate(objInfo)
                            End If
                        Else
                            objCtl.UpdateObjShippingRate(objInfo)
                        End If
                        UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
                    Next

                    If pnlCountry.Visible Then
                        objInfo = New NB_Store_ShippingRatesInfo
                        objInfo.ShipMethodID = _ShipMethodID
                        objInfo.ItemId = CInt(lblCDefaultID.Text)
                        objInfo.ObjectId = -1
                        objInfo.Range1 = 0
                        objInfo.Range2 = 0
                        Try
                            objInfo.ShipCost = CDec(txtCDefault.Text)
                        Catch ex As Exception
                            objInfo.ShipCost = 0
                        End Try
                        objInfo.ShipType = "DCO"
                        objInfo.Description = "Country Default"
                        objInfo.ProductWeight = 0
                        objInfo.ProductHeight = 0
                        objInfo.ProductLength = 0
                        objInfo.ProductWidth = 0
                        objInfo.PortalID = PortalId
                        objCtl.UpdateObjShippingRate(objInfo)
                        UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
                    End If

                    If pnlProduct.Visible Then
                        objInfo = New NB_Store_ShippingRatesInfo
                        objInfo.ShipMethodID = -1
                        objInfo.ItemId = CInt(lblPDefaultID.Text)
                        objInfo.ObjectId = -1
                        objInfo.Range1 = "0"
                        objInfo.Range2 = "0"
                        Try
                            objInfo.ProductWeight = CDec(txtPDefault.Text)
                        Catch ex As Exception
                            objInfo.ProductWeight = 0
                        End Try
                        objInfo.ShipType = "DPO"
                        objInfo.Description = "Product Weight Default"
                        objInfo.ShipCost = 0
                        objInfo.ProductHeight = 0
                        objInfo.ProductLength = 0
                        objInfo.ProductWidth = 0
                        objInfo.PortalID = PortalId
                        objCtl.UpdateObjShippingRate(objInfo)
                        UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
                    End If

                    If pnlFree.Visible Then
                        objInfo = New NB_Store_ShippingRatesInfo
                        objInfo.ShipMethodID = _ShipMethodID
                        objInfo.ItemId = CInt(lblFreeShip.Text)
                        objInfo.ObjectId = -1
                        objInfo.Range1 = 0
                        objInfo.Range2 = 0
                        Try
                            objInfo.ShipCost = CDec(txtFreeShip.Text)
                        Catch ex As Exception
                            objInfo.ShipCost = 0
                        End Try
                        objInfo.ProductWeight = 0
                        objInfo.ShipType = "FRE"
                        objInfo.Description = "Free Shipping Level"
                        objInfo.ProductHeight = 0
                        objInfo.ProductLength = 0
                        objInfo.ProductWidth = 0
                        objInfo.PortalID = PortalId
                        objCtl.UpdateObjShippingRate(objInfo)
                        UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
                        SetStoreSetting(PortalId, "shipfree.list", txtFreeShipList.Text, "None", False)
                    End If
                    If _ShipType = "PRD" Then
                        populateRangeDG()
                    Else
                        Response.Redirect(Request.Url.ToString)
                    End If
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub loadCountryList()
            populateCountryList(PortalId, ddlCountry, "", "XX - " & Localization.GetString("Default", NBSTORERESX), "XX - " & Localization.GetString("Default", NBSTORERESX), True)
        End Sub

#End Region

    End Class

End Namespace
