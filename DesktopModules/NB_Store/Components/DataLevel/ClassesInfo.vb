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
Imports System.Configuration
Imports System.Data

Namespace NEvoWeb.Modules.NB_Store

    Public Class NB_StoreUsersInfo

#Region "Private Members"
        Private _UserID As Integer
        Private _Username As String
        Private _FirstName As String
        Private _LastName As String
        Private _IsSuperUser As Boolean
        Private _AffiliateId As Integer
        Private _Email As String
        Private _DisplayName As String
        Private _UpdatePassword As Boolean

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal Value As Integer)
                _UserID = Value
            End Set
        End Property

        Public Property Username() As String
            Get
                Return _Username
            End Get
            Set(ByVal Value As String)
                _Username = Value
            End Set
        End Property

        Public Property FirstName() As String
            Get
                Return _FirstName
            End Get
            Set(ByVal Value As String)
                _FirstName = Value
            End Set
        End Property

        Public Property LastName() As String
            Get
                Return _LastName
            End Get
            Set(ByVal Value As String)
                _LastName = Value
            End Set
        End Property

        Public Property IsSuperUser() As Boolean
            Get
                Return _IsSuperUser
            End Get
            Set(ByVal Value As Boolean)
                _IsSuperUser = Value
            End Set
        End Property

        Public Property AffiliateId() As Integer
            Get
                Return _AffiliateId
            End Get
            Set(ByVal Value As Integer)
                _AffiliateId = Value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return _Email
            End Get
            Set(ByVal Value As String)
                _Email = Value
            End Set
        End Property

        Public Property DisplayName() As String
            Get
                Return _DisplayName
            End Get
            Set(ByVal Value As String)
                _DisplayName = Value
            End Set
        End Property

        Public Property UpdatePassword() As Boolean
            Get
                Return _UpdatePassword
            End Get
            Set(ByVal Value As Boolean)
                _UpdatePassword = Value
            End Set
        End Property


#End Region

    End Class

    Public Class ProductTaxInfo
        Inherits CartTaxInfo

#Region "Private Members"
        Private _ModelID As Integer
        Private _TaxPercent As String
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ModelID() As Integer
            Get
                Return _ModelID
            End Get
            Set(ByVal Value As Integer)
                _ModelID = Value
            End Set
        End Property

        Public Property TaxPercent() As String
            Get
                Return _TaxPercent
            End Get
            Set(ByVal Value As String)
                _TaxPercent = Value
            End Set
        End Property

#End Region

    End Class

    Public Class CartTaxInfo

#Region "Private Members"
        Private _CartID As String
        Private _TaxAmount As String
        Private _TotalNET As String
        Private _TotalGROSS As String
        Private _TaxOption As String
        Private _ShipTax As String
        Private _WhatIfTaxAmount As String
        Private _WhatIfShipTax As String
        Private _WhatIfTotalNET As String
        Private _WhatIfTotalGROSS As String
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property CartID() As String
            Get
                Return _CartID
            End Get
            Set(ByVal Value As String)
                _CartID = Value
            End Set
        End Property

        Public Property TaxAmount() As String
            Get
                Return _TaxAmount
            End Get
            Set(ByVal Value As String)
                _TaxAmount = Value
            End Set
        End Property

        Public Property TotalNET() As String
            Get
                Return _TotalNET
            End Get
            Set(ByVal Value As String)
                _TotalNET = Value
            End Set
        End Property

        Public Property TotalGROSS() As String
            Get
                Return _TotalGROSS
            End Get
            Set(ByVal Value As String)
                _TotalGROSS = Value
            End Set
        End Property

        Public Property TaxOption() As String
            Get
                Return _TaxOption
            End Get
            Set(ByVal Value As String)
                _TaxOption = Value
            End Set
        End Property

        Public Property ShipTax() As String
            Get
                Return _ShipTax
            End Get
            Set(ByVal Value As String)
                _ShipTax = Value
            End Set
        End Property

#End Region

    End Class

    Public Class ProductStockLevels

#Region "Private Members"
        Private _Percent As Integer = 0
        Private _PercentInProgess As Integer = 0
        Private _PercentSold As Integer = 0
        Private _PercentActual As Integer = 0
        Private _Qty As Integer = 0
        Private _MaxQty As Integer = 0
        Private _CartQty As Integer = 0
        Private _StockOn As Boolean = False
#End Region

        Public Sub New()
            _Percent = 0
            _PercentInProgess = 0
            _PercentSold = 0
            _PercentActual = 0
            _Qty = 0
            _MaxQty = 0
            _CartQty = 0
            _StockOn = False
        End Sub

#Region "Cart Public properties"

        Public Property Percent() As Integer
            Get
                Return _Percent
            End Get
            Set(ByVal Value As Integer)
                _Percent = Value
            End Set
        End Property
        Public Property PercentInProgess() As Integer
            Get
                Return _PercentInProgess
            End Get
            Set(ByVal Value As Integer)
                _PercentInProgess = Value
            End Set
        End Property
        Public Property PercentSold() As Integer
            Get
                Return _PercentSold
            End Get
            Set(ByVal Value As Integer)
                _PercentSold = Value
            End Set
        End Property
        Public Property PercentActual() As Integer
            Get
                Return _PercentActual
            End Get
            Set(ByVal Value As Integer)
                _PercentActual = Value
            End Set
        End Property

        Public Property Qty() As Integer
            Get
                Return _Qty
            End Get
            Set(ByVal Value As Integer)
                _Qty = Value
            End Set
        End Property

        Public Property MaxQty() As Integer
            Get
                Return _MaxQty
            End Get
            Set(ByVal Value As Integer)
                _MaxQty = Value
            End Set
        End Property

        Public Property CartQty() As Integer
            Get
                Return _CartQty
            End Get
            Set(ByVal Value As Integer)
                _CartQty = Value
            End Set
        End Property

        Public Property StockOn() As Boolean
            Get
                Return _StockOn
            End Get
            Set(ByVal Value As Boolean)
                _StockOn = Value
            End Set
        End Property

#End Region

    End Class

    Public Class CartTotals

#Region "Private Members"
        Private _TaxAmt As Decimal = 0
        Private _TaxAppliedAmt As Decimal = 0
        Private _ShipAmt As Decimal = 0
        Private _DiscountAmt As Decimal = 0
        Private _TotalAmt As Decimal = 0
        Private _OrderTotal As Decimal = 0
        Private _Qty As Integer = 0
        Private _Balance As Decimal = 0
#End Region

        Public Sub New()
            _TaxAmt = 0
            _TaxAppliedAmt = 0
            _ShipAmt = 0
            _DiscountAmt = 0
            _TotalAmt = 0
            _OrderTotal = 0
            _Qty = 0
            _Balance = 0
        End Sub

#Region "Cart Public properties"

        Public Property TotalAmt() As Decimal
            Get
                Return _TotalAmt
            End Get
            Set(ByVal Value As Decimal)
                _TotalAmt = Value
            End Set
        End Property

        Public Property DiscountAmt() As Decimal
            Get
                Return _DiscountAmt
            End Get
            Set(ByVal Value As Decimal)
                _DiscountAmt = Value
            End Set
        End Property

        Public Property TaxAmt() As Decimal
            Get
                Return _TaxAmt
            End Get
            Set(ByVal Value As Decimal)
                _TaxAmt = Value
            End Set
        End Property

        Public Property TaxAppliedAmt() As Decimal
            Get
                Return _TaxAppliedAmt
            End Get
            Set(ByVal Value As Decimal)
                _TaxAppliedAmt = Value
            End Set
        End Property

        Public Property ShipAmt() As Decimal
            Get
                Return _ShipAmt
            End Get
            Set(ByVal Value As Decimal)
                _ShipAmt = Value
            End Set
        End Property

        Public Property OrderTotal() As Decimal
            Get
                Return _OrderTotal
            End Get
            Set(ByVal Value As Decimal)
                _OrderTotal = Value
            End Set
        End Property

        Public Property Qty() As Integer
            Get
                Return _Qty
            End Get
            Set(ByVal Value As Integer)
                _Qty = Value
            End Set
        End Property

        Public Property Balance() As Decimal
            Get
                Return _Balance
            End Get
            Set(ByVal Value As Decimal)
                _Balance = Value
            End Set
        End Property

#End Region

    End Class

    Public Class OptCodeInfo

#Region "Private Members"
        Private _UnitCost As Decimal
        Private _Discount As Decimal
        Private _ItemDesc As String
        Private _OptCode As String
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property UnitCost() As Decimal
            Get
                Return _UnitCost
            End Get
            Set(ByVal Value As Decimal)
                _UnitCost = Value
            End Set
        End Property

        Public Property Discount() As Decimal
            Get
                Return _Discount
            End Get
            Set(ByVal Value As Decimal)
                _Discount = Value
            End Set
        End Property

        Public Property ItemDesc() As String
            Get
                Return _ItemDesc
            End Get
            Set(ByVal Value As String)
                _ItemDesc = Value
            End Set
        End Property

        Public Property OptCode() As String
            Get
                Return _OptCode
            End Get
            Set(ByVal Value As String)
                _OptCode = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_AddressInfo

#Region "Private Members"
        Private _AddressID As Integer
        Private _PortalID As Integer
        Private _UserID As Integer
        Private _AddressDescription As String
        Private _AddressName As String
        Private _AddressName2 As String
        Private _Address1 As String
        Private _Address2 As String
        Private _City As String
        Private _RegionCode As String
        Private _CountryCode As String
        Private _PostalCode As String
        Private _Phone1 As String
        Private _Phone2 As String
        Private _PrimaryAddress As Boolean
        Private _CreatedByUser As String
        Private _CreatedDate As Date
        Private _OrderID As Integer
        Private _CompanyName As String
        Private _Extra1 As String
        Private _Extra2 As String
        Private _Extra3 As String
        Private _Extra4 As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property AddressID() As Integer
            Get
                Return _AddressID
            End Get
            Set(ByVal Value As Integer)
                _AddressID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal Value As Integer)
                _UserID = Value
            End Set
        End Property

        Public Property AddressDescription() As String
            Get
                Return _AddressDescription
            End Get
            Set(ByVal Value As String)
                _AddressDescription = Value
            End Set
        End Property

        Public Property AddressName() As String
            Get
                Return _AddressName
            End Get
            Set(ByVal Value As String)
                _AddressName = Value
            End Set
        End Property

        Public Property AddressName2() As String
            Get
                Return _AddressName2
            End Get
            Set(ByVal Value As String)
                _AddressName2 = Value
            End Set
        End Property

        Public Property Address1() As String
            Get
                Return _Address1
            End Get
            Set(ByVal Value As String)
                _Address1 = Value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Return _Address2
            End Get
            Set(ByVal Value As String)
                _Address2 = Value
            End Set
        End Property

        Public Property City() As String
            Get
                Return _City
            End Get
            Set(ByVal Value As String)
                _City = Value
            End Set
        End Property

        Public Property RegionCode() As String
            Get
                Return _RegionCode
            End Get
            Set(ByVal Value As String)
                _RegionCode = Value
            End Set
        End Property

        Public Property CountryCode() As String
            Get
                Return _CountryCode
            End Get
            Set(ByVal Value As String)
                _CountryCode = Value
            End Set
        End Property

        Public Property PostalCode() As String
            Get
                Return _PostalCode
            End Get
            Set(ByVal Value As String)
                _PostalCode = Value
            End Set
        End Property

        Public Property Phone1() As String
            Get
                Return _Phone1
            End Get
            Set(ByVal Value As String)
                _Phone1 = Value
            End Set
        End Property

        Public Property Phone2() As String
            Get
                Return _Phone2
            End Get
            Set(ByVal Value As String)
                _Phone2 = Value
            End Set
        End Property

        Public Property PrimaryAddress() As Boolean
            Get
                Return _PrimaryAddress
            End Get
            Set(ByVal Value As Boolean)
                _PrimaryAddress = Value
            End Set
        End Property

        Public Property CreatedByUser() As String
            Get
                Return _CreatedByUser
            End Get
            Set(ByVal Value As String)
                _CreatedByUser = Value
            End Set
        End Property

        Public Property CreatedDate() As Date
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As Date)
                _CreatedDate = Value
            End Set
        End Property

        Public Property OrderID() As Integer
            Get
                Return _OrderID
            End Get
            Set(ByVal Value As Integer)
                _OrderID = Value
            End Set
        End Property

        Public Property CompanyName() As String
            Get
                Return _CompanyName
            End Get
            Set(ByVal Value As String)
                _CompanyName = Value
            End Set
        End Property

        Public Property Extra1() As String
            Get
                Return _Extra1
            End Get
            Set(ByVal Value As String)
                _Extra1 = Value
            End Set
        End Property

        Public Property Extra2() As String
            Get
                Return _Extra2
            End Get
            Set(ByVal Value As String)
                _Extra2 = Value
            End Set
        End Property

        Public Property Extra3() As String
            Get
                Return _Extra3
            End Get
            Set(ByVal Value As String)
                _Extra3 = Value
            End Set
        End Property

        Public Property Extra4() As String
            Get
                Return _Extra4
            End Get
            Set(ByVal Value As String)
                _Extra4 = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_CartInfo

