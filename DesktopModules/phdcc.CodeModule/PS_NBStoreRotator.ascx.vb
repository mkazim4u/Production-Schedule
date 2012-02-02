Imports Telerik.Web.UI
Imports System.IO
Imports System.Collections.Generic

Partial Class PS_NBStoreRotator
    Inherits System.Web.UI.UserControl

    'Private virtualPath As String = "~/Portals/2/productImages"
    'Private virtualPath As String = "productImages"


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' By default, use the RotatorType.CoverFlow mode
            ConfigureRadRotator()
        End If

    End Sub

    Private Sub ConfigureRadRotator()

        ' Change rotator's type
        RadRotator1.DataSource = GetFilesInFolder()
        ' Set datasource
        RadRotator1.DataBind()
    End Sub

    Protected Function GetFilesInFolder() As List(Of String)

        Dim virtualPathsCollection As New List(Of String)()
        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        Dim objStreamReader As StreamReader
        Dim imageFolderName As String = "carousel images"
        Dim imagesFileName As String = "Images Name"
        'Dim imageVirtualPath As String = "~/portals/" + DNN.GetPMB(Me).PortalId.ToString() + "/" + imageFolderName + "/" + imagesFileName
        Dim imageVirtualPath As String = "http://www.sprintexpress.co.uk/common/prod_images/jpgs/"

        Dim physicalPathToFile As String = ps.HomeDirectoryMapPath & imageFolderName & "\" & imagesFileName

        If File.Exists(physicalPathToFile) Then

            objStreamReader = File.OpenText(physicalPathToFile)

            While objStreamReader.Peek <> -1

                Dim virtualPath As String = VirtualPathUtility.AppendTrailingSlash(imageVirtualPath) + objStreamReader.ReadLine
                virtualPathsCollection.Add(virtualPath)
            End While


        End If


        Return virtualPathsCollection

    End Function

    '''''''''''''''''''''''' Accessing Rotator Folder ''''''''''''''''''''''''

    'Protected Function GetFilesInFolder() As List(Of String)

    '    Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings

    '    Dim imageFolderName As String = "rotatorimages"

    '    Dim imageVirtualPath As String = "~/portals/" + DNN.GetPMB(Me).PortalId.ToString() + "/" + imageFolderName

    '    Dim physicalPathToFolder As String = ps.HomeDirectoryMapPath & imageFolderName

    '    Dim physicalPathsCollection As String() = System.IO.Directory.GetFiles(physicalPathToFolder)

    '    Dim virtualPathsCollection As New List(Of String)()

    '    For Each path As [String] In physicalPathsCollection

    '        Dim virtualPath As String = VirtualPathUtility.AppendTrailingSlash(imageVirtualPath) + System.IO.Path.GetFileName(path)
    '        virtualPathsCollection.Add(virtualPath)
    '    Next

    '    Return virtualPathsCollection



    'End Function

    

End Class


