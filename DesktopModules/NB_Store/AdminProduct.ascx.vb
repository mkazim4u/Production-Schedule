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

    Partial Public Class AdminProduct
        Inherits BaseAdminModule


#Region "Private Members"

        Protected ProdID As Integer = -1
        Protected _RtnTabID As Integer = -1
        Protected _PageIndex As Integer = 1
        Protected _CatID As Integer = -1
        Protected _SkinSrc As String = ""
        Protected _Related As Boolean = False
        Private _CurrentPage As Integer = 1

#End Region

#Region "Event Handlers"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                ' Determine ItemId of NB_Store to Update
                ProdID = -1
                If Not (Request.QueryString("ProdID") Is Nothing) Then
                    If IsNumeric(Request.QueryString("ProdID")) Then
                        ProdID = CInt(Request.QueryString("ProdID"))
                    End If
                End If
                'store current productid in cookie for use in admin
                setAdminCookieValue(PortalId, "ProdID", ProdID.ToString)


                If Not (Request.QueryString("SkinSrc") Is Nothing) Then
                    _SkinSrc = Request.QueryString("SkinSrc")
                End If

                _CatID = -1
                If Not (Request.QueryString("CatID") Is Nothing) Then
                    If IsNumeric(Request.QueryString("CatID")) Then
                        _CatID = CInt(Request.QueryString("CatID"))
                    End If
                End If

                _RtnTabID = -1
                If Not (Request.QueryString("RtnTab") Is Nothing) Then
                    If IsNumeric(Request.QueryString("RtnTab")) Then
                        _RtnTabID = CInt(Request.QueryString("RtnTab"))
                    End If
                End If

                _PageIndex = -1
                If Not (Request.QueryString("PageIndex") Is Nothing) Then
                    If IsNumeric(Request.QueryString("PageIndex")) Then
                        _PageIndex = CInt(Request.QueryString("PageIndex"))
                    End If
                End If

                _CurrentPage = 1
                If Not (Request.QueryString("currentpage") Is Nothing) Then
                    If IsNumeric(Request.QueryString("currentpage")) Then
                        _CurrentPage = Request.QueryString("currentpage")
                    End If
                End If

                _Related = False
                If Not (Request.QueryString("rel") Is Nothing) Then
                    If Request.QueryString("rel") = "1" Then
                        _Related = True
                    End If
                End If

                If ProdID = 0 Then
                    'new product display create button
                    cmdUpdate.Text = Localization.GetString("CreateProduct", LocalResourceFile)
                    tabs.Visible = False
                Else
                    tabs.Visible = True
                    cmdUpdate.Text = Localization.GetString("cmdUpdate")
                End If


                If _SkinSrc.EndsWith("Edit") Or _Related Then
                    'edit entry from entry list so hide backoffice
                    Me.BackOfficeMenu.Visible = False
                    ''Controls.AddAt(0, New LiteralControl("<br /><br /><br /><br /><br /><br /><br />"))
                Else
                    Me.BackOfficeMenu.Visible = True
                End If

                ' If this is the first visit to the page, bind the role data to the datalist
                If Not Page.IsPostBack Then

                    If Not ReachedProductLimit() Then

                        '------------------------------------------
                        'clear the wishlist, 
                        'this turns off the option to multi select products on the releated product select.
                        WishList.ClearList(PortalId)
                        '------------------------------------------

                        If ProdID = -1 Or _Related Then
                            populateList()
                        Else
                            pnlProductTabs.Visible = True
                            populateEdit()
                        End If

                    End If
                End If


            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#Region "command buttons"

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            If Page.IsValid Then
                Dim objInfo As NB_Store_ProductsInfo
                objInfo = updateProduct()
                If _RtnTabID > 0 And ProdID > 0 Then
                    Response.Redirect(NavigateURL(_RtnTabID, "", "currentpage=" & _PageIndex.ToString, "CatID=" & _CatID.ToString), True)
                Else
                    If Not objInfo Is Nothing And ProdID = 0 Then
                        If _RtnTabID > 0 Then
                            Response.Redirect(EditUrl("ProdId", objInfo.ProductID.ToString, "AdminProduct", "RtnTab=" & _RtnTabID.ToString), True)
                        Else
                            Response.Redirect(EditUrl("ProdId", objInfo.ProductID.ToString, "AdminProduct"), True)
                        End If
                    Else
                        If _CurrentPage > 0 Then
                            Response.Redirect(EditUrl("currentpage", _CurrentPage.ToString, "AdminProduct"), True)
                        Else
                            Response.Redirect(EditUrl("AdminProduct"), True)
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            If _RtnTabID > 0 Then
                Response.Redirect(NavigateURL(_RtnTabID, "", "currentpage=" & _PageIndex.ToString, "CatID=" & _CatID.ToString), True)
            Else
                If _CurrentPage > 0 Then
                    Response.Redirect(EditUrl("currentpage", _CurrentPage.ToString, "AdminProduct"), True)
                Else
                    Response.Redirect(EditUrl("AdminProduct"), True)
                End If
            End If
        End Sub

        Private Sub cmdClearSelected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClearSelected.Click
            setUrlCookieInfo(PortalId, Request)
            WishList.ClearList(PortalId)
            Response.Redirect(EditUrl("ProdId", ProdID.ToString, "AdminProduct", getUrlCookieInfo(PortalId, "")), True)
        End Sub

        Private Sub cmdSelectRelated_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelectRelated.Click
            Dim objPInfo As NB_Store_ProductsInfo
            objPInfo = updateProduct()
            If Not objPInfo Is Nothing Then
                setUrlCookieInfo(PortalId, Request)
                Response.Redirect(EditUrl("ProdId", objPInfo.ProductID.ToString, "AdminProduct", "rel=1"), True)
            End If
        End Sub

        Private Sub cmdAddSelected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddSelected.Click
            'related product select using the Wishlist has been disabled in v2.1 to make it more simple. 
            RelatedProducts.AddProductFromWishList(PortalId, ProdID)
            Response.Redirect(EditUrl("ProdId", ProdID.ToString, "AdminProduct", getUrlCookieInfo(PortalId, "")), True)
        End Sub