#Region "Private Members"
        Private _CartID As String
        Private _PortalID As Integer
        Private _UserID As Integer
        Private _DateCreated As Date
        Private _OrderID As Integer
        Private _VATNumber As String
        Private _PromoCode As String
        Private _CountryCode As String
        Private _ShipType As String
        Private _BankTransID As Integer
        Private _BankHtmlRedirect As String
        Private _ShipMethodID As Integer
        Private _CartDiscount As Decimal
        Private _XMLInfo As String
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property CartID() As String
            Get
                Return _CartID
            End Get
            Set(ByVal Value As String)
                _CartID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal Value As Integer)
                _UserID = Value
            End Set
        End Property

        Public Property DateCreated() As Date
            Get
                Return _DateCreated
            End Get
            Set(ByVal Value As Date)
                _DateCreated = Value
            End Set
        End Property

        Public Property OrderID() As Integer
            Get
                Return _OrderID
            End Get
            Set(ByVal Value As Integer)
                _OrderID = Value
            End Set
        End Property

        Public Property VATNumber() As String
            Get
                Return _VATNumber
            End Get
            Set(ByVal Value As String)
                _VATNumber = Value
            End Set
        End Property

        Public Property PromoCode() As String
            Get
                Return _PromoCode
            End Get
            Set(ByVal Value As String)
                _PromoCode = Value
            End Set
        End Property

        Public Property CountryCode() As String
            Get
                Return _CountryCode
            End Get
            Set(ByVal Value As String)
                _CountryCode = Value
            End Set
        End Property

        Public Property ShipType() As String
            Get
                Return _ShipType
            End Get
            Set(ByVal Value As String)
                _ShipType = Value
            End Set
        End Property

        Public Property BankTransID() As Integer
            Get
                Return _BankTransID
            End Get
            Set(ByVal Value As Integer)
                _BankTransID = Value
            End Set
        End Property

        Public Property BankHtmlRedirect() As String
            Get
                Return _BankHtmlRedirect
            End Get
            Set(ByVal Value As String)
                _BankHtmlRedirect = Value
            End Set
        End Property

        Public Property ShipMethodID() As Integer
            Get
                Return _ShipMethodID
            End Get
            Set(ByVal Value As Integer)
                _ShipMethodID = Value
            End Set
        End Property

        Public Property CartDiscount() As Decimal
            Get
                'make sure we don't pass null
                If _CartDiscount = Null.NullDecimal Then
                    _CartDiscount = 0
                End If
                Return _CartDiscount
            End Get
            Set(ByVal Value As Decimal)
                _CartDiscount = Value
            End Set
        End Property

        Public Property XMLInfo() As String
            Get
                Return _XMLInfo
            End Get
            Set(ByVal Value As String)
                _XMLInfo = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_CartItemsInfo

#Region "Private Members"
        Private _ItemID As Integer
        Private _CartID As String
        Private _Quantity As Integer
        Private _DateCreated As Date
        Private _UnitCost As Decimal
        Private _ModelID As Integer
        Private _OptCode As String
        Private _ItemDesc As String
        Private _Discount As Decimal
        Private _Tax As Decimal
        Private _ProductURL As String
        Private _XMLInfo As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ItemID() As Integer
            Get
                Return _ItemID
            End Get
            Set(ByVal Value As Integer)
                _ItemID = Value
            End Set
        End Property

        Public Property CartID() As String
            Get
                Return _CartID
            End Get
            Set(ByVal Value As String)
                _CartID = Value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return _Quantity
            End Get
            Set(ByVal Value As Integer)
                _Quantity = Value
            End Set
        End Property

        Public Property DateCreated() As Date
            Get
                Return _DateCreated
            End Get
            Set(ByVal Value As Date)
                _DateCreated = Value
            End Set
        End Property

        Public Property UnitCost() As Decimal
            Get
                Return _UnitCost
            End Get
            Set(ByVal Value As Decimal)
                _UnitCost = Value
            End Set
        End Property

        Public Property ModelID() As Integer
            Get
                Return _ModelID
            End Get
            Set(ByVal Value As Integer)
                _ModelID = Value
            End Set
        End Property

        Public Property OptCode() As String
            Get
                Return _OptCode
            End Get
            Set(ByVal Value As String)
                _OptCode = Value
            End Set
        End Property

        Public Property ItemDesc() As String
            Get
                Return _ItemDesc
            End Get
            Set(ByVal Value As String)
                _ItemDesc = Value
            End Set
        End Property

        Public Property Discount() As Decimal
            Get
                Return _Discount
            End Get
            Set(ByVal Value As Decimal)
                _Discount = Value
            End Set
        End Property

        Public Property Tax() As Decimal
            Get
                Return _Tax
            End Get
            Set(ByVal Value As Decimal)
                _Tax = Value
            End Set
        End Property

        Public Property ProductURL() As String
            Get
                Return _ProductURL
            End Get
            Set(ByVal Value As String)
                _ProductURL = Value
            End Set
        End Property

        Public Property XMLInfo() As String
            Get
                Return _XMLInfo
            End Get
            Set(ByVal Value As String)
                _XMLInfo = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_CartListInfo

#Region "Private Members"
        Private _ItemID As Integer
        Private _ModelID As Integer
        Private _OptCode As String
        Private _Quantity As Integer
        Private _UnitCost As Decimal
        Private _ItemDesc As String
        Private _SubTotal As Decimal
        Private _ProductURL As String
        Private _Discount As Decimal

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ItemID() As Integer
            Get
                Return _ItemID
            End Get
            Set(ByVal Value As Integer)
                _ItemID = Value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return _Quantity
            End Get
            Set(ByVal Value As Integer)
                _Quantity = Value
            End Set
        End Property

        Public Property UnitCost() As Decimal
            Get
                Return _UnitCost
            End Get
            Set(ByVal Value As Decimal)
                _UnitCost = Value
            End Set
        End Property

        Public Property ModelID() As Integer
            Get
                Return _ModelID
            End Get
            Set(ByVal Value As Integer)
                _ModelID = Value
            End Set
        End Property

        Public Property OptCode() As String
            Get
                Return _OptCode
            End Get
            Set(ByVal Value As String)
                _OptCode = Value
            End Set
        End Property

        Public Property ItemDesc() As String
            Get
                Return _ItemDesc
            End Get
            Set(ByVal Value As String)
                _ItemDesc = Value
            End Set
        End Property

        Public Property SubTotal() As Decimal
            Get
                Return _SubTotal
            End Get
            Set(ByVal Value As Decimal)
                _SubTotal = Value
            End Set
        End Property

        Public Property ProductURL() As String
            Get
                Return _ProductURL
            End Get
            Set(ByVal Value As String)
                _ProductURL = Value
            End Set
        End Property

        Public Property Discount() As Decimal
            Get
                Return _Discount
            End Get
            Set(ByVal Value As Decimal)
                _Discount = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_CategoriesInfo

#Region "Private Members"
        Private _CategoryID As Integer
        Private _PortalID As Integer
        Private _Archived As Boolean
        Private _Hide As Boolean
        Private _CreatedByUser As String
        Private _CreatedDate As Date
        Private _ParentCategoryID As Integer
        Private _ListOrder As Integer
        Private _Lang As String
        Private _CategoryName As String
        Private _ParentName As String
        Private _CategoryDesc As String
        Private _Message As String
        Private _ProductCount As Integer
        Private _ProductTemplate As String
        Private _ListItemTemplate As String
        Private _ListAltItemTemplate As String
        Private _ImageURL As String
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property CategoryID() As Integer
            Get
                Return _CategoryID
            End Get
            Set(ByVal Value As Integer)
                _CategoryID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property Archived() As Boolean
            Get
                Return _Archived
            End Get
            Set(ByVal Value As Boolean)
                _Archived = Value
            End Set
        End Property

        Public Property Hide() As Boolean
            Get
                Return _Hide
            End Get
            Set(ByVal Value As Boolean)
                _Hide = Value
            End Set
        End Property

        Public Property CreatedByUser() As String
            Get
                Return _CreatedByUser
            End Get
            Set(ByVal Value As String)
                _CreatedByUser = Value
            End Set
        End Property

        Public Property CreatedDate() As Date
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As Date)
                _CreatedDate = Value
            End Set
        End Property

        Public Property ParentCategoryID() As Integer
            Get
                Return _ParentCategoryID
            End Get
            Set(ByVal Value As Integer)
                _ParentCategoryID = Value
            End Set
        End Property

        Public Property ListOrder() As Integer
            Get
                Return _ListOrder
            End Get
            Set(ByVal Value As Integer)
                _ListOrder = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property CategoryName() As String
            Get
                Return _CategoryName
            End Get
            Set(ByVal Value As String)
                _CategoryName = Value
            End Set
        End Property

        Public Property ParentName() As String
            Get
                Return _ParentName
            End Get
            Set(ByVal Value As String)
                _ParentName = Value
            End Set
        End Property

        Public Property CategoryDesc() As String
            Get
                Return _CategoryDesc
            End Get
            Set(ByVal Value As String)
                _CategoryDesc = Value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return _Message
            End Get
            Set(ByVal Value As String)
                _Message = Value
            End Set
        End Property

        Public Property ProductCount() As Integer
            Get
                Return _ProductCount
            End Get
            Set(ByVal Value As Integer)
                _ProductCount = Value
            End Set
        End Property

        Public Property ProductTemplate() As String
            Get
                Return _ProductTemplate
            End Get
            Set(ByVal Value As String)
                _ProductTemplate = Value
            End Set
        End Property

        Public Property ListItemTemplate() As String
            Get
                Return _ListItemTemplate
            End Get
            Set(ByVal Value As String)
                _ListItemTemplate = Value
            End Set
        End Property

        Public Property ListAltItemTemplate() As String
            Get
                Return _ListAltItemTemplate
            End Get
            Set(ByVal Value As String)
                _ListAltItemTemplate = Value
            End Set
        End Property

        Public Property ImageURL() As String
            Get
                Return _ImageURL
            End Get
            Set(ByVal Value As String)
                _ImageURL = Value
            End Set
        End Property

#End Region

    End Class


    Public Class NB_Store_ModelInfo

#Region "Private Members"
        Private _ModelID As Integer
        Private _ProductID As Integer
        Private _ListOrder As Integer
        Private _UnitCost As Decimal
        Private _Barcode As String
        Private _ModelRef As String
        Private _Lang As String
        Private _ModelName As String
        Private _QtyRemaining As Integer
        Private _QtyTrans As Integer
        Private _QtyTransDate As Date
        Private _ProductName As String
        Private _PortalID As Integer
        Private _Weight As Decimal
        Private _Height As Decimal
        Private _Length As Decimal
        Private _Width As Decimal
        Private _Deleted As Boolean
        Private _QtyStockSet As Integer
        Private _DealerCost As Decimal
        Private _PurchaseCost As Decimal
        Private _XMLData As String
        Private _Extra As String
        Private _DealerOnly As Boolean
        Private _Allow As Integer
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ModelID() As Integer
            Get
                Return _ModelID
            End Get
            Set(ByVal Value As Integer)
                _ModelID = Value
            End Set
        End Property

        Public Property ProductID() As Integer
            Get
                Return _ProductID
            End Get
            Set(ByVal Value As Integer)
                _ProductID = Value
            End Set
        End Property

        Public Property ListOrder() As Integer
            Get
                Return _ListOrder
            End Get
            Set(ByVal Value As Integer)
                _ListOrder = Value
            End Set
        End Property

        Public Property UnitCost() As Decimal
            Get
                Return _UnitCost
            End Get
            Set(ByVal Value As Decimal)
                _UnitCost = Value
            End Set
        End Property

        Public Property Barcode() As String
            Get
                Return _Barcode
            End Get
            Set(ByVal Value As String)
                _Barcode = Value
            End Set
        End Property

        Public Property ModelRef() As String
            Get
                Return _ModelRef
            End Get
            Set(ByVal Value As String)
                _ModelRef = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property ModelName() As String
            Get
                Return _ModelName
            End Get
            Set(ByVal Value As String)
                _ModelName = Value
            End Set
        End Property

        Public Property QtyRemaining() As Integer
            Get
                Return _QtyRemaining
            End Get
            Set(ByVal Value As Integer)
                _QtyRemaining = Value
            End Set
        End Property

        Public Property QtyTrans() As Integer
            Get
                Return _QtyTrans
            End Get
            Set(ByVal Value As Integer)
                _QtyTrans = Value
            End Set
        End Property

        Public Property QtyTransDate() As Date
            Get
                Return _QtyTransDate
            End Get
            Set(ByVal Value As Date)
                _QtyTransDate = Value
            End Set
        End Property

        Public Property ProductName() As String
            Get
                Return _ProductName
            End Get
            Set(ByVal Value As String)
                _ProductName = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property Weight() As Decimal
            Get
                Return _Weight
            End Get
            Set(ByVal Value As Decimal)
                _Weight = Value
            End Set
        End Property
        Public Property Height() As Decimal
            Get
                Return _Height
            End Get
            Set(ByVal Value As Decimal)
                _Height = Value
            End Set
        End Property
        Public Property Length() As Decimal
            Get
                Return _Length
            End Get
            Set(ByVal Value As Decimal)
                _Length = Value
            End Set
        End Property
        Public Property Width() As Decimal
            Get
                Return _Width
            End Get
            Set(ByVal Value As Decimal)
                _Width = Value
            End Set
        End Property

        Public Property Deleted() As Boolean
            Get
                Return _Deleted
            End Get
            Set(ByVal Value As Boolean)
                _Deleted = Value
            End Set
        End Property

        Public Property QtyStockSet() As Integer
            Get
                Return _QtyStockSet
            End Get
            Set(ByVal Value As Integer)
                _QtyStockSet = Value
            End Set
        End Property

        Public Property DealerCost() As Decimal
            Get
                Return _DealerCost
            End Get
            Set(ByVal Value As Decimal)
                _DealerCost = Value
            End Set
        End Property

        Public Property PurchaseCost() As Decimal
            Get
                Return _PurchaseCost
            End Get
            Set(ByVal Value As Decimal)
                _PurchaseCost = Value
            End Set
        End Property

        Public Property XMLData() As String
            Get
                Return _XMLData
            End Get
            Set(ByVal Value As String)
                _XMLData = Value
            End Set
        End Property

        Public Property Extra() As String
            Get
                Return _Extra
            End Get
            Set(ByVal Value As String)
                _Extra = Value
            End Set
        End Property

        Public Property DealerOnly() As Boolean
            Get
                Return _DealerOnly
            End Get
            Set(ByVal Value As Boolean)
                _DealerOnly = Value
            End Set
        End Property

        Public Property Allow() As Integer
            Get
                Return _Allow
            End Get
            Set(ByVal Value As Integer)
                _Allow = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_OptionInfo

