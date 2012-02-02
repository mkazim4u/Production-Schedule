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

    Partial Public Class TaxRates
        Inherits BaseAdminModule


        Private _TaxType As String

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                'Sample code to get data
                If Not Request.QueryString("spg") Is Nothing Then
                    Select Case Request.QueryString("spg").ToUpper
                        Case "SET"
                            _TaxType = Request.QueryString("spg").ToUpper
                        Case "CAT"
                            _TaxType = Request.QueryString("spg").ToUpper
                        Case "PRD"
                            _TaxType = Request.QueryString("spg").ToUpper
                    End Select

                    If Not Page.IsPostBack Then
                        Select Case _TaxType.ToUpper
                            Case "SET"
                                grdRange.Visible = False
                                pnlCategory.Visible = False
                                pnlProduct.Visible = False
                                pnlSetup.Visible = True
                                populateTDefault()
                                populateTaxOptions()
                            Case "CAT"
                                pnlCategory.Visible = True
                                pnlProduct.Visible = False
                                pnlSetup.Visible = False
                                populateRangeDG()
                                populateCategoryList(PortalId, ddlCategory)
                            Case "PRD"
                                pnlCategory.Visible = False
                                pnlProduct.Visible = True
                                pnlSetup.Visible = False
                                populateRangeDG()
                            Case Else
                                pnlEdit.Visible = False
                        End Select

                    End If
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            Try
                updateRangeDG()
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        Private Sub grdRange_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdRange.DeleteCommand
            Dim objCtl As New TaxController
            Dim objInfo As NB_Store_TaxRatesInfo
            objInfo = objCtl.GetTaxRate(PortalId, CInt(e.Item.Cells(1).Text))
            objCtl.DeleteTaxRate(CInt(e.Item.Cells(1).Text))
            If Not objInfo Is Nothing Then
                UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
            End If
            Response.Redirect(Request.Url.ToString)
        End Sub

        Private Sub cmdAddCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddCategory.Click
            AddNewRecord(CInt(ddlCategory.SelectedValue), ddlCategory.SelectedItem.Text)
            Response.Redirect(Request.Url.ToString)
        End Sub

        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            grdRange.CurrentPageIndex = 0
            populateRangeDG()
        End Sub

        Private Sub grdRange_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdRange.PageIndexChanged
            grdRange.CurrentPageIndex = e.NewPageIndex
            populateRangeDG()
        End Sub

#End Region

