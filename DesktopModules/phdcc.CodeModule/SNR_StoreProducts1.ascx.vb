Imports System
Imports System.Web.UI.WebControls
Imports System.Collections
Imports System.Data
Imports System.IO
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports System.Linq
Imports System.Reflection
Imports Telerik.Web.UI

Partial Class SNR_StoreProducts1
    Inherits System.Web.UI.UserControl

    Private dbContextLogistic As New LogisticsDBLayerDataContext
    Private dbContextStore As New SNRDentonDBLayerDataContext
    Private pc As New DotNetNuke.Entities.Portals.PortalController
    Private imagesPath As String = "http://www.sprintexpress.co.uk/common/prod_images/jpgs/"
    Private sb As New StringBuilder

    Property pnCatID() As Integer
        Get
            Dim o As Object = ViewState("CatID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("CatID") = Value
        End Set
    End Property
    Property pnProductID() As Integer
        Get
            Dim o As Object = ViewState("ProductID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("ProductID") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            PopulateCategory()
            'rwEditProduct.VisibleOnPageLoad = False


        End If

    End Sub

    Protected Sub PopulateCategory()


        Dim ICat = (From Category In dbContextStore.NB_Store_Categories
                   Join CategoryLang In dbContextStore.NB_Store_CategoryLangs On Category.CategoryID Equals CategoryLang.CategoryID
                   Where Category.PortalID = DNN.GetPMB(Me).PortalId
                   Select Category.CategoryID, CategoryLang.CategoryName, Category.PortalID).Distinct


        tvCategory.DataSource = ICat.ToList
        tvCategory.DataBind()

    End Sub

    Protected Sub tvCategory_NodeClick(ByVal sender As Object, ByVal e As RadTreeNodeEventArgs) Handles tvCategory.NodeClick

        Dim node As RadTreeNode = e.Node

        Dim nCatId As Integer = node.Value

        pnCatID = nCatId

        BindProducts()

    End Sub
    Protected Sub rlvProducts_ItemDataBound(ByVal sender As Object, ByVal e As RadListViewItemEventArgs) Handles rlvProducts.ItemDataBound

        If e.Item.ItemType = RadListViewItemType.DataItem Or e.Item.ItemType = RadListViewItemType.AlternatingItem Then

            Dim item As RadListViewItem = e.Item

            Dim imgEdit As Image = item.FindControl("imgEdit")

            Dim imgProduct As System.Web.UI.WebControls.Image = item.FindControl("imgProduct")

            Dim hidImageUrl As HiddenField = item.FindControl("hidImageUrl")

            Dim ImageUrl As String = hidImageUrl.Value

            Dim ImageName As String = ImageUrl.Substring(ImageUrl.LastIndexOf("\"))

            'Dim imgThubnailView As System.Web.UI.WebControls.Image = item.FindControl("imgThubnailView")

            imgProduct.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages/" + ImageName

            'imgThubnailView.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages/" + ImageName

            imgEdit.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/Images/" + "Edit.gif"


        End If


    End Sub

    Protected Sub Insert(ByVal e As RadListViewCommandEventArgs)

        txtProductName.Text = String.Empty
        rntbPrice.Text = String.Empty
        rntbQuantity.Text = String.Empty
        lblProductName.Text = String.Empty
        hidProductID.Value = -1
        'imgProduct.
        imgProduct.AlternateText = ""
        imgProduct.ImageUrl = ""
        trImage.Visible = False
        rauImage.TargetFolder = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages"
        rwEditProduct.Visible = True
        rwEditProduct.VisibleOnPageLoad = True
        pnProductID = -1

    End Sub

    Protected Sub Edit(ByVal e As RadListViewCommandEventArgs)

        Dim item As RadListViewItem = e.ListViewItem

        Dim lnkbtnEdit As LinkButton = item.FindControl("lnkbtnEdit")

        Dim nProductID As Integer = lnkbtnEdit.CommandArgument

        pnProductID = nProductID

        Dim Product = (From p In dbContextStore.NB_Store_Products Join Model In dbContextStore.NB_Store_Models On p.ProductID Equals Model.ProductID
                       Join ProductLang In dbContextStore.NB_Store_ProductLangs On p.ProductID Equals ProductLang.ProductID
                       Join ProdLang In dbContextStore.NB_Store_ProductLangs On p.ProductID Equals ProdLang.ProductID
                       Join ProductImage In dbContextStore.NB_Store_ProductImages On p.ProductID Equals ProductImage.ProductID
                       Where p.ProductID = pnProductID And p.PortalID = DNN.GetPMB(Me).PortalId
                       Select p.ProductID, ProdLang.ProductName, Model.UnitCost, Model.QtyRemaining, ProductImage.ImageURL).Distinct.SingleOrDefault


        Dim ImageName As String = Product.ImageURL.Substring(Product.ImageURL.LastIndexOf("/"))

        txtProductName.Text = Product.ProductName
        rntbPrice.Text = Product.UnitCost
        rntbQuantity.Text = Product.QtyRemaining
        lblProductName.Text = Product.ProductName
        hidProductID.Value = Product.ProductID
        trImage.Visible = True
        imgProduct.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages/" + ImageName
        rauImage.TargetFolder = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages"
        rwEditProduct.Visible = True
        rwEditProduct.VisibleOnPageLoad = True

    End Sub

    Protected Sub rlvProducts_ItemCommand(ByVal sender As Object, ByVal e As RadListViewCommandEventArgs) Handles rlvProducts.ItemCommand

        If e.CommandName = "Insert" Then

            Insert(e)

        ElseIf e.CommandName = "Edit" Then

            Edit(e)

        End If

    End Sub

    Protected Sub BindProducts()

        Dim IItems = (From Product In dbContextStore.NB_Store_Products Join Model In dbContextStore.NB_Store_Models On Product.ProductID Equals Model.ProductID
                                Join ProductLang In dbContextStore.NB_Store_ProductLangs On Product.ProductID Equals ProductLang.ProductID
                                Join ProdLang In dbContextStore.NB_Store_ProductLangs On Product.ProductID Equals ProdLang.ProductID
                                Join ProductImage In dbContextStore.NB_Store_ProductImages On Product.ProductID Equals ProductImage.ProductID
                                Join Portals In dbContextStore.PortalLocalizations On Product.PortalID Equals Portals.PortalID
                                Join ProCat In dbContextStore.NB_Store_ProductCategories On ProCat.ProductID Equals Product.ProductID
                                Where ProCat.CategoryID = pnCatID
                                Select Product.ProductID, Product.ProductRef, ProdLang.ProductName, Model.UnitCost, Model.QtyRemaining, ProductImage.ImagePath, ProductLang.Description, ProCat.CategoryID).Distinct


        rlvProducts.DataSource = IItems

    End Sub

    Protected Sub rlvProducts_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.RadListViewNeedDataSourceEventArgs) Handles rlvProducts.NeedDataSource

        If pnCatID > 0 Then
            BindProducts()
        End If

    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click

        rwEditProduct.VisibleOnPageLoad = False
        rwEditProduct.Visible = False

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click

        If pnProductID > 0 Then

            Dim targetFolder As String = Server.MapPath(rauImage.TargetFolder)

            Dim Model = (From m In dbContextStore.NB_Store_Models
                         Where m.ProductID = pnProductID
                         ).SingleOrDefault


            Model.UnitCost = rntbPrice.Text.Trim
            Model.QtyRemaining = rntbQuantity.Text.Trim

            dbContextStore.SubmitChanges()

            Dim sql As String

            sb = New StringBuilder()

            sb.Append("update NB_Store_ProductLang set ProductName = '" + txtProductName.Text.Trim() + "'")
            'sb.Append(" , Description = '" + txtDescription.Text.Trim() + "'")
            sb.Append(" where ProductID = " + pnProductID.ToString)

            sql = sb.ToString

            dbContextStore.ExecuteCommand(sql)

            If rauImage.UploadedFiles.Count > 0 Then

                Dim Image = (From i In dbContextStore.NB_Store_ProductImages
                Where i.ProductID = pnProductID
                Select i).SingleOrDefault

                Image.ImagePath = targetFolder.ToString + "\" + rauImage.UploadedFiles(0).FileName
                Dim strArr() As String = Request.Path.Split("/")
                Image.ImageURL = "/" + strArr(1) + "/portals/" + DNN.GetPMB(Me).PortalId.ToString + "/productimages/" + rauImage.UploadedFiles(0).FileName

                dbContextStore.SubmitChanges()

            End If

        Else



            Dim product As New NB_Store_Product

            product.PortalID = DNN.GetPMB(Me).PortalId
            product.TaxCategoryID = -1
            product.Featured = False
            product.Archived = False
            product.CreatedByUser = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            product.CreatedDate = DateTime.Now
            product.IsDeleted = False
            product.ProductRef = txtProductName.Text.Trim
            product.IsHidden = False

            dbContextStore.NB_Store_Products.InsertOnSubmit(product)
            dbContextStore.SubmitChanges()

            Dim proCat As New NB_Store_ProductCategory

            proCat.ProductID = product.ProductID
            proCat.CategoryID = tvCategory.SelectedNode.Value

            dbContextStore.NB_Store_ProductCategories.InsertOnSubmit(proCat)
            dbContextStore.SubmitChanges()

            Dim productLang As New NB_Store_ProductLang

            productLang.ProductID = product.ProductID
            productLang.ProductName = txtProductName.Text.Trim
            productLang.Lang = "en-US"
            productLang.Summary = txtProductName.Text.Trim
            productLang.Description = txtProductName.Text.Trim
            productLang.Manufacturer = txtProductName.Text.Trim
            productLang.SEOName = txtProductName.Text.Trim
            productLang.TagWords = txtProductName.Text.Trim

            dbContextStore.NB_Store_ProductLangs.InsertOnSubmit(productLang)
            dbContextStore.SubmitChanges()

            Dim model As New NB_Store_Model

            model.ProductID = product.ProductID
            model.ListOrder = "1"
            model.UnitCost = rntbPrice.Text.Trim
            model.ModelRef = txtProductName.Text.Trim
            model.QtyRemaining = rntbQuantity.Text.Trim
            model.Deleted = False

            dbContextStore.NB_Store_Models.InsertOnSubmit(model)
            dbContextStore.SubmitChanges()

            If rauImage.UploadedFiles.Count > 0 Then

                Dim img As New NB_Store_ProductImage
                Dim targetFolder As String = Server.MapPath(rauImage.TargetFolder)
                img.ImagePath = targetFolder.ToString + "\" + rauImage.UploadedFiles(0).FileName
                Dim strArr() As String = Request.Path.Split("/")
                img.ImageURL = "/" + strArr(1) + "/portals/" + DNN.GetPMB(Me).PortalId.ToString + "/productimages/" + rauImage.UploadedFiles(0).FileName
                img.Hidden = False
                img.ListOrder = 1
                img.ProductID = product.ProductID

                dbContextStore.NB_Store_ProductImages.InsertOnSubmit(img)
                dbContextStore.SubmitChanges()

            End If



            BindProducts()




        End If



        rwEditProduct.Visible = False
        rwEditProduct.VisibleOnPageLoad = False

        BindProducts()



    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

        If rauImage.UploadedFiles.Count > 0 Then

            Dim targetFolder As String = Server.MapPath(rauImage.TargetFolder)

            Dim uploadFile As UploadedFile = rauImage.UploadedFiles(0)

            Dim fi As New System.IO.FileInfo(targetFolder + "\" + uploadFile.FileName)

            fi.Delete()

        End If

        rwEditProduct.VisibleOnPageLoad = False
        rwEditProduct.Visible = False

    End Sub

    Protected Sub rauImage_FileUploaded(ByVal sender As Object, ByVal e As FileUploadedEventArgs)

        'Dim stream As Stream = e.File.InputStream
        'Dim imageData As Byte() = New Byte(stream.Length - 1) {}
        'stream.Read(imageData, 0, CInt(stream.Length))
        'imgProduct.da()
        'Thumbnail.DataValue = imageData

    End Sub


End Class


'Protected Sub dlProducts_ItemCommand(ByVal sender As Object, ByVal e As DataListCommandEventArgs)

'    If e.CommandName = "Edit" Then

'        Dim dlProducts As DataList = sender


'        If Not String.IsNullOrEmpty(e.CommandArgument) Then

'            Dim nCatID As Integer = e.CommandArgument
'            dlProducts.EditItemIndex = e.Item.ItemIndex
'            BindProducts(nCatID)

'        End If

'    ElseIf e.CommandName = "Update" Then

'        Dim dlProducts As DataList = sender

'        Dim nProductID As Integer = Convert.ToInt64(e.CommandArgument)

'        Dim hidCategoryID As HiddenField = e.Item.FindControl("hidCategoryID")

'        Dim nCategoryID As Integer = hidCategoryID.Value

'        Dim Model = (From m In dbContextStore.NB_Store_Models
'                     Where m.ProductID = nProductID
'                     ).SingleOrDefault


'        Dim txtProductName As TextBox = e.Item.FindControl("txtProductName")
'        Dim rntbPrice As RadNumericTextBox = e.Item.FindControl("rntbPrice")
'        Dim txtDescription As RadTextBox = e.Item.FindControl("txtDescription")

'        Model.UnitCost = rntbPrice.Text.Trim

'        dbContextStore.SubmitChanges()

'        Dim sql As String

'        sb = New StringBuilder()

'        sb.Append("update NB_Store_ProductLang set ProductName = '" + txtProductName.Text.Trim() + "'")
'        sb.Append(" , Description = '" + txtDescription.Text.Trim() + "'")
'        sb.Append(" where ProductID = " + nProductID.ToString)

'        sql = sb.ToString

'        dbContextStore.ExecuteCommand(sql)

'        dlProducts.EditItemIndex = -1

'        BindProducts(nCategoryID)

'    End If

'End Sub

'Protected Sub dlProducts_OnItemDataBound(ByVal source As Object, ByVal e As DataListItemEventArgs)

'    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

'        Dim imgEdit As Image = e.Item.FindControl("imgEdit")

'        Dim imgProduct As System.Web.UI.WebControls.Image = e.Item.FindControl("imgProduct")

'        Dim hidImageUrl As HiddenField = e.Item.FindControl("hidImageUrl")

'        Dim ImageUrl As String = hidImageUrl.Value

'        Dim ImageName As String = ImageUrl.Substring(ImageUrl.LastIndexOf("\"))

'        Dim imgThubnailView As System.Web.UI.WebControls.Image = e.Item.FindControl("imgThubnailView")

'        imgProduct.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages/" + ImageName

'        imgThubnailView.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages/" + ImageName

'        imgEdit.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages/" + ImageName

'    ElseIf e.Item.ItemType = ListItemType.EditItem Then

'        Dim imgProduct As System.Web.UI.WebControls.Image = e.Item.FindControl("imgProduct")

'        Dim hidImageUrl As HiddenField = e.Item.FindControl("hidImageUrl")

'        Dim ImageUrl As String = hidImageUrl.Value

'        Dim ImageName As String = ImageUrl.Substring(ImageUrl.LastIndexOf("\"))

'        imgProduct.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages/" + ImageName

'    End If

'End Sub
'ElseIf e.CommandName = "Update" Then

'    Dim dlProducts As DataList = sender

'    Dim nProductID As Integer = Convert.ToInt64(e.CommandArgument)

'    Dim hidCategoryID As HiddenField = e.Item.FindControl("hidCategoryID")

'    Dim nCategoryID As Integer = hidCategoryID.Value

'    Dim Model = (From m In dbContextStore.NB_Store_Models
'                 Where m.ProductID = nProductID
'                 ).SingleOrDefault


'    Dim txtProductName As TextBox = e.Item.FindControl("txtProductName")
'    Dim rntbPrice As RadNumericTextBox = e.Item.FindControl("rntbPrice")
'    Dim txtDescription As RadTextBox = e.Item.FindControl("txtDescription")

'    Model.UnitCost = rntbPrice.Text.Trim

'    dbContextStore.SubmitChanges()

'    Dim sql As String

'    sb = New StringBuilder()

'    sb.Append("update NB_Store_ProductLang set ProductName = '" + txtProductName.Text.Trim() + "'")
'    sb.Append(" , Description = '" + txtDescription.Text.Trim() + "'")
'    sb.Append(" where ProductID = " + nProductID.ToString)

'    sql = sb.ToString

'    dbContextStore.ExecuteCommand(sql)

'    dlProducts.EditItemIndex = -1

'BindProducts(nCategoryID)

'Protected Sub Update(ByVal e As RadListViewCommandEventArgs)

'Dim rauImage As RadAsyncUpload = e.ListViewItem.FindControl("rauImage")

'Dim targetFolder As String = Server.MapPath(rauImage.TargetFolder)

'Dim nProductID As Integer = Convert.ToInt64(e.CommandArgument)

''Dim hidCategoryID As HiddenField = e.ListViewItem.FindControl("hidCategoryID")

'Dim nCategoryID As Integer = pnCatID

'Dim Model = (From m In dbContextStore.NB_Store_Models
'             Where m.ProductID = nProductID
'             ).SingleOrDefault


'Dim txtProductName As TextBox = e.ListViewItem.FindControl("txtProductName")
'Dim rntbPrice As RadNumericTextBox = e.ListViewItem.FindControl("rntbPrice")
''Dim txtDescription As RadTextBox = e.ListViewItem.FindControl("txtDescription")
'Dim rntbQuantity As RadNumericTextBox = e.ListViewItem.FindControl("rntbQuantity")


'Model.UnitCost = rntbPrice.Text.Trim
'Model.QtyRemaining = rntbQuantity.Text.Trim

'dbContextStore.SubmitChanges()

'Dim sql As String

'sb = New StringBuilder()

'sb.Append("update NB_Store_ProductLang set ProductName = '" + txtProductName.Text.Trim() + "'")
''sb.Append(" , Description = '" + txtDescription.Text.Trim() + "'")
'sb.Append(" where ProductID = " + nProductID.ToString)

'sql = sb.ToString

'dbContextStore.ExecuteCommand(sql)

'Dim Image = (From i In dbContextStore.NB_Store_ProductImages
'            Where i.ProductID = nProductID
'            Select i).SingleOrDefault

'Image.ImagePath = targetFolder.ToString + "\" + rauImage.UploadedFiles(0).FileName

'dbContextStore.SubmitChanges()

'End Sub

'ElseIf e.Item.ItemType = RadListViewItemType.EditItem Then

'Dim item As RadListViewItem = e.Item

'Dim imgProduct As System.Web.UI.WebControls.Image = item.FindControl("imgProduct")

'Dim hidImageUrl As HiddenField = item.FindControl("hidImageUrl")

'Dim ImageUrl As String = hidImageUrl.Value

'Dim ImageName As String = ImageUrl.Substring(ImageUrl.LastIndexOf("\"))

'imgProduct.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages/" + ImageName

'Dim rauImage As RadAsyncUpload = item.FindControl("rauImage")

'rauImage.TargetFolder = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/ProductImages"


