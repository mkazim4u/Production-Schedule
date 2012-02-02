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


Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports NEvoWeb.Modules.NB_Store.SharedFunctions


Namespace NEvoWeb.Modules.NB_Store



    Public Class ImgReSize

        Public _ImageQuality As Integer = 85
        Public _SmoothingMode As Integer = 2
        Public _InterpolationMode As Integer = 7
        Public _PixelOffsetMode As Integer = 0
        Public _CompositingQuality As Integer = 0


        Public Function ResizeImageFile(ByVal PortalSettings As DotNetNuke.Entities.Portals.PortalSettings, ByVal DestFolderPath As String, ByVal SrcFilePathName As String, ByVal imgSize As Double) As String
            Dim fOut As String = ""
            Dim colDelFile As New Collection

            DestFolderPath = FileSystemUtils.FormatFolderPath(DestFolderPath)
            If IsImageFile(System.IO.Path.GetExtension(SrcFilePathName)) Then
                If imgSize > 0 Or System.IO.Path.GetExtension(SrcFilePathName) = ".bmp" Or System.IO.Path.GetExtension(SrcFilePathName) = ".tiff" Then
                    fOut = ConvertImage(PortalSettings, SrcFilePathName, DestFolderPath & Path.GetFileName(SrcFilePathName), imgSize)
                Else
                    If DestFolderPath.EndsWith("/") Then DestFolderPath = DestFolderPath.TrimEnd("/") & "\"
                    fOut = DestFolderPath & Path.GetFileName(SrcFilePathName)
                    Try
                        'will fail with medium trust
                        System.IO.File.Move(SrcFilePathName, fOut)
                    Catch ex As Exception
                        'try this, doesn't seem to work, and haven;t got time to fix at the moment
                        FileSystemUtils.MoveFile(SrcFilePathName, fOut, PortalSettings)
                    End Try
                End If
            End If
            Return fOut
        End Function


        Public Function ConvertImage(ByVal PortalSettings As DotNetNuke.Entities.Portals.PortalSettings, ByVal FileNameIN As String, ByVal FileNameOUT As String, ByVal imgSize As Double) As String
            Try
                Return DoResizeThumb(FileNameIN, ReplaceFileExt(FileNameOUT, ".jpg"), imgSize)
            Catch exc As Exception
                Return ""
            End Try
        End Function


        Public Function DoResizeThumb(ByVal FileNamePath As String, ByVal FileNamePathOut As String, ByVal intMaxWidth As Double) As String
            Try

                Dim newImage As Bitmap

                ' Get source Image
                Using sourceImage As Bitmap = New Bitmap(FileNamePath)

                    Dim intSourceWidth As Integer = sourceImage.Width
                    If intMaxWidth > 0 And (intMaxWidth < intSourceWidth Or intMaxWidth < sourceImage.Height) Then
                        ' Resize image:
                        Dim aspect As Double
                        aspect = sourceImage.PhysicalDimension.Width / sourceImage.PhysicalDimension.Height

                        Dim NewWidth As Integer
                        Dim NewHeight As Integer
                        If sourceImage.PhysicalDimension.Height < sourceImage.PhysicalDimension.Width Then
                            NewWidth = CInt(intMaxWidth)
                            NewHeight = CInt(intMaxWidth / aspect)
                        Else
                            NewWidth = CInt(intMaxWidth * aspect)
                            NewHeight = CInt(intMaxWidth)
                        End If

                        newImage = New Bitmap(NewWidth, NewHeight)
                        Dim g As Graphics = Graphics.FromImage(newImage)

                        g.InterpolationMode = _InterpolationMode
                        g.SmoothingMode = _SmoothingMode
                        g.PixelOffsetMode = _PixelOffsetMode
                        g.CompositingQuality = _CompositingQuality


                        g.FillRectangle(Brushes.White, 0, 0, NewWidth, NewHeight)
                        g.DrawImage(sourceImage, 0, 0, NewWidth, NewHeight)
                        '--  end new method
                        newImage.SetResolution(72, 72)

                    Else 'use original width (no maxwidth given or image is narrow enough: 
                        newImage = sourceImage
                        newImage.SetResolution(72, 72)
                    End If

                    Dim fName1 As String = Path.GetFileName(FileNamePathOut)
                    Dim fName2 As String = Replace(fName1, " ", "_")
                    FileNamePathOut = Replace(FileNamePathOut, fName1, fName2)

                    'Use this method to try and get over medium trust issues
                    FileSystemUtils.SaveFile(FileNamePathOut, BmpToBytes_MemStream(newImage, _ImageQuality, 1))

                    ' Clean up
                    newImage.Dispose()
                    If Not sourceImage Is Nothing Then sourceImage.Dispose()
                End Using

                Return FileNamePathOut
            Catch exc As Exception
                Return ""
            End Try

        End Function

        ' Bitmap bytes have to be created via a direct memory copy of the bitmap
        Public Function BmpToBytes_MemStream(ByVal bmp As Bitmap, ByVal ImageQuality As Long, ByVal ImageEncodersInt As Integer) As Byte()
            Dim ms As New MemoryStream()

            ' Save to memory using the Jpeg format
            Dim info() As System.Drawing.Imaging.ImageCodecInfo

            info = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders
            Dim encoderParameters As System.Drawing.Imaging.EncoderParameters
            encoderParameters = New System.Drawing.Imaging.EncoderParameters(1)
            encoderParameters.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality)

            bmp.Save(ms, info(ImageEncodersInt), encoderParameters)

            ' read to end
            Dim bmpBytes As Byte() = ms.GetBuffer()
            bmp.Dispose()
            ms.Close()

            Return bmpBytes
        End Function

        'Bitmap bytes have to be created using Image.Save()
        Public Function BytesToImg(ByVal bmpBytes As Byte()) As Image
            Dim ms As New MemoryStream(bmpBytes)
            Dim img As Image = Image.FromStream(ms)
            ' Do NOT close the stream!

            Return img
        End Function

    End Class

End Namespace

