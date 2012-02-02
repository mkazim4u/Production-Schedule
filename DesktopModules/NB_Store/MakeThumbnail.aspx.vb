' DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2006
' by Shaun Walker ( sales@perpetualmotion.ca ) of Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'

Option Strict On

Imports System.IO
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Imaging

Namespace NEvoWeb.Modules.NB_Store

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    '''     Resizes portal images to the specified width and sends them as binary data.
    '''     Usage: <img src="makethumbnail.ashx?Image=[file]&w=[width]" />
    ''' </summary>
    ''' <remarks>
    '''     Security facts:
    '''     -   Access to other filetypes than images is prevented by opening every file with the 
    '''         Bitmap-Constructor. Unsupported filetypes throw a System.ArgumentException.
    '''     -   Image-parameter is limited to virtual paths. The usage of full qualified URLs 
    '''         causes a System.ArgumentException (thrown by the MapPath-method).
    '''     -   Access is limited to files in the current portal. The attempt to access a file
    '''         in another portal will throw a System.SecurityException. 
    ''' </remarks>
    ''' <history>
    '''     [scullmann]  10/26/2005 Class added as enhancements from gammacon.UserDefinedTable
    '''     [mhamburger] 10/27/2005 Changed Resize Method, so that backcolor can be specified
    '''     [mhamburger] 11/02/2005 Added security fix to allow access only to images within the current portal
    '''                             Added known and testes security facts as remarks.
    '''     [sleupold]   10/27/2005 Support for width parameter added.
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class MakeThumbnail
        Implements System.Web.IHttpHandler

#Region "Event Handlers"
        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

        Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
            Dim iFormat As ImageFormat = Nothing
            Dim intMaxWidth As Integer = 0
            Dim intMaxHeight As Integer = 0
            Dim strCacheKey As String = ""
            Dim sourceImage As Bitmap = Nothing
            Dim cropImage As Bitmap = Nothing
            Dim newImage As Bitmap = Nothing
            Dim Request As System.Web.HttpRequest = context.Request
            Dim cropType As String = ""
            Dim _PortalID As Integer = -1

            ' Get max. width, if any
            If Not Request.Params("w") = String.Empty Then
                intMaxWidth = Int32.Parse(Request.QueryString("w"))
            End If

            ' Get max. height, if any
            If Not Request.Params("h") = String.Empty Then
                intMaxHeight = Int32.Parse(Request.QueryString("h"))
            End If

            ' Get PortalID, if any
            If Not Request.Params("p") = String.Empty Then
                _PortalID = Int32.Parse(Request.QueryString("p"))
            End If

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

            Dim strFilepath As String = ""
            Dim HomeDirectoryMapPath As String = ""

            HomeDirectoryMapPath = Entities.Portals.PortalController.GetCurrentPortalSettings.HomeDirectoryMapPath

            If Not IsNumeric(Request.Params("Image")) Then
                ' Entities.Portals.PortalSettings - (Doesn't work on child portals, brings back parent portal settings)
                ' Get source image path
                strFilepath = HomeDirectoryMapPath & Request.Params("Image") 'only virtual paths are valid!

                'pick up "noimage.png" if can't find
                If Not File.Exists(strFilepath) Then
                    strFilepath = HomeDirectoryMapPath & "noimage.png"
                End If

                ' Check cache for thumbnail
                strCacheKey = strFilepath.Substring(strFilepath.LastIndexOf("\Portals\")) & intMaxWidth & "x" & intMaxHeight
            Else
                ' Check cache for thumbnail
                strCacheKey = "NB_Store" & Request.Params("Image") & "." & intMaxWidth & "x" & intMaxHeight
                If DataCache.GetCache(strCacheKey) Is Nothing Then
                    Dim objPCtrl As New ProductController
                    Dim objPIInfo As NB_Store_ProductImageInfo
                    objPIInfo = objPCtrl.GetProductImage(CInt(Request.Params("Image")), "")
                    If objPIInfo Is Nothing Then
                        strFilepath = HomeDirectoryMapPath & "noimage.png"
                    Else
                        strFilepath = objPIInfo.ImagePath
                    End If
                End If
            End If


            If DataCache.GetCache(strCacheKey) Is Nothing Then

                ' Get source Image
                If File.Exists(strFilepath) Then
                    sourceImage = New Bitmap(strFilepath)
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

                        'Cache thumbnail clone (Disposing a cached image will destroy its cache too)
                        DataCache.SetCache(strCacheKey, newImage.Clone)
                    Else 'use original width (no maxwidth given or image is narrow enough: 
                        newImage = sourceImage
                    End If
                End If
            Else
                ' Get (cloned) cached thumbnail
                newImage = CType(CType(DataCache.GetCache(strCacheKey), Bitmap).Clone, Bitmap)
                iFormat = ImageFormat.Jpeg 'ImageFormat.Gif
            End If

            If Not iFormat Is Nothing And Not newImage Is Nothing Then
                ' Send image to the browser.
                Dim info() As System.Drawing.Imaging.ImageCodecInfo

                info = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders
                Dim encoderParameters As System.Drawing.Imaging.EncoderParameters
                encoderParameters = New System.Drawing.Imaging.EncoderParameters(1)
                encoderParameters.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, CType(85L, Int32))

                context.Response.ContentType = GetContentType(iFormat)
                newImage.Save(context.Response.OutputStream, info(1), encoderParameters)

                ' Clean up
                newImage.Dispose()
            End If
            If Not sourceImage Is Nothing Then sourceImage.Dispose()
        End Sub
#End Region

#Region "Private Methods"
        Private Function GetContentType(ByVal iFormat As ImageFormat) As String
            Dim contentType As String

            If iFormat.Guid.Equals(ImageFormat.Jpeg.Guid) Then
                contentType = "image/jpeg"
            ElseIf iFormat.Guid.Equals(ImageFormat.Gif.Guid) Then
                contentType = "image/gif"
            ElseIf iFormat.Guid.Equals(ImageFormat.Png.Guid) Then
                contentType = "image/png"
            ElseIf iFormat.Guid.Equals(ImageFormat.Tiff.Guid) Then
                contentType = "image/tiff"
            ElseIf iFormat.Guid.Equals(ImageFormat.Bmp.Guid) Then
                contentType = "image/x-ms-bmp"
            Else
                contentType = "image/jpeg"
            End If
            Return contentType
        End Function
#End Region


    End Class

End Namespace
