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
Imports DotNetNuke.UI.Utilities

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class ProductList
        Inherits BaseModule
        Implements Entities.Modules.IActionable
        Implements Entities.Modules.ISearchable
        Implements Entities.Modules.IPortable


#Region "Private Members"

        Private CatID As Integer = -1
        Private ProdID As Integer = -1
        Private _CurrentPage As Integer = 1
        Private _PageIndex As Integer = 1
        Private _EditCtrlKey As String
        Private _RtnTabID As Integer
        Private _searchQuery As String
        Private _OrderByQuery As String
        Private _OrderDESCQuery As Boolean
        Private _WishList As Boolean
        Private _CasCade As Boolean = False

#End Region

        Public ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return CType(Me.Page, DotNetNuke.Framework.CDefault)
            End Get
        End Property

#Region "Event Handlers"

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim objSTCtrl As New SettingsController
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim objSInfo As NB_Store_SettingsInfo
            Dim strDTempl As String = ""
            Dim strPTempl As String = "productlist.template"
            Dim strAPTempl As String = "productlist.template"
            Dim LightBoxFlag As Boolean = False
            Dim ProductTabID As Integer
            Dim StaticListView As Boolean = False
            Dim StaticTemplates As Boolean = False

            Try
                StaticListView = CType(Settings("chkStaticListView"), String)
            Catch ex As Exception
                StaticListView = False
            End Try

            Try
                StaticTemplates = CType(Settings("chkStaticTemplates"), Boolean)
            Catch ex As Exception
                StaticTemplates = False
            End Try

            ProdID = -1
            If Not (Request.QueryString("ProdID") Is Nothing) Then
                If IsNumeric(Request.QueryString("ProdID")) Then
                    ProdID = Request.QueryString("ProdID")
                End If
            End If

            _CurrentPage = 1
            If Not (Request.QueryString("currentpage") Is Nothing) Then
                If IsNumeric(Request.QueryString("currentpage")) Then
                    _CurrentPage = Request.QueryString("currentpage")
                End If
            End If

            If IsNumeric(CType(Settings("ddlTabDefaultRedirect"), String)) Then
                _RtnTabID = CType(Settings("ddlTabDefaultRedirect"), Integer)
            Else
                _RtnTabID = TabId
            End If
            If Not (Request.QueryString("RtnTab") Is Nothing) Then
                If IsNumeric(Request.QueryString("RtnTab")) Then
                    _RtnTabID = CType(Request.QueryString("RtnTab"), Integer)
                End If
            End If

            If CType(Settings("chkBrowseCategory"), Boolean) = True Then
                'allow querystrings to change category...
                If Not (Request.QueryString("CatID") Is Nothing) Then
                    If IsNumeric(Request.QueryString("CatID")) Then
                        CatID = Request.QueryString("CatID")
                        If CatID = 0 Then CatID = assignCatDefault()
                    Else
                        CatID = assignCatDefault()
                    End If
                Else
                    CatID = assignCatDefault()
                End If
            Else
                'disallow querystrings from changing the category... 
                CatID = assignCatDefault()
            End If


            If IsNumeric(CType(Settings("lstProductTabs"), String)) Then
                ProductTabID = CType(Settings("lstProductTabs"), Integer)
            Else
                ProductTabID = TabId
            End If

            _EditCtrlKey = "AdminProduct"

            Dim blnHasModuleEditPermissions As Boolean = PortalSecurity.HasNecessaryPermission(SecurityAccessLevel.Edit, PortalSettings, ModuleConfiguration)

            If CType(Settings("ddlTemplate"), String) <> "" Then
                strPTempl = CType(Settings("ddlTemplate"), String)
            End If
            If CType(Settings("ddlAlterTemplate"), String) <> "" Then
                strAPTempl = CType(Settings("ddlAlterTemplate"), String)
            End If

            'check if a template exists for category
            If CatID > 0 And Not StaticTemplates Then
                Dim objCCtrl As New CategoryController
                Dim objCInfo As NB_Store_CategoriesInfo
                objCInfo = objCCtrl.GetCategory(CatID, GetCurrentCulture)
                If Not objCInfo Is Nothing Then
                    If objCInfo.Archived And assignCatDefault() <> CatID Then
                        'if category archived redirect
                        Response.Redirect(NavigateURL())
                    End If
                    If objCInfo.ProductTemplate <> "" Then
                        strDTempl = objCInfo.ProductTemplate
                    End If
                    If objCInfo.ListItemTemplate <> "" Then
                        strPTempl = objCInfo.ListItemTemplate
                    End If
                    If objCInfo.ListAltItemTemplate <> "" Then
                        strAPTempl = objCInfo.ListAltItemTemplate
                    End If
                End If
            End If

            'get buy button text
            Dim strBuyText As String = Localization.GetString("BuyText", LocalResourceFile)
            objSTInfo = objSTCtrl.GetSettingsText(PortalId, "buybutton.text", GetCurrentCulture)
            If Not objSTInfo Is Nothing Then
                If objSTInfo.SettingValue <> "" Then
                    strBuyText = System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingValue)
                End If
            End If

            'get Zero Price Message
            Dim ZeroPriceMsg As String = ""
            objSTInfo = objSTCtrl.GetSettingsText(PortalId, "zeroprice.text", GetCurrentCulture)
            If Not objSTInfo Is Nothing Then
                If objSTInfo.SettingValue <> "" Then
                    ZeroPriceMsg = objSTInfo.SettingValue
                End If
            End If

            'get Sold Out Image html
            Dim SoldOutImg As String = ""
            objSTInfo = objSTCtrl.GetSettingsText(PortalId, "soldoutimg.template", GetCurrentCulture)
            If Not objSTInfo Is Nothing Then
                If objSTInfo.SettingText <> "" Then
                    SoldOutImg = objSTInfo.SettingText
                End If
            End If

            Dim blnLockOnCart As Boolean = GetStoreSettingBoolean(PortalId, "lockstockoncart", GetCurrentCulture)

            Dim QtyLimit As Integer = 999999
            objSInfo = objSTCtrl.GetSetting(PortalId, "productqty.limit", GetCurrentCulture)
            If Not objSInfo Is Nothing Then
                If IsNumeric(objSInfo.SettingValue) Then
                    QtyLimit = CInt(objSInfo.SettingValue)
                End If
            End If

            Dim UserTempl As Boolean = False
            objSInfo = objSTCtrl.GetSetting(PortalId, "usertemplates.flag", GetCurrentCulture)
            If Not objSInfo Is Nothing Then
                UserTempl = CBool(objSInfo.SettingValue)
            End If

            If UserTempl And UserId >= 0 Then
                'special user templates are being used, check if they exists (cached so preread doesn't matter that much)
                objSTInfo = objSTCtrl.GetSettingsText(PortalId, strPTempl & "user", GetCurrentCulture)
                If Not objSTInfo Is Nothing Then strPTempl = strPTempl & "user"
                objSTInfo = objSTCtrl.GetSettingsText(PortalId, strAPTempl & "user", GetCurrentCulture)
                If Not objSTInfo Is Nothing Then strAPTempl = strAPTempl & "user"
            End If

            'get list templates
            objSTInfo = objSTCtrl.GetSettingsText(PortalId, strPTempl, GetCurrentCulture)
            If Not objSTInfo Is Nothing Then
                dlProductList.ItemTemplate = New ProductTemplate(TabId, ModuleId, StoreInstallPath, CType(Settings("txtThumbnailSize"), String), Server.HtmlDecode(objSTInfo.SettingText), blnHasModuleEditPermissions, strBuyText, CType(Settings("txtCssBuyButton"), String), _CurrentPage, CatID, CType(Settings("txtGalleryThumbnailSize"), String), _EditCtrlKey, ProductTabID, UserId, UserInfo, ZeroPriceMsg, SoldOutImg, blnLockOnCart, QtyLimit)
            End If

            objSTInfo = objSTCtrl.GetSettingsText(PortalId, strAPTempl, GetCurrentCulture)
            If Not objSTInfo Is Nothing Then
                dlProductList.AlternatingItemTemplate = New ProductTemplate(TabId, ModuleId, StoreInstallPath, CType(Settings("txtThumbnailSize"), String), Server.HtmlDecode(objSTInfo.SettingText), blnHasModuleEditPermissions, strBuyText, CType(Settings("txtCssBuyButton"), String), _CurrentPage, CatID, CType(Settings("txtGalleryThumbnailSize"), String), _EditCtrlKey, ProductTabID, UserId, UserInfo, ZeroPriceMsg, SoldOutImg, blnLockOnCart, QtyLimit)
            End If

            If ProdID >= 0 Then
                'get detail template
                If CType(Settings("ddlDetailTemplate"), String) <> "" And strDTempl = "" Then
                    strDTempl = CType(Settings("ddlDetailTemplate"), String)
                End If

                If UserTempl And UserId >= 0 Then
                    'special user templates are being used, check if they exists (cached so preread doesn't matter that much)
                    objSTInfo = objSTCtrl.GetSettingsText(PortalId, strDTempl & "user", GetCurrentCulture)
                    If Not objSTInfo Is Nothing Then strDTempl = strDTempl & "user"
                End If

                objSTInfo = objSTCtrl.GetSettingsText(PortalId, strDTempl, GetCurrentCulture)
                If Not objSTInfo Is Nothing Then
                    dlProductDetail.ItemTemplate = New ProductTemplate(TabId, ModuleId, StoreInstallPath, CType(Settings("txtDetailThumbnailSize"), String), Server.HtmlDecode(objSTInfo.SettingText), blnHasModuleEditPermissions, strBuyText, CType(Settings("txtCssBuyButton"), String), _CurrentPage, CatID, CType(Settings("txtGalleryThumbnailSize"), String), _EditCtrlKey, ProductTabID, UserId, UserInfo, ZeroPriceMsg, SoldOutImg, blnLockOnCart, QtyLimit)
                End If

            End If

            IncludeScripts(PortalId, StoreInstallPath, Page, "productlistjs.includes ", "productliststartupjs.includes", "productlistcss.includes")

            If CType(Settings("txtSep"), String) <> "" Then
                dlProductList.SeparatorTemplate = New BaseDisplayTemplate(Server.HtmlDecode(CType(Settings("txtSep"), String)))
            End If

            If CType(Settings("ddlHeaderText"), String) <> "" Then
                DisplayMsgText(PortalId, phProductModuleHeader, CType(Settings("ddlHeaderText"), String), "")
            End If
            If CType(Settings("ddlListHeaderText"), String) <> "" Then
                DisplayMsgText(PortalId, phProductListHeader, CType(Settings("ddlListHeaderText"), String), "")
            End If
            If CType(Settings("ddlDetailHeaderText"), String) <> "" Then
                DisplayMsgText(PortalId, phProductDetailHeader, CType(Settings("ddlDetailHeaderText"), String), "")
            End If


            Try

                _PageIndex = _CurrentPage
                If Not (Request.QueryString("PageIndex") Is Nothing) Then
                    If IsNumeric(Request.QueryString("PageIndex")) Then
                        _PageIndex = Request.QueryString("PageIndex")
                    End If
                End If

                _searchQuery = ""
                If Not Request.Params("Search") Is Nothing Then
                    _searchQuery = Request.Params("Search").ToString
                End If

                _OrderByQuery = ""
                If Not Request.Params("orderby") Is Nothing Then
                    _OrderByQuery = Request.Params("orderby").ToString
                End If

                _OrderDESCQuery = False
                If Not Request.Params("desc") Is Nothing Then
                    If Request.Params("desc") <> "0" Then
                        _OrderDESCQuery = True
                    End If
                End If

                _WishList = False
                If Not Request.Params("wishlist") Is Nothing Then
                    _WishList = True
                End If

                _CasCade = CType(Settings("chkCascadeResults"), Boolean)
                If Not Request.Params("cascade") Is Nothing Then
                    _CasCade = True
                End If

                If Not Page.IsPostBack Then
                    If ProdID > 0 And Not CType(Settings("chkFeatured"), Boolean) And Not StaticListView Then
                        populateSP(ProdID)
                    Else
                        If Not CType(Settings("chkFeatured"), Boolean) Then
                            setUrlCookieInfo(PortalId, Request)
                        End If
                        populateList(CatID)
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try



        End Sub

