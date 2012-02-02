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
Imports System.IO

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class CategoryMenu
        Inherits BaseModule
        Implements Entities.Modules.IPortable

        Private CatID As Integer = 0

        Public _chkHideSubMenu As Boolean
        Public _txtColumns As String
        Public _txtSubLeftHtml As String
        Public _txtSubNameTemplate As String
        Public _txtSubRightHtml As String
        Public _txtSubHeadHtml As String
        Public _txtSubSelectCSS As String
        Public _txtCSS As String
        Public _txtSubMenuSep As String
        Public _SubMenuOnly As Boolean = False
        Public _ddlDefaultCategory As Integer
        Public _txtThumbnailSize As String
        Public _TabId As Integer = -1
        Public _chkPatchWork As Boolean = False
        Public _SelectedCategories As Hashtable

#Region "Events"

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            ' menu is running as control in productlist when SubmenuOnly is set to true
            If Not _SubMenuOnly Then
                _chkHideSubMenu = CType(Settings("chkHideSubMenu"), Boolean)
                _chkPatchWork = CType(Settings("chkPatchWork"), Boolean)
                _txtColumns = CType(Settings("txtColumns"), String)
                _txtSubLeftHtml = CType(Settings("txtSubLeftHtml"), String)
                _txtSubNameTemplate = CType(Settings("txtSubNameTemplate"), String)
                _txtSubRightHtml = CType(Settings("txtSubRightHtml"), String)
                _txtSubHeadHtml = CType(Settings("txtSubHeadHtml"), String)
                _txtSubSelectCSS = CType(Settings("txtSubSelectCSS"), String)
                _txtSubMenuSep = CType(Settings("txtSep"), String)
                _txtCSS = CType(Settings("txtCSS"), String)
                _txtThumbnailSize = CType(Settings("txtThumbnailSize"), String)
                If IsNumeric(CType(Settings("ddlDefaultCategory"), String)) Then
                    _ddlDefaultCategory = CType(Settings("ddlDefaultCategory"), Integer)
                Else
                    _ddlDefaultCategory = -1
                End If
                _TabId = TabId
            End If

            If _txtThumbnailSize = "" Then _txtThumbnailSize = "100"

            If CType(Settings("txtRootSep"), String) <> "" Then
                dlRootMenu.SeparatorTemplate = New BaseDisplayTemplate(Server.HtmlDecode(CType(Settings("txtRootSep"), String)))
            End If
            If _txtSubMenuSep <> "" Then
                dlCategoryMenu.SeparatorTemplate = New BaseDisplayTemplate(Server.HtmlDecode(_txtSubMenuSep))
            End If

        End Sub


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try
                Dim StaticCatID As Integer

                CatID = _ddlDefaultCategory
                StaticCatID = CatID

                If Not (Request.QueryString("CatID") Is Nothing) Then
                    If IsNumeric(Request.QueryString("CatID")) Then
                        CatID = Request.QueryString("CatID")
                        If CatID = -1 Then CatID = _ddlDefaultCategory
                    End If
                Else
                    'get from cookie,
                    Dim tmp As String = getCookieURLparam(PortalId, "CatID")
                    tmp = Replace(tmp, "CatID=", "") 'remove prefix
                    If IsNumeric(tmp) Then
                        CatID = CInt(tmp)
                    End If
                End If

				If Not CType(Settings("chkStaticCategory"), Boolean) Then
                    StaticCatID = CatID
                End If
				
                If (Not (Request.QueryString("ProdID") Is Nothing)) And CType(Settings("chkViewProdHide"), Boolean) And Not _SubMenuOnly Then
                    'hide the menu if viewing a product
                    dlCategoryMenu.Visible = False
                    dlRootMenu.Visible = False
                    lblBreadcrumbs.Visible = False
                Else
                    If Not Page.IsPostBack Then
                        _SelectedCategories = New Hashtable
                        If CatID >= 0 Then
                            GetSelectedCategories(CatID, _SelectedCategories)
                            populateList(CatID, StaticCatID)
                        Else
                            dlCategoryMenu.Visible = False
                            dlRootMenu.Visible = False
                            lblBreadcrumbs.Visible = False
                        End If
                        If Not _SubMenuOnly Then

                            phSecSep1.Controls.Add(New LiteralControl(CType(Settings("txtSectionSep"), String)))
                            phSecSep2.Controls.Add(New LiteralControl(CType(Settings("txtSectionSep2"), String)))
                            phSecSep2.Controls.Add(New LiteralControl(CType(Settings("txtSectionSep3"), String)))

                            ShowTreeMenu()

                            ShowAccordion()
                        End If
                    End If

                End If


            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        Private Sub dlCategoryMenu_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlCategoryMenu.ItemDataBound
            Dim itemInfo As NB_Store_CategoriesInfo = CType(e.Item.DataItem, NB_Store_CategoriesInfo)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim phL As PlaceHolder = DirectCast(e.Item.FindControl("phCatLink"), PlaceHolder)
                If Not phL Is Nothing Then
                    phL.Controls.Add(New LiteralControl(BuildhtmlCatLink(_txtSubNameTemplate, _txtSubLeftHtml, _txtSubRightHtml, _txtSubSelectCSS, itemInfo)))
                End If
            End If

        End Sub

        Private Sub dlRootMenu_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlRootMenu.ItemDataBound
            Dim itemInfo As NB_Store_CategoriesInfo = CType(e.Item.DataItem, NB_Store_CategoriesInfo)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim phL As PlaceHolder = DirectCast(e.Item.FindControl("phCatLink"), PlaceHolder)
                If Not phL Is Nothing Then
                    phL.Controls.Add(New LiteralControl(BuildhtmlCatLink(CType(Settings("txtRootNameTemplate"), String), CType(Settings("txtRootLeftHtml"), String), CType(Settings("txtRootRightHtml"), String), CType(Settings("txtRootSelectCSS"), String), itemInfo)))
                End If
            End If
        End Sub


