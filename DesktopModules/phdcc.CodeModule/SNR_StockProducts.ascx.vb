Imports System
Imports System.Web.UI.WebControls
Imports System.Collections
Imports System.IO
Imports System.Net
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports System.Linq
Imports System.Reflection
Imports Telerik.Web.UI

Partial Class SNR_StockProducts
    Inherits System.Web.UI.UserControl

    Private dbContextLogistic As New LogisticsDBLayerDataContext
    Private dbContextStore As New SNRDentonDBLayerDataContext
    Private pc As New DotNetNuke.Entities.Portals.PortalController
    Private imagesPath As String = "http://www.sprintexpress.co.uk/common/prod_images/jpgs/"
    Private productImages As String = "productimages"
    Private sb As New StringBuilder
    Public Property pnProductType() As Int32
        Get
            Dim o As Object = ViewState("ProductType")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("ProductType") = Value
        End Set
    End Property

    Public Property pnStoreProductID() As Int32
        Get
            Dim o As Object = ViewState("StoreProductID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("StoreProductID") = Value
        End Set
    End Property

    Public Property pnPortalID() As Int32
        Get
            Dim o As Object = ViewState("PortalID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("PortalID") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            'PopulateCustomerDropDown()

            PopulateProdcutType()


        End If

    End Sub

    Protected Sub PopulateCategory(ByVal nPortalID As Integer)

        Dim ICat = (From Category In dbContextStore.NB_Store_Categories
                  Join CategoryLang In dbContextStore.NB_Store_CategoryLangs On Category.CategoryID Equals CategoryLang.CategoryID
                  Where Category.PortalID = nPortalID
                  Select Category.CategoryID, CategoryLang.CategoryName, Category.PortalID).Distinct

        rcbCategory.DataSource = ICat
        rcbCategory.DataBind()

        rcbCategory.Items.Insert(0, New RadComboBoxItem("- Select Category -", "-1"))


    End Sub

    Protected Sub PopulateCustomerDropDown()

        'rcbCustomer.Items.Clear()

        'Dim IEnumerableList As IEnumerable(Of Customer)

        ''Dim sql As String = "SELECT CustomerAccountCode +  ' (' + CustomerName + ')' 'Customer', CustomerKey FROM Customer WHERE CustomerStatusId = 'ACTIVE' AND ISNULL(AccountHandlerKey, 0) > 0 ORDER BY CustomerAccountCode"

        'Dim sql As String = "SELECT CustomerAccountCode , CustomerName , CustomerKey FROM Customer WHERE CustomerStatusId = 'ACTIVE' AND ISNULL(AccountHandlerKey, 0) > 0 ORDER BY CustomerAccountCode"

        'IEnumerableList = dbContextLogistic.ExecuteQuery(Of Customer)(sql)

        'Dim cust As IList(Of Customer) = IEnumerableList.ToList

        'For Each c As Customer In cust

        '    Dim item As New RadComboBoxItem(c.CustomerAccountCode + "(" + c.CustomerName + ")", c.CustomerKey)
        '    rcbCustomer.Items.Add(item)

        'Next

        'rcbCustomer.Items.Insert(0, New RadComboBoxItem("- Select Customer -", "-1"))

    End Sub
    Private Sub PopulateProdcutType()

        rcbProductsType.Items.Add(New RadComboBoxItem("- All -", "-1"))
        rcbProductsType.Items.Add(New RadComboBoxItem("UK Products", FF_GLOBALS.PRODUCTS_TYPE.UK_PRODUCTS))
        rcbProductsType.Items.Add(New RadComboBoxItem("USA Products", FF_GLOBALS.PRODUCTS_TYPE.USA_PRODUCTS))
        rcbProductsType.Items.Add(New RadComboBoxItem("ME Products", FF_GLOBALS.PRODUCTS_TYPE.ME_PRODUCTS))

        rcbProductsType.SelectedValue = "-1"
        pnProductType = -1

    End Sub

    

    Protected Sub rgStockProducts_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgStockProducts.NeedDataSource

        Dim nCustomerKey As Integer

        Dim config = (From con In dbContextStore.SNR_Configurations
                     Where con.ConfigKey = FF_GLOBALS.CUSTOMER_ID_KEY).SingleOrDefault

        nCustomerKey = config.ConfigValue

        Dim sProducttype As String = String.Empty

        If pnProductType = FF_GLOBALS.PRODUCTS_TYPE.UK_PRODUCTS Then

            sProducttype = FF_GLOBALS.UK_PRODUCT_IDENTIFIER


        ElseIf pnProductType = FF_GLOBALS.PRODUCTS_TYPE.USA_PRODUCTS Then

            sProducttype = FF_GLOBALS.USA_PRODUCT_IDENTIFIER

        ElseIf pnProductType = FF_GLOBALS.PRODUCTS_TYPE.ME_PRODUCTS Then

            sProducttype = FF_GLOBALS.ME_PRODUCT_IDENTIFIER

        ElseIf pnProductType = -1 Then

            sProducttype = String.Empty

        End If

        Dim sb As StringBuilder = New StringBuilder()

        sb.Append("SELECT DISTINCT(lp.LogisticProductKey) 'SprintProductKey', lp.ProductCode 'ProductCode', lp.ProductDescription 'ProductDescription',")
        sb.Append("lp.ProductCategory 'Category',")
        sb.Append("lp.SubCategory 'SubCategory',")
        sb.Append("lp.UnitValue 'Price',")
        sb.Append("lp.UnitWeightGrams 'Weight',")
        sb.Append("QuantityAvailable = CASE ISNUMERIC((select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey))")
        sb.Append("WHEN 0 THEN 0 ELSE (select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey)")
        sb.Append("END,")
        sb.Append("lp.OriginalImage 'Image',")
        sb.Append("lp.ThumbNailImage 'ThumbNailImage'")
        sb.Append("FROM LogisticProduct AS lp ")
        sb.Append("LEFT OUTER JOIN LogisticProductLocation AS lpl ")
        sb.Append("ON lp.LogisticProductKey = lpl.LogisticProductKey ")
        sb.Append("INNER JOIN UserProductProfile As upp ")
        sb.Append("ON lp.LogisticProductKey = upp.ProductKey ")
        sb.Append("WHERE lp.CustomerKey = " & nCustomerKey)
        If pnProductType <> -1 Then
            sb.Append("AND lp.productcode like '%" & sProducttype & "%'")
        End If
        sb.Append("AND lp.DeletedFlag = 'N' ")
        sb.Append("AND lp.ArchiveFlag = 'N'")

        Dim dt As DataTable = SprintDB.Query(sb.ToString())

        rgStockProducts.DataSource = dt



    End Sub
    Protected Sub rcbProductsType_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbProductsType.SelectedIndexChanged

        Dim nCustomerKey As Integer
        pnProductType = Convert.ToInt64(rcbProductsType.SelectedValue)

        Dim sProducttype As String = String.Empty

        If pnProductType = FF_GLOBALS.PRODUCTS_TYPE.UK_PRODUCTS Then

            sProducttype = FF_GLOBALS.UK_PRODUCT_IDENTIFIER

            PopulateCategory(FF_GLOBALS.UK_PORTAL_ID)

            pnPortalID = FF_GLOBALS.UK_PORTAL_ID

        ElseIf pnProductType = FF_GLOBALS.PRODUCTS_TYPE.USA_PRODUCTS Then

            sProducttype = FF_GLOBALS.USA_PRODUCT_IDENTIFIER

            PopulateCategory(FF_GLOBALS.US_PORTAL_ID)

            pnPortalID = FF_GLOBALS.US_PORTAL_ID

        ElseIf pnProductType = FF_GLOBALS.PRODUCTS_TYPE.ME_PRODUCTS Then

            sProducttype = FF_GLOBALS.ME_PRODUCT_IDENTIFIER

            PopulateCategory(FF_GLOBALS.ME_PORTAL_ID)

            pnPortalID = FF_GLOBALS.ME_PORTAL_ID

        ElseIf pnProductType = -1 Then

            sProducttype = String.Empty

        End If

        Dim config = (From con In dbContextStore.SNR_Configurations
                     Where con.ConfigKey = FF_GLOBALS.CUSTOMER_ID_KEY).SingleOrDefault

        nCustomerKey = config.ConfigValue

        Dim sb As StringBuilder = New StringBuilder()

        sb.Append("SELECT DISTINCT(lp.LogisticProductKey) 'SprintProductKey', lp.ProductCode 'ProductCode', lp.ProductDescription 'ProductDescription',")
        sb.Append("lp.ProductCategory 'Category',")
        sb.Append("lp.SubCategory 'SubCategory',")
        sb.Append("lp.UnitValue 'Price',")
        sb.Append("lp.UnitWeightGrams 'Weight',")
        sb.Append("QuantityAvailable = CASE ISNUMERIC((select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey))")
        sb.Append("WHEN 0 THEN 0 ELSE (select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey)")
        sb.Append("END,")
        sb.Append("lp.OriginalImage 'Image',")
        sb.Append("lp.ThumbNailImage 'ThumbNailImage'")
        sb.Append("FROM LogisticProduct AS lp ")
        sb.Append("LEFT OUTER JOIN LogisticProductLocation AS lpl ")
        sb.Append("ON lp.LogisticProductKey = lpl.LogisticProductKey ")
        sb.Append("INNER JOIN UserProductProfile As upp ")
        sb.Append("ON lp.LogisticProductKey = upp.ProductKey ")
        sb.Append("WHERE lp.CustomerKey = " & nCustomerKey & " ")
        If pnProductType <> -1 Then
            sb.Append("AND lp.productcode like '%" & sProducttype & "%'")
        End If
        sb.Append("AND lp.DeletedFlag = 'N' ")
        sb.Append("AND lp.ArchiveFlag = 'N'")

        Dim dt As DataTable = SprintDB.Query(sb.ToString())

        rgStockProducts.DataSource = dt
        rgStockProducts.DataBind()

    End Sub

    Private Function RemoveSubStringFromPrdouctCode(ByVal productCode As String) As String

        Dim sProductCode As String = productCode.Remove(productCode.IndexOf("("), 4)

        Return sProductCode.Trim


    End Function
    Protected Sub btnUpdateUSProducts_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateUSProducts.Click

        Dim nCustomerKey As Integer

        Dim config = (From con In dbContextStore.SNR_Configurations
                     Where con.ConfigKey = FF_GLOBALS.CUSTOMER_ID_KEY).SingleOrDefault

        nCustomerKey = config.ConfigValue

        Dim sb As StringBuilder = New StringBuilder()

        sb.Append("SELECT DISTINCT(lp.LogisticProductKey) 'SprintProductKey', lp.ProductCode 'ProductCode', lp.ProductDescription 'ProductDescription',")
        sb.Append("lp.ProductCategory 'Category',")
        sb.Append("lp.SubCategory 'SubCategory',")
        sb.Append("lp.UnitValue 'Price',")
        sb.Append("lp.UnitWeightGrams 'Weight',")
        sb.Append("QuantityAvailable = CASE ISNUMERIC((select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey))")
        sb.Append("WHEN 0 THEN 0 ELSE (select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey)")
        sb.Append("END,")
        sb.Append("lp.OriginalImage 'Image',")
        sb.Append("lp.ThumbNailImage 'ThumbNailImage'")
        sb.Append("FROM LogisticProduct AS lp ")
        sb.Append("LEFT OUTER JOIN LogisticProductLocation AS lpl ")
        sb.Append("ON lp.LogisticProductKey = lpl.LogisticProductKey ")
        sb.Append("INNER JOIN UserProductProfile As upp ")
        sb.Append("ON lp.LogisticProductKey = upp.ProductKey ")
        sb.Append("WHERE lp.CustomerKey = " & nCustomerKey & " ")
        If pnProductType <> -1 Then
            sb.Append("AND lp.productcode like '%" & FF_GLOBALS.PRODUCTS_TYPE.USA_PRODUCTS & "%'")
        End If
        sb.Append("AND lp.DeletedFlag = 'N' ")
        sb.Append("AND lp.ArchiveFlag = 'N'")

        Dim dt As DataTable = SprintDB.Query(sb.ToString())

        For Each dr As DataRow In dt.Rows

            Dim nProductRefKey As Integer = Convert.ToInt64(dr("SprintProductKey"))

            Dim product = (From p In dbContextStore.NB_Store_Products
                          Where p.PortalID = FF_GLOBALS.US_PORTAL_ID And p.ProductRef = nProductRefKey.ToString
                          Select p).SingleOrDefault


            If product IsNot Nothing Then

                Dim model = (From m In dbContextStore.NB_Store_Models
                Where m.ProductID = product.ProductID
                Select m).SingleOrDefault

                model.UnitCost = Convert.ToDecimal(dr("Price"))
                model.DealerCost = Convert.ToDecimal(dr("Price"))
                model.QtyRemaining = Convert.ToInt64(dr("QuantityAvailable"))

                dbContextStore.SubmitChanges()


            End If

        Next

        WebMsgBox.Show("All US Products are Updated.")


    End Sub

    Protected Sub btnUpdateUKProducts_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateUKProducts.Click

        Dim nCustomerKey As Integer

        Dim config = (From con In dbContextStore.SNR_Configurations
                     Where con.ConfigKey = FF_GLOBALS.CUSTOMER_ID_KEY).SingleOrDefault

        nCustomerKey = config.ConfigValue

        Dim sb As StringBuilder = New StringBuilder()

        sb.Append("SELECT DISTINCT(lp.LogisticProductKey) 'SprintProductKey', lp.ProductCode 'ProductCode', lp.ProductDescription 'ProductDescription',")
        sb.Append("lp.ProductCategory 'Category',")
        sb.Append("lp.SubCategory 'SubCategory',")
        sb.Append("lp.UnitValue 'Price',")
        sb.Append("lp.UnitWeightGrams 'Weight',")
        sb.Append("QuantityAvailable = CASE ISNUMERIC((select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey))")
        sb.Append("WHEN 0 THEN 0 ELSE (select sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey)")
        sb.Append("END,")
        sb.Append("lp.OriginalImage 'Image',")
        sb.Append("lp.ThumbNailImage 'ThumbNailImage'")
        sb.Append("FROM LogisticProduct AS lp ")
        sb.Append("LEFT OUTER JOIN LogisticProductLocation AS lpl ")
        sb.Append("ON lp.LogisticProductKey = lpl.LogisticProductKey ")
        sb.Append("INNER JOIN UserProductProfile As upp ")
        sb.Append("ON lp.LogisticProductKey = upp.ProductKey ")
        sb.Append("WHERE lp.CustomerKey = " & nCustomerKey & " ")
        If pnProductType <> -1 Then
            sb.Append("AND lp.productcode like '%" & FF_GLOBALS.PRODUCTS_TYPE.UK_PRODUCTS & "%'")
        End If
        sb.Append("AND lp.DeletedFlag = 'N' ")
        sb.Append("AND lp.ArchiveFlag = 'N'")

        Dim dt As DataTable = SprintDB.Query(sb.ToString())

        For Each dr As DataRow In dt.Rows

            Dim nProductRefKey As Integer = Convert.ToInt64(dr("SprintProductKey"))

            Dim product = (From p In dbContextStore.NB_Store_Products
                          Where p.PortalID = FF_GLOBALS.UK_PORTAL_ID And p.ProductRef = nProductRefKey.ToString
                          Select p).SingleOrDefault


            If product IsNot Nothing Then

                Dim model = (From m In dbContextStore.NB_Store_Models
                Where m.ProductID = product.ProductID
                Select m).SingleOrDefault

                model.UnitCost = Convert.ToDecimal(dr("Price"))
                model.DealerCost = Convert.ToDecimal(dr("Price"))
                model.QtyRemaining = Convert.ToInt64(dr("QuantityAvailable"))

                dbContextStore.SubmitChanges()
       

            End If

        Next

        WebMsgBox.Show("All UK Products are Updated.")

    End Sub

    Protected Sub chkExportProduct_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkExportProduct As CheckBox = sender

        If chkExportProduct.Checked Then

            Dim hidSprintProductKey As HiddenField = chkExportProduct.NamingContainer.FindControl("hidSprintProductKey")
            Dim lblProductCode As Label = chkExportProduct.NamingContainer.FindControl("lblProductCode")
            Dim lblPrice As Label = chkExportProduct.NamingContainer.FindControl("lblPrice")
            Dim lblQuantity As Label = chkExportProduct.NamingContainer.FindControl("lblQuantity")
            Dim hidImage As HiddenField = chkExportProduct.NamingContainer.FindControl("hidImage")

            Dim nSprintProductKey As Integer = hidSprintProductKey.Value
            Dim sProductCode As String = lblProductCode.Text
            Dim dPrice As Decimal = Convert.ToDecimal(lblPrice.Text)
            Dim nQuantity As Integer = Convert.ToInt64(lblQuantity.Text)
            Dim sImageName As String = hidImage.Value
            Dim nModelId As Integer
            Dim nImageId As Integer
            Dim nCatId As Integer





            ''''''''''''''' Is Product Exist ??? '''''''''''''''''''

            IsProductExist(nSprintProductKey)

            If pnStoreProductID > 0 Then

                Dim Model = (From m In dbContextStore.NB_Store_Models
                            Where m.ProductID = pnStoreProductID
                            Select m).SingleOrDefault

                If Model IsNot Nothing Then
                    nModelId = Model.ModelID
                Else
                    nModelId = -1
                End If



                Dim Image = (From ProductImage In dbContextStore.NB_Store_ProductImages
                            Where ProductImage.ProductID = pnStoreProductID
                            Select ProductImage).SingleOrDefault

                If Image IsNot Nothing Then
                    nImageId = Image.ImageID
                Else
                    nImageId = -1
                End If



                Dim Category = (From Cat In dbContextStore.NB_Store_ProductCategories
                               Where Cat.ProductID = pnStoreProductID).SingleOrDefault

                If Category IsNot Nothing Then
                    nCatId = Category.CategoryID
                Else
                    nCatId = -1
                End If




            End If


            ''''''''''' Insert / Update New Product ''''''''''''''

            Dim spcProductUpdate(17) As SqlParameter

            spcProductUpdate(0) = New SqlClient.SqlParameter("@ProductId", SqlDbType.BigInt)
            spcProductUpdate(0).Value = pnStoreProductID

            spcProductUpdate(1) = New SqlClient.SqlParameter("@PortalId", SqlDbType.BigInt)
            spcProductUpdate(1).Value = pnPortalID

            spcProductUpdate(2) = New SqlClient.SqlParameter("@TaxCategoryID", SqlDbType.BigInt)
            spcProductUpdate(2).Value = 0

            spcProductUpdate(3) = New SqlClient.SqlParameter("@Featured", SqlDbType.Bit)
            spcProductUpdate(3).Value = 0

            spcProductUpdate(4) = New SqlClient.SqlParameter("@Archived", SqlDbType.Bit)
            spcProductUpdate(4).Value = 0

            spcProductUpdate(5) = New SqlClient.SqlParameter("@CreatedByUser", SqlDbType.NVarChar)
            spcProductUpdate(5).Value = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username

            spcProductUpdate(6) = New SqlClient.SqlParameter("@CreatedDate", SqlDbType.DateTime)
            spcProductUpdate(6).Value = DateTime.Now

            spcProductUpdate(7) = New SqlClient.SqlParameter("@IsDeleted", SqlDbType.Bit)
            spcProductUpdate(7).Value = 0

            spcProductUpdate(8) = New SqlClient.SqlParameter("@ProductRef", SqlDbType.NVarChar)
            spcProductUpdate(8).Value = nSprintProductKey

            spcProductUpdate(9) = New SqlClient.SqlParameter("@Lang", SqlDbType.Char)
            spcProductUpdate(9).Value = "en-US"

            spcProductUpdate(10) = New SqlClient.SqlParameter("@Summary", SqlDbType.NVarChar)
            spcProductUpdate(10).Value = sProductCode

            spcProductUpdate(11) = New SqlClient.SqlParameter("@Description", SqlDbType.NVarChar)
            spcProductUpdate(11).Value = sProductCode

            spcProductUpdate(12) = New SqlClient.SqlParameter("@Manufacturer", SqlDbType.NVarChar)
            spcProductUpdate(12).Value = ""

            spcProductUpdate(13) = New SqlClient.SqlParameter("@ProductName", SqlDbType.NVarChar)
            spcProductUpdate(13).Value = RemoveSubStringFromPrdouctCode(sProductCode)

            spcProductUpdate(14) = New SqlClient.SqlParameter("@XMLdata", SqlDbType.Xml)
            spcProductUpdate(14).Value = RemoveSubStringFromPrdouctCode(sProductCode)

            spcProductUpdate(15) = New SqlClient.SqlParameter("@SEOName", SqlDbType.NVarChar)
            spcProductUpdate(15).Value = RemoveSubStringFromPrdouctCode(sProductCode)

            spcProductUpdate(16) = New SqlClient.SqlParameter("@TagWords", SqlDbType.NVarChar)
            spcProductUpdate(16).Value = RemoveSubStringFromPrdouctCode(sProductCode)

            spcProductUpdate(17) = New SqlClient.SqlParameter("@IsHidden", SqlDbType.Bit)
            spcProductUpdate(17).Value = 0

            Dim retProductId As Int64 = DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_Products_Update", spcProductUpdate).Rows(0)(0)



            ''''''''''''''''''''''''''''''''''''''''''''''''' Product Category '''''''''''''''''''''''''''''''''''''''''''''''''''''

            If pnStoreProductID < 0 Then

                Dim spcCategoryProductUpdate(1) As SqlParameter

                spcCategoryProductUpdate(0) = New SqlClient.SqlParameter("@ProductId", SqlDbType.BigInt)
                spcCategoryProductUpdate(0).Value = retProductId

                spcCategoryProductUpdate(1) = New SqlClient.SqlParameter("@CategoryID", SqlDbType.BigInt)
                spcCategoryProductUpdate(1).Value = Convert.ToInt64(rcbCategory.SelectedValue)

                DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_ProductCategory_Update", spcCategoryProductUpdate)


            End If




            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            '''''''''''''''''''''''''''''''' Product Price and Quantity '''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim spcProductModelUpdate(18) As SqlParameter


            spcProductModelUpdate(0) = New SqlClient.SqlParameter("@ModelID", SqlDbType.BigInt)

            If pnStoreProductID > 0 Then
                spcProductModelUpdate(0).Value = nModelId
            Else
                spcProductModelUpdate(0).Value = -1
            End If


            spcProductModelUpdate(1) = New SqlClient.SqlParameter("@ProductID", SqlDbType.BigInt)
            spcProductModelUpdate(1).Value = retProductId

            spcProductModelUpdate(2) = New SqlClient.SqlParameter("@ListOrder", SqlDbType.BigInt)
            spcProductModelUpdate(2).Value = 1

            spcProductModelUpdate(3) = New SqlClient.SqlParameter("@UnitCost", SqlDbType.Money)

            spcProductModelUpdate(3).Value = dPrice

            spcProductModelUpdate(4) = New SqlClient.SqlParameter("@Barcode", SqlDbType.NVarChar)
            spcProductModelUpdate(4).Value = ""

            spcProductModelUpdate(5) = New SqlClient.SqlParameter("@ModelRef", SqlDbType.NVarChar)
            spcProductModelUpdate(5).Value = nSprintProductKey

            spcProductModelUpdate(6) = New SqlClient.SqlParameter("@Lang", SqlDbType.NChar)
            spcProductModelUpdate(6).Value = "en-US"

            spcProductModelUpdate(7) = New SqlClient.SqlParameter("@ModelName", SqlDbType.NVarChar)
            spcProductModelUpdate(7).Value = sProductCode

            spcProductModelUpdate(8) = New SqlClient.SqlParameter("@QtyRemaining", SqlDbType.BigInt)
            spcProductModelUpdate(8).Value = nQuantity

            spcProductModelUpdate(9) = New SqlClient.SqlParameter("@QtyTrans", SqlDbType.BigInt)
            spcProductModelUpdate(9).Value = 0

            spcProductModelUpdate(10) = New SqlClient.SqlParameter("@QtyTransDate", SqlDbType.DateTime)
            spcProductModelUpdate(10).Value = DateTime.Now

            spcProductModelUpdate(11) = New SqlClient.SqlParameter("@Deleted", SqlDbType.Bit)
            spcProductModelUpdate(11).Value = False

            spcProductModelUpdate(12) = New SqlClient.SqlParameter("@QtyStockSet", SqlDbType.BigInt)
            spcProductModelUpdate(12).Value = 0

            spcProductModelUpdate(13) = New SqlClient.SqlParameter("@DealerCost", SqlDbType.Money)
            spcProductModelUpdate(13).Value = dPrice

            spcProductModelUpdate(14) = New SqlClient.SqlParameter("@PurchaseCost", SqlDbType.Money)
            spcProductModelUpdate(14).Value = 0

            spcProductModelUpdate(15) = New SqlClient.SqlParameter("@XMLData", SqlDbType.Xml)
            spcProductModelUpdate(15).Value = ""

            spcProductModelUpdate(16) = New SqlClient.SqlParameter("@Extra", SqlDbType.NVarChar)
            spcProductModelUpdate(16).Value = ""

            spcProductModelUpdate(17) = New SqlClient.SqlParameter("@DealerOnly", SqlDbType.Bit)
            spcProductModelUpdate(17).Value = False

            spcProductModelUpdate(18) = New SqlClient.SqlParameter("@Allow", SqlDbType.BigInt)
            spcProductModelUpdate(18).Value = 0

            DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_Model_Update", spcProductModelUpdate)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



            ''''''''''''''''''''''''''''''''''''' Images '''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
            Dim DestinationPath As String = ps.HomeDirectoryMapPath & productImages
            Dim imageName As String = sImageName

            Dim spcProductImageUpdate(7) As SqlParameter

            spcProductImageUpdate(0) = New SqlClient.SqlParameter("@ImageID", SqlDbType.BigInt)

            If pnStoreProductID > 0 Then
                spcProductImageUpdate(0).Value = nImageId
            Else
                spcProductImageUpdate(0).Value = -1
                spcProductImageUpdate(1) = New SqlClient.SqlParameter("@ProductID", SqlDbType.BigInt)
                spcProductImageUpdate(1).Value = retProductId

                spcProductImageUpdate(2) = New SqlClient.SqlParameter("@ImagePath", SqlDbType.NVarChar)
                spcProductImageUpdate(2).Value = DestinationPath & "\" & imageName

                spcProductImageUpdate(3) = New SqlClient.SqlParameter("@ListOrder", SqlDbType.Int)
                spcProductImageUpdate(3).Value = 1

                spcProductImageUpdate(4) = New SqlClient.SqlParameter("@Hidden", SqlDbType.Bit)
                spcProductImageUpdate(4).Value = False

                spcProductImageUpdate(5) = New SqlClient.SqlParameter("@Lang", SqlDbType.NChar)
                spcProductImageUpdate(5).Value = "en-US"

                spcProductImageUpdate(6) = New SqlClient.SqlParameter("@ImageDesc", SqlDbType.NVarChar)
                spcProductImageUpdate(6).Value = ""

                spcProductImageUpdate(7) = New SqlClient.SqlParameter("@ImageURL", SqlDbType.NVarChar)
                spcProductImageUpdate(7).Value = VirtualPathUtility.AppendTrailingSlash(ps.HomeDirectory) & productImages & "/" & imageName

                DNNDB.ExecuteStoredProcedure("NEvoweb_NB_Store_ProductImage_Update", spcProductImageUpdate)

                '''''''''''''''''''''''''''''''''''''''''' Copy Image To Folder '''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Dim Url As String
                Url = imagesPath & sImageName
                Call CopyImageFromUrl(Url)

            End If


           
            'Dim Product = (From p In dbContextStore.NB_Store_Products
            '               Join pc In dbContextStore.NB_Store_ProductCategories
            '               On p.ProductID Equals pc.ProductID
            '               Join pl In dbContextStore.NB_Store_ProductLangs
            '               On p.ProductID Equals pl.ProductID
            '               Where p.ProductRef = nSprintProductKey.ToString And p.ProductID = retProductId
            '               Select p).Distinct

            Dim Product = (From pl In dbContextStore.NB_Store_ProductLangs
                          Where pl.ProductID = retProductId
                          Select productName = pl.ProductName).SingleOrDefault


            If Product IsNot Nothing Then
                Dim pi As PortalInfo = pc.GetPortal(pnPortalID)
                WebMsgBox.Show(Product + " Updated To " + pi.PortalName + " Store")
            Else
                WebMsgBox.Show(retProductId.ToString + " Not Updated To Store. Contact The System Administrator for further assistance.")
            End If


            chkExportProduct.Checked = False



        End If




    End Sub

    Protected Sub CopyImageFromUrl(ByVal url As String)

        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        Dim DestinationPath As String = ps.HomeDirectoryMapPath & "productimages"

        Dim imageUrl As String = url
        Dim filename As String = imageUrl.Substring(imageUrl.LastIndexOf("/"c) + 1)
        Dim bytes As Byte() = GetBytesFromUrl(imageUrl)
        Call CreateFolder(productImages)
        WriteBytesToFile(DestinationPath + "/" + filename, bytes)

    End Sub

    Public Shared Sub WriteBytesToFile(fileName As String, content As Byte())

        Dim fs As New FileStream(fileName, FileMode.Create)
        Dim w As New BinaryWriter(fs)
        Try
            w.Write(content)
        Finally
            fs.Close()
            w.Close()
        End Try

    End Sub

    Public Shared Function GetBytesFromUrl(url As String) As Byte()

        Dim b As Byte()
        Dim myReq As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        Dim myResp As WebResponse = myReq.GetResponse()

        Dim stream As Stream = myResp.GetResponseStream()
        'int i;
        Using br As New BinaryReader(stream)
            'i = (int)(stream.Length);
            b = br.ReadBytes(500000)
            br.Close()
        End Using
        myResp.Close()
        Return b
    End Function

    Public Sub CreateFolder(folderName As String)

        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        Dim DestinationPath As String = ps.HomeDirectoryMapPath & productImages

        If Not Directory.Exists(DestinationPath) Then
            Directory.CreateDirectory(DestinationPath)
        End If

    End Sub

    Protected Function IsProductExist(ByVal nProductId As Integer) As Boolean

        Dim bExist As Boolean = False

        Dim storeProduct = (From p In dbContextStore.NB_Store_Products
                           Where p.ProductRef = nProductId.ToString).SingleOrDefault

        If storeProduct IsNot Nothing Then
            pnStoreProductID = storeProduct.ProductID
            bExist = True
            Return bExist

        Else
            pnStoreProductID = -1
            Return bExist

        End If

    End Function

  


    Protected Sub rgStockProducts_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgStockProducts.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim imgProduct As System.Web.UI.WebControls.Image = e.Item.FindControl("imgProduct")

            Dim hidImage As HiddenField = e.Item.FindControl("hidImage")

            Dim hidImagedName As String = hidImage.Value

            Dim imgThubnailView As System.Web.UI.WebControls.Image = e.Item.FindControl("imgThubnailView")

            imgProduct.ImageUrl = imagesPath + hidImagedName

            imgThubnailView.ImageUrl = imagesPath + hidImagedName

        End If


    End Sub

End Class