#End Region

#Region "Methods"

        Private Function assignCatDefault() As Integer
            If IsNumeric(CType(Settings("ddlDefaultCategory"), String)) Then
                Return CType(Settings("ddlDefaultCategory"), Integer) ' set default
            Else
                Return -1
            End If
        End Function

        Private Sub populateList(ByVal CatID As Integer)
            Dim aryList As ArrayList
            Dim objCtrl As New ProductController
            Dim PgSize As Integer
            Dim FeaturedOnly As Boolean = False
            Dim AllowCategoryBrowse As Boolean = True
            Dim ReturnLimit As Integer = 0
            Dim objCCtrl As New CategoryController
            Dim objCInfo As NB_Store_CategoriesInfo = Nothing
            Dim ListSize As Integer = 0
            Dim ExcludeFeatured As Boolean = False
            Dim ExcludeProduct As Boolean = False

            CategoryMenu.Visible = False
            CategoryMenu._SubMenuOnly = True

            If IsNumeric(CType(Settings("txtPageSize"), String)) Then
                PgSize = CType(Settings("txtPageSize"), String)
            Else
                Exit Sub ' no setting so exit
                'PgSize = 10
            End If

            If Settings("ddlLayout") Is Nothing Then
                dlProductList.RepeatLayout = RepeatLayout.Table
            Else
                If IsNumeric(Settings("ddlLayout")) Then
                    dlProductList.RepeatLayout = CInt(Settings("ddlLayout"))
                Else
                    dlProductList.RepeatLayout = RepeatLayout.Table
                End If
            End If

            dlProductList.ItemStyle.VerticalAlign = VerticalAlign.Top
            If Settings("txtTableWidth") Is Nothing Then
                dlProductList.Width = System.Web.UI.WebControls.Unit.Parse("100%")
            Else
                If Settings("txtTableWidth") <> "" Then
                    dlProductList.Width = System.Web.UI.WebControls.Unit.Parse(Settings("txtTableWidth"))
                End If
            End If

            If IsNumeric(CType(Settings("txtCellPadding"), String)) Then
                dlProductList.CellPadding = CType(Settings("txtCellPadding"), Integer)
            Else
                dlProductList.CellPadding = 0
            End If

            If IsNumeric(CType(Settings("txtCellSpacing"), String)) Then
                dlProductList.CellSpacing = CType(Settings("txtCellSpacing"), Integer)
            End If

            If CType(Settings("txtItemStyleCssClass"), String) <> "" Then
                dlProductList.ItemStyle.CssClass = CType(Settings("txtItemStyleCssClass"), String)
            End If

            If CType(Settings("txtAlternatingItemStyleCssClass"), String) <> "" Then
                dlProductList.AlternatingItemStyle.CssClass = CType(Settings("txtAlternatingItemStyleCssClass"), String)
            End If

            If IsNumeric(CType(Settings("ddlItemStyleHorizontalAlign"), String)) Then
                dlProductList.ItemStyle.HorizontalAlign = CType(Settings("ddlItemStyleHorizontalAlign"), Integer)
            End If

            If CType(Settings("txtItemStyleHeight"), String) <> "" Then
                dlProductList.ItemStyle.Height = System.Web.UI.WebControls.Unit.Parse(CType(Settings("txtItemStyleHeight"), String))
            End If

            If CType(Settings("txtCssClass"), String) <> "" Then
                dlProductList.CssClass = CType(Settings("txtCssClass"), String)
            End If


            If CType(Settings("txtColWidth"), String) <> "" Then
                dlProductList.ItemStyle.Width = System.Web.UI.WebControls.Unit.Parse(CType(Settings("txtColWidth"), String))
            End If

            If IsNumeric(CType(Settings("txtColumns"), String)) Then
                dlProductList.RepeatColumns = CType(Settings("txtColumns"), Integer)
            End If
            If IsNumeric(CType(Settings("ddlDirection"), String)) Then
                dlProductList.RepeatDirection = CType(Settings("ddlDirection"), Integer)
            End If

            FeaturedOnly = CType(Settings("chkFeatured"), Boolean)
            AllowCategoryBrowse = CType(Settings("chkBrowseCategory"), Boolean)
            ExcludeFeatured = CType(Settings("chkExcludeFeatured"), Boolean)
            ExcludeProduct = CType(Settings("chkExcludeProduct"), Boolean)

            If IsNumeric(CType(Settings("txtReturnLimit"), String)) Then
                ReturnLimit = CType(Settings("txtReturnLimit"), Integer)
            End If

            If _OrderByQuery = "" Then
                _OrderByQuery = CType(Settings("ddlDefaultOrder"), String)
            End If

            If Request.Params("desc") Is Nothing Then
                _OrderDESCQuery = CType(Settings("chkDefaultOrderDESC"), Boolean)
            End If

            Dim CategoryList As String = ""
            If _CasCade Then
                aryList = objCCtrl.GetCategories(PortalId, GetCurrentCulture)
                CategoryList = objCCtrl.GetSubCategoryList(aryList, "", CatID)
                CategoryList &= CatID.ToString
            End If

            If PgSize <= 0 Then
                _CurrentPage = 1
                PgSize = 9999
            End If

            If FeaturedOnly Then
                If AllowCategoryBrowse = True AndAlso CatID > 0 Then
                    'a category is requested, however we're also in featured only mode... but browsing is enabled so we should load that category... 
                    aryList = objCtrl.GetProductList(PortalId, CatID, GetCurrentCulture, "", False, FeaturedOnly, _OrderByQuery, _OrderDESCQuery, ReturnLimit, _CurrentPage, PgSize, False, IsDealer(PortalId, UserInfo, GetCurrentCulture), CategoryList, ExcludeFeatured)
                    ListSize = objCtrl.GetProductListSize(PortalId, CatID, GetCurrentCulture, "", False, FeaturedOnly, False, IsDealer(PortalId, UserInfo, GetCurrentCulture), CategoryList, ExcludeFeatured)
                Else
                    'this should be featured always, nothing but... (CatID passed because featured default)
                    aryList = objCtrl.GetProductList(PortalId, assignCatDefault(), GetCurrentCulture, "", False, FeaturedOnly, _OrderByQuery, _OrderDESCQuery, ReturnLimit, _CurrentPage, PgSize, False, IsDealer(PortalId, UserInfo, GetCurrentCulture), CategoryList, ExcludeFeatured)
                    ListSize = objCtrl.GetProductListSize(PortalId, assignCatDefault(), GetCurrentCulture, "", False, FeaturedOnly, False, IsDealer(PortalId, UserInfo, GetCurrentCulture), CategoryList, ExcludeFeatured)
                End If
            Else
                If _WishList Then
                    'wishlist to be displayed, get from wishlist class
                    aryList = WishList.GetList(PortalId)
                    ListSize = aryList.Count
                Else
                    aryList = objCtrl.GetProductList(PortalId, CatID, GetCurrentCulture, _searchQuery, False, FeaturedOnly, _OrderByQuery, _OrderDESCQuery, ReturnLimit, _CurrentPage, PgSize, GetStoreSettingBoolean(PortalId, "searchdescription.flag", GetCurrentCulture), IsDealer(PortalId, UserInfo, GetCurrentCulture), CategoryList, ExcludeFeatured)
                    ListSize = objCtrl.GetProductListSize(PortalId, CatID, GetCurrentCulture, _searchQuery, False, FeaturedOnly, GetStoreSettingBoolean(PortalId, "searchdescription.flag", GetCurrentCulture), IsDealer(PortalId, UserInfo, GetCurrentCulture), CategoryList, ExcludeFeatured)
                    If ReturnLimit > 0 Then
                        If ListSize > ReturnLimit Then ListSize = ReturnLimit
                    End If
                End If
            End If

            'only one product exists so skip list and display
            If Not FeaturedOnly And CType(Settings("chkSkipList"), Boolean) Then
                If aryList.Count = 1 And CatID > -1 Then
                    Dim objPInfo As ProductListInfo
                    objPInfo = CType(aryList(0), ProductListInfo)
                    Response.Redirect(GetProductUrl(PortalId, TabId, objPInfo, CatID, False))
                End If
            End If

            If ExcludeProduct Then
                If ProdID >= 0 Then
                    'remove the current display product from the list.
                    For Each objPInfo As ProductListInfo In aryList
                        If objPInfo.ProductID = ProdID Then
                            aryList.Remove(objPInfo)
                            Exit For
                        End If
                    Next
                Else
                    If aryList.Count > 1 Then
                        aryList.RemoveAt(0)
                    End If
                End If
            End If

            dlProductList.DataSource = aryList
            dlProductList.DataBind()

            If ListSize <= PgSize Or PgSize = -1 Then
                ctlPagingControl.Visible = False
                lblLineBreak.Visible = False
            End If

            ctlPagingControl.TotalRecords = ListSize
            ctlPagingControl.PageSize = PgSize
            ctlPagingControl.CurrentPage = _CurrentPage
            ctlPagingControl.TabID = TabId
            ctlPagingControl.BorderWidth = 0

            If _searchQuery <> "" Then
                ctlPagingControl.QuerystringParams = "Search=" & Server.UrlEncode(_searchQuery)
            End If

            If _OrderByQuery <> "" Then
                If ctlPagingControl.QuerystringParams <> "" Then ctlPagingControl.QuerystringParams &= "&"
                ctlPagingControl.QuerystringParams &= "orderby=" & _OrderByQuery
            End If
            If _OrderDESCQuery Then
                ctlPagingControl.QuerystringParams &= "&desc=1"
            End If
            If _WishList Then
                If ctlPagingControl.QuerystringParams <> "" Then ctlPagingControl.QuerystringParams &= "&"
                ctlPagingControl.QuerystringParams &= "wishlist=1"
            End If


            If Not (Request.QueryString("CatID") Is Nothing) Then
                If ctlPagingControl.QuerystringParams <> "" Then ctlPagingControl.QuerystringParams &= "&"
                ctlPagingControl.QuerystringParams &= "CatID=" & CatID
            End If

            If (_searchQuery <> "" Or _WishList) And aryList.Count = 0 Then
                If _WishList Then
                    DisplayMsgText(PortalId, plhCatMsg, "nowishlist.template", "No WishList Found")
                Else
                    DisplayMsgText(PortalId, plhCatMsg, "nosearchresult.template", "No Search Results Found")
                End If
            Else
                If (aryList.Count = 0 And CType(Settings("rblCategoryMessage"), String) = 3) Or CType(Settings("rblCategoryMessage"), String) = 2 Then
                    'No Products so display category message
                    objCInfo = objCCtrl.GetCategory(CatID, GetCurrentCulture)
                    If Not objCInfo Is Nothing Then
                        If aryList.Count = 0 Then ctlPagingControl.Visible = False

                        If Not EventInterface.Instance() Is Nothing Then
                            objCInfo.Message = EventInterface.Instance.getCategoryMessage(PortalId, CatID, UserInfo)
                        End If

                        If objCInfo.Message <> "" Then
                            objCInfo.Message = Replace(objCInfo.Message, "[TAG:CATIMAGEURL]", objCInfo.ImageURL)
                        End If
                        plhCatMsg.Controls.Add(New LiteralControl(Server.HtmlDecode(objCInfo.Message)))

                        CategoryMenu.Visible = True

                        CategoryMenu._chkHideSubMenu = Not CType(Settings("chkShowSubMenu"), Boolean)
                        CategoryMenu._txtColumns = CType(Settings("txtSubMenuCols"), String)
                        CategoryMenu._txtSubLeftHtml = CType(Settings("txtSubLeftHtml"), String)
                        CategoryMenu._txtSubNameTemplate = CType(Settings("txtSubNameTemplate"), String)
                        CategoryMenu._txtSubRightHtml = CType(Settings("txtSubRightHtml"), String)
                        CategoryMenu._txtSubHeadHtml = CType(Settings("txtSubHeadHtml"), String)
                        CategoryMenu._txtSubSelectCSS = CType(Settings("txtSubSelectCSS"), String)
                        CategoryMenu._txtCSS = CType(Settings("txtCSS"), String)
                        CategoryMenu._txtSubMenuSep = CType(Settings("txtSubMenuSep"), String)
                        CategoryMenu._txtThumbnailSize = CType(Settings("txtSubThumbnailSize"), String)
                        CategoryMenu._ddlDefaultCategory = CatID
                        CategoryMenu._TabId = TabId
                    End If
                End If
            End If

            'check if we have a module title setting, and override the moduletitle if the setting equals one. 
            If Settings.ContainsKey("lstModuleTitle") Then
                If Settings("lstModuleTitle").ToString = "2" Or Settings("lstModuleTitle").ToString = "3" Then
                    If _searchQuery <> "" Then
                        Me.ModuleConfiguration.ModuleTitle = Localization.GetString("SearchResults", LocalResourceFile)
                    Else
                        If objCInfo Is Nothing Then
                            objCInfo = objCCtrl.GetCategory(CatID, GetCurrentCulture)
                        End If
                        If Not objCInfo Is Nothing Then
                            Me.ModuleConfiguration.ModuleTitle = objCInfo.CategoryName
                        Else
                            Me.ModuleConfiguration.ModuleTitle = Localization.GetString("ModuleTitleAll", LocalResourceFile)
                        End If
                    End If
                End If
            End If

            If CatID > 0 Then
                If objCInfo Is Nothing Then
                    objCInfo = objCCtrl.GetCategory(CatID, GetCurrentCulture)
                End If
                If Not objCInfo Is Nothing Then
                    If objCInfo.CategoryDesc <> "" Then
                        Me.BasePage.Description = objCInfo.CategoryDesc
                        Me.BasePage.Title = objCInfo.CategoryDesc
                    End If
                    'include canonical link
                    IncludeCanonicalLink(Page, GetProductListUrlByCatID(PortalId, TabId, CatID, objCInfo.CategoryName, GetCurrentCulture))
                End If

            End If

            pnlSPO.Visible = False

        End Sub

        Private Sub populateSP(ByVal ProdID As Integer)
            Dim aryList As ArrayList
            Dim objCtrl As New ProductController
            Dim objCCtrl As New CategoryController
            Dim objCInfo As NB_Store_CategoriesInfo

            aryList = objCtrl.GetProductInArray(ProdID, GetCurrentCulture)

            If aryList.Count = 1 Then
                Dim objPInfo As ProductListInfo = CType(aryList(0), ProductListInfo)
                If objPInfo.Archived Then
                    'if product archived redirect
                    Response.Redirect(NavigateURL())
                End If
                If CatID = -1 Then
                    objCInfo = objCCtrl.GetCategory(objPInfo.TaxCategoryID, GetCurrentCulture)
                Else
                    objCInfo = objCCtrl.GetCategory(CatID, GetCurrentCulture)
                End If
                If objCInfo Is Nothing Then
                    Me.BasePage.Title = ProductPageName(objPInfo, "", PortalSettings.PortalName)
                Else
                    Me.BasePage.Title = ProductPageName(objPInfo, objCInfo.CategoryName, PortalSettings.PortalName)
                End If

                If objPInfo.TagWords <> "" Then
                    Me.BasePage.KeyWords = objPInfo.TagWords
                End If

                If objPInfo.Summary <> "" Then
                    Me.BasePage.Description = objPInfo.Summary
                End If

                'check if we have a module title setting, and override the moduletitle if the setting equals one. 
                If Settings.ContainsKey("lstModuleTitle") Then
                    If Settings("lstModuleTitle").ToString = "1" Or Settings("lstModuleTitle").ToString = "3" Then
                        Me.ModuleConfiguration.ModuleTitle = objPInfo.ProductName
                    End If
                    If Settings("lstModuleTitle").ToString = "2" Then
                        Me.ModuleConfiguration.ModuleTitle = ""
                    End If
                End If

                'include canonical link
                IncludeCanonicalLink(Page, GetProductUrl(PortalId, TabId, objPInfo, CatID, True))

            End If


            dlProductDetail.Width = System.Web.UI.WebControls.Unit.Parse("100%")
            dlProductDetail.CellPadding = 0
            dlProductDetail.CellSpacing = 0

            dlProductDetail.DataSource = aryList
            dlProductDetail.DataBind()

            'Display out of stock message if not models in stock
            Dim aryStock As ArrayList
            aryStock = GetAvailableModelList(PortalId, ProdID, GetCurrentCulture, IsDealer(PortalId, UserInfo, GetCurrentCulture))
            If aryStock.Count = 0 Then
                Dim strOutOfStock As String = GetStoreSettingText(PortalId, "outofstock.text", GetCurrentCulture)
                If strOutOfStock = "" Then
                    strOutOfStock = Localization.GetString("OutOfStock", LocalResourceFile)
                End If
                lblMsg.Text = strOutOfStock
            Else
                lblMsg.Text = ""
            End If

            If Not EventInterface.Instance() Is Nothing Then
                EventInterface.Instance.DisplayProduct(PortalId, ProdID, UserInfo)
            End If

            pnlProductList.Visible = False
        End Sub

