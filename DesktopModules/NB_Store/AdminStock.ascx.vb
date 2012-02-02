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

    Partial Public Class AdminStock
        Inherits BaseAdminModule

#Region "Private Members"
        Private _CurrentPage As Integer = 1
        Private _ctl As String = ""
        Private _mid As String = ""
        Private _SkinSrc As String = ""
#End Region

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            _CurrentPage = 1
            If Not (Request.QueryString("currentpage") Is Nothing) Then
                If IsNumeric(Request.QueryString("currentpage")) Then
                    _CurrentPage = Request.QueryString("currentpage")
                End If
            End If

            _ctl = ""
            If Not (Request.QueryString("ctl") Is Nothing) Then
                _ctl = Request.QueryString("ctl")
            End If

            _mid = ""
            If Not (Request.QueryString("mid") Is Nothing) Then
                _mid = Request.QueryString("mid")
            End If

            _SkinSrc = ""
            If Not (Request.QueryString("SkinSrc") Is Nothing) Then
                _SkinSrc = Request.QueryString("SkinSrc")
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    populateCategoryList(PortalId, cmbCategory, "-1", Localization.GetString("All", LocalResourceFile), "")
                    populateModel()
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            Try
                dgModel.CurrentPageIndex = 0
                _CurrentPage = 1
                setStoreCookieValue(PortalSettings.PortalId, Me.ID, "SearchText", txtSearch.Text, 1)
                setStoreCookieValue(PortalSettings.PortalId, Me.ID, "CatId", cmbCategory.SelectedValue, 1)
                populateModel()
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            Try
                UpdateModel()
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub dgModel_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgModel.PageIndexChanged
            dgModel.CurrentPageIndex = e.NewPageIndex
            populateModel()
        End Sub


        Private Sub populateModel()
            Dim objCtrl As New ProductController
            Dim PgSize As Integer = 25
            Dim ListSize As Integer = 0

            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgModel, LocalResourceFile)

            If IsNumeric(GetStoreSetting(PortalId, "stockpage.size")) Then
                PgSize = CInt(GetStoreSetting(PortalId, "stockpage.size"))
            End If

            If Not Page.IsPostBack Then
                txtSearch.Text = getStoreCookieValue(PortalSettings.PortalId, Me.ID, "SearchText")
                Dim CatValue As String = getStoreCookieValue(PortalSettings.PortalId, Me.ID, "CatId")
                If Not cmbCategory.Items.FindByValue(CatValue) Is Nothing Then
                    cmbCategory.SelectedValue = CatValue
                End If
            End If

            ' get content
            Dim aryList As ArrayList

            aryList = objCtrl.GetModelStockList(PortalId, txtSearch.Text, GetCurrentCulture, cmbCategory.SelectedValue, _CurrentPage, PgSize, IsDealer(PortalId, UserInfo, GetCurrentCulture))
            ListSize = objCtrl.GetModelStockListSize(PortalId, txtSearch.Text, GetCurrentCulture, cmbCategory.SelectedValue, IsDealer(PortalId, UserInfo, GetCurrentCulture))

            dgModel.DataSource = aryList
            dgModel.DataBind()

            If ListSize <= PgSize Or PgSize = -1 Then
                ctlPagingControl.Visible = False
                lblLineBreak.Visible = False
            Else
                ctlPagingControl.Visible = True
                lblLineBreak.Visible = True
            End If

            ctlPagingControl.TotalRecords = ListSize
            ctlPagingControl.PageSize = PgSize
            ctlPagingControl.CurrentPage = _CurrentPage
            ctlPagingControl.TabID = PortalSettings.ActiveTab.TabID
            ctlPagingControl.BorderWidth = 0
            ctlPagingControl.QuerystringParams = "ctl=" & _ctl & "&mid=" & _mid
            ctlPagingControl.SkinSrc = _SkinSrc

        End Sub


        Private Sub UpdateModel()
            If Page.IsValid = True Then

                Dim objRow As DataGridItem
                Dim objCtrl As New ProductController
                Dim objInfo As NB_Store_ModelInfo
                Dim txtValue As String
                Dim UpdateFlag As Boolean = False

                For Each objRow In dgModel.Items
                    UpdateFlag = False
                    objInfo = objCtrl.GetModel(CInt(objRow.Cells(0).Text), objRow.Cells(1).Text)
                    If Not objInfo Is Nothing Then

                        txtValue = CType(objRow.FindControl("txtMaxStock"), TextBox).Text
                        If IsNumeric(txtValue) Then
                            If objInfo.QtyStockSet <> CInt(txtValue) Then
                                objInfo.QtyStockSet = CInt(txtValue)
                                UpdateFlag = True
                            End If
                        End If

                        txtValue = CType(objRow.FindControl("txtQtyRemaining"), TextBox).Text
                        If IsNumeric(txtValue) Then
                            If objInfo.QtyRemaining <> CInt(txtValue) Then
                                objInfo.QtyRemaining = CInt(txtValue)
                                If objInfo.QtyRemaining > objInfo.QtyStockSet Then
                                    objInfo.QtyStockSet = objInfo.QtyRemaining
                                End If
                                UpdateFlag = True
                            End If
                        End If

                        txtValue = CType(objRow.FindControl("txtUnitCost"), TextBox).Text
                        If IsNumeric(txtValue) Then
                            If objInfo.UnitCost <> CDec(txtValue) Then
                                objInfo.UnitCost = CDec(txtValue)
                                UpdateFlag = True
                            End If
                        End If

                        txtValue = CType(objRow.FindControl("txtDealerCost"), TextBox).Text
                        If IsNumeric(txtValue) Then
                            If objInfo.DealerCost <> CDec(txtValue) Then
                                objInfo.DealerCost = CDec(txtValue)
                                UpdateFlag = True
                            End If
                        End If

                        txtValue = CType(objRow.FindControl("txtPurchaseCost"), TextBox).Text
                        If IsNumeric(txtValue) Then
                            If objInfo.PurchaseCost <> CDec(txtValue) Then
                                objInfo.PurchaseCost = CDec(txtValue)
                                UpdateFlag = True
                            End If
                        End If

                        Dim dgchkDealerOnly As CheckBox = DirectCast(objRow.FindControl("chkDealerOnly"), CheckBox)
                        If dgchkDealerOnly.Checked <> objInfo.DealerOnly Then
                            objInfo.DealerOnly = dgchkDealerOnly.Checked
                            UpdateFlag = True
                        End If

                        If UpdateFlag Then
                            objInfo.QtyTrans = 0
                            objCtrl.UpdateObjModel(objInfo)
                        End If
                    End If
                Next

                Response.Redirect(EditUrl("AdminStock"), True)

            End If
        End Sub

        Private Sub dgModel_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgModel.ItemDataBound
            Dim item As DataGridItem = e.Item
            If item.ItemType = ListItemType.Item Or _
                item.ItemType = ListItemType.AlternatingItem Or _
                item.ItemType = ListItemType.SelectedItem Then

                Dim dgtxtUnitCost As TextBox = DirectCast(e.Item.FindControl("txtUnitCost"), TextBox)
                Dim dgtxtDealerCost As TextBox = DirectCast(e.Item.FindControl("txtDealerCost"), TextBox)
                Dim dgtxtPurchaseCost As TextBox = DirectCast(e.Item.FindControl("txtPurchaseCost"), TextBox)

                Dim dglblModelName As Label = DirectCast(e.Item.FindControl("lblModelName"), Label)
                Dim dglblProductName As Label = DirectCast(e.Item.FindControl("lblProductName"), Label)

                If Not dglblProductName Is Nothing Then
                    If CType(e.Item.DataItem, NB_Store_ModelInfo).ProductName.Length > 20 Then
                        dglblProductName.Text = CType(e.Item.DataItem, NB_Store_ModelInfo).ProductName.Substring(0, 20)
                        dglblProductName.ToolTip = CType(e.Item.DataItem, NB_Store_ModelInfo).ProductName
                    Else
                        dglblProductName.Text = CType(e.Item.DataItem, NB_Store_ModelInfo).ProductName
                    End If
                End If

                If Not dglblModelName Is Nothing Then
                    If CType(e.Item.DataItem, NB_Store_ModelInfo).ModelName.Length > 20 Then
                        dglblModelName.Text = CType(e.Item.DataItem, NB_Store_ModelInfo).ModelName.Substring(0, 20)
                        dglblModelName.ToolTip = CType(e.Item.DataItem, NB_Store_ModelInfo).ModelName
                    Else
                        dglblModelName.Text = CType(e.Item.DataItem, NB_Store_ModelInfo).ModelName
                    End If
                End If


                If Not dgtxtUnitCost Is Nothing Then
                    If IsNumeric(dgtxtUnitCost.Text) Then
                        dgtxtUnitCost.Text = CDbl(dgtxtUnitCost.Text).ToString
                    End If
                End If

                If Not dgtxtDealerCost Is Nothing Then
                    If IsNumeric(dgtxtDealerCost.Text) Then
                        dgtxtDealerCost.Text = CDbl(dgtxtDealerCost.Text).ToString
                    End If
                End If

                If Not dgtxtPurchaseCost Is Nothing Then
                    If IsNumeric(dgtxtPurchaseCost.Text) Then
                        dgtxtPurchaseCost.Text = CDbl(dgtxtPurchaseCost.Text).ToString
                    End If
                End If

            End If

        End Sub

    End Class

End Namespace