#Region "Private Members"
        Private _OptionID As Integer
        Private _ProductID As Integer
        Private _ListOrder As Integer
        Private _Lang As String
        Private _OptionDesc As String
        Private _Attributes As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property OptionID() As Integer
            Get
                Return _OptionID
            End Get
            Set(ByVal Value As Integer)
                _OptionID = Value
            End Set
        End Property

        Public Property ProductID() As Integer
            Get
                Return _ProductID
            End Get
            Set(ByVal Value As Integer)
                _ProductID = Value
            End Set
        End Property

        Public Property ListOrder() As Integer
            Get
                Return _ListOrder
            End Get
            Set(ByVal Value As Integer)
                _ListOrder = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property OptionDesc() As String
            Get
                Return _OptionDesc
            End Get
            Set(ByVal Value As String)
                _OptionDesc = Value
            End Set
        End Property

        Public Property Attributes() As String
            Get
                Return _Attributes
            End Get
            Set(ByVal Value As String)
                _Attributes = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_OptionValueInfo

#Region "Private Members"
        Private _OptionValueID As Integer
        Private _OptionID As Integer
        Private _AddedCost As Decimal
        Private _ListOrder As Integer
        Private _Lang As String
        Private _OptionValueDesc As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property OptionValueID() As Integer
            Get
                Return _OptionValueID
            End Get
            Set(ByVal Value As Integer)
                _OptionValueID = Value
            End Set
        End Property

        Public Property OptionID() As Integer
            Get
                Return _OptionID
            End Get
            Set(ByVal Value As Integer)
                _OptionID = Value
            End Set
        End Property

        Public Property AddedCost() As Decimal
            Get
                Return _AddedCost
            End Get
            Set(ByVal Value As Decimal)
                _AddedCost = Value
            End Set
        End Property

        Public Property ListOrder() As Integer
            Get
                Return _ListOrder
            End Get
            Set(ByVal Value As Integer)
                _ListOrder = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property OptionValueDesc() As String
            Get
                Return _OptionValueDesc
            End Get
            Set(ByVal Value As String)
                _OptionValueDesc = Value
            End Set
        End Property



#End Region

    End Class

    Public Class NB_Store_OrderDetailsInfo

#Region "Private Members"
        Private _OrderDetailID As Integer
        Private _OrderID As Integer
        Private _Quantity As Integer
        Private _UnitCost As Decimal
        Private _ModelID As Integer
        Private _OptCode As String
        Private _ItemDesc As String
        Private _Discount As Decimal
        Private _Tax As Decimal
        Private _ProductURL As String
        Private _PurchaseCost As Decimal
        Private _CartXMLInfo As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property OrderDetailID() As Integer
            Get
                Return _OrderDetailID
            End Get
            Set(ByVal Value As Integer)
                _OrderDetailID = Value
            End Set
        End Property

        Public Property OrderID() As Integer
            Get
                Return _OrderID
            End Get
            Set(ByVal Value As Integer)
                _OrderID = Value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return _Quantity
            End Get
            Set(ByVal Value As Integer)
                _Quantity = Value
            End Set
        End Property

        Public Property UnitCost() As Decimal
            Get
                Return _UnitCost
            End Get
            Set(ByVal Value As Decimal)
                _UnitCost = Value
            End Set
        End Property

        Public Property ModelID() As Integer
            Get
                Return _ModelID
            End Get
            Set(ByVal Value As Integer)
                _ModelID = Value
            End Set
        End Property

        Public Property OptCode() As String
            Get
                Return _OptCode
            End Get
            Set(ByVal Value As String)
                _OptCode = Value
            End Set
        End Property

        Public Property ItemDesc() As String
            Get
                Return _ItemDesc
            End Get
            Set(ByVal Value As String)
                _ItemDesc = Value
            End Set
        End Property

        Public Property Discount() As Decimal
            Get
                Return _Discount
            End Get
            Set(ByVal Value As Decimal)
                _Discount = Value
            End Set
        End Property

        Public Property Tax() As Decimal
            Get
                Return _Tax
            End Get
            Set(ByVal Value As Decimal)
                _Tax = Value
            End Set
        End Property

        Public Property ProductURL() As String
            Get
                Return _ProductURL
            End Get
            Set(ByVal Value As String)
                _ProductURL = Value
            End Set
        End Property

        Public ReadOnly Property Total() As Decimal
            Get
                Return (_Quantity * _UnitCost)
            End Get
        End Property

        Public ReadOnly Property TotalDiscount() As Decimal
            Get
                Return (_Quantity * _Discount)
            End Get
        End Property

        Public Property PurchaseCost() As Decimal
            Get
                Return _PurchaseCost
            End Get
            Set(ByVal Value As Decimal)
                _PurchaseCost = Value
            End Set
        End Property

        Public Property CartXMLInfo() As String
            Get
                Return _CartXMLInfo
            End Get
            Set(ByVal Value As String)
                _CartXMLInfo = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_OrdersInfo

#Region "Private Members"
        Private _OrderID As Integer
        Private _UserID As Integer
        Private _PortalID As Integer
        Private _OrderNumber As String
        Private _OrderDate As Date
        Private _ShipDate As Date
        Private _ShippingAddressID As Integer
        Private _BillingAddressID As Integer
        Private _AppliedTax As Decimal
        Private _ShippingCost As Decimal
        Private _OrderIsPlaced As Boolean
        Private _OrderStatusID As Integer
        Private _PayType As String
        Private _CalculatedTax As Decimal
        Private _NoteMsg As String
        Private _VATNumber As String
        Private _Discount As Decimal
        Private _PromoCode As String
        Private _Total As Decimal
        Private _Email As String
        Private _BankAuthCode As String
        Private _ShipMethodID As Integer
        Private _TrackingCode As String
        Private _Stg2FormXML As String
        Private _Stg3FormXML As String
        Private _AlreadyPaid As Decimal
        Private _OrderGUID As String
        Private _ElapsedDate As Date
        Private _GatewayProvider As String
        Private _CartXMLInfo As String

#End Region

#Region "Constructors"
        Public Sub New()
            'assign here so we know we've got a fresh order.
            _OrderNumber = ""
            _OrderIsPlaced = False
        End Sub
#End Region

#Region "Public Properties"
        Public Property OrderID() As Integer
            Get
                Return _OrderID
            End Get
            Set(ByVal Value As Integer)
                _OrderID = Value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal Value As Integer)
                _UserID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property OrderNumber() As String
            Get
                Return _OrderNumber
            End Get
            Set(ByVal Value As String)
                _OrderNumber = Value
            End Set
        End Property

        Public ReadOnly Property ShortOrderNumber() As String
            Get
                If _OrderNumber.Length > 14 Then
                    Return _OrderNumber.Substring(InStr(_OrderNumber, "-") + 6, 5)
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Property OrderDate() As Date
            Get
                Return _OrderDate
            End Get
            Set(ByVal Value As Date)
                _OrderDate = Value
            End Set
        End Property

        Public Property ShipDate() As Date
            Get
                Return _ShipDate
            End Get
            Set(ByVal Value As Date)
                _ShipDate = Value
            End Set
        End Property

        Public Property ShippingAddressID() As Integer
            Get
                Return _ShippingAddressID
            End Get
            Set(ByVal Value As Integer)
                _ShippingAddressID = Value
            End Set
        End Property

        Public Property BillingAddressID() As Integer
            Get
                Return _BillingAddressID
            End Get
            Set(ByVal Value As Integer)
                _BillingAddressID = Value
            End Set
        End Property

        Public Property AppliedTax() As Decimal
            Get
                Return _AppliedTax
            End Get
            Set(ByVal Value As Decimal)
                _AppliedTax = Value
            End Set
        End Property

        Public Property ShippingCost() As Decimal
            Get
                Return _ShippingCost
            End Get
            Set(ByVal Value As Decimal)
                _ShippingCost = Value
            End Set
        End Property

        Public Property OrderIsPlaced() As Boolean
            Get
                Return _OrderIsPlaced
            End Get
            Set(ByVal Value As Boolean)
                _OrderIsPlaced = Value
            End Set
        End Property

        Public Property OrderStatusID() As Integer
            Get
                Return _OrderStatusID
            End Get
            Set(ByVal Value As Integer)
                _OrderStatusID = Value
            End Set
        End Property

        Public Property PayType() As String
            Get
                Return _PayType
            End Get
            Set(ByVal Value As String)
                _PayType = Value
            End Set
        End Property

        Public Property CalculatedTax() As Decimal
            Get
                Return _CalculatedTax
            End Get
            Set(ByVal Value As Decimal)
                _CalculatedTax = Value
            End Set
        End Property

        Public Property NoteMsg() As String
            Get
                Return _NoteMsg
            End Get
            Set(ByVal Value As String)
                _NoteMsg = Value
            End Set
        End Property

        Public Property VATNumber() As String
            Get
                Return _VATNumber
            End Get
            Set(ByVal Value As String)
                _VATNumber = Value
            End Set
        End Property

        Public Property Discount() As Decimal
            Get
                Return _Discount
            End Get
            Set(ByVal Value As Decimal)
                _Discount = Value
            End Set
        End Property

        Public Property PromoCode() As String
            Get
                Return _PromoCode
            End Get
            Set(ByVal Value As String)
                _PromoCode = Value
            End Set
        End Property

        Public Property Total() As Decimal
            Get
                Return _Total
            End Get
            Set(ByVal Value As Decimal)
                _Total = Value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return _Email
            End Get
            Set(ByVal Value As String)
                _Email = Value
            End Set
        End Property

        Public ReadOnly Property CartTotal() As Decimal
            Get
                Return _Total - (_AppliedTax + _ShippingCost)
            End Get
        End Property

        Public Property BankAuthCode() As String
            Get
                Return _BankAuthCode
            End Get
            Set(ByVal Value As String)
                _BankAuthCode = Value
            End Set
        End Property

        Public Property ShipMethodID() As Integer
            Get
                Return _ShipMethodID
            End Get
            Set(ByVal Value As Integer)
                _ShipMethodID = Value
            End Set
        End Property

        Public Property TrackingCode() As String
            Get
                Return _TrackingCode
            End Get
            Set(ByVal Value As String)
                _TrackingCode = Value
            End Set
        End Property

        Public Property Stg2FormXML() As String
            Get
                Return _Stg2FormXML
            End Get
            Set(ByVal Value As String)
                _Stg2FormXML = Value
            End Set
        End Property

        Public Property Stg3FormXML() As String
            Get
                Return _Stg3FormXML
            End Get
            Set(ByVal Value As String)
                _Stg3FormXML = Value
            End Set
        End Property

        Public Property AlreadyPaid() As Decimal
            Get
                Return _AlreadyPaid
            End Get
            Set(ByVal Value As Decimal)
                _AlreadyPaid = Value
            End Set
        End Property

        Public ReadOnly Property BalanceOutstanding() As Decimal
            Get
                Try
                    Return _Total - _AlreadyPaid
                Catch ex As Exception
                    Return _Total
                End Try
            End Get
        End Property

        Public Property OrderGUID() As String
            Get
                Return _OrderGUID
            End Get
            Set(ByVal Value As String)
                _OrderGUID = Value
            End Set
        End Property

        Public Property ElapsedDate() As Date
            Get
                Return _ElapsedDate
            End Get
            Set(ByVal value As Date)
                _ElapsedDate = value
            End Set
        End Property

        Public Property GatewayProvider() As String
            Get
                Return _GatewayProvider
            End Get
            Set(ByVal Value As String)
                _GatewayProvider = Value
            End Set
        End Property

        Public Property CartXMLInfo() As String
            Get
                Return _CartXMLInfo
            End Get
            Set(ByVal Value As String)
                _CartXMLInfo = Value
            End Set
        End Property

        Public ReadOnly Property Tax() As Decimal
            Get
                If _AppliedTax = 0 Then
                    Return _CalculatedTax
                Else
                    Return _AppliedTax
                End If
            End Get
        End Property