#End Region

#Region "Methods"

        Private Function BuildHtmlCatLink(ByVal NameTemplate As String, ByVal Lefthtml As String, ByVal RightHtml As String, ByVal SelectCSS As String, ByVal itemInfo As NB_Store_CategoriesInfo) As String
            Dim strHtmlOut As String = ""
            Dim strHtmlLink As String = ""
            Dim strHtmlText As String = ""
            Dim NonEmptyCat As Integer
            Dim reg As New Regex("\s*")
            Dim catMsg As String = ""

            catMsg = Replace(itemInfo.Message, "&lt;p&gt;&amp;#160;&lt;/p&gt;", "") 'remove fck editor gaff!!
            If catMsg <> "" Then
                catMsg = Regex.Replace(catMsg, "\s*", "")
            End If

            If CType(Settings("chkSkipBlankCat"), Boolean) And itemInfo.ProductCount = 0 And catMsg = "" Then
                NonEmptyCat = GetFirstNonEmptyChild(itemInfo.CategoryID)
            Else
                NonEmptyCat = itemInfo.CategoryID
            End If

            If IsNumeric(CType(Settings("lstProductTabs"), String)) Then
                strHtmlLink = GetProductListUrlByCatID(PortalId, CType(Settings("lstProductTabs"), Integer), NonEmptyCat, itemInfo.CategoryName, GetCurrentCulture)
            Else
                strHtmlLink = GetProductListUrlByCatID(PortalId, _TabId, NonEmptyCat, itemInfo.CategoryName, GetCurrentCulture)
            End If

            If NameTemplate = "" Then
                strHtmlText = Lefthtml & itemInfo.CategoryName & RightHtml
            Else
                strHtmlText = ReplaceLinkTokens(NameTemplate, Lefthtml, RightHtml, itemInfo, strHtmlLink)
            End If

            If strHtmlText.StartsWith("<a") Then
                'a href already build into template
                Return strHtmlText
            Else
                strHtmlOut = "<a href=""" & strHtmlLink & """ "
                If (itemInfo.CategoryID = CatID Or _SelectedCategories.ContainsKey(itemInfo.CategoryID)) And SelectCSS <> "" Then
                    strHtmlOut &= "class=""" & SelectCSS & """"
                End If
                strHtmlOut &= ">" & strHtmlText
                strHtmlOut &= "</a>"
                Return strHtmlOut
            End If

        End Function

        Private Function ReplaceLinkTokens(ByVal NameTemplate As String, ByVal Lefthtml As String, ByVal RightHtml As String, ByVal itemInfo As NB_Store_CategoriesInfo, ByVal strHtmlLink As String) As String
            Dim strText As String = ""
            strText = Lefthtml & Replace(NameTemplate, "[TAG:CATEGORYNAME]", itemInfo.CategoryName) & RightHtml
            strText = Replace(strText, "[TAG:PRODUCTCOUNT]", itemInfo.ProductCount.ToString)

            If itemInfo.ImageURL = "" Then
                strText = Replace(strText, "[TAG:IMAGE]", itemInfo.CategoryName)
                strText = Replace(strText, "[TAG:IMAGEURL]", "")
            Else
                strText = Replace(strText, "[TAG:IMAGE]", "<img border=""0"" src=""" & StoreInstallPath & "makethumbnail.ashx?Image=" & QueryStringEncode(PRODUCTIMAGESFOLDER & "\" & System.IO.Path.GetFileName(itemInfo.ImageURL)) & "&w=" & _txtThumbnailSize & "&tabid=" & _TabId & """ alt=""" & itemInfo.CategoryName & """/>")
                strText = Replace(strText, "[TAG:IMAGEURL]", itemInfo.ImageURL)
            End If
            strText = Replace(strText, "[TAG:IMAGEURLTHUMB]", StoreInstallPath & "makethumbnail.ashx?Image=" & QueryStringEncode(PRODUCTIMAGESFOLDER & "\" & System.IO.Path.GetFileName(itemInfo.ImageURL)) & "&w=" & _txtThumbnailSize & "&tabid=" & _TabId)

            strText = Replace(strText, "[TAG:LINK]", strHtmlLink)
            If itemInfo.CategoryID = CatID Then
                strText = Replace(strText, "[TAG:CATEGORYNAMECSS]", Replace(itemInfo.CategoryName, " ", "_") & "sel")
            Else
                strText = Replace(strText, "[TAG:CATEGORYNAMECSS]", Replace(itemInfo.CategoryName, " ", "_"))
            End If
            Return strText
        End Function

        Private Sub populateList(ByVal CatID As Integer, ByVal StaticCatID As Integer)

            Dim aryList As ArrayList
            Dim aryListRoot As ArrayList
            Dim objCtrl As New CategoryController
            Dim objInfo As NB_Store_CategoriesInfo


            If IsNumeric(_txtColumns) Then
                dlCategoryMenu.RepeatColumns = _txtColumns
                dlRootMenu.RepeatColumns = _txtColumns
            Else
                dlRootMenu.RepeatColumns = 4
                dlCategoryMenu.RepeatColumns = 4
            End If


            'populate root menu
            If Not CType(Settings("chkHideRootMenu"), Boolean) And Not _SubMenuOnly Then
                If _chkPatchWork Then
                    Dim aryPatchList As ArrayList
                    aryListRoot = New ArrayList
                    aryPatchList = objCtrl.GetCategories(PortalId, GetCurrentCulture, -1, False, False)

                    Dim catMsg As String = ""

                    For Each objCatInfo As NB_Store_CategoriesInfo In aryPatchList
                        catMsg = Replace(objCatInfo.Message, "&lt;p&gt;&amp;#160;&lt;/p&gt;", "") 'remove fck editor gaff!!
                        If catMsg <> "" Then
                            catMsg = Regex.Replace(catMsg, "\s*", "")
                        End If

                        If objCatInfo.ProductCount > 0 Or catMsg <> "" Then
                            aryListRoot.Add(objCatInfo)
                        End If
                    Next
                Else
                    aryListRoot = objCtrl.GetCategories(PortalId, GetCurrentCulture, 0, False, False)
                End If

                If Not aryListRoot Is Nothing Then
                    dlRootMenu.DataSource = aryListRoot
                    dlRootMenu.DataBind()
                End If
                dlRootMenu.Visible = True

                If CType(Settings("txtRootCSS"), String) <> "" Then
                    dlRootMenu.CssClass = CType(Settings("txtRootCSS"), String)
                End If

                If CType(Settings("txtRootHeadHtml"), String) <> "" Then
                    phRootHead.Controls.Add(New LiteralControl(CType(Settings("txtRootHeadHtml"), String)))
                End If

            Else
                dlRootMenu.Visible = False
            End If

            If CatID = 0 And CType(Settings("chkHideWhenRoot"), Boolean) Then
                dlCategoryMenu.Visible = False
                lblBreadcrumbs.Visible = False
            Else
                lblBreadcrumbs.Visible = False
                If Not _chkHideSubMenu Or Not CType(Settings("chkHideBreadCrumb"), Boolean) Then

                    'populate submenu
                    aryList = objCtrl.GetCategories(PortalId, GetCurrentCulture, StaticCatID, False, False)
                    If Not aryList Is Nothing Then
                        dlCategoryMenu.Visible = True
                        If aryList.Count = 0 Then
                            'no more sub categories so display previous one
                            objInfo = objCtrl.GetCategory(CatID, GetCurrentCulture)
                            If Not objInfo Is Nothing Then
                                If objInfo.ParentCategoryID = 0 Then
                                    'parent category hide sub categories
                                    dlCategoryMenu.Visible = False
                                Else
                                    aryList = objCtrl.GetCategories(PortalId, GetCurrentCulture, objInfo.ParentCategoryID, False, False)
                                End If
                            End If
                        End If

                        If CType(Settings("chkHideBreadCrumb"), Boolean) Or _SubMenuOnly Then
                            lblBreadcrumbs.Visible = False
                        Else
                            lblBreadcrumbs.Visible = True
                            lblBreadcrumbs.Text = BuildBreadCrumbs(CatID, aryList)
                            If lblBreadcrumbs.Text = "" Then lblBreadcrumbs.Visible = False
                        End If

                        'check if sub menu hidden
                        If _chkHideSubMenu Then
                            dlCategoryMenu.Visible = False
                        Else

                            'check if the category is empty, if so skip.
                            If CType(Settings("chkSkipBlankCat"), Boolean) Then
                                For Each objInfo In aryList
                                    If objInfo.ProductCount = 0 Then
                                        objInfo.CategoryID = GetFirstNonEmptyChild(objInfo.CategoryID)
                                    End If
                                Next
                            End If

                            If _txtCSS <> "" Then
                                dlCategoryMenu.CssClass = _txtCSS
                            End If

                            If _txtSubHeadHtml <> "" Then
                                Dim PName As String = ""
                                If aryList.Count > 0 Then
                                    PName = CType(aryList(0), NB_Store_CategoriesInfo).ParentName
                                Else
                                    Dim objCatInfo As NB_Store_CategoriesInfo = objCtrl.GetCategory(CatID, GetCurrentCulture)
                                    If Not objCatInfo Is Nothing Then
                                        PName = objCatInfo.CategoryName
                                    Else
                                        PName = "Category " & CatID.ToString
                                    End If
                                End If
                                phSubHead.Controls.Add(New LiteralControl(Replace(_txtSubHeadHtml, "[TAG:PARENTNAME]", PName)))
                            End If


                            dlCategoryMenu.DataSource = aryList
                            dlCategoryMenu.DataBind()


                        End If
                    Else
                        dlCategoryMenu.Visible = False
                    End If
                End If
            End If

        End Sub

        Private Function GetFirstNonEmptyChild(ByVal CategoryID As Integer) As Integer
            'this function gets the first nonempty child category
            '  or the bottom level category
            Dim objCtrl As New CategoryController
            Dim aryList2 As ArrayList
            aryList2 = objCtrl.GetCategories(PortalId, GetCurrentCulture, CategoryID, False, False)
            If aryList2.Count > 0 Then
                'has sub cats
                If CType(aryList2(0), NB_Store_CategoriesInfo).ProductCount = 0 Then
                    'has no product, so search for next child
                    If CType(aryList2(0), NB_Store_CategoriesInfo).CategoryID <> CategoryID Then
                        Return GetFirstNonEmptyChild(CType(aryList2(0), NB_Store_CategoriesInfo).CategoryID)
                    Else
                        Return CategoryID
                    End If
                Else
                    'has products so display.
                    Return CType(aryList2(0), NB_Store_CategoriesInfo).CategoryID
                End If
            Else
                'has no sub cats, so display
                Return CategoryID
            End If
        End Function

        Private Sub GetSelectedCategories(ByVal CategoryID As Integer, ByRef htList As Hashtable)
            Dim objCtrl As New CategoryController
            Dim objInfo As NB_Store_CategoriesInfo

            objInfo = objCtrl.GetCategory(CategoryID, GetCurrentCulture)
            If Not objInfo Is Nothing Then
                htList.Add(CategoryID, objInfo)
                If objInfo.ParentCategoryID > 0 And htList.Count < 20 Then ' count is just to make sure we don't get infnate loop
                    GetSelectedCategories(objInfo.ParentCategoryID, htList)
                End If
            End If

        End Sub

        Private Function BuildBreadCrumbs(ByVal CatID As Integer, ByVal aryList As ArrayList) As String
            Dim CurrentCatID As Integer = CatID
            Dim strHTML As String = ""
            Dim objCtrl As New CategoryController
            Dim objInfo As NB_Store_CategoriesInfo
            Dim CatName As String = ""
            Dim CurrentCatName As String = ""
            Dim LinkCategoryID As Integer = 0
            Dim BreadCrumbCSS As String = ""
            Dim BreadCrumbSep As String = ""

            Do
                If CurrentCatID > 0 Then
                    objInfo = objCtrl.GetCategory(CurrentCatID, GetCurrentCulture)
                    If Not objInfo Is Nothing Then

                        If CType(Settings("chkHideBreadCrumbRoot"), Boolean) And objInfo.ParentName = "" Then
                            CurrentCatID = objInfo.ParentCategoryID
                        Else
                            'get current cart to add as test to end of breadcrumb
                            If objInfo.CategoryID = CatID Then
                                CurrentCatName = objInfo.CategoryName
                            End If

                            'get category name of parent or take root name
                            CatName = objInfo.ParentName
                            'assign parent category to current
                            CurrentCatID = objInfo.ParentCategoryID

                            LinkCategoryID = CurrentCatID

                            If CType(Settings("chkSkipBlankCat"), Boolean) Then
                                LinkCategoryID = GetFirstNonEmptyChild(LinkCategoryID)
                            End If

                            If CatName = "" Then
                                CatName = Localization.GetString("Root", LocalResourceFile)
                            End If

                            If CType(Settings("txtBreadCrumbSep"), String) = "" Then
                                BreadCrumbSep = " &gt; "
                            Else
                                BreadCrumbSep = CType(Settings("txtBreadCrumbSep"), String)
                            End If
                            If IsNumeric(CType(Settings("lstProductTabs"), String)) Then
                                strHTML = "<a href=""" & NavigateURL(CType(Settings("lstProductTabs"), Integer), "", "CatID=" & LinkCategoryID.ToString) & """" & BreadCrumbCSS & ">" & CatName & "</a>" & BreadCrumbSep & strHTML
                            Else
                                strHTML = "<a href=""" & NavigateURL(_TabId, "", "CatID=" & LinkCategoryID.ToString) & """" & BreadCrumbCSS & ">" & CatName & "</a>" & BreadCrumbSep & strHTML
                            End If
                        End If
                    Else
                        CurrentCatID = 0
                    End If
                End If

            Loop While CurrentCatID > 0

            If strHTML <> "" Then
                strHTML &= CurrentCatName
            End If

            If CType(Settings("txtBreadCrumbCSS"), String) <> "" Then
                strHTML = "<div Class=""" & CType(Settings("txtBreadCrumbCSS"), String) & """>" & strHTML & "</div>"
            Else
                strHTML = "<div>" & strHTML & "</div>"
            End If


            Return strHTML
        End Function

        Private Sub ShowTreeMenu()
            If CType(Settings("chkShowTreeMenu"), Boolean) Then

                Dim objCtrl As New CategoryController
                Dim aryList As ArrayList
                Dim strHtml As String = ""

                If CType(Settings("txtTreeHeadHtml"), String) <> "" Then
                    phTreeHead.Controls.Add(New LiteralControl(CType(Settings("txtTreeHeadHtml"), String)))
                End If

                aryList = objCtrl.GetCategories(PortalId, GetCurrentCulture)

                strHtml = BuildTreeMenu(aryList, "", 0)

                phTreeMenu.Controls.Add(New LiteralControl(strHtml))

                IncludeScripts(PortalId, StoreInstallPath, Page, "categorymenujs.includes", "categorymenustartupjs.includes", "categorymenucss.includes")

            End If
        End Sub

        Private Sub ShowAccordion()
            If CType(Settings("chkShowAccordionMenu"), Boolean) Then

                Dim objCtrl As New CategoryController
                Dim aryList As ArrayList
                Dim strHtml As String = ""

                If CType(Settings("txtAccordionHeadHtml"), String) <> "" Then
                    phTreeHead.Controls.Add(New LiteralControl(CType(Settings("txtAccordionHeadHtml"), String)))
                End If

                aryList = objCtrl.GetCategories(PortalId, GetCurrentCulture)

                strHtml = BuildAccordionMenu(aryList, "", 0, -1)

                phTreeMenu.Controls.Add(New LiteralControl(strHtml))

                IncludeScripts(PortalId, StoreInstallPath, Page, "categorymenujs.includes ", "categorymenustartupjs.includes", "categorymenucss.includes")

            End If
        End Sub


        Private Function BuildTreeMenu(ByVal aryList As ArrayList, ByVal htmlText As String, ByVal ParentID As Integer) As String
            Dim objCInfo As NB_Store_CategoriesInfo
            Dim strHeader As String = ""
            Dim strFooter As String = "</ul>"

            If ParentID = 0 Then
                Dim TreeCSS As String = CType(Settings("txtTreeCSS"), String)
                If TreeCSS = "" Then TreeCSS = "treeview"
                strHeader &= "<ul id=""NBStoreTreeMenu"" class=""" & TreeCSS & """>"
            Else
                strHeader &= "<ul>"
            End If

            For Each objCInfo In aryList
                If objCInfo.ParentCategoryID = ParentID And objCInfo.Archived = False And objCInfo.Hide = False Then
                    htmlText &= "<li>"
                    htmlText &= BuildHtmlCatLink(CType(Settings("txtTreeNameTemplate"), String), CType(Settings("txtTreeLeftHtml"), String), CType(Settings("txtTreeRightHtml"), String), CType(Settings("txtTreeSelectCSS"), String), objCInfo)
                    htmlText &= BuildTreeMenu(aryList, "", objCInfo.CategoryID)
                    htmlText &= "</li>"
                End If
            Next

            If htmlText <> "" Then
                htmlText = strHeader & htmlText & strFooter
            End If

            Return htmlText

        End Function

        Private Function BuildAccordionMenu(ByVal aryList As ArrayList, ByVal htmlText As String, ByVal ParentID As Integer, ByVal AccordionLevel As Integer) As String
            Dim objCInfo As NB_Store_CategoriesInfo
            Dim strHeader As String = ""
            Dim strFooter As String = "</ul>"

            AccordionLevel += 1

            If ParentID = 0 Then
                strHeader &= "<ul id=""NBStoreAccordion"" >"
            Else
                strHeader &= "<ul>"
            End If

            If AccordionLevel > 1 Then
                'clear ul on sub sub level (make only 1 sub level)
                strHeader = ""
                strFooter = ""
            End If


            For Each objCInfo In aryList
                If objCInfo.ParentCategoryID = ParentID And objCInfo.Archived = False And objCInfo.Hide = False Then
                    Dim AccCssClass As String = ""

                    If AccordionLevel > 1 Then
                        htmlText &= "</li><li>"
                    Else
                        htmlText &= "<li>"
                    End If

                    If ParentID = 0 Then
                        AccCssClass = "nbstoremenuhead"
                        AccordionLevel = 0
                    Else
                        AccCssClass = "nbstoremenusub" & AccordionLevel.ToString
                    End If


                    htmlText &= BuildHtmlCatLink(CType(Settings("txtAccordionNameTemplate"), String), CType(Settings("txtAccordionLeftHtml"), String), CType(Settings("txtAccordionRightHtml"), String), AccCssClass, objCInfo)
                    htmlText &= BuildAccordionMenu(aryList, "", objCInfo.CategoryID, AccordionLevel)

                    If AccordionLevel <= 1 Then
                        htmlText &= "</li>"
                    End If

                End If
            Next

            If htmlText <> "" Then
                htmlText = strHeader & htmlText & strFooter
            End If

            Return htmlText

        End Function

#Region "Optional Interfaces"

        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements Entities.Modules.IPortable.ExportModule
            ' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserID As Integer) Implements Entities.Modules.IPortable.ImportModule
            ' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
        End Sub

#End Region


#End Region


    End Class

End Namespace
