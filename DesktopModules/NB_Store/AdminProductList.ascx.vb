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

    Partial Public Class AdminProductList
        Inherits Framework.UserControlBase

#Region "Private Members"

        Protected ProdID As Integer = -1
        Private ResourceFile As String
        Private _CurrentPage As Integer = 1
        Private _ctl As String = ""
        Private _mid As String = ""
        Private _SkinSrc As String = ""
        Protected _CatID As Integer = -1
        Protected _Related As Boolean = False
        Private _Parent As NEvoWeb.Modules.NB_Store.BaseAdmin = Nothing

#End Region

        Public Event AddButton(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event ReturnButton(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Public Event EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Public Event AddRelated(ByVal ProductID As Integer, ByVal RelatedID As Integer)

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            _Parent = CType(Me.Parent.Parent, NEvoWeb.Modules.NB_Store.BaseAdmin)

            ProdID = -1
            If Not (Request.QueryString("ProdID") Is Nothing) Then
                If IsNumeric(Request.QueryString("ProdID")) Then
                    ProdID = CInt(Request.QueryString("ProdID"))
                End If
            End If

            _CurrentPage = 1
            If Not (Request.QueryString("currentpage") Is Nothing) Then
                If IsNumeric(Request.QueryString("currentpage")) Then
                    _CurrentPage = Request.QueryString("currentpage")
                End If
            End If

            _CatID = -1
            If Not (Request.QueryString("CatID") Is Nothing) Then
                If IsNumeric(Request.QueryString("CatID")) Then
                    _CatID = CInt(Request.QueryString("CatID"))
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

            _Related = False
            If Not (Request.QueryString("rel") Is Nothing) Then
                If Request.QueryString("rel") = "1" Then
                    _Related = True
                End If
            End If

            If _SkinSrc.EndsWith("Edit") Or _Related Then
                'not displying menu, so pad it a little.
                phPadding.Controls.Add(New LiteralControl("<br/><br/><br/>"))
            End If


            Dim strI As String = GetStoreSettingText(_Parent.PortalId, "adminproductitem.template", GetCurrentCulture)
            Dim strA As String = GetStoreSettingText(_Parent.PortalId, "adminproductalt.template", GetCurrentCulture)
            Dim ThumbSize As String = GetStoreSetting(_Parent.PortalId, "adminproduct.thumbsize", GetCurrentCulture)
            If strI <> "" And strA <> "" Then
                dlProducts.ItemTemplate = New ProductTemplate(_Parent.TabId, _Parent.ModuleId, DotNetNuke.Common.ResolveUrl(_Parent.AppRelativeTemplateSourceDirectory), ThumbSize, Server.HtmlDecode(strI), True, "", "", _CurrentPage, _CatID, 100, "", _Parent.TabId, _Parent.UserId, _Parent.UserInfo)
                dlProducts.AlternatingItemTemplate = New ProductTemplate(_Parent.TabId, _Parent.ModuleId, DotNetNuke.Common.ResolveUrl(_Parent.AppRelativeTemplateSourceDirectory), ThumbSize, Server.HtmlDecode(strA), True, "", "", _CurrentPage, _CatID, 100, "", _Parent.TabId, _Parent.UserId, _Parent.UserInfo)
            End If

        End Sub

        Public Sub HideAddButton()
            cmdAdd.Visible = False
        End Sub

        Public Sub populateList()
            Dim objCtrl As New ProductController
            Dim PgSize As Integer = 25
            Dim ListSize As Integer = 0

            If IsNumeric(GetStoreSetting(_Parent.PortalId, "adminproduct.pagesize", GetCurrentCulture)) Then
                PgSize = GetStoreSetting(_Parent.PortalId, "adminproduct.pagesize", GetCurrentCulture)
            End If

            ResourceFile = Services.Localization.Localization.GetResourceFile(Me, Me.GetType().BaseType.Name + ".ascx")
            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgProducts, ResourceFile)

            If _CatID = -1 Then
                Dim strTemp As String = getStoreCookieValue(PortalSettings.PortalId, Me.ID, "SelectedCat")
                If IsNumeric(strTemp) Then
                    _CatID = CInt(strTemp)
                End If
            Else
                If Not Page.IsPostBack Then
                    setStoreCookieValue(PortalSettings.PortalId, Me.ID, "SearchText", "", 0)
                    setStoreCookieValue(PortalSettings.PortalId, Me.ID, "SelectedCat", _CatID, 0)
                End If
            End If

            populateCategoryList(PortalSettings.PortalId, ddlCategory, -1, "All Categories", _CatID)

            ResourceFile = Services.Localization.Localization.GetResourceFile(Me, Me.GetType().BaseType.Name + ".ascx")
            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgProducts, ResourceFile)

            If Not Page.IsPostBack Then
                txtSearch.Text = getStoreCookieValue(PortalSettings.PortalId, Me.ID, "SearchText")
            End If

            Dim aryList As ArrayList
            Dim CatID As Integer

            If IsNumeric(ddlCategory.SelectedValue) Then
                CatID = CInt(ddlCategory.SelectedValue)
            Else
                CatID = -1
            End If

            Dim LookForArchived As Boolean = True
            If _Related Then
                LookForArchived = False
            End If

            aryList = objCtrl.GetProductList(PortalSettings.PortalId, _CatID, GetCurrentCulture, txtSearch.Text, LookForArchived, _CurrentPage, PgSize, GetStoreSettingBoolean(PortalSettings.PortalId, "searchdescription.flag", GetCurrentCulture), True)
            ListSize = objCtrl.GetProductListSize(PortalSettings.PortalId, _CatID, GetCurrentCulture, txtSearch.Text, LookForArchived, False, GetStoreSettingBoolean(PortalSettings.PortalId, "searchdescription.flag", GetCurrentCulture), True, "", False)

            If _Related Then
                If IsNumeric(GetStoreSetting(_Parent.PortalId, "adminproduct.repeatcolumns", GetCurrentCulture)) Then
                    dlProducts.RepeatColumns = CInt(GetStoreSetting(_Parent.PortalId, "adminproduct.repeatcolumns", GetCurrentCulture))
                Else
                    dlProducts.RepeatColumns = 4
                End If
                cmdAdd.Visible = False

                Dim strMsg As String
                strMsg = Localization.GetString("cmdReturn", ResourceFile)
                cmdReturn.Text = strMsg

                dlProducts.DataSource = aryList
                dlProducts.DataBind()
                dgProducts.Visible = False
                ctlPagingControl.QuerystringParams = "ctl=" & _ctl & "&mid=" & _mid & "&CatID=" & CatID & "&ProdID=" & ProdID & "&rel=1"
            Else
                cmdReturn.Visible = False
                dgProducts.DataSource = aryList
                dgProducts.DataBind()
                dlProducts.Visible = False
                ctlPagingControl.QuerystringParams = "ctl=" & _ctl & "&mid=" & _mid & "&CatID=" & CatID
            End If

            ctlPagingControl.TotalRecords = ListSize
            ctlPagingControl.PageSize = PgSize
            ctlPagingControl.CurrentPage = _CurrentPage
            ctlPagingControl.TabID = PortalSettings.ActiveTab.TabID
            ctlPagingControl.BorderWidth = 0
            ctlPagingControl.SkinSrc = _SkinSrc

            If ListSize <= PgSize Or PgSize = -1 Then
                ctlPagingControl.Visible = False
                lblLineBreak.Visible = False
            Else
                ctlPagingControl.Visible = True
                lblLineBreak.Visible = True
            End If


        End Sub

        Private Sub dgProducts_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgProducts.EditCommand
            RaiseEvent EditCommand(source, e)
        End Sub

        Private Sub dgProducts_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgProducts.ItemCommand
            If e.CommandName = "Copy" Then
                Dim item As DataGridItem = e.Item
                Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
                Dim objCtrl As New ProductController
                objCtrl.CopyProduct(ItemId)
                RaiseEvent ItemCommand(source, e)
            End If
        End Sub

        Private Sub dgProducts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgProducts.ItemDataBound
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim imgColumnControl As Control = e.Item.Controls(8).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                    remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdCopyProduct", ResourceFile) & "');")
                End If

                If e.Item.Cells(4).Text = "True" Then
                    e.Item.Cells(4).Text = "<img src=""" & _Parent.StoreInstallPath & "img/accept.png" & """ border=""0"" />"
                Else
                    e.Item.Cells(4).Text = ""
                End If

                If e.Item.Cells(5).Text = "True" Then
                    e.Item.Cells(5).Text = "<img src=""" & _Parent.StoreInstallPath & "img/zip.png" & """ border=""0"" />"
                Else
                    e.Item.Cells(5).Text = ""
                End If

                If e.Item.Cells(6).Text = "True" Then
                    e.Item.Cells(6).Text = "<img src=""" & _Parent.StoreInstallPath & "img/delete.png" & """ border=""0"" />"
                Else
                    e.Item.Cells(6).Text = ""
                End If

                If e.Item.Cells(7).Text = "True" Then
                    e.Item.Cells(7).Text = "<img src=""" & _Parent.StoreInstallPath & "img/hidden.png" & """ border=""0"" />"
                Else
                    e.Item.Cells(7).Text = ""
                End If


            End If
        End Sub

        Private Sub dlProducts_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlProducts.ItemCommand
            Select Case e.CommandSource.CommandName
                Case "AddToWishList"
                    WishList.AddProduct(_Parent.PortalId, e.CommandArgument.ToString, Nothing)
                Case "RemoveFromWishList"
                    WishList.RemoveProduct(_Parent.PortalId, e.CommandArgument.ToString)
                Case "AddRelated"
                    Dim ProdID As String = getAdminCookieValue(_Parent.PortalId, "ProdID")
                    If IsNumeric(ProdID) Then
                        RaiseEvent AddRelated(ProdID, e.CommandArgument.ToString)
                    End If
            End Select
            populateList()
        End Sub

        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            dgProducts.CurrentPageIndex = 0
            _CurrentPage = 1
            setStoreCookieValue(PortalSettings.PortalId, Me.ID, "SearchText", txtSearch.Text, 1)
            setStoreCookieValue(PortalSettings.PortalId, Me.ID, "SelectedCat", ddlCategory.SelectedValue, 0)
            _CatID = ddlCategory.SelectedValue
            populateList()
        End Sub

        Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
            RaiseEvent AddButton(sender, e)
        End Sub

        Private Sub cmdReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturn.Click
            RaiseEvent ReturnButton(sender, e)
        End Sub

    End Class

End Namespace