#End Region

    End Class

    Public Class NB_Store_OrderStatusInfo

#Region "Private Members"
        Private _OrderStatusID As Integer
        Private _Lang As String
        Private _OrderStatusText As String
        Private _ListOrder As Integer

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property OrderStatusID() As Integer
            Get
                Return _OrderStatusID
            End Get
            Set(ByVal Value As Integer)
                _OrderStatusID = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property OrderStatusText() As String
            Get
                Return _OrderStatusText
            End Get
            Set(ByVal Value As String)
                _OrderStatusText = Value
            End Set
        End Property

        Public Property ListOrder() As Integer
            Get
                Return _ListOrder
            End Get
            Set(ByVal Value As Integer)
                _ListOrder = Value
            End Set
        End Property


#End Region
    End Class

    Public Class NB_Store_ProductImageInfo

#Region "Private Members"
        Private _ImageID As Integer
        Private _ProductID As Integer
        Private _ImagePath As String
        Private _ListOrder As Integer
        Private _Hidden As Boolean
        Private _Lang As String
        Private _ImageDesc As String
        Private _ImageURL As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ImageID() As Integer
            Get
                Return _ImageID
            End Get
            Set(ByVal Value As Integer)
                _ImageID = Value
            End Set
        End Property

        Public Property ProductID() As Integer
            Get
                Return _ProductID
            End Get
            Set(ByVal Value As Integer)
                _ProductID = Value
            End Set
        End Property

        Public Property ImagePath() As String
            Get
                Return _ImagePath
            End Get
            Set(ByVal Value As String)
                _ImagePath = Value
            End Set
        End Property

        Public Property ListOrder() As Integer
            Get
                Return _ListOrder
            End Get
            Set(ByVal Value As Integer)
                _ListOrder = Value
            End Set
        End Property

        Public Property Hidden() As Boolean
            Get
                Return _Hidden
            End Get
            Set(ByVal Value As Boolean)
                _Hidden = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property ImageDesc() As String
            Get
                Return _ImageDesc
            End Get
            Set(ByVal Value As String)
                _ImageDesc = Value
            End Set
        End Property

        Public Property ImageURL() As String
            Get
                Return _ImageURL
            End Get
            Set(ByVal Value As String)
                _ImageURL = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_ProductsInfo

#Region "Private Members"
        Private _ProductID As Integer
        Private _PortalID As Integer
        Private _TaxCategoryID As Integer
        Private _Featured As Boolean
        Private _Archived As Boolean
        Private _CreatedByUser As String
        Private _CreatedDate As Date
        Private _IsDeleted As Boolean
        Private _ProductRef As String
        Private _Lang As String
        Private _Summary As String
        Private _Description As String
        Private _Manufacturer As String
        Private _ProductName As String
        Private _XMLData As String
        Private _ModifiedDate As Date
        Private _SEOName As String
        Private _TagWords As String
        Private _IsHidden As Boolean
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ProductID() As Integer
            Get
                Return _ProductID
            End Get
            Set(ByVal Value As Integer)
                _ProductID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property TaxCategoryID() As Integer
            Get
                Return _TaxCategoryID
            End Get
            Set(ByVal Value As Integer)
                _TaxCategoryID = Value
            End Set
        End Property

        Public Property Featured() As Boolean
            Get
                Return _Featured
            End Get
            Set(ByVal Value As Boolean)
                _Featured = Value
            End Set
        End Property

        Public Property Archived() As Boolean
            Get
                Return _Archived
            End Get
            Set(ByVal Value As Boolean)
                _Archived = Value
            End Set
        End Property

        Public Property CreatedByUser() As String
            Get
                Return _CreatedByUser
            End Get
            Set(ByVal Value As String)
                _CreatedByUser = Value
            End Set
        End Property

        Public Property CreatedDate() As Date
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As Date)
                _CreatedDate = Value
            End Set
        End Property

        Public Property IsDeleted() As Boolean
            Get
                Return _IsDeleted
            End Get
            Set(ByVal Value As Boolean)
                _IsDeleted = Value
            End Set
        End Property

        Public Property ProductRef() As String
            Get
                Return _ProductRef
            End Get
            Set(ByVal Value As String)
                _ProductRef = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property Summary() As String
            Get
                Return _Summary
            End Get
            Set(ByVal Value As String)
                _Summary = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property

        Public Property Manufacturer() As String
            Get
                Return _Manufacturer
            End Get
            Set(ByVal Value As String)
                _Manufacturer = Value
            End Set
        End Property

        Public Property ProductName() As String
            Get
                Return _ProductName
            End Get
            Set(ByVal Value As String)
                _ProductName = Value
            End Set
        End Property

        Public Property XMLData() As String
            Get
                Return _XMLData
            End Get
            Set(ByVal Value As String)
                _XMLData = Value
            End Set
        End Property

        Public Property ModifiedDate() As Date
            Get
                Return _ModifiedDate
            End Get
            Set(ByVal Value As Date)
                _ModifiedDate = Value
            End Set
        End Property

        Public Property SEOName() As String
            Get
                Return _SEOName
            End Get
            Set(ByVal Value As String)
                _SEOName = Value
            End Set
        End Property

        Public Property TagWords() As String
            Get
                Return _TagWords
            End Get
            Set(ByVal Value As String)
                _TagWords = Value
            End Set
        End Property

        Public Property IsHidden() As Boolean
            Get
                Return _IsHidden
            End Get
            Set(ByVal Value As Boolean)
                _IsHidden = Value
            End Set
        End Property

#End Region

    End Class

    Public Class ProductListInfo

#Region "Private Members"
        Private _ProductID As Integer
        Private _PortalID As Integer
        Private _TaxCategoryID As Integer
        Private _Featured As Boolean
        Private _Archived As Boolean
        Private _CreatedByUser As String
        Private _CreatedDate As Date
        Private _IsDeleted As Boolean
        Private _ProductRef As String
        Private _Lang As String
        Private _Summary As String
        Private _Description As String
        Private _Manufacturer As String
        Private _ProductName As String
        Private _FromPrice As Decimal
        Private _SalePrice As Decimal
        Private _QtyRemaining As Integer
        Private _QtyStockSet As Integer
        Private _ImageURL As String
        Private _ImageDESC As String
        Private _ImageID As Integer
        Private _CategoryName As String
        Private _XMLData As String
        Private _ModifiedDate As Date
        Private _SEOName As String
        Private _TagWords As String
        Private _IsHidden As Boolean
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ProductID() As Integer
            Get
                Return _ProductID
            End Get
            Set(ByVal Value As Integer)
                _ProductID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property TaxCategoryID() As Integer
            Get
                Return _TaxCategoryID
            End Get
            Set(ByVal Value As Integer)
                _TaxCategoryID = Value
            End Set
        End Property

        Public Property Featured() As Boolean
            Get
                Return _Featured
            End Get
            Set(ByVal Value As Boolean)
                _Featured = Value
            End Set
        End Property

        Public Property Archived() As Boolean
            Get
                Return _Archived
            End Get
            Set(ByVal Value As Boolean)
                _Archived = Value
            End Set
        End Property

        Public Property CreatedByUser() As String
            Get
                Return _CreatedByUser
            End Get
            Set(ByVal Value As String)
                _CreatedByUser = Value
            End Set
        End Property

        Public Property CreatedDate() As Date
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As Date)
                _CreatedDate = Value
            End Set
        End Property

        Public Property IsDeleted() As Boolean
            Get
                Return _IsDeleted
            End Get
            Set(ByVal Value As Boolean)
                _IsDeleted = Value
            End Set
        End Property

        Public Property ProductRef() As String
            Get
                Return _ProductRef
            End Get
            Set(ByVal Value As String)
                _ProductRef = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property Summary() As String
            Get
                Return _Summary
            End Get
            Set(ByVal Value As String)
                _Summary = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property

        Public Property Manufacturer() As String
            Get
                Return _Manufacturer
            End Get
            Set(ByVal Value As String)
                _Manufacturer = Value
            End Set
        End Property

        Public Property ProductName() As String
            Get
                Return _ProductName
            End Get
            Set(ByVal Value As String)
                _ProductName = Value
            End Set
        End Property

        Public Property QtyRemaining() As Integer
            Get
                Return _QtyRemaining
            End Get
            Set(ByVal Value As Integer)
                _QtyRemaining = Value
            End Set
        End Property

        Public Property QtyStockSet() As Integer
            Get
                Return _QtyStockSet
            End Get
            Set(ByVal Value As Integer)
                _QtyStockSet = Value
            End Set
        End Property

        Public Property FromPrice() As Decimal
            Get
                Return _FromPrice
            End Get
            Set(ByVal Value As Decimal)
                _FromPrice = Value
            End Set
        End Property

        Public Property SalePrice() As Decimal
            Get
                Return _SalePrice
            End Get
            Set(ByVal Value As Decimal)
                _SalePrice = Value
            End Set
        End Property

        Public Property ImageURL() As String
            Get
                Return _ImageURL
            End Get
            Set(ByVal Value As String)
                _ImageURL = Value
            End Set
        End Property

        Public Property ImageDesc() As String
            Get
                Return _ImageDESC
            End Get
            Set(ByVal Value As String)
                _ImageDESC = Value
            End Set
        End Property

        Public Property ImageID() As Integer
            Get
                Return _ImageID
            End Get
            Set(ByVal Value As Integer)
                _ImageID = Value
            End Set
        End Property

        Public Property CategoryName() As String
            Get
                Return _CategoryName
            End Get
            Set(ByVal Value As String)
                _CategoryName = Value
            End Set
        End Property

        Public Property XMLData() As String
            Get
                Return _XMLData
            End Get
            Set(ByVal Value As String)
                _XMLData = Value
            End Set
        End Property

        Public Property ModifiedDate() As Date
            Get
                Return _ModifiedDate
            End Get
            Set(ByVal Value As Date)
                _ModifiedDate = Value
            End Set
        End Property

        Public Property SEOName() As String
            Get
                Return _SEOName
            End Get
            Set(ByVal Value As String)
                _SEOName = Value
            End Set
        End Property

        Public Property TagWords() As String
            Get
                Return _TagWords
            End Get
            Set(ByVal Value As String)
                _TagWords = Value
            End Set
        End Property

        Public Property IsHidden() As Boolean
            Get
                Return _IsHidden
            End Get
            Set(ByVal Value As Boolean)
                _IsHidden = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_ReviewsInfo

#Region "Private Members"
        Private _ReviewID As Integer
        Private _PortalID As Integer
        Private _ProductID As Integer
        Private _UserID As Integer
        Private _UserName As String
        Private _Rating As Integer
        Private _Comments As String
        Private _Authorized As Boolean
        Private _CreatedDate As Date

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ReviewID() As Integer
            Get
                Return _ReviewID
            End Get
            Set(ByVal Value As Integer)
                _ReviewID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property ProductID() As Integer
            Get
                Return _ProductID
            End Get
            Set(ByVal Value As Integer)
                _ProductID = Value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal Value As Integer)
                _UserID = Value
            End Set
        End Property

        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal Value As String)
                _UserName = Value
            End Set
        End Property

        Public Property Rating() As Integer
            Get
                Return _Rating
            End Get
            Set(ByVal Value As Integer)
                _Rating = Value
            End Set
        End Property

        Public Property Comments() As String
            Get
                Return _Comments
            End Get
            Set(ByVal Value As String)
                _Comments = Value
            End Set
        End Property

        Public Property Authorized() As Boolean
            Get
                Return _Authorized
            End Get
            Set(ByVal Value As Boolean)
                _Authorized = Value
            End Set
        End Property

        Public Property CreatedDate() As Date
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As Date)
                _CreatedDate = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_SaleRatesInfo

#Region "Private Members"
        Private _ItemID As Integer
        Private _CacheKey As String
        Private _PortalID As Integer
        Private _RoleName As String
        Private _CategoryID As Integer
        Private _ModelID As Integer
        Private _SalePrice As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ItemID() As Integer
            Get
                Return _ItemID
            End Get
            Set(ByVal Value As Integer)
                _ItemID = Value
            End Set
        End Property

        Public Property CacheKey() As String
            Get
                Return _CacheKey
            End Get
            Set(ByVal Value As String)
                _CacheKey = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal Value As String)
                _RoleName = Value
            End Set
        End Property

        Public Property CategoryID() As Integer
            Get
                Return _CategoryID
            End Get
            Set(ByVal Value As Integer)
                _CategoryID = Value
            End Set
        End Property

        Public Property ModelID() As Integer
            Get
                Return _ModelID
            End Get
            Set(ByVal Value As Integer)
                _ModelID = Value
            End Set
        End Property

        Public Property SalePrice() As Decimal
            Get
                Return _SalePrice
            End Get
            Set(ByVal Value As Decimal)
                _SalePrice = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_SettingsInfo

