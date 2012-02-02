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

Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports DotNetNuke.Common.Utilities
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Imaging

Namespace NEvoWeb.Modules.NB_Store

    Public Class ThumbFunctions

        Public _ImageQuality As Integer = 85


        Public Sub CreateAllThumbsOnDisk(ByVal PortalID As Integer, ByVal Lang As String, ByVal ThumbSize As String)
            Dim objCtrl As New ProductController
            Dim aryList As ArrayList

            'clear down existing thumbs
            Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo
            Dim fileInfo As DotNetNuke.Services.FileSystem.FileInfo
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings


            folderInfo = FileSystemUtils.GetFolder(PortalID, PRODUCTTHUMBSFOLDER)
            If Not folderInfo Is Nothing Then
                aryList = FileSystemUtils.GetFilesByFolder(PortalID, folderInfo.FolderID)
                For Each fileInfo In aryList
                    Try
                        FileSystemUtils.DeleteFile(PS.HomeDirectoryMapPath & PRODUCTTHUMBSFOLDER & "\" & fileInfo.FileName, PS, True)
                    Catch ex As Exception
                        'maybe locked, ignore for this operation
                    End Try
                    fileInfo = Nothing
                Next
            End If
            folderInfo = Nothing

            aryList = objCtrl.GetProductExportList(PortalID, Lang, False)
            For Each objInfo As NB_Store_ProductsInfo In aryList
                CreateProductThumbsOnDisk(objInfo.ProductID, Lang, ThumbSize)
            Next

        End Sub

        Public Sub CreateProductThumbsOnDisk(ByVal PortalID As Integer, ByVal ProductID As Integer)
            Dim ThumbSizes As String = GetStoreSetting(PortalId, "diskthumbnails.size")
            CreateProductThumbsOnDisk(ProductID, GetCurrentCulture, ThumbSizes)
        End Sub

        Public Sub CreateProductThumbsOnDisk(ByVal ProductID As Integer, ByVal Lang As String, ByVal ThumbSize As String)
            Dim objCtrl As New ProductController
            Dim aryList As ArrayList

            aryList = objCtrl.GetProductImageList(ProductID, Lang)
            For Each objInfo As NB_Store_ProductImageInfo In aryList
                CreateThumbOnDisk(objInfo.ImagePath, ThumbSize)
            Next

        End Sub


        Public Function getThumbWidth(ByVal ThumbSize As String) As String
            Dim ThumbW As String
            If Not IsNumeric(ThumbSize) And ThumbSize <> "" Then
                Dim ThumbSplit() As String
                ThumbSplit = ThumbSize.Split("x"c)
                ThumbW = ThumbSplit(0)
            Else
                ThumbW = ThumbSize
            End If
            Return ThumbW
        End Function


        Public Function getThumbHeight(ByVal ThumbSize As String) As String
            Dim ThumbH As String = "0"
            If Not IsNumeric(ThumbSize) And ThumbSize <> "" Then
                Dim ThumbSplit() As String
                ThumbSplit = ThumbSize.Split("x"c)
                If ThumbSplit.GetUpperBound(0) = 1 Then
                    ThumbH = ThumbSplit(1)
                End If
            End If
            Return ThumbH
        End Function

        Public Function GetThumbFileName(ByVal ImgPathName As String, ByVal ThumbW As String, ByVal ThumbH As String) As String
            Return Path.GetFileNameWithoutExtension(ImgPathName) & "_" & ThumbW & "x" & ThumbH & Path.GetExtension(ImgPathName)
        End Function

        Public Function GetThumbFilePathName(ByVal ImgPathName As String, ByVal ThumbW As String, ByVal ThumbH As String) As String
            Return Replace(ImgPathName, PRODUCTIMAGESFOLDER & "\" & Path.GetFileName(ImgPathName), PRODUCTTHUMBSFOLDER & "\" & GetThumbFileName(ImgPathName, ThumbW, ThumbH))
        End Function

        Public Function GetThumbURLName(ByVal ImgPathName As String, ByVal ThumbW As String, ByVal ThumbH As String) As String
            Return Replace(ImgPathName, PRODUCTIMAGESFOLDER & "/" & Path.GetFileName(ImgPathName), PRODUCTTHUMBSFOLDER & "/" & GetThumbFileName(ImgPathName, ThumbW, ThumbH))
        End Function


        Public Sub CreateThumbOnDisk(ByVal ImgPathName As String, ByVal ThumbSize As String)
            Dim ThumbSizeList As String()
            Dim ThumbW As String = ""
            Dim ThumbH As String = ""

            If ThumbSize <> "" Then
                If ImgPathName <> "" Then

                    ThumbSizeList = ThumbSize.Split(","c)
                    For lp As Integer = 0 To ThumbSizeList.GetUpperBound(0)

                        ThumbW = getThumbWidth(ThumbSizeList(lp))
                        ThumbH = getThumbHeight(ThumbSizeList(lp))

                        Dim FilePathOut As String
                        FilePathOut = GetThumbFilePathName(ImgPathName, ThumbW, ThumbH)

                        Using newImage As Bitmap = GetThumbnailExtra(ImgPathName, CInt(ThumbW), CInt(ThumbH))

                            If Not newImage Is Nothing Then
                                Dim info() As System.Drawing.Imaging.ImageCodecInfo
                                info = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders
                                Dim encoderParameters As System.Drawing.Imaging.EncoderParameters
                                encoderParameters = New System.Drawing.Imaging.EncoderParameters(1)
                                encoderParameters.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, _ImageQuality)

                                Try
                                    newImage.Save(FilePathOut, info(1), encoderParameters)
                                Catch ex As Exception
                                    GC.Collect() ' attempt to clear all file locks and try again
                                    Try
                                        newImage.Save(FilePathOut, info(1), encoderParameters)
                                    Catch exSkip As Exception
                                        'abandon save. 
                                        'Assumption is the thumb already is there, but locked. So no need for error.
                                    End Try
                                End Try

                                ' Clean up
                                newImage.Dispose()
                            End If
                        End Using
                    Next
                End If
            End If


        End Sub


        Public Function GetThumbnailExtra(ByVal strFilepath As String, ByVal intMaxWidth As Integer, ByVal intMaxHeight As Integer) As Bitmap
            Dim iFormat As ImageFormat = Nothing
            Dim cropImage As Bitmap = Nothing
            Dim newImage As Bitmap = Nothing
            Dim cropType As String = ""


            If intMaxHeight <> 0 Then
                If intMaxHeight > intMaxWidth Then
                    cropType = "P"
                ElseIf intMaxHeight = intMaxWidth Then
                    cropType = "S"
                Else
                    cropType = "L"
                End If
            Else
                cropType = ""
                intMaxHeight = intMaxWidth
            End If

            If System.IO.File.Exists(strFilepath) Then

                Using sourceImage As Bitmap = New Bitmap(strFilepath)

                    'previous: Uncached Bitmaps will keep their format
                    'iFormat = sourceImage.RawFormat
                    'new: Uncached Bitmaps will be transformed to Jpeg independent from their original format
                    iFormat = ImageFormat.Jpeg
                    Dim intSourceWidth As Integer = sourceImage.Width
                    Dim intSourceHeight As Integer = sourceImage.Height
                    If (intMaxWidth > 0 And intMaxWidth < intSourceWidth) Or (intMaxHeight > 0 And intMaxHeight < intSourceHeight) Then
                        ' Resize image:
                        Dim aspect As Double
                        aspect = sourceImage.PhysicalDimension.Width / sourceImage.PhysicalDimension.Height

                        Dim NewWidth As Integer
                        Dim NewHeight As Integer
                        If sourceImage.PhysicalDimension.Height < sourceImage.PhysicalDimension.Width Then
                            NewWidth = intMaxWidth
                            NewHeight = CInt(intMaxWidth / aspect)
                        Else
                            NewWidth = CInt(intMaxWidth * aspect)
                            NewHeight = intMaxWidth
                        End If

                        If cropType = "" Then
                            newImage = New Bitmap(NewWidth, NewHeight)
                            Dim g As Graphics = Graphics.FromImage(newImage)
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic

                            g.FillRectangle(Brushes.White, 0, 0, NewWidth, NewHeight)
                            g.DrawImage(sourceImage, 0, 0, NewWidth, NewHeight)
                        ElseIf cropType = "L" Then
                            NewWidth = intMaxWidth
                            NewHeight = CInt(intMaxWidth / aspect)

                            cropImage = New Bitmap(NewWidth, NewHeight)
                            Dim gc As Graphics = Graphics.FromImage(cropImage)
                            gc.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                            gc.FillRectangle(Brushes.White, 0, 0, NewWidth, NewHeight)
                            gc.DrawImage(sourceImage, 0, 0, NewWidth, NewHeight)

                            Dim DestinationRec As New Drawing.Rectangle(0, 0, NewWidth, intMaxHeight)
                            newImage = New Bitmap(NewWidth, intMaxHeight)
                            Dim g As Graphics = Graphics.FromImage(newImage)
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                            g.FillRectangle(Brushes.White, 0, 0, NewWidth, intMaxHeight)
                            g.DrawImage(cropImage, DestinationRec, 0, CInt((cropImage.Height - intMaxHeight) / 2), NewWidth, intMaxHeight, Drawing.GraphicsUnit.Pixel)
                        ElseIf cropType = "P" Then
                            NewWidth = CInt(intMaxHeight * aspect)
                            NewHeight = intMaxHeight

                            cropImage = New Bitmap(NewWidth, NewHeight)
                            Dim gc As Graphics = Graphics.FromImage(cropImage)
                            gc.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                            gc.FillRectangle(Brushes.White, 0, 0, NewWidth, NewHeight)
                            gc.DrawImage(sourceImage, 0, 0, NewWidth, NewHeight)

                            Dim DestinationRec As New Drawing.Rectangle(0, 0, intMaxWidth, NewHeight)
                            newImage = New Bitmap(intMaxWidth, NewHeight)
                            Dim g As Graphics = Graphics.FromImage(newImage)
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                            g.FillRectangle(Brushes.White, 0, 0, intMaxWidth, NewHeight)
                            g.DrawImage(cropImage, DestinationRec, CInt((cropImage.Width - intMaxWidth) / 2), 0, intMaxWidth, NewHeight, Drawing.GraphicsUnit.Pixel)
                        ElseIf cropType = "S" Then

                            If sourceImage.PhysicalDimension.Height < sourceImage.PhysicalDimension.Width Then
                                NewWidth = CInt(intMaxHeight * aspect)
                                NewHeight = intMaxHeight
                            Else
                                NewWidth = intMaxWidth
                                NewHeight = CInt(intMaxWidth / aspect)
                            End If

                            cropImage = New Bitmap(NewWidth, NewHeight)
                            Dim gc As Graphics = Graphics.FromImage(cropImage)
                            gc.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                            gc.FillRectangle(Brushes.White, 0, 0, NewWidth, NewHeight)
                            gc.DrawImage(sourceImage, 0, 0, NewWidth, NewHeight)

                            Dim DestinationRec As New Drawing.Rectangle(0, 0, intMaxWidth, intMaxHeight)
                            newImage = New Bitmap(intMaxWidth, intMaxHeight)
                            Dim g As Graphics = Graphics.FromImage(newImage)
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                            g.FillRectangle(Brushes.White, 0, 0, intMaxWidth, intMaxHeight)
                            g.DrawImage(cropImage, DestinationRec, 0, CInt((cropImage.Height - intMaxHeight) / 2), intMaxWidth, intMaxHeight, Drawing.GraphicsUnit.Pixel)
                        End If

                    Else 'use original width (no maxwidth given or image is narrow enough: 
                        newImage = sourceImage
                        newImage.SetResolution(72, 72)
                    End If

                    If Not sourceImage Is Nothing Then sourceImage.Dispose()
                End Using

            End If

            Return newImage

        End Function

    End Class


End Namespace