#Region "Procedures"

        Private Sub AddNewRecord(ByVal ObjectID As Integer, ByVal Description As String)
            Dim objCtrl As New TaxController
            Dim objInfo As New NB_Store_TaxRatesInfo

            objInfo.ItemID = 0
            objInfo.ObjectID = ObjectID
            objInfo.TaxDesc = Description
            objInfo.Disable = False
            objInfo.TaxType = _TaxType
            objInfo.TaxPercent = "0"
            objInfo.PortalID = PortalId
            objCtrl.UpdateObjTaxRate(objInfo)
            If Not objInfo Is Nothing Then
                UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
            End If
        End Sub

        Private Sub populateRangeDG()
            Dim objCtrl As New TaxController
            Dim list As ArrayList

            list = objCtrl.GetTaxRatesList(PortalId, _TaxType, GetCurrentCulture, txtSearch.Text)

            grdRange.DataSource = list
            grdRange.DataBind()

        End Sub


        Private Sub populateTaxOptions()
            Dim objCtrl As New TaxController
            Dim list As ArrayList
            Dim objInfo As New NB_Store_TaxRatesInfo

            rblTaxOptions.Items(0).Text = Localization.GetString("rblTaxOptions1", LocalResourceFile)
            rblTaxOptions.Items(1).Text = Localization.GetString("rblTaxOptions2", LocalResourceFile)
            rblTaxOptions.Items(2).Text = Localization.GetString("rblTaxOptions3", LocalResourceFile)

            list = objCtrl.GetTaxRatesList(PortalId, "OTX")
            If list Is Nothing Then
                lblTOptionsID.Text = "0"
                rblTaxOptions.SelectedValue = "1"
            Else
                If list.Count = 0 Then
                    lblTOptionsID.Text = "0"
                    rblTaxOptions.SelectedValue = "1"
                Else
                    For Each objInfo In list
                        lblTOptionsID.Text = objInfo.ItemID.ToString
                        rblTaxOptions.SelectedValue = CInt(objInfo.TaxPercent).ToString
                    Next
                End If
            End If

        End Sub

        Private Sub populateTDefault()
            Dim objCtrl As New TaxController
            Dim list As ArrayList
            Dim objInfo As New NB_Store_TaxRatesInfo

            list = objCtrl.GetTaxRatesList(PortalId, "DTX")
            If list Is Nothing Then
                txtTDefault.Text = "0"
                lblTDefaultID.Text = "0"
            Else
                If list.Count = 0 Then
                    txtTDefault.Text = "0"
                    lblTDefaultID.Text = "0"
                Else
                    For Each objInfo In list
                        txtTDefault.Text = objInfo.TaxPercent
                        lblTDefaultID.Text = objInfo.ItemID.ToString
                    Next
                End If
            End If

            list = objCtrl.GetTaxRatesList(PortalId, "STX")
            If list Is Nothing Then
                txtShipTax.Text = "0"
                lblShipTaxID.Text = "0"
            Else
                If list.Count = 0 Then
                    txtShipTax.Text = "0"
                    lblShipTaxID.Text = "0"
                Else
                    For Each objInfo In list
                        txtShipTax.Text = objInfo.TaxPercent
                        lblShipTaxID.Text = objInfo.ItemID.ToString
                    Next
                End If
            End If

        End Sub

        Private Sub updateRangeDG()
            Try
                ' Only Update if the Entered Data is Valid
                If Page.IsValid = True Then

                    Dim objCtl As New TaxController
                    Dim objInfo As NB_Store_TaxRatesInfo
                    Dim objRow As DataGridItem

                    If grdRange.Visible Then
                        For Each objRow In grdRange.Items
                            objInfo = New NB_Store_TaxRatesInfo
                            objInfo.PortalID = PortalId
                            objInfo.ItemID = CInt(objRow.Cells(1).Text)
                            objInfo.ObjectID = CInt(objRow.Cells(2).Text)
                            objInfo.TaxType = objRow.Cells(3).Text
                            If objRow.Cells(4).Visible Then
                                objInfo.TaxDesc = objRow.Cells(4).Text
                            Else
                                objInfo.TaxDesc = ""
                            End If
                            Try
                                objInfo.TaxPercent = CDec(CType(objRow.FindControl("txtTaxPercent"), TextBox).Text).ToString
                            Catch ex As Exception
                                objInfo.TaxPercent = "0"
                            End Try
                            objInfo.Disable = CType(objRow.FindControl("chkDisable"), CheckBox).Checked
                            If objInfo.TaxType = "PRD" Then
                                If objInfo.TaxPercent > 0 Then
                                    objCtl.UpdateObjTaxRate(objInfo)
                                End If
                            Else
                                objCtl.UpdateObjTaxRate(objInfo)
                            End If
                            If Not objInfo Is Nothing Then
                                UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
                            End If
                        Next
                    End If

                    If pnlSetup.Visible Then
                        'default tax
                        objInfo = New NB_Store_TaxRatesInfo
                        objInfo.ItemID = CInt(lblTDefaultID.Text)
                        objInfo.ObjectID = -1
                        Try
                            objInfo.TaxPercent = CDec(txtTDefault.Text).ToString
                        Catch ex As Exception
                            objInfo.TaxPercent = "0"
                        End Try
                        objInfo.TaxType = "DTX"
                        objInfo.TaxDesc = "Tax Default"
                        objInfo.Disable = False
                        objInfo.PortalID = PortalId
                        objCtl.UpdateObjTaxRate(objInfo)
                        If Not objInfo Is Nothing Then
                            UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
                        End If

                        'ship tax
                        objInfo = New NB_Store_TaxRatesInfo
                        objInfo.ItemID = CInt(lblShipTaxID.Text)
                        objInfo.ObjectID = -1
                        Try
                            objInfo.TaxPercent = CDec(txtShipTax.Text).ToString
                        Catch ex As Exception
                            objInfo.TaxPercent = "0"
                        End Try
                        objInfo.TaxType = "STX"
                        objInfo.TaxDesc = "Ship Tax"
                        objInfo.Disable = False
                        objInfo.PortalID = PortalId
                        objCtl.UpdateObjTaxRate(objInfo)
                        If Not objInfo Is Nothing Then
                            UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
                        End If

                        'tax options
                        objInfo = New NB_Store_TaxRatesInfo
                        objInfo.ItemID = CInt(lblTOptionsID.Text)
                        objInfo.ObjectID = -1
                        objInfo.TaxPercent = rblTaxOptions.SelectedValue
                        objInfo.TaxType = "OTX"
                        objInfo.TaxDesc = "Tax Option"
                        objInfo.Disable = False
                        objInfo.PortalID = PortalId
                        objCtl.UpdateObjTaxRate(objInfo)
                        If Not objInfo Is Nothing Then
                            UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
                        End If
                    End If


                    Response.Redirect(Request.Url.ToString)

                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