#Region "Private Members"
        Private _PortalID As Integer
        Private _SettingName As String
        Private _Lang As String
        Private _SettingValue As String
        Private _HostOnly As Boolean
        Private _GroupRef As String
        Private _CtrlType As String
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property SettingName() As String
            Get
                Return _SettingName
            End Get
            Set(ByVal Value As String)
                _SettingName = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property SettingValue() As String
            Get
                Return _SettingValue
            End Get
            Set(ByVal Value As String)
                _SettingValue = Value
            End Set
        End Property

        Public Property HostOnly() As Boolean
            Get
                Return _HostOnly
            End Get
            Set(ByVal Value As Boolean)
                _HostOnly = Value
            End Set
        End Property

        Public Property GroupRef() As String
            Get
                Return _GroupRef
            End Get
            Set(ByVal Value As String)
                _GroupRef = Value
            End Set
        End Property
        Public Property CtrlType() As String
            Get
                Return _CtrlType
            End Get
            Set(ByVal Value As String)
                _CtrlType = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_SettingsTextInfo

#Region "Private Members"
        Private _PortalID As Integer
        Private _SettingName As String
        Private _Lang As String
        Private _SettingText As String
        Private _HostOnly As Boolean
        Private _GroupRef As String
        Private _CtrlType As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property SettingName() As String
            Get
                Return _SettingName
            End Get
            Set(ByVal Value As String)
                _SettingName = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property SettingText() As String
            Get
                Return _SettingText
            End Get
            Set(ByVal Value As String)
                _SettingText = Value
            End Set
        End Property

        Public Property HostOnly() As Boolean
            Get
                Return _HostOnly
            End Get
            Set(ByVal Value As Boolean)
                _HostOnly = Value
            End Set
        End Property

        Public Property GroupRef() As String
            Get
                Return _GroupRef
            End Get
            Set(ByVal Value As String)
                _GroupRef = Value
            End Set
        End Property
        Public Property CtrlType() As String
            Get
                Return _CtrlType
            End Get
            Set(ByVal Value As String)
                _CtrlType = Value
            End Set
        End Property

        Public Property SettingValue() As String
            Get
                Return _SettingText
            End Get
            Set(ByVal Value As String)
                _SettingText = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_ShippingRatesInfo

#Region "Private Members"
        Private _PortalId As Integer
        Private _ItemId As Integer
        Private _Range1 As Decimal
        Private _Range2 As Decimal
        Private _ObjectId As Integer
        Private _ShipCost As Decimal
        Private _ShipType As String
        Private _Disable As Boolean
        Private _Description As String
        Private _ProductWeight As Decimal
        Private _ProductHeight As Decimal
        Private _ProductLength As Decimal
        Private _ProductWidth As Decimal
        Private _ShipMethodID As Integer

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property PortalID() As Integer
            Get
                Return _PortalId
            End Get
            Set(ByVal Value As Integer)
                _PortalId = Value
            End Set
        End Property

        Public Property ItemId() As Integer
            Get
                Return _ItemId
            End Get
            Set(ByVal Value As Integer)
                _ItemId = Value
            End Set
        End Property

        Public Property Range1() As Decimal
            Get
                Return _Range1
            End Get
            Set(ByVal Value As Decimal)
                _Range1 = Value
            End Set
        End Property

        Public Property Range2() As Decimal
            Get
                Return _Range2
            End Get
            Set(ByVal Value As Decimal)
                _Range2 = Value
            End Set
        End Property

        Public Property ObjectId() As Integer
            Get
                Return _ObjectId
            End Get
            Set(ByVal Value As Integer)
                _ObjectId = Value
            End Set
        End Property

        Public Property ShipCost() As Decimal
            Get
                Return _ShipCost
            End Get
            Set(ByVal Value As Decimal)
                _ShipCost = Value
            End Set
        End Property

        Public Property ShipType() As String
            Get
                Return _ShipType
            End Get
            Set(ByVal Value As String)
                _ShipType = Value
            End Set
        End Property

        Public Property Disable() As Boolean
            Get
                Return _Disable
            End Get
            Set(ByVal Value As Boolean)
                _Disable = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property

        Public Property ProductWeight() As Decimal
            Get
                Return _ProductWeight
            End Get
            Set(ByVal Value As Decimal)
                _ProductWeight = Value
            End Set
        End Property

        Public Property ProductHeight() As Decimal
            Get
                Return _ProductHeight
            End Get
            Set(ByVal Value As Decimal)
                _ProductHeight = Value
            End Set
        End Property

        Public Property ProductLength() As Decimal
            Get
                Return _ProductLength
            End Get
            Set(ByVal Value As Decimal)
                _ProductLength = Value
            End Set
        End Property

        Public Property ProductWidth() As Decimal
            Get
                Return _ProductWidth
            End Get
            Set(ByVal Value As Decimal)
                _ProductWidth = Value
            End Set
        End Property

        Public Property ShipMethodID() As Integer
            Get
                Return _ShipMethodID
            End Get
            Set(ByVal Value As Integer)
                _ShipMethodID = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_StockInfo

#Region "Private Members"
        Private _StockID As Integer
        Private _ModelID As Integer
        Private _QtyRemaining As Integer
        Private _QtyTrans As Integer
        Private _QtyTransDate As Date
        Private _ModifiedDate As Date

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property StockID() As Integer
            Get
                Return _StockID
            End Get
            Set(ByVal Value As Integer)
                _StockID = Value
            End Set
        End Property

        Public Property ModelID() As Integer
            Get
                Return _ModelID
            End Get
            Set(ByVal Value As Integer)
                _ModelID = Value
            End Set
        End Property

        Public Property QtyRemaining() As Integer
            Get
                Return _QtyRemaining
            End Get
            Set(ByVal Value As Integer)
                _QtyRemaining = Value
            End Set
        End Property

        Public Property QtyTrans() As Integer
            Get
                Return _QtyTrans
            End Get
            Set(ByVal Value As Integer)
                _QtyTrans = Value
            End Set
        End Property

        Public Property QtyTransDate() As Date
            Get
                Return _QtyTransDate
            End Get
            Set(ByVal Value As Date)
                _QtyTransDate = Value
            End Set
        End Property

        Public Property ModifiedDate() As Date
            Get
                Return _ModifiedDate
            End Get
            Set(ByVal Value As Date)
                _ModifiedDate = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_TaxRatesInfo

#Region "Private Members"
        Private _PortalID As Integer
        Private _ItemID As Integer
        Private _ObjectID As Integer
        Private _TaxPercent As String
        Private _TaxDesc As String
        Private _TaxType As String
        Private _Disable As Boolean

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property ItemID() As Integer
            Get
                Return _ItemID
            End Get
            Set(ByVal Value As Integer)
                _ItemID = Value
            End Set
        End Property

        Public Property ObjectID() As Integer
            Get
                Return _ObjectID
            End Get
            Set(ByVal Value As Integer)
                _ObjectID = Value
            End Set
        End Property

        Public Property TaxPercent() As String
            Get
                Return _TaxPercent
            End Get
            Set(ByVal Value As String)
                _TaxPercent = Value
            End Set
        End Property

        Public Property TaxDesc() As String
            Get
                Return _TaxDesc
            End Get
            Set(ByVal Value As String)
                _TaxDesc = Value
            End Set
        End Property

        Public Property TaxType() As String
            Get
                Return _TaxType
            End Get
            Set(ByVal Value As String)
                _TaxType = Value
            End Set
        End Property

        Public Property Disable() As Boolean
            Get
                Return _Disable
            End Get
            Set(ByVal Value As Boolean)
                _Disable = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_ProductDocInfo

#Region "Private Members"
        Private _DocID As Integer
        Private _ProductID As Integer
        Private _DocPath As String
        Private _ListOrder As Integer
        Private _Hidden As Boolean
        Private _FileName As String
        Private _FileExt As String
        Private _Lang As String
        Private _DocDesc As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property DocID() As Integer
            Get
                Return _DocID
            End Get
            Set(ByVal Value As Integer)
                _DocID = Value
            End Set
        End Property

        Public Property ProductID() As Integer
            Get
                Return _ProductID
            End Get
            Set(ByVal Value As Integer)
                _ProductID = Value
            End Set
        End Property

        Public Property DocPath() As String
            Get
                Return _DocPath
            End Get
            Set(ByVal Value As String)
                _DocPath = Value
            End Set
        End Property

        Public Property ListOrder() As Integer
            Get
                Return _ListOrder
            End Get
            Set(ByVal Value As Integer)
                _ListOrder = Value
            End Set
        End Property

        Public Property Hidden() As Boolean
            Get
                Return _Hidden
            End Get
            Set(ByVal Value As Boolean)
                _Hidden = Value
            End Set
        End Property

        Public Property FileName() As String
            Get
                Return _FileName
            End Get
            Set(ByVal Value As String)
                _FileName = Value
            End Set
        End Property

        Public Property FileExt() As String
            Get
                Return _FileExt
            End Get
            Set(ByVal Value As String)
                _FileExt = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property DocDesc() As String
            Get
                Return _DocDesc
            End Get
            Set(ByVal Value As String)
                _DocDesc = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_ProductSelectDocInfo

#Region "Private Members"
        Private _DocPath As String
        Private _FileName As String
        Private _FileExt As String
        Private _Lang As String
        Private _DocDesc As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"

        Public Property DocPath() As String
            Get
                Return _DocPath
            End Get
            Set(ByVal Value As String)
                _DocPath = Value
            End Set
        End Property

        Public Property FileName() As String
            Get
                Return _FileName
            End Get
            Set(ByVal Value As String)
                _FileName = Value
            End Set
        End Property

        Public Property FileExt() As String
            Get
                Return _FileExt
            End Get
            Set(ByVal Value As String)
                _FileExt = Value
            End Set
        End Property

        Public Property Lang() As String
            Get
                Return _Lang
            End Get
            Set(ByVal Value As String)
                _Lang = Value
            End Set
        End Property

        Public Property DocDesc() As String
            Get
                Return _DocDesc
            End Get
            Set(ByVal Value As String)
                _DocDesc = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_ProductCategoryInfo

#Region "Private Members"
        Private _ProductID As Integer
        Private _CategoryID As Integer

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ProductID() As Integer
            Get
                Return _ProductID
            End Get
            Set(ByVal Value As Integer)
                _ProductID = Value
            End Set
        End Property

        Public Property CategoryID() As Integer
            Get
                Return _CategoryID
            End Get
            Set(ByVal Value As Integer)
                _CategoryID = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_ShippingMethodInfo

#Region "Private Members"
        Private _ShipMethodID As Integer
        Private _PortalID As Integer
        Private _MethodName As String
        Private _MethodDesc As String
        Private _SortOrder As Integer
        Private _TemplateName As String
        Private _Disabled As Boolean
        Private _URLtracker As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ShipMethodID() As Integer
            Get
                Return _ShipMethodID
            End Get
            Set(ByVal Value As Integer)
                _ShipMethodID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property MethodName() As String
            Get
                Return _MethodName
            End Get
            Set(ByVal Value As String)
                _MethodName = Value
            End Set
        End Property

        Public Property MethodDesc() As String
            Get
                Return _MethodDesc
            End Get
            Set(ByVal Value As String)
                _MethodDesc = Value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return _SortOrder
            End Get
            Set(ByVal Value As Integer)
                _SortOrder = Value
            End Set
        End Property

        Public Property TemplateName() As String
            Get
                Return _TemplateName
            End Get
            Set(ByVal Value As String)
                _TemplateName = Value
            End Set
        End Property

        Public Property Disabled() As Boolean
            Get
                Return _Disabled
            End Get
            Set(ByVal Value As Boolean)
                _Disabled = Value
            End Set
        End Property

        Public Property URLtracker() As String
            Get
                Return _URLtracker
            End Get
            Set(ByVal Value As String)
                _URLtracker = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_PromoInfo