#End Region

        Private Sub dlProductDetail_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlProductDetail.ItemCommand
            Select Case e.CommandSource.CommandName
                Case "AddToBasket"
                    Dim item As DataListItem = e.Item
                    CurrentCart.AddItemToCart(PortalId, item, CType(Settings("chkIncrementCart"), Boolean), UserInfo, Request)
                    WishList.RemoveProduct(PortalId, e.CommandArgument.ToString)
                    DoAfterBuyRedirect()
                Case "DocDownload"
                    Dim DocId As Integer = Int32.Parse(e.CommandArgument.ToString)
                    ForceDocDownload(DocId)
                Case "DocPurchased"
                    Dim DocId As Integer = Int32.Parse(e.CommandArgument.ToString)
                    If DocHasBeenPurchasedByDocID(UserId, DocId) Then
                        ForceDocDownload(DocId)
                    End If
                Case "AddToWishList"
                    Dim item As DataListItem = e.Item
                    WishList.AddProduct(PortalId, e.CommandArgument.ToString, UserInfo)
                    DoAfterWishListRedirect()
                Case "RemoveFromWishList"
                    Dim item As DataListItem = e.Item
                    WishList.RemoveProduct(PortalId, e.CommandArgument.ToString)
                    DoAfterWishListRedirect()
            End Select

        End Sub

        Private Sub dlProductList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlProductList.ItemCommand
            Select Case e.CommandSource.CommandName
                Case "AddToBasket"
                    Dim item As DataListItem = e.Item
                    lblMsg.Text = ""
                    CurrentCart.AddItemToCart(PortalId, item, CType(Settings("chkIncrementCart"), Boolean), UserInfo, Request)
                    WishList.RemoveProduct(PortalId, e.CommandArgument.ToString)
                    DoAfterBuyRedirect()
                Case "DocDownload"
                    Dim DocId As Integer = Int32.Parse(e.CommandArgument.ToString)
                    ForceDocDownload(DocId)
                Case "DocPurchased"
                    Dim DocId As Integer = Int32.Parse(e.CommandArgument.ToString)
                    If DocHasBeenPurchasedByDocID(UserId, DocId) Then
                        ForceDocDownload(DocId)
                    End If
                Case "AddToWishList"
                    setUrlCookieInfo(PortalId, Request)
                    WishList.AddProduct(PortalId, e.CommandArgument.ToString, UserInfo)
                    DoAfterWishListRedirect()
                Case "RemoveFromWishList"
                    setUrlCookieInfo(PortalId, Request)
                    WishList.RemoveProduct(PortalId, e.CommandArgument.ToString)
                    DoAfterWishListRedirect()
                Case "AddRelatedProduct"
                    setUrlCookieInfo(PortalId, Request)
                    Dim item As DataListItem = e.Item
                    Dim ProductID As String = getAdminCookieValue(PortalId, "ProdID")
                    Dim RelatedProductID As String = e.CommandArgument.ToString
                    If IsNumeric(ProductID) And IsNumeric(RelatedProductID) Then
                        RelatedProducts.AddProduct(PortalId, CInt(ProductID), CInt(RelatedProductID))
                    End If
                    DoAfterRelatedRedirect()
            End Select
        End Sub

        Private Sub ForceDocDownload(ByVal DocID As Integer)
            Dim objPCtrl As New ProductController
            Dim objDInfo As NB_Store_ProductDocInfo

            objDInfo = objPCtrl.GetProductDoc(DocID, GetCurrentCulture)
            If Not objDInfo Is Nothing Then
                If Not EventInterface.Instance() Is Nothing Then
                    EventInterface.Instance.DownloadDocument(PortalId, DocID, UserInfo)
                End If

                Dim useragent As String = Request.Headers("User-Agent")
                If useragent.Contains("MSIE 7.0") Or useragent.Contains("MSIE 8.0") Then
                    Response.AppendHeader("content-disposition", "attachment; filename=""" & objDInfo.FileName & objDInfo.FileExt & """")
                Else
                    Response.AppendHeader("content-disposition", "attachment; filename=""" & System.Web.HttpUtility.UrlDecode(objDInfo.FileName) & objDInfo.FileExt & """")
                End If
                Response.ContentType = "application/octet-stream"
                Response.WriteFile(objDInfo.DocPath)
                Response.End()
            End If

        End Sub


        Private Sub DoAfterWishListRedirect()
            'Dim strOrderDesc As String = ""
            'Dim strOrder As String = ""
            'Dim strWishList As String = ""

            'If _WishList Then
            '    strWishList = "wishlist=1"
            'End If

            'If _OrderDESCQuery Then
            '    strOrderDesc = "desc=1"
            'End If

            'If _OrderByQuery <> "" Then
            '    strOrderDesc = "orderby=" & _OrderByQuery
            'End If


            'If _PageIndex > 1 Then
            '    Response.Redirect(NavigateURL(_RtnTabID, "", "currentpage=" & _PageIndex.ToString, "CatID=" & CatID.ToString, strOrder, strOrderDesc, strWishList))
            'Else
            '    Response.Redirect(NavigateURL(_RtnTabID, "", "CatID=" & CatID.ToString, strOrder, strOrderDesc, strWishList))
            'End If

            Response.Redirect(NavigateURL(_RtnTabID, "", getUrlCookieInfo(PortalId, "")))

        End Sub

        Private Sub DoAfterRelatedRedirect()
            Response.Redirect(NavigateURL(_RtnTabID, "", getUrlCookieInfo(PortalId, "")))
        End Sub

        Private Sub DoAfterBuyRedirect()
            Dim strOrderDesc As String = ""
            Dim strOrder As String = ""

            If GetStoreSettingBoolean(PortalId, "orderedititems.flag") Then
                Dim ReturnUrl As String = getAdminCookieValue(PortalId, "OrderEditRtnURL")
                If ReturnUrl <> "" Then
                    Response.Redirect(ReturnUrl)
                End If
            End If

            If _OrderDESCQuery Then
                strOrderDesc = "desc=1"
            End If

            If _OrderByQuery <> "" Then
                strOrderDesc = "orderby=" & _OrderByQuery
            End If


            If CType(Settings("chkRedirectToCart"), Boolean) Then
                If IsNumeric(CType(Settings("lstTabs"), String)) Then
                    Response.Redirect(NavigateURL(CType(Settings("lstTabs"), Integer)))
                Else
                    If _PageIndex > 1 Then
                        Response.Redirect(NavigateURL(_RtnTabID, "", "currentpage=" & _PageIndex.ToString, "CatID=" & CatID.ToString, strOrder, strOrderDesc))
                    Else
                        Response.Redirect(NavigateURL(_RtnTabID, "", "CatID=" & CatID.ToString, strOrder, strOrderDesc))
                    End If
                End If
            Else
                If CType(Settings("chkTabDefaultRedirect"), Boolean) Then
                    Response.Redirect(NavigateURL(_RtnTabID, "", getUrlCookieInfo(PortalId, "")))
                Else
                    Response.Redirect(NavigateURL(_RtnTabID))
                End If
            End If

        End Sub


#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                'can't add [, "RtnTab=" & _RtnTabID, "CatID=" & CatID, "currentpage=" & _CurrentPage"] to the add product action, because the create product button redirects, but needs to stay on the same tab. (To do this we need to change the AdminProduct control as well)
                Actions.Add(GetNextActionID, Localization.GetString("AddProduct.Action", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl("ProdId", "0", _EditCtrlKey, "RtnTab=" & TabId.ToString, "PageIndex=" & _PageIndex.ToString, "CatID=" & CatID.ToString, "SkinSrc=" & QueryStringEncode(DotNetNuke.Common.ResolveUrl("~/DesktopModules/NB_Store/Skins/Dark/Edit"))), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
                Actions.Add(GetNextActionID, Localization.GetString("Options.Action", LocalResourceFile), "Options", "", "", EditUrl("Options"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)

                Return Actions
            End Get
        End Property

        Public Function GetSearchItems(ByVal ModInfo As Entities.Modules.ModuleInfo) As Services.Search.SearchItemInfoCollection Implements Entities.Modules.ISearchable.GetSearchItems
            ' included as a stub only so that the core knows this module Implements Entities.Modules.ISearchable
        End Function

        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements Entities.Modules.IPortable.ExportModule
            ' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserID As Integer) Implements Entities.Modules.IPortable.ImportModule
            ' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
        End Sub

#End Region

    End Class

End Namespace