#End Region

#Region "productlist Events"

        Private Sub productlist_AddButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles productlist.AddButton
            setUrlCookieInfo(PortalId, Request)
            Response.Redirect(EditUrl("ProdId", "0", "AdminProduct"), True)
        End Sub

        Private Sub productlist_AddRelated(ByVal ProductID As Integer, ByVal RelatedID As Integer) Handles productlist.AddRelated
            RelatedProducts.AddProduct(PortalId, ProductID, RelatedID)
            Response.Redirect(EditUrl("ProdId", ProductID.ToString, "AdminProduct", getUrlCookieInfo(PortalId, "")), True)
        End Sub

        Private Sub productlist_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles productlist.EditCommand
            Dim ItemId As String = e.CommandArgument.ToString
            setUrlCookieInfo(PortalId, Request)
            Response.Redirect(EditUrl("ProdId", ItemId, "AdminProduct", "currentpage=" & _CurrentPage))
        End Sub

        Private Sub productlist_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles productlist.ItemCommand
            Response.Redirect(EditUrl("AdminProduct"), True)
        End Sub

        Private Sub productlist_ReturnButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles productlist.ReturnButton
            Response.Redirect(EditUrl("ProdId", ProdID.ToString, "AdminProduct", getUrlCookieInfo(PortalId, "")), True)
        End Sub



#End Region

#Region "Category DualList"

        Private Sub populateDualList(ByVal ProdID As Integer)
            If populateCategoryDualList(PortalSettings.PortalId, ProdID, DirectCast(dlCategories, DotNetNuke.UI.UserControls.DualListControl)) = 0 Then
                cmdUpdate.Visible = False
            End If
        End Sub

        Private Sub updateDualList(ByVal objInfo As NB_Store_ProductsInfo)
            Dim objI As ListItem
            Dim objCtrl As New ProductController

            objCtrl.DeleteProductCategory(objInfo.ProductID)
            For Each objI In DirectCast(dlCategories, DotNetNuke.UI.UserControls.DualListControl).Assigned
                objCtrl.UpdateProductCategory(objInfo.ProductID, CInt(objI.Value))
            Next

        End Sub

#End Region

#Region "productimage events"
        Private Sub productimage_AddButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles productimage.AddButton
            Dim ImageSize As Integer = 450
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo

            updateProduct()

            objSInfo = objSCtrl.GetSetting(PortalId, "image.resize", "None")
            If Not objSInfo Is Nothing Then
                If IsNumeric(objSInfo.SettingValue) Then
                    If CInt(objSInfo.SettingValue) >= 50 Then
                        ImageSize = CInt(objSInfo.SettingValue)
                    Else
                        ImageSize = 0
                    End If
                End If
            End If

            Dim ImageQuality As Integer = 85
            objSInfo = objSCtrl.GetSetting(PortalId, "image.quality", "None")
            If Not objSInfo Is Nothing Then
                If IsNumeric(objSInfo.SettingValue) Then
                    ImageQuality = CInt(objSInfo.SettingValue)
                End If
            End If

            Dim InterpolationMode As Integer = 7
            Dim SmoothingMode As Integer = 2
            Dim PixelOffsetMode As Integer = 0
            Dim CompositingQuality As Integer = 0
            Dim ImageAdvanced() As String
            objSInfo = objSCtrl.GetSetting(PortalId, "image.advanced", "None")
            If Not objSInfo Is Nothing Then
                ImageAdvanced = objSInfo.SettingValue.Split(","c)
                If ImageAdvanced.GetUpperBound(0) = 3 Then
                    If IsNumeric(ImageAdvanced(0)) Then InterpolationMode = CInt(ImageAdvanced(0))
                    If IsNumeric(ImageAdvanced(1)) Then SmoothingMode = CInt(ImageAdvanced(1))
                    If IsNumeric(ImageAdvanced(2)) Then PixelOffsetMode = CInt(ImageAdvanced(2))
                    If IsNumeric(ImageAdvanced(3)) Then CompositingQuality = CInt(ImageAdvanced(3))
                End If
            End If

            productimage.UploadImage(ProdID, selectlang.SelectedLang, ImageSize, ImageQuality, InterpolationMode, SmoothingMode, PixelOffsetMode, CompositingQuality)

            ReturnToProduct()
        End Sub

        Private Sub productimage_DeleteImage() Handles productimage.DeleteImage
            'productimage.populateImages(ProdID, selectlang.SelectedLang)
            updateProduct()
            ReturnToProduct()
        End Sub


#End Region

#Region "productdoc events"

        Private Sub productdoc_AddButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles productdoc.AddButton
            updateProduct()
            productdoc.UploadDoc(ProdID, selectlang.SelectedLang)
            'update shipping doc just been added so set doc count to 1. (Maybe more but true/false test in shipping update)
            UpdateShippingData(ProdID, 1)
            ReturnToProduct()
        End Sub

        Private Sub productdoc_DeleteDoc(ByVal DocID As Integer) Handles productdoc.DeleteDoc
            'productdoc.populateDocs(ProdID, selectlang.SelectedLang)
            'update shipping so zero weight can be added to model or shipping deleted if zero with no documents.
            UpdateShippingData(ProdID)
            updateModels()
            ReturnToProduct()
        End Sub

        Private Sub productdoc_SearchDocs(ByVal FilterText As String) Handles productdoc.SearchDocs
            If FilterText = "" Then
                productdoc.HideSelectList()
            Else
                productdoc.populateSelectDocs(selectlang.SelectedLang, FilterText)
            End If
        End Sub

        Private Sub productdoc_SelectDoc(ByVal FileName As String, ByVal DocDesc As String, ByVal DocPath As String, ByVal FileExt As String) Handles productdoc.SelectDoc
            Dim objCtrl As New ProductController
            Dim objDInfo As New NB_Store_ProductDocInfo

            If DocDesc = "&nbsp;" Then DocDesc = ""

            objDInfo.DocID = -1
            objDInfo.DocDesc = DocDesc
            objDInfo.DocPath = DocPath
            objDInfo.FileExt = FileExt
            objDInfo.FileName = FileName
            objDInfo.Lang = selectlang.SelectedLang
            objDInfo.Hidden = False
            objDInfo.ListOrder = 1
            objDInfo.ProductID = ProdID

            objCtrl.UpdateObjProductDoc(objDInfo)

            productdoc.HideSelectList()
            productdoc.populateDocs(ProdID, selectlang.SelectedLang)

        End Sub