#Region "Private Members"
        Private _PromoID As Integer
        Private _PortalID As Integer
        Private _ObjectID As Integer
        Private _PromoName As String
        Private _PromoType As String
        Private _Range1 As String
        Private _Range2 As String
        Private _RangeStartDate As Date
        Private _RangeEndDate As Date
        Private _PromoAmount As String
        Private _PromoPercent As Integer
        Private _Disabled As Boolean
        Private _PromoCode As String
        Private _PromoGroup As String
        Private _PromoUser As String
        Private _QtyRange1 As Integer
        Private _QtyRange2 As Integer
        Private _PromoEmail As String
        Private _XMLData As String
        Private _MaxUsage As Integer = 1
        'added by Philipp Becker 12/07/2010
        Private _MaxUsagePerUser As Integer = 1

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"


        'added by Philipp Becker 12/07/2010
        Public Property MaxUsagePerUser() As Integer
            Get
                Return _MaxUsagePerUser
            End Get
            Set(ByVal Value As Integer)
                _MaxUsagePerUser = Value
            End Set
        End Property

        Public Property MaxUsage() As Integer
            Get
                Return _MaxUsage
            End Get
            Set(ByVal Value As Integer)
                _MaxUsage = Value
            End Set
        End Property

        Public Property PromoID() As Integer
            Get
                Return _PromoID
            End Get
            Set(ByVal Value As Integer)
                _PromoID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property ObjectID() As Integer
            Get
                Return _ObjectID
            End Get
            Set(ByVal Value As Integer)
                _ObjectID = Value
            End Set
        End Property

        Public Property PromoName() As String
            Get
                Return _PromoName
            End Get
            Set(ByVal Value As String)
                _PromoName = Value
            End Set
        End Property

        Public Property PromoType() As String
            Get
                Return _PromoType
            End Get
            Set(ByVal Value As String)
                _PromoType = Value
            End Set
        End Property

        Public Property Range1() As String
            Get
                Return _Range1
            End Get
            Set(ByVal Value As String)
                _Range1 = Value
            End Set
        End Property

        Public Property Range2() As String
            Get
                Return _Range2
            End Get
            Set(ByVal Value As String)
                _Range2 = Value
            End Set
        End Property

        Public Property RangeStartDate() As Date
            Get
                Return _RangeStartDate
            End Get
            Set(ByVal Value As Date)
                _RangeStartDate = Value
            End Set
        End Property

        Public Property RangeEndDate() As Date
            Get
                Return _RangeEndDate
            End Get
            Set(ByVal Value As Date)
                _RangeEndDate = Value
            End Set
        End Property

        Public Property PromoAmount() As String
            Get
                Return _PromoAmount
            End Get
            Set(ByVal Value As String)
                _PromoAmount = Value
            End Set
        End Property

        Public Property PromoPercent() As Integer
            Get
                Return _PromoPercent
            End Get
            Set(ByVal Value As Integer)
                _PromoPercent = Value
            End Set
        End Property

        Public Property Disabled() As Boolean
            Get
                Return _Disabled
            End Get
            Set(ByVal Value As Boolean)
                _Disabled = Value
            End Set
        End Property

        Public Property PromoCode() As String
            Get
                Return _PromoCode
            End Get
            Set(ByVal Value As String)
                _PromoCode = Value
            End Set
        End Property

        Public Property PromoGroup() As String
            Get
                Return _PromoGroup
            End Get
            Set(ByVal Value As String)
                _PromoGroup = Value
            End Set
        End Property

        Public Property PromoUser() As String
            Get
                Return _PromoUser
            End Get
            Set(ByVal Value As String)
                _PromoUser = Value
            End Set
        End Property

        Public Property QtyRange1() As Integer
            Get
                Return _QtyRange1
            End Get
            Set(ByVal Value As Integer)
                _QtyRange1 = Value
            End Set
        End Property

        Public Property QtyRange2() As Integer
            Get
                Return _QtyRange2
            End Get
            Set(ByVal Value As Integer)
                _QtyRange2 = Value
            End Set
        End Property

        Public Property PromoEmail() As String
            Get
                Return _PromoEmail
            End Get
            Set(ByVal Value As String)
                _PromoEmail = Value
            End Set
        End Property

        Public Property XMLData() As String
            Get
                Return _XMLData
            End Get
            Set(ByVal Value As String)
                _XMLData = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_SQLReportInfo

#Region "Private Members"
        Private _ReportID As Integer
        Private _PortalID As Integer
        Private _ReportName As String
        Private _SQL As String
        Private _SchedulerFlag As Boolean
        Private _SchStartHour As String
        Private _SchStartMins As String
        Private _SchReRunMins As String
        Private _LastRunTime As Date
        Private _AllowExport As Boolean
        Private _AllowDisplay As Boolean
        Private _DisplayInLine As Boolean
        Private _EmailResults As Boolean
        Private _EmailFrom As String
        Private _EmailTo As String


        Private _ShowSQL As Boolean
        Private _ConnectionString As String
        Private _ReportRef As String
        Private _AllowPaging As Boolean

        Private _ReportTitle As String
        Private _FieldDelimeter As String
        Private _FieldQualifier As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ReportID() As Integer
            Get
                Return _ReportID
            End Get
            Set(ByVal Value As Integer)
                _ReportID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property ReportName() As String
            Get
                Return _ReportName
            End Get
            Set(ByVal Value As String)
                _ReportName = Value
            End Set
        End Property

        Public Property ReportTitle() As String
            Get
                Return _ReportTitle
            End Get
            Set(ByVal Value As String)
                _ReportTitle = Value
            End Set
        End Property

        Public Property SQL() As String
            Get
                Return _SQL
            End Get
            Set(ByVal Value As String)
                _SQL = Value
            End Set
        End Property

        Public Property SchedulerFlag() As Boolean
            Get
                Return _SchedulerFlag
            End Get
            Set(ByVal Value As Boolean)
                _SchedulerFlag = Value
            End Set
        End Property

        Public Property SchStartHour() As String
            Get
                Return _SchStartHour
            End Get
            Set(ByVal Value As String)
                _SchStartHour = Value
            End Set
        End Property

        Public Property SchStartMins() As String
            Get
                Return _SchStartMins
            End Get
            Set(ByVal Value As String)
                _SchStartMins = Value
            End Set
        End Property

        Public Property SchReRunMins() As String
            Get
                Return _SchReRunMins
            End Get
            Set(ByVal Value As String)
                _SchReRunMins = Value
            End Set
        End Property

        Public Property LastRunTime() As Date
            Get
                Return _LastRunTime
            End Get
            Set(ByVal Value As Date)
                _LastRunTime = Value
            End Set
        End Property

        Public Property AllowExport() As Boolean
            Get
                Return _AllowExport
            End Get
            Set(ByVal Value As Boolean)
                _AllowExport = Value
            End Set
        End Property

        Public Property AllowDisplay() As Boolean
            Get
                Return _AllowDisplay
            End Get
            Set(ByVal Value As Boolean)
                _AllowDisplay = Value
            End Set
        End Property

        Public Property DisplayInLine() As Boolean
            Get
                Return _DisplayInLine
            End Get
            Set(ByVal Value As Boolean)
                _DisplayInLine = Value
            End Set
        End Property

        Public Property EmailResults() As Boolean
            Get
                Return _EmailResults
            End Get
            Set(ByVal Value As Boolean)
                _EmailResults = Value
            End Set
        End Property

        Public Property EmailFrom() As String
            Get
                Return _EmailFrom
            End Get
            Set(ByVal Value As String)
                _EmailFrom = Value
            End Set
        End Property

        Public Property EmailTo() As String
            Get
                Return _EmailTo
            End Get
            Set(ByVal Value As String)
                _EmailTo = Value
            End Set
        End Property




        Public Property ShowSQL() As Boolean
            Get
                Return _ShowSQL
            End Get
            Set(ByVal Value As Boolean)
                _ShowSQL = Value
            End Set
        End Property

        Public Property ConnectionString() As String
            Get
                Return _ConnectionString
            End Get
            Set(ByVal Value As String)
                _ConnectionString = Value
            End Set
        End Property

        Public Property ReportRef() As String
            Get
                Return _ReportRef
            End Get
            Set(ByVal Value As String)
                _ReportRef = Value
            End Set
        End Property

        Public Property AllowPaging() As Boolean
            Get
                Return _AllowPaging
            End Get
            Set(ByVal Value As Boolean)
                _AllowPaging = Value
            End Set
        End Property



        Public Property FieldDelimeter() As String
            Get
                Return _FieldDelimeter
            End Get
            Set(ByVal Value As String)
                _FieldDelimeter = Value
            End Set
        End Property

        Public Property FieldQualifier() As String
            Get
                Return _FieldQualifier
            End Get
            Set(ByVal Value As String)
                _FieldQualifier = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_SQLReportParamInfo

#Region "Private Members"
        Private _ReportParamID As Integer
        Private _ReportID As Integer
        Private _ParamName As String
        Private _ParamType As String
        Private _ParamValue As String
        Private _ParamSource As Integer
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ReportParamID() As Integer
            Get
                Return _ReportParamID
            End Get
            Set(ByVal Value As Integer)
                _ReportParamID = Value
            End Set
        End Property

        Public Property ReportID() As Integer
            Get
                Return _ReportID
            End Get
            Set(ByVal Value As Integer)
                _ReportID = Value
            End Set
        End Property

        Public Property ParamName() As String
            Get
                Return _ParamName
            End Get
            Set(ByVal Value As String)
                _ParamName = Value
            End Set
        End Property

        Public Property ParamType() As String
            Get
                Return _ParamType
            End Get
            Set(ByVal Value As String)
                _ParamType = Value
            End Set
        End Property

        Public Property ParamValue() As String
            Get
                Return _ParamValue
            End Get
            Set(ByVal Value As String)
                _ParamValue = Value
            End Set
        End Property

        Public Property ParamSource() As Integer
            Get
                Return _ParamSource
            End Get
            Set(ByVal Value As Integer)
                _ParamSource = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_SQLReportXSLInfo

#Region "Private Members"
        Private _ReportXSLID As Integer
        Private _ReportID As Integer
        Private _XMLInput As String
        Private _XSLFile As String
        Private _OutputFile As String
        Private _DisplayResults As Boolean
        Private _SortOrder As Integer

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ReportXSLID() As Integer
            Get
                Return _ReportXSLID
            End Get
            Set(ByVal Value As Integer)
                _ReportXSLID = Value
            End Set
        End Property

        Public Property ReportID() As Integer
            Get
                Return _ReportID
            End Get
            Set(ByVal Value As Integer)
                _ReportID = Value
            End Set
        End Property

        Public Property XMLInput() As String
            Get
                Return _XMLInput
            End Get
            Set(ByVal Value As String)
                _XMLInput = Value
            End Set
        End Property

        Public Property XSLFile() As String
            Get
                Return _XSLFile
            End Get
            Set(ByVal Value As String)
                _XSLFile = Value
            End Set
        End Property

        Public Property OutputFile() As String
            Get
                Return _OutputFile
            End Get
            Set(ByVal Value As String)
                _OutputFile = Value
            End Set
        End Property

        Public Property DisplayResults() As Boolean
            Get
                Return _DisplayResults
            End Get
            Set(ByVal Value As Boolean)
                _DisplayResults = Value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return _SortOrder
            End Get
            Set(ByVal Value As Integer)
                _SortOrder = Value
            End Set
        End Property


#End Region

    End Class


    Public Class NB_Store_ProductRelatedListInfo
        Inherits NB_Store_ProductRelatedInfo

#Region "Private Members"
        Private _RelatedProductRef As String
        Private _RelatedProductName As String
#End Region


#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"

        Public Property RelatedProductRef() As String
            Get
                Return _RelatedProductRef
            End Get
            Set(ByVal Value As String)
                _RelatedProductRef = Value
            End Set
        End Property

        Public Property RelatedProductName() As String
            Get
                Return _RelatedProductName
            End Get
            Set(ByVal Value As String)
                _RelatedProductName = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_ProductRelatedInfo

#Region "Private Members"
        Private _RelatedID As Integer
        Private _PortalID As Integer
        Private _ProductID As Integer
        Private _RelatedProductID As Integer
        Private _DiscountAmt As Decimal
        Private _DiscountPercent As Decimal
        Private _ProductQty As Integer
        Private _MaxQty As Integer
        Private _RelatedType As Integer
        Private _Disabled As Boolean
        Private _NotAvailable As Boolean
        Private _BiDirectional As Boolean

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property RelatedID() As Integer
            Get
                Return _RelatedID
            End Get
            Set(ByVal Value As Integer)
                _RelatedID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property


        Public Property ProductID() As Integer
            Get
                Return _ProductID
            End Get
            Set(ByVal Value As Integer)
                _ProductID = Value
            End Set
        End Property

        Public Property RelatedProductID() As Integer
            Get
                Return _RelatedProductID
            End Get
            Set(ByVal Value As Integer)
                _RelatedProductID = Value
            End Set
        End Property

        Public Property DiscountAmt() As Decimal
            Get
                Return _DiscountAmt
            End Get
            Set(ByVal Value As Decimal)
                _DiscountAmt = Value
            End Set
        End Property

        Public Property DiscountPercent() As Decimal
            Get
                Return _DiscountPercent
            End Get
            Set(ByVal Value As Decimal)
                _DiscountPercent = Value
            End Set
        End Property

        Public Property ProductQty() As Integer
            Get
                Return _ProductQty
            End Get
            Set(ByVal Value As Integer)
                _ProductQty = Value
            End Set
        End Property

        Public Property MaxQty() As Integer
            Get
                Return _MaxQty
            End Get
            Set(ByVal Value As Integer)
                _MaxQty = Value
            End Set
        End Property

        Public Property RelatedType() As Integer
            Get
                Return _RelatedType
            End Get
            Set(ByVal Value As Integer)
                _RelatedType = Value
            End Set
        End Property

        Public Property Disabled() As Boolean
            Get
                Return _Disabled
            End Get
            Set(ByVal Value As Boolean)
                _Disabled = Value
            End Set
        End Property

        Public Property NotAvailable() As Boolean
            Get
                Return _NotAvailable
            End Get
            Set(ByVal Value As Boolean)
                _NotAvailable = Value
            End Set
        End Property

        Public Property BiDirectional() As Boolean
            Get
                Return _BiDirectional
            End Get
            Set(ByVal Value As Boolean)
                _BiDirectional = Value
            End Set
        End Property

