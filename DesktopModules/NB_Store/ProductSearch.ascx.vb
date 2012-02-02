' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2008 SARL NevoWeb.  www.nevoweb.com. All rights are reserved.
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
Imports System.Web
Imports System.Text.RegularExpressions
Imports System.IO
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class ProductSearch
        Inherits BaseModule

        Private _OrderByQuery As String
        Private _OrderDESCQuery As Boolean

#Region "Controls"
#End Region

#Region "Private Methods"

        Private Sub ShowHideImages()
            Dim ShowGoImage As String = CType(Settings("ShowGoImage"), String)
            Dim ShowSearchImage As String = CType(Settings("ShowSearchImage"), String)

            Dim bShowGoImage As Boolean = False
            Dim bShowSearchImage As Boolean = False

            If Not ShowGoImage Is Nothing Then
                bShowGoImage = CType(ShowGoImage, Boolean)
            End If

            If Not ShowSearchImage Is Nothing Then
                bShowSearchImage = CType(ShowSearchImage, Boolean)
            End If

            imgSearch.Visible = bShowSearchImage
            plSearch.Visible = False
            imgGo.Visible = bShowGoImage
            cmdGo.Visible = Not bShowGoImage
        End Sub

#End Region

#Region "Public Methods"
        Private Sub SearchExecute()
            Dim ResultsTabid As Integer

            If Not Settings("SearchResultsModule") Is Nothing Then
                ResultsTabid = Integer.Parse(CType(Settings("SearchResultsModule"), String))
            Else
                ResultsTabid = TabId
            End If

            Dim OrderDESCQueryFlag As String = "0"
            If _OrderDESCQuery Then
                OrderDESCQueryFlag = "1"
            End If

            'get first word of search only
            Dim strSearch() As String
            strSearch = txtSearch.Text.Split(" "c)
            If strSearch.GetUpperBound(0) >= 0 Then
                Dim strSearchWord As String = ""
                For lp As Integer = 0 To strSearch.GetUpperBound(0)
                    strSearchWord &= Server.UrlEncode(strSearch(lp)) & "+"
                Next
                strSearchWord = strSearchWord.TrimEnd("+"c)

                'update searchstats
                If GetStoreSettingBoolean(PortalId, "createsearchstats.flag") Then
                    Dim objSCtrl As New StatsController
                    For lp As Integer = 0 To strSearch.GetUpperBound(0)
                        objSCtrl.UpdateSearchWordHits(PortalId, strSearch(lp), lp + 1)
                    Next
                End If

                If DotNetNuke.Entities.Host.HostSettings.GetHostSetting("UseFriendlyUrls") = "Y" Then
                    If _OrderByQuery = "" Then
                        Response.Redirect(NavigateURL(ResultsTabid) & "?Search=" & Server.UrlEncode(strSearchWord))
                    Else
                        Response.Redirect(NavigateURL(ResultsTabid, "", "orderby=" & _OrderByQuery, "desc=" & OrderDESCQueryFlag) & "?Search=" & Server.UrlEncode(strSearchWord))
                    End If
                Else
                    If _OrderByQuery = "" Then
                        Response.Redirect(NavigateURL(ResultsTabid) & "&Search=" & Server.UrlEncode(strSearchWord))
                    Else
                        Response.Redirect(NavigateURL(ResultsTabid, "", "orderby=" & _OrderByQuery, "desc=" & OrderDESCQueryFlag) & "&Search=" & Server.UrlEncode(strSearchWord))
                    End If
                End If
            End If
        End Sub
#End Region

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Dim GoUrl As String = Localization.GetString("imgGo.ImageUrl", Me.LocalResourceFile)
            Dim SearchUrl As String = Localization.GetString("imgSearch.ImageUrl", Me.LocalResourceFile)
            Dim SImageURL As String = CType(Settings("txtSImagePath"), String)
            Dim ImageURL As String = CType(Settings("txtImagePath"), String)

            _OrderByQuery = ""
            If Not Request.Params("orderby") Is Nothing Then
                _OrderByQuery = Request.Params("orderby").ToString
            End If

            _OrderDESCQuery = False
            If Not Request.Params("desc") Is Nothing Then
                _OrderDESCQuery = True
            End If

            If ImageURL = "" Then
                If GoUrl.StartsWith("~") Then
                    imgGo.ImageUrl = GoUrl
                Else
                    imgGo.ImageUrl = Path.Combine(PortalSettings.HomeDirectory, GoUrl)
                End If
            Else
                If ImageURL.StartsWith("~") Then
                    imgGo.ImageUrl = ImageURL
                Else
                    imgGo.ImageUrl = Path.Combine(PortalSettings.HomeDirectory, ImageURL)
                End If
            End If

            If SImageURL = "" Then
                If SearchUrl.StartsWith("~") Then
                    imgSearch.ImageUrl = SearchUrl
                Else
                    imgSearch.ImageUrl = Path.Combine(PortalSettings.HomeDirectory, SearchUrl)
                End If
            Else
                If SImageURL.StartsWith("~") Then
                    imgSearch.ImageUrl = SImageURL
                Else
                    imgSearch.ImageUrl = Path.Combine(PortalSettings.HomeDirectory, SImageURL)
                End If
            End If

            ShowHideImages()

            cmdGo.Text = Localization.GetString("cmdGo.Text", Me.LocalResourceFile)

        End Sub

        Private Sub imgGo_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgGo.Click
            SearchExecute()
        End Sub

        Private Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click
            SearchExecute()
        End Sub

        Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
            SearchExecute()
        End Sub

#End Region

    End Class

End Namespace