#End Region


        Private Sub cmdAddModel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddModel.Click
            Try
                Dim lp As Integer
                If IsNumeric(txtAddModels.Text) Then
                    For lp = 1 To CInt(txtAddModels.Text)
                        AddModel(ProdID)
                    Next
                    populateEdit()
                End If

                'If ProdID > 0 Then
                ' Response.Redirect(EditUrl("ProdId", ProdID.ToString, "AdminProduct", "vtyp", "1", DSEDITLOCAL, selectlang.SelectedLang), True)
                'End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dgModel_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgModel.DeleteCommand
            Dim item As DataGridItem = e.Item
            Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
            Dim objCtrl As New ProductController
            Dim objMInfo As NB_Store_ModelInfo
            objMInfo = objCtrl.GetModel(ItemId, GetCurrentCulture)
            If Not objMInfo Is Nothing Then
                If objMInfo.Deleted Then
                    'restore deleted
                    objMInfo.Deleted = False
                    objCtrl.UpdateObjModel(objMInfo)
                Else
                    'check if model exists in orders
                    If objCtrl.GetModelInOrders(ItemId) > 0 Then
                        objMInfo.QtyRemaining = 0
                        objMInfo.Deleted = True
                        objCtrl.UpdateObjModel(objMInfo)
                    Else
                        objCtrl.DeleteModel(PortalId, ItemId)
                    End If
                End If
            End If

            populateEdit()
            
        End Sub

        Private Sub dgModel_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgModel.PreRender
            Dim dg As DataGrid = sender
            If dg.Controls.Count > 0 Then
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("ShowSelectLang"), ShowSelectLang).Refresh()
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("ShowSelectLang2"), ShowSelectLang).Refresh()
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("nlName"), Label).Text = Localization.GetString("nlName", LocalResourceFile)
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("nlExtra"), Label).Text = Localization.GetString("nlExtra", LocalResourceFile)
            End If

        End Sub

        Private Sub dgModel_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgModel.ItemDataBound
            Dim item As DataGridItem = e.Item

            If item.ItemType = ListItemType.Item Or _
                item.ItemType = ListItemType.AlternatingItem Or _
                item.ItemType = ListItemType.SelectedItem Then

                'show or hide cart loow amount
                If GetStoreSettingBoolean(PortalId, "allowcartmodellimit.flag") Then
                    dgModel.Columns.Item(12).Visible = True
                Else
                    dgModel.Columns.Item(12).Visible = False
                End If


                Dim blnDel As Boolean = False

                blnDel = CType(e.Item.DataItem, NB_Store_ModelInfo).Deleted

                If blnDel Then
                    Dim dgtxt1 As TextBox = DirectCast(e.Item.FindControl("txtModelRef"), TextBox)
                    Dim dgtxt2 As TextBox = DirectCast(e.Item.FindControl("txtModelName"), TextBox)
                    Dim dgtxt3 As TextBox = DirectCast(e.Item.FindControl("txtBarCode"), TextBox)
                    Dim dgtxt4 As TextBox = DirectCast(e.Item.FindControl("txtWeight"), TextBox)
                    Dim dgtxt5 As TextBox = DirectCast(e.Item.FindControl("txtUnitCost"), TextBox)
                    Dim dgtxt6 As TextBox = DirectCast(e.Item.FindControl("txtQtyRemaining"), TextBox)
                    Dim dgtxt7 As TextBox = DirectCast(e.Item.FindControl("txtListOrder"), TextBox)
                    Dim dgval1 As CompareValidator = DirectCast(e.Item.FindControl("validatorWeight"), CompareValidator)
                    Dim dgval2 As CompareValidator = DirectCast(e.Item.FindControl("validatorUnitPrice"), CompareValidator)
                    Dim dgval3 As CompareValidator = DirectCast(e.Item.FindControl("validatorDealerCost"), CompareValidator)
                    Dim dgval4 As CompareValidator = DirectCast(e.Item.FindControl("validatorPurchaseCost"), CompareValidator)

                    Dim dgtxt8 As TextBox = DirectCast(e.Item.FindControl("txtMaxStock"), TextBox)
                    Dim dgtxt9 As TextBox = DirectCast(e.Item.FindControl("txtDealerCost"), TextBox)
                    Dim dgtxt10 As TextBox = DirectCast(e.Item.FindControl("txtPurchaseCost"), TextBox)
                    Dim dgtxt11 As TextBox = DirectCast(e.Item.FindControl("txtExtra"), TextBox)
                    Dim dgchk1 As CheckBox = DirectCast(e.Item.FindControl("chkDealerOnly"), CheckBox)


                    If Not dgtxt1 Is Nothing Then dgtxt1.Enabled = False
                    If Not dgtxt2 Is Nothing Then dgtxt2.Enabled = False
                    If Not dgtxt3 Is Nothing Then dgtxt3.Enabled = False
                    If Not dgtxt4 Is Nothing Then dgtxt4.Enabled = False
                    If Not dgtxt5 Is Nothing Then dgtxt5.Enabled = False
                    If Not dgtxt6 Is Nothing Then dgtxt6.Enabled = False
                    If Not dgtxt7 Is Nothing Then dgtxt7.Enabled = False
                    If Not dgtxt8 Is Nothing Then dgtxt8.Enabled = False
                    If Not dgtxt9 Is Nothing Then dgtxt9.Enabled = False
                    If Not dgtxt10 Is Nothing Then dgtxt10.Enabled = False
                    If Not dgtxt11 Is Nothing Then dgtxt11.Enabled = False
                    If Not dgval1 Is Nothing Then dgval1.Enabled = False
                    If Not dgval2 Is Nothing Then dgval2.Enabled = False
                    If Not dgval3 Is Nothing Then dgval3.Enabled = False
                    If Not dgval4 Is Nothing Then dgval4.Enabled = False
                    If Not dgchk1 Is Nothing Then dgchk1.Enabled = False

                    Dim imgColumnControl As Control = item.Controls(1).Controls(0)
                    If TypeOf imgColumnControl Is ImageButton Then
                        Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                        If item.ItemIndex = 0 Then
                            remImage.Visible = False
                        Else
                            remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdRestoreModel", LocalResourceFile) & "');")
                            remImage.ImageUrl = "/images/restore.gif"
                            remImage.ToolTip = Localization.GetString("tipRestoreModel", LocalResourceFile)
                        End If
                    End If


                Else
                    Dim dgtxtUnitCost As TextBox = DirectCast(e.Item.FindControl("txtUnitCost"), TextBox)
                    Dim dgtxtDealerCost As TextBox = DirectCast(e.Item.FindControl("txtDealerCost"), TextBox)
                    Dim dgtxtPurchaseCost As TextBox = DirectCast(e.Item.FindControl("txtPurchaseCost"), TextBox)

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

                    If Not dgtxtUnitCost Is Nothing Then
                        If IsNumeric(dgtxtUnitCost.Text) Then
                            dgtxtUnitCost.Text = CDbl(dgtxtUnitCost.Text).ToString
                        End If
                    End If

                    Dim imgColumnControl As Control = item.Controls(1).Controls(0)
                    If TypeOf imgColumnControl Is ImageButton Then
                        Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                        If item.ItemIndex = 0 Then
                            remImage.Visible = False
                        Else
                            remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdDeleteModel", LocalResourceFile) & "');")
                            remImage.ToolTip = Localization.GetString("tipDeleteModel", LocalResourceFile)
                        End If
                    End If
                End If

            End If

        End Sub

        Private Sub dgRelated_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRelated.ItemDataBound
            Dim item As DataGridItem = e.Item

            If item.ItemType = ListItemType.Item Or _
                item.ItemType = ListItemType.AlternatingItem Or _
                item.ItemType = ListItemType.SelectedItem Then

                Dim imgColumnControl As Control = item.Controls(3).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                    remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdDelete", LocalResourceFile) & "');")
                    remImage.ToolTip = Localization.GetString("cmdDelete", LocalResourceFile)
                End If

                Dim ctrl As Control = item.Controls(10).Controls(1)
                If TypeOf ctrl Is DropDownList Then
                    Dim ddl As DropDownList = CType(ctrl, DropDownList)
                    ddl.SelectedValue = CType(e.Item.DataItem, NB_Store_ProductRelatedListInfo).RelatedType
                End If

                If CType(e.Item.DataItem, NB_Store_ProductRelatedListInfo).NotAvailable Then
                    item.Controls(6).Visible = False
                    item.Controls(7).Visible = False
                    item.Controls(8).Visible = False
                    item.Controls(9).Visible = False
                    item.Controls(10).Visible = False
                    item.Controls(11).Visible = False
                    item.Controls(12).Visible = False
                End If

            End If

        End Sub

        Private Sub cmdAddOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddOption.Click
            Try
                Dim lp As Integer
                If IsNumeric(txtAddOption.Text) Then
                    For lp = 1 To txtAddOption.Text
                        AddOption(ProdID)
                    Next
                    populateEdit()
                End If
                'If ProdID > 0 Then
                ' Response.Redirect(EditUrl("ProdId", ProdID.ToString, "AdminProduct", "vtyp", "1", DSEDITLOCAL, selectlang.SelectedLang), True)
                'End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdAddOptionValue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddOptionValue.Click
            Try
                Dim lp As Integer
                If IsNumeric(txtAddOptionValue.Text) Then
                    If IsNumeric(lblSelectedOptionID.Text) Then
                        Dim OptionID As Integer = CInt(lblSelectedOptionID.Text)
                        For lp = 1 To txtAddOptionValue.Text
                            AddOptionValue(OptionID)
                        Next
                        populateEdit()
                        populateOptionEdit(OptionID)
                        optionvaluesdiv.Visible = True
                    End If
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub dgOption_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgOption.DeleteCommand
            Try
                Dim item As DataGridItem = e.Item
                Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
                Dim objCtrl As New ProductController
                objCtrl.DeleteOption(ItemId)
                populateEdit()
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub dgOption_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgOption.EditCommand
            Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
            dgOption.SelectedIndex = e.Item.ItemIndex
            lblSelectedOptionID.Text = ItemId.ToString
            updateProduct()
            populateEdit()
            populateOptionEdit(ItemId)
            optionvaluesdiv.Visible = True
        End Sub

        Private Sub dgOption_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgOption.ItemDataBound
            Dim item As DataGridItem = e.Item

            If item.ItemType = ListItemType.Item Or _
                item.ItemType = ListItemType.AlternatingItem Or _
                item.ItemType = ListItemType.SelectedItem Then

                Dim imgColumnControl As Control = item.Controls(2).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                    remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdDeleteOption", LocalResourceFile) & "');")
                End If

            End If
        End Sub

        Private Sub dgOption_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgOption.PreRender
            Dim dg As DataGrid = sender

            If dg.Controls.Count > 0 Then
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("ShowSelectLang"), ShowSelectLang).Refresh()
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("nlName"), Label).Text = Localization.GetString("nlName", LocalResourceFile)
            End If

        End Sub


        Private Sub dgOptionEdit_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgOptionEdit.DeleteCommand
            Try
                Dim item As DataGridItem = e.Item
                Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
                Dim objCtrl As New ProductController
                objCtrl.DeleteOptionValue(ItemId)
                Dim OptionID As Integer = CInt(item.Cells(1).Text)
                populateEdit()
                populateOptionEdit(OptionID)
                optionvaluesdiv.Visible = True

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub dgOptionEdit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgOptionEdit.ItemDataBound
            Dim item As DataGridItem = e.Item

            If item.ItemType = ListItemType.Item Or _
                item.ItemType = ListItemType.AlternatingItem Or _
                item.ItemType = ListItemType.SelectedItem Then

                Dim dgtxtAddedCost As TextBox = DirectCast(e.Item.FindControl("txtAddedCost"), TextBox)

                If Not dgtxtAddedCost Is Nothing Then
                    If IsNumeric(dgtxtAddedCost.Text) Then
                        dgtxtAddedCost.Text = CDbl(dgtxtAddedCost.Text).ToString
                    End If
                End If

                Dim imgColumnControl As Control = item.Controls(2).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                    remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdDeleteOption", LocalResourceFile) & "');")
                End If

            End If

        End Sub

        Private Sub dgOptionEdit_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgOptionEdit.PreRender
            Dim dg As DataGrid = sender

            If dg.Controls.Count > 0 Then
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("ShowSelectLang"), ShowSelectLang).Refresh()
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("nlName"), Label).Text = Localization.GetString("nlName", LocalResourceFile)
            End If

        End Sub

        Private Sub selectlang_AfterChange(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs, ByVal PreviousEditLang As String) Handles selectlang.AfterChange
            Select Case e.CommandName
                Case "Change"
                    populateEdit()
            End Select
        End Sub

        Private Sub selectlang_BeforeChange(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs, ByVal NewEditLang As String) Handles selectlang.BeforeChange
            Select Case e.CommandName
                Case "Change"
                    Dim objInfo As New NB_Store_ProductsInfo
                    objInfo = updateProduct()
                    If ProdID = 0 Then
                        'new entry created, so reset the url
                        selectlang.SelectedLang = NewEditLang
                        Response.Redirect(EditUrl("ProdId", objInfo.ProductID.ToString, "AdminProduct"))
                    End If
            End Select
        End Sub

        Private Sub dgRelated_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRelated.DeleteCommand
            Dim item As DataGridItem = e.Item
            Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
            Dim objCtrl As New ProductController
            objCtrl.DeleteProductRelated(ItemId)
            populateEdit()
        End Sub