#End Region

    End Class

    Public Class NB_Store_GatewayInfo

#Region "Private Members"
        Private _ref As String
        Private _name As String
        Private _assembly As String
        Private _class As String
        Private _gatewaymsg As String
        Private _gatewaytype As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"

        Public Property ref() As String
            Get
                Return _ref
            End Get
            Set(ByVal Value As String)
                _ref = Value
            End Set
        End Property

        Public Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal Value As String)
                _name = Value
            End Set
        End Property

        Public Property assembly() As String
            Get
                Return _assembly
            End Get
            Set(ByVal Value As String)
                _assembly = Value
            End Set
        End Property

        Public Property classname() As String
            Get
                Return _class
            End Get
            Set(ByVal Value As String)
                _class = Value
            End Set
        End Property

        Public Property gatewaymsg() As String
            Get
                Return _gatewaymsg
            End Get
            Set(ByVal Value As String)
                _gatewaymsg = Value
            End Set
        End Property

        Public Property gatewaytype() As String
            Get
                Return _gatewaytype
            End Get
            Set(ByVal Value As String)
                _gatewaytype = Value
            End Set
        End Property


#End Region

    End Class

    Public Class NB_Store_SearchWordHitsInfo

#Region "Private Members"
        Private _SearchWordHitID As Integer
        Private _SearchWordID As Integer
        Private _HitDate As Date
        Private _HitCount As Integer
        Private _WordPosition As Integer

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property SearchWordHitID() As Integer
            Get
                Return _SearchWordHitID
            End Get
            Set(ByVal Value As Integer)
                _SearchWordHitID = Value
            End Set
        End Property

        Public Property SearchWordID() As Integer
            Get
                Return _SearchWordID
            End Get
            Set(ByVal Value As Integer)
                _SearchWordID = Value
            End Set
        End Property

        Public Property HitDate() As Date
            Get
                Return _HitDate
            End Get
            Set(ByVal Value As Date)
                _HitDate = Value
            End Set
        End Property

        Public Property HitCount() As Integer
            Get
                Return _HitCount
            End Get
            Set(ByVal Value As Integer)
                _HitCount = Value
            End Set
        End Property

        Public Property WordPosition() As Integer
            Get
                Return _WordPosition
            End Get
            Set(ByVal Value As Integer)
                _WordPosition = Value
            End Set
        End Property


#End Region

    End Class


    Public Class NB_Store_SearchWordsInfo

#Region "Private Members"
        Private _SearchWordID As Integer
        Private _PortalID As Integer
        Private _SearchWord As String
        Private _ExistsOnProduct As Boolean
        Private _ExistsCount As Integer
        Private _LastHitDate As Date

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property SearchWordID() As Integer
            Get
                Return _SearchWordID
            End Get
            Set(ByVal Value As Integer)
                _SearchWordID = Value
            End Set
        End Property

        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As Integer)
                _PortalID = Value
            End Set
        End Property

        Public Property SearchWord() As String
            Get
                Return _SearchWord
            End Get
            Set(ByVal Value As String)
                _SearchWord = Value
            End Set
        End Property

        Public Property ExistsOnProduct() As Boolean
            Get
                Return _ExistsOnProduct
            End Get
            Set(ByVal Value As Boolean)
                _ExistsOnProduct = Value
            End Set
        End Property

        Public Property ExistsCount() As Integer
            Get
                Return _ExistsCount
            End Get
            Set(ByVal Value As Integer)
                _ExistsCount = Value
            End Set
        End Property

        Public Property LastHitDate() As Date
            Get
                Return _LastHitDate
            End Get
            Set(ByVal Value As Date)
                _LastHitDate = Value
            End Set
        End Property


#End Region

    End Class


    Public Class NB_Store_SearchWordUpdateInfo

#Region "Private Members"
        Private _SearchWord As String
        Private _WordPosition As Integer
#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"

        Public Property SearchWord() As String
            Get
                Return _SearchWord
            End Get
            Set(ByVal Value As String)
                _SearchWord = Value
            End Set
        End Property

        Public Property WordPosition() As Integer
            Get
                Return _WordPosition
            End Get
            Set(ByVal Value As Integer)
                _WordPosition = Value
            End Set
        End Property


#End Region

    End Class


    Public Class NBright_TextBox

#Region "Private Members"

        Private _Text As String = ""
        Private _ID As String = ""
        Private _CausesValidation As Boolean = True
        Private _Columns As Integer = 0
        Private _MaxLength As Integer = 0
        Private _TextMode As TextBoxMode = TextBoxMode.SingleLine
        Private _Rows As Integer = 0
        Private _Wrap As Boolean = True
        Private _BackColor As String = ""
        Private _BorderColor As String = ""
        Private _BorderWidth As String = ""
        Private _BorderStyle As BorderStyle = Web.UI.WebControls.BorderStyle.NotSet
        Private _CssClass As String = ""
        Private _Enabled As Boolean = True
        Private _ForeColor As String = ""
        Private _Height As String = ""
        Private _TabIndex As Integer = 0
        Private _ToolTip As String = ""
        Private _Width As String = ""
        Private _Visible As Boolean = True

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ID() As String
            Get
                Return _ID
            End Get
            Set(ByVal Value As String)
                _ID = Value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return _Text
            End Get
            Set(ByVal Value As String)
                _Text = Value
            End Set
        End Property

        Public Property CausesValidation() As Boolean
            Get
                Return _CausesValidation
            End Get
            Set(ByVal Value As Boolean)
                _CausesValidation = Value
            End Set
        End Property

        Public Property Columns() As Integer
            Get
                Return _Columns
            End Get
            Set(ByVal Value As Integer)
                _Columns = Value
            End Set
        End Property

        Public Property MaxLength() As Integer
            Get
                Return _MaxLength
            End Get
            Set(ByVal Value As Integer)
                _MaxLength = Value
            End Set
        End Property

        Public Property TextMode() As TextBoxMode
            Get
                Return _TextMode
            End Get
            Set(ByVal Value As TextBoxMode)
                _TextMode = Value
            End Set
        End Property

        Public Property Rows() As Integer
            Get
                Return _Rows
            End Get
            Set(ByVal Value As Integer)
                _Rows = Value
            End Set
        End Property

        Public Property Wrap() As Boolean
            Get
                Return _Wrap
            End Get
            Set(ByVal Value As Boolean)
                _Wrap = Value
            End Set
        End Property

        Public Property BackColor() As String
            Get
                Return _BackColor
            End Get
            Set(ByVal Value As String)
                _BackColor = Value
            End Set
        End Property

        Public Property BorderWidth() As String
            Get
                Return _BorderWidth
            End Get
            Set(ByVal Value As String)
                _BorderWidth = Value
            End Set
        End Property

        Public Property BorderColor() As String
            Get
                Return _BorderColor
            End Get
            Set(ByVal Value As String)
                _BorderColor = Value
            End Set
        End Property

        Public Property BorderStyle() As BorderStyle
            Get
                Return _BorderStyle
            End Get
            Set(ByVal Value As BorderStyle)
                _BorderStyle = Value
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return _CssClass
            End Get
            Set(ByVal Value As String)
                _CssClass = Value
            End Set
        End Property

        Public Property Enabled() As Boolean
            Get
                Return _Enabled
            End Get
            Set(ByVal Value As Boolean)
                _Enabled = Value
            End Set
        End Property

        Public Property ForeColor() As String
            Get
                Return _ForeColor
            End Get
            Set(ByVal Value As String)
                _ForeColor = Value
            End Set
        End Property

        Public Property Height() As String
            Get
                Return _Height
            End Get
            Set(ByVal Value As String)
                _Height = Value
            End Set
        End Property

        Public Property TabIndex() As Integer
            Get
                Return _TabIndex
            End Get
            Set(ByVal Value As Integer)
                _TabIndex = Value
            End Set
        End Property

        Public Property ToolTip() As String
            Get
                Return _ToolTip
            End Get
            Set(ByVal Value As String)
                _ToolTip = Value
            End Set
        End Property

        Public Property Width() As String
            Get
                Return _Width
            End Get
            Set(ByVal Value As String)
                _Width = Value
            End Set
        End Property

        Public Property Visible() As Boolean
            Get
                Return _Visible
            End Get
            Set(ByVal Value As Boolean)
                _Visible = Value
            End Set
        End Property
#End Region

    End Class


    Public Class NBright_DropDownList

#Region "Private Members"
        Private _ID As String = ""
        Private _BorderColor As String = ""
        Private _BorderStyle As BorderStyle = Web.UI.WebControls.BorderStyle.NotSet
        Private _BorderWidth As String = ""
        Private _SelectedIndex As Integer = -1
        Private _CausesValidation As Boolean = True
        Private _SelectedValue As String = ""
        Private _Text As String = ""
        Private _BackColor As String = ""
        Private _CssClass As String = ""
        Private _Enabled As Boolean = True
        Private _ForeColor As String = ""
        Private _Height As String = ""
        Private _TabIndex As Integer = 0
        Private _ToolTip As String = ""
        Private _Width As String = ""
        Private _Visible As Boolean = True
        Private _Data As String = ""
        Private _DataValue As String = ""

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ID() As String
            Get
                Return _ID
            End Get
            Set(ByVal Value As String)
                _ID = Value
            End Set
        End Property

        Public Property BorderColor() As String
            Get
                Return _BorderColor
            End Get
            Set(ByVal Value As String)
                _BorderColor = Value
            End Set
        End Property

        Public Property BorderStyle() As BorderStyle
            Get
                Return _BorderStyle
            End Get
            Set(ByVal Value As BorderStyle)
                _BorderStyle = Value
            End Set
        End Property

        Public Property BorderWidth() As String
            Get
                Return _BorderWidth
            End Get
            Set(ByVal Value As String)
                _BorderWidth = Value
            End Set
        End Property

        Public Property SelectedIndex() As Integer
            Get
                Return _SelectedIndex
            End Get
            Set(ByVal Value As Integer)
                _SelectedIndex = Value
            End Set
        End Property

        Public Property CausesValidation() As Boolean
            Get
                Return _CausesValidation
            End Get
            Set(ByVal Value As Boolean)
                _CausesValidation = Value
            End Set
        End Property

        Public Property SelectedValue() As String
            Get
                Return _SelectedValue
            End Get
            Set(ByVal Value As String)
                _SelectedValue = Value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return _Text
            End Get
            Set(ByVal Value As String)
                _Text = Value
            End Set
        End Property

        Public Property BackColor() As String
            Get
                Return _BackColor
            End Get
            Set(ByVal Value As String)
                _BackColor = Value
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return _CssClass
            End Get
            Set(ByVal Value As String)
                _CssClass = Value
            End Set
        End Property

        Public Property Enabled() As Boolean
            Get
                Return _Enabled
            End Get
            Set(ByVal Value As Boolean)
                _Enabled = Value
            End Set
        End Property

        Public Property ForeColor() As String
            Get
                Return _ForeColor
            End Get
            Set(ByVal Value As String)
                _ForeColor = Value
            End Set
        End Property

        Public Property Height() As String
            Get
                Return _Height
            End Get
            Set(ByVal Value As String)
                _Height = Value
            End Set
        End Property

        Public Property TabIndex() As Integer
            Get
                Return _TabIndex
            End Get
            Set(ByVal Value As Integer)
                _TabIndex = Value
            End Set
        End Property

        Public Property ToolTip() As String
            Get
                Return _ToolTip
            End Get
            Set(ByVal Value As String)
                _ToolTip = Value
            End Set
        End Property

        Public Property Width() As String
            Get
                Return _Width
            End Get
            Set(ByVal Value As String)
                _Width = Value
            End Set
        End Property

        Public Property Visible() As Boolean
            Get
                Return _Visible
            End Get
            Set(ByVal Value As Boolean)
                _Visible = Value
            End Set
        End Property

        Public Property data() As String
            Get
                Return _Data
            End Get
            Set(ByVal Value As String)
                _Data = Value
            End Set
        End Property

        Public Property datavalue() As String
            Get
                Return _DataValue
            End Get
            Set(ByVal Value As String)
                _DataValue = Value
            End Set
        End Property


#End Region

    End Class


    Public Class NBright_CheckBox

