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


Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Framework.Providers

Namespace NEvoWeb.Modules.NB_Store

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' SQL Server implementation of the abstract DataProvider class
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class SqlDataProvider

        Inherits DataProvider

#Region "Private Members"

        Private Const ProviderType As String = "data"
        Private Const ModuleQualifier As String = "NEvoWeb_"

        Private _providerConfiguration As ProviderConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region "Constructors"

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Provider)

            ' Read the attributes for this provider

            'Get Connection string from web.config
            _connectionString = Config.GetConnectionString()

            If _connectionString = "" Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("connectionString")
            End If

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property

#End Region

#Region "Private Methods"

        Private Function GetFullyQualifiedName(ByVal name As String) As String
            Return DatabaseOwner & ObjectQualifier & ModuleQualifier & name
        End Function

        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

#End Region

#Region "NB_Store_Address Methods"

        Public Overrides Function GetNB_Store_Address(ByVal AddressID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Address_Get"), AddressID), IDataReader)
        End Function

        'Public Overrides Function UpdateNB_Store_Address(ByVal AddressID As Integer, ByVal PortalID As Integer, ByVal UserID As Integer, ByVal AddressDescription As String, ByVal AddressName As String, ByVal AddressName2 As String, ByVal Address1 As String, ByVal Address2 As String, ByVal City As String, ByVal RegionCode As String, ByVal CountryCode As String, ByVal PostalCode As String, ByVal Phone1 As String, ByVal Phone2 As String, ByVal PrimaryAddress As Boolean, ByVal CreatedByUser As String, ByVal CreatedDate As Date, ByVal OrderID As Integer, ByVal CompanyName As String, ByVal Extra1 As String, ByVal Extra2 As String, ByVal Extra3 As String, ByVal Extra4 As String) As IDataReader
        '    Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Address_Update"), AddressID, PortalID, UserID, AddressDescription, AddressName, AddressName2, Address1, Address2, City, RegionCode, CountryCode, PostalCode, Phone1, Phone2, PrimaryAddress, CreatedByUser, CreatedDate, OrderID, CompanyName, Extra1, Extra2, Extra3, Extra4), IDataReader)
        'End Function

        Public Overrides Function UpdateNB_Store_Address(ByVal AddressID As Integer, ByVal PortalID As Integer, ByVal UserID As Integer, ByVal AddressDescription As String, ByVal AddressName As String, ByVal Address1 As String, ByVal Address2 As String, ByVal City As String, ByVal RegionCode As String, ByVal CountryCode As String, ByVal PostalCode As String, ByVal Phone1 As String, ByVal Phone2 As String, ByVal PrimaryAddress As Boolean, ByVal CreatedByUser As String, ByVal CreatedDate As Date, ByVal OrderID As Integer, ByVal CompanyName As String, ByVal Extra1 As String, ByVal Extra2 As String, ByVal Extra3 As String, ByVal Extra4 As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Address_Update"), AddressID, PortalID, UserID, AddressDescription, AddressName, Address1, Address2, City, RegionCode, CountryCode, PostalCode, Phone1, Phone2, PrimaryAddress, CreatedByUser, CreatedDate, OrderID, CompanyName, Extra1, Extra2, Extra3, Extra4), IDataReader)
        End Function

        Public Overrides Sub DeleteNB_Store_Address(ByVal AddressID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Address_Delete"), AddressID)
        End Sub

#End Region

#Region "NB_Store_Cart Methods"

        Public Overrides Function GetNB_Store_Cart(ByVal CartID As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Cart_Get"), CartID), IDataReader)
        End Function

        Public Overrides Sub UpdateNB_Store_Cart(ByVal CartID As String, ByVal PortalID As Integer, ByVal UserID As Integer, ByVal DateCreated As Date, ByVal OrderID As Integer, ByVal VATNumber As String, ByVal PromoCode As String, ByVal CountryCode As String, ByVal ShipType As String, ByVal BankTransID As Integer, ByVal BankHtmlRedirect As String, ByVal ShipMethodID As Integer, ByVal CartDiscount As Decimal, ByVal XMLInfo As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Cart_Update"), CartID, PortalID, UserID, DateCreated, OrderID, VATNumber, PromoCode, CountryCode, ShipType, BankTransID, BankHtmlRedirect, ShipMethodID, CartDiscount, XMLInfo)
        End Sub

        Public Overrides Sub DeleteNB_Store_Cart(ByVal CartID As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Cart_Delete"), CartID)
        End Sub
        Public Overrides Sub DeleteOldCarts(ByVal PortalID As Integer, ByVal CartMins As Integer, ByVal OrderMins As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Cart_DeleteOldCarts"), PortalID, CartMins, OrderMins)
        End Sub

        Public Overrides Function GetCartModelQty(ByVal PortalID As Integer, ByVal ModelID As Integer, Optional ByVal CartID As String = "") As Integer
            'If cartID blank then get search all portal carts
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_Cart_GetModelQty"), PortalID, ModelID, CartID), Integer)
        End Function


#End Region

#Region "NB_Store_CartItems Methods"

        Public Overrides Function GetNB_Store_CartItemss(ByVal CartID As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_CartItems_GetList"), CartID), IDataReader)
        End Function
        Public Overrides Function GetNB_Store_CartList(ByVal CartID As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_CartItems_GetCartList"), CartID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_CartItems(ByVal ItemID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_CartItems_Get"), ItemID), IDataReader)
        End Function
        Public Overrides Function GetNB_Store_CartItemsByOptCode(ByVal CartID As String, ByVal OptCode As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_CartItems_GetByOptCode"), CartID, OptCode), IDataReader)
        End Function

        Public Overrides Function UpdateNB_Store_CartItems(ByVal ItemID As Integer, ByVal CartID As String, ByVal Quantity As Integer, ByVal DateCreated As Date, ByVal UnitCost As Decimal, ByVal ModelID As Integer, ByVal OptCode As String, ByVal ItemDesc As String, ByVal Discount As Decimal, ByVal Tax As Decimal, ByVal ProductURL As String, ByVal XMLInfo As String) As Integer
            Return CType(SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_CartItems_Update"), ItemID, CartID, Quantity, DateCreated, UnitCost, ModelID, OptCode, ItemDesc, Discount, Tax, ProductURL, XMLInfo), Integer)
        End Function

        Public Overrides Sub DeleteNB_Store_CartItems(ByVal ItemID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_CartItems_Delete"), ItemID)
        End Sub

#End Region

#Region "NB_Store_Categories Methods"

        Public Overrides Function GetNB_Store_ProductCategoriesAssigned(ByVal ProductID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ProductCategories_GetAssigned"), ProductID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_CategoriesList(ByVal PortalID As Integer, ByVal Lang As String, ByVal ParentID As Integer, ByVal Archived As Boolean, ByVal IncludeArchived As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Categories_GetList"), PortalID, Lang, ParentID, Archived, IncludeArchived), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_Categories(ByVal CategoryID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Categories_Get"), CategoryID, Lang), IDataReader)
        End Function

        Public Overrides Function UpdateNB_Store_Categories(ByVal CategoryID As Integer, ByVal Lang As String, ByVal CategoryName As String, ByVal CategoryDesc As String, ByVal Message As String, ByVal PortalID As Integer, ByVal Archived As Boolean, ByVal CreatedByUser As String, ByVal CreatedDate As Date, ByVal ParentCategoryID As Integer, ByVal ListOrder As Integer, ByVal ProductTemplate As String, ByVal ListItemTemplate As String, ByVal ListAltItemTemplate As String, ByVal Hide As Boolean, ByVal ImageURL As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Categories_Update"), CategoryID, Lang, CategoryName, CategoryDesc, Message, PortalID, Archived, CreatedByUser, CreatedDate, ParentCategoryID, ListOrder, ProductTemplate, ListItemTemplate, ListAltItemTemplate, Hide, ImageURL), IDataReader)
        End Function

        Public Overrides Sub DeleteNB_Store_Categories(ByVal CategoryID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Categories_Delete"), CategoryID)
        End Sub

        Public Overrides Sub ClearCategory(ByVal CategoryID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Categories_Clear"), CategoryID)
        End Sub

#End Region

#Region "NB_Store_Model Methods"


        Public Overrides Function GetNB_Store_ModelStockList(ByVal PortalID As Integer, ByVal Filter As String, ByVal Lang As String, ByVal CategoryID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal IsDealer As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Model_GetStockList"), PortalID, Filter, Lang, CategoryID, PageIndex, PageSize, IsDealer), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ModelStockListSize(ByVal PortalID As Integer, ByVal Filter As String, ByVal Lang As String, ByVal CategoryID As Integer, ByVal IsDealer As Boolean) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_Model_GetStockListSize"), PortalID, Filter, Lang, CategoryID, IsDealer), Integer)
        End Function

        Public Overrides Function GetNB_Store_Models(ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal Lang As String, ByVal IsDealer As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Model_GetList"), PortalID, ProductID, Lang, IsDealer), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_DeletedModels(ByVal PortalID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Model_GetDeletedList"), PortalID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ModelsInStock(ByVal ProductID As Integer, ByVal Lang As String, ByVal IsDealer As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Model_GetInStockList"), ProductID, Lang, IsDealer), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_Model(ByVal ModelID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Model_Get"), ModelID, Lang), IDataReader)
        End Function

        Public Overrides Function GetModelByRef(ByVal ProductID As Integer, ByVal ModelRef As String, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Model_GetByRef"), ProductID, ModelRef, Lang), IDataReader)
        End Function

        Public Overrides Function UpdateNB_Store_Model(ByVal ModelID As Integer, ByVal ProductID As Integer, ByVal ListOrder As Integer, ByVal UnitCost As Decimal, ByVal Barcode As String, ByVal ModelRef As String, ByVal Lang As String, ByVal ModelName As String, ByVal QtyRemaining As Integer, ByVal QtyTrans As Integer, ByVal QtyTransDate As Date, ByVal Deleted As Boolean, ByVal QtyStockSet As Integer, ByVal DealerCost As Decimal, ByVal PurchaseCost As Decimal, ByVal XMLData As String, ByVal Extra As String, ByVal DealerOnly As Boolean, ByVal Allow As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_Model_Update"), ModelID, ProductID, ListOrder, UnitCost, Barcode, ModelRef, Lang, ModelName, QtyRemaining, QtyTrans, QtyTransDate, Deleted, QtyStockSet, DealerCost, PurchaseCost, XMLData, Extra, DealerOnly, Allow), Integer)
        End Function

        Public Overrides Sub DeleteNB_Store_Model(ByVal ModelID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Model_Delete"), ModelID)
        End Sub

        Public Overrides Function GetModelInOrders(ByVal ModelID As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_Model_GetInOrders"), ModelID), Integer)
        End Function


#End Region

#Region "NB_Store_Option Methods"

        Public Overrides Function GetNB_Store_Options(ByVal ProductID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Option_GetList"), ProductID, Lang), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_Option(ByVal OptionID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Option_Get"), OptionID, Lang), IDataReader)
        End Function

        Public Overrides Function UpdateNB_Store_Option(ByVal OptionID As Integer, ByVal ProductID As Integer, ByVal ListOrder As Integer, ByVal Lang As String, ByVal OptionDesc As String, ByVal Attributes As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Option_Update"), OptionID, ProductID, ListOrder, Lang, OptionDesc, Attributes), IDataReader)
        End Function

        Public Overrides Sub DeleteNB_Store_Option(ByVal OptionID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Option_Delete"), OptionID)
        End Sub

#End Region

#Region "NB_Store_OptionValue Methods"

        Public Overrides Function GetNB_Store_OptionValues(ByVal OptionValueID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_OptionValue_GetList"), OptionValueID, Lang), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_OptionValue(ByVal OptionValueID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_OptionValue_Get"), OptionValueID, Lang), IDataReader)
        End Function

        Public Overrides Function UpdateNB_Store_OptionValue(ByVal OptionValueID As Integer, ByVal OptionID As Integer, ByVal AddedCost As Decimal, ByVal ListOrder As Integer, ByVal Lang As String, ByVal OptionValueDesc As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_OptionValue_Update"), OptionValueID, OptionID, AddedCost, ListOrder, Lang, OptionValueDesc), Integer)
        End Function

        Public Overrides Sub DeleteNB_Store_OptionValue(ByVal OptionValueID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_OptionValue_Delete"), OptionValueID)
        End Sub

#End Region

#Region "NB_Store_OrderDetails Methods"

        Public Overrides Function GetNB_Store_OrderDetailss(ByVal OrderID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_OrderDetails_GetList"), OrderID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_OrderDetails(ByVal OrderDetailID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_OrderDetails_Get"), OrderDetailID), IDataReader)
        End Function

        Public Overrides Sub UpdateNB_Store_OrderDetails(ByVal OrderDetailID As Integer, ByVal OrderID As Integer, ByVal Quantity As Integer, ByVal UnitCost As Decimal, ByVal ModelID As Integer, ByVal OptCode As String, ByVal ItemDesc As String, ByVal Discount As Decimal, ByVal Tax As Decimal, ByVal ProductURL As String, ByVal PurchaseCost As Decimal, ByVal CartXMLInfo As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_OrderDetails_Update"), OrderDetailID, OrderID, Quantity, UnitCost, ModelID, OptCode, ItemDesc, Discount, Tax, ProductURL, PurchaseCost, CartXMLInfo)
        End Sub

        Public Overrides Sub DeleteNB_Store_OrderDetails(ByVal OrderDetailID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_OrderDetails_Delete"), OrderDetailID)
        End Sub

#End Region

#Region "NB_Store_Orders Methods"


        Public Overrides Function GetOrdersExportList(ByVal PortalID As Integer, ByVal StatusID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Orders_GetExportList"), PortalID, StatusID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_Orderss(ByVal PortalID As Integer, ByVal UsrID As Integer, ByVal FromDate As Date, ByVal ToDate As Date, ByVal StatusID As Integer, ByVal Filter As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Orders_GetList"), PortalID, UsrID, FromDate, ToDate, StatusID, Filter), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_Orders(ByVal OrderID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Orders_Get"), OrderID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_OrdersByGUID(ByVal OrderGUID As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Orders_GetByGUID"), OrderGUID), IDataReader)
        End Function

        Public Overrides Function UpdateNB_Store_Orders(ByVal OrderID As Integer, ByVal UserID As Integer, ByVal PortalID As Integer, ByVal OrderNumber As String, ByVal OrderDate As Date, ByVal ShipDate As Date, ByVal ShippingAddressID As Integer, ByVal BillingAddressID As Integer, ByVal AppliedTax As Decimal, ByVal ShippingCost As Decimal, ByVal OrderIsPlaced As Boolean, ByVal OrderStatusID As Integer, ByVal PayType As String, ByVal CalculatedTax As Decimal, ByVal NoteMsg As String, ByVal VATNumber As String, ByVal Discount As Decimal, ByVal PromoCode As String, ByVal Total As Decimal, ByVal Email As String, ByVal BankAuthCode As String, ByVal ShipMethodID As Integer, ByVal TrackingCode As String, ByVal Stg2FormXML As String, ByVal Stg3FormXML As String, ByVal AlreadyPaid As Decimal, ByVal OrderGUID As String, ByVal ElapsedDate As Date, ByVal GatewayProvider As String, ByVal CartXMLInfo As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Orders_Update"), OrderID, UserID, PortalID, OrderNumber, GetNull(OrderDate), GetNull(ShipDate), ShippingAddressID, BillingAddressID, AppliedTax, ShippingCost, OrderIsPlaced, OrderStatusID, PayType, CalculatedTax, NoteMsg, VATNumber, Discount, PromoCode, Total, Email, BankAuthCode, ShipMethodID, TrackingCode, Stg2FormXML, Stg3FormXML, AlreadyPaid, OrderGUID, GetNull(ElapsedDate), GatewayProvider, CartXMLInfo), IDataReader)
        End Function

        Public Overrides Sub DeleteNB_Store_Orders(ByVal OrderID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Orders_Delete"), OrderID)
        End Sub

#End Region

#Region "NB_Store_OrderStatus Methods"

        Public Overrides Function GetNB_Store_OrderStatuss(ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_OrderStatus_GetList"), Lang), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_OrderStatus(ByVal OrderStatusID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_OrderStatus_Get"), OrderStatusID, Lang), IDataReader)
        End Function

        Public Overrides Sub UpdateNB_Store_OrderStatus(ByVal OrderStatusID As Integer, ByVal Lang As String, ByVal OrderStatusText As String, ByVal ListOrder As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_OrderStatus_Update"), OrderStatusID, Lang, OrderStatusText, ListOrder)
        End Sub

        Public Overrides Sub DeleteNB_Store_OrderStatus(ByVal OrderStatusID As Integer, ByVal Lang As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_OrderStatus_Delete"), OrderStatusID, Lang)
        End Sub

#End Region

#Region "NB_Store_ProductImage Methods"

        Public Overrides Function GetProductExportImages(ByVal PortalID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ProductImage_GetExportList"), PortalID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ProductImages(ByVal ProductID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ProductImage_GetList"), ProductID, Lang), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ProductImage(ByVal ImageID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ProductImage_Get"), ImageID, Lang), IDataReader)
        End Function

        Public Overrides Function UpdateNB_Store_ProductImage(ByVal ImageID As Integer, ByVal ProductID As Integer, ByVal ImagePath As String, ByVal ListOrder As Integer, ByVal Hidden As Boolean, ByVal Lang As String, ByVal ImageDesc As String, ByVal ImageURL As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_ProductImage_Update"), ImageID, ProductID, ImagePath, ListOrder, Hidden, Lang, ImageDesc, ImageURL), Integer)
        End Function

        Public Overrides Sub UpdateNB_Store_ProductImageOnly(ByVal ImageID As Integer, ByVal ProductID As Integer, ByVal ImagePath As String, ByVal ListOrder As Integer, ByVal Hidden As Boolean, ByVal ImageURL As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ProductImage_UpdateImageOnly"), ImageID, ProductID, ImagePath, ListOrder, Hidden, ImageURL)
        End Sub

        Public Overrides Sub DeleteNB_Store_ProductImage(ByVal ImageID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ProductImage_Delete"), ImageID)
        End Sub

#End Region

#Region "NB_Store_Products Methods"

        Public Overrides Function GetProductExportList(ByVal PortalID As Integer, ByVal Lang As String, ByVal DeletedOnly As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Products_GetExportList"), PortalID, Lang, DeletedOnly), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_Productss(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal GetArchived As Boolean, ByVal FeaturedOnly As Boolean, ByVal OrderBY As String, ByVal OrderDESC As Boolean, ByVal ReturnLimit As Integer, ByVal PageIndex As Integer, ByVal Pagesize As Integer, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean, ByVal CategoryList As String, ByVal ExcludeFeatured As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Products_GetList"), PortalID, CategoryID, Lang, SearchText, GetArchived, FeaturedOnly, OrderBY, OrderDESC, ReturnLimit, PageIndex, Pagesize, SearchDescription, isDealer, CategoryList, ExcludeFeatured), IDataReader)
        End Function

        Public Overrides Function GetProductListSize(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal GetArchived As Boolean, ByVal FeaturedOnly As Boolean, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean, ByVal CategoryList As String, ByVal ExcludeFeatured As Boolean) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_Products_GetListSize"), PortalID, CategoryID, Lang, SearchText, GetArchived, FeaturedOnly, SearchDescription, isDealer, CategoryList, ExcludeFeatured), Integer)
        End Function

        Public Overrides Function GetNB_Store_Products(ByVal ProductID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Products_Get"), ProductID, Lang), IDataReader)
        End Function

        Public Overrides Function GetProductByRef(ByVal PortalID As Integer, ByVal ProductRef As String, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Products_GetByRef"), PortalID, ProductRef, Lang), IDataReader)
        End Function

        Public Overrides Function GetProductInOrders(ByVal ProductID As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_Products_GetInOrders"), ProductID), Integer)
        End Function

        Public Overrides Function CheckIfProductPurchased(ByVal ProductID As Integer, ByVal UserID As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_Products_CheckPurchased"), ProductID, UserID), Integer)
        End Function

        Public Overrides Function GetProductCount(ByVal PortalID As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_Products_GetCount"), PortalID), Integer)
        End Function

        Public Overrides Function UpdateNB_Store_Products(ByVal ProductID As Integer, ByVal PortalID As Integer, ByVal TaxCategoryID As Integer, ByVal Featured As Boolean, ByVal Archived As Boolean, ByVal CreatedByUser As String, ByVal CreatedDate As Date, ByVal IsDeleted As Boolean, ByVal ProductRef As String, ByVal Lang As String, ByVal Summary As String, ByVal Description As String, ByVal Manufacturer As String, ByVal ProductName As String, ByVal XMLData As String, ByVal SEOName As String, ByVal TagWords As String, ByVal IsHidden As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Products_Update"), ProductID, PortalID, TaxCategoryID, Featured, Archived, CreatedByUser, CreatedDate, IsDeleted, ProductRef, Lang, Summary, Description, Manufacturer, ProductName, XMLData, SEOName, TagWords, IsHidden), IDataReader)
        End Function

        Public Overrides Sub DeleteNB_Store_Products(ByVal ProductID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Products_Delete"), ProductID)
        End Sub

#End Region

#Region "NB_Store_Reviews Methods"

        Public Overrides Function GetNB_Store_Reviewss(ByVal ReviewID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Reviews_GetList"), ReviewID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_Reviews(ByVal ReviewID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Reviews_Get"), ReviewID), IDataReader)
        End Function

        Public Overrides Sub UpdateNB_Store_Reviews(ByVal ReviewID As Integer, ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal UserID As Integer, ByVal UserName As String, ByVal Rating As Integer, ByVal Comments As String, ByVal Authorized As Boolean, ByVal CreatedDate As Date)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Reviews_Update"), ReviewID, PortalID, ProductID, UserID, UserName, Rating, Comments, Authorized, CreatedDate)
        End Sub

        Public Overrides Sub DeleteNB_Store_Reviews(ByVal ReviewID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Reviews_Delete"), ReviewID)
        End Sub

#End Region

#Region "NB_Store_SaleRates Methods"

        Public Overrides Function GetNB_Store_SaleRatesList(ByVal PortalID As Integer, ByVal ModelID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SaleRates_GetList"), PortalID, ModelID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_SaleRates(ByVal ItemID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SaleRates_Get"), ItemID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_SaleRatesByCacheKey(ByVal CacheKey As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SaleRates_Get"), CacheKey), IDataReader)
        End Function

        Public Overrides Sub UpdateNB_Store_SaleRates(ByVal ItemID As Integer, ByVal CacheKey As String, ByVal PortalID As Integer, ByVal RoleName As String, ByVal CategoryID As Integer, ByVal ModelID As Integer, ByVal SalePrice As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SaleRates_Update"), ItemID, CacheKey, PortalID, RoleName, CategoryID, ModelID, SalePrice)
        End Sub

        Public Overrides Sub DeleteNB_Store_SaleRates(ByVal ItemID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SaleRates_Delete"), ItemID)
        End Sub

        Public Overrides Sub ClearNB_Store_SaleRates(ByVal PortalID As Integer, ByVal ModelID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SaleRates_Clear"), PortalID, ModelID)
        End Sub

#End Region

#Region "NB_Store_Settings Methods"

        Public Overrides Function GetNB_Store_Settingss(ByVal PortalID As Integer, ByVal Lang As String, ByVal IsHost As Boolean, ByVal SettingName As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Settings_GetList"), PortalID, Lang, IsHost, SettingName), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_Settings(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Settings_Get"), PortalID, SettingName, Lang), IDataReader)
        End Function

        Public Overrides Sub UpdateNB_Store_Settings(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String, ByVal SettingValue As String, ByVal HostOnly As Boolean, ByVal GroupRef As String, ByVal CtrlType As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Settings_Update"), PortalID, SettingName, Lang, SettingValue, HostOnly, GroupRef, CtrlType)
        End Sub

        Public Overrides Sub DeleteNB_Store_Settings(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Settings_Delete"), PortalID, SettingName, Lang)
        End Sub

#End Region

#Region "NB_Store_SettingsText Methods"

        Public Overrides Function GetNB_Store_SettingsTexts(ByVal PortalID As Integer, ByVal Lang As String, ByVal IsHost As Boolean, ByVal SettingName As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SettingsText_GetList"), PortalID, Lang, IsHost, SettingName), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_SettingsText(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SettingsText_Get"), PortalID, SettingName, Lang), IDataReader)
        End Function

        Public Overrides Sub UpdateNB_Store_SettingsText(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String, ByVal SettingText As String, ByVal HostOnly As Boolean, ByVal GroupRef As String, ByVal CtrlType As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SettingsText_Update"), PortalID, SettingName, Lang, SettingText, HostOnly, GroupRef, CtrlType)
        End Sub

        Public Overrides Sub DeleteNB_Store_SettingsText(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SettingsText_Delete"), PortalID, SettingName, Lang)
        End Sub

#End Region

#Region "NB_Store_ShippingRates Methods"

        Public Overrides Function GetNB_Store_ShippingRatess(ByVal PortalID As Integer, ByVal ShipType As String, ByVal Lang As String, ByVal Filter As String, ByVal CategoryID As Integer, ByVal ShipMethodID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingRates_GetList"), PortalID, ShipType, Lang, Filter, CategoryID, ShipMethodID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ShippingRatesListByShipMethodID(ByVal PortalID As Integer, ByVal ShipMethodID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingRates_GetListByShipMethodID"), PortalID, ShipMethodID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ShippingRatesByObjID(ByVal PortalID As Integer, ByVal ObjectId As Integer, ByVal ShipType As String, ByVal ShipMethodID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingRates_GetByObjID"), PortalID, ObjectId, ShipType, ShipMethodID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ShippingRates(ByVal PortalID As Integer, ByVal ItemId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingRates_Get"), PortalID, ItemId), IDataReader)
        End Function


        Public Overrides Sub UpdateNB_Store_ShippingRates(ByVal PortalID As Integer, ByVal ItemId As Integer, ByVal Range1 As Decimal, ByVal Range2 As Decimal, ByVal ObjectId As Integer, ByVal ShipCost As Decimal, ByVal ShipType As String, ByVal Disable As Boolean, ByVal Description As String, ByVal ProductWeight As Decimal, ByVal ProductHeight As Decimal, ByVal ProductLength As Decimal, ByVal ProductWidth As Decimal, ByVal ShipMethodID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingRates_Update"), PortalID, ItemId, Range1, Range2, ObjectId, ShipCost, ShipType, Disable, Description, ProductWeight, ProductHeight, ProductLength, ProductWidth, ShipMethodID)
        End Sub

        Public Overrides Sub DeleteNB_Store_ShippingRates(ByVal ItemId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingRates_Delete"), ItemId)
        End Sub

#End Region

#Region "NB_Store_Stock Methods"

        Public Overrides Function GetNB_Store_Stocks(ByVal StockID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Stock_GetList"), StockID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_Stock(ByVal StockID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Stock_Get"), StockID), IDataReader)
        End Function

        Public Overrides Sub UpdateNB_Store_Stock(ByVal StockID As Integer, ByVal ModelID As Integer, ByVal QtyRemaining As Integer, ByVal QtyTrans As Integer, ByVal QtyTransDate As Date, ByVal ModifiedDate As Date)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Stock_Update"), StockID, ModelID, QtyRemaining, QtyTrans, QtyTransDate, ModifiedDate)
        End Sub

        Public Overrides Sub DeleteNB_Store_Stock(ByVal StockID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Stock_Delete"), StockID)
        End Sub

#End Region

#Region "NB_Store_TaxRates Methods"

        Public Overrides Function GetNB_Store_TaxRatess(ByVal PortalID As Integer, ByVal TaxType As String, ByVal Lang As String, ByVal Filter As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_TaxRates_GetList"), PortalID, TaxType, Lang, Filter), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_TaxRates(ByVal PortalID As Integer, ByVal ItemID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_TaxRates_Get"), PortalID, ItemID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_TaxRatesByObjID(ByVal PortalID As Integer, ByVal ObjectID As Integer, ByVal TaxType As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_TaxRates_GetByObjID"), PortalID, ObjectID, TaxType), IDataReader)
        End Function

        Public Overrides Sub UpdateNB_Store_TaxRates(ByVal PortalID As Integer, ByVal ItemID As Integer, ByVal ObjectID As Integer, ByVal TaxPercent As String, ByVal TaxDesc As String, ByVal TaxType As String, ByVal Disable As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_TaxRates_Update"), PortalID, ItemID, ObjectID, TaxPercent, TaxDesc, TaxType, Disable)
        End Sub

        Public Overrides Sub DeleteNB_Store_TaxRates(ByVal ItemID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_TaxRates_Delete"), ItemID)
        End Sub

#End Region

#Region "NB_Store_ProductDoc Methods"

        Public Overrides Function GetNB_Store_ProductDocList(ByVal ProductID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ProductDoc_GetList"), ProductID, Lang), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ProductDocExportList(ByVal PortalID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ProductDoc_GetExportList"), PortalID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ProductSelectDocList(ByVal Lang As String, ByVal FilterText As String, ByVal PortalID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ProductDoc_GetSelectList"), Lang, FilterText, PortalID), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ProductDoc(ByVal DocID As Integer, ByVal Lang As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ProductDoc_Get"), DocID, Lang), IDataReader)
        End Function

        Public Overrides Function UpdateNB_Store_ProductDoc(ByVal DocID As Integer, ByVal ProductID As Integer, ByVal DocPath As String, ByVal ListOrder As Integer, ByVal Hidden As Boolean, ByVal FileName As String, ByVal FileExt As String, ByVal Lang As String, ByVal DocDesc As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_ProductDoc_Update"), DocID, ProductID, DocPath, ListOrder, Hidden, FileName, FileExt, Lang, DocDesc), Integer)
        End Function

        Public Overrides Sub DeleteNB_Store_ProductDoc(ByVal DocID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ProductDoc_Delete"), DocID)
        End Sub

#End Region

#Region "Clients Control Methods"

        Public Overrides Function GetNB_Store_GetUsers(ByVal PortalID As Integer, ByVal Filter As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_GetUsers"), PortalID, Filter), IDataReader)
        End Function

#End Region

#Region "NB_Store_ProductCategory Methods"

        Public Overrides Sub UpdateProductCategory(ByVal ProductID As Integer, ByVal CategoryID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ProductCategory_Update"), ProductID, CategoryID)
        End Sub

        Public Overrides Sub DeleteProductCategory(ByVal ProductID As Integer, ByVal CategoryID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ProductCategory_Delete"), ProductID, CategoryID)
        End Sub

#End Region

#Region "NB_Store_ShippingMethod Methods"

        Public Overrides Function GetShippingMethodList(ByVal PortalID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingMethod_GetList"), PortalID), IDataReader)
        End Function

        Public Overrides Function GetShippingMethodEnabledList(ByVal PortalID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingMethod_GetEnabledList"), PortalID), IDataReader)
        End Function

        Public Overrides Function GetShippingMethod(ByVal ShipMethodID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingMethod_Get"), ShipMethodID), IDataReader)
        End Function

        Public Overrides Function UpdateShippingMethod(ByVal ShipMethodID As Integer, ByVal PortalID As Integer, ByVal MethodName As String, ByVal MethodDesc As String, ByVal SortOrder As Integer, ByVal TemplateName As String, ByVal Disabled As Boolean, ByVal URLtracker As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingMethod_Update"), ShipMethodID, PortalID, MethodName, MethodDesc, SortOrder, TemplateName, Disabled, URLtracker), Integer)
        End Function

        Public Overrides Sub DeleteShippingMethod(ByVal ShipMethodID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ShippingMethod_Delete"), ShipMethodID)
        End Sub

#End Region

#Region "NB_Store_Promo Methods"

        Public Overrides Function GetNB_Store_PromoList(ByVal PortalID As Integer, ByVal PromoType As String, ByVal SearchText As String, ByVal GetActiveOnly As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Promo_GetList"), PortalID, PromoType, SearchText, GetActiveOnly), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_Promo(ByVal PromoID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_Promo_Get"), PromoID), IDataReader)
        End Function

        Public Overrides Sub UpdateNB_Store_Promo(ByVal PromoID As Integer, ByVal PortalID As Integer, ByVal ObjectID As Integer, ByVal PromoName As String, ByVal PromoType As String, ByVal Range1 As Decimal, ByVal Range2 As Decimal, ByVal RangeStartDate As Date, ByVal RangeEndDate As Date, ByVal PromoAmount As String, ByVal PromoPercent As Integer, ByVal Disabled As Boolean, ByVal PromoCode As String, ByVal PromoGroup As String, ByVal PromoUser As String, ByVal QtyRange1 As Integer, ByVal QtyRange2 As Integer, ByVal PromoEmail As String, ByVal XMLData As String, ByVal MaxUsagePerUser As Integer, ByVal MaxUsage As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Promo_Update"), PromoID, PortalID, ObjectID, PromoName, PromoType, Range1, Range2, RangeStartDate, RangeEndDate, PromoAmount, PromoPercent, Disabled, PromoCode, PromoGroup, PromoUser, QtyRange1, QtyRange2, PromoEmail, XMLData, MaxUsagePerUser, MaxUsage)
        End Sub

        Public Overrides Sub DeleteNB_Store_Promo(ByVal PromoID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_Promo_Delete"), PromoID)
        End Sub

        Public Overrides Function GetNB_Store_PromoCodeUsage(ByVal PortalID As Integer, ByVal PromoCode As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_Promo_GetPromoCodeUsage"), PortalID, PromoCode), Integer)
        End Function

#End Region

#Region "NB_Store_SQLReport Methods"

        Public Overrides Function GetSQLAdminReportList(ByVal PortalID As Integer, ByVal IsEditable As Boolean, ByVal SearchText As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReport_GetList"), PortalID, IsEditable, SearchText), IDataReader)
        End Function

        Public Overrides Function GetSQLReport(ByVal ReportID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReport_Get"), ReportID), IDataReader)
        End Function

        Public Overrides Function GetSQLReportByRef(ByVal PortalID As Integer, ByVal ReportRef As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReport_GetByRef"), PortalID, ReportRef), IDataReader)
        End Function

        Public Overrides Function UpdateSQLReport(ByVal ReportID As Integer, ByVal PortalID As Integer, ByVal ReportName As String, ByVal SQL As String, ByVal SchedulerFlag As Boolean, ByVal SchStartHour As String, ByVal SchStartMins As String, ByVal SchReRunMins As String, ByVal LastRunTime As Date, ByVal AllowExport As Boolean, ByVal AllowDisplay As Boolean, ByVal DisplayInLine As Boolean, ByVal EmailResults As Boolean, ByVal EmailFrom As String, ByVal EmailTo As String, ByVal ShowSQL As Boolean, ByVal SQLConnectionString As String, ByVal ReportRef As String, ByVal AllowPaging As Boolean, ByVal ReportTitle As String, ByVal FieldDelimeter As String, ByVal FieldQualifier As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReport_Update"), ReportID, PortalID, ReportName, SQL, SchedulerFlag, SchStartHour, SchStartMins, SchReRunMins, LastRunTime, AllowExport, AllowDisplay, DisplayInLine, EmailResults, EmailFrom, EmailTo, ShowSQL, SQLConnectionString, ReportRef, AllowPaging, ReportTitle, FieldDelimeter, FieldQualifier), Integer)
        End Function

        Public Overrides Sub DeleteSQLReport(ByVal ReportID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReport_Delete"), ReportID)
        End Sub

#End Region

#Region "NEvoWeb_SQLReportParam Methods"

        Public Overrides Function GetSQLReportParamList(ByVal ReportID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReportParam_GetList"), ReportID), IDataReader)
        End Function

        Public Overrides Function GetSQLReportParam(ByVal ReportParamID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReportParam_Get"), ReportParamID), IDataReader)
        End Function

        Public Overrides Sub UpdateSQLReportParam(ByVal ReportParamID As Integer, ByVal ReportID As Integer, ByVal ParamName As String, ByVal ParamType As String, ByVal ParamValue As String, ByVal ParamSource As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReportParam_Update"), ReportParamID, ReportID, ParamName, ParamType, ParamValue, ParamSource)
        End Sub

        Public Overrides Sub DeleteSQLReportParam(ByVal ReportParamID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReportParam_Delete"), ReportParamID)
        End Sub

#End Region

#Region "NB_Store_SQLReportXSL Methods"

        Public Overrides Function GetSQLReportXSLList(ByVal ReportID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReportXSL_GetList"), ReportID), IDataReader)
        End Function

        Public Overrides Function GetSQLReportXSL(ByVal ReportXSLID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReportXSL_Get"), ReportXSLID), IDataReader)
        End Function

        Public Overrides Sub UpdateSQLReportXSL(ByVal ReportXSLID As Integer, ByVal ReportID As Integer, ByVal XMLInput As String, ByVal XSLFile As String, ByVal OutputFile As String, ByVal DisplayResults As Boolean, ByVal SortOrder As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReportXSL_Update"), ReportXSLID, ReportID, XMLInput, XSLFile, OutputFile, DisplayResults, SortOrder)
        End Sub

        Public Overrides Sub DeleteSQLReportXSL(ByVal ReportXSLID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SQLReportXSL_Delete"), ReportXSLID)
        End Sub

        Public Overrides Function ExecuteSQLReportXml(ByVal SQLcommand As String) As String
            Dim strRtn As String = ""
            Dim conn As New SqlClient.SqlConnection(ConnectionString)

            conn.Open()

            Dim xreader As System.Xml.XmlReader = SqlHelper.ExecuteXmlReader(conn, CommandType.Text, SQLcommand)
            Dim sb As New StringBuilder()
            sb.Append("<root>")
            xreader.MoveToContent()
            While xreader.Read()
                sb.Append(xreader.ReadOuterXml)
            End While
            sb.Append("</root>")
            conn.Close()

            Return sb.ToString
        End Function

        Private Sub ShowData(ByVal MyIDataReader As IDataReader, ByVal GridView As DataGrid)
            GridView.DataSource = MyIDataReader
            GridView.DataBind()
        End Sub

        Public Overrides Sub popDataGridSQL(ByVal SQLcommand As String, ByVal GridView As DataGrid)
            ' script dynamic substitution
            SQLcommand = SQLcommand.Replace("{databaseOwner}", DatabaseOwner)
            SQLcommand = SQLcommand.Replace("{objectQualifier}", ObjectQualifier)
            ShowData(SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, SQLcommand), GridView)
        End Sub

        Public Overrides Function ExecuteSQL(ByVal SQLcommand As String) As IDataReader
            ' script dynamic substitution
            SQLcommand = SQLcommand.Replace("{databaseOwner}", DatabaseOwner)
            SQLcommand = SQLcommand.Replace("{objectQualifier}", ObjectQualifier)
            Return SqlHelper.ExecuteReader(_connectionString, CommandType.Text, SQLcommand)
        End Function

        Public Overrides Function ExecuteSQLReportText(ByVal SQLcommand As String, ByVal FieldDelimeter As String, ByVal FieldQualifier As String, ByVal ExportHeader As Boolean) As String
            Dim dr As IDataReader = ExecuteSQL(SQLcommand)
            Dim rtnStr As String = ""

            FieldQualifier = Trim(FieldQualifier)
            FieldDelimeter = Trim(FieldDelimeter)

            If Not dr Is Nothing Then

                'do header
                If ExportHeader Then
                    For x As Integer = 0 To dr.FieldCount - 1
                        rtnStr &= FieldQualifier & dr.GetName(x).ToString & FieldQualifier & FieldDelimeter
                    Next
                    rtnStr = rtnStr.TrimEnd(","c) & vbCrLf
                End If

                'do data
                While dr.Read
                    For x As Integer = 0 To dr.FieldCount - 1
                        rtnStr &= FieldQualifier & dr.Item(x).ToString & FieldQualifier & FieldDelimeter
                    Next
                    rtnStr = rtnStr.TrimEnd(","c) & vbCrLf

                End While

            End If

            Return rtnStr
        End Function


#End Region

#Region "NB_Store_ProductRelated Methods"

        Public Overrides Function GetNB_Store_ProductRelatedList(ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal Lang As String, ByVal RelatedType As Integer, ByVal GetAll As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ProductRelated_GetList"), PortalID, ProductID, Lang, RelatedType, GetAll), IDataReader)
        End Function

        Public Overrides Function GetNB_Store_ProductRelated(ByVal RelatedID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NB_Store_ProductRelated_Get"), RelatedID), IDataReader)
        End Function

        Public Overrides Function UpdateNB_Store_ProductRelated(ByVal RelatedID As Integer, ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal RelatedProductID As Integer, ByVal DiscountAmt As Decimal, ByVal DiscountPercent As Decimal, ByVal ProductQty As Integer, ByVal MaxQty As Integer, ByVal RelatedType As Integer, ByVal Disabled As Boolean, ByVal NotAvailable As Boolean, ByVal BiDirectional As Boolean) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NB_Store_ProductRelated_Update"), RelatedID, PortalID, ProductID, RelatedProductID, DiscountAmt, DiscountPercent, ProductQty, MaxQty, RelatedType, Disabled, NotAvailable, BiDirectional), Integer)
        End Function

        Public Overrides Sub DeleteNB_Store_ProductRelated(ByVal RelatedID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ProductRelated_Delete"), RelatedID)
        End Sub

        Public Overrides Sub DeleteNB_Store_ProductRelatedByProduct(ByVal ProductID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ProductRelated_DeleteByProduct"), ProductID)
        End Sub

        Public Overrides Sub NotAvailableProductRelated(ByVal ProductID As Integer, ByVal Flag As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ProductRelated_NotAvailable"), ProductID, Flag)
        End Sub


#End Region

#Region "NB_Store_SearchWords Methods"


        Public Overrides Sub UpdateSearchWord(ByVal PortalID As Integer, ByVal SearchWord As String, ByVal WordPosition As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SearchWords_Update"), PortalID, SearchWord, WordPosition)
        End Sub

        Public Overrides Sub PurgeSearchWord(ByVal PortalID As Integer, ByVal PurgeBeforeDate As Date)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SearchWords_Purge"), PortalID, PurgeBeforeDate)
        End Sub

        Public Overrides Sub ProcessSearchWords(ByVal PortalID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_SearchWords_Process"), PortalID)
        End Sub

#End Region

        Public Overrides Sub ClearDownStore(ByVal PortalID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NB_Store_ClearDownStore"), PortalID)
        End Sub


    End Class

End Namespace