#End Region


#Region "Methods"

        Private Sub populateList()
            pnlProduct.Visible = False
            pnlModel.Visible = False
            pnlLang.Visible = False
            pnlImageCtrl.Visible = False
            pnlDocCtrl.Visible = False
            pnlOptions.Visible = False
            pnlList.Visible = True
            pnlProductTabs.Visible = False
            pnlRelated.Visible = False
            productlist.populateList()
        End Sub

        Private Function updateProduct() As NB_Store_ProductsInfo
            Dim objCtrl As New ProductController
            Dim objInfo As NB_Store_ProductsInfo
            Dim objCurrentInfo As NB_Store_ProductsInfo
            Dim CurrentDeleted As Boolean = False
            Dim CurrentArchived As Boolean = False

            If ProdID > 0 Then
                objCurrentInfo = objCtrl.GetProduct(ProdID, selectlang.SelectedLang)
                CurrentDeleted = objCurrentInfo.IsDeleted
                CurrentArchived = objCurrentInfo.Archived
            End If

            CreateDir(PortalSettings, PRODUCTIMAGESFOLDER)
            CreateDir(PortalSettings, PRODUCTDOCSFOLDER)

            objInfo = productdetail.UpdateDetails(selectlang.SelectedLang, UserId, ProdID)

            updateDualList(objInfo)

            If ProdID = 0 Then
                'new product being created, created models automatically
                objCtrl.AddNewModel(objInfo)
            Else
                updateModels()
                updateOptions()
                updateOptionValues()
                productimage.updateImages(selectlang.SelectedLang)
                productdoc.updateDocs(selectlang.SelectedLang)
                updateRelated()
            End If

            'copy to languages not created yet
            objCtrl.CopyProductToLanguages(objInfo, False)

            'If product has been deleted, check if exists in any order
            If objInfo.IsDeleted Then
                If objCtrl.GetProductInOrders(objInfo.ProductID) = 0 Then
                    '    If not delete and redirect
                    objCtrl.DeleteProduct(objInfo.ProductID)
                    If _RtnTabID > 0 Then
                        Response.Redirect(NavigateURL(_RtnTabID, "", "currentpage=" & _PageIndex.ToString, "CatID=" & _CatID.ToString), True)
                    Else
                        Response.Redirect(EditUrl("AdminProduct"), True)
                    End If
                Else
                    If Not CurrentDeleted Then
                        'Make the related product not available
                        objCtrl.NotAvailableProductRelated(objInfo.ProductID, True)
                    End If
                End If
            Else
                If CurrentDeleted Then
                    'Make the related product available
                    objCtrl.NotAvailableProductRelated(objInfo.ProductID, False)
                End If
            End If

            If objInfo.Archived Then
                If Not CurrentArchived Then
                    'Make the related product not available
                    objCtrl.NotAvailableProductRelated(objInfo.ProductID, True)
                End If
            Else
                If CurrentArchived Then
                    'Make the related product available
                    objCtrl.NotAvailableProductRelated(objInfo.ProductID, False)
                End If
            End If


            'clear any cached objects for product
            ClearProductCache(PortalId, objInfo.ProductID)


            Return objInfo
        End Function

        Private Sub updateModels()
            Dim objCtrl As New ProductController
            Dim objInfo As NB_Store_ModelInfo
            Dim objPromoCtrl As New PromoController
            Dim i As DataGridItem
            Dim ModelID As Integer

            For Each i In dgModel.Items
                ModelID = CInt(i.Cells(0).Text)
                objInfo = objCtrl.GetModel(ModelID, selectlang.SelectedLang)
                If Not objInfo Is Nothing Then
                    objInfo.Barcode = CType(i.FindControl("txtBarCode"), TextBox).Text
                    objInfo.ModelName = CType(i.FindControl("txtModelName"), TextBox).Text
                    objInfo.Extra = CType(i.FindControl("txtExtra"), TextBox).Text
                    objInfo.ModelRef = CType(i.FindControl("txtModelRef"), TextBox).Text

                    If IsNumeric(CType(i.FindControl("txtDealerCost"), TextBox).Text) Then
                        objInfo.DealerCost = CType(CType(i.FindControl("txtDealerCost"), TextBox).Text, Decimal)
                    End If

                    If IsNumeric(CType(i.FindControl("txtPurchaseCost"), TextBox).Text) Then
                        objInfo.PurchaseCost = CType(CType(i.FindControl("txtPurchaseCost"), TextBox).Text, Decimal)
                    End If

                    objInfo.DealerOnly = CType(i.FindControl("chkDealerOnly"), CheckBox).Checked
                    objInfo.QtyStockSet = CInt(CType(i.FindControl("txtMaxStock"), TextBox).Text)

                    If IsNumeric(CType(i.FindControl("txtQtyRemaining"), TextBox).Text) Then
                        objInfo.QtyRemaining = CInt(CType(i.FindControl("txtQtyRemaining"), TextBox).Text)
                        If objInfo.QtyRemaining > objInfo.QtyStockSet Then
                            objInfo.QtyStockSet = CInt(CType(i.FindControl("txtQtyRemaining"), TextBox).Text)
                        End If
                    End If

                    If IsNumeric(CType(i.FindControl("txtAllow"), TextBox).Text) Then
                        objInfo.Allow = CInt(CType(i.FindControl("txtAllow"), TextBox).Text)
                    End If

                    If IsNumeric(CType(i.FindControl("txtUnitCost"), TextBox).Text) Then
                        objInfo.UnitCost = CType(CType(i.FindControl("txtUnitCost"), TextBox).Text, Decimal)
                        If objInfo.UnitCost > 0 And objInfo.DealerCost = 0 And Not GetStoreSettingBoolean(PortalId, "allowzerodealerprice.flag") Then
                            'make sure we've got a value in dealercost
                            objInfo.DealerCost = objInfo.UnitCost
                        End If
                    End If


                    objInfo.Lang = selectlang.SelectedLang
                    objInfo.ListOrder = CType(i.FindControl("txtListOrder"), TextBox).Text
                    objCtrl.UpdateObjModel(objInfo)

                    'update Shipping data
                    If IsNumeric(CType(i.FindControl("txtWeight"), TextBox).Text) Then
                        UpdateShippingData(objInfo, CType(CType(i.FindControl("txtWeight"), TextBox).Text, Decimal), productdoc.DocsCount)
                    End If

                    'update the sales rate cache table
                    objPromoCtrl.createSalePriceTable(PortalId, ModelID)
                End If
            Next
        End Sub

        Private Sub updateRelated()
            Dim objCtrl As New ProductController
            Dim objInfo As NB_Store_ProductRelatedInfo
            Dim i As DataGridItem
            Dim RelatedID As Integer

            For Each i In dgRelated.Items
                RelatedID = CInt(i.Cells(0).Text)
                objInfo = objCtrl.GetProductRelated(RelatedID)
                If Not objInfo Is Nothing Then
                    objInfo.BiDirectional = CType(i.FindControl("chkBiDirectional"), CheckBox).Checked
                    objInfo.Disabled = Not CType(i.FindControl("chkEnabled"), CheckBox).Checked
                    If IsNumeric(CType(i.FindControl("txtDiscountAmt"), TextBox).Text) Then
                        objInfo.DiscountAmt = CType(i.FindControl("txtDiscountAmt"), TextBox).Text
                    End If
                    If IsNumeric(CType(i.FindControl("txtDiscountPercent"), TextBox).Text) Then
                        objInfo.DiscountPercent = CType(i.FindControl("txtDiscountPercent"), TextBox).Text
                    End If
                    If IsNumeric(CType(i.FindControl("txtMaxQty"), TextBox).Text) Then
                        objInfo.MaxQty = CType(i.FindControl("txtMaxQty"), TextBox).Text
                    End If
                    If IsNumeric(CType(i.FindControl("txtProductQty"), TextBox).Text) Then
                        objInfo.ProductQty = CType(i.FindControl("txtProductQty"), TextBox).Text
                    End If
                    objInfo.RelatedType = CType(i.FindControl("ddlRelatedType"), DropDownList).SelectedValue
                    objCtrl.UpdateObjProductRelated(objInfo)
                End If
            Next
        End Sub



        Private Sub UpdateShippingData(ByVal ProductID As Integer)
            'this function has been created so that a zero weight can be added for a model, when a downloadable product may be used.
            Dim objCtrl As New ProductController
            Dim aryList As ArrayList

            aryList = objCtrl.GetProductDocList(ProductID, GetCurrentCulture)

            UpdateShippingData(ProductID, aryList.Count)

        End Sub

        Private Sub UpdateShippingData(ByVal ProductID As Integer, ByVal ProductDocCount As Integer)
            'this function has been created so that a zero weight can be added for a model, when a downloadable product may be used.
            Dim objCtrl As New ProductController
            Dim objMInfo As NB_Store_ModelInfo
            Dim aryList As ArrayList

            aryList = objCtrl.GetModelList(PortalId, ProductID, GetCurrentCulture, True)
            For Each objMInfo In aryList
                UpdateShippingData(objMInfo, objMInfo.Weight, ProductDocCount)
            Next
        End Sub

        Private Sub UpdateShippingData(ByVal objMInfo As NB_Store_ModelInfo, ByVal NewWeight As Decimal, ByVal ProductDocCount As Integer)
            'update Shipping data
            Dim objSCtrl As New ShipController
            Dim objSInfo As NB_Store_ShippingRatesInfo
            Dim blnUpdateFlg As Boolean = False
            objSInfo = objSCtrl.GetShippingRateByObjID(PortalId, objMInfo.ModelID, "PRD", -1)

            'check if any needed values are included in the sippig record, if so we need to keep it
            If Not objSInfo Is Nothing Then
                If objSInfo.ShipCost > 0 Or objSInfo.ProductLength > 0 Or objSInfo.ProductWidth > 0 Or objSInfo.ProductHeight > 0 Then
                    blnUpdateFlg = True
                End If
            End If
            If NewWeight > 0 Or ProductDocCount > 0 Then
                blnUpdateFlg = True
            End If

            If blnUpdateFlg Then
                If objSInfo Is Nothing Then
                    objSInfo = New NB_Store_ShippingRatesInfo
                    objSInfo.ItemId = 0
                    objSInfo.Description = objMInfo.ModelRef & " - " & objMInfo.ModelName
                    objSInfo.Disable = False
                    objSInfo.ObjectId = objMInfo.ModelID
                    objSInfo.PortalID = PortalId
                    objSInfo.ProductHeight = 0
                    objSInfo.ProductLength = 0
                    objSInfo.ProductWeight = NewWeight
                    objSInfo.ProductWidth = 0
                    objSInfo.Range1 = 0
                    objSInfo.Range2 = 0
                    objSInfo.ShipCost = 0
                    objSInfo.ShipType = "PRD"
                    objSInfo.ShipMethodID = -1
                Else
                    objSInfo.ProductWeight = NewWeight
                End If
                objSCtrl.UpdateObjShippingRate(objSInfo)
            Else
                If Not objSInfo Is Nothing Then
                    objSCtrl.DeleteShippingRate(objSInfo.ItemId)
                End If
            End If

        End Sub

        Private Sub AddModel(ByVal ProdID As Integer)
            Dim objCtrl As New ProductController
            Dim objProductInfo As New NB_Store_ProductsInfo
            objProductInfo = objCtrl.GetProduct(ProdID, selectlang.SelectedLang)
            If Not objProductInfo Is Nothing Then
                objCtrl.AddNewModel(objProductInfo)
            End If
        End Sub

        Private Sub AddOption(ByVal ProdID As Integer)
            Dim objCtrl As New ProductController
            Dim objProductInfo As New NB_Store_ProductsInfo
            objProductInfo = objCtrl.GetProduct(ProdID, selectlang.SelectedLang)
            If Not objProductInfo Is Nothing Then
                objCtrl.AddNewOption(objProductInfo)
            End If
        End Sub

        Private Sub AddOptionValue(ByVal OptionID As Integer)
            Dim objCtrl As New ProductController
            Dim objOptInfo As New NB_Store_OptionInfo
            objOptInfo = objCtrl.GetOption(OptionID, selectlang.SelectedLang)
            If Not objOptInfo Is Nothing Then
                objCtrl.AddNewOptionValue(objOptInfo)
            End If
        End Sub

        Private Sub updateOptions()
            Dim objCtrl As New ProductController
            Dim objInfo As NB_Store_OptionInfo
            Dim i As DataGridItem
            Dim OptionID As Integer

            If dgOption.Visible Then
                For Each i In dgOption.Items
                    OptionID = CInt(i.Cells(0).Text)
                    objInfo = objCtrl.GetOption(OptionID, selectlang.SelectedLang)
                    If Not objInfo Is Nothing Then
                        objInfo.OptionDesc = CType(i.FindControl("txtOptionDesc"), TextBox).Text
                        objInfo.ListOrder = CType(i.FindControl("txtListOrder"), TextBox).Text
                        objInfo.Lang = selectlang.SelectedLang
                        objInfo.Attributes = "" ' create for attributes, but not used.
                        objCtrl.UpdateObjOption(objInfo)
                    End If
                Next
            End If
        End Sub

        Private Sub updateOptionValues()
            Dim objCtrl As New ProductController
            Dim objInfo As NB_Store_OptionValueInfo
            Dim i As DataGridItem
            Dim OptionValueID As Integer

            If dgOptionEdit.Visible Then
                For Each i In dgOptionEdit.Items
                    OptionValueID = CInt(i.Cells(0).Text)
                    objInfo = objCtrl.GetOptionValue(OptionValueID, selectlang.SelectedLang)
                    If Not objInfo Is Nothing Then
                        objInfo.OptionValueDesc = CType(i.FindControl("txtOptionValueDesc"), TextBox).Text
                        If IsNumeric(CType(i.FindControl("txtAddedCost"), TextBox).Text) Then
                            objInfo.AddedCost = CType(i.FindControl("txtAddedCost"), TextBox).Text
                        End If
                        objInfo.Lang = selectlang.SelectedLang
                        objInfo.ListOrder = CType(i.FindControl("txtListOrder"), TextBox).Text
                        objCtrl.UpdateObjOptionValue(objInfo)
                    End If
                Next
            End If

        End Sub


        Private Sub populateEdit()
            Dim objCtrl As New ProductController
            Dim objInfo As New NB_Store_ProductsInfo

            pnlList.Visible = False
            pnlImageCtrl.Visible = True
            pnlLang.Visible = True
            pnlModel.Visible = True
            pnlOptions.Visible = True
            pnlProduct.Visible = True
            optionvaluesdiv.Visible = False
            pnlDocCtrl.Visible = True
            pnlRelated.Visible = True
            dgOption.SelectedIndex = -1

            If IsOnlyManagerLite(PortalId, UserInfo) Then
                pnlRelated.Visible = False
                pnlOptions.Visible = False
            End If

            objInfo = objCtrl.GetProduct(ProdID, selectlang.SelectedLang)
            If objInfo Is Nothing Then
                'culture may not be there, if not get currentculture
                objInfo = objCtrl.GetProduct(ProdID, GetCurrentCulture)
                selectlang.SelectedLang = GetCurrentCulture()
            End If
            If Not objInfo Is Nothing Then

                phPadding.Controls.Add(New LiteralControl("<div class=""NBright_NameHeader"">" & objInfo.ProductName & "</div>"))

                productdetail.populateDetails(objInfo)

                populateDualList(objInfo.ProductID)

                populateModel(objInfo.ProductID)
                populateOptions(objInfo.ProductID)

                productimage.populateImages(objInfo.ProductID, selectlang.SelectedLang)

                productdoc.populateDocs(objInfo.ProductID, selectlang.SelectedLang)

                populateRelated(objInfo.ProductID)

            Else
                ProdID = 0
                'create a blank entry to populate duellist of categories 
                objInfo = New NB_Store_ProductsInfo
                objInfo.PortalID = PortalId
                objInfo.Lang = GetCurrentCulture()
                objInfo.ProductID = -1
                productdetail.populateDetails(objInfo)
                populateDualList(-1)
                'new product hide all product extras
                pnlModel.Visible = False
                pnlImageCtrl.Visible = False
                pnlOptions.Visible = False
                pnlDocCtrl.Visible = False
                pnlRelated.Visible = False
            End If

        End Sub

        Private Sub ShowDocUpload()

            pnlList.Visible = False
            pnlLang.Visible = False
            pnlModel.Visible = False
            pnlOptions.Visible = False
            pnlProduct.Visible = False
            pnlImageCtrl.Visible = False
            optionvaluesdiv.Visible = False
            pnlDocCtrl.Visible = True
            
            productdoc.populateSelectDocs(selectlang.SelectedLang, "")

        End Sub

        Private Sub populateModel(ByVal ProductID As Integer)
            Dim objCtrl As New ProductController

            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgModel, LocalResourceFile)

            ' get content
            Dim aryList As ArrayList

            aryList = objCtrl.GetModelList(PortalId, ProductID, selectlang.SelectedLang, True)

            dgModel.DataSource = aryList
            dgModel.DataBind()

        End Sub

        Private Sub populateOptions(ByVal ProductID As Integer)
            Dim objCtrl As New ProductController

            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgOption, LocalResourceFile)

            ' get content
            Dim aryList As ArrayList

            aryList = objCtrl.GetOptionList(ProductID, selectlang.SelectedLang)

            dgOption.DataSource = aryList
            dgOption.DataBind()

        End Sub

        Private Sub populateOptionEdit(ByVal OptionID As Integer)
            Dim objCtrl As New ProductController

            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgOptionEdit, LocalResourceFile)

            ' get content
            Dim aryList As ArrayList

            aryList = objCtrl.GetOptionValueList(OptionID, selectlang.SelectedLang)

            dgOptionEdit.DataSource = aryList
            dgOptionEdit.DataBind()
        End Sub


        Private Sub populateRelated(ByVal ProductID As Integer)
            Dim objCtrl As New ProductController
            Dim strMsg As String
            Dim WishCount As Integer

            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgRelated, LocalResourceFile)

            WishCount = WishList.GetItemCountInt(PortalId)

            If WishCount = 0 Then
                cmdAddSelected.Visible = False
                cmdClearSelected.Visible = False
            Else
                strMsg = Localization.GetString("cmdAddSelected", LocalResourceFile)
                strMsg = Replace(strMsg, "[COUNT]", WishCount.ToString)
                cmdAddSelected.Text = strMsg
            End If


            ' get content
            Dim aryList As ArrayList

            aryList = objCtrl.GetProductRelatedList(PortalId, ProductID, selectlang.SelectedLang, -1, 1)

            dgRelated.DataSource = aryList
            dgRelated.DataBind()

        End Sub

        Private Sub copyLang(ByVal ForceOverwrite As Boolean)
            Dim objCtrl As New ProductController
            Dim objInfo As NB_Store_ProductsInfo

            'copy products
            objInfo = updateProduct()
            If Not objInfo Is Nothing Then
                objCtrl.CopyProductToLanguages(objInfo, ForceOverwrite)
            End If

        End Sub

        Private Function ReachedProductLimit() As Boolean
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo

            objSInfo = objSCtrl.GetSetting(PortalId, "product.limit", "None")
            If Not objSInfo Is Nothing Then
                If IsNumeric(objSInfo.SettingValue) Then
                    If CInt(objSInfo.SettingValue) >= 1 Then
                        Dim objPCtrl As New ProductController
                        If objPCtrl.GetProductCount(PortalId) >= CInt(objSInfo.SettingValue) Then
                            DisplayMsgText(PortalId, plhMsg, "productlimit.text", "Product Limit Reached")
                            pnlImageCtrl.Visible = False
                            pnlDocCtrl.Visible = False
                            pnlLang.Visible = False
                            pnlList.Visible = False
                            pnlModel.Visible = False
                            pnlProduct.Visible = False
                            pnlOptions.Visible = False
                            Return True
                        End If
                    End If
                End If
            End If
            Return False
        End Function


        Private Sub ReturnToProduct()
            If _RtnTabID > -1 Then
                Response.Redirect(EditUrl("ProdId", ProdID.ToString, "AdminProduct", "RtnTab=" & _RtnTabID, "PageIndex=" & _PageIndex, "CatID=" & _CatID), True)
            Else
                Response.Redirect(EditUrl("ProdId", ProdID.ToString, "AdminProduct"), True)
            End If
        End Sub


#End Region



    End Class

End Namespace
