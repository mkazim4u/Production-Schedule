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
Imports DotNetNuke

Namespace NEvoWeb.Modules.NB_Store

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' An abstract class for the data access layer
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "NEvoWeb.Modules.NB_Store", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "NB_Store_Address Abstract Methods"

        Public MustOverride Function GetNB_Store_Address(ByVal AddressID As Integer) As IDataReader
        Public MustOverride Function UpdateNB_Store_Address(ByVal AddressID As Integer, ByVal PortalID As Integer, ByVal UserID As Integer, ByVal AddressDescription As String, ByVal AddressName As String, ByVal Address1 As String, ByVal Address2 As String, ByVal City As String, ByVal RegionCode As String, ByVal CountryCode As String, ByVal PostalCode As String, ByVal Phone1 As String, ByVal Phone2 As String, ByVal PrimaryAddress As Boolean, ByVal CreatedByUser As String, ByVal CreatedDate As Date, ByVal OrderID As Integer, ByVal CompanyName As String, ByVal Extra1 As String, ByVal Extra2 As String, ByVal Extra3 As String, ByVal Extra4 As String) As IDataReader
        Public MustOverride Sub DeleteNB_Store_Address(ByVal AddressID As Integer)

#End Region

#Region "NB_Store_Cart Abstract Methods"

        Public MustOverride Function GetNB_Store_Cart(ByVal CartID As String) As IDataReader
        Public MustOverride Sub UpdateNB_Store_Cart(ByVal CartID As String, ByVal PortalID As Integer, ByVal UserID As Integer, ByVal DateCreated As Date, ByVal OrderID As Integer, ByVal VATNumber As String, ByVal PromoCode As String, ByVal CountryCode As String, ByVal ShipType As String, ByVal BankTransID As Integer, ByVal BankHtmlRedirect As String, ByVal ShipMethodID As Integer, ByVal CartDiscount As Decimal, ByVal XMLInfo As String)
        Public MustOverride Sub DeleteNB_Store_Cart(ByVal CartID As String)
        Public MustOverride Sub DeleteOldCarts(ByVal PortalID As Integer, ByVal CartMins As Integer, ByVal OrderMins As Integer)
        Public MustOverride Function GetCartModelQty(ByVal PortalID As Integer, ByVal ModelID As Integer, Optional ByVal CartID As String = "") As Integer

#End Region

