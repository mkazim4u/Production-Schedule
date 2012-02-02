Imports Telerik.Web.UI
Imports System.IO
Imports System.Collections.Generic

Partial Class PS_DisplayImageGallery
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Call PopulateCustomerDropDown()
            Call GetFilesInFolder()

        End If

    End Sub

    Protected Sub PopulateCustomerDropDown()

        Dim sql As String = "SELECT CustomerAccountCode +  ' (' + CustomerName + ')' 'Customer', CustomerKey FROM Customer WHERE CustomerStatusId = 'ACTIVE' AND ISNULL(AccountHandlerKey, 0) > 0 ORDER BY CustomerAccountCode"
        Dim dt As DataTable = SprintDB.Query(sql)
        rcbCustomer.DataTextField = "Customer"
        rcbCustomer.DataValueField = "CustomerKey"
        rcbCustomer.DataSource = dt
        rcbCustomer.DataSource = dt
        rcbCustomer.DataBind()
        rcbCustomer.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("- Select - ", -1))


    End Sub


    Protected Sub rcbCustomer_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)

        Dim virtualPathsCollection As New List(Of CustomItem)()

        Dim sb As StringBuilder = New StringBuilder()
        Dim imageVirtualPath As String = "http://www.sprintexpress.co.uk/common/prod_images/thumbs/"
        'Dim virtualPathsCollection As New List(Of String)()
        'Dim sqlParamCustomer(0) As SqlParameter
        'sqlParamCustomer(0).Value = 579

        sb.Append("SELECT ProductCode, ProductDescription, OriginalImage FROM LogisticProduct WHERE ArchiveFlag = 'N' and DeletedFlag = 'N' AND OriginalImage <> 'blank_image.jpg' AND ")
        sb.Append("CustomerKey = " & rcbCustomer.SelectedValue)
        sb.Append("order by ProductCode ")

        Dim dt As DataTable = SprintDB.Query(sb.ToString())
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            For Each dr As DataRow In dt.Rows
                Dim virtualPath As String = VirtualPathUtility.AppendTrailingSlash(imageVirtualPath) + dr("OriginalImage")
                Dim cItem As New CustomItem
                cItem.sImageUrl = virtualPath
                cItem.sfileName = GetFileName(virtualPath)
                virtualPathsCollection.Add(cItem)
                'virtualPathsCollection.Add(virtualPath)
            Next
            BindImages(virtualPathsCollection)
        End If

    End Sub

    Protected Sub BindImages(ByVal images As List(Of CustomItem))

        rlbImages.DataSource = images
        rlbImages.DataBind()

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        UpdateCarouselImages()

    End Sub

    Protected Sub GetFilesInFolder()

        Dim virtualPathsCollection As New List(Of CustomItem)()

        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        Dim imageFolderName As String = "carousel images"
        Dim imagesFileName As String = "Images Name"
        Dim objStreamReader As StreamReader
        Dim imageVirtualPath As String = "http://www.sprintexpress.co.uk/common/prod_images/thumbs/"

        Dim physicalPathToFolder As String = ps.HomeDirectoryMapPath & imageFolderName
        Dim physicalPathToFile As String = ps.HomeDirectoryMapPath & imageFolderName & "\" & imagesFileName

        If File.Exists(physicalPathToFile) Then
            objStreamReader = File.OpenText(physicalPathToFile)
            While objStreamReader.Peek() <> -1
                'Dim item As New RadListBoxItem
                'item.Text = objStreamReader.ReadLine
                Dim virtualPath As String = VirtualPathUtility.AppendTrailingSlash(imageVirtualPath) + objStreamReader.ReadLine
                Dim cItem As New CustomItem
                cItem.sImageUrl = virtualPath
                cItem.sfileName = GetFileName(virtualPath)
                virtualPathsCollection.Add(cItem)
                'rlbCarouselImages.Items.Add(item)
            End While
        End If

        BindCarouselImages(virtualPathsCollection)


    End Sub

    Protected Sub BindCarouselImages(ByVal images As List(Of CustomItem))

        rlbCarouselImages.DataSource = images
        rlbCarouselImages.DataBind()

    End Sub

    Public Function GetFileName(ByVal imgUrl As String) As String
        Dim fileName As String = String.Empty
        If imgUrl IsNot Nothing And imgUrl <> String.Empty Then
            fileName = imgUrl.Substring(imgUrl.LastIndexOf("/") + 1)
        End If
        Return fileName
    End Function

    Protected Sub UpdateCarouselImages()

        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings

        Dim imageFolderName As String = "carousel images"
        Dim imagesFileName As String = "Images Name"
        Dim objStreamWriter As StreamWriter
        Dim fs As FileStream

        Dim physicalPathToFolder As String = ps.HomeDirectoryMapPath & imageFolderName

        Dim physicalPathToFile As String = ps.HomeDirectoryMapPath & imageFolderName & "\" & imagesFileName

        If Directory.Exists(physicalPathToFolder) = False Then

            MkDir(physicalPathToFolder)

        End If

        If File.Exists(physicalPathToFile) Then

            File.Delete(physicalPathToFile)
            fs = File.Create(physicalPathToFile)
            fs.Close()

        End If

        objStreamWriter = File.AppendText(physicalPathToFile)

        For Each item As RadListBoxItem In rlbCarouselImages.Items
            Dim lblImgTitle As Label = item.FindControl("lblImgTitle")
            objStreamWriter.WriteLine(lblImgTitle.Text)
        Next

        If objStreamWriter IsNot Nothing Then

            objStreamWriter.Close()

        End If

    End Sub

    Protected Sub rlbImages_Transferred(ByVal sender As Object, ByVal e As RadListBoxTransferredEventArgs) Handles rlbImages.Transferred

        For Each item As RadListBoxItem In e.Items

            Dim imgProduct As System.Web.UI.WebControls.Image = item.FindControl("imgProduct")
            Dim lblImgTitle As Label = item.FindControl("lblImgTitle")
            imgProduct.ImageUrl = item.Text
            lblImgTitle.Text = GetFileName(item.Text)

        Next

    End Sub

    Protected Sub rlbCarouselImages_Transferred(ByVal sender As Object, ByVal e As RadListBoxTransferredEventArgs)

        For Each item As RadListBoxItem In e.Items

            Dim imgProduct As System.Web.UI.WebControls.Image = item.FindControl("imgProduct")
            Dim lblImgTitle As Label = item.FindControl("lblImgTitle")
            imgProduct.ImageUrl = item.Text
            lblImgTitle.Text = item.Text
            item.DataBind()
            e.DestinationListBox.Items.Add(item)

        Next

    End Sub

    Public Class CustomItem

        Private imageUrl As String
        Private fileName As String

        Public Property sImageUrl() As String
            Get
                Return imageUrl
            End Get
            Set(ByVal value As String)
                imageUrl = value
            End Set
        End Property

        Public Property sfileName() As String
            Get
                Return fileName
            End Get
            Set(ByVal value As String)
                fileName = value
            End Set
        End Property

    End Class

End Class