#Region "Private Members"
        Private _ID As String = ""
        Private _CausesValidation As Boolean = True
        Private _Checked As Boolean = False
        Private _Text As String = ""
        Private _TextAlign As TextAlign = Web.UI.WebControls.TextAlign.Left
        Private _BackColor As String = ""
        Private _BorderColor As String = ""
        Private _BorderWidth As String = ""
        Private _BorderStyle As BorderStyle = Web.UI.WebControls.BorderStyle.NotSet
        Private _CssClass As String = ""
        Private _Enabled As Boolean = True
        Private _ForeColor As String = ""
        Private _Height As String = ""
        Private _TabIndex As Integer = 0
        Private _ToolTip As String = ""
        Private _Width As String = ""
        Private _Visible As Boolean = True

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ID() As String
            Get
                Return _ID
            End Get
            Set(ByVal Value As String)
                _ID = Value
            End Set
        End Property

        Public Property CausesValidation() As Boolean
            Get
                Return _CausesValidation
            End Get
            Set(ByVal Value As Boolean)
                _CausesValidation = Value
            End Set
        End Property

        Public Property Checked() As Boolean
            Get
                Return _Checked
            End Get
            Set(ByVal Value As Boolean)
                _Checked = Value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return _Text
            End Get
            Set(ByVal Value As String)
                _Text = Value
            End Set
        End Property

        Public Property TextAlign() As TextAlign
            Get
                Return _TextAlign
            End Get
            Set(ByVal Value As TextAlign)
                _TextAlign = Value
            End Set
        End Property

        Public Property BackColor() As String
            Get
                Return _BackColor
            End Get
            Set(ByVal Value As String)
                _BackColor = Value
            End Set
        End Property

        Public Property BorderColor() As String
            Get
                Return _BorderColor
            End Get
            Set(ByVal Value As String)
                _BorderColor = Value
            End Set
        End Property

        Public Property BorderWidth() As String
            Get
                Return _BorderWidth
            End Get
            Set(ByVal Value As String)
                _BorderWidth = Value
            End Set
        End Property

        Public Property BorderStyle() As BorderStyle
            Get
                Return _BorderStyle
            End Get
            Set(ByVal Value As BorderStyle)
                _BorderStyle = Value
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return _CssClass
            End Get
            Set(ByVal Value As String)
                _CssClass = Value
            End Set
        End Property

        Public Property Enabled() As Boolean
            Get
                Return _Enabled
            End Get
            Set(ByVal Value As Boolean)
                _Enabled = Value
            End Set
        End Property

        Public Property ForeColor() As String
            Get
                Return _ForeColor
            End Get
            Set(ByVal Value As String)
                _ForeColor = Value
            End Set
        End Property

        Public Property Height() As String
            Get
                Return _Height
            End Get
            Set(ByVal Value As String)
                _Height = Value
            End Set
        End Property

        Public Property TabIndex() As Integer
            Get
                Return _TabIndex
            End Get
            Set(ByVal Value As Integer)
                _TabIndex = Value
            End Set
        End Property

        Public Property ToolTip() As String
            Get
                Return _ToolTip
            End Get
            Set(ByVal Value As String)
                _ToolTip = Value
            End Set
        End Property

        Public Property Width() As String
            Get
                Return _Width
            End Get
            Set(ByVal Value As String)
                _Width = Value
            End Set
        End Property

        Public Property Visible() As Boolean
            Get
                Return _Visible
            End Get
            Set(ByVal Value As Boolean)
                _Visible = Value
            End Set
        End Property


#End Region

    End Class


    Public Class NBright_RadioButtonList

#Region "Private Members"
        Private _ID As String = ""
        Private _CellPadding As Integer = -1
        Private _CellSpacing As Integer = -1
        Private _RepeatColumns As Integer = 0
        Private _RepeatDirection As RepeatDirection = Web.UI.WebControls.RepeatDirection.Vertical
        Private _RepeatLayout As RepeatLayout = Web.UI.WebControls.RepeatLayout.Table
        Private _TextAlign As TextAlign = Web.UI.WebControls.TextAlign.Left
        Private _CausesValidation As Boolean = True
        Private _SelectedIndex As Integer = -1
        Private _SelectedValue As String = ""
        Private _Text As String = ""
        Private _BackColor As String = ""
        Private _BorderColor As String = ""
        Private _BorderWidth As String = ""
        Private _BorderStyle As BorderStyle = Web.UI.WebControls.BorderStyle.NotSet
        Private _CssClass As String = ""
        Private _Enabled As Boolean = True
        Private _ForeColor As String = ""
        Private _Height As String = ""
        Private _TabIndex As Integer = 0
        Private _ToolTip As String = ""
        Private _Width As String = ""
        Private _Visible As Boolean = True
        Private _Data As String = ""
        Private _DataValue As String = ""

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ID() As String
            Get
                Return _ID
            End Get
            Set(ByVal Value As String)
                _ID = Value
            End Set
        End Property

        Public Property CellPadding() As Integer
            Get
                Return _CellPadding
            End Get
            Set(ByVal Value As Integer)
                _CellPadding = Value
            End Set
        End Property

        Public Property CellSpacing() As Integer
            Get
                Return _CellSpacing
            End Get
            Set(ByVal Value As Integer)
                _CellSpacing = Value
            End Set
        End Property

        Public Property RepeatColumns() As Integer
            Get
                Return _RepeatColumns
            End Get
            Set(ByVal Value As Integer)
                _RepeatColumns = Value
            End Set
        End Property

        Public Property RepeatDirection() As String
            Get
                Return _RepeatDirection
            End Get
            Set(ByVal Value As String)
                _RepeatDirection = Value
            End Set
        End Property

        Public Property RepeatLayout() As String
            Get
                Return _RepeatLayout
            End Get
            Set(ByVal Value As String)
                _RepeatLayout = Value
            End Set
        End Property

        Public Property TextAlign() As String
            Get
                Return _TextAlign
            End Get
            Set(ByVal Value As String)
                _TextAlign = Value
            End Set
        End Property

        Public Property CausesValidation() As Boolean
            Get
                Return _CausesValidation
            End Get
            Set(ByVal Value As Boolean)
                _CausesValidation = Value
            End Set
        End Property

        Public Property SelectedIndex() As Integer
            Get
                Return _SelectedIndex
            End Get
            Set(ByVal Value As Integer)
                _SelectedIndex = Value
            End Set
        End Property

        Public Property SelectedValue() As String
            Get
                Return _SelectedValue
            End Get
            Set(ByVal Value As String)
                _SelectedValue = Value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return _Text
            End Get
            Set(ByVal Value As String)
                _Text = Value
            End Set
        End Property

        Public Property BackColor() As String
            Get
                Return _BackColor
            End Get
            Set(ByVal Value As String)
                _BackColor = Value
            End Set
        End Property

        Public Property BorderColor() As String
            Get
                Return _BorderColor
            End Get
            Set(ByVal Value As String)
                _BorderColor = Value
            End Set
        End Property

        Public Property BorderWidth() As String
            Get
                Return _BorderWidth
            End Get
            Set(ByVal Value As String)
                _BorderWidth = Value
            End Set
        End Property

        Public Property BorderStyle() As String
            Get
                Return _BorderStyle
            End Get
            Set(ByVal Value As String)
                _BorderStyle = Value
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return _CssClass
            End Get
            Set(ByVal Value As String)
                _CssClass = Value
            End Set
        End Property

        Public Property Enabled() As Boolean
            Get
                Return _Enabled
            End Get
            Set(ByVal Value As Boolean)
                _Enabled = Value
            End Set
        End Property

        Public Property ForeColor() As String
            Get
                Return _ForeColor
            End Get
            Set(ByVal Value As String)
                _ForeColor = Value
            End Set
        End Property

        Public Property Height() As String
            Get
                Return _Height
            End Get
            Set(ByVal Value As String)
                _Height = Value
            End Set
        End Property

        Public Property TabIndex() As Integer
            Get
                Return _TabIndex
            End Get
            Set(ByVal Value As Integer)
                _TabIndex = Value
            End Set
        End Property

        Public Property ToolTip() As String
            Get
                Return _ToolTip
            End Get
            Set(ByVal Value As String)
                _ToolTip = Value
            End Set
        End Property

        Public Property Width() As String
            Get
                Return _Width
            End Get
            Set(ByVal Value As String)
                _Width = Value
            End Set
        End Property

        Public Property Visible() As Boolean
            Get
                Return _Visible
            End Get
            Set(ByVal Value As Boolean)
                _Visible = Value
            End Set
        End Property

        Public Property data() As String
            Get
                Return _Data
            End Get
            Set(ByVal Value As String)
                _Data = Value
            End Set
        End Property

        Public Property datavalue() As String
            Get
                Return _DataValue
            End Get
            Set(ByVal Value As String)
                _DataValue = Value
            End Set
        End Property


#End Region

    End Class


    Public Class NBright_dateEditControl

#Region "Private Members"

        Private _Text As String = ""
        Private _ID As String = ""
        Private _CausesValidation As Boolean = True
        Private _Columns As Integer = 0
        Private _MaxLength As Integer = 0
        Private _TextMode As TextBoxMode = TextBoxMode.SingleLine
        Private _Rows As Integer = 0
        Private _Wrap As Boolean = True
        Private _BackColor As String = ""
        Private _BorderColor As String = ""
        Private _BorderWidth As String = ""
        Private _BorderStyle As BorderStyle = Web.UI.WebControls.BorderStyle.NotSet
        Private _CssClass As String = ""
        Private _Enabled As Boolean = True
        Private _ForeColor As String = ""
        Private _Height As String = ""
        Private _TabIndex As Integer = 0
        Private _ToolTip As String = ""
        Private _Width As String = ""
        Private _Visible As Boolean = True

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property ID() As String
            Get
                Return _ID
            End Get
            Set(ByVal Value As String)
                _ID = Value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return _Text
            End Get
            Set(ByVal Value As String)
                _Text = Value
            End Set
        End Property

        Public Property CausesValidation() As Boolean
            Get
                Return _CausesValidation
            End Get
            Set(ByVal Value As Boolean)
                _CausesValidation = Value
            End Set
        End Property

        Public Property Columns() As Integer
            Get
                Return _Columns
            End Get
            Set(ByVal Value As Integer)
                _Columns = Value
            End Set
        End Property

        Public Property MaxLength() As Integer
            Get
                Return _MaxLength
            End Get
            Set(ByVal Value As Integer)
                _MaxLength = Value
            End Set
        End Property

        Public Property TextMode() As TextBoxMode
            Get
                Return _TextMode
            End Get
            Set(ByVal Value As TextBoxMode)
                _TextMode = Value
            End Set
        End Property

        Public Property Rows() As Integer
            Get
                Return _Rows
            End Get
            Set(ByVal Value As Integer)
                _Rows = Value
            End Set
        End Property

        Public Property Wrap() As Boolean
            Get
                Return _Wrap
            End Get
            Set(ByVal Value As Boolean)
                _Wrap = Value
            End Set
        End Property

        Public Property BackColor() As String
            Get
                Return _BackColor
            End Get
            Set(ByVal Value As String)
                _BackColor = Value
            End Set
        End Property

        Public Property BorderWidth() As String
            Get
                Return _BorderWidth
            End Get
            Set(ByVal Value As String)
                _BorderWidth = Value
            End Set
        End Property

        Public Property BorderColor() As String
            Get
                Return _BorderColor
            End Get
            Set(ByVal Value As String)
                _BorderColor = Value
            End Set
        End Property

        Public Property BorderStyle() As BorderStyle
            Get
                Return _BorderStyle
            End Get
            Set(ByVal Value As BorderStyle)
                _BorderStyle = Value
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return _CssClass
            End Get
            Set(ByVal Value As String)
                _CssClass = Value
            End Set
        End Property

        Public Property Enabled() As Boolean
            Get
                Return _Enabled
            End Get
            Set(ByVal Value As Boolean)
                _Enabled = Value
            End Set
        End Property

        Public Property ForeColor() As String
            Get
                Return _ForeColor
            End Get
            Set(ByVal Value As String)
                _ForeColor = Value
            End Set
        End Property

        Public Property Height() As String
            Get
                Return _Height
            End Get
            Set(ByVal Value As String)
                _Height = Value
            End Set
        End Property

        Public Property TabIndex() As Integer
            Get
                Return _TabIndex
            End Get
            Set(ByVal Value As Integer)
                _TabIndex = Value
            End Set
        End Property

        Public Property ToolTip() As String
            Get
                Return _ToolTip
            End Get
            Set(ByVal Value As String)
                _ToolTip = Value
            End Set
        End Property

        Public Property Width() As String
            Get
                Return _Width
            End Get
            Set(ByVal Value As String)
                _Width = Value
            End Set
        End Property

        Public Property Visible() As Boolean
            Get
                Return _Visible
            End Get
            Set(ByVal Value As Boolean)
                _Visible = Value
            End Set
        End Property
#End Region

    End Class



    Public Class FeederSetInfo

#Region "Private Members"
        Private _Key As String
        Private _password As String
        Private _reportref As String
        Private _functionkey As String
        Private _cachemins As String

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"

        Public Property Key() As String
            Get
                Return _Key
            End Get
            Set(ByVal Value As String)
                _Key = Value
            End Set
        End Property

        Public Property password() As String
            Get
                Return _password
            End Get
            Set(ByVal Value As String)
                _password = Value
            End Set
        End Property
        Public Property reportref() As String
            Get
                Return _reportref
            End Get
            Set(ByVal Value As String)
                _reportref = Value
            End Set
        End Property
        Public Property functionkey() As String
            Get
                Return _functionkey
            End Get
            Set(ByVal Value As String)
                _functionkey = Value
            End Set
        End Property

        Public Property cachemins() As String
            Get
                Return _cachemins
            End Get
            Set(ByVal Value As String)
                _cachemins = Value
            End Set
        End Property


#End Region

    End Class


End Namespace