#Region "NB_Store_CartItems Abstract Methods"

        Public MustOverride Function GetNB_Store_CartItemss(ByVal CartID As String) As IDataReader
        Public MustOverride Function GetNB_Store_CartList(ByVal CartID As String) As IDataReader
        Public MustOverride Function GetNB_Store_CartItems(ByVal ItemID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_CartItemsByOptCode(ByVal CartID As String, ByVal OptCode As String) As IDataReader
        Public MustOverride Function UpdateNB_Store_CartItems(ByVal ItemID As Integer, ByVal CartID As String, ByVal Quantity As Integer, ByVal DateCreated As Date, ByVal UnitCost As Decimal, ByVal ModelID As Integer, ByVal OptCode As String, ByVal ItemDesc As String, ByVal Discount As Decimal, ByVal Tax As Decimal, ByVal ProductURL As String, ByVal XMLInfo As String) As Integer
        Public MustOverride Sub DeleteNB_Store_CartItems(ByVal ItemID As Integer)

#End Region

#Region "NB_Store_Categories Abstract Methods"

        Public MustOverride Function GetNB_Store_ProductCategoriesAssigned(ByVal ProductID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_Categories(ByVal CategoryID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function GetNB_Store_CategoriesList(ByVal PortalID As Integer, ByVal Lang As String, ByVal ParentID As Integer, ByVal Archived As Boolean, ByVal IncludeArchived As Boolean) As IDataReader
        Public MustOverride Function UpdateNB_Store_Categories(ByVal CategoryID As Integer, ByVal Lang As String, ByVal CategoryName As String, ByVal CategoryDesc As String, ByVal Message As String, ByVal PortalID As Integer, ByVal Archived As Boolean, ByVal CreatedByUser As String, ByVal CreatedDate As Date, ByVal ParentCategoryID As Integer, ByVal ListOrder As Integer, ByVal ProductTemplate As String, ByVal ListItemTemplate As String, ByVal ListAltItemTemplate As String, ByVal Hide As Boolean, ByVal ImageURL As String) As IDataReader
        Public MustOverride Sub DeleteNB_Store_Categories(ByVal CategoryID As Integer)
        Public MustOverride Sub ClearCategory(ByVal CategoryID As Integer)

#End Region

#Region "NB_Store_Model Abstract Methods"

        Public MustOverride Function GetNB_Store_ModelStockList(ByVal PortalID As Integer, ByVal Filter As String, ByVal Lang As String, ByVal CategoryID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal IsDealer As Boolean) As IDataReader
        Public MustOverride Function GetNB_Store_ModelStockListSize(ByVal PortalID As Integer, ByVal Filter As String, ByVal Lang As String, ByVal CategoryID As Integer, ByVal IsDealer As Boolean) As Integer
        Public MustOverride Function GetNB_Store_Models(ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal Lang As String, ByVal IsDealer As Boolean) As IDataReader
        Public MustOverride Function GetNB_Store_DeletedModels(ByVal PortalID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_ModelsInStock(ByVal ProductID As Integer, ByVal Lang As String, ByVal IsDealer As Boolean) As IDataReader
        Public MustOverride Function GetNB_Store_Model(ByVal ModelID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function GetModelByRef(ByVal ProductID As Integer, ByVal ModelRef As String, ByVal Lang As String) As IDataReader
        Public MustOverride Function UpdateNB_Store_Model(ByVal ModelID As Integer, ByVal ProductID As Integer, ByVal ListOrder As Integer, ByVal AddedCost As Decimal, ByVal Barcode As String, ByVal ModelRef As String, ByVal Lang As String, ByVal ModelDesc As String, ByVal QtyRemaining As Integer, ByVal QtyTrans As Integer, ByVal QtyTransDate As Date, ByVal Deleted As Boolean, ByVal QtyStockSet As Integer, ByVal DealerCost As Decimal, ByVal PurchaseCost As Decimal, ByVal XMLData As String, ByVal Extra As String, ByVal DealerOnly As Boolean, ByVal Allow As Integer) As Integer
        Public MustOverride Sub DeleteNB_Store_Model(ByVal ModelID As Integer)
        Public MustOverride Function GetModelInOrders(ByVal ModelID As Integer) As Integer

#End Region

#Region "NB_Store_Option Abstract Methods"

        Public MustOverride Function GetNB_Store_Options(ByVal ProductID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function GetNB_Store_Option(ByVal OptionID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function UpdateNB_Store_Option(ByVal OptionID As Integer, ByVal ProductID As Integer, ByVal ListOrder As Integer, ByVal Lang As String, ByVal OptionDesc As String, ByVal Attributes As String) As IDataReader
        Public MustOverride Sub DeleteNB_Store_Option(ByVal OptionID As Integer)

#End Region

#Region "NB_Store_OptionValue Abstract Methods"

        Public MustOverride Function GetNB_Store_OptionValues(ByVal OptionValueID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function GetNB_Store_OptionValue(ByVal OptionValueID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function UpdateNB_Store_OptionValue(ByVal OptionValueID As Integer, ByVal OptionID As Integer, ByVal AddedCost As Decimal, ByVal ListOrder As Integer, ByVal Lang As String, ByVal OptionValueDesc As String) As Integer
        Public MustOverride Sub DeleteNB_Store_OptionValue(ByVal OptionValueID As Integer)

#End Region

#Region "NB_Store_OrderDetails Abstract Methods"

        Public MustOverride Function GetNB_Store_OrderDetailss(ByVal OrderID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_OrderDetails(ByVal OrderDetailID As Integer) As IDataReader
        Public MustOverride Sub UpdateNB_Store_OrderDetails(ByVal OrderDetailID As Integer, ByVal OrderID As Integer, ByVal Quantity As Integer, ByVal UnitCost As Decimal, ByVal ModelID As Integer, ByVal OptCode As String, ByVal ItemDesc As String, ByVal Discount As Decimal, ByVal Tax As Decimal, ByVal ProductURL As String, ByVal PurchaseCost As Decimal, ByVal CartXMLInfo As String)
        Public MustOverride Sub DeleteNB_Store_OrderDetails(ByVal OrderDetailID As Integer)

#End Region

#Region "NB_Store_Orders Abstract Methods"

        Public MustOverride Function GetOrdersExportList(ByVal PortalID As Integer, ByVal StatusID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_Orderss(ByVal PortalID As Integer, ByVal UsrID As Integer, ByVal FromDate As Date, ByVal ToDate As Date, ByVal StatusID As Integer, ByVal Filter As String) As IDataReader
        Public MustOverride Function GetNB_Store_Orders(ByVal OrderID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_OrdersByGUID(ByVal OrderGUID As String) As IDataReader
        Public MustOverride Function UpdateNB_Store_Orders(ByVal OrderID As Integer, ByVal UserID As Integer, ByVal PortalID As Integer, ByVal OrderNumber As String, ByVal OrderDate As Date, ByVal ShipDate As Date, ByVal ShippingAddressID As Integer, ByVal BillingAddressID As Integer, ByVal AppliedTax As Decimal, ByVal ShippingCost As Decimal, ByVal OrderIsPlaced As Boolean, ByVal OrderStatusID As Integer, ByVal PayType As String, ByVal CalculatedTax As Decimal, ByVal NoteMsg As String, ByVal VATNumber As String, ByVal Discount As Decimal, ByVal PromoCode As String, ByVal Total As Decimal, ByVal Email As String, ByVal BankAuthCode As String, ByVal ShipMethodID As Integer, ByVal TrackingCode As String, ByVal Stg2FormXML As String, ByVal Stg3FormXML As String, ByVal AlreadyPaid As Decimal, ByVal OrderGUID As String, ByVal ElapsedDate As Date, ByVal GatewayProvider As String, ByVal CartXMLInfo As String) As IDataReader
        Public MustOverride Sub DeleteNB_Store_Orders(ByVal OrderID As Integer)

#End Region

#Region "NB_Store_OrderStatus Abstract Methods"

        Public MustOverride Function GetNB_Store_OrderStatuss(ByVal Lang As String) As IDataReader
        Public MustOverride Function GetNB_Store_OrderStatus(ByVal OrderStatusID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Sub UpdateNB_Store_OrderStatus(ByVal OrderStatusID As Integer, ByVal Lang As String, ByVal OrderStatusText As String, ByVal ListOrder As Integer)
        Public MustOverride Sub DeleteNB_Store_OrderStatus(ByVal OrderStatusID As Integer, ByVal Lang As String)

#End Region

#Region "NB_Store_ProductImage Abstract Methods"

        Public MustOverride Function GetProductExportImages(ByVal PortalID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_ProductImages(ByVal ProductID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function GetNB_Store_ProductImage(ByVal ImageID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function UpdateNB_Store_ProductImage(ByVal ImageID As Integer, ByVal ProductID As Integer, ByVal ImagePath As String, ByVal ListOrder As Integer, ByVal Hidden As Boolean, ByVal Lang As String, ByVal ImageDesc As String, ByVal ImageURL As String) As Integer
        Public MustOverride Sub UpdateNB_Store_ProductImageOnly(ByVal ImageID As Integer, ByVal ProductID As Integer, ByVal ImagePath As String, ByVal ListOrder As Integer, ByVal Hidden As Boolean, ByVal ImageURL As String)
        Public MustOverride Sub DeleteNB_Store_ProductImage(ByVal ImageID As Integer)

#End Region

#Region "NB_Store_Products Abstract Methods"

        Public MustOverride Function GetProductExportList(ByVal PortalID As Integer, ByVal Lang As String, ByVal DeletedOnly As Boolean) As IDataReader
        Public MustOverride Function GetNB_Store_Productss(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal GetArchived As Boolean, ByVal FeaturedOnly As Boolean, ByVal OrderBY As String, ByVal OrderDESC As Boolean, ByVal ReturnLimit As Integer, ByVal PageIndex As Integer, ByVal Pagesize As Integer, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean, ByVal CategoryList As String, ByVal ExcludeFeatured As Boolean) As IDataReader
        Public MustOverride Function GetNB_Store_Products(ByVal ProductID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function GetProductByRef(ByVal PortalID As Integer, ByVal ProductRef As String, ByVal Lang As String) As IDataReader
        Public MustOverride Function GetProductInOrders(ByVal ProductID As Integer) As Integer
        Public MustOverride Function GetProductCount(ByVal PortalID As Integer) As Integer
        Public MustOverride Function CheckIfProductPurchased(ByVal ProductID As Integer, ByVal UserID As Integer) As Integer
        Public MustOverride Function UpdateNB_Store_Products(ByVal ProductID As Integer, ByVal PortalID As Integer, ByVal TaxCategoryID As Integer, ByVal Featured As Boolean, ByVal Archived As Boolean, ByVal CreatedByUser As String, ByVal CreatedDate As Date, ByVal IsDeleted As Boolean, ByVal ProductRef As String, ByVal Lang As String, ByVal Summary As String, ByVal Description As String, ByVal Manufacturer As String, ByVal ProductName As String, ByVal XMLData As String, ByVal SEOName As String, ByVal TagWords As String, ByVal IsHidden As Boolean) As IDataReader
        Public MustOverride Sub DeleteNB_Store_Products(ByVal ProductID As Integer)
        Public MustOverride Function GetProductListSize(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal GetArchived As Boolean, ByVal FeaturedOnly As Boolean, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean, ByVal CategoryList As String, ByVal ExcludeFeatured As Boolean) As Integer

#End Region

#Region "NB_Store_Reviews Abstract Methods"

        Public MustOverride Function GetNB_Store_Reviewss(ByVal ReviewID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_Reviews(ByVal ReviewID As Integer) As IDataReader
        Public MustOverride Sub UpdateNB_Store_Reviews(ByVal ReviewID As Integer, ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal UserID As Integer, ByVal UserName As String, ByVal Rating As Integer, ByVal Comments As String, ByVal Authorized As Boolean, ByVal CreatedDate As Date)
        Public MustOverride Sub DeleteNB_Store_Reviews(ByVal ReviewID As Integer)

#End Region

#Region "NB_Store_SaleRates Abstract Methods"

        Public MustOverride Function GetNB_Store_SaleRatesList(ByVal PortalID As Integer, ByVal ModelID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_SaleRates(ByVal ItemID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_SaleRatesByCacheKey(ByVal CacheKey As String) As IDataReader
        Public MustOverride Sub UpdateNB_Store_SaleRates(ByVal ItemID As Integer, ByVal CacheKey As String, ByVal PortalID As Integer, ByVal RoleName As String, ByVal CategoryID As Integer, ByVal ModelID As Integer, ByVal SalePrice As String)
        Public MustOverride Sub DeleteNB_Store_SaleRates(ByVal ItemID As Integer)
        Public MustOverride Sub ClearNB_Store_SaleRates(ByVal PortalID As Integer, ByVal ModelID As Integer)

#End Region

#Region "NB_Store_Settings Abstract Methods"

        Public MustOverride Function GetNB_Store_Settingss(ByVal PortalID As Integer, ByVal Lang As String, ByVal IsHost As Boolean, ByVal SettingName As String) As IDataReader
        Public MustOverride Function GetNB_Store_Settings(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String) As IDataReader
        Public MustOverride Sub UpdateNB_Store_Settings(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String, ByVal SettingValue As String, ByVal HostOnly As Boolean, ByVal GroupRef As String, ByVal CtrlType As String)
        Public MustOverride Sub DeleteNB_Store_Settings(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String)

#End Region

#Region "NB_Store_SettingsText Abstract Methods"

        Public MustOverride Function GetNB_Store_SettingsTexts(ByVal PortalID As Integer, ByVal Lang As String, ByVal IsHost As Boolean, ByVal SettingName As String) As IDataReader
        Public MustOverride Function GetNB_Store_SettingsText(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String) As IDataReader
        Public MustOverride Sub UpdateNB_Store_SettingsText(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String, ByVal SettingText As String, ByVal HostOnly As Boolean, ByVal GroupRef As String, ByVal CtrlType As String)
        Public MustOverride Sub DeleteNB_Store_SettingsText(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String)

#End Region

#Region "NB_Store_ShippingRates Abstract Methods"

        Public MustOverride Function GetNB_Store_ShippingRatess(ByVal PortalID As Integer, ByVal ShipType As String, ByVal Lang As String, ByVal Filter As String, ByVal CategoryID As Integer, ByVal ShipMethodID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_ShippingRatesListByShipMethodID(ByVal PortalID As Integer, ByVal ShipMethodID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_ShippingRates(ByVal PortalID As Integer, ByVal ItemId As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_ShippingRatesByObjID(ByVal PortalID As Integer, ByVal ObjectId As Integer, ByVal ShipType As String, ByVal ShipMethodID As Integer) As IDataReader
        Public MustOverride Sub UpdateNB_Store_ShippingRates(ByVal PortalID As Integer, ByVal ItemId As Integer, ByVal Range1 As Decimal, ByVal Range2 As Decimal, ByVal ObjectId As Integer, ByVal ShipCost As Decimal, ByVal ShipType As String, ByVal Disable As Boolean, ByVal Description As String, ByVal ProductWeight As Decimal, ByVal ProductHeight As Decimal, ByVal ProductLength As Decimal, ByVal ProductWidth As Decimal, ByVal ShipMethodID As Integer)
        Public MustOverride Sub DeleteNB_Store_ShippingRates(ByVal ItemId As Integer)

#End Region

#Region "NB_Store_Stock Abstract Methods"

        Public MustOverride Function GetNB_Store_Stocks(ByVal StockID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_Stock(ByVal StockID As Integer) As IDataReader
        Public MustOverride Sub UpdateNB_Store_Stock(ByVal StockID As Integer, ByVal ModelID As Integer, ByVal QtyRemaining As Integer, ByVal QtyTrans As Integer, ByVal QtyTransDate As Date, ByVal ModifiedDate As Date)
        Public MustOverride Sub DeleteNB_Store_Stock(ByVal StockID As Integer)

#End Region

#Region "NB_Store_TaxRates Abstract Methods"

        Public MustOverride Function GetNB_Store_TaxRatess(ByVal PortalID As Integer, ByVal TaxType As String, ByVal Lang As String, ByVal Filter As String) As IDataReader
        Public MustOverride Function GetNB_Store_TaxRates(ByVal PortalID As Integer, ByVal ItemID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_TaxRatesByObjID(ByVal PortalID As Integer, ByVal ObjectID As Integer, ByVal TaxType As String) As IDataReader
        Public MustOverride Sub UpdateNB_Store_TaxRates(ByVal PortalID As Integer, ByVal ItemID As Integer, ByVal ObjectID As Integer, ByVal TaxPercent As String, ByVal TaxDesc As String, ByVal TaxType As String, ByVal Disable As Boolean)
        Public MustOverride Sub DeleteNB_Store_TaxRates(ByVal ItemID As Integer)

#End Region

#Region "NB_Store_ProductDoc Abstract Methods"

        Public MustOverride Function GetNB_Store_ProductDocList(ByVal ProductID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function GetNB_Store_ProductDocExportList(ByVal PortalID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_ProductSelectDocList(ByVal Lang As String, ByVal FilterText As String, ByVal PortalID As Integer) As IDataReader
        Public MustOverride Function GetNB_Store_ProductDoc(ByVal DocID As Integer, ByVal Lang As String) As IDataReader
        Public MustOverride Function UpdateNB_Store_ProductDoc(ByVal DocID As Integer, ByVal ProductID As Integer, ByVal DocPath As String, ByVal ListOrder As Integer, ByVal Hidden As Boolean, ByVal FileName As String, ByVal FileExt As String, ByVal Lang As String, ByVal DocDesc As String) As Integer
        Public MustOverride Sub DeleteNB_Store_ProductDoc(ByVal DocID As Integer)

#End Region

#Region "NB_Store_ProductCategory Abstract Methods"

        Public MustOverride Sub UpdateProductCategory(ByVal ProductID As Integer, ByVal CategoryID As Integer)
        Public MustOverride Sub DeleteProductCategory(ByVal ProductID As Integer, ByVal CategoryID As Integer)

#End Region

#Region "Clients Control Methods"

        Public MustOverride Function GetNB_Store_GetUsers(ByVal PortalID As Integer, ByVal Filter As String) As IDataReader

#End Region

#Region "NB_Store_ShippingMethod Abstract Methods"

        Public MustOverride Function GetShippingMethodList(ByVal PortalID As Integer) As IDataReader
        Public MustOverride Function GetShippingMethodEnabledList(ByVal PortalID As Integer) As IDataReader
        Public MustOverride Function GetShippingMethod(ByVal ShipMethodID As Integer) As IDataReader
        Public MustOverride Function UpdateShippingMethod(ByVal ShipMethodID As Integer, ByVal PortalID As Integer, ByVal MethodName As String, ByVal MethodDesc As String, ByVal SortOrder As Integer, ByVal TemplateName As String, ByVal Disabled As Boolean, ByVal URLtracker As String) As Integer
        Public MustOverride Sub DeleteShippingMethod(ByVal ShipMethodID As Integer)

#End Region

#Region "NB_Store_Promo Abstract Methods"

        Public MustOverride Function GetNB_Store_PromoList(ByVal PortalID As Integer, ByVal PromoType As String, ByVal SearchText As String, ByVal GetActiveOnly As Boolean) As IDataReader
        Public MustOverride Function GetNB_Store_Promo(ByVal PromoID As Integer) As IDataReader
        Public MustOverride Sub UpdateNB_Store_Promo(ByVal PromoID As Integer, ByVal PortalID As Integer, ByVal ObjectID As Integer, ByVal PromoName As String, ByVal PromoType As String, ByVal Range1 As Decimal, ByVal Range2 As Decimal, ByVal RangeStartDate As Date, ByVal RangeEndDate As Date, ByVal PromoAmount As String, ByVal PromoPercent As Integer, ByVal Disabled As Boolean, ByVal PromoCode As String, ByVal PromoGroup As String, ByVal PromoUser As String, ByVal QtyRange1 As Integer, ByVal QtyRange2 As Integer, ByVal PromoEmail As String, ByVal XMLData As String, ByVal MaxUsagePerUser As Integer, ByVal MaxUsage As Integer)
        Public MustOverride Sub DeleteNB_Store_Promo(ByVal PromoID As Integer)
        Public MustOverride Function GetNB_Store_PromoCodeUsage(ByVal PortalID As Integer, ByVal PromoCode As String) As Integer

#End Region

#Region "SQLReport Abstract Methods"

        Public MustOverride Function GetSQLAdminReportList(ByVal PortalID As Integer, ByVal IsEditable As Boolean, ByVal SearchText As String) As IDataReader
        Public MustOverride Function GetSQLReport(ByVal ReportID As Integer) As IDataReader
        Public MustOverride Function GetSQLReportByRef(ByVal PortalID As Integer, ByVal ReportRef As String) As IDataReader
        Public MustOverride Function UpdateSQLReport(ByVal ReportID As Integer, ByVal PortalID As Integer, ByVal ReportName As String, ByVal SQL As String, ByVal SchedulerFlag As Boolean, ByVal SchStartHour As String, ByVal SchStartMins As String, ByVal SchReRunMins As String, ByVal LastRunTime As Date, ByVal AllowExport As Boolean, ByVal AllowDisplay As Boolean, ByVal DisplayInLine As Boolean, ByVal EmailResults As Boolean, ByVal EmailFrom As String, ByVal EmailTo As String, ByVal ShowSQL As Boolean, ByVal SQLConnectionString As String, ByVal ReportRef As String, ByVal AllowPaging As Boolean, ByVal ReportTitle As String, ByVal FieldDelimeter As String, ByVal FieldQualifier As String) As Integer
        Public MustOverride Sub DeleteSQLReport(ByVal ReportID As Integer)
        Public MustOverride Function ExecuteSQLReportXml(ByVal SQLcommand As String) As String
        Public MustOverride Sub popDataGridSQL(ByVal SQLcommand As String, ByVal GridView As DataGrid)
        Public MustOverride Function ExecuteSQL(ByVal SQLcommand As String) As IDataReader
        Public MustOverride Function ExecuteSQLReportText(ByVal SQLcommand As String, ByVal FieldDelimeter As String, ByVal FieldQualifier As String, ByVal ExportHeader As Boolean) As String

#End Region

#Region "SQLReportParam Abstract Methods"

        Public MustOverride Function GetSQLReportParamList(ByVal ReportID As Integer) As IDataReader
        Public MustOverride Function GetSQLReportParam(ByVal ReportParamID As Integer) As IDataReader
        Public MustOverride Sub UpdateSQLReportParam(ByVal ReportParamID As Integer, ByVal ReportID As Integer, ByVal ParamName As String, ByVal ParamType As String, ByVal ParamValue As String, ByVal ParamSource As Integer)
        Public MustOverride Sub DeleteSQLReportParam(ByVal ReportParamID As Integer)

#End Region

#Region "SQLReportXSL Abstract Methods"

        Public MustOverride Function GetSQLReportXSLList(ByVal ReportID As Integer) As IDataReader
        Public MustOverride Function GetSQLReportXSL(ByVal ReportXSLID As Integer) As IDataReader
        Public MustOverride Sub UpdateSQLReportXSL(ByVal ReportXSLID As Integer, ByVal ReportID As Integer, ByVal XMLInput As String, ByVal XSLFile As String, ByVal OutputFile As String, ByVal DisplayResults As Boolean, ByVal SortOrder As Integer)
        Public MustOverride Sub DeleteSQLReportXSL(ByVal ReportXSLID As Integer)

#End Region

#Region "NB_Store_ProductRelated Abstract Methods"

        Public MustOverride Function GetNB_Store_ProductRelatedList(ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal Lang As String, ByVal RelatedType As Integer, ByVal GetAll As Boolean) As IDataReader
        Public MustOverride Function GetNB_Store_ProductRelated(ByVal RelatedID As Integer) As IDataReader
        Public MustOverride Function UpdateNB_Store_ProductRelated(ByVal RelatedID As Integer, ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal RelatedProductID As Integer, ByVal DiscountAmt As Decimal, ByVal DiscountPercent As Decimal, ByVal ProductQty As Integer, ByVal MaxQty As Integer, ByVal RelatedType As Integer, ByVal Disabled As Boolean, ByVal NotAvailable As Boolean, ByVal BiDirectional As Boolean) As Integer
        Public MustOverride Sub DeleteNB_Store_ProductRelated(ByVal RelatedID As Integer)
        Public MustOverride Sub NotAvailableProductRelated(ByVal ProductID As Integer, ByVal Flag As Boolean)
        Public MustOverride Sub DeleteNB_Store_ProductRelatedByProduct(ByVal ProductID As Integer)

#End Region

#Region "NB_Store_SearchWordHits Abstract Methods"

        Public MustOverride Sub UpdateSearchWord(ByVal PortalID As Integer, ByVal SearchWord As String, ByVal WordPosition As Integer)
        Public MustOverride Sub PurgeSearchWord(ByVal PortalID As Integer, ByVal PurgeBeforeDate As Date)
        Public MustOverride Sub ProcessSearchWords(ByVal PortalID As Integer)

#End Region


        Public MustOverride Sub ClearDownStore(ByVal PortalID As Integer)


    End Class

End Namespace