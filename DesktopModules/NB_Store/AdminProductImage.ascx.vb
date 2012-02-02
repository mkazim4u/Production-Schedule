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

    Partial Public Class AdminProductImage
        Inherits Framework.UserControlBase

#Region "Private Members"
        Private ResourceFile As String
#End Region

        Public Event AddButton(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event DeleteImage()

#Region "Events"

        Private Sub cmdAddImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddImage.Click
            RaiseEvent AddButton(sender, e)
        End Sub

        Private Sub dgImages_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgImages.DeleteCommand
            Try
                Dim item As DataGridItem = e.Item
                Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
                Dim objCtrl As New ProductController
                objCtrl.DeleteProductImage(ItemId)
                RaiseEvent DeleteImage()
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub dgImages_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgImages.ItemDataBound
            Dim item As DataGridItem = e.Item

            If item.ItemType = ListItemType.Item Or _
                item.ItemType = ListItemType.AlternatingItem Or _
                item.ItemType = ListItemType.SelectedItem Then

                Dim dgImg As Image = DirectCast(e.Item.FindControl("imgProduct"), Image)

                If Not dgImg Is Nothing Then
                    Dim strIMGSRC As String = Me.TemplateSourceDirectory & "/" & "makethumbnail.ashx?Image=" & DirectCast(e.Item.DataItem, NB_Store_ProductImageInfo).ImageID & "&w=60&portalid=" & PortalSettings.PortalId.ToString
                    dgImg.ImageUrl = strIMGSRC
                End If

                Dim imgColumnControl As Control = item.Controls(1).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                    remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdDeleteImage", ResourceFile) & "');")
                End If

            End If

        End Sub

        Private Sub dgImages_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgImages.PreRender
            Dim dg As DataGrid = sender
            ResourceFile = Services.Localization.Localization.GetResourceFile(Me, Me.GetType().BaseType.Name + ".ascx")

            If dg.Controls.Count > 0 Then
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("ShowSelectLang"), ShowSelectLang).Refresh()
                DirectCast(dg.Controls(0).Controls(0).Controls(0).FindControl("nlDescription"), Label).Text = Localization.GetString("nlName", ResourceFile)
            End If

        End Sub


#End Region

#Region "Methods"

        Public Sub populateImages(ByVal ProductID As Integer, ByVal Lang As String)
            Dim objCtrl As New ProductController

            ResourceFile = Services.Localization.Localization.GetResourceFile(Me, Me.GetType().BaseType.Name + ".ascx")
            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgImages, ResourceFile)

            ' get content
            Dim aryList As ArrayList

            aryList = objCtrl.GetProductImageList(ProductID, Lang)

            dgImages.DataSource = aryList
            dgImages.DataBind()


        End Sub

        Public Sub updateImages(ByVal Lang As String)
            Dim objCtrl As New ProductController
            Dim objInfo As NB_Store_ProductImageInfo
            Dim i As DataGridItem
            Dim ImageID As Integer
            For Each i In dgImages.Items
                ImageID = CInt(i.Cells(0).Text)
                objInfo = objCtrl.GetProductImage(ImageID, Lang)
                If Not objInfo Is Nothing Then
                    objInfo.ImageDesc = CType(i.FindControl("txtImageDesc"), TextBox).Text
                    objInfo.ListOrder = CType(i.FindControl("txtListOrder"), TextBox).Text
                    objInfo.Hidden = CType(i.FindControl("chkHide"), CheckBox).Checked
                    objInfo.ImageURL = PortalSettings.HomeDirectory & PRODUCTIMAGESFOLDER & "/" & System.IO.Path.GetFileName(objInfo.ImagePath)
                    objInfo.Lang = Lang
                    objCtrl.UpdateObjProductImage(objInfo)
                End If
            Next
        End Sub

        Public Sub UploadImage(ByVal ProdID As Integer, ByVal Lang As String, ByVal ImageSize As Integer, Optional ByVal ImageQuality As Integer = 85, Optional ByVal InterpolationMode As Integer = 7, Optional ByVal SmoothingMode As Integer = 2, Optional ByVal PixelOffsetMode As Integer = 0, Optional ByVal CompositingQuality As Integer = 0)
            Dim strMsg As String = ""
            Dim fs As New FileSystemUtils
            Dim objCtrl As New ProductController
            Dim HideFlag As Boolean = False

            If cmdBrowse.FileName <> "" Then

                'try and clear any file that may be in temp directory (IIS may lock them)
                RemoveFiles(PortalSettings, "temp")

                Dim NewFileName As String = ""
                Dim arylist As ArrayList

                arylist = objCtrl.GetProductImageList(ProdID, GetCurrentCulture)
                NewFileName = ProdID & "_" & arylist.Count & Left(System.Guid.NewGuid.ToString, 4) & System.IO.Path.GetExtension(cmdBrowse.FileName)

                If arylist.Count = 0 Then
                    HideFlag = True
                End If

                CreateDir(PortalSettings, "temp")
                CreateDir(PortalSettings, PRODUCTIMAGESFOLDER)

                FileSystemUtils.SaveFile(PortalSettings.HomeDirectoryMapPath & "temp\" & NewFileName, cmdBrowse.FileBytes)

                Dim objImgResize As New ImgReSize
                objImgResize._ImageQuality = ImageQuality
                objImgResize._InterpolationMode = InterpolationMode
                objImgResize._CompositingQuality = CompositingQuality
                objImgResize._PixelOffsetMode = PixelOffsetMode
                objImgResize._SmoothingMode = SmoothingMode
                Dim strUploadFile As String = ""
                strUploadFile = objImgResize.ResizeImageFile(PortalSettings, PortalSettings.HomeDirectoryMapPath & PRODUCTIMAGESFOLDER, PortalSettings.HomeDirectoryMapPath & "temp\" & NewFileName, ImageSize)

                objCtrl.AddNewImage(ProdID, Lang, PortalSettings.HomeDirectoryMapPath & PRODUCTIMAGESFOLDER & "\" & System.IO.Path.GetFileName(strUploadFile), PortalSettings.HomeDirectory & PRODUCTIMAGESFOLDER & "/" & System.IO.Path.GetFileName(strUploadFile), HideFlag)

                ''clear down temp folder 
                FileSystemUtils.DeleteFile(PortalSettings.HomeDirectoryMapPath & "temp\" & NewFileName, PortalSettings, True)

                'create thumbnails
                If GetStoreSettingBoolean(PortalSettings.PortalId, "diskthumbnails.flag") Then
                    Dim objThumb As New ThumbFunctions
                    If IsNumeric(GetStoreSetting(PortalSettings.PortalId, "image.quality")) Then
                        objThumb._ImageQuality = CInt(GetStoreSetting(PortalSettings.PortalId, "image.quality"))
                    End If
                    objThumb.CreateProductThumbsOnDisk(PortalSettings.PortalId, ProdID)
                End If


            End If
        End Sub

#End Region

    End Class

End Namespace
