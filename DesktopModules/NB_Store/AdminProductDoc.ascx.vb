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
Imports DotNetNuke.Services.FileSystem

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class AdminProductDoc
        Inherits Framework.UserControlBase

#Region "Private Members"
        Private ResourceFile As String
#End Region

        Public Event AddButton(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event DeleteDoc(ByVal DocID As Integer)
        Public Event SelectDoc(ByVal FileName As String, ByVal DocDesc As String, ByVal DocPath As String, ByVal FileExt As String)
        Public Event SearchDocs(ByVal FilterText As String)

#Region "Events"


        Private Sub cmdAddDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddDoc.Click
            RaiseEvent AddButton(sender, e)
        End Sub

        Private Sub dgDocs_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDocs.DeleteCommand
            Try
                Dim item As DataGridItem = e.Item
                Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
                Dim objCtrl As New ProductController
                objCtrl.DeleteProductDoc(ItemId)
                RaiseEvent DeleteDoc(ItemId)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub dgDocs_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDocs.ItemDataBound
            Dim item As DataGridItem = e.Item
            If item.ItemType = ListItemType.Item Or _
                item.ItemType = ListItemType.AlternatingItem Or _
                item.ItemType = ListItemType.SelectedItem Then

                Dim imgColumnControl As Control = item.Controls(1).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                    remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdDeleteDoc", ResourceFile) & "');")
                End If

            End If

        End Sub

        Private Sub dgDocs_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgDocs.PreRender
            Dim dg As DataGrid = sender
            ResourceFile = Services.Localization.Localization.GetResourceFile(Me, Me.GetType().BaseType.Name + ".ascx")

            If dg.Controls.Count > 0 Then
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("ShowSelectLang"), ShowSelectLang).Refresh()
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("nlName"), Label).Text = Localization.GetString("nlName", ResourceFile)
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("ShowSelectLang2"), ShowSelectLang).Refresh()
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("nlDescription"), Label).Text = Localization.GetString("nlDescription", ResourceFile)
            End If

        End Sub


        Private Sub dgSelectDoc_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSelectDoc.EditCommand
            Try
                RaiseEvent SelectDoc(e.Item.Cells(1).Text, e.Item.Cells(2).Text, e.Item.Cells(3).Text, e.Item.Cells(4).Text)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dgSelectDoc_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSelectDoc.PageIndexChanged
            dgSelectDoc.CurrentPageIndex = e.NewPageIndex
            RaiseEvent SearchDocs(txtSearch.Text)
        End Sub

        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            dgSelectDoc.CurrentPageIndex = 0
            RaiseEvent SearchDocs(txtSearch.Text)
        End Sub


#End Region

#Region "Methods"

        Public Sub HideSelectList()
            dgSelectDoc.Visible = False
        End Sub


        Public Sub populateSelectDocs(ByVal Lang As String, ByVal FilterText As String)
            Dim objCtrl As New ProductController

            ResourceFile = Services.Localization.Localization.GetResourceFile(Me, Me.GetType().BaseType.Name + ".ascx")
            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgSelectDoc, ResourceFile)

            ' get content
            Dim aryList As ArrayList

            aryList = objCtrl.GetProductSelectDocList(Lang, FilterText, PortalSettings.PortalId)

            dgSelectDoc.DataSource = aryList
            dgSelectDoc.DataBind()

            dgSelectDoc.Visible = True

        End Sub


        Public Sub populateDocs(ByVal ProductID As Integer, ByVal Lang As String)
            Dim objCtrl As New ProductController

            ResourceFile = Services.Localization.Localization.GetResourceFile(Me, Me.GetType().BaseType.Name + ".ascx")
            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgDocs, ResourceFile)

            ' get content
            Dim aryList As ArrayList

            aryList = objCtrl.GetProductDocList(ProductID, Lang)

            dgDocs.DataSource = aryList
            dgDocs.DataBind()

        End Sub

        Public Function DocsCount() As Integer
            Return dgDocs.Items.Count
        End Function

        Public Sub updateDocs(ByVal Lang As String)
            Dim objCtrl As New ProductController
            Dim objInfo As NB_Store_ProductDocInfo
            Dim i As DataGridItem
            Dim DocID As Integer
            For Each i In dgDocs.Items
                DocID = CInt(i.Cells(0).Text)
                objInfo = objCtrl.GetProductDoc(DocID, Lang)
                If Not objInfo Is Nothing Then
                    objInfo.DocDesc = CType(i.FindControl("txtDocDesc"), TextBox).Text
                    objInfo.ListOrder = CType(i.FindControl("txtListOrder"), TextBox).Text
                    objInfo.Hidden = CType(i.FindControl("chkHide"), CheckBox).Checked
                    objInfo.FileName = CType(i.FindControl("txtDocName"), TextBox).Text
                    objInfo.Lang = Lang
                    objCtrl.UpdateObjProductDoc(objInfo)
                End If
            Next
        End Sub

        Public Sub UploadDoc(ByVal ProdID As Integer, ByVal Lang As String)
            Dim strMsg As String = ""
            Dim fs As New FileSystemUtils
            Dim objCtrl As New ProductController
            Dim HideFlag As Boolean = False
            Dim strGUID As String

            If cmdBrowse.FileName <> "" Then

                CreateDir(PortalSettings, PRODUCTDOCSFOLDER)

                Dim NewFileName As String = ""

                strGUID = Guid.NewGuid.ToString
                Dim objDInfo As New NB_Store_ProductDocInfo

                objDInfo.DocID = -1
                objDInfo.DocDesc = ""
                objDInfo.DocPath = PortalSettings.HomeDirectoryMapPath & PRODUCTDOCSFOLDER & "\" & strGUID & System.IO.Path.GetExtension(cmdBrowse.FileName)
                objDInfo.FileExt = System.IO.Path.GetExtension(cmdBrowse.FileName)
                objDInfo.FileName = System.IO.Path.GetFileNameWithoutExtension(cmdBrowse.FileName)
                objDInfo.Hidden = False
                objDInfo.Lang = Lang
                objDInfo.ListOrder = 1
                objDInfo.ProductID = ProdID

                Dim strFolderpath As String = GetSubFolderPath(objDInfo.DocPath, PortalSettings.PortalId)

                FileSystemUtils.UploadFile(objDInfo.DocPath, cmdBrowse.PostedFile)
                FileSystemUtils.MoveFile(objDInfo.DocPath & cmdBrowse.FileName, objDInfo.DocPath, PortalSettings)

                Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo = FileSystemUtils.GetFolder(PortalSettings.PortalId, PRODUCTDOCSFOLDER)
                If folderInfo.StorageLocation = FolderController.StorageLocationTypes.SecureFileSystem Then
                    objDInfo.DocPath = PortalSettings.HomeDirectoryMapPath & PRODUCTDOCSFOLDER & "\" & strGUID & System.IO.Path.GetExtension(cmdBrowse.FileName) & glbProtectedExtension
                End If

                objCtrl.UpdateObjProductDoc(objDInfo)

            End If
        End Sub

#End Region

    End Class

End Namespace